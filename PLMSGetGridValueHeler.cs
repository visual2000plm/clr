using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;

namespace PLMCLRTools
{
    public class PLMSGetGridValueHeler
    {

      

         //[Microsoft.SqlServer.Server.SqlProcedure]
        public static void GetGridValue(int? gridTabId, int currentGridBlockId, string referenceIds, bool isShowLookup, bool isOnlyShowColumnName, bool IsGetAliasname )
        {
            if (string.IsNullOrEmpty(referenceIds))
                return;

            PdmBlockClrDto dmBlockClrDto = PdmCacheManager.DictBlockCache[currentGridBlockId];
            if (dmBlockClrDto.BlockPdmGridDto == null)
                return;

           
            // Get Grading Size will call PrepareGetGridDataTable and set 
            DataTable gridcolumnResultDataTable = LoadVariousGridColumnValue(gridTabId, currentGridBlockId, referenceIds, isShowLookup, dmBlockClrDto, isOnlyShowColumnName, false, IsGetAliasname);

            // it is external name:
            if (!IsGetAliasname)
            {
                if (gridcolumnResultDataTable.Columns.Contains("RowValueGUID"))
                {
                    gridcolumnResultDataTable.Columns.Remove("RowValueGUID");
                }
            }

            CLROutput.SendDataTable(gridcolumnResultDataTable);
        }

        internal static void GetUserPrintGrid(int userId, int gridTabId, int currentGridBlockId, string referenceIds, bool isShowLookup, bool isOnlyShowColumnName)
        {
            if (string.IsNullOrEmpty(referenceIds))
                return;

            PdmBlockClrDto dmBlockClrDto = PdmCacheManager.DictBlockCache[currentGridBlockId];
            if (dmBlockClrDto.BlockPdmGridDto == null)
                return;

            bool IsGetAliasname = true;
            DataTable gridcolumnResultDataTable = LoadVariousGridColumnValue(gridTabId, currentGridBlockId, referenceIds, isShowLookup, dmBlockClrDto, isOnlyShowColumnName, false, IsGetAliasname);


          PdmGridClrDto blockPdmGridDto = dmBlockClrDto.BlockPdmGridDto;

          PdmGridMetaColumnClrDto requestByColumn = blockPdmGridDto[GridRegister.SystemDefinePrintGrid.SystemDefinePrintRequestBy];
          PdmGridMetaColumnClrDto requestDateColumn = blockPdmGridDto[GridRegister.SystemDefinePrintGrid.SystemDefinePrintRequestDate];
           PdmGridMetaColumnClrDto requestSelectRowDateColumn = blockPdmGridDto[GridRegister.SystemDefinePrintGrid.SystemDefinePrintSelectedRow];



           if (requestByColumn != null && requestDateColumn != null && requestSelectRowDateColumn != null)
           {

               int entityId = requestByColumn.EntityId.Value;

               string userDisplay = string.Empty;
               if (PdmCacheManager.DictEntityLookupCache.ContainsKey(entityId))
               {
                   Dictionary<object, string> dictLookitem = PdmCacheManager.DictEntityLookupCache[entityId];
                   if (dictLookitem.ContainsKey(userId))
                   {
                       userDisplay = dictLookitem[userId];
                   }

                  

               }
        

             var requByDataTableColumn = gridcolumnResultDataTable.Columns.Cast<DataColumn>().Where(o => o.ColumnName.EndsWith("_" + requestByColumn.GridColumnId.ToString())).FirstOrDefault();
             if (requByDataTableColumn != null &&  !string.IsNullOrEmpty (userDisplay))
             {

                 string fitler = string.Format(@"{0}='{1}' ", requByDataTableColumn.ColumnName, userDisplay);
                 DataRow[] filterByUserRow = gridcolumnResultDataTable.Select(fitler);


                 CLROutput.OutputDebug("fitler string =" + fitler); 
                  CLROutput.OutputDebug("filterByUserRow=" + filterByUserRow.Length); 

                  DataTable filterDatatable = gridcolumnResultDataTable.Clone();
                 int  totalColun = filterDatatable.Columns.Count ;
                  foreach (DataRow row in filterByUserRow)
                  {
                      //System.ArgumentException: This row already belongs to another table.

                      var newRow = filterDatatable.NewRow();
                      for (int i = 0; i < totalColun; i++)
                      {
                          newRow[i] = row[i];
                      }
                      filterDatatable.Rows.Add(newRow); 
                  }


                  gridcolumnResultDataTable = filterDatatable;
                 
             
             }
 
           }

        

           CLROutput.SendDataTable(gridcolumnResultDataTable);
        }


        internal static void GetUserReqPrintGrid(int requestId, int gridTabId, int currentGridBlockId, string referenceIds, bool isShowLookup, bool isOnlyShowColumnName)
        {
            if (string.IsNullOrEmpty(referenceIds))
                return;

            PdmBlockClrDto dmBlockClrDto = PdmCacheManager.DictBlockCache[currentGridBlockId];
            if (dmBlockClrDto.BlockPdmGridDto == null)
                return;

            bool IsGetAliasname = true;
            DataTable gridcolumnResultDataTable = LoadVariousGridColumnValue(gridTabId, currentGridBlockId, referenceIds, isShowLookup, dmBlockClrDto, isOnlyShowColumnName, false, IsGetAliasname);


            PdmGridClrDto blockPdmGridDto = dmBlockClrDto.BlockPdmGridDto;

            PdmGridMetaColumnClrDto requestByColumn = blockPdmGridDto[GridRegister.SystemDefinePrintGrid.SystemDefinePrintRequestBy];
            PdmGridMetaColumnClrDto requestDateColumn = blockPdmGridDto[GridRegister.SystemDefinePrintGrid.SystemDefinePrintRequestDate];
            PdmGridMetaColumnClrDto requestSelectRowDateColumn = blockPdmGridDto[GridRegister.SystemDefinePrintGrid.SystemDefinePrintSelectedRow];



            if (requestByColumn != null && requestDateColumn != null && requestSelectRowDateColumn != null)
            {

                int entityId = requestByColumn.EntityId.Value;

                string userDisplay = string.Empty;
                if (PdmCacheManager.DictEntityLookupCache.ContainsKey(entityId))
                {
                    Dictionary<object, string> dictLookitem = PdmCacheManager.DictEntityLookupCache[entityId];
                    //if (dictLookitem.ContainsKey(userId))
                    //{
                    //    userDisplay = dictLookitem[userId];
                    //}



                }


                var requByDataTableColumn = gridcolumnResultDataTable.Columns.Cast<DataColumn>().Where(o => o.ColumnName.EndsWith("_" + requestByColumn.GridColumnId.ToString())).FirstOrDefault();
                if (requByDataTableColumn != null && !string.IsNullOrEmpty(userDisplay))
                {

                    string fitler = string.Format(@"{0}='{1}' ", requByDataTableColumn.ColumnName, userDisplay);
                    DataRow[] filterByUserRow = gridcolumnResultDataTable.Select(fitler);


                    CLROutput.OutputDebug("fitler string =" + fitler);
                    CLROutput.OutputDebug("filterByUserRow=" + filterByUserRow.Length);

                    DataTable filterDatatable = gridcolumnResultDataTable.Clone();
                    int totalColun = filterDatatable.Columns.Count;
                    foreach (DataRow row in filterByUserRow)
                    {
                        //System.ArgumentException: This row already belongs to another table.

                        var newRow = filterDatatable.NewRow();
                        for (int i = 0; i < totalColun; i++)
                        {
                            newRow[i] = row[i];
                        }
                        filterDatatable.Rows.Add(newRow);
                    }


                    gridcolumnResultDataTable = filterDatatable;


                }

            }



            CLROutput.SendDataTable(gridcolumnResultDataTable);
        }


       // GetUserPrintGrid

//step1: Get all Reference Grid Value
//step2: get all current reference  depdendent  grid value  ( extra -step !)
//step3: Get all  current reference first grid level reference
//step4: Get  grid  second level data

//step5;  Get all reference( current reference + external reference simple duc value  Simple 
//step6: update   second level grid ( CurrentRefSimpleDcuAndCurrentRefKey)

//step7: update first level grid  with seond level grid (CurrentRefSimpleDcuAndCurrentRefKey)

//step8: update current reference depdende grid column value
//step9: update main reference grid value


        // public static DataTable PrepareGetGridDataTable(int? gridTabId, int currentGridBlockId, string referenceIds, bool isShowLookup, PdmBlockClrDto dmBlockClrDto, bool isOnlyShowGridColumnName  ,bool isUsedToCacualteGradingSizeValue  )



        internal static DataTable LoadVariousGridColumnValue(int? gridTabId, int currentGridBlockId, string referenceIds, bool isShowLookup, PdmBlockClrDto dmBlockClrDto, bool isOnlyShowGridColumnName, bool isUsedToCacualteGradingSizeValue, bool IsGetAliasname)
        {

            PdmGridClrDto blockPdmGridDto = dmBlockClrDto.BlockPdmGridDto;



         

            #region ----------------------  step1 get all Grid structure column ( currnet, depedency column etc)

            // get specail column
           // List<PdmGridMetaColumnClrDto> gridAllColumnListDto = blockPdmGridDto.PdmGridMetaColumnList;//.Where(c => columnIds.Contains(c.GridColumnId)).ToList();
            // collect All columns for: Simple DCU ( DcuId) and DCUColumnBlockID and DcuColumnId
            List<int> allDCUColumnBlockID_ColumnIds = new List<int>();
            List<int> allDCUColumnID_ColumnIds = new List<int>();
            List<int> allGridRefSimpleDcuIds = new List<int>();

            PLMSGetGridSpecialColumn.GetAllGridSpecailColumnIds(blockPdmGridDto, allDCUColumnBlockID_ColumnIds, allDCUColumnID_ColumnIds, allGridRefSimpleDcuIds);
            //foreach (int blockId in allDCUColumnBlockID_ColumnIds)
            //{
            //    CLROutput.OutputDebug("blockId=" + blockId);
            //}


            allDCUColumnBlockID_ColumnIds.Add(currentGridBlockId);
            allDCUColumnID_ColumnIds.AddRange(blockPdmGridDto.PdmGridMetaColumnList.Select(o => o.GridColumnId));
 

            int[] ducGridBlockIds = allDCUColumnBlockID_ColumnIds.Distinct().ToArray();
            int[] allDcuGridColumnidArray = allDCUColumnID_ColumnIds.ToArray();



  


            #endregion

            #region-----------------     step2: get all current reference all Reference Grid Value and   depdendent  grid value

            //  arrayInputmainReferenceIds
            string[] arrayInputmainReferenceIds = referenceIds.Trim().Replace(System.Environment.NewLine, "").Split(',');
            List<int> mainReferenceIntIds = new List<int>();
            foreach (string refid in arrayInputmainReferenceIds)
            {
                mainReferenceIntIds.Add(int.Parse(refid));
            }



        
            // Key1: blockId   key: ReferenceId,  List<SimpleGridProductRow>
            Dictionary<int, Dictionary<int, List<SimpleGridProductRow>>> dictCurrentRefGridProductRow =
                PLMSReferenceValueRetrieveBL.RetrieveBlocksReferencesGridColumnsRowList(mainReferenceIntIds.ToArray(), ducGridBlockIds, allDcuGridColumnidArray);

             // no result set from dbase !
            if (!dictCurrentRefGridProductRow.ContainsKey(currentGridBlockId))
            {
                DataTable emptyDataTable = new DataTable();

                //emptyDataTable.Columns.Add("ProductReferenceID");
                //emptyDataTable.Columns.Add("RowID");
                //emptyDataTable.Columns.Add("RowID");
                //emptyDataTable.Columns.Add("Sort");

                emptyDataTable.Columns.Add(GridColumnConstantName.ProductReferenceID);
                emptyDataTable.Columns.Add(GridColumnConstantName.RowValueGUID);

                emptyDataTable.Columns.Add(GridColumnConstantName.Sort);


                foreach (var colimnDto in blockPdmGridDto.PdmGridMetaColumnList)
                {
                    emptyDataTable.Columns.Add(colimnDto.GridColumnId.ToString());
                }
              //  blockPdmGridDto.PdmGridMetaColumnList.ToDictionary(o => o.GridColumnId.ToString(), o => o);

                SetupTabBlockDatatableColumnName(gridTabId, dmBlockClrDto, blockPdmGridDto, emptyDataTable, IsGetAliasname);

          

               return emptyDataTable;
              
            }

#if DEBUG
            //  DebugDictBlockReferenceSimpleGridProductRowList(dictCurrentRefGridProductRow);
#endif


            #endregion

       
            #region ------------- step3 Get  first grid level reference

            // get first level productgrid reference
            List<int> firstlevelProductGridReferenceIds = new List<int>();
            GetDictionaryProductGridReferenceIds(dictCurrentRefGridProductRow, firstlevelProductGridReferenceIds);
            

            #endregion


            #region ------- step4 Get all  current reference all product Grid first grid level reference


            Dictionary<int, Dictionary<int, List<SimpleGridProductRow>>> dictFirstlevelGridBlockGridproductRow =
               PLMSReferenceValueRetrieveBL.RetrieveBlocksReferencesGridColumnsRowList(firstlevelProductGridReferenceIds.ToArray(), ducGridBlockIds, allDcuGridColumnidArray);

          //  DebugDictBlockReferenceSimpleGridProductRowList(dictFirstlevelGridBlockGridproductRow);

            // get secondlevel level productgrid reference 
            List<int> secondlevelProductGridReferenceIds = new List<int>();
            GetDictionaryProductGridReferenceIds(dictFirstlevelGridBlockGridproductRow, secondlevelProductGridReferenceIds);


            #endregion

            #region---------- step5 product Grid second grid level reference

            Dictionary<int, Dictionary<int, List<SimpleGridProductRow>>> dictSecondlevelGridBlockGridproductRow =
              PLMSReferenceValueRetrieveBL.RetrieveBlocksReferencesGridColumnsRowList(secondlevelProductGridReferenceIds.ToArray(), ducGridBlockIds, allDcuGridColumnidArray);


            #endregion

            #region ------------ step6 Get all reference( current reference + external reference simple duc value  Simple

            List<int> allReferenceIds = new List<int>(mainReferenceIntIds);
            allReferenceIds.AddRange(firstlevelProductGridReferenceIds);
            allReferenceIds.AddRange(secondlevelProductGridReferenceIds);



            List<PdmBlockSubItemClrDto> listPdmBlockSubItemClrDto = PdmCacheManager.GetMutiplePdmBlockSubItemEntityFromCache(allGridRefSimpleDcuIds);
            Dictionary<int, Dictionary<int, object>> dictMainReferenceAndFirstAndSecondLevelReferencesSimpleDcuValues = PLMSReferenceValueRetrieveBL.RetrieveReferenceSimpleDcutValue(allReferenceIds, listPdmBlockSubItemClrDto);

            #endregion

            #region-------- step7 update SecondLevel grid/ First level
            //Current Ref and Grid:  update seconde level current Ref and Current Ref Key column
            UpdateSimpleGridProductRowCurrentRefSimpleDcuAndKeyDependentColumn(dictSecondlevelGridBlockGridproductRow, dictMainReferenceAndFirstAndSecondLevelReferencesSimpleDcuValues);

            // Current Ref and Grid: update first level current Ref and Current Ref Key column
            UpdateSimpleGridProductRowCurrentRefSimpleDcuAndKeyDependentColumn(dictFirstlevelGridBlockGridproductRow, dictMainReferenceAndFirstAndSecondLevelReferencesSimpleDcuValues);


            // Product Grid Line item: update first level Product grid with Second level
            UpdateSimpleGridProductRowExternalProductRefKeyAndDependentColumn(dictFirstlevelGridBlockGridproductRow, dictSecondlevelGridBlockGridproductRow, dictMainReferenceAndFirstAndSecondLevelReferencesSimpleDcuValues);


            // update 0 level prodcutgrid
            UpdateSimpleGridProductRowExternalProductRefKeyAndDependentColumn(dictCurrentRefGridProductRow, dictFirstlevelGridBlockGridproductRow, dictMainReferenceAndFirstAndSecondLevelReferencesSimpleDcuValues);


            UpdateSimpleGridProductRowCurrentRefSimpleDcuAndKeyDependentColumn(dictCurrentRefGridProductRow, dictMainReferenceAndFirstAndSecondLevelReferencesSimpleDcuValues);

            #endregion

            #region------------- Convert ConvertSimpleGridProduct row to Datatable
            //DataTable gridcolumnResultDataTable = new DataTable();

         
       

            //gridcolumnResultDataTable.Columns.Add(GridColumnConstantName.ProductReferenceID);
            //gridcolumnResultDataTable.Columns.Add(GridColumnConstantName.RowID);
            //gridcolumnResultDataTable.Columns.Add(GridColumnConstantName.Sort);

            var columnIds = blockPdmGridDto.PdmGridMetaColumnList.Select(o => o.GridColumnId).ToList();
            //foreach (int columnId in columnIds)
            //{
            //    gridcolumnResultDataTable.Columns.Add(columnId.ToString());
            //}


          DataTable gridcolumnResultDataTable  =  PLMSReferenceValueRetrieveBL.CreateDataTableStrcutureFromGridColumn(blockPdmGridDto.PdmGridMetaColumnList);
            //DataTable roReturnDcuTable = CreateDataTableStrcutureFromGridColumn(columnList);




            Dictionary<int, List<SimpleGridProductRow>>  dictRefereIDRowList = dictCurrentRefGridProductRow[currentGridBlockId];

            foreach (int refId in dictRefereIDRowList.Keys)
            {
                var listproductRow = dictRefereIDRowList[refId];

                DataTableUtility.ConvertSimpleGridProductRowToDataTable(listproductRow, columnIds, gridcolumnResultDataTable);

            }

            #endregion

            #region--------------- Master Entity depedent column

            PLMSSetDepdentMasterEntityAndLookup.SetDepdentMasterEntityColumn(blockPdmGridDto, gridcolumnResultDataTable);

            #endregion

            #region----------  Setup DataTable DD LLookupValue

            if (isShowLookup)
            {
                PLMSSetDepdentMasterEntityAndLookup.SetupDataTableDDLLookupValue(blockPdmGridDto.PdmGridMetaColumnList, gridcolumnResultDataTable);
            }
           #endregion

            #region------------------ finally Update columnName as qulified path name


            if (isOnlyShowGridColumnName)
            {

             

                for (int i = 3; i <= gridcolumnResultDataTable.Columns.Count - 1; i++)
                {
                    //dictDataColumn.Add(dataColumn.ColumnName, dataColumn);
                    var dataColumn = gridcolumnResultDataTable.Columns[i];
                    string columnIdName = dataColumn.ColumnName.ToString();
                    int columnid = int.Parse(columnIdName);

                    dataColumn.ColumnName = PdmCacheManager.DictAllPdmGridMetaColumnCache[columnid].ColumnName;

                    CLROutput.OutputDebug("dataColumn.ColumnName" + dataColumn.ColumnName);

                }
            }
            else
            {
                SetupTabBlockDatatableColumnName(gridTabId, dmBlockClrDto, blockPdmGridDto, gridcolumnResultDataTable, IsGetAliasname);

            }
           

          

            #endregion


            #region ------------------ check if this Fit, pp , top, gradingg ,and QC

            if (! isUsedToCacualteGradingSizeValue)
            {
                if (mainReferenceIntIds != null && mainReferenceIntIds.Count > 0)
                {
                    PLMSpecGradingHelper.CheckSpecialColumnToInchOrCM(blockPdmGridDto, gridcolumnResultDataTable, mainReferenceIntIds[0]);
                }
               

            }

            PLMSpecGradingHelper.SetupBodypartName(blockPdmGridDto, gridcolumnResultDataTable);


            #endregion
         
            return gridcolumnResultDataTable;
        }

         private static void SetupTabBlockDatatableColumnName(int? gridTabId, PdmBlockClrDto dmBlockClrDto, PdmGridClrDto blockPdmGridDto, DataTable gridcolumnResultDataTable, bool IsGetAliasname)
         {
             List<GridColumnClrUserDefineDto> selectedTabLayoutColumnListDto = new List<GridColumnClrUserDefineDto>();
             if (gridTabId.HasValue)
             {
                 selectedTabLayoutColumnListDto = PdmCacheManager.GetTabGridSelectColumnAndAliasDtoFromCache(gridTabId.Value, dmBlockClrDto, IsGetAliasname);
             }

             Dictionary<string, GridColumnClrUserDefineDto> dictSelectedTabLayoutColumnListDto = selectedTabLayoutColumnListDto.ToDictionary(o => o.GridColumnID.ToString(), o => o);

             var dictAllGridColumnDto = blockPdmGridDto.PdmGridMetaColumnList.ToDictionary(o => o.GridColumnId.ToString(), o => o);



             foreach (DataColumn dataColumn in gridcolumnResultDataTable.Columns)
             {
                 string columnIdName = dataColumn.ColumnName.ToString();

                 if (IsGetAliasname)  // show alias name
                 {
                   

                     string columnSuffix = "_" + columnIdName;
                     string finalColumn = string.Empty;

                     if (dictSelectedTabLayoutColumnListDto.ContainsKey(columnIdName))
                     {
                         var columnDto = dictSelectedTabLayoutColumnListDto[columnIdName];

                         if (!columnDto.ColumnName.Contains(columnSuffix))
                         {
                             dataColumn.ColumnName = columnDto.ColumnName + columnSuffix;
                         }
                         else
                         {
                             dataColumn.ColumnName = columnDto.ColumnName;
                         }
                     }
                     else // user defailt grid columId
                     {
                         if (dictAllGridColumnDto.ContainsKey(columnIdName))
                         {
                             dataColumn.ColumnName = dictAllGridColumnDto[columnIdName].ColumnName + columnSuffix;
                         }
                     }
                 }
                 else // it will show extternal mappiung  name 
                 {

                     if (dictSelectedTabLayoutColumnListDto.ContainsKey(columnIdName))
                     {
                         var columnDto = dictSelectedTabLayoutColumnListDto[columnIdName];

                         dataColumn.ColumnName = columnDto.ColumnName;
                     }
                     else // user defailt grid columId
                     {
                         if (dictAllGridColumnDto.ContainsKey(columnIdName))
                         {
                             dataColumn.ColumnName = dictAllGridColumnDto[columnIdName].ColumnName;
                         }
                     }
                 }



             }
         }

         private static void DebugDictBlockReferenceSimpleGridProductRowList(Dictionary<int, Dictionary<int, List<SimpleGridProductRow>>> dictCurrentRefGridProductRow)
         {
             foreach (var blockId in dictCurrentRefGridProductRow.Keys)
             {
                 Dictionary<int, List<SimpleGridProductRow>> debugResult = dictCurrentRefGridProductRow[blockId];

                 foreach (var refid in debugResult.Keys)
                 {
                     List<SimpleGridProductRow> debugResult2 = debugResult[refid];
                     CLROutput.SendDataTable(DataTableUtility.ConvertSimpleGridProductRowToDataTable(debugResult2));

                 }

             }
         }

     
         internal static void UpdateSimpleGridProductRowExternalProductRefKeyAndDependentColumn(Dictionary<int, Dictionary<int, List<SimpleGridProductRow>>> dictBlockFirstLevelReferenceGridRowDataSource, Dictionary<int, Dictionary<int, List<SimpleGridProductRow>>> dictBlockSecondLevelReferenceGridRowDataSource, Dictionary<int, Dictionary<int, object>> dictSecondLevelProductGridReferenceSimpleDcuValues)
         {

             foreach (int gridBlockId in dictBlockFirstLevelReferenceGridRowDataSource.Keys)
             {

                 PdmBlockClrDto gridBlockEntity = PdmCacheManager.DictBlockCache[gridBlockId];


                 PdmGridClrDto firstLevelPdmGridEntity = gridBlockEntity.BlockPdmGridDto;


                 var currentGridProductReferenceIdColumn = firstLevelPdmGridEntity.ProductGridProductReferenceColumn;

                 Dictionary<int, List<SimpleGridProductRow>> dictFirstLevelRefGridProductRow = dictBlockFirstLevelReferenceGridRowDataSource[gridBlockId];

                 foreach (int currentMainReferenceId in dictFirstLevelRefGridProductRow.Keys)
                 {

                     List<SimpleGridProductRow> firstLevelReferenceGridProductRowList = dictFirstLevelRefGridProductRow[currentMainReferenceId];

                     foreach (var firstLevelforeignGridKeyColumn in firstLevelPdmGridEntity.ProductGridForeignKeyColumns)
                     {

                         // pfkDcuBlockId
                         var fkDcuBlockId = firstLevelforeignGridKeyColumn.DcucolumnBlockId.Value;
                         if (dictBlockSecondLevelReferenceGridRowDataSource.ContainsKey(fkDcuBlockId))
                         {

                             var dictFKProductGridRowInSecondLevel = dictBlockSecondLevelReferenceGridRowDataSource[fkDcuBlockId];

                             //begin gridProductRow loop
                             foreach (var firstLevelGridProductRow in firstLevelReferenceGridProductRowList)
                             {
                                 var lineItemProductReferenceId = ControlTypeValueConverter.ConvertValueToInt(firstLevelGridProductRow[currentGridProductReferenceIdColumn.GridColumnId]);
                                 if (lineItemProductReferenceId.HasValue)
                                 {
                                     if (dictFKProductGridRowInSecondLevel.ContainsKey((int)lineItemProductReferenceId))
                                     {
                                         var secondLevelGridProductRowList = dictFKProductGridRowInSecondLevel[(int)lineItemProductReferenceId];

                                         var firstLevelforeignGridKeyColumnValueId = firstLevelGridProductRow[firstLevelforeignGridKeyColumn.GridColumnId];
                                         if (firstLevelforeignGridKeyColumnValueId != null)
                                         {
                                             var secondLevelGridKeyColumnId = firstLevelforeignGridKeyColumn.DcucolumnId.Value;

                                             var secondLevelFiltedGridProductRowList = secondLevelGridProductRowList
                                                 .Where(r => r[secondLevelGridKeyColumnId] != null && r[secondLevelGridKeyColumnId].ToString() == firstLevelforeignGridKeyColumnValueId.ToString())
                                                 .FirstOrDefault();

                                             if (secondLevelFiltedGridProductRowList != null)
                                             {
                                                 // Key: ForeigkKeyColumnId; Value ForeignKeyDepdedentColumns
                                                 var dictfirstLevelPdmGridEntityForeignKeyDepdedentColumns = firstLevelPdmGridEntity.ForeignKeyDepdedentColumns;

                                                 if (dictfirstLevelPdmGridEntityForeignKeyDepdedentColumns.ContainsKey(firstLevelforeignGridKeyColumn.GridColumnId))
                                                 {
                                                     var firstLevelDepdedentColumnList = firstLevelPdmGridEntity.ForeignKeyDepdedentColumns[firstLevelforeignGridKeyColumn.GridColumnId];

                                                     // default value set
                                                     foreach (var firstLeveldepdenColumn in firstLevelDepdedentColumnList)
                                                     {
                                                         int secondLevelMappingGridColumnId = firstLeveldepdenColumn.DcucolumnId.Value;

                                                         object value = ControlTypeValueConverter.ConvertValueToObject(secondLevelFiltedGridProductRowList[secondLevelMappingGridColumnId], firstLeveldepdenColumn.ColumnTypeId);

                                                         firstLevelGridProductRow[firstLeveldepdenColumn.GridColumnId] = value;
                                                     }

                                                 }


                                             }


                                         }


                                     }
                                 }
                             }
                             //end gridProductRow loop

                         }

                     } // end  foreach (var firstLevelforeignGridKeyColumn in firstLevelPdmGridEntity.ProductGridForeignKeyColumns)



                     if (firstLevelPdmGridEntity.GridType == (int)EmGridType.ProductBasedGrid)
                     {
                         ProcessProductGridLineItemSimpleDcuValues(dictSecondLevelProductGridReferenceSimpleDcuValues, firstLevelPdmGridEntity, currentGridProductReferenceIdColumn, firstLevelReferenceGridProductRowList);
                     }

#if DEBUG
                   //  DataTable currentGridProductRowDataTable = DataTableUtility.ConvertSimpleGridProductRowToDataTable(firstLevelReferenceGridProductRowList);

                  //   CLROutput.SendDataTable(currentGridProductRowDataTable);

# endif



                 }



                 //  }


             }


         }

         private static void ProcessProductGridLineItemSimpleDcuValues(Dictionary<int, Dictionary<int, object>> dictParentLevelOneSimpleDcuValues, PdmGridClrDto aPdmGridEntity, PdmGridMetaColumnClrDto productReferenceIdColumn, List<SimpleGridProductRow> currentGridProductRowList)
         {
             foreach (var gridProductRow in currentGridProductRowList)
             {
                 var lineItemProductReferenceId = ControlTypeValueConverter.ConvertValueToInt(gridProductRow[productReferenceIdColumn.GridColumnId]);
                 if (lineItemProductReferenceId.HasValue)
                 {
                     if (dictParentLevelOneSimpleDcuValues.ContainsKey((int)lineItemProductReferenceId))
                     {
                         var dictSubitemValueList = dictParentLevelOneSimpleDcuValues[(int)lineItemProductReferenceId.Value];
                         foreach (var column in aPdmGridEntity.ProductGridSimpleDCUColumn)
                         {
                             if (dictSubitemValueList.ContainsKey(column.Dcuid.Value))
                             {
                                 object cellValue = dictSubitemValueList[column.Dcuid.Value];
                                 gridProductRow[column.GridColumnId] = ControlTypeValueConverter.ConvertValueToObject(cellValue, column.ColumnTypeId);
                             }
                         }
                     }
                 }
             }
         }


         internal static void UpdateSimpleGridProductRowCurrentRefSimpleDcuAndKeyDependentColumn(
         Dictionary<int, Dictionary<int, List<SimpleGridProductRow>>> dictBlockReferenceGridRowDataSource,
            Dictionary<int, Dictionary<int, object>> dictProductGridReferenceSimpleDcuValues)
         {

             foreach (int gridBlockId in dictBlockReferenceGridRowDataSource.Keys)
             {

                 PdmBlockClrDto gridBlockEntity = PdmCacheManager.DictBlockCache[gridBlockId];
             

                 if (gridBlockEntity.BlockPdmGridDto != null)
                 {

                     PdmGridClrDto aPdmGridEntity = gridBlockEntity.BlockPdmGridDto;

                     Dictionary<int, List<SimpleGridProductRow>> dictRefGridProductRow = dictBlockReferenceGridRowDataSource[gridBlockId];

                     foreach (int currentMainReferenceId in dictRefGridProductRow.Keys)
                     {

                         List<SimpleGridProductRow> currentMainReferenceGridProductRowList = dictRefGridProductRow[currentMainReferenceId];

                         //step1: check the current Reference Simple DCU
                         if (dictProductGridReferenceSimpleDcuValues.ContainsKey(currentMainReferenceId))
                         {

                             Dictionary<int, object> dictSimpleDcuValues = dictProductGridReferenceSimpleDcuValues[currentMainReferenceId];
                             LoadCurrnetRefSimpleDcu(aPdmGridEntity, currentMainReferenceGridProductRowList, dictSimpleDcuValues);

                         }

                         // step2: check  the current Reference Grid Keycolumn


                         UpdateCurrentRefRegularColumnKeyAndDynamicMatrixKeyAndDepdentColumnValue(dictBlockReferenceGridRowDataSource, aPdmGridEntity, currentMainReferenceId, currentMainReferenceGridProductRowList);




                     }



                 }


             }


         }

         private static void UpdateCurrentRefRegularColumnKeyAndDynamicMatrixKeyAndDepdentColumnValue(Dictionary<int, Dictionary<int, List<SimpleGridProductRow>>> dictBlockSecondLevelReferenceGridRowDataSource, PdmGridClrDto aPdmGridEntity, int currentMainReferenceId, List<SimpleGridProductRow> currentMainReferenceGridProductRowList)
         {
             foreach (var currentRefGridKeyColumn in aPdmGridEntity.CurrentRefRegularColumnKeyAndDynamicMatrixKeyDCUColumn)
             {
                 int fkDcuBlockId = currentRefGridKeyColumn.DcucolumnBlockId.Value;
                 int fkColumnId = currentRefGridKeyColumn.DcucolumnId.Value;

                 if (dictBlockSecondLevelReferenceGridRowDataSource.ContainsKey(fkDcuBlockId))
                 {
                     var fkDcuDictProductGridRow = dictBlockSecondLevelReferenceGridRowDataSource[fkDcuBlockId];

                     //  var currentGridProductRowList = dictGridProductRowList[subItemIdkey];

                     if (fkDcuDictProductGridRow.ContainsKey(currentMainReferenceId))
                     {
                         var currentRefFkDcuGridRowList = fkDcuDictProductGridRow[currentMainReferenceId];

                         // foreach (var gridProductRow in currentGridProductRowList)
                         foreach (var gridProductRow in currentMainReferenceGridProductRowList)
                         {
                             var currentRefKeyColumnValue = gridProductRow[currentRefGridKeyColumn.GridColumnId];
                             if (currentRefKeyColumnValue != null)
                             {
                                 //o[fkColumnId] != null &&

                                 // CLROutput.OutputDebug("o[fkColumnId]" );// o[fkColumnId] != null &&
                                 var searchRefFkDcuCurrentRefRow = currentRefFkDcuGridRowList.Where(o => ( o[fkColumnId].ToString() == currentRefKeyColumnValue.ToString())).FirstOrDefault();

                                 if (searchRefFkDcuCurrentRefRow != null)
                                 {
                                     if (aPdmGridEntity.CurrentRefRegularColumnKeyAndDynamicMatrixKeyDCUColumnDepdedentColumns.ContainsKey(currentRefGridKeyColumn.GridColumnId))
                                     {
                                         foreach (var depdenColumn in aPdmGridEntity.CurrentRefRegularColumnKeyAndDynamicMatrixKeyDCUColumnDepdedentColumns[currentRefGridKeyColumn.GridColumnId])
                                         {
                                             int fkGridColumnId = depdenColumn.DcucolumnId.Value;
                                             object value = ControlTypeValueConverter.ConvertValueToObject(searchRefFkDcuCurrentRefRow[fkGridColumnId], depdenColumn.ColumnTypeId);

                                             gridProductRow[depdenColumn.GridColumnId] = value;
                                         }
                                     }
                                 }
                             }
                         }
                     }
                 }
             }
         }

         private static void LoadCurrnetRefSimpleDcu(PdmGridClrDto aPdmGridEntity, List<SimpleGridProductRow> gridRowList, Dictionary<int, object> dictBlockSubitemValue)
         {

             // if no 
             foreach (var metaColumn in aPdmGridEntity.CurrentRefSimpleDCUColumns)
             {
                 int columnId = metaColumn.GridColumnId;
                 int currentRefSimpleDCUID = metaColumn.Dcuid.Value;
                 if (dictBlockSubitemValue.ContainsKey(currentRefSimpleDCUID))
                 {
                     object simpleDcuValue = dictBlockSubitemValue[currentRefSimpleDCUID];

                     foreach (var productRow in gridRowList)
                     {
                         productRow[columnId] = simpleDcuValue;

                     }

                 }

             }



         }

         private static void GetDictionaryProductGridReferenceIds(Dictionary<int, Dictionary<int, List<SimpleGridProductRow>>> dictCurrentRefGridProductRow, List<int> listProductGridReferenceIds)
         {
             foreach (int blockId in dictCurrentRefGridProductRow.Keys)
             {

                 PdmBlockClrDto dependecyBlockClrDto = PdmCacheManager.DictBlockCache[blockId];
                 if (dependecyBlockClrDto.BlockPdmGridDto != null)
                 {
                     if (dependecyBlockClrDto.BlockPdmGridDto.GridType == (int)EmGridType.ProductBasedGrid)
                     {
                         Dictionary<int, List<SimpleGridProductRow>> dictRefGrid = dictCurrentRefGridProductRow[blockId];

                         foreach (int referenceID in dictRefGrid.Keys)
                         {
                             var listSimpleGridProductRow = dictRefGrid[referenceID];
                             //CLROutput.SendDataTable (  DataTableUtility.ConvertSimpleGridProductRowToDataTable(listSimpleGridProductRow));
                             CollectProductGridLineItemReferenceIds(listProductGridReferenceIds, dependecyBlockClrDto.BlockPdmGridDto, listSimpleGridProductRow);
                         }

                     }


                 }


             }
         }

         private static void CollectProductGridLineItemReferenceIds(List<int> allProductGridReferenceIds, PdmGridClrDto aPdmGridEntity, List<SimpleGridProductRow> gridProductRowList)
        {
            int gridId = aPdmGridEntity.GridId;

            var productGridProductReferenceColumn = aPdmGridEntity.ProductGridProductReferenceColumn;

            if (productGridProductReferenceColumn != null)
            {
                foreach (var row in gridProductRowList)
                {
                    var lineReferedId = row[productGridProductReferenceColumn.GridColumnId];
                    if (lineReferedId != null)
                    {
                        //int
                        allProductGridReferenceIds.Add(int.Parse(lineReferedId.ToString()));
                    }
                }
            }
        }




         internal static DataTable GetOneGridBlockSelectRows(int gridTabId, int currentGridBlockId, string block1RwValueFilter, string referenceIds, bool isShowLookup, bool isOnlyShowColumnName)
         {
             if (string.IsNullOrEmpty(referenceIds))
                 return new DataTable ();

             PdmBlockClrDto dmBlockClrDto = PdmCacheManager.DictBlockCache[currentGridBlockId];
             if (dmBlockClrDto.BlockPdmGridDto == null)
                 return new DataTable();

             bool IsGetAliasname = true;

             DataTable gridcolumnResultDataTable = LoadVariousGridColumnValue(gridTabId, currentGridBlockId, referenceIds, isShowLookup, dmBlockClrDto, isOnlyShowColumnName, false, IsGetAliasname);

             

            // block1RwValueFilter = @"'1e1cde8e-28b6-493d-b6ce-d1b81b69e83f','1e1cde8e-28b6-493d-b6ce-d1b81b69e83f'";
             //dt.Select("RefID in('1e1cde8e-28b6-493d-b6ce-d1b81b69e83f','1e1cde8e-28b6-493d-b6ce-d1b81b69e83f')");

           //  string orgstring = "6BB03992-E5FF-4B01-A41B-438CA9A7E560,6BB03992-E5FF-4B01-A41B-438CA9A7E560";
             if ( !string.IsNullOrEmpty(block1RwValueFilter))
                {


                    block1RwValueFilter = ConverGuidInstring(block1RwValueFilter);

                     string fitler = string.Format(@"{0}  in  ({1}) ", GridColumnConstantName.RowValueGUID, block1RwValueFilter);


                     CLROutput.OutputDebug("fitler string =" + fitler);
                   

                     DataRow[] filterRowValueGUIDs = gridcolumnResultDataTable.Select(fitler);

                     CLROutput.OutputDebug("filterByUserRow=" + filterRowValueGUIDs.Length);


                     DataTable filterDatatable = gridcolumnResultDataTable.Clone();
                     int totalColun = filterDatatable.Columns.Count;
                     foreach (DataRow row in filterRowValueGUIDs)
                     {
                         //System.ArgumentException: This row already belongs to another table.

                         var newRow = filterDatatable.NewRow();
                         for (int i = 0; i < totalColun; i++)
                         {
                             newRow[i] = row[i];
                         }
                         filterDatatable.Rows.Add(newRow);
                     }


                     gridcolumnResultDataTable = filterDatatable;


                 }

             return gridcolumnResultDataTable;
         }


         // ( CONVERT('1e1cde8e-28b6-493d-b6ce-d1b81b69e83f', 'System.Guid'),CONVERT('1e1cde8e-28b6-493d-b6ce-d1b81b69e83f', 'System.Guid') )
         static string ConverGuidInstring(string orgString)
         {
             //string orgstring = "6BB03992-E5FF-4B01-A41B-438CA9A7E560,6BB03992-E5FF-4B01-A41B-438CA9A7E560";

             string []  guidStrings =orgString.Split(",".ToArray ());

             string toReturn = string.Empty ;

             foreach ( string guidstr in guidStrings)
             {
                 string formatString = string.Format(@"CONVERT('{0}', 'System.Guid')",guidstr)+",";
                 toReturn = toReturn +  formatString;

             }

             if (toReturn != string.Empty)
             {
                 toReturn = toReturn.Substring(0, toReturn.Length - 1);
             }

             return toReturn;
         
         }

    }
}