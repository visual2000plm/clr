using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
namespace PLMCLRTools
{
    public static class PLMSReferenceValueRetrieveBL
    {
      

        public static Dictionary<int, Dictionary<int, List<SimpleGridProductRow>>> RetrieveBlocksReferencesGridColumnsRowList(int[] referenceIds, int[] blockIds, int[] columnIds)
        {

            if (referenceIds.Length == 0 || blockIds.Length == 0 || columnIds.Length == 0)
            {
                return new Dictionary<int, Dictionary<int, List<SimpleGridProductRow>>>();
            }


            Dictionary<int, List<int>> dictBlockNeedColumnIds = new Dictionary<int, List<int>>();

            foreach (int blockId in blockIds)
            {

                PdmGridClrDto pdmGridClrDto = PdmCacheManager.DictBlockCache[blockId].BlockPdmGridDto;
              //  List<int> needcolumnIds = pdmGridClrDto.PdmGridMetaColumnList.Select(o => o.GridColumnId) .Intersect(columnIds).ToList();

                List<int> needcolumnIds = pdmGridClrDto.PdmGridMetaColumnList.Select(o => o.GridColumnId).ToList();
                dictBlockNeedColumnIds.Add(blockId, needcolumnIds);



            }




            // CLROutput.OutputDebug("Dictionary<int, Dictionary<int, List<SimpleGridProductRow>>> RetrieveBlocksReferencesGridColumnsRowList?? statr"  );
            DataTable dtReferenceGridValue = GetMutipleBlockGridCellValueTable(referenceIds, blockIds, columnIds);

            //   CLROutput.SendDataTable(dtReferenceGridValue);


            var gridColumnQuery = from row in dtReferenceGridValue.AsDataRowEnumerable()
                                  group row by new
                                  {
                                      BlockId = (int)row[GridColumnConstantName.BlockID],
                                      ProductReferenceId = (int)row[GridColumnConstantName.ProductReferenceID],
                                    //  RowId = (int)row[GridColumnConstantName.RowID],
                                      RowValueGUID = row[GridColumnConstantName.RowValueGUID] as Guid?,
                                      Sort = (int)row[GridColumnConstantName.Sort],
                                  } into grp

                                  select new
                                  {
                                      BlockId = grp.Key.BlockId,
                                      ProductReferenceId = grp.Key.ProductReferenceId,
                                    //  RowId = grp.Key.RowId,
                                      Sort = grp.Key.Sort,
                                      RowValueGUID = grp.Key.RowValueGUID,
                                      ColumnIDAndValueList = grp.Select(r => new { GridColumnId = (int)r[GridColumnConstantName.GridColumnID], ValueText = r[GridColumnConstantName.ValueText] }),
                                  };

            Dictionary<int, Dictionary<int, List<SimpleGridProductRow>>> dictAllBlockRefereRowColumn = new Dictionary<int, Dictionary<int, List<SimpleGridProductRow>>>();

            foreach (int blockId in gridColumnQuery.Select(o => o.BlockId).Distinct())
            {
                Dictionary<int, List<SimpleGridProductRow>> dictOneBlockReferencesRowColumn = new Dictionary<int, List<SimpleGridProductRow>>();

                dictAllBlockRefereRowColumn.Add(blockId, dictOneBlockReferencesRowColumn);

                var oneBlockReferenceList = gridColumnQuery.Where(o => o.BlockId == blockId).ToList();
                var refereceids = oneBlockReferenceList.Select(o => o.ProductReferenceId).Distinct();

                foreach (var refId in refereceids)
                {
                    List<SimpleGridProductRow> listDto = new List<SimpleGridProductRow>();

                    dictOneBlockReferencesRowColumn.Add(refId, listDto);

                    var oneRefereRowList = oneBlockReferenceList.Where(o => o.ProductReferenceId == refId).ToList();

                    foreach (var gridRow in oneRefereRowList)
                    {
                        SimpleGridProductRow aGridProductRowDto = new SimpleGridProductRow();
                        listDto.Add(aGridProductRowDto);
                        aGridProductRowDto.ProductReferenceId = refId;
                      //  aGridProductRowDto.RowId = gridRow.RowId;
                        aGridProductRowDto.Sort = gridRow.Sort;
                        aGridProductRowDto.RowValueGuId = gridRow.RowValueGUID;


                        //!!!
                        foreach (var cellValue in gridRow.ColumnIDAndValueList)
                        {
                            object value = cellValue.ValueText;

                            aGridProductRowDto.Add(cellValue.GridColumnId, value);
                        }




                        // if(aGridProductRowDto.ContainColumnId(
                    }


                    if (listDto.Count > 0)
                    {
                        //List<int> needColumnIds = dictBlockNeedColumnIds[blockId];
                        //var rowColumnIds = listDto[0].DictColumnCellValue.Keys;

                        //var listNotInRowColumns = needColumnIds.Except(rowColumnIds);

                        //foreach (SimpleGridProductRow simpleGridProductRow in listDto)
                        //{
                        //    foreach (int columnId in listNotInRowColumns)
                        //    {

                        //        simpleGridProductRow.Add(columnId, null);


                        //    }
                        //}


                        // need to add miss column value place holder

                        List<int> needColumnIds = dictBlockNeedColumnIds[blockId];


                        foreach (SimpleGridProductRow simpleGridProductRow in listDto)
                        {
                            var rowColumnIds = simpleGridProductRow.DictColumnCellValue.Keys;

                            var listNotInRowColumns = needColumnIds.Except(rowColumnIds);

                            foreach (int columnId in listNotInRowColumns)
                            {

                                simpleGridProductRow.Add(columnId, null);


                            }
                        }

                    }

                   
                }
            }

            return dictAllBlockRefereRowColumn;
        }

        public static DataTable RetrieveDataTableReferenceSimpleDcutValue(IEnumerable<string> referenceIds, List<PdmBlockSubItemClrDto> subItemList, bool isShowLookUpItem = true)
        {
            Dictionary<int, int> dictSuibtemIdControType = new Dictionary<int, int>();

            foreach (var blockSubitem in subItemList)
            {
                dictSuibtemIdControType.Add(blockSubitem.SubItemId, blockSubitem.ControlType);
            }

            Dictionary<int, Dictionary<int, object>> toReturn = new Dictionary<int, Dictionary<int, object>>();

            //if (referenceIds == null || referenceIds.Count() == 0)
            //    return simpleDcuTable;

            var subitemIds = subItemList.Select(o => o.SubItemId).ToList();

            if (subitemIds.Count == 0)
            {
                return new DataTable();
            }

            string referenceInClause = DataAcessHelper.GenerateColumnInClauseWithAndCondition(referenceIds, GridColumnConstantName.ProductReferenceID, true);
            if (referenceIds.Count() == 1 && referenceIds.ElementAt(0) == "-2")
            {
                referenceInClause = string.Empty;
            
            }


            string subitemIdInclause = DataAcessHelper.GenerateColumnInClauseWithAndCondition(subitemIds, GridColumnConstantName.SubItemID, false);

            StringBuilder aQuery = new StringBuilder();

            //   aQuery.Append( string" SELECT ProductReferenceID,BlockID, GridID,  RowID,  RowValueGUID ,Sort,");

            aQuery.Append(" SELECT " +
                GridColumnConstantName.ProductReferenceID + ", " +
                GridColumnConstantName.SubItemID + ", " +
                GridColumnConstantName.ValueText
           );

            string queryString = aQuery.ToString() + " from pdmSearchSimpleDcuValue where " + subitemIdInclause + referenceInClause;

           

            DataTable dtSimpleValue;

            var allSubitemeEntityIds = subItemList.Where(o => o.EntityId.HasValue).Select(o => o.EntityId.Value).ToList();
            Dictionary<int, Dictionary<object, string>> dictKeyEntityDictKeyIdDisplayString = PLMSEntityClrBL.GetDictEntityDictDisplayString(allSubitemeEntityIds);




            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();
                DataAcessHelper.ExecuteReadUnCommmited(conn);

                dtSimpleValue = DataAcessHelper.GetDataTableQueryResult(conn, queryString);
               // dictKeyEntityDictKeyIdDisplayString = PLMSEntityClrBL.GetDictEntityDictDisplayString(allSubitemeEntityIds, conn);

                DataAcessHelper.ExecuteReadCommmited(conn);
            }

            var subItemQuery = from row in dtSimpleValue.AsDataRowEnumerable()
                               group row by new
                               {
                                   ProductReferenceID = (int)row[GridColumnConstantName.ProductReferenceID]
                               } into grp
                               orderby grp.Key.ProductReferenceID
                               select new
                               {
                                   Key = grp.Key.ProductReferenceID,

                                   SubitemIDAndValueList = grp.Select(r => new { id = (int)r[GridColumnConstantName.SubItemID], value = r[GridColumnConstantName.ValueText] }),
                               };

            DataTable roReturnDcuTable = CreateDataTableStrcutureFromBlockSubitem(subItemList);
            foreach (var o in subItemQuery)
            {
                //Dictionary<int, object> dictSubitemValue = new Dictionary<int, object>();

                DataRow dataRow = roReturnDcuTable.NewRow();
                dataRow[GridColumnConstantName.ProductReferenceID] = o.Key;
                foreach (var subitem in o.SubitemIDAndValueList)
                {
                    object value = ControlTypeValueConverter.ConvertValueToObject(subitem.value, dictSuibtemIdControType[subitem.id]);

                   
                    if (value == null)
                    {
                        dataRow[subitem.id.ToString()] = DBNull.Value;
                    }
                    else
                    {
                      //  CLROutput.Output("convervalue=" + value);
                        dataRow[subitem.id.ToString()] = value;

                      //  CLROutput.Output("dataRow[subitem.id.ToString()]" + dataRow[subitem.id.ToString()]);
                    }
                }

                roReturnDcuTable.Rows.Add(dataRow);
            }

            if (isShowLookUpItem)
            {
                List<PdmBlockSubItemClrDto> dDLSubitemDtoList = subItemList.Where(o => o.ControlType == (int)EmControlType.DDL && o.EntityId.HasValue).ToList();

                foreach (var dictDDLSubitemDto in dDLSubitemDtoList)
                {
                    int entityId = dictDDLSubitemDto.EntityId.Value;
                    string stringSubItemId = dictDDLSubitemDto.SubItemId.ToString();
                    if (dictKeyEntityDictKeyIdDisplayString.ContainsKey(entityId))
                    {
                        Dictionary<object, string> lookUpItem = dictKeyEntityDictKeyIdDisplayString[entityId];
                        foreach (DataRow row in roReturnDcuTable.Rows)
                        {
                            int? lookupid = ControlTypeValueConverter.ConvertValueToInt(row[stringSubItemId]);
                            if (lookupid.HasValue)
                            {
                                int idvalue = lookupid.Value;
                                if (lookUpItem.ContainsKey(idvalue))
                                {
                                    row[stringSubItemId] = lookUpItem[idvalue]; ;

                                    CLROutput.OutputDebug("entityID_idvalue+dislayvalue="+entityId.ToString ()+"_"+idvalue.ToString ()+"_" + "_" + lookUpItem[idvalue]);
                                }
                            }
                        }
                    }
                }
            
            }
           

            return roReturnDcuTable;
        }        // key1: blockId, key2:referecne, value

        public static Dictionary<int, Dictionary<int, object>> RetrieveReferenceSimpleDcutValue(IEnumerable<int> referenceIds, List<PdmBlockSubItemClrDto> subItemList)
        {
            // subitemIds = subitemIds.Distinct().ToArray ();

            if (referenceIds.Count() == 0 || subItemList.Count == 0)
            {
                return new Dictionary<int, Dictionary<int, object>>();
 
            }

            Dictionary<int, int> dictSuibtemIdControType = new Dictionary<int, int>();

            foreach (var blockSubitem in subItemList)
            {
                dictSuibtemIdControType.Add(blockSubitem.SubItemId, blockSubitem.ControlType);
            }

            Dictionary<int, Dictionary<int, object>> toReturn = new Dictionary<int, Dictionary<int, object>>();

            if (referenceIds == null || referenceIds.Count() == 0)
                return toReturn;

            var subitemIds = subItemList.Select(o => o.SubItemId).ToList();
            var referenceInClause = DataAcessHelper.GenerateColumnInClauseWithAndCondition(referenceIds, GridColumnConstantName.ProductReferenceID, false);
            var subitemIdInclause = DataAcessHelper.GenerateColumnInClauseWithAndCondition(subitemIds, GridColumnConstantName.SubItemID, true);

            StringBuilder aQuery = new StringBuilder();

            //   aQuery.Append( string" SELECT ProductReferenceID,BlockID, GridID,  RowID,  RowValueGUID ,Sort,");

            aQuery.Append(" SELECT " +
                GridColumnConstantName.ProductReferenceID + ", " +
                GridColumnConstantName.SubItemID + ", " +
                GridColumnConstantName.ValueText
           );

            string queryString = aQuery.ToString() + " from pdmSearchSimpleDcuValue where " + referenceInClause + subitemIdInclause;

            CLROutput.OutputDebug("from pdmSearchSimpleDcuValu" + queryString);

            DataTable dtSimpleValue;
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();
                DataAcessHelper.ExecuteReadUnCommmited(conn);

                dtSimpleValue = DataAcessHelper.GetDataTableQueryResult(conn, queryString);

                DataAcessHelper.ExecuteReadCommmited(conn);
            }

            var subItemQuery = from row in dtSimpleValue.AsDataRowEnumerable()
                               group row by new
                               {
                                   ProductReferenceID = (int)row[GridColumnConstantName.ProductReferenceID]
                               } into grp
                               orderby grp.Key.ProductReferenceID
                               select new
                               {
                                   Key = grp.Key.ProductReferenceID,

                                   SubitemIDAndValueList = grp.Select(r => new { id = (int)r[GridColumnConstantName.SubItemID], value = r[GridColumnConstantName.ValueText] }),
                               };

            foreach (var o in subItemQuery)
            {
                Dictionary<int, object> dictSubitemValue = new Dictionary<int, object>();

                foreach (var subitem in o.SubitemIDAndValueList)
                {
                    var value = ControlTypeValueConverter.ConvertValueToObject(subitem.value, dictSuibtemIdControType[subitem.id]);
                    dictSubitemValue.Add(subitem.id, value);
                }

                var notInDict = subitemIds.Except(dictSubitemValue.Keys);
                foreach (var subitId in notInDict)
                {
                    dictSubitemValue.Add(subitId, null);
                }

                toReturn.Add(o.Key, dictSubitemValue);
            }

            return toReturn;
        }        // key1: blockId, key2:referecne, value

        private static DataTable CreateDataTableStrcutureFromBlockSubitem(List<PdmBlockSubItemClrDto> listSubitems)
        {
            DataTable table = new DataTable("SubitemTable");

            DataColumn aRefDataColumn = new DataColumn(GridColumnConstantName.ProductReferenceID, typeof(int));
            table.Columns.Add(aRefDataColumn);


            foreach (PdmBlockSubItemClrDto aSubitem in listSubitems)
            {
                DataColumn aDataColumn;

                string aSubItemId = aSubitem.SubItemId.ToString();
                if (aSubitem.ControlType == (int)EmControlType.DDL)
                {
                    aDataColumn = new DataColumn(aSubItemId, typeof(string));
                }
                else
                {
                    aDataColumn = new DataColumn(aSubItemId, ControlTypeValueConverter.GetDataTypeByControlType(aSubitem.ControlType));
                }

                table.Columns.Add(aDataColumn);
            }

            return table;
        }

        internal static DataTable CreateDataTableStrcutureFromGridColumn(List<PdmGridMetaColumnClrDto> listSubitems)
        {
            DataTable table = new DataTable("GridTable");

            DataColumn aRefDataColumn = new DataColumn(GridColumnConstantName.ProductReferenceID, typeof(int));
            table.Columns.Add(aRefDataColumn);

            //aRefDataColumn = new DataColumn(GridColumnConstantName.RowID, typeof(int));
            //table.Columns.Add(aRefDataColumn);


            aRefDataColumn = new DataColumn(GridColumnConstantName.RowValueGUID, typeof(Guid));
            table.Columns.Add(aRefDataColumn);

            aRefDataColumn = new DataColumn(GridColumnConstantName.Sort, typeof(int));
            table.Columns.Add(aRefDataColumn);

            foreach (PdmGridMetaColumnClrDto aSubitem in listSubitems)
            {
                DataColumn aDataColumn;

                string aSubItemId = aSubitem.GridColumnId.ToString();
                if (aSubitem.ColumnTypeId == (int)EmControlType.DDL)
                {
                    aDataColumn = new DataColumn(aSubItemId, typeof(string));
                }
                else
                {
                    aDataColumn = new DataColumn(aSubItemId, ControlTypeValueConverter.GetDataTypeByControlType(aSubitem.ColumnTypeId));
                }

                table.Columns.Add(aDataColumn);
            }

            return table;
        }
 
        private static DataTable GetMutipleBlockGridCellValueTable(int[] referenceIds, int[] blockIds, int[] columnIds)
        {
            var andblockIdclause = DataAcessHelper.GenerateColumnInClauseWithAndCondition(blockIds, GridColumnConstantName.BlockID, false); ;

            string referenceInClause = DataAcessHelper.GenerateColumnInClauseWithAndCondition(referenceIds, GridColumnConstantName.ProductReferenceID, true);

            // string referenceInClause = string.Empty;

            var columnIdInclause = DataAcessHelper.GenerateColumnInClauseWithAndCondition(columnIds, GridColumnConstantName.GridColumnID, true);

            StringBuilder aQuery = new StringBuilder();

            //   aQuery.Append( string" SELECT ProductReferenceID,BlockID, GridID,  RowID,  RowValueGUID ,Sort,");

            aQuery.Append(" SELECT " +
           GridColumnConstantName.ProductReferenceID + ", " +
            GridColumnConstantName.BlockID + ", " +
         //  GridColumnConstantName.RowID + ", " +
           GridColumnConstantName.Sort + ", " +
           GridColumnConstantName.GridColumnID + ", " +
            GridColumnConstantName.RowValueGUID + ", " +
           GridColumnConstantName.ValueText
           );

            aQuery.Append(" from pdmSearchComplexColumnValue where " + andblockIdclause + referenceInClause + columnIdInclause);

            CLROutput.OutputDebug(aQuery.ToString());

            string queryCell = aQuery.ToString();

            CLROutput.OutputDebug("GetMutipleBlockGridCellValueTable=" + queryCell);

            // test 4 Grid for all cell values forthe perforamce
            // queryCell = @" SELECT ProductReferenceID, BlockID, RowID, Sort, GridColumnID, ValueText from pdmSearchComplexColumnValue where    BlockID in ( 20, 14 ,64,65) ";

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();
                DataAcessHelper.ExecuteReadUnCommmited(conn);

                var dtReferenceGridValue = DataAcessHelper.GetDataTableQueryResult(conn, queryCell);

                DataAcessHelper.ExecuteReadCommmited(conn);

                return dtReferenceGridValue;

               //CLROutput.SendDataTable(dtReferenceGridValue);
            }
        }

       
    }
}


 //private static DataTable RetriveGridPivotDataTable(List<PdmGridMetaColumnClrDto> columnList, DataTable dtReferenceGridValue)
 //       {
 //           Dictionary<int, int> dictColumnIdControType = new Dictionary<int, int>();

 //           foreach (var column in columnList)
 //           {
 //               dictColumnIdControType.Add(column.GridColumnId, column.ColumnTypeId);
 //           }

 //           var gridColumnQuery = from row in dtReferenceGridValue.AsDataRowEnumerable()
 //                                 group row by new
 //                                 {
 //                                     ProductReferenceId = (int)row[GridColumnConstantName.ProductReferenceID],
 //                                     RowValueGUID = (int)row[GridColumnConstantName.RowValueGUID],
 //                                     Sort = (int)row[GridColumnConstantName.Sort],
 //                                 } into grp

 //                                 select new
 //                                 {
 //                                     ProductReferenceId = grp.Key.ProductReferenceId,
 //                                     RowValueGUID = grp.Key.RowValueGUID,
 //                                     Sort = grp.Key.Sort,

 //                                     ColumnIDAndValueList = grp.Select(r => new { GridColumnId = (int)r[GridColumnConstantName.GridColumnID], ValueText = r[GridColumnConstantName.ValueText] }),
 //                                 };

 //           // Dictionary<int, List<SimpleGridProductRow>> dictBlockIdReferenceIdSimpleGridProductList = new Dictionary<int, List<SimpleGridProductRow>>();

 //           //    DataTable toReturn = CreateDataTableStrcutureFromGridColumn(columnList);
 //           DataTable roReturnDcuTable = CreateDataTableStrcutureFromGridColumn(columnList);
 //           foreach (var o in gridColumnQuery)
 //           {
 //               //Dictionary<int, object> dictSubitemValue = new Dictionary<int, object>();

 //               DataRow dataRow = roReturnDcuTable.NewRow();
 //               dataRow[GridColumnConstantName.ProductReferenceID] = o.ProductReferenceId;
 //               dataRow[GridColumnConstantName.RowValueGUID] = o.RowValueGUID;
 //               dataRow[GridColumnConstantName.Sort] = o.Sort;

 //               foreach (var column in o.ColumnIDAndValueList)
 //               {
 //                   int columnId = column.GridColumnId;

 //                   // object value = column.Value;

 //                   object value = ControlTypeValueConverter.ConvertValueToObject(column.ValueText, dictColumnIdControType[columnId]);
 //                   if (value == null)
 //                   {
 //                       dataRow[columnId.ToString()] = DBNull.Value;
 //                   }
 //                   else
 //                   {
 //                       dataRow[columnId.ToString()] = value;
 //                   }
 //               }

 //               roReturnDcuTable.Rows.Add(dataRow);
 //           }
 //           return roReturnDcuTable;
 //       }


//public static DataTable RetrieveOneBlockGridColumnsRowDatable(string [] referenceIds, int blockId, List<PdmGridMetaColumnClrDto> listGridMetaColumnClrDto)
//{
//    int[] columnIds = listGridMetaColumnClrDto.Select(o => o.GridColumnId).ToArray();

//    DataTable gridCellValueDataTable = GetGridCellValueTable(referenceIds, blockId, columnIds);

//    DataTable pivotDataTable = RetriveGridPivotDataTable(listGridMetaColumnClrDto, gridCellValueDataTable);

//    return pivotDataTable;
//}

//private static DataTable GetGridCellValueTable(string [] referenceIds, int blockIds, int[] columnIds)
//{
//    var andblockIdclause = " " + GridColumnConstantName.BlockID + "=" + blockIds;

//    string referenceInClause = DataAcessHelper.GenerateColumnInClauseWithAndCondition(referenceIds, GridColumnConstantName.ProductReferenceID, true);
//    if (referenceIds.Length == 1 && referenceIds[0] == "-2")
//    {
//        referenceInClause = string.Empty;
//    }

//    // for perforance testing
//   //  string referenceInClause = string.Empty;

//    var columnIdInclause = DataAcessHelper.GenerateColumnInClauseWithAndCondition(columnIds, GridColumnConstantName.GridColumnID, true);

//    StringBuilder aQuery = new StringBuilder();

//    //   aQuery.Append( string" SELECT ProductReferenceID,BlockID, GridID,  RowID,  RowValueGUID ,Sort,");

//    aQuery.Append(" SELECT " +
//   GridColumnConstantName.ProductReferenceID + ", " +
//   GridColumnConstantName.RowID + ", " +
//   GridColumnConstantName.Sort + ", " +
//   GridColumnConstantName.GridColumnID + ", " +
//   GridColumnConstantName.ValueText
//   );

//    aQuery.Append(" from pdmSearchComplexColumnValue where " + andblockIdclause + columnIdInclause + referenceInClause);

//    using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
//    {
//        conn.Open();
//        DataAcessHelper.ExecuteReadUnCommmited(conn);

//        DataTable dtReferenceGridValue = DataAcessHelper.GetDataTableQueryResult(conn, aQuery.ToString());

//        DataAcessHelper.ExecuteReadCommmited(conn);
//        return dtReferenceGridValue;
//    }
//}

//Key1: block   key:Block Reference,  List<SimpleGridProductRow>>>
