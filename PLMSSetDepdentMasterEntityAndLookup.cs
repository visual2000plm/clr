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
    public class PLMSSetDepdentMasterEntityAndLookup
    {

        internal static void SetDepdentMasterEntityColumn(PdmGridClrDto aPdmGridEntity, DataTable toReturnDcuTable)
        {


            //var dictSysDefineMasterEntityColumnIdEntityIds = aPdmGridEntity.MasterEntityColumn.Where(col => col.EntityId.HasValue && col.EntityId.Value < 3000).ToDictionary(col => col.GridColumnId, col => col.EntityId.Value);

            Dictionary<int, int> dictUserDefineMasterEntityColumnIdEntityIds = new Dictionary<int, int>();

            foreach (var column in aPdmGridEntity.MasterEntityColumn)
            {

                if (column.EntityId.HasValue)
                {
                    int entityId = column.EntityId.Value;
                    int masterColumnId = column.GridColumnId;

                    PdmEntityBlClrDto aPdmEntityBlClrDto = PdmCacheManager.DictPdmEntityBlEntity[entityId];
                    if (aPdmEntityBlClrDto.EntityType ==(int) EmEntityType.SystemDefineTable)
                    {
                      
                       // int entityId = columnIdEntity.Value;

                        List<string> columnNames = new List<string>();
                        if (aPdmGridEntity.MasterEntityDepdentColumn.ContainsKey(masterColumnId))
                        {
                            columnNames = aPdmGridEntity.MasterEntityDepdentColumn[masterColumnId].Where(o => !string.IsNullOrEmpty(o.InternalCode)).Select(o => o.InternalCode).ToList();
                            DataTable aDtResult = PLMSEntityClrBL.GetMutipleSysDefineEntityRowValue(entityId, columnNames);
                            ProcessSystemDefineEntityColumn(aPdmGridEntity, toReturnDcuTable, aDtResult, masterColumnId);
                        }

                    }
                    else if (aPdmEntityBlClrDto.EntityType == (int)EmEntityType.UserDefineTable)
                    {

                        dictUserDefineMasterEntityColumnIdEntityIds.Add(masterColumnId, entityId);

                    }
                
                
                }
            
            }

            List<int> userDefineColumnIds = new List<int>();
            foreach (var columnIdEntity in dictUserDefineMasterEntityColumnIdEntityIds)
            {
                int masterColumnId = columnIdEntity.Key;
                int entityId = columnIdEntity.Value;

                var pdmUserDefineEntity = PdmCacheManager.DictPdmEntityBlEntity[entityId];

                if (aPdmGridEntity.MasterEntityDepdentColumn.ContainsKey(masterColumnId))
                {
                    // if (aDtcolumn.UserDefineEntityColumnId.ToString() == dependentEntityColumn.InternalCode)
                    var columnids = aPdmGridEntity.MasterEntityDepdentColumn[masterColumnId]
                        .Where(o => !string.IsNullOrEmpty(o.InternalCode))
                        .Select(o => int.Parse(o.InternalCode.Trim()));

                    userDefineColumnIds.AddRange(columnids);
                }
            }
            if (userDefineColumnIds.Count > 0)
            {
                // Key: EntityId, vlaue
                Dictionary<int, List<SimpleUserDefineEntityRow>> result = PLMSEntityClrBL.GetDictEntityUserDefineRows(dictUserDefineMasterEntityColumnIdEntityIds.Values.ToList(), userDefineColumnIds);

                foreach (var columnIdEntity in dictUserDefineMasterEntityColumnIdEntityIds)
                {
                    int masterColumnId = columnIdEntity.Key;
                    int entityId = columnIdEntity.Value;
                    if (result.ContainsKey(entityId))
                    {
                        List<SimpleUserDefineEntityRow> listRow = result[entityId];

                        CLROutput.OutputDebug("masterColumnId =" + masterColumnId);

                        ProcessUserDefineEntityColumn(aPdmGridEntity, toReturnDcuTable, listRow, masterColumnId);
                    }
                }
            }
        }
        private static void ProcessUserDefineEntityColumn(PdmGridClrDto aPdmGridEntity, DataTable toReturnDcuTable, List<SimpleUserDefineEntityRow> listRow, int masterColumnId)
        {
            Dictionary<int, SimpleUserDefineEntityRow> dictRow = listRow.ToDictionary(o => o.RowId, o => o);
            foreach (DataRow dataRow in toReturnDcuTable.Rows)
            {
                string valueId = dataRow[masterColumnId.ToString()] as string;

                CLROutput.OutputDebug("masterColumnId value" + valueId);
                if (!string.IsNullOrEmpty(valueId))
                {
                    // int.Parse (
                    int intValueId = int.Parse(valueId);
                    if (dictRow.ContainsKey(intValueId))
                    {
                        var dtRow = dictRow[intValueId];
                        foreach (var dependentEntityColumn in aPdmGridEntity.MasterEntityDepdentColumn[masterColumnId])
                        {
                            int userDefineColumnId = int.Parse(dependentEntityColumn.InternalCode);

                            //  CLROutput.Output("userDefineColumnId" + userDefineColumnId);

                            object value = ControlTypeValueConverter.ConvertValueToObject(dtRow[userDefineColumnId], dependentEntityColumn.ColumnTypeId);
                            if (value != null)
                            {
                                dataRow[dependentEntityColumn.GridColumnId.ToString()] = value;
                            }
                        }
                    }
                }
            }


        }
        private static void ProcessSystemDefineEntityColumn(PdmGridClrDto aPdmGridEntity, DataTable toReturnDcuTable, DataTable aDtResult, int gridColumnId)
        {
            var dictRowId = aDtResult.AsDataRowEnumerable().ToDictionary(dtRow => (int)dtRow["Id"], dtRow => dtRow);

            foreach (DataRow dataRow in toReturnDcuTable.Rows)
            {
                string valueId = dataRow[gridColumnId.ToString()] as string;
                if (!string.IsNullOrEmpty(valueId))
                {
                    // int.Parse (
                    int intValueId = int.Parse(valueId);
                    if (dictRowId.ContainsKey(intValueId))
                    {
                        var dtRow = dictRowId[intValueId];
                        foreach (var dependentEntityColumn in aPdmGridEntity.MasterEntityDepdentColumn[gridColumnId])
                        {
                            foreach (DataColumn aDtcolumn in aDtResult.Columns)
                            {
                                if (aDtcolumn.ColumnName == dependentEntityColumn.InternalCode)
                                {
                                    // cannot pass DataTable column DataType to the silverlight, SL doesn't support Sql.client type data

                                    object value = ControlTypeValueConverter.ConvertValueToObject(dtRow[aDtcolumn], dependentEntityColumn.ColumnTypeId);
                                    if (value != null)
                                    {
                                        dataRow[dependentEntityColumn.GridColumnId.ToString()] = value;
                                    }

                                    //else
                                    //{}
                                }
                            }
                        }
                    }
                }
            }
        }
        internal static void SetupDataTableDDLLookupValue(List<PdmGridMetaColumnClrDto> columnList, DataTable roReturnDcuTable)
        {
            var allSubitemeEntityIds = columnList.Where(o => o.EntityId.HasValue).Select(o => o.EntityId.Value).ToList();

            Dictionary<int, Dictionary<object, string>> dictKeyEntityDictKeyIdDisplayString = PLMSEntityClrBL.GetDictEntityDictDisplayString(allSubitemeEntityIds);
          

            var dDLColumnDtoList = columnList.Where(o => o.ColumnTypeId == (int)EmControlType.DDL).ToList();
            foreach (var aDdlColumnDto in dDLColumnDtoList)
            {
                int columnId = aDdlColumnDto.GridColumnId;
                if (aDdlColumnDto.ColumnTypeId == (int)EmControlType.DDL && aDdlColumnDto.EntityId.HasValue)
                {
                    int entityId = aDdlColumnDto.EntityId.Value;

                    if (dictKeyEntityDictKeyIdDisplayString.ContainsKey(entityId))
                    {
                        Dictionary<object, string> lookUpItem = dictKeyEntityDictKeyIdDisplayString[entityId];
                        foreach (DataRow row in roReturnDcuTable.Rows)
                        {
                            string stringColumnId = columnId.ToString();

                            //  CLROutput.Output("stringColumnId=" + stringColumnId);
                            SqlInt32 lookupid = Converter.ToDDLSqlInt32(row[stringColumnId]);
                            if (!lookupid.IsNull)
                            {
                                int idvalue = lookupid.Value;
                                if (lookUpItem.ContainsKey(idvalue))
                                {
                                    row[stringColumnId] = lookUpItem[idvalue]; ;
                                }
                            }
                        }
                    }
                }
            }
        }
        //private static void SetTabLookupItem(List<BlockSubitemClrUserDefineDto> listSubitems, SqlConnection conn, System.Data.DataTable tabFieldResultDataTable)
        //{
        //    List<BlockSubitemClrUserDefineDto> dDLSubitemDtoList = listSubitems.Where(o => o.ControlType == (int)EmControlType.DDL && o.EntityID.HasValue).ToList();

        //    var allSubitemeEntityIds = dDLSubitemDtoList.Select(o => o.EntityID.Value).ToList();

        //    Dictionary<int, Dictionary<int, string>> dictKeyEntityDictKeyIdDisplayString = PLMSEntityClrBL.GetDictEntityDictDisplayString(allSubitemeEntityIds, conn);

        //    foreach (var dictDDLSubitemDto in dDLSubitemDtoList)
        //    {
        //        string stringSubItemId = dictDDLSubitemDto.SubItemID.ToString();
        //        if (dictDDLSubitemDto.ControlType == (int)EmControlType.DDL && dictDDLSubitemDto.EntityID.HasValue)
        //        {
        //            int entityId = dictDDLSubitemDto.EntityID.Value;

        //            if (dictKeyEntityDictKeyIdDisplayString.ContainsKey(entityId))
        //            {
        //                Dictionary<int, string> lookUpItem = dictKeyEntityDictKeyIdDisplayString[entityId];
        //                foreach (DataRow row in tabFieldResultDataTable.Rows)
        //                {
        //                    SqlInt32 lookupid = Converter.ToDDLSqlInt32(row[stringSubItemId]);
        //                    if (!lookupid.IsNull)
        //                    {
        //                        int idvalue = lookupid.Value;
        //                        if (lookUpItem.ContainsKey(idvalue))
        //                        {
        //                            row[stringSubItemId] = lookUpItem[idvalue]; ;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

    }
}