using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace PLMCLRTools
{
    public partial class TestPLMSUserDefineTableExchagne
    {
        // Auto Create PLM user Define Entity
       // [Microsoft.SqlServer.Server.SqlProcedure]
        public static void TestUserDefineConnection()
        {
            //Step4 Transfer NewRecord FromERP To PLM

            if (CheckErpAndPLMStrcuture())
            {
                List<PdmClrEntitySimpleStructureDto> plmImportEntityDtoList = GetPlmImportedEntityDtoList();

                // step1 Transfer New O rModified Record From ExChange
                TransferNewOrModifiedRecordFromExChangeToPLM(plmImportEntityDtoList);

                //Step 2: update FK relation Ship
                UpdateForeignKeyRelationFromERPToPLM(plmImportEntityDtoList);

                //step3: save ex-change table last change timestamp;
                UpdateLastScanTimeStamp(plmImportEntityDtoList);
            }
        }


        // need to manully delete user define entity in PLMS if ex-chagne table is delete

        private static bool CheckErpAndPLMStrcuture()
        {
            BunchObject aBunchObject = new BunchObject();

            // Get all table from Ex-change database
            Dictionary<string, List<PdmEntityColumnClrUserDefineDto>> dictExchangeTableNameAndColumnDto
                = GetErpExDatabaseTableNameAndStructure();
            List<string> erpExChangeDataBaseEntityListPrefixList = dictExchangeTableNameAndColumnDto.Keys.ToList();

            // Gel all import Entity from PLMS database
            //Key : systable Name
            var dictPlmImportedTableColumnDto = GetPLMImportedEntityNameAndStrcuture();
            List<string> ImportedSystableNameList = dictPlmImportedTableColumnDto.Keys.ToList();

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                string folderId = GetImportDefaultFolderId(conn);

                // o1: new Entity list From ERP
                List<string> newAddedTableList = erpExChangeDataBaseEntityListPrefixList.Except(ImportedSystableNameList).ToList();
                aBunchObject.o1 = newAddedTableList;

                // o2: share entity list
                List<string> commonEntity = erpExChangeDataBaseEntityListPrefixList.Intersect(ImportedSystableNameList).ToList();
                aBunchObject.o2 = commonEntity;

                List<string> notSameStructureList = new List<string>();
                foreach (string commtable in commonEntity)
                {
                    bool isSamestrut = IsSameStrcuture(commtable, dictExchangeTableNameAndColumnDto, dictPlmImportedTableColumnDto);

                    if (!isSamestrut)
                    {
                        notSameStructureList.Add(commtable);
                    }
                }

                if (notSameStructureList.Count > 0)
                {
                    SqlContext.Pipe.Send("Erp and PLM  structure not match ");

                    foreach (string changestrcuture in notSameStructureList)
                    {
                        SqlContext.Pipe.Send(changestrcuture + " structure is changed ");
                    }

                    return false;
                }

                SqlContext.Pipe.Send("done:check common tables structure ");

                // o3: entity in PLM but not in ERP, need to use user define mapping
                List<string> newPlmEntityList = ImportedSystableNameList.Except(erpExChangeDataBaseEntityListPrefixList).ToList();
                aBunchObject.o3 = newPlmEntityList;

                foreach (string newTableName in newAddedTableList)
                {
                    CLROutput.OutputDebug("newTableName=" + newTableName);
                    string entityCode = PLMConstantString.EX_PLM_Import_Prefix + newTableName;
                    object newEntityID = InsertNewExportUserDefineEntity(folderId, conn, entityCode, newTableName);
                    List<PdmEntityColumnClrUserDefineDto> columnDtoList = dictExchangeTableNameAndColumnDto[newTableName];

                    foreach (PdmEntityColumnClrUserDefineDto pdmEntityColumnClrDto in columnDtoList)
                    {
                        pdmEntityColumnClrDto.EntityId = (int)newEntityID;
                        InsertPdmUserDefineColunmDto(pdmEntityColumnClrDto, conn);
                    }

                    //TODO
                    // need to register mapping in
                }
            }

            return true;
        }
        private static void UpdateLastScanTimeStamp(List<PdmClrEntitySimpleStructureDto> plmImportEntityDtoList)
        {
            DateTime now = System.DateTime.Now;

            Dictionary<string, byte[]> dictExTablelastScanTimeStamp = new Dictionary<string, byte[]>();
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
            {
                conn.Open();
                foreach (PdmClrEntitySimpleStructureDto aPdmEntityClrUserDefineDto in plmImportEntityDtoList)
                {
                    string exTableName = aPdmEntityClrUserDefineDto.SysTableName;

                    string query = @" select MAX(ChangeTimeStamp)  as MaxTimeStamp  from  " + exTableName;
                    CLROutput.OutputDebug(query);

                    DataTable result = DataAcessHelper.GetDataTableQueryResult(conn, query);
                    if (result.Rows.Count > 0)
                    {
                        dictExTablelastScanTimeStamp.Add(exTableName, result.Rows[0]["MaxTimeStamp"] as byte[]);
                    }
                }
            }

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();
                foreach (PdmClrEntitySimpleStructureDto aPdmEntityClrUserDefineDto in plmImportEntityDtoList)
                {
                    string exTableName = aPdmEntityClrUserDefineDto.SysTableName;
                    if (dictExTablelastScanTimeStamp.ContainsKey(exTableName))
                    {
                        string insertTimeStamp = string.Format(@" insert into [dbo].[PdmTableLastScanLog]
                                    (
                                        [LastScanTime]
                                        ,[TableName]
                                        ,[LastModifyTimeStamp]
                                    )
                                    values
                                    (
                                        '{0}',
                                        '{1}',
                                       @MaxTimeStamp

                                    )  ",
                                now.ToString(),
                                exTableName
                             );

                        List<SqlParameter> listparamter = new List<SqlParameter>();
                        listparamter.Add(new SqlParameter("@MaxTimeStamp", dictExTablelastScanTimeStamp[exTableName]));

                        DataAcessHelper.GetDataTableQueryResult(conn, insertTimeStamp, listparamter);
                    }
                }
            }
        }

        private static Dictionary<string, List<PdmEntityColumnClrUserDefineDto>> GetPLMImportedEntityNameAndStrcuture()
        {
            List<PdmClrEntitySimpleStructureDto> importedPLMEntityDtoList = GetPlmImportedEntityDtoList();

            //!!!!!! need to use System Table name to match EX-change table name
            var dictPlmImportedTableColumnDto = importedPLMEntityDtoList.ToDictionary(o => o.SysTableName, o => o.Columns);
            return dictPlmImportedTableColumnDto;
        }

        private static List<PdmClrEntitySimpleStructureDto> GetPlmImportedEntityDtoList()
        {
            string queryllEntityImport = @"  select  EntityID from pdmEntity where IsImport =1 and   EntityType = " + (int)EmEntityType.UserDefineTable;
           
            List<int> enityIds = null; ;
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                DataTable dtresult = DataAcessHelper.GetDataTableQueryResult(conn, queryllEntityImport);
                 enityIds = dtresult.GetDistinctOneColumnValueIds("EntityID");
               
            }
            return PLMSEntityClrBL.GetEntityAndColumnStrcutureInfoList(enityIds);
        }

        private static bool IsSameStrcuture(string commtable, Dictionary<string, List<PdmEntityColumnClrUserDefineDto>> dictErpExchangeTableColumnDto, Dictionary<string, List<PdmEntityColumnClrUserDefineDto>> dictPlmExchangeTableColumnDto)
        {
            List<PdmEntityColumnClrUserDefineDto> erpDtoColumnList = dictErpExchangeTableColumnDto[commtable];
            List<PdmEntityColumnClrUserDefineDto> plmDtoColumnList = dictPlmExchangeTableColumnDto[commtable];

            //CLROutput.Output(commtable + "erpDtoColumnList count= " + erpDtoColumnList.Count() );
            //CLROutput.Output(commtable + "plmDtoColumnList count= " + plmDtoColumnList.Count());

            List<string> plmcolumnNameList = plmDtoColumnList.Select(o => o.ColumnName.ToLowerInvariant()).ToList();

            Dictionary<string, PdmEntityColumnClrUserDefineDto> dictPlmEntityDto = plmDtoColumnList.ToDictionary(o => o.ColumnName.ToLowerInvariant(), o => o);

            foreach (PdmEntityColumnClrUserDefineDto erpColumnDto in erpDtoColumnList)
            {
                string erpColumnNam = erpColumnDto.ColumnName.ToLowerInvariant();

                //  CLROutput.Output("erpColumnNam=" + erpColumnNam);
                if (dictPlmEntityDto.ContainsKey(erpColumnNam))
                {
                    continue;
                }
                else // contain name need to check if the datatype is matching
                {
                    //  CLROutput.Output("dictPlmEntityDto.ContainsKey is false" + erpColumnNam);
                    return false;
                }
            }

            return true;
        }

        private static void UpdateForeignKeyRelationFromERPToPLM(List<PdmClrEntitySimpleStructureDto> plmImportEntityDtoList)
        {
            //CLROutput.Output("begine plm query");

            List<string> exchangeTableNameList = plmImportEntityDtoList.Select(o => o.SysTableName).ToList();

            Dictionary<string, PdmClrEntitySimpleStructureDto> dictAllImportEntity = plmImportEntityDtoList.ToDictionary(o => o.SysTableName, o => o);

            string queryForeignTable = @" SELECT distinct OBJECT_NAME(PARENT_OBJECT_ID)        TABLE_NAME,
           PT.NAME                              FIELD_NAME,
           OBJECT_NAME(REFERENCED_OBJECT_ID)    REFTABLE_NAME,
           FT.NAME                              REFFIELD_NAME
            FROM   SYS.FOREIGN_KEY_COLUMNS FKC
           JOIN SYS.COLUMNS PT
             ON FKC.PARENT_OBJECT_ID = PT.OBJECT_ID
                AND FKC.PARENT_COLUMN_ID = PT.COLUMN_ID
           JOIN SYS.COLUMNS FT
             ON FKC.REFERENCED_OBJECT_ID = FT.OBJECT_ID
                AND FKC.REFERENCED_COLUMN_ID = FT.COLUMN_ID ";

            // WHERE     (OBJECT_NAME(FKC.parent_object_id))";.

            string tableInClause = DataAcessHelper.GenerateColumnInClauseWithAndCondition(exchangeTableNameList, "OBJECT_NAME(FKC.parent_object_id)", false);

            //  SqlContext.Pipe.Send("tableInClause=" + tableInClause);

            queryForeignTable = queryForeignTable + " WHERE " + tableInClause;

            //  CLROutput.Output(queryForeignTable);

            DataTable foreignKeyTable = null;

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
            {
                conn.Open();
                foreignKeyTable = DataAcessHelper.GetDataTableQueryResult(conn, queryForeignTable);

                //  CLROutput.SendDataTable(foreignKeyTable);
            }

            // need to map new RowID to the  export new ID
            int rowCount = 1;
            foreach (DataRow fkDataRow in foreignKeyTable.Rows)
            {
                string childEntityEntityName = (fkDataRow["TABLE_NAME"] as string).Trim().ToLowerInvariant();
                string childFkColumnName = (fkDataRow["FIELD_NAME"] as string).Trim().ToLowerInvariant();
                string masterEntityTableName = (fkDataRow["REFTABLE_NAME"] as string).Trim().ToLowerInvariant(); ;
                string masterKeyName = (fkDataRow["REFFIELD_NAME"] as string).Trim().ToLowerInvariant();

                //  CLROutput.Output("before childEntityEntityName=" + childEntityEntityName);

                PdmClrEntitySimpleStructureDto ChildEntityDto = dictAllImportEntity[childEntityEntityName];

                //  CLROutput.Output("after EntityWithFKEntityDto=" + EntityWithFKEntityDto.EntityCode);

                Dictionary<string, string> dictErpChildEntityPKAndFKInExTableValue = null;
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
                {
                    conn.Open();

                    string selectKeyColunm = "";
                    string appendSign = "+'_' +";
                    string CombKey = "CombKey";

                    // select  ( cast (BrandID as nvarchar) +'_' + cast (TypeID as nvarchar)) as ComKey
                    foreach (var keyColumn in ChildEntityDto.PrimaryKeyColumn)
                    {
                        selectKeyColunm += string.Format(" cast ({0} as nvarchar)", keyColumn.SystemTableColumnName) + appendSign;
                    }

                    selectKeyColunm = selectKeyColunm.Substring(0, selectKeyColunm.Length - appendSign.Length);

                    selectKeyColunm = string.Format("({0}) as {1}", selectKeyColunm, CombKey);

                 
                    string childTablname = ChildEntityDto.SysTableName;
                    string querytchildEntityPKAndFkValue = string.Format(@" select   {0},{1} from {2} ", selectKeyColunm, childFkColumnName, childTablname);

                    CLROutput.OutputDebug("querytchildEntityPKAndFkValue=" + querytchildEntityPKAndFkValue);
                    DataTable ErpChildEntityPKAndFKInExTable = DataAcessHelper.GetDataTableQueryResult(conn, querytchildEntityPKAndFkValue);
                    dictErpChildEntityPKAndFKInExTableValue = ErpChildEntityPKAndFKInExTable.AsDataRowEnumerable().ToDictionary(o => o[CombKey].ToString(), o => o[childFkColumnName].ToString());
                }

                // check next Row if
                if (dictErpChildEntityPKAndFKInExTableValue == null)
                    continue;

                PdmClrEntitySimpleStructureDto masterEntityEntityDto = dictAllImportEntity[masterEntityTableName];
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                {
                    conn.Open();

                    // need to update mapping Value betweek RowID and ValueId
                    string constValueText = "ValueText";
                    string constRowID = "RowID";
                    string queryMasterEntityRowIDValueID = string.Format(@"select {0},{1}  from pdmUserDefineEntityRow where EntityID ={2}",
                        constValueText, constRowID, masterEntityEntityDto.EntityId);

                    DataTable MasterEntityErpPrimayKeyDataTable = DataAcessHelper.GetDataTableQueryResult(conn, queryMasterEntityRowIDValueID);
                    Dictionary<string, int> dictMasterEntityErpPrimayKeyAsValueTextAndRowID = MasterEntityErpPrimayKeyDataTable.AsDataRowEnumerable()
                        .Where(o => ((o[constValueText] as string) != null))
                        .ToDictionary(o => (string)o[constValueText], o => (int)o[constRowID]);

                    foreach (var childFKColumn in ChildEntityDto.Columns)
                    {
                        if (childFKColumn.ColumnName.ToLowerInvariant() == childFkColumnName.ToLowerInvariant())
                        {
                            string getchildentityFkUserDefineValue = string.Format(@"select {0},{1}  from pdmUserDefineEntityRow where EntityID ={2}",
                            constValueText, constRowID, ChildEntityDto.EntityId);

                            DataTable childentityFkUserDefineValueDataTable = DataAcessHelper.GetDataTableQueryResult(conn, getchildentityFkUserDefineValue);

                            //  CLROutput.SendDataTable(childentityFkUserDefineValueDataTable);

                            foreach (DataRow valueRow in childentityFkUserDefineValueDataTable.Rows)
                            {
                                int rowId = (int)valueRow[constRowID];
                                string childErpPKId = valueRow[constValueText] as string;

                                int? mappingMasterEntityRowID = null;
                                if (childErpPKId != null)
                                {
                                    // CLROutput.Output("childErpPKId=" + childErpPKId);
                                    // possible is conbiationKey
                                    string erpExchangechildId = childErpPKId;
                                    if (dictErpChildEntityPKAndFKInExTableValue.ContainsKey(erpExchangechildId))
                                    {
                                        // CLROutput.Output("dictErpChildEntityPKAndFKInExTableValue.ContainsKey exchangechildid=" + erpExchangechildId);

                                        string erpMasterEntityPK = dictErpChildEntityPKAndFKInExTableValue[erpExchangechildId];

                                        // CLROutput.Output("dictErpChildEntityPKAndFKInExTableValue[exchangechildid];=" + erpMasterEntityPK);
                                        if (erpMasterEntityPK != null)
                                        {
                                            if (dictMasterEntityErpPrimayKeyAsValueTextAndRowID.ContainsKey(erpMasterEntityPK))
                                            {
                                                mappingMasterEntityRowID = dictMasterEntityErpPrimayKeyAsValueTextAndRowID[erpMasterEntityPK];
                                            }
                                        }
                                    }
                                }

                                string updateFKColumnValueID = @" update pdmUserDefineEntityRowValue
                                                                             set  ValueID = @ValueID,
                                                                                  ValueDate =@ValueDate,
                                                                                  ValueText = @ValueText

                                                                                 WHERE RowID =@RowID
                                                                                AND     UserDefineEntityColumnID =@FkColumnID";

                                SqlParameter aRowIdPameter = new SqlParameter("@RowID", SqlDbType.Int);
                                SqlParameter aUserDefineColumnIdIdPameter = new SqlParameter("@FkColumnID", SqlDbType.Int);

                                SqlParameter aValueIdPameter = new SqlParameter("@ValueID", SqlDbType.Int);
                                SqlParameter aValueDatePameter = new SqlParameter("@ValueDate", SqlDbType.DateTime);
                                SqlParameter aValueTextPameter = new SqlParameter("@ValueText", SqlDbType.NVarChar);

                                aRowIdPameter.Value = rowId;
                                aUserDefineColumnIdIdPameter.Value = childFKColumn.UserDefineEntityColumnID;
                                if (mappingMasterEntityRowID.HasValue)
                                {
                                    //  CLROutput.Output("mappingMasterEntityRowID=" + mappingMasterEntityRowID);
                                    aValueIdPameter.Value = mappingMasterEntityRowID.Value;
                                }
                                else
                                {
                                    aValueIdPameter.Value = DBNull.Value;
                                }

                                aValueTextPameter.Value = System.Data.SqlTypes.SqlString.Null;
                                aValueDatePameter.Value = System.Data.SqlTypes.SqlDateTime.Null;

                                SqlCommand cmdupdateFkColumnMappinfcolumn = new SqlCommand(updateFKColumnValueID, conn);
                                cmdupdateFkColumnMappinfcolumn.Parameters.Add(aRowIdPameter);
                                cmdupdateFkColumnMappinfcolumn.Parameters.Add(aUserDefineColumnIdIdPameter);
                                cmdupdateFkColumnMappinfcolumn.Parameters.Add(aValueIdPameter);
                                cmdupdateFkColumnMappinfcolumn.Parameters.Add(aValueDatePameter);
                                cmdupdateFkColumnMappinfcolumn.Parameters.Add(aValueTextPameter);

                                cmdupdateFkColumnMappinfcolumn.ExecuteNonQuery();

                                //  CLROutput.Output("cmdupdateCell=" + updateFKColumnValueID);
                            }

                            string updatepdmUserDefineEntityColumn = string.Format(@"update pdmUserDefineEntityColumn
                                        set UIControlType={0},
                                         FKEntityID ={1}
                                        where UserDefineEntityColumnID ={2}", (int)EmControlType.DDL, masterEntityEntityDto.EntityId, childFKColumn.UserDefineEntityColumnID);
                            SqlCommand cmdupdate = new SqlCommand(updatepdmUserDefineEntityColumn, conn);

                            //  CLROutput.Output("updatepdmUserDefineEntityColumn=" + updatepdmUserDefineEntityColumn);
                            cmdupdate.ExecuteNonQuery();
                        }
                    }

                    // need to updat FK control Type as DDL type
                    foreach (var childFKColumn in ChildEntityDto.Columns)
                    {
                        if (childFKColumn.ColumnName.ToLowerInvariant() == childFkColumnName.ToLowerInvariant())
                        {
                            string updatepdmUserDefineEntityColumn = string.Format(@"update pdmUserDefineEntityColumn
                                        set UIControlType={0},
                                         FKEntityID ={1}
                                        where UserDefineEntityColumnID ={2}", (int)EmControlType.DDL, masterEntityEntityDto.EntityId, childFKColumn.UserDefineEntityColumnID);
                            SqlCommand cmdupdate = new SqlCommand(updatepdmUserDefineEntityColumn, conn);

                            // CLROutput.Output("updatepdmUserDefineEntityColumn=" + updatepdmUserDefineEntityColumn);
                            cmdupdate.ExecuteNonQuery();
                        }
                    }

                    // PdmEntityClrDto masterEntityEntityDto
                    // no need to isnert relationmship
                    // InsertEntityWithFKRelaship(ChildEntityDto, childFkColumnName, masterEntityEntityDto, conn);
                }
            }
        }

        //?????????? TODO list....
        private static void InsertEntityWithFKRelaship(PdmClrEntitySimpleStructureDto EntityWithFKEntityDto, string EntityWithFKEntityForeighKeyColumn, PdmClrEntitySimpleStructureDto masterEntityEntityDto, SqlConnection conn)
        {
            // EntityWithFKEntityDto.EntityId, Both PrimaryKeyColumn.UserDefineEntityColumnID and MasterEntityColumn.UserDefineEntityColumnID are in the same EntityWithFKEntityDto

            string masterEntityTableName = masterEntityEntityDto.SysTableName;

            string queryGetImportFolderId = " SELECT SetupValue FROM pdmsetup WHERE setupCode='ImportEntityFolder'";
            SqlCommand cmdGetFodlerId = new SqlCommand(queryGetImportFolderId, conn);
            string folderId = cmdGetFodlerId.ExecuteScalar().ToString();

            var PrimaryKeyColumn = EntityWithFKEntityDto.Columns.Where(o => o.IsPrimaryKey.HasValue && o.IsPrimaryKey.Value).FirstOrDefault(); ;
            if (PrimaryKeyColumn == null)
                return;

            var MasterEntityColumn = EntityWithFKEntityDto.Columns.Where(o => o.ColumnName.ToLowerInvariant() == EntityWithFKEntityForeighKeyColumn.ToLowerInvariant()).First();
            if (MasterEntityColumn == null)
                return;

            string selectRelationEntity = string.Format(@" select EntityID  from  dbo.pdmentity
            where  EntityWithFKEntityID={0}
            and ChildEntityColumnID={1}
            and MasterEntityColumnID={2}",
              EntityWithFKEntityDto.EntityId,
              PrimaryKeyColumn.UserDefineEntityColumnID,
              MasterEntityColumn.UserDefineEntityColumnID
            );
            SqlCommand cmdGetRelationEntityId = new SqlCommand(selectRelationEntity, conn);
            object relationEntityId = cmdGetRelationEntityId.ExecuteScalar();

            if (relationEntityId == null)
            {
                string insertNewRaltionEntity = string.Format(@" insert into dbo.pdmentity
                    (
                        EntityCode,
                        IsImport,
                        EntityType,

                        EntityWithFKEntityID,
                        MasterEntityColumnID ,
                        ChildEntityColumnID,
                        IsRelationEntity ,
                        FolderID,
                        MasterEntityID,
                         ChildEntityID
                    )
                     values
                    (
                       '{0}',
                        {1},
                        {2},
                        {3},
                        {4},
                        {5},
                        {6} ,
                        {7},
                        {8},
                        {9}

                    )  ",
               "Import_Relation_" + masterEntityTableName + "_" + EntityWithFKEntityDto.EntityCode,
               1,
               (int)EmEntityType.RelationFKEntity,

               EntityWithFKEntityDto.EntityId,
               MasterEntityColumn.UserDefineEntityColumnID,
               PrimaryKeyColumn.UserDefineEntityColumnID,
               1,
               folderId,
               masterEntityEntityDto.EntityId,
               EntityWithFKEntityDto.EntityId

               );

                SqlCommand cmdInsertRelationEntityId = new SqlCommand(insertNewRaltionEntity, conn);

                //   CLROutput.Output("insertNewRaltionEntity=" + insertNewRaltionEntity);
                cmdInsertRelationEntityId.ExecuteNonQuery();
            }

            // need to add new RelatipnEntity
        }

        private static void UpdateModifyRecordFromERPToPLM(List<PdmClrEntitySimpleStructureDto> plmImportEntityDtoList)
        {
            foreach (PdmClrEntitySimpleStructureDto aPdmEntityClrDto in plmImportEntityDtoList)
            {
                string exchangeTableName = aPdmEntityClrDto.EntityCode;
                DataTable modifyRecordsTable = null;

                //Get Ex-change new Row Data
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
                {
                    conn.Open();

                    string queryERPNewRecord = string.Format(@" select * from  {0} where  {1}={2} ", aPdmEntityClrDto.EntityCode, PLMConstantString.ExchangeRowDataERPFlagColumn, (int)EmExChangeActionType.Modified);

                    modifyRecordsTable = DataAcessHelper.GetDataTableQueryResult(conn, queryERPNewRecord);

                    //  CLROutput.SendDataTable(newRecordsTable);
                }

                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                {
                    conn.Open();

                    foreach (DataRow row in modifyRecordsTable.Rows)
                    {
                        int rowId = (int)row[PLMConstantString.ExchangeRowDataPLMPrimayKeyColumn];
                        SqlCommand cmdDelete = new SqlCommand();
                        cmdDelete.CommandText = @" delete pdmUserDefineEntityRowValue where RowID=" + rowId;
                        cmdDelete.Connection = conn;
                        cmdDelete.ExecuteNonQuery();
                        InsertRowCellValue(row, conn, aPdmEntityClrDto, rowId);
                    }
                }

                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
                {
                    conn.Open();

                    string updateExchangeStatus = string.Format(@"
                                               update  {0}
                                               set  {1}={2} ,
                                               {3}=CURRENT_TIMESTAMP
                                               where {4}={5} ",
                                          aPdmEntityClrDto.EntityCode,
                                          PLMConstantString.ExchangeRowDataERPFlagColumn, (int)EmExChangeActionType.NoChange,
                                         PLMConstantString.ExchangeRowDataPLMImportDateTimeColumn,
                                         PLMConstantString.ExchangeRowDataERPFlagColumn, (int)EmExChangeActionType.Modified);

                    SqlCommand cmdUpdateExTableFromNewToNochnage = new SqlCommand(updateExchangeStatus, conn);
                    cmdUpdateExTableFromNewToNochnage.ExecuteNonQuery();
                }

                // Update Ex-change Tabl
            }
        }

     
        private static void TransferNewOrModifiedRecordFromExChangeToPLM(List<PdmClrEntitySimpleStructureDto> plmImportEntityDtoList)
        {

            Dictionary<string, byte[]> dictTableLastScanTimeStamp;
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                string querylastScanTimeStamp = @"select TableName,LastModifyTimeStamp from PdmTableLastScanLog where LastScanTime = (select  MAX(LastScanTime) from PdmTableLastScanLog)";

                DataTable result = DataAcessHelper.GetDataTableQueryResult(conn, querylastScanTimeStamp);

                dictTableLastScanTimeStamp = result.AsDataRowEnumerable().ToDictionary(o => o["TableName"] as string, o => o["LastModifyTimeStamp"] as byte[]);

            }


            foreach (PdmClrEntitySimpleStructureDto aPdmEntityClrDto in plmImportEntityDtoList)
            {
                string exchangeTableName = aPdmEntityClrDto.SysTableName;

                DataTable exNewOrModifiedRecordsTable = null;
              


                //Get Ex-change new Row Data
                
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
                {
                    conn.Open();
                    string queryERPNewRecord = string.Empty;
                    List<SqlParameter> listParamter = new List<SqlParameter>();
                    if (dictTableLastScanTimeStamp.ContainsKey(exchangeTableName))
                    {
                        queryERPNewRecord = string.Format(@" select * from  {0}  where  {1} > @LastScanTimeStamp", exchangeTableName, PLMConstantString.ExchangeRowDataChangeTimeStamp );

                        listParamter.Add(new SqlParameter("@LastScanTimeStamp", dictTableLastScanTimeStamp[exchangeTableName]));

                    }
                    else
                    {

                        queryERPNewRecord = string.Format(@" select * from  {0}   ", exchangeTableName);
                    }
                   

                    CLROutput.OutputDebug(queryERPNewRecord);
                    exNewOrModifiedRecordsTable = DataAcessHelper.GetDataTableQueryResult(conn, queryERPNewRecord, listParamter);

                    //  CLROutput.SendDataTable(newRecordsTable);
                }

              

                Dictionary<string ,DataRow>  dictExChagneNewOrModifyRow = new Dictionary<string,DataRow> ();
                foreach ( DataRow row in exNewOrModifiedRecordsTable.Rows)
                {
                  string pkValue =  GetOnePrimaryKeyColumnValue(row, aPdmEntityClrDto.PrimaryKeyColumn);
                  dictExChagneNewOrModifyRow.Add (pkValue,row);
             
                
                }

                

                // need to check which row is modifed , which one is new

                //exNewOrModifiedRecordsTable.se 

                // key comKey, value RowId( need to update this row
                Dictionary<string, int> dictPLMExsitngRow = new Dictionary<string, int>();
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                {
                    conn.Open();

                    string constValueText = "ValueText";
                    string constRowID = "RowID";
                    string queryPLMRowIDValueId = string.Format(@"select {0} as Combkey ,{1}  from pdmUserDefineEntityRow where EntityID ={2}",
                        constValueText, constRowID, aPdmEntityClrDto.EntityId);

                    DataTable plmDataTable = DataAcessHelper.GetDataTableQueryResult(conn, queryPLMRowIDValueId);
                    foreach (DataRow aRow in plmDataTable.Rows)
                    {
                        dictPLMExsitngRow.Add(aRow["Combkey"].ToString(), (int)aRow["RowID"]);
 
                    
                    }

                }


                List<DataRow> needToAddToPLMRowList = new List<DataRow>();

                //Key:RowID in PLM userDefine tAble
                Dictionary<int, DataRow> needToUpdatePLMRowLsit = new Dictionary<int, DataRow>();

             

                foreach (string comKey in dictExChagneNewOrModifyRow.Keys)
                {
                    if (dictPLMExsitngRow.ContainsKey(comKey))
                    {
                        needToUpdatePLMRowLsit.Add(dictPLMExsitngRow[comKey], dictExChagneNewOrModifyRow[comKey]);

                    }
                    else
                    {

                        needToAddToPLMRowList.Add(dictExChagneNewOrModifyRow[comKey]);
                    
                    }
                
                }


                List<string> exCombinePKValueList;
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
                {
                    conn.Open();

                    string selectKeyColunm = "";
                    string appendSign = "+'_' +";
                    string CombKey = "CombKey";

                    // select  ( cast (BrandID as nvarchar) +'_' + cast (TypeID as nvarchar)) as ComKey
                    foreach (var keyColumn in aPdmEntityClrDto.PrimaryKeyColumn)
                    {
                        selectKeyColunm += string.Format(" cast ({0} as nvarchar)", keyColumn.SystemTableColumnName) + appendSign;
                    }

                    selectKeyColunm = selectKeyColunm.Substring(0, selectKeyColunm.Length - appendSign.Length);

                    selectKeyColunm = string.Format("({0}) as {1}", selectKeyColunm, CombKey);


                    //  string childTablname = ChildEntityDto.SysTableName;
                    string querytcCombinePKValueList = string.Format(@" select   {0} from {1} ", selectKeyColunm, exchangeTableName);

                   exCombinePKValueList= DataAcessHelper.GetDataTableQueryResult(conn, querytcCombinePKValueList).AsDataRowEnumerable().Select(o => o[CombKey] as string).ToList(); 


                }


                // need to remove From PLMM list !!! A big bug here, cannot user dictExChagneNewOrModifyRow, 
                // !!!!!!!!!it will delete all no-change exsitng row
                List<int> needToDeleteFromPLMRowLsit = new List<int>();
                foreach (string comKey in dictPLMExsitngRow.Keys)
                {
                    if (!exCombinePKValueList.Contains(comKey))
                    {
                        needToDeleteFromPLMRowLsit.Add(dictPLMExsitngRow[comKey]);

                    }


                }
               



                // insert  mew Rowto PLM
                //Dictionary<object, DataRow> dictNewRowIdDataRow = new Dictionary<object, DataRow>();
                //List<PdmEntityColumnClrUserDefineDto> userDefineColumns = aPdmEntityClrDto.Columns;
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                {
                    conn.Open();
                    foreach (DataRow row in needToAddToPLMRowList)
                    {
                        object rowId = InsertPdmUserDefineEntityRowAndCellValue(aPdmEntityClrDto, row, conn);

                        ////  CLROutput.Output("rowid=" + rowId);
                        //if (rowId != null)
                        //{
                        //    dictNewRowIdDataRow.Add(rowId, row);
                        //}
                    }

                    foreach (var pairRow  in needToUpdatePLMRowLsit)
                    {
                        int rowId = pairRow.Key;
                        DataRow row = pairRow.Value;
                        SqlCommand cmdDelete = new SqlCommand();
                        cmdDelete.CommandText = @" delete pdmUserDefineEntityRowValue where RowID=" + rowId;
                        cmdDelete.Connection = conn;
                        cmdDelete.ExecuteNonQuery();
                        InsertRowCellValue(row, conn, aPdmEntityClrDto, rowId);
                    }

                    foreach (int rowId in needToDeleteFromPLMRowLsit)
                    {

                        SqlCommand cmdDelete = new SqlCommand();
                        cmdDelete.CommandText = @" delete pdmUserDefineEntityRowValue where RowID="
                            + rowId + System.Environment.NewLine +
                             @" delete pdmUserDefineEntityRow where RowID=" + rowId
                            ;
                        cmdDelete.Connection = conn;
                        cmdDelete.ExecuteNonQuery();
                    }


                        //TODO: need to set all Foreign Key as null 
                        
                }

              
                // Update Modified Row
            }

//select CURRENT_TIMESTAMP
//SELECT {fn NOW()}
//SELECT GETDATE()
        }

        // will return new RowID
        private static object InsertPdmUserDefineEntityRowAndCellValue(PdmClrEntitySimpleStructureDto aPdmEntityClrDto, DataRow row, SqlConnection conn)
        {
            if (aPdmEntityClrDto.PrimaryKeyColumn == null)
                return null;

            //PdmEntityColumnClrDto afirstPdmEntityColumnClrDto = userDefineColumns[0];
            CLROutput.OutputDebug(" insert new aPdmEntityClrDto.EntityCode " + aPdmEntityClrDto.EntityCode);

            List<PdmEntityColumnClrUserDefineDto> userDefineColumns = aPdmEntityClrDto.Columns;
            int entityID = aPdmEntityClrDto.EntityId;

            // Get keyValue
            string keyCombineValue = GetOnePrimaryKeyColumnValue(row, aPdmEntityClrDto.PrimaryKeyColumn);

            string sqlInsertpdmUserDefineEntityRow = @"

                                     Insert into    dbo.pdmUserDefineEntityRow
                                              (
                                                    EntityID,
                                                    ValueText

                                               )  " +
                                     @" values
                                            (
                                             @EntityID,
                                             @ValueText

                                            )";

            object rowID;
            using (SqlCommand cmdInsertNewentity = new SqlCommand())
            {
                SqlParameter paramterEntityId = new SqlParameter("@EntityID", SqlDbType.Int);
                paramterEntityId.Value = aPdmEntityClrDto.EntityId;
                SqlParameter paramterValueText = new SqlParameter("@ValueText", SqlDbType.NVarChar);
                if (string.IsNullOrEmpty(keyCombineValue))
                {
                    paramterValueText.Value = DBNull.Value;
                }
                else
                {
                    paramterValueText.Value = keyCombineValue;
                }

                cmdInsertNewentity.Parameters.Add(paramterEntityId);
                cmdInsertNewentity.Parameters.Add(paramterValueText);

                rowID = InsertNewEntityWithReturnNewIdentityId(conn, sqlInsertpdmUserDefineEntityRow, cmdInsertNewentity);
                InsertRowCellValue(row, conn, aPdmEntityClrDto, rowID);
            }

            return rowID;
        }

        private static int InsertNewEntityWithReturnNewIdentityId(SqlConnection conn, string sqlInsertpdmUserDefineEntityRow, SqlCommand cmdInsertNewentity)
        {
            cmdInsertNewentity.Connection = conn;
            cmdInsertNewentity.CommandText = sqlInsertpdmUserDefineEntityRow + " ; SELECT SCOPE_IDENTITY()";

            decimal scalarRowID = (decimal)cmdInsertNewentity.ExecuteScalar();

            int rowID = (int)scalarRowID;

            //Scope_Identity() returns a decimal. You can either use Convert.ToInt32 on the result of your query, or you can cast the return value to decimal and then to int.

            //cmdInsertNewentity.CommandText = "SELECT SCOPE_IDENTITY()";
            // object rowID = cmdInsertNewentity.ExecuteScalar();

            CLROutput.OutputDebug("SELECT SCOPE_IDENTITY( ) RowId=" + rowID.ToString());

            return rowID;
        }

        private static void InsertRowCellValue(DataRow row, SqlConnection conn, PdmClrEntitySimpleStructureDto aPdmEntityClrDto, object rowID)
        {
            foreach (PdmEntityColumnClrUserDefineDto columnClrDto in aPdmEntityClrDto.Columns)
            {
                //if(columnClrDto.

                using (SqlCommand cmdInsertCell = new SqlCommand())
                {
                    SqlParameter aRowIdPameter = new SqlParameter("@RowID", SqlDbType.Int);
                    SqlParameter aUserDefineColumnIdIdPameter = new SqlParameter("@UserDefineEntityColumnID", SqlDbType.Int);

                    SqlParameter aValueIdPameter = new SqlParameter("@ValueID", SqlDbType.Int);
                    SqlParameter aValueDatePameter = new SqlParameter("@ValueDate", SqlDbType.DateTime);
                    SqlParameter aValueTextPameter = new SqlParameter("@ValueText", SqlDbType.NVarChar);

                    aRowIdPameter.Value = rowID;
                    aUserDefineColumnIdIdPameter.Value = columnClrDto.UserDefineEntityColumnID;
                    aValueIdPameter.Value = System.Data.SqlTypes.SqlInt32.Null;
                    aValueDatePameter.Value = System.Data.SqlTypes.SqlDateTime.Null;
                    aValueTextPameter.Value = System.Data.SqlTypes.SqlString.Null;

                    // CLROutput.Output("SELECT SCOPE_IDENTITY( ) RowId=" + rowID);

                    //  CLROutput.Output("columnClrDto.ColumnName" + columnClrDto.ColumnName + "row[columnClrDto.ColumnName]" + row[columnClrDto.ColumnName]);

                    if (columnClrDto.UicontrolType == (int)EmControlType.DDL)
                    {
                        aValueIdPameter.Value = Converter.ToDDLSqlInt32(row[columnClrDto.ColumnName]);
                    }
                    else if (columnClrDto.UicontrolType == (int)EmControlType.Date)
                    {
                        aValueDatePameter.Value = Converter.ToSqlDateTime(row[columnClrDto.ColumnName]);
                    }
                    else if (columnClrDto.UicontrolType == (int)EmControlType.CheckBox)
                    {
                        //From Datable
                        SqlBoolean boolValue = Converter.ToSqlBoolean(row[columnClrDto.ColumnName]);

                        if (!boolValue.IsNull)
                        {
                            if (boolValue.Value)
                            {
                                aValueTextPameter.Value = "true";
                            }
                            else
                            {
                                aValueTextPameter.Value = "false";
                            }
                        }
                        else
                        {
                            aValueTextPameter.Value = DBNull.Value;
                        }
                    }
                    else
                    {
                        aValueTextPameter.Value = Converter.ToSqlString(row[columnClrDto.ColumnName]);
                    }

                    string sqlpdmUserDefineEntityRowValue = @"

                         Insert into    dbo.pdmUserDefineEntityRowValue
                                  (
                                        RowID,
                                        UserDefineEntityColumnID,
                                        ValueID,
                                        ValueDate,
                                        ValueText

                                   )  " +
                              @" values
                                (
                                 @RowID,
                                 @UserDefineEntityColumnID,
                                 @ValueID,
                                 @ValueDate,
                                 @ValueText

                                )";

                    CLROutput.OutputDebug("sqlpdmUserDefineEntityRowValue" + sqlpdmUserDefineEntityRowValue);

                    //The parameterized query '(@RowID int,@UserDefineEntityColumnID int,@ValueID int,@ValueDat' expects the parameter '@ValueText', which was not supplied.

                    cmdInsertCell.Connection = conn;
                    cmdInsertCell.CommandText = sqlpdmUserDefineEntityRowValue;

                    cmdInsertCell.Parameters.Add(aRowIdPameter);
                    cmdInsertCell.Parameters.Add(aUserDefineColumnIdIdPameter);
                    cmdInsertCell.Parameters.Add(aValueIdPameter);
                    cmdInsertCell.Parameters.Add(aValueDatePameter);
                    cmdInsertCell.Parameters.Add(aValueTextPameter);

                    cmdInsertCell.ExecuteNonQuery();
                }
            }
        }

        // if there mutiple PK , need to add more
        private static string GetOnePrimaryKeyColumnValue(DataRow row, List<PdmEntityColumnClrUserDefineDto> userDefineColumns)
        {
            string keyCombineValue = string.Empty;
            List<string> keyColumnName = userDefineColumns.Select(o => o.SystemTableColumnName ).ToList();

            foreach (string pkColumn in keyColumnName)
            {
                keyCombineValue += row[pkColumn].ToString() + "_";
            }

            keyCombineValue = keyCombineValue.Substring(0, keyCombineValue.Length - 1);

            return keyCombineValue;
        }

        private static Dictionary<string, List<PdmEntityColumnClrUserDefineDto>> GetErpExDatabaseTableNameAndStructure()
        {
            Dictionary<string, List<PdmEntityColumnClrUserDefineDto>> dictExchangeTableColumnDto = new Dictionary<string, List<PdmEntityColumnClrUserDefineDto>>();
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
            {
                conn.Open();

                string QueryExchangeTable = @"select distinct Left(  sysobj .Name , 50 ) AS [ExChangeTableName] from sysObjects  as sysobj
            inner join dbo.sysColumns  as sysColumn on sysColumn.ID =  sysobj .ID
            where    ( sysobj .name not like '%sysdiagrams%')  and sysobj.xtype ='U' ";

                CLROutput.OutputDebug("QueryExchangeTable=" + QueryExchangeTable);

                DataTable exchangeResult = DataAcessHelper.GetDataTableQueryResult(conn, QueryExchangeTable);

                List<string> exChangeDbTableList = exchangeResult.AsDataRowEnumerable().Select(o => (o["ExChangeTableName"] as string).Trim().ToLowerInvariant()).ToList();

                foreach (string exchangeTable in exChangeDbTableList)
                {
                    List<PdmEntityColumnClrUserDefineDto> exEntituyColumn = PLMSEntityClrBL.GetErpExchangeDatabaseTableColumnDto(conn, exchangeTable);
                    var pkColumn = exEntituyColumn.Where(o => o.IsPrimaryKey.HasValue && o.IsPrimaryKey.Value).FirstOrDefault();

                    // must have PK column in -Ex-change table
                    if (pkColumn != null)
                    {
                        dictExchangeTableColumnDto.Add(exchangeTable, exEntituyColumn);
                    }
                    else
                    {
                       // throw new Exception(exchangeTable + " has no Primary key Column");
                    }
                }
            }

            //Dictionary<string, List<PdmEntityColumnClrUserDefineDto>> dictPrefixErpExchangeTableNameAndColumnDto = new Dictionary<string, List<PdmEntityColumnClrUserDefineDto>>();
            //foreach (var pair in dictExchangeTableColumnDto)
            //{
            //    string entityCode = (PLMConstantString.EX_PLM_Import_Prefix + pair.Key).ToLowerInvariant();
            //    dictPrefixErpExchangeTableNameAndColumnDto.Add(entityCode, pair.Value);
            //}

            return dictExchangeTableColumnDto;
        }

        private static Dictionary<string, List<string>> CheckErpTableHasMutiplePrimayKey(List<string> erpExChangeDbTableList)
        {
            Dictionary<string, List<string>> dictErpTableMutipleKey = new Dictionary<string, List<string>>();

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
            {
                conn.Open();
                foreach (string erpTableName in erpExChangeDbTableList)
                {
                    List<string> mutipleKey = PLMSEntityClrBL.GetPrimarykeyList(conn, erpTableName);

                    if (mutipleKey.Count > 1)
                    {
                        // SqlContext.Pipe.Send("erpTableName mutipleKey=" + mutipleKey.Aggregate((i, j) => i + ";" + j));
                        dictErpTableMutipleKey.Add(erpTableName, mutipleKey);
                    }
                }
            }

            return dictErpTableMutipleKey;
        }

        private static List<string> GetPLMExistingImportTable(SqlConnection conn)
        {
            string querExistingImportTableInPLm = string.Format(@" select EntityCode  from pdmentity where  IsImport =1  and EntityType = "+(int) EmEntityType.UserDefineTable +"    and EntityCode like '{0}%'   ", PLMConstantString.EX_PLM_Import_Prefix);
            SqlCommand cmdGetExistingTable = new SqlCommand(querExistingImportTableInPLm, conn);

            var datatable = DataAcessHelper.GetDataTableQueryResult(conn, querExistingImportTableInPLm);
            List<string> plmExistingEntityList = datatable.GetDistinctOneColumnStringValue("EntityCode").Select(o => o.ToLowerInvariant()).ToList();
            return plmExistingEntityList;
        }

        private static string GetImportDefaultFolderId(SqlConnection conn)
        {
            string queryGetImportFolderId = " SELECT SetupValue FROM pdmsetup WHERE setupCode='ImportEntityFolder'";
            SqlCommand cmdGetFodlerId = new SqlCommand(queryGetImportFolderId, conn);
            string folderId = cmdGetFodlerId.ExecuteScalar().ToString();
            return folderId;
        }

        private static object InsertNewExportUserDefineEntity(string folderId, SqlConnection conn, string newEntityCode, string sysTableName)
        {
            string sqlInsertPdmEntity = string.Format(@"

                         Insert into    dbo.PdmEntity
                                  (
                                        EntityCode,
                                        FolderID,
                                        IsImport,
                                        EntityType,
                                        SysTableName

                                   )  " +
                      @" values
                                (
                                 '{0}',{1},{2},{3},'{4}'

                                )
                                    ", newEntityCode, folderId, 1, (int)EmEntityType.UserDefineTable, sysTableName);

            using (SqlCommand insertNewentity = new SqlCommand(sqlInsertPdmEntity, conn))
            {
                CLROutput.OutputDebug("sqlInsertPdmEntity" + sqlInsertPdmEntity);
                insertNewentity.ExecuteNonQuery();
            }

            string newEntityIdQuery = @"select EntityID  from pdmEntity  where entitycode='" + newEntityCode + "' and EntityType = " + (int)EmEntityType.UserDefineTable ;
            using (SqlCommand cmdGetNewEntityId = new SqlCommand(newEntityIdQuery, conn))
            {
                return cmdGetNewEntityId.ExecuteScalar(); ;
            }
        }

        private static void InsertPdmUserDefineColunmDto(PdmEntityColumnClrUserDefineDto pdmEntityColumnClrDto, SqlConnection conn)
        {
            int bitPrimaryKey = 0;
            if (pdmEntityColumnClrDto.IsPrimaryKey.HasValue && pdmEntityColumnClrDto.IsPrimaryKey.Value)
            {
                bitPrimaryKey = 1;
            }

            int NBDecimal = 0;
            if (pdmEntityColumnClrDto.Nbdecimal.HasValue)
            {
                NBDecimal = pdmEntityColumnClrDto.Nbdecimal.Value;
            }

            int bitsedByDropDownList = 0;
            if (pdmEntityColumnClrDto.UsedByDropDownList.HasValue && pdmEntityColumnClrDto.UsedByDropDownList.Value)
            {
                bitsedByDropDownList = 1;
            }

            string sqlInsertPdmEntityColumn = string.Format(@"

                         Insert into    dbo.pdmUserDefineEntityColumn
                                  (
                                        EntityID,
                                        ColumnName,
                                        IsPrimaryKey,
                                        UIControlType,
                                         NBDecimal,
                                         UsedByDropDownList,
                                         SystemTableColumnName

                                   )  " +
                      @" values
                                (
                                 {0},'{1}',{2},{3},{4},{5},'{6}'

                                )
                                    ", pdmEntityColumnClrDto.EntityId, pdmEntityColumnClrDto.ColumnName, bitPrimaryKey, pdmEntityColumnClrDto.UicontrolType, NBDecimal, bitsedByDropDownList, pdmEntityColumnClrDto.ColumnName);

            using (SqlCommand insertNewentity = new SqlCommand(sqlInsertPdmEntityColumn, conn))
            {
                CLROutput.OutputDebug("sqlInsertPdmEntity" + sqlInsertPdmEntityColumn);
                insertNewentity.ExecuteNonQuery();
            }
        }
    }
}