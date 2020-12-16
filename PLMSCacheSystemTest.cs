  using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using System.Timers;
//using System.Threading;
using System;

namespace PLMCLRTools
{
    public static class PLMSCacheSystemTest
    {
        public static readonly Dictionary<int, PdmBlockClrDto> DictBlockCache = new Dictionary<int, PdmBlockClrDto>();
        public static readonly Dictionary<int, PdmBlockSubItemClrDto> DictBlockSubItemCache = new Dictionary<int, PdmBlockSubItemClrDto>();

        public static readonly Dictionary<int, PdmGridClrDto> DictGridCache = new Dictionary<int, PdmGridClrDto>();
        public static Dictionary<int, PdmGridMetaColumnClrDto> DictAllPdmGridMetaColumnCache = new Dictionary<int, PdmGridMetaColumnClrDto>();

        public static readonly Dictionary<int, PdmEntityBlClrDto> DictPdmEntityBlCache = new Dictionary<int, PdmEntityBlClrDto>();

        public static readonly Dictionary<string, PdmEntityBlClrDto> DictCodeKeyPdmEntityBlCache = new Dictionary<string, PdmEntityBlClrDto>();
        public static readonly Dictionary<int, PdmUserDefineEntityColumnClrDto> DictEntityColumnCache = new Dictionary<int, PdmUserDefineEntityColumnClrDto>();


        public static readonly Dictionary<int, PdmReferenceViewClrDto> DictPdmReferenceViewCache = new Dictionary<int, PdmReferenceViewClrDto>();
        public static readonly Dictionary<int, PdmReferenceViewColumnClrDto> DictPdmReferenceViewColumnCache = new Dictionary<int, PdmReferenceViewColumnClrDto>();

        public static readonly Dictionary<int, PdmTabClrDto> DictTabCache = new Dictionary<int, PdmTabClrDto>();
      //  public static readonly Dictionary<int, PdmReferenceViewColumnClrDto> DictPdmReferenceViewColumnCache = new Dictionary<int, PdmReferenceViewColumnClrDto>();

        
  

        // load on demand
        private static readonly Dictionary<int, List<BlockSubitemClrUserDefineDto>> DictMainTabBlokSuibtemCache = new Dictionary<int, List<BlockSubitemClrUserDefineDto>>();
        private static readonly Dictionary<string, List<GridColumnClrUserDefineDto>> DictMainTabBlokGridColumnCache = new Dictionary<string, List<GridColumnClrUserDefineDto>>();

        public static readonly Dictionary<int, Dictionary<object, string>> DictEntityLookupCache = new Dictionary<int, Dictionary<object, string>>();

        public static readonly Dictionary<string, string> DictSystemTableLastScanTimeStamp = new Dictionary<string, string>();

        public static readonly PdmEntityBlClrDto LastScanUserDefinePdmEntityBlClrDto = new PdmEntityBlClrDto();

        public static readonly string UserDefineEntityTableName = "pdmUserDefineEntityRowValue";

        public static readonly List<PdmEntityBlClrDto> SystemDefineEntityList;
        public static readonly List<PdmEntityBlClrDto> UserDefineEntityList;

       // private static System.Threading.Timer _timer;

       // private static readonly Thread JobThread = new Thread(new ThreadStart(CallthreadTask));



        static PLMSCacheSystemTest()
        {
            PLMSCacheSystemTest.StartBaseCache();


           SystemDefineEntityList = DictPdmEntityBlCache.Where(o => o.Value.EntityType.HasValue && o.Value.EntityType.Value == (int)EmEntityType.SystemDefineTable).Select(o => o.Value).ToList();
           UserDefineEntityList = DictPdmEntityBlCache.Where(o => (o.Value.EntityType.HasValue) && o.Value.EntityType.Value == (int)EmEntityType.UserDefineTable).Select(o => o.Value).ToList();

           PdmCacheEntityLookupItem.SetupAllLookItemCache();

           SetupTimer();

        }


       private static void SetupTimer()
       {
          // CLROutput.OutputDebug("tiemrestart");
           Timer SessionTimer = new Timer();
           SessionTimer.Enabled = true;
           SessionTimer.AutoReset = true;

           SessionTimer.Elapsed += new ElapsedEventHandler(ScanSessionTimer_Elapsed);

           // open datasource, edist save,, then open teckpakc print need one 1 minus
           // cause bug 1.5 ???
           SessionTimer.Interval = 1 * 20 * 1000;
           //  SessionTimer.Interval = 5 * 1000;
           SessionTimer.Start();
       }

       private static void ScanSessionTimer_Elapsed(object sender, ElapsedEventArgs e)
       {
           CLROutput.OutputDebug("ScanSessionTimer_Elapsed??");

           foreach (var sysDefineEntity in SystemDefineEntityList)
           {

               UpdateSystemDefineCache(sysDefineEntity);


           }
           UpdateUserDefineCache();

       }




       private static void StartThread()
       {

         //  JobThread.Start();


          
       }

     

        private static void StartBaseCache()
        {
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();
                SetupEntityAndEntityColumn(conn);
                SetupBlockAndBlockSubitem(conn);
                SetupGridAndGridColumn(conn);
                SetupTabColumn(conn);
                SetupReferenceViewAndColumn(conn);

            }


           
        }

        private static void SetupThreadTimer()
        {

           // _timer = new Timer(Callback, null, 1 * 35 * 1000, Timeout.Infinite);
        }

        private static void Callback(Object state)
        {
           // Stopwatch watch = new Stopwatch();

          //  watch.Start();
            // Long running operation

          

          //  _timer.Change(1 * 35 * 1000, Timeout.Infinite);
        }

        private static void CallthreadTask()
        {
            while (true)
            {
                SynchronizeClrDataSource();

               // Thread.Sleep(1 * 35 * 1000);
            }

           
            
        }

     //   [Microsoft.SqlServer.Server.SqlProcedure]
        public static void SynchronizeClrDataSource()
        {
       
            foreach (var sysDefineEntity in SystemDefineEntityList)
            {
                   
                UpdateSystemDefineCache(sysDefineEntity);


            }
            UpdateUserDefineCache();
        }

        //private static void SetupTimer()
        //{
        //    CLROutput.OutputDebug("tiemrestart");
        //    Timer SessionTimer = new Timer();
        //    SessionTimer.Enabled = true;
        //    SessionTimer.AutoReset = true;
           
        //    SessionTimer.Elapsed += new ElapsedEventHandler(ScanSessionTimer_Elapsed);

        //    // open datasource, edist save,, then open teckpakc print need one 1 minus
        //    // cause bug 1.5 ???
        //   SessionTimer.Interval = 1 * 35 * 1000;
        //  //  SessionTimer.Interval = 5 * 1000;
        //    SessionTimer.Start();
        //}

        private static void ScanSessionTimer_Elapsed(object sender, EventArgs e)
        {
            foreach (var sysDefineEntity in SystemDefineEntityList)
            {
             
                 UpdateSystemDefineCache(sysDefineEntity);
 

            }
            UpdateUserDefineCache();
            
        }

        private static void UpdateUserDefineCache()
        {
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                string lastChange = PdmCacheEntityLookupItem.GetLastChangeCheckSum(conn, UserDefineEntityTableName);


                if ( lastChange != string.Empty &&  lastChange != LastScanUserDefinePdmEntityBlClrDto.LastScanCheckSum)
                {

                     // CLROutput.InsertException(conn, " lastChange before userdefine " + LastScanUserDefinePdmEntityBlClrDto.LastScanCheckSum);

                      Dictionary<int, List<LookupItemDto>> dictUserDefineLookupItem = PLMSEntityClrBL.GetUserDefineEntityDisplayInfoList(UserDefineEntityList, conn);


                    foreach (int entityId in dictUserDefineLookupItem.Keys)
                    {
                        Dictionary<object, string> result = PdmCacheEntityLookupItem.ConvertLookItemToDictionaryItem(dictUserDefineLookupItem[entityId]);

                        // if(
                        DictEntityLookupCache.Remove(entityId);
                        DictEntityLookupCache.Add(entityId, result);

                    }



                    LastScanUserDefinePdmEntityBlClrDto.LastScanCheckSum = lastChange;

                     // CLROutput.InsertException(conn, " lastChange userdefine  after" + LastScanUserDefinePdmEntityBlClrDto.LastScanCheckSum);

                }
            }
        }

        private static void UpdateSystemDefineCache( PdmEntityBlClrDto entity)
        {
            int entityId = entity.EntityId;

            CLROutput.OutputDebug("UpdateSystemDefineCache start...  ConnectInfo:" + entity.ConnectInfo);

           // CLROutput.OutputDebug("UpdateSystemDefineCache start...");

            if (!string.IsNullOrEmpty(entity.ConnectInfo))
            {
               

                //try
                //{
                    using (SqlConnection conn = new SqlConnection(entity.ConnectInfo))
                    {
                        conn.Open();

                        string lastChange = PdmCacheEntityLookupItem.GetLastChangeCheckSum(conn, entity.SysTableName);

                      //  CLROutput.InsertException(conn, "entity.LastScanCheckSum: " + entity.LastScanCheckSum + "lastChange :" + lastChange);

                        CLROutput.OutputDebug("entity.LastScanCheckSum: " + entity.LastScanCheckSum + "lastChange :" + lastChange);

                        if (lastChange != entity.LastScanCheckSum)
                        {

                            if (DictEntityLookupCache.ContainsKey(entityId))
                            {

                               CLROutput.OutputDebug( " lastChange sysdefine before DictEntityLookupCache " + "__" + entity.EntityCode + "__" + entity.LastScanCheckSum,conn);
                                List<LookupItemDto> newLookupitem = PdmCacheEntityLookupItem.GetOneSystemDefinTableLookupItems(conn, PLMSEntityClrBL.GetSysDefineQueryIDAndDisplay(entity));
                                Dictionary<object, string> result = PdmCacheEntityLookupItem.ConvertLookItemToDictionaryItem(newLookupitem);

                                DictEntityLookupCache.Remove(entityId);
                                DictEntityLookupCache.Add(entityId, result);

                                entity.LastScanCheckSum = lastChange;
                               // CLROutput.InsertException(conn, " lastChange sysdefine  after" + "__" + entity.EntityCode + "__" + entity.LastScanCheckSum);

                            }

                        }

                    }

                //}
                //catch (Exception ex)
                //{

                //    //using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                //    //{
                //    //    conn.Open();
                //    //    CLROutput.InsertException(conn, " exception " + entity.EntityId  + "__" + entity.EntityCode   + ex.ToString());
                           
                //    //}
                
                //}
               

            }
        }

        internal static List<PdmBlockSubItemClrDto> GetMutiplePdmBlockSubItemEntityFromCache(IEnumerable<int> subitemIds)
        {
            return DictBlockSubItemCache.Where(pair => subitemIds.Contains(pair.Key)).Select(pair => pair.Value).ToList();
        }

        internal static List<PdmGridMetaColumnClrDto> GetMutiplePdmGridMetaColumnEntityFromCache(IEnumerable<int> columnIds)
        {
            return DictAllPdmGridMetaColumnCache.Where(pair => columnIds.Contains(pair.Key)).Select(pair => pair.Value).ToList();
        }

        internal static List<GridColumnClrUserDefineDto> GetTabGridSelectColumnAndAliasDtoFromCache(int tabId, PdmBlockClrDto dmBlockClrDto)
        {

            string key = tabId.ToString() + "_" + dmBlockClrDto.BlockId .ToString();

            if (DictMainTabBlokGridColumnCache.ContainsKey(key))
            {
                CLROutput.OutputDebug ( "string key = " + key);
                return DictMainTabBlokGridColumnCache[key];
            }
            else
            {
                List<GridColumnClrUserDefineDto> printColumnListDto = GetTabGridBlockColumnAliasName(tabId, dmBlockClrDto);
                DictMainTabBlokGridColumnCache.Add (key,printColumnListDto);
                return printColumnListDto;
            
            }
           
           
        }

        private static List<GridColumnClrUserDefineDto> GetTabGridBlockColumnAliasName(int tabId, PdmBlockClrDto dmBlockClrDto)
        {

            int gridId = dmBlockClrDto.BlockPdmGridDto.GridId;

            List<GridColumnClrUserDefineDto> printColumnListDto = PLMSDWScriptGenerator.GetGridColumnIDAndName(int.Parse(gridId.ToString()));
            Dictionary<int, GridColumnClrUserDefineDto> dictAllGridColumnClrDto = printColumnListDto.ToDictionary(o => o.GridColumnID, o => o);

            List<GridColumnClrUserDefineDto> selectGridColumnClrDto = new List<GridColumnClrUserDefineDto>();

            //  need to check pdmgridmatacolumn

            string queryPdmTabGridMetaColumn = @" SELECT  distinct   dbo.pdmTabGridMetaColumn.GridColumnID,
                        dbo.pdmTabGridMetaColumn.Visible, dbo.pdmTabGridMetaColumn.AliasName,

                         dbo.pdmTabGridMetaColumn.BlockID AS GridBlockID
                        FROM         dbo.pdmBlockSubItem INNER JOIN
                                              dbo.PdmTabBlock ON dbo.pdmBlockSubItem.BlockID = dbo.PdmTabBlock.BlockID LEFT OUTER JOIN
                                              dbo.pdmTabGridMetaColumn ON dbo.PdmTabBlock.BlockID = dbo.pdmTabGridMetaColumn.BlockID AND
                                              dbo.PdmTabBlock.TabID = dbo.pdmTabGridMetaColumn.TabID
                        WHERE     (dbo.pdmBlockSubItem.ControlType = @ControlType)
                        AND (dbo.PdmTabBlock.TabID = @TabID )
                        AND (dbo.pdmTabGridMetaColumn.BlockID = @GridBlockID)";

            //

            List<SqlParameter> listParamtetrs = new List<SqlParameter>();

            SqlParameter ControlTypePara = new SqlParameter("@ControlType", (int)EmControlType.Grid);
            SqlParameter TabIDPara = new SqlParameter("@TabID", tabId);
            SqlParameter GridBlockIDpara = new SqlParameter("@GridBlockID", dmBlockClrDto.BlockId);

            listParamtetrs.Add(ControlTypePara);
            listParamtetrs.Add(TabIDPara);
            listParamtetrs.Add(GridBlockIDpara);


            DataTable TabGridColumnResult;

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                TabGridColumnResult = DataAcessHelper.GetDataTableQueryResult(conn, queryPdmTabGridMetaColumn, listParamtetrs);


            }



            // CLROutput.SendDataTable(TabGridColumnResult);
            if (TabGridColumnResult.Rows.Count > 0)
            {
                foreach (DataRow aRow in TabGridColumnResult.Rows)
                {
                    int GridColumnID = (int)aRow["GridColumnID"];
                    bool? isVisalbe = aRow["Visible"] as bool?;
                    string AliasName = aRow["AliasName"] as string;

                    if (isVisalbe.HasValue && isVisalbe.Value)
                    {
                        GridColumnClrUserDefineDto aGridColumnClrDto = dictAllGridColumnClrDto[GridColumnID];
                        if (!string.IsNullOrEmpty(AliasName))
                        {
                            string aName = AliasName.Trim();
                            if (aName.Length > 0)
                            {
                                aGridColumnClrDto.ColumnName = AliasName;
                            }
                        }
                        selectGridColumnClrDto.Add(aGridColumnClrDto);
                    }
                }

                printColumnListDto = selectGridColumnClrDto;
            }
            return printColumnListDto;
        }


        public static List<BlockSubitemClrUserDefineDto>  GetTabBlockSubitem(int tabId)
        {
            if (DictMainTabBlokSuibtemCache.ContainsKey(tabId))
            {
                return DictMainTabBlokSuibtemCache[tabId];
            }

            else
            {
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                {
                    conn.Open();

                    List<BlockSubitemClrUserDefineDto> result = GetOneMainTabBlockSubitemFullPathNameAndControlType(tabId, conn);
                        DictMainTabBlokSuibtemCache.Add(tabId ,result);
                    return result ;

                }
          
            
            }
        
        }


        private static List<BlockSubitemClrUserDefineDto> GetOneMainTabBlockSubitemFullPathNameAndControlType(int anytabID, SqlConnection conn)
        {
            string qeuryTabBlockSubitem = string.Format(@"SELECT DISTINCT
                      TOP (100) PERCENT dbo.pdmBlockSubItem.SubItemID, dbo.pdmBlockSubItem.ControlType, dbo.pdmBlockSubItem.SubItemName, dbo.pdmBlockSubItem.EntityID,
                      dbo.pdmEntity.EntityCode, dbo.pdmEntity.SysTableName
FROM         dbo.pdmBlockSubItem INNER JOIN
                      dbo.pdmBlock ON dbo.pdmBlockSubItem.BlockID = dbo.pdmBlock.BlockID INNER JOIN
                      dbo.PdmTabBlock ON dbo.pdmBlock.BlockID = dbo.PdmTabBlock.BlockID INNER JOIN
                      dbo.pdmTab ON dbo.PdmTabBlock.TabID = dbo.pdmTab.TabID LEFT OUTER JOIN
                      dbo.pdmEntity ON dbo.pdmBlockSubItem.EntityID = dbo.pdmEntity.EntityID
WHERE     (dbo.pdmTab.TabID = {0}) AND (dbo.pdmBlockSubItem.ControlType IN (1, 2, 3, 4, 5, 7, 8, 9, 13, 15, 19, 20, 21, 23))
ORDER BY dbo.pdmBlockSubItem.SubItemID", anytabID);

           // qeuryTabBlockSubitem = C

            //ExecuteReadUnCommmited(conn);
            return RetriveBlockSubitemFullPathNameAndControlType(conn, qeuryTabBlockSubitem);
        }

        private static List<BlockSubitemClrUserDefineDto> RetriveBlockSubitemFullPathNameAndControlType(SqlConnection conn, string qeuryTabBlockSubitem)
        {
            List<BlockSubitemClrUserDefineDto> aDictSubitemID_Name = new List<BlockSubitemClrUserDefineDto>();
            SqlCommand cmd = new SqlCommand(qeuryTabBlockSubitem, conn);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            System.Data.DataTable resultTabel = new DataTable();
            adapter.Fill(resultTabel);

            foreach (DataRow aRow in resultTabel.Rows)
            {
                int aSubItemId = (int)aRow["SubItemID"];//.ToString();
                string subItemName = aRow["SubItemName"].ToString();
                string subItemFullPathName = DataTableUtility.FilterSQLDBInvalidChar(aRow["SubItemName"].ToString()) + "_" + aSubItemId;

                //dbo.pdmBlockSubItem.EntityID dbo.pdmEntity.EntityCode, dbo.pdmEntity.SysTableName

                int? entityID = aRow["EntityID"] as int?;
                string entityCode = DataTableUtility.FilterSQLDBInvalidChar(aRow["EntityCode"].ToString());
                string systemTable = aRow["SysTableName"].ToString();

                

                if (entityID.HasValue)
                {
                    PdmEntityBlClrDto aEntityBlClrDto = DictPdmEntityBlCache[entityID.Value];

                    //EntityType = " + (int)EmEntityType.UserDefineTable;
                    if (aEntityBlClrDto.EntityType == (int)EmEntityType.UserDefineTable)
                    {
                        subItemFullPathName += "_FK_" + PLMConstantString.PLM_DW_UserDefineTablePrefix + entityCode + "_" + entityID;
                    }
                    else
                    {
                        subItemFullPathName += "_FK_" + systemTable;
                    }
                }

                // need to

                int controlType = int.Parse(aRow["ControlType"].ToString());

                //    SqlInt32 rootCopySubitemID = Converter.ToDDLSqlInt32(aRow["RootCopySubItemID"]);

                BlockSubitemClrUserDefineDto aDto = new BlockSubitemClrUserDefineDto();
                aDto.SubItemID = aSubItemId;
                aDto.SubItemName = subItemName;
                aDto.SubItemFullPathName = subItemFullPathName;
                aDto.EntityID = entityID;
                aDto.ControlType = controlType;

                aDictSubitemID_Name.Add(aDto);
            }

            return aDictSubitemID_Name;
        }




      //  MainTabBlockSubitem
              

       

        private static void SetupBlockAndBlockSubitem(SqlConnection conn)
        {
            List<PdmBlockClrDto> blockList = PdmBlockDal.GetAllList(conn);
            List<PdmBlockSubItemClrDto> blockSubitemList = PdmBlockSubItemDal.GetAllList(conn);
            foreach (PdmBlockClrDto aPdmBlockClrDto in blockList)
            {

                DictBlockCache.Add(aPdmBlockClrDto.BlockId, aPdmBlockClrDto);
                aPdmBlockClrDto.PdmBlockSubItemList = blockSubitemList.Where(o => o.BlockId == aPdmBlockClrDto.BlockId).ToList();

                //aPdmBlockClrDto

            }

            foreach (PdmBlockSubItemClrDto aPdmBlockSubItemClrDto in blockSubitemList)
            {
               DictBlockSubItemCache.Add(aPdmBlockSubItemClrDto.SubItemId, aPdmBlockSubItemClrDto);

            }
           
        }

        private static void SetupGridAndGridColumn(SqlConnection conn)
        {
            List<PdmGridClrDto> gridList = PdmGridDal.GetAllList(conn);
            List<PdmGridMetaColumnClrDto> columnList = PdmGridMetaColumnDal.GetAllList(conn);
            foreach (PdmGridClrDto aPdmGridClrDto in gridList)
            {
                DictGridCache.Add(aPdmGridClrDto.GridId, aPdmGridClrDto);
                aPdmGridClrDto.PdmGridMetaColumnList = columnList.Where(o => o.GridId == aPdmGridClrDto.GridId).ToList();

            }

            foreach (PdmGridMetaColumnClrDto aPdmGridMetaColumnClrDto in columnList)
            {
              
                DictAllPdmGridMetaColumnCache.Add(aPdmGridMetaColumnClrDto.GridColumnId, aPdmGridMetaColumnClrDto);
                if (aPdmGridMetaColumnClrDto.EntityId.HasValue)
                {
                    PdmEntityBlClrDto aEntityBlClrDto = DictPdmEntityBlCache[aPdmGridMetaColumnClrDto.EntityId.Value];
                    aPdmGridMetaColumnClrDto.EntityCode = aEntityBlClrDto.EntityCode; ;
                    aPdmGridMetaColumnClrDto.EntityBlClrDto = aEntityBlClrDto;
                }


            }

        }

        private static void SetupTabColumn(SqlConnection conn)
        {
            List<PdmTabClrDto> gridList = PdmTabDal.GetAllList(conn);

            foreach (PdmTabClrDto aPdmTabClrDto in gridList)
            {
                DictTabCache.Add(aPdmTabClrDto.TabId, aPdmTabClrDto);
                

            }

         

        }

        //  SetupTabColumn(conn);

        private static void SetupEntityAndEntityColumn(SqlConnection conn)
        {
            List<PdmEntityBlClrDto> entityList = PdmEntityBlDal.GetAllList(conn);
            List<PdmUserDefineEntityColumnClrDto> columnList = PdmUserDefineEntityColumnDal.GetAllList(conn);
            foreach (PdmEntityBlClrDto aPdmEntityBlClrDto in entityList)
            {
                aPdmEntityBlClrDto.PdmUserDefineEntityColumnList = columnList.Where(o => o.EntityId == aPdmEntityBlClrDto.EntityId).ToList();
                DictPdmEntityBlCache.Add(aPdmEntityBlClrDto.EntityId, aPdmEntityBlClrDto);
                DictCodeKeyPdmEntityBlCache.Add(aPdmEntityBlClrDto.EntityCode, aPdmEntityBlClrDto);
             
            }

            foreach (PdmUserDefineEntityColumnClrDto aPdmUserDefineEntityColumnClrDto in columnList)
            {

                DictEntityColumnCache.Add(aPdmUserDefineEntityColumnClrDto.UserDefineEntityColumnId, aPdmUserDefineEntityColumnClrDto);


            }

          //  CLROutput.Output("entityList=" + entityList.Count);

        }

        private static void SetupReferenceViewAndColumn(SqlConnection conn)
        {
            List<PdmReferenceViewClrDto> gridList = PdmReferenceViewDal.GetAllList(conn);
            List<PdmReferenceViewColumnClrDto> columnList = PdmReferenceViewColumnDal.GetAllList(conn);
            foreach (PdmReferenceViewClrDto aPdmReferenceViewClrDto in gridList)
            {
                aPdmReferenceViewClrDto.PdmReferenceViewColumnList = columnList.Where(o => o.ReferenceViewId == aPdmReferenceViewClrDto.ReferenceViewId).ToList();
                DictPdmReferenceViewCache.Add(aPdmReferenceViewClrDto.ReferenceViewId, aPdmReferenceViewClrDto);

            }

            foreach (PdmReferenceViewColumnClrDto columnDto in columnList)
            {

                DictPdmReferenceViewColumnCache.Add(columnDto.ReferenceViewColumnId, columnDto);


            }

          //  CLROutput.Output("view columnList=" + columnList.Count);

        }



      
    }
}