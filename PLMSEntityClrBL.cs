using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace PLMCLRTools
{
    public static class PLMSEntityClrBL
    {
        public static readonly string PdmUserDefineEntityRowCellValueView = "pdmUserDefineEntityRowCellValue";

        private static readonly string pdmUserDefineMutipleColumnEntityRowValue = @"SELECT     dbo.pdmUserDefineEntityRow.EntityID, dbo.pdmUserDefineEntityRow.RowID, dbo.pdmUserDefineEntityRow.SortOrder,

                      dbo.pdmUserDefineEntityRowValue.UserDefineEntityColumnID, CASE WHEN pdmUserDefineEntityRowValue.ValueID IS NOT NULL
                      THEN CAST(pdmUserDefineEntityRowValue.ValueID AS varchar(10)) WHEN pdmUserDefineEntityRowValue.ValueDate IS NOT NULL
                      THEN CAST(pdmUserDefineEntityRowValue.ValueDate AS varchar) ELSE pdmUserDefineEntityRowValue.ValueText END AS ValueText
                        FROM         dbo.pdmUserDefineEntityRow INNER JOIN
                      dbo.pdmUserDefineEntityRowValue ON dbo.pdmUserDefineEntityRow.RowID = dbo.pdmUserDefineEntityRowValue.RowID";

        
        public static List<PdmClrEntitySimpleStructureDto> GetEntityAndColumnStrcutureInfoList(List<int> entityIDs)
        {
            List<PdmClrEntitySimpleStructureDto> listPdmEntityDto = new List<PdmClrEntitySimpleStructureDto>();

            if (entityIDs != null && entityIDs.Count > 0)
            {
                string entityQuery = "select distinct EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity ";
                string entityIdInClause = DataAcessHelper.GenerateColumnInClauseWithAndCondition(entityIDs, "EntityID", false);

                entityQuery = entityQuery + "   where " + entityIdInClause;

                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                {
                    conn.Open();

                    DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, entityQuery);

                    foreach (DataRow row in entityDataTable.Rows)
                    {
                        listPdmEntityDto.Add(PLMDataRowConverter.ConvertRowToPdmEntityDto(row));
                    }

                    string queryColumn = @" select UserDefineEntityColumnID, EntityId, ColumnName,DataType,UsedByDropDownList,DataRowSort,IsPrimaryKey,IsIdentity,SystemTableColumnName,UicontrolType,Nbdecimal,FkentityId

                                          from PdmUserDefineEntityColumn " + "   where " + entityIdInClause;

                    DataTable columnDataTable = DataAcessHelper.GetDataTableQueryResult(conn, queryColumn);

                    List<PdmEntityColumnClrUserDefineDto> allColumnDto = new List<PdmEntityColumnClrUserDefineDto>();

                    foreach (DataRow row in columnDataTable.Rows)
                    {
                        allColumnDto.Add(PLMDataRowConverter.ConvertUserDefineEntityColumnDataRowToPdmEntityColumnDto(row));
                    }
                    foreach (var entityDto in listPdmEntityDto)
                    {
                        entityDto.Columns = allColumnDto.Where(o => o.EntityId == entityDto.EntityId).ToList();
                    }

                 
                }
     
            }

            return listPdmEntityDto;
        }

      
        public static Dictionary<int, List<SimpleUserDefineEntityRow>> GetDictEntityUserDefineRows(List<int> entityIDs,   List<int> ddlUserDefineEntityColumnIDs )
        {



            Dictionary<int, List<SimpleUserDefineEntityRow>> toReturn = new Dictionary<int, List<SimpleUserDefineEntityRow>>();

            entityIDs = entityIDs.Distinct().ToList();

        

            string queryRowValue = pdmUserDefineMutipleColumnEntityRowValue;

            if (entityIDs != null && entityIDs.Count > 0)
            {
                //AND (EntityID IN (3031, 3095))
                queryRowValue = queryRowValue + "   where " + DataAcessHelper.GenerateColumnInClauseWithAndCondition(entityIDs, "EntityID", false);
            }

            if (ddlUserDefineEntityColumnIDs != null && ddlUserDefineEntityColumnIDs.Count > 0)
            {
                //and  UserDefineEntityColumnID in                       ( 442,444)
                queryRowValue = queryRowValue + DataAcessHelper.GenerateColumnInClauseWithAndCondition(ddlUserDefineEntityColumnIDs, "UserDefineEntityColumnID", true);
            }

            DataTable RowValuDatatable;
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
              conn.Open();
              RowValuDatatable = DataAcessHelper.GetDataTableQueryResult(conn, queryRowValue);
            }

            CLROutput.SendDebugDataTable(RowValuDatatable);
          

            //  DataSetUtilities.SendDataTable(RowValuDatatable);

            // Entity level group
            var groupByEntityIdQuery = from row in RowValuDatatable.AsDataRowEnumerable()
                                       group row by new
                                       {
                                           EntityID = (int)row["EntityID"]
                                       } into grp

                                       select new
                                       {
                                           EntityID = grp.Key.EntityID,
                                           RowIDAndValueList = grp.Select(r => new
                                           {
                                               RowID = (int)r["RowID"],
                                               SortOrder = (r["SortOrder"] as int?).HasValue ? (r["SortOrder"] as int?).Value : 0,
                                               UserDefineEntityColumnID = (int)r["UserDefineEntityColumnID"],
                                               ValueText = r["ValueText"] as string
                                           }).ToList(),
                                       };

            foreach (var o in groupByEntityIdQuery)
            {
                int entityId = o.EntityID;
                var RowIDAndValueList = o.RowIDAndValueList;

                var simpleUserDefineEntityRowList = from row in RowIDAndValueList
                                                    group row by new
                                                    {
                                                        RowID = row.RowID,
                                                        SortOrder = row.SortOrder
                                                    } into grpRow

                                                    select new
                                                    {
                                                        RowId = grpRow.Key.RowID,
                                                        SortOrder = grpRow.Key.SortOrder,
                                                        RowCellValueList = grpRow.Select
                                                        (
                                                            row => new
                                                            {
                                                                ColumnId = row.UserDefineEntityColumnID,
                                                                ValueText = row.ValueText
                                                            }

                                                        )
                                                    };

                List<SimpleUserDefineEntityRow> listDto = new List<SimpleUserDefineEntityRow>();
                foreach (var queryItem in simpleUserDefineEntityRowList)
                {
                    SimpleUserDefineEntityRow aSimpleUserDefineEntityRow = new SimpleUserDefineEntityRow();
                    aSimpleUserDefineEntityRow.RowId = queryItem.RowId;
                    aSimpleUserDefineEntityRow.SortOrder = queryItem.SortOrder;
                    foreach (var column in queryItem.RowCellValueList)
                    {
                       int columnId = column.ColumnId;
                        //int controlType = dictColumnIdControType[columnId];
                        //object value= ControlTypeValueConverter.ConvertValueToObject(column.ValueText, controlType);
                       aSimpleUserDefineEntityRow.Add(columnId, column.ValueText);
                    }

                    listDto.Add(aSimpleUserDefineEntityRow);
                }

                toReturn.Add(o.EntityID, listDto);
            }

            return toReturn;
        }




        public static Dictionary<int, Dictionary<object, string>> GetDictEntityDictDisplayString(List<int> entityIDs)
        {

            return PdmCacheEntityLookupItem.GetDictEntityDictDisplayInforFromCache(entityIDs);
      
        }

    
        
        //TODO List !!!! CLR link to ERP data source direcly !

        public static Dictionary<int, List<LookupItemDto>> GetUserDefineEntityDisplayInfoList(List<PdmEntityBlClrDto> userDefineEntityList, SqlConnection plmConnection)
        {
            Dictionary<int, List<LookupItemDto>> toReturn = new Dictionary<int, List<LookupItemDto>>();

            var entityIDs = userDefineEntityList.Select(o => o.EntityId).Distinct().ToList();

            List<int> ddlUserDefineEntityColumnIDs = new List<int>();

            foreach (var enityDto in userDefineEntityList)
            {
                ddlUserDefineEntityColumnIDs.AddRange
                    (
                    enityDto.PdmUserDefineEntityColumnList.Where(o => o.UsedByDropDownList.HasValue && o.UsedByDropDownList.Value).Select(o => o.UserDefineEntityColumnId).ToList()
                    );
            }

            string queryRowValue = pdmUserDefineMutipleColumnEntityRowValue;

            if (entityIDs != null && entityIDs.Count > 0)
            {
                //AND (EntityID IN (3031, 3095))
                queryRowValue = queryRowValue + "   where " + DataAcessHelper.GenerateColumnInClauseWithAndCondition(entityIDs, "EntityID", false);
            }

            if (ddlUserDefineEntityColumnIDs != null && ddlUserDefineEntityColumnIDs.Count > 0)
            {
                //and  UserDefineEntityColumnID in                       ( 442,444)
                queryRowValue = queryRowValue + DataAcessHelper.GenerateColumnInClauseWithAndCondition(ddlUserDefineEntityColumnIDs, "UserDefineEntityColumnID", true);
            }

            //   SqlContext.Pipe.Send("queryRowValue=" + queryRowValue);

            DataTable RowValuDatatable = DataAcessHelper.GetDataTableQueryResult(plmConnection, queryRowValue);

         

         

            var subItemQuery = from row in RowValuDatatable.AsDataRowEnumerable()
                               group row by new
                               {
                                   EntityID = (int)row["EntityID"]
                               } into grp

                               select new
                               {
                                   EntityID = grp.Key.EntityID,
                                   RowIDAndValueList = grp.Select(r => new
                                   {
                                       RowID = (int)r["RowID"],
                                       ValueText = r["ValueText"] as string
                                   }).ToList(),
                               };

            foreach (var o in subItemQuery)
            {
                int entityId = o.EntityID;
                var RowIDAndValueList = o.RowIDAndValueList;

                var lookupItemDtoList = from row in RowIDAndValueList
                                        group row by new
                                        {
                                            RowID = row.RowID
                                        } into grp

                                        select new LookupItemDto()
                                        {
                                            Id = grp.Key.RowID,
                                            Display = grp.Select(r => r.ValueText as string).ToList().Aggregate((current, next) => current + "|" + next)
                                        };

                List<LookupItemDto> listDto = new List<LookupItemDto>();
                foreach (var itme in lookupItemDtoList)
                {
                    listDto.Add(itme);
                }

                toReturn.Add(o.EntityID, listDto);
            }

            return toReturn;
        }

        private static Dictionary<int, List<LookupItemDto>> GetDictEntityLookItemDto(List<int> entityIDs)
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

                if (pdmEntityBlClrDto.EntityType == (int)EmEntityType.SystemDefineTable)
                {

                    string connectInfo = GetConnectionInfoWithCode(pdmEntityBlClrDto.DataSourceFrom);

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

                    GetSysDefineDictLookItemNew(conn, dictEntityDisplayInfoList, listEntityDto);
                
                }
            
            
            }

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString ))
            {
                conn.Open();
                GetEnumDictLookItemNew(conn, dictEntityDisplayInfoList, enumEntityList);
                GetUserDefineDictLookItem(conn, dictEntityDisplayInfoList, userDefineEntityList);

            }

            return dictEntityDisplayInfoList;
        }

        internal static void GetUserDefineDictLookItem(SqlConnection conn, Dictionary<int, List<LookupItemDto>> dictEntityDisplayInfoList, List<PdmEntityBlClrDto> listPdmEntityDto)
        {
            List<PdmEntityBlClrDto> userDefineEntityList = listPdmEntityDto.Where(o => o.EntityType ==(int)EmEntityType.UserDefineTable ).ToList();

            //Important, need to setyp last sca ltime
            string lastScaltime = PdmCacheEntityLookupItem.GetLastChangeCheckSum(conn, PdmCacheManager.UserDefineEntityTableName);
            PdmCacheManager.LastScanUserDefinePdmEntityBlClrDto.LastScanCheckSum = lastScaltime;

            if (userDefineEntityList.Count > 0)
            {
                Dictionary<int, List<LookupItemDto>> dictEnumList = GetUserDefineEntityDisplayInfoList(userDefineEntityList, conn);
                foreach (var dictitem in dictEnumList)
                {
                    dictEntityDisplayInfoList.Add(dictitem.Key, dictitem.Value);
                }
            }
      
        }



        private static void GetSysDefineDictLookItemNew(SqlConnection conn, Dictionary<int, List<LookupItemDto>> dictEntityDisplayInfoList, List<PdmEntityBlClrDto> listPdmEntityDto)
        {
            List<PdmEntityBlClrDto> sysDefineEntityList = listPdmEntityDto.Where(o => (o.EntityType.HasValue)
                && (o.EntityType.Value == (int)EmEntityType.SystemDefineTable)).Distinct().ToList();
          

            Dictionary<int, List<LookupItemDto>> dictEnumList = GetSysDefineEntityDisplayInfoListNew(sysDefineEntityList, conn); ;
            foreach (var dictitem in dictEnumList)
            {
                dictEntityDisplayInfoList.Add(dictitem.Key, dictitem.Value);

            }
        }

    
        private static Dictionary<int, List<LookupItemDto>> GetSysDefineEntityDisplayInfoListNew(List<PdmEntityBlClrDto> sysDefineEntityList, SqlConnection conn)
        {
            Dictionary<int, List<LookupItemDto>> toReturn = new Dictionary<int, List<LookupItemDto>>();

            Dictionary<string, string> dictTableNameAndQuery = new Dictionary<string, string>();

            foreach (var aPdmEntityDto in sysDefineEntityList)
            {
                string systemQueryTable = GetSysDefineQueryIDAndDisplay(aPdmEntityDto);
              //  CLROutput.OutputDebug("aPdmEntityDto.SysTableName" + aPdmEntityDto.EntityId + "=" + aPdmEntityDto.EntityCode + aPdmEntityDto.SysTableName);
                if (!dictTableNameAndQuery.ContainsKey(aPdmEntityDto.SysTableName))
                {
                    dictTableNameAndQuery.Add(aPdmEntityDto.SysTableName, systemQueryTable);

                    CLROutput.OutputDebug("aPdmEntityDto.SysTableName" + aPdmEntityDto.EntityId + "=" + aPdmEntityDto.EntityCode + aPdmEntityDto.SysTableName + " systemQueryTable=" + systemQueryTable);
                }
            }

            // SqlContext.Pipe.Send("queryRowValue=" + allSystemQueryTable);
            DataSet result = DataAcessHelper.GetDataSetQueryResult(conn, dictTableNameAndQuery);

            //  var dictTableNameAndDto = sysDefineEntityList.ToDictionary(o => o.EntityId , o => o);

            foreach (var SysEntityDto in sysDefineEntityList)
            {
                string sysTableName = SysEntityDto.SysTableName;
                foreach (DataTable datatable in result.Tables)
                {
                  //  CLROutput.SendDataTable(datatable);

                    if (sysTableName == datatable.TableName)
                    {
                        List<LookupItemDto> listDto = new List<LookupItemDto>();
                        foreach (DataRow row in datatable.Rows)
                        {
                            LookupItemDto itemDto = new LookupItemDto();
                            itemDto.Id = (int)row["Id"];
                            itemDto.Display = row["Display"] as string;
                            listDto.Add(itemDto);
                        }

                        toReturn.Add(SysEntityDto.EntityId, listDto);
                    }

                    //if(
                    // DataSetUtilities.SendDataTable(datatable);
                }
            }

            return toReturn;
        }
        internal static string GetSysDefineQueryIDAndDisplay(PdmEntityBlClrDto aPdmEntityDto)
        {
            var SystemDefinePrimaryKeyColumnName = aPdmEntityDto.PdmUserDefineEntityColumnList.Where(o => o.IsPrimaryKey.HasValue && o.IsPrimaryKey.Value).FirstOrDefault();
            var SystemDefineDisplayColumnNames = aPdmEntityDto.PdmUserDefineEntityColumnList.Where(o => !string.IsNullOrEmpty(o.SystemTableColumnName)).ToList();

            string splitToken = "+' | '+";

            string aselectIdQuery = string.Empty;
            if (SystemDefinePrimaryKeyColumnName != null)
            {
                aselectIdQuery = "select " + SystemDefinePrimaryKeyColumnName.SystemTableColumnName + " as Id, ";
            }

            string aDisplay = GetDisplayColumnNew(splitToken, aPdmEntityDto);

            if (!string.IsNullOrEmpty(aselectIdQuery) && SystemDefineDisplayColumnNames.Count > 1)
            {
                return aselectIdQuery + "( " + aDisplay + " )  as Display " + " from " + aPdmEntityDto.SysTableName + " order by " + SystemDefineDisplayColumnNames[0].SystemTableColumnName;
            }
            else
            {
                return string.Empty;
            }
        }
        private static string GetDisplayColumnNew(string splitToken, PdmEntityBlClrDto aPdmEntityDto)
        {
            string aDisplay = string.Empty;
            string orderby = string.Empty;

            var SystemDefineDisplayColumnNames = aPdmEntityDto.PdmUserDefineEntityColumnList.Where(o => !string.IsNullOrEmpty(o.SystemTableColumnName)).ToList();

            foreach (var aPdmUserDefineEntityColumnEntity in SystemDefineDisplayColumnNames)
            {
                if (aPdmUserDefineEntityColumnEntity.IsPrimaryKey != true)
                {
                    aDisplay = aDisplay + " IsNull(  cast ( " + aPdmUserDefineEntityColumnEntity.SystemTableColumnName + " as nvarchar(MAX)  ) , '' )" + splitToken;
                }
                   // aDisplay = aDisplay + " IsNull(  cast ( " + aPdmUserDefineEntityColumnEntity.SystemTableColumnName + " as varchar ) , '' )" + splitToken;
            }

            if (aDisplay != string.Empty)
            {
                aDisplay = aDisplay.Substring(0, aDisplay.Length - splitToken.Length);
            }
            return aDisplay;
        }
        internal static void GetEnumDictLookItemNew(SqlConnection conn, Dictionary<int, List<LookupItemDto>> dictEntityDisplayInfoList, List<PdmEntityBlClrDto> listPdmEntityDto)
        {
            List<PdmEntityBlClrDto> EnumEntityList = listPdmEntityDto.Where(o => o.EntityType.HasValue && o.EntityType.Value == (int)EmEntityType.PDMEnum).ToList();

        
            var EnumEntityIds = EnumEntityList.Select(o => o.EntityId).Distinct().ToList();
            if (EnumEntityIds.Count > 0)
            {
                Dictionary<int, List<LookupItemDto>> dictEnumList = GetEnumEntityDisplayInfoList(EnumEntityIds, conn);
                foreach (var dictitem in dictEnumList)
                {
                    dictEntityDisplayInfoList.Add(dictitem.Key, dictitem.Value);
                }
            }
        }
        private static Dictionary<int, List<LookupItemDto>> GetEnumEntityDisplayInfoList(List<int> entityIDs, SqlConnection conn)
        {
            Dictionary<int, List<LookupItemDto>> toReturn = new Dictionary<int, List<LookupItemDto>>();

            string enumquery = "  select distinct  EntityID ,EnumKey ,EnumValue  from PdmEntityEnumValue  ";
            if (entityIDs != null && entityIDs.Count > 0)
            {
                enumquery = enumquery + "   where " + DataAcessHelper.GenerateColumnInClauseWithAndCondition(entityIDs, "EntityID", false);
            }

            //  SqlContext.Pipe.Send("enumquery=" + enumquery);

            DataTable dataTable = DataAcessHelper.GetDataTableQueryResult(conn, enumquery);

            //  var result = dataTable.

            var subItemQuery = from row in dataTable.AsDataRowEnumerable()
                               group row by new
                               {
                                   EntityID = (int)row["EntityID"]
                               } into grp

                               select new
                               {
                                   EntityID = grp.Key.EntityID,
                                   EnumKeyIDAndEnumValueList = grp.Select(r => new LookupItemDto { Id = (int)r["EnumKey"], Display = r["EnumValue"] as string }).ToList(),
                               };

            foreach (var o in subItemQuery)
            {
                toReturn.Add(o.EntityID, o.EnumKeyIDAndEnumValueList);
            }

            return toReturn;
        }

        public static List<PdmEntityColumnClrUserDefineDto> GetErpExchangeDatabaseTableColumnDto(SqlConnection conn, string databaseTableName)
        {
            string query = string.Format(@"SELECT  Columns.COLUMN_NAME as ColumnName,Columns.DATA_TYPE as DataType,
             Columns.CHARACTER_MAXIMUM_LENGTH as MaxLength, Columns.DATETIME_PRECISION as Precision,  Columns.NUMERIC_SCALE as Scale,
            cast( (case when  PK.COLUMN_NAME Is not null then 1 else 0 end) as bit) as PrimaryKey

            FROM information_schema.columns as Columns
            left join
            (
               SELECT column_name
            FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
            WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1
            AND table_name = '{0}'
            ) as PK on Pk.COLUMN_NAME =  Columns.COLUMN_NAME
            WHERE table_name = '{0}'  and
             ( Columns.DATA_TYPE <> 'timestamp'  and  Columns.DATA_TYPE <>'image'
                and   Columns.DATA_TYPE <>'xml'
                      and   Columns.DATA_TYPE <>'varbinary'
             )
           and  Columns.COLUMN_NAME  NOT IN ('{1}') ",
                                          databaseTableName,

                                        PLMConstantString.ExchangeRowDataChangeTimeStamp

                                        );

            List<PdmEntityColumnClrUserDefineDto> toreturnList = new List<PdmEntityColumnClrUserDefineDto>();

            // PLMSDataImport.ExchangeRowDataPLMImportDateTimeColumn
            // CLROutput.Output(query);
            var result = DataAcessHelper.GetDataTableQueryResult(conn, query);

            //  CLROutput.SendDataTable(result);

            foreach (DataRow arow in result.Rows)
            {
                PdmEntityColumnClrUserDefineDto fromDataBaseDto = PLMDataRowConverter.ConvertErpExchangeDatabaseTableColumnDataRowToPdmEntityColumnDto(arow);

                toreturnList.Add(fromDataBaseDto);
            }

            var textboxColumn = toreturnList.Where(o => o.UicontrolType == (int)EmControlType.TextBox).FirstOrDefault();
            if (textboxColumn != null)
            {
                textboxColumn.UsedByDropDownList = true;
            }
            return toreturnList;
        }

        public static List<string> GetPrimarykeyList(SqlConnection conn, string databaseTableName)
        {
            string query = string.Format(@"  SELECT  column_name
            FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
            WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1
            AND table_name = '{0}' ", databaseTableName);

            //  SqlContext.Pipe.Send( query);
            var result = DataAcessHelper.GetDataTableQueryResult(conn, query);

            //CLROutput.SendDataTable(result);

            return result.AsDataRowEnumerable().Select(row => row["column_name"].ToString()).ToList();
        }

        public static DataTable GetMutipleSysDefineEntityRowValue(int entityID, List<string> systemColumNames)
        {
            string systemDefineColumnName = string.Empty;
            foreach (var columnName in systemColumNames)
            {
                if (columnName != string.Empty)
                {
                    systemDefineColumnName = systemDefineColumnName + columnName + ",";
                }
            }

            if (systemDefineColumnName != string.Empty)
            {
                systemDefineColumnName = systemDefineColumnName.Substring(0, systemDefineColumnName.Length - 1);

                // var pdmSystemEntity = GetBLEntityStructureById(entityID);

                var pdmSystemEntity = PdmCacheManager.DictPdmEntityBlEntity[entityID];
                string connectionInfo = GetConnectionInfoWithCode(pdmSystemEntity.DataSourceFrom); 

                //  string idInclause = DataAcessHelper.GenerateColumnInClauseWithAndCondition(keyValues, pdmSystemEntity.SystemDefinePrimaryKeyColumnName.SystemTableColumnName, false);
                string aQuery = "select distinct " + pdmSystemEntity.SystemDefinePrimaryKeyColumnName.SystemTableColumnName + " as Id, " + systemDefineColumnName
                              + " from " + pdmSystemEntity.SysTableName; ///+ "  where " + idInclause;

                // need to filter
                using (SqlConnection conn = new SqlConnection(connectionInfo))
                {
                    //SqlContext.
                    conn.Open();
                    return DataAcessHelper.GetDataTableQueryResult(conn, aQuery);
                }
            }

            return new DataTable();
        }

        internal static string GetConnectionInfoWithCode(int? dataSourceFrom)
        {
        
            if (dataSourceFrom.HasValue)
            {
                string dataSourceFronQuery = @"select ConnectionString from PdmDataSource where  DataSourceFrom= @DataSourceFrom";
                List<SqlParameter> listParamter = new List<SqlParameter> ();
                listParamter.Add ( new SqlParameter("@DataSourceFrom",dataSourceFrom));

                string dataSourValue = null;
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                {
               
                    conn.Open();
                   dataSourValue = DataAcessHelper.RetriveSigleValue(conn, dataSourceFronQuery, listParamter) as string;
                }
       
                if (string.IsNullOrEmpty(dataSourValue))
                {
                  
                    return PLMConstantString.PLM_APP_ConnectionString;
                }
                else
                {
            
                    return dataSourValue.ToString();
                }
              
            }
            else
            {
                return PLMConstantString.PLM_APP_ConnectionString; 
            
            }
          
        }
    }
}