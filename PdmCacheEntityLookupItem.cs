using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System;

namespace PLMCLRTools
{
    public static class PdmCacheEntityLookupItem
    {


        public static void SetupAllLookItemCache()
        {

            var allCacheEntity = PdmCacheManager.DictPdmEntityBlEntity;
            List<int> entityIDs = allCacheEntity.Keys.ToList();
            Dictionary<int, List<LookupItemDto>> dictEntityLookupItemDto = GetDictEntityLookItemDtoFromDataBase(entityIDs);
            AddEntityToSystemCache(dictEntityLookupItemDto);

            CLROutput.OutputDebug("done: add cache with last scan timestamp");


        }
      

        public static Dictionary<int, Dictionary<object, string>> GetDictEntityDictDisplayInforFromCache(List<int> entityIDs)
        {

            Dictionary<int, Dictionary<object, string>> dictEntityDisplayInfoList = new Dictionary<int, Dictionary<object, string>>();
  
            foreach (int entityid in entityIDs.Distinct())
            {
                if (PdmCacheManager.DictEntityLookupCache.ContainsKey(entityid))
                {
                    dictEntityDisplayInfoList.Add(entityid, PdmCacheManager.DictEntityLookupCache[entityid]);
                }
                else
                {

                    dictEntityDisplayInfoList.Add(entityid, new Dictionary<object, string>());
                }
                

            }


            return dictEntityDisplayInfoList;
        }


        public static Dictionary<int, List<LookupItemDto>> GetDictEntityLookItemDtoFromDataBase(List<int> entityIDs)
        {

            var discitnEntitIds = entityIDs.Distinct().ToList();
            Dictionary<int, List<LookupItemDto>> dictEntityDisplayInfoList = new Dictionary<int, List<LookupItemDto>>();

            List<PdmEntityBlClrDto> listPdmEntityDto = PdmCacheManager.DictPdmEntityBlEntity.Where(pair => entityIDs.Contains(pair.Key)).Select(pair => pair.Value).ToList();

            Dictionary<string, List<PdmEntityBlClrDto>> dictConnInfoSysDefineEntityList = new Dictionary<string, List<PdmEntityBlClrDto>>();
            List<PdmEntityBlClrDto> userDefineEntityList = new List<PdmEntityBlClrDto>();
            List<PdmEntityBlClrDto> enumEntityList = new List<PdmEntityBlClrDto>();

            // need to classify entity type !!
            foreach (PdmEntityBlClrDto pdmEntityBlClrDto in listPdmEntityDto)
            {
                //tblCompanySetup=
                if (pdmEntityBlClrDto.EntityType == (int)EmEntityType.SystemDefineTable)
                {

                    // need to filter tblsketch ,tblskech could come fro mother applcation place
                    //if (pdmEntityBlClrDto.EntityCode == EmEntityCode.Sketch.ToString ())
                    //    continue;

                    string connectInfo = PLMSEntityClrBL.GetConnectionInfoWithCode(pdmEntityBlClrDto.DataSourceFrom);
                //  CLROutput.OutputDebug(pdmEntityBlClrDto.EntityCode + "c?" + connectInfo);
              

                    pdmEntityBlClrDto.ConnectInfo = connectInfo;

                  //  CLROutput.OutputDebug("pdmEntityBlClrDto.ConnectInfo=" + pdmEntityBlClrDto.ConnectInfo);


                    if (dictConnInfoSysDefineEntityList.ContainsKey(connectInfo))
                    {
                        List<PdmEntityBlClrDto> listEntityDto = dictConnInfoSysDefineEntityList[connectInfo];
                        listEntityDto.Add(pdmEntityBlClrDto);

                    }
                    else // not include conenction info key: first time 
                    {
                        List<PdmEntityBlClrDto> listEntityDto = new List<PdmEntityBlClrDto>();
                        dictConnInfoSysDefineEntityList.Add(connectInfo, listEntityDto);
                        listEntityDto.Add(pdmEntityBlClrDto);


                    }

                }
                else if (pdmEntityBlClrDto.EntityType == (int)EmEntityType.UserDefineTable)
                {

                    userDefineEntityList.Add(pdmEntityBlClrDto);

                }
                else if (pdmEntityBlClrDto.EntityType == (int)EmEntityType.PDMEnum)
                {

                    enumEntityList.Add(pdmEntityBlClrDto);

                }

            }


            // 
            foreach (string connectInfo in dictConnInfoSysDefineEntityList.Keys)
            {
                using (SqlConnection conn = new SqlConnection(connectInfo))
                {
                    conn.Open();
                    List<PdmEntityBlClrDto> listEntityDto = dictConnInfoSysDefineEntityList[connectInfo];

                    GetSysDefineDictLookItem(conn, dictEntityDisplayInfoList, listEntityDto);

                }


            }

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();
                PLMSEntityClrBL.GetEnumDictLookItemNew(conn, dictEntityDisplayInfoList, enumEntityList);
                PLMSEntityClrBL.GetUserDefineDictLookItem(conn, dictEntityDisplayInfoList, userDefineEntityList);

            }

            return dictEntityDisplayInfoList;
        }



        private static void GetSysDefineDictLookItem(SqlConnection conn, Dictionary<int, List<LookupItemDto>> dictEntityDisplayInfoList, List<PdmEntityBlClrDto> listPdmEntityDto)
        {
            List<PdmEntityBlClrDto> sysDefineEntityList = listPdmEntityDto.Where(o => (o.EntityType.HasValue)
                && (o.EntityType.Value == (int)EmEntityType.SystemDefineTable)).Distinct().ToList();


            Dictionary<int, List<LookupItemDto>> dictSystemTableLookupItem = LoadSysDefineLookupItemFromDabaBase(sysDefineEntityList, conn); ;
            foreach (var dictitem in dictSystemTableLookupItem)
            {
                dictEntityDisplayInfoList.Add(dictitem.Key, dictitem.Value);

            }
        }
        private static Dictionary<int, List<LookupItemDto>> LoadSysDefineLookupItemFromDabaBase(List<PdmEntityBlClrDto> sysDefineEntityList, SqlConnection conn)
        {
            Dictionary<int, List<LookupItemDto>> toReturn = new Dictionary<int, List<LookupItemDto>>();

            Dictionary<string, string> dictTableNameAndQuery = new Dictionary<string, string>();

            foreach (var aPdmEntityDto in sysDefineEntityList)
            {
                string systemQueryTable = PLMSEntityClrBL.GetSysDefineQueryIDAndDisplay(aPdmEntityDto);

            //    CLROutput.OutputDebug("aPdmEntityDto.SysTableName systemQueryTable" + aPdmEntityDto.EntityId + "=" + systemQueryTable);


                if (string.IsNullOrEmpty(systemQueryTable))
                    continue;

            //    CLROutput.OutputDebug("aPdmEntityDto.SysTableName" + aPdmEntityDto.EntityId + "=" + aPdmEntityDto.EntityCode + aPdmEntityDto.SysTableName);

                if (!String.IsNullOrEmpty(aPdmEntityDto.SysTableName))
                {

                    if (!dictTableNameAndQuery.ContainsKey(aPdmEntityDto.SysTableName))
                    {
                        dictTableNameAndQuery.Add(aPdmEntityDto.SysTableName, systemQueryTable);

                    }

                }


            }


            foreach (string tablname in dictTableNameAndQuery.Keys)
            {
               

              
                string queryTAble = dictTableNameAndQuery[tablname];

               List<LookupItemDto> listDto = GetOneSystemDefinTableLookupItems(conn,  queryTAble);
               foreach (PdmEntityBlClrDto SysEntityDto in sysDefineEntityList)
                {
                    string sysTableName = SysEntityDto.SysTableName;

                    //  CLROutput.SendDataTable(datatable);

                    if (sysTableName == tablname)
                    {

                        toReturn.Add(SysEntityDto.EntityId, listDto);

                      //  CLROutput.OutputDebug("before EntityCode " + SysEntityDto.EntityCode + " tableName="+ sysTableName + "=" + " lastScaltime=" );
                        //Important
                        //Important
                        string lastScaltime = GetLastChangeCheckSum(conn, tablname);

                        //Fuck conn.ConnectionString will retur no password !!!!
                      //  SysEntityDto.ConnectInfo = conn.ConnectionString;

                        CLROutput.OutputDebug("SysEntityDto.ConnectInfo = conn.ConnectionString" + SysEntityDto.ConnectInfo);


                        SysEntityDto.LastScanCheckSum = lastScaltime;

                        CLROutput.OutputDebug( sysTableName + " lastScaltime :" + lastScaltime);




                    }

                }


            }


            return toReturn;
        }

        internal  static List<LookupItemDto>  GetOneSystemDefinTableLookupItems(SqlConnection conn,  string queryTAble)
        {

            List<LookupItemDto> listDto = new List<LookupItemDto> ();

            try
            {
                DataTable datatable = DataAcessHelper.GetDataTableQueryResult(conn, queryTAble);

                foreach (DataRow row in datatable.Rows)
                {
                    LookupItemDto itemDto = new LookupItemDto();
                    // itemDto.Id = (int)row["Id"];
                    itemDto.Id = row["Id"];
                    itemDto.Display = row["Display"] as string;
                    listDto.Add(itemDto);
                }
            }
            catch (Exception ex)
            {
            }

           

            return listDto;


        }

        internal static string GetLastChangeCheckSum(SqlConnection conn, string tablname)
        {
         //  string querylastScanTimstmap = @" select master.sys.fn_varbintohexstr(cast( MAX(SystemTimeStamp) as varbinary(8)))as  varbintohexstr   from  " + tablname;

           string querylastScanTimstmap = @" SELECT CHECKSUM_AGG(BINARY_CHECKSUM(*)) FROM  " + tablname;


          //  SELECT CHECKSUM_AGG(BINARY_CHECKSUM(*)) FROM tblsketch WITH (NOLOCK);

           try
           {
               object lastScaltime = DataAcessHelper.RetriveSigleValue(conn, querylastScanTimstmap, null);
               if (lastScaltime == null)
               {
                   return string.Empty;
               }
               else
               {
                   return lastScaltime.ToString();
               }
           }
           catch (Exception ex)
           {
               CLROutput.OutputDebug(tablname + "missing Timstmap" + ex.ToString());
               return string.Empty;
           }

          

            
        }

        private static void AddEntityToSystemCache(Dictionary<int, List<LookupItemDto>> dictEntityLookupItemDto)
        {
            foreach (var pair in dictEntityLookupItemDto)
            {
                Dictionary<int, string> dictLookupIfno = new Dictionary<int, string>();
                int entityId = pair.Key;
                List<LookupItemDto> listDto = pair.Value;

                var dictDisplyInfor = ConvertLookItemToDictionaryItem(listDto);

             

                // addADD to cache !
                PdmCacheManager.DictEntityLookupCache.Add(entityId, dictDisplyInfor);
            }
        }

        internal static Dictionary<object, string> ConvertLookItemToDictionaryItem(List<LookupItemDto> listDto)
        {
            var dictDisplyInfor = listDto.GroupBy(x => x.Id)
                      .Select(g => g.First())
                      .ToDictionary(o => o.Id, o => o.Display as string);
            return dictDisplyInfor;
        }

      







    }
}