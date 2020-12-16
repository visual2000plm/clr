using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;
using System.Collections;
using System.Diagnostics;

namespace PLMCLRTools
{
    public class PLMSTechPackStoredProcedures
    {
   
        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void GetTabWithTimeZone(int tabId, [SqlFacet(MaxSize = -1)]string referenceIds,  string clientTimeZonekey )
        {
            GetTabValue(tabId, referenceIds, true);
        }


        // need define  "MutipleProductReferenceIDs" paramter in report design

        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void SplitStringToMutipleColumn([SqlFacet(MaxSize = -1)]string referenceIds,int numberOfColumn)
        {
            DataTable datatable = new DataTable();
            for (int i = 1; i <= numberOfColumn; i++)
            {
                datatable.Columns.Add("C" + i.ToString()); 
 
            }

            string [] referenceidList =  referenceIds.Split(new char[] { ',' });
           
            DataRow row= datatable.NewRow();
            int columncount = 0;

            for (int i = 0; i < referenceidList.Length; i++)
            {
     
                int remain = i % numberOfColumn;
                if (remain == 0)
                {
                    row = datatable.NewRow();
                    datatable.Rows.Add(row);
                    columncount = 0;
                    row[columncount] = referenceidList[i];
           
                   
                }
                else
                {
                    columncount++;
                    row[columncount] = referenceidList[i];
                    
                }

               
            }
   

            CLROutput.SendDataTable(datatable);
         
        }


        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void GetTabWithValueIDWithTimeZone(int tabId, [SqlFacet(MaxSize = -1)]string referenceIds, string clientTimeZonekey )
        {
            GetTabValue( tabId,  referenceIds,  false);
        }
          
        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void GetGridWithTimeZone(int tabId, int currentGridBlockId, string referenceIds, string clientTimeZonekey )
        {
            //old way
          //  GetGridValue(tabId, currentGridBlockId, referenceIds, true,false);

            // new Way to load GridValue
            PLMSGetGridValueHeler.GetGridValue(tabId, currentGridBlockId, referenceIds, true, false,true); ;
        }


        //[SqlFunction(FillRowMethodName = "FillRow")]
        //public static IEnumerable InitMethod(String logname)
        //{
        //    return new EventLog(logname).Entries;
        //}

        //public static void FillRow(Object obj, out SqlDateTime timeWritten, out SqlChars message, out SqlChars category, out long instanceId)
        //{
        //    EventLogEntry eventLogEntry = (EventLogEntry)obj;
        //    timeWritten = new SqlDateTime(eventLogEntry.TimeWritten);
        //    message = new SqlChars(eventLogEntry.Message);
        //    category = new SqlChars(eventLogEntry.Category);
        //    instanceId = eventLogEntry.InstanceId;
        //}  

        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void GetGridExternalMappingValue(int tabId, int currentGridBlockId, string referenceIds)
        {
            //old way
          PLMSGetGridValueHeler.GetGridValue(tabId, currentGridBlockId, referenceIds, false, false,false); ;

          
        }
        // public static void GetTabValue(int tabId, string referenceIds, bool isShowLookupitem) //tabId,  referenceIds,  false
        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void GetTabExternalMappingValue(int tabId, string referenceIds)
        {
            //old way
            GetTabValue(tabId,  referenceIds,  false,false); ;


        }


        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void GetUserPrintGrid(int userId, int tabId, int currentGridBlockId, string referenceIds)
        {
            //old way
            //  GetGridValue(tabId, currentGridBlockId, referenceIds, true,false);

            // new Way to load GridValue
            PLMSGetGridValueHeler.GetUserPrintGrid(userId, tabId, currentGridBlockId, referenceIds, true, false); ;
        }

        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void GetTabAndGridExternalColumnValue(int gridTabId, int currentGridBlockId, string referenceIds)
        {
            if (string.IsNullOrEmpty(referenceIds))
                return;

            PdmBlockClrDto dmBlockClrDto = PdmCacheManager.DictBlockCache[currentGridBlockId];
            if (dmBlockClrDto.BlockPdmGridDto == null)
                return;

            bool IsGetAliasname = false;

            DataTable tabFieldResultDataTable = GetTabDataTable(gridTabId, referenceIds, false, false);
            // Get Grading Size will call PrepareGetGridDataTable and set 
            DataTable gridcolumnResultDataTable = PLMSGetGridValueHeler.LoadVariousGridColumnValue(gridTabId, currentGridBlockId, referenceIds, false, dmBlockClrDto, false, false, IsGetAliasname);
           

           List<string> gridExternalMappingNameList = GetExternalNappingName(gridTabId, currentGridBlockId);


           string firstReferencecolumn = gridcolumnResultDataTable.Columns[0].ColumnName;
           gridExternalMappingNameList.Add(firstReferencecolumn);

           List<string> allCoumns = new List<string>();
           foreach (DataColumn column in gridcolumnResultDataTable.Columns)
           {
               allCoumns.Add(column.ColumnName);
           }

           var needTOremovecolumn =allCoumns.Except(gridExternalMappingNameList);

           foreach (string needToRemove in needTOremovecolumn)
           {
               gridcolumnResultDataTable.Columns.Remove(needToRemove);
           }

           DataTable mergeTable = null;
           if (gridcolumnResultDataTable.Rows.Count > 0)
           {
               mergeTable =  DataTableUtility.Join(tabFieldResultDataTable, gridcolumnResultDataTable, tabFieldResultDataTable.Columns[0], gridcolumnResultDataTable.Columns[0]);
           }
           else

           {
               //mergeTable

               foreach (DataColumn gridColumn in gridcolumnResultDataTable.Columns)
               {
                   if (! tabFieldResultDataTable.Columns.Contains(gridColumn.ColumnName))
                   {
                       tabFieldResultDataTable.Columns.Add(gridColumn.ColumnName);
                   }
               }

               mergeTable = tabFieldResultDataTable;
           }

           

            CLROutput.SendDataTable(mergeTable);
        }

        private static List<string> GetExternalNappingName(int gridTabId, int currentGridBlockId)
        {

            List<string> toReturn = new List<string>();
            string query = string.Format(@"SELECT  distinct   dbo.pdmTabGridMetaColumn.GridColumnID,
                        dbo.pdmTabGridMetaColumn.Visible, dbo.pdmTabGridMetaColumn.ExternalMappingName ,

                         dbo.pdmTabGridMetaColumn.BlockID AS GridBlockID
                        FROM         dbo.pdmBlockSubItem INNER JOIN
                                              dbo.PdmTabBlock ON dbo.pdmBlockSubItem.BlockID = dbo.PdmTabBlock.BlockID LEFT OUTER JOIN
                                              dbo.pdmTabGridMetaColumn ON dbo.PdmTabBlock.BlockID = dbo.pdmTabGridMetaColumn.BlockID AND
                                              dbo.PdmTabBlock.TabID = dbo.pdmTabGridMetaColumn.TabID
                        WHERE     (dbo.pdmBlockSubItem.ControlType = 6)
                        AND (dbo.PdmTabBlock.TabID = {0} )
                  AND (dbo.pdmTabGridMetaColumn.BlockID ={1})
				  and pdmTabGridMetaColumn.ExternalMappingName <> ''", gridTabId, currentGridBlockId);


            DataTable TabGridColumnResult = new DataTable ();

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                TabGridColumnResult = DataAcessHelper.GetDataTableQueryResult(conn, query, null);


            }

            if (TabGridColumnResult.Rows.Count > 0)
            { 
               toReturn= TabGridColumnResult.AsDataRowEnumerable().Select(o => o["ExternalMappingName"] as string).ToList();
            }

            return toReturn;

        }
        // cannot use int? as paratmer !!
        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void GetGridExternalColumnValue(int gridTabId, int currentGridBlockId, string referenceIds)
        {
            if (string.IsNullOrEmpty(referenceIds))
                return;

            PdmBlockClrDto dmBlockClrDto = PdmCacheManager.DictBlockCache[currentGridBlockId];
            if (dmBlockClrDto.BlockPdmGridDto == null)
                return;

            bool IsGetAliasname = false;
            // Get Grading Size will call PrepareGetGridDataTable and set 
            DataTable gridcolumnResultDataTable = PLMSGetGridValueHeler.LoadVariousGridColumnValue(gridTabId, currentGridBlockId, referenceIds, false, dmBlockClrDto, false, false, IsGetAliasname);

            CLROutput.SendDataTable(gridcolumnResultDataTable);
        }

        // if gridBlockId2 == -1, block 2 doesnt exsit 
        // option paramter 1: Color, 2:Uda, 3:
        // One week
        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void GetMergeBlockPrintGrid(int tabId, int gridBlockId1, string block1RwValueFilter, int gridBlockId2, string block2RwValueFilter, string referenceIds)
        { 

            if (gridBlockId2 == -1) // it is one block
            {
               DataTable gridBlock1 =  PLMSGetGridValueHeler.GetOneGridBlockSelectRows(tabId, gridBlockId1,  block1RwValueFilter, referenceIds, true, false);

               CLROutput.SendDataTable(gridBlock1);


            }
            else // it is merger gridblcok
            {
               DataTable gridTable1 =  PLMSGetGridValueHeler.GetOneGridBlockSelectRows(tabId, gridBlockId1, block1RwValueFilter, referenceIds, true, false); 

               DataTable gridTable2 = PLMSGetGridValueHeler.GetOneGridBlockSelectRows(tabId, gridBlockId2, block2RwValueFilter, referenceIds, true, false); ;




               if (  gridTable2.Columns.Count == gridTable1.Columns.Count )
               {
                   for (int i = 0; i < gridTable2.Columns.Count; i++)
                   {
                       gridTable2.Columns[i].ColumnName = gridTable1.Columns[i].ColumnName;
                   }


                   gridTable1.Merge(gridTable2);


                  

               }



               // int count =1;
               //foreach (DataRow row in gridTable1.Rows)
               //{
               //    if (row.Table.Columns.Contains("Sort"))
               //    {
               //        row["Sort"] = count;
               //        count++;
 
               //    }
               
               //}



               CLROutput.SendDataTable(gridTable1);
             

            }
            
      
        }


        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void GetGridWithValueIDWithTimeZone(int tabId, int currentGridBlockId, string referenceIds, string clientTimeZonekey )
        {
            PLMSGetGridValueHeler.GetGridValue(tabId, currentGridBlockId, referenceIds, false, false,true);
        }


        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void GetGridWithValueIDWithTimeZoneWithColumnName( int currentGridBlockId, string referenceIds, string clientTimeZonekey)
        {
            PLMSGetGridValueHeler.GetGridValue(null, currentGridBlockId, referenceIds, false, true,true);
        }

        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void GetRichText(int richTectFileId)
        {

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();
                DataAcessHelper.ExecuteReadUnCommmited(conn);

                string query = @"select  tblSketch.OriginalImage    from tblSketch  where SketchID =@richTectFileId";
                List<SqlParameter> listPara = new List<SqlParameter>();
                listPara.Add(new SqlParameter("@richTectFileId", richTectFileId));

                //command.Parameters.Add("@Photo",   SqlDbType.Image, photo.Length).Value = photo;

                DataTable result = DataAcessHelper.GetDataTableQueryResult(conn, query, listPara);

                if (result.Rows.Count > 0)
                {

                    byte[] rowData = result.Rows[0]["OriginalImage"] as byte[];

                    if (rowData != null)
                    {
                        string str = System.Text.Encoding.Default.GetString(rowData);
                       // CLROutput.OutputDebug(str);

                        CLROutput.OutputDebug("------------------");
                      //  string str2 = HtmlToPlainText.StripHTML(str);
                        DataTable datatable = new DataTable();

                        DataColumn column = new DataColumn("Text");
                        datatable.Columns.Add(column);

                       //column.DataType = typeof (String );
                      // column.MaxLength  = 200;


                       
                        var row = datatable.NewRow();
                        row["Text"] = str;
                        datatable.Rows.Add (row); 
                        CLROutput.SendDataTable(datatable);

 


                             //LROutput.OutputDebug(str2);
                    }
                
                }
              

                DataAcessHelper.ExecuteReadCommmited(conn);
            }

          //  GetGridValue(tabId, currentGridBlockId, referenceIds, false);
        }


        public static void GetTabValue(int tabId, string referenceIds, bool isShowLookupitem, bool IsSubItemFullPathName = true)
        {
            if (string.IsNullOrEmpty(referenceIds))
                return;

            System.Data.DataTable tabFieldResultDataTable = GetTabDataTable(tabId, referenceIds, isShowLookupitem, IsSubItemFullPathName);



           CLROutput.SendDataTable(tabFieldResultDataTable);
        }

        private static System.Data.DataTable GetTabDataTable(int tabId, string referenceIds, bool isShowLookupitem, bool IsSubItemFullPathName)
        {
            string[] arrayInputmainReferenceIds = referenceIds.Trim().Replace(System.Environment.NewLine, "").Split(',');

            PdmTabClrDto aPdmTabClrDto = PdmCacheManager.DictTabCache[tabId];


            List<BlockSubitemClrUserDefineDto> tabSubitemDtoList = PdmCacheManager.GetTabBlockSubitem(tabId);
            List<int> subitemIds = tabSubitemDtoList.Select(o => o.SubItemID).ToList();
            List<PdmBlockSubItemClrDto> listclrSubitems = PdmCacheManager.GetMutiplePdmBlockSubItemEntityFromCache(subitemIds);


            System.Data.DataTable tabFieldResultDataTable = PLMSReferenceValueRetrieveBL.RetrieveDataTableReferenceSimpleDcutValue(arrayInputmainReferenceIds, listclrSubitems, isShowLookupitem);


            #region------------- update Datatable column name by name conversion

            Dictionary<string, DataColumn> dictDataColumn = new Dictionary<string, DataColumn>();
            foreach (DataColumn dataColumn in tabFieldResultDataTable.Columns)
            {
                dictDataColumn.Add(dataColumn.ColumnName, dataColumn);
            }


            if (IsSubItemFullPathName)
            {
                foreach (var subitemDto in tabSubitemDtoList)
                {
                    if (dictDataColumn.ContainsKey(subitemDto.SubItemID.ToString()))
                    {
                        dictDataColumn[subitemDto.SubItemID.ToString()].ColumnName = subitemDto.SubItemFullPathName;
                    }
                }
            }
            else // it iwll show externa mappimg name
            {
                var dictSubItemMappng = PdmTabBlockSubItemExtraInfoDal.DictTabSubitemExternapMappingName[tabId];

                List<string> noMappingColumnsList = new List<string>();

                var productRefereIdSubItemDto = PdmCacheManager.GetPdmBlockSubItemDtoWithInternalCode(BlockRegister.ProductReferenceIdBlock.ProductReferenceId);

                string referenceIdExternaName = "";

                foreach (int subItemID in dictSubItemMappng.Keys)
                {

                    if (dictDataColumn.ContainsKey(subItemID.ToString()))
                    {
                        string exterName = dictSubItemMappng[subItemID];
                        if (string.IsNullOrEmpty(exterName))
                        {
                            exterName = subItemID.ToString() + "_NoMapping";

                            noMappingColumnsList.Add(exterName);
                        }

                        dictDataColumn[subItemID.ToString()].ColumnName = exterName;

                        if (subItemID == productRefereIdSubItemDto.SubItemId)
                        {
                            referenceIdExternaName = exterName;
                        }
                    }

                }

                foreach (string nomappingColumn in noMappingColumnsList)
                {
                    tabFieldResultDataTable.Columns.Remove(nomappingColumn);
                }

                if (!string.IsNullOrEmpty(referenceIdExternaName))
                {
                    // need to remove hradr coding referenceId
                    if (tabFieldResultDataTable.Columns.Contains(referenceIdExternaName))
                    {
                        tabFieldResultDataTable.Columns.Remove(referenceIdExternaName);

                        // rename productReferenId as referenceIdExternaName
                        tabFieldResultDataTable.Columns["ProductReferenceID"].ColumnName = referenceIdExternaName;

                    }
                }



            }

            #endregion
            return tabFieldResultDataTable;
        }

 
    }
}