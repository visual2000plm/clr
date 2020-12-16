using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Linq;
using System.Collections.Generic;

namespace PLMCLRTools
{

    public enum EmExChangeActionType { New = 1, Modified = 2, Deleted = 3, NoChange = 4 };
    public partial class PLMSDataImport
    {

        public static readonly string PLM_ExChangeDatabase_ConnectionString = string.Empty;
        private static string _PLM_ExChangeDatabase_ServerAndDatabaseName = string.Empty;


        private static string PLM_ExChangeDatabase_ServerAndDatabaseName
        {
            get
            {
                if (_PLM_ExChangeDatabase_ServerAndDatabaseName == string.Empty)
                {
                    string plmExchangeServer = string.Empty;
                    string plmExchangeDataBase = string.Empty;

                    using (SqlConnection conn = new SqlConnection(PLM_ExChangeDatabase_ConnectionString))
                    {
                        conn.Open();
                        plmExchangeServer = conn.DataSource;
                        plmExchangeDataBase = conn.Database;
                    }

                    _PLM_ExChangeDatabase_ServerAndDatabaseName = "[" + plmExchangeServer + "]." + "[" + plmExchangeDataBase + "]." + "dbo.";
                }

                return _PLM_ExChangeDatabase_ServerAndDatabaseName;
            }
        }



        static PLMSDataImport()
        {
            string exChangeDBConn = " SELECT SetupValue FROM pdmsetup WHERE setupCode='PLMExChangeDatabaseConnection'";

            using (SqlConnection plmconn_from_context = new SqlConnection("context connection=true"))
            {
                SqlCommand cmd = new SqlCommand(exChangeDBConn, plmconn_from_context);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable resultTabel = new DataTable();
                adapter.Fill(resultTabel);

                if (resultTabel.Rows.Count > 0)
                {
                    PLM_ExChangeDatabase_ConnectionString = resultTabel.Rows[0]["SetupValue"].ToString();
                }
            }

        }




        // [ExChangeActionBy] [nchar](10) NULL,   
        //	[ExChangeActionType] [int] NULL,     
        //	[ExChangeActionDatetime] [datetime] NULL,


        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void TestExchangeDBConnection()
        {

            //step1: Get all Ex-change Tables from ex-Chang Dababae
            List<string> exChangeDbTableList = null;
            Dictionary<string, List<PdmEntityColumnClrDto>> dictExchangeTableColumnDto = new Dictionary<string, List<PdmEntityColumnClrDto>>();

            exChangeDbTableList = GetExistingExDatabaseTableNameAndStructure(exChangeDbTableList, dictExchangeTableColumnDto);

            //Step2: insert new Ex-change table 
            AddNewExchangeTableToPLMSUserDefineEntity(exChangeDbTableList, dictExchangeTableColumnDto);

            // step2-2 update strcutre


            // Get all Refresh Userdefinetable from PLMS

            string queryllEntityImport = @"  select  EntityID from pdmEntity where IsImport =1 and EntityID > 3000 ";

            List<PdmEntityClrDto> plmImportEntityDtoList = null;
            using (SqlConnection conn = new SqlConnection(PLMSDWStoredProcedures.PLM_APP_ConnectionString))
            {
                List<int> enityIds = DataAcessHelper.GetDataTableQueryResult(conn, queryllEntityImport).AsEnumerable().Select(o => (int)o["EntityID"]).ToList(); ;

                plmImportEntityDtoList = PLMSEntityClrBL.GetEntityAndColumnStrcutureInfoList(enityIds, conn);

            }



            //Step3 Transfer NewRecord FromERP To PLM
              TransferNewRecordFromERPToPLM(plmImportEntityDtoList);

            ////Step4; upadte modfied fileds
              UpdateModifyRecordFromERPToPLM(plmImportEntityDtoList);

            //ste5: Delete--toDO

            //Step 6: update FK relation Ship
            UpdateForeignKeyRelationFromERPToPLM(plmImportEntityDtoList);








        }

        private static void UpdateForeignKeyRelationFromERPToPLM(List<PdmEntityClrDto> plmImportEntityDtoList)
        {
            List<string> exchangeTableNameList = plmImportEntityDtoList.Select(o => o.EntityCode).ToList();

            Dictionary<string, PdmEntityClrDto> dictAllImportEntity = plmImportEntityDtoList.ToDictionary(o => o.EntityCode.ToLowerInvariant(), o => o);

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
            // WHERE     (OBJECT_NAME(FKC.parent_object_id))";

            string tableInClause = DataAcessHelper.GenerateColumnInClauseWithAndCondition(exchangeTableNameList, "OBJECT_NAME(FKC.parent_object_id)", false);



            queryForeignTable = queryForeignTable + " WHERE " + tableInClause;



            DataTable foreignKeyTable = null;


            using (SqlConnection conn = new SqlConnection(PLM_ExChangeDatabase_ConnectionString))
            {
                conn.Open();
                foreignKeyTable = DataAcessHelper.GetDataTableQueryResult(conn, queryForeignTable);
                CLROutput.SendDataTable(foreignKeyTable);

            }

            foreach (DataRow fkDataRow in foreignKeyTable.Rows)
            {
                string childEntityEntityName = (fkDataRow["TABLE_NAME"] as string).Trim().ToLowerInvariant();
                string childFkColumnName = (fkDataRow["FIELD_NAME"] as string).Trim().ToLowerInvariant();
                string masterEntityTableName = (fkDataRow["REFTABLE_NAME"] as string).Trim().ToLowerInvariant(); ;
                string masterKeyName = (fkDataRow["REFFIELD_NAME"] as string).Trim().ToLowerInvariant();

                // clear old relationship

                int childEntityId = dictAllImportEntity[childEntityEntityName].EntityId;
                int masterEntityId = dictAllImportEntity[masterEntityTableName].EntityId;


                // clear old  relationship   

                using (SqlConnection conn = new SqlConnection(PLMSDWStoredProcedures.PLM_APP_ConnectionString))
                {
                    conn.Open();
                    PdmEntityClrDto EntityWithFKEntityDto = dictAllImportEntity[childEntityEntityName];
                    // need to updat FK control Type
                    foreach (var column in EntityWithFKEntityDto.Columns)
                    {
                        if (column.ColumnName.ToLowerInvariant() == childFkColumnName.ToLowerInvariant())
                        {
                            string updatepdmUserDefineEntityColumn = string.Format(@"update pdmUserDefineEntityColumn 
                                        set UIControlType={0},
                                         FKEntityID ={1} 
                                        where UserDefineEntityColumnID ={2}", (int)EmControlType.DDL, masterEntityId, column.UserDefineEntityColumnID);
                            SqlCommand cmdupdate = new SqlCommand(updatepdmUserDefineEntityColumn, conn);

                            CLROutput.Output("updatepdmUserDefineEntityColumn=" + updatepdmUserDefineEntityColumn);

                            cmdupdate.ExecuteNonQuery();


                        }

                    }

                    InsertEntityWithFKRelaship(EntityWithFKEntityDto, childFkColumnName, masterEntityTableName, conn);

                }





            }

        }

        //?????????? TODO list....
        private static void InsertEntityWithFKRelaship(PdmEntityClrDto EntityWithFKEntityDto, string EntityWithFKEntityForeighKeyColumn, string masterEntityTableName, SqlConnection conn)
        {

            // EntityWithFKEntityDto.EntityId, Both PrimaryKeyColumn.UserDefineEntityColumnID and MasterEntityColumn.UserDefineEntityColumnID are in the same EntityWithFKEntityDto

            var PrimaryKeyColumn = EntityWithFKEntityDto.Columns.Where(o => o.IsPrimaryKey.HasValue && o.IsPrimaryKey.Value).First(); ;
            var MasterEntityColumn = EntityWithFKEntityDto.Columns.Where(o => o.ColumnName.ToLowerInvariant() == EntityWithFKEntityForeighKeyColumn.ToLowerInvariant()).First();



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

            CLROutput.Output("cmdGetRelationEntityId=" + cmdGetRelationEntityId);

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
                        IsRelationEntity 
                    )
                     values
                    ( 
                       '{0}',
                        {1}, 
                        {2},
                        {3},
                        {4},
                        {5},
                        {6} 
                          
                    )  ",
               "Import_Relation_" + masterEntityTableName + "_" + EntityWithFKEntityDto.EntityCode,
               1,
               (int)EmEntityType.RelationFKEntity,

               EntityWithFKEntityDto.EntityId,
               MasterEntityColumn.UserDefineEntityColumnID,
               PrimaryKeyColumn.UserDefineEntityColumnID,
               1
               );

                SqlCommand cmdInsertRelationEntityId = new SqlCommand(insertNewRaltionEntity, conn);

                CLROutput.Output("insertNewRaltionEntity=" + insertNewRaltionEntity);
                cmdInsertRelationEntityId.ExecuteNonQuery();

            }




            // need to add new RelatipnEntity

        }

        //        private static void InsertManyToManyRelationshipAndData(Dictionary<string, PdmEntityClrDto> dictAllImportEntity, string childEntityEntityName, string masterEntityTableName, int childEntityId, int masterEntityId, SqlConnection conn)
        //        {
        //            string queryRelationEntity = string.Format(@" select EntityID from  dbo.pdmentity where  ChildEntityID={0} and MasterEntityID={1}", childEntityId, masterEntityId);
        //            SqlCommand cmdGetRelationEntityId = new SqlCommand(queryRelationEntity, conn);
        //            object relationEntityId = cmdGetRelationEntityId.ExecuteScalar();

        //            if (relationEntityId != null)
        //            {

        //                string delete1 = @"delete  from  dbo.pdmEntityMasterChildValue  where RelationEntityID = " + relationEntityId;
        //                string delete2 = string.Format(@" delete  from  dbo.pdmentity where ChildEntityID={0} and MasterEntityID={1} and IsRelationEntity=1", childEntityId, masterEntityId);

        //                SqlCommand cmdDeleteRelationEntityId = new SqlCommand(delete1 + System.Environment.NewLine + delete2, conn);
        //                cmdDeleteRelationEntityId.ExecuteNonQuery();
        //            }
        //            string insertNewRaltionEntity = string.Format(@" insert into dbo.pdmentity
        //                    ( 
        //                        EntityCode,   
        //                        IsImport,
        //                        EntityType, 
        // 
        //                        EntityWithFKEntityID, 
        //                        MasterEntityColumnID ,
        //                        ChildEntityColumnID,
        //                        IsRelationEntity 
        //                    )
        //                     values
        //                    ( 
        //                        '{0}',
        //                        {1} 
        //                        {2},
        //                        {3},
        //                        {4},
        //                        {5} 
        //                    )  ", "Import_Relation_" + dictAllImportEntity[masterEntityTableName].EntityCode + "_" + dictAllImportEntity[childEntityEntityName].EntityCode,
        //                  childEntityId,
        //                  masterEntityId
        //                  );

        //            SqlCommand cmdInsertNewEntity = new SqlCommand();

        //            CLROutput.Output("insertNewRaltionEntity=" + insertNewRaltionEntity);
        //            object entityId = InsertNewEntityWithReturnNewIdentityId(conn, insertNewRaltionEntity, cmdInsertNewEntity);
        //        }

        private static void UpdateModifyRecordFromERPToPLM(List<PdmEntityClrDto> plmImportEntityDtoList)
        {


            foreach (PdmEntityClrDto aPdmEntityClrDto in plmImportEntityDtoList)
            {
                string exchangeTableName = aPdmEntityClrDto.EntityCode;
                DataTable modifyRecordsTable = null;

                //Get Ex-change new Row Data
                using (SqlConnection conn = new SqlConnection(PLM_ExChangeDatabase_ConnectionString))
                {
                    conn.Open();

                    string queryERPNewRecord = string.Format(@" select * from  {0} where  {1}={2} ", aPdmEntityClrDto.EntityCode, PLMConstantString.ExchangeRowDataERPFlagColumn, (int)EmExChangeActionType.Modified);

                    modifyRecordsTable = DataAcessHelper.GetDataTableQueryResult(conn, queryERPNewRecord);
                    //  CLROutput.SendDataTable(newRecordsTable);

                }


                using (SqlConnection conn = new SqlConnection(PLMSDWStoredProcedures.PLM_APP_ConnectionString))
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

                using (SqlConnection conn = new SqlConnection(PLM_ExChangeDatabase_ConnectionString))
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

        private static void UpdatePdmUserDefineEntityRow(PdmEntityClrDto aPdmEntityClrDto, DataRow row, SqlConnection conn)
        {




            //PdmEntityColumnClrDto afirstPdmEntityColumnClrDto = userDefineColumns[0];

            List<PdmEntityColumnClrDto> userDefineColumns = aPdmEntityClrDto.Columns;
            int entityID = aPdmEntityClrDto.EntityId;

            string sqlInsertpdmUserDefineEntityRow = string.Format(@" 

                         Insert into    dbo.pdmUserDefineEntityRow
                                  (
                                        EntityID
                                        
                                        
                                   )  " +
                      @" values 
                                (
                                 {0}
 
                                )
                                    ", entityID);

            using (SqlCommand cmdInsertNewentity = new SqlCommand())
            {

                cmdInsertNewentity.Connection = conn;
                cmdInsertNewentity.CommandText = sqlInsertpdmUserDefineEntityRow;



                CLROutput.Output("sqlInsertpdmUserDefineEntityRow" + sqlInsertpdmUserDefineEntityRow);
                cmdInsertNewentity.ExecuteNonQuery();



                cmdInsertNewentity.CommandText = "SELECT SCOPE_IDENTITY()";
                object rowID = cmdInsertNewentity.ExecuteScalar();

                CLROutput.Output("SELECT SCOPE_IDENTITY( ) RowId=" + rowID);




                foreach (PdmEntityColumnClrDto columnClrDto in userDefineColumns)
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
                    aValueTextPameter.Value = string.Empty;


                    // CLROutput.Output("SELECT SCOPE_IDENTITY( ) RowId=" + rowID);

                    CLROutput.Output("columnClrDto.ColumnName" + columnClrDto.ColumnName + "row[columnClrDto.ColumnName]" + row[columnClrDto.ColumnName]);

                    if (columnClrDto.UicontrolType == (int)EmControlType.DDL)
                    {
                        aValueIdPameter.Value = Converter.ToDDLSqlInt32(row[columnClrDto.ColumnName]);
                    }
                    else if (columnClrDto.UicontrolType == (int)EmControlType.DATE)
                    {

                        aValueDatePameter.Value = Converter.ToSqlDateTime(row[columnClrDto.ColumnName]);
                    }
                    else if (columnClrDto.UicontrolType == (int)EmControlType.CHKBOX)
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
                    }
                    else
                    {

                        aValueTextPameter.Value = Converter.ToString(row[columnClrDto.ColumnName]);
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


                    using (SqlCommand cmdInsertCell = new SqlCommand())
                    {
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




        }



        private static void TransferNewRecordFromERPToPLM(List<PdmEntityClrDto> plmImportEntityDtoList)
        {


            foreach (PdmEntityClrDto aPdmEntityClrDto in plmImportEntityDtoList)
            {
                string exchangeTableName = aPdmEntityClrDto.EntityCode;
                DataTable newRecordsTable = null;

                //Get Ex-change new Row Data
                using (SqlConnection conn = new SqlConnection(PLM_ExChangeDatabase_ConnectionString))
                {
                    conn.Open();

                    string queryERPNewRecord = string.Format(@" select * from  {0} where  {1}={2} ", aPdmEntityClrDto.EntityCode, PLMConstantString.ExchangeRowDataERPFlagColumn, (int)EmExChangeActionType.New);

                    newRecordsTable = DataAcessHelper.GetDataTableQueryResult(conn, queryERPNewRecord);
                    //  CLROutput.SendDataTable(newRecordsTable);

                }

                // insert to PLM
                Dictionary<object, DataRow> dictNewRowIdDataRow = new Dictionary<object, DataRow>();
                List<PdmEntityColumnClrDto> userDefineColumns = aPdmEntityClrDto.Columns;
                using (SqlConnection conn = new SqlConnection(PLMSDWStoredProcedures.PLM_APP_ConnectionString))
                {
                    conn.Open();
                    foreach (DataRow row in newRecordsTable.Rows)
                    {

                        object rowId = InsertPdmUserDefineEntityRow(aPdmEntityClrDto, row, conn);
                        //CLROutput.Output("rowid=" + rowId); 

                        dictNewRowIdDataRow.Add(rowId, row);

                    }


                }

                using (SqlConnection conn = new SqlConnection(PLM_ExChangeDatabase_ConnectionString))
                {
                    conn.Open();
                    //select CURRENT_TIMESTAMP
                    // SELECT {fn NOW()}
                    //  CURRENT_TIMESTAMP
                    // SELECT GETDATE() ",

                    string erpPKColumn = aPdmEntityClrDto.Columns.Where(o => o.IsPrimaryKey.HasValue && o.IsPrimaryKey.Value).First().ColumnName;

                    foreach (object rowId in dictNewRowIdDataRow.Keys)
                    {

                        DataRow row = dictNewRowIdDataRow[rowId];

                        string updateExchangeStatus = string.Format(@" 
                                               update  {0} 
                                               set  {1}={2} ,
                                               {3}=CURRENT_TIMESTAMP ,
                                               {4}='{5}'
                                               where {6}='{7}' 
                                                    and {8}={9}
                                                                ",
                                            aPdmEntityClrDto.EntityCode,
                                            PLMConstantString.ExchangeRowDataERPFlagColumn, (int)EmExChangeActionType.NoChange,
                                           PLMConstantString.ExchangeRowDataPLMImportDateTimeColumn,
                                           PLMConstantString.ExchangeRowDataPLMPrimayKeyColumn, rowId,
                                           erpPKColumn, GetOnePrimaryKeyColumnValue(row, aPdmEntityClrDto.Columns),
                                           PLMConstantString.ExchangeRowDataERPFlagColumn, (int)EmExChangeActionType.New);

                        CLROutput.Output("updateExchangeStatus=" + updateExchangeStatus);
                        SqlCommand cmdUpdateExTableFromNewToNochnage = new SqlCommand(updateExchangeStatus, conn);
                        cmdUpdateExTableFromNewToNochnage.ExecuteNonQuery();
                    }



                }

                // Update Ex-change Tabl

            }




        }



        // will return new RowID
        private static object InsertPdmUserDefineEntityRow(PdmEntityClrDto aPdmEntityClrDto, DataRow row, SqlConnection conn)
        {




            //PdmEntityColumnClrDto afirstPdmEntityColumnClrDto = userDefineColumns[0];

            List<PdmEntityColumnClrDto> userDefineColumns = aPdmEntityClrDto.Columns;
            int entityID = aPdmEntityClrDto.EntityId;

            // Get keyValue
            string keyCombineValue = GetOnePrimaryKeyColumnValue(row, userDefineColumns);

            //            string sqlInsertpdmUserDefineEntityRow = string.Format(@" 
            //
            //                         Insert into    dbo.pdmUserDefineEntityRow
            //                                  (
            //                                        EntityID,
            //                                        TextValue
            //                                        
            //                                        
            //                                   )  " +
            //                      @" values 
            //                                (
            //                                 {0},'{1}'
            // 
            //                                )
            //                                    ", entityID, keyCombineValue);


            string sqlInsertpdmUserDefineEntityRow = string.Format(@" 

                         Insert into    dbo.pdmUserDefineEntityRow
                                  (
                                        EntityID,
                                      
                                        
                                        
                                   )  " +
                      @" values 
                                (
                                 {0},'{1}'
 
                                )
                                    ", entityID);

            object rowID;
            using (SqlCommand cmdInsertNewentity = new SqlCommand())
            {

                rowID = InsertNewEntityWithReturnNewIdentityId(conn, sqlInsertpdmUserDefineEntityRow, cmdInsertNewentity);
                InsertRowCellValue(row, conn, aPdmEntityClrDto, rowID);

            }
            return rowID;




        }

        private static object InsertNewEntityWithReturnNewIdentityId(SqlConnection conn, string sqlInsertpdmUserDefineEntityRow, SqlCommand cmdInsertNewentity)
        {

            cmdInsertNewentity.Connection = conn;
            cmdInsertNewentity.CommandText = sqlInsertpdmUserDefineEntityRow;



            CLROutput.Output("sqlInsertpdmUserDefineEntityRow" + sqlInsertpdmUserDefineEntityRow);
            cmdInsertNewentity.ExecuteNonQuery();



            cmdInsertNewentity.CommandText = "SELECT SCOPE_IDENTITY()";
            object rowID = cmdInsertNewentity.ExecuteScalar();

            CLROutput.Output("SELECT SCOPE_IDENTITY( ) RowId=" + rowID);
            return rowID;
        }

        private static void InsertRowCellValue(DataRow row, SqlConnection conn, PdmEntityClrDto aPdmEntityClrDto, object rowID)
        {

            foreach (PdmEntityColumnClrDto columnClrDto in aPdmEntityClrDto.Columns)
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
                aValueTextPameter.Value = string.Empty;


                // CLROutput.Output("SELECT SCOPE_IDENTITY( ) RowId=" + rowID);

                CLROutput.Output("columnClrDto.ColumnName" + columnClrDto.ColumnName + "row[columnClrDto.ColumnName]" + row[columnClrDto.ColumnName]);

                if (columnClrDto.UicontrolType == (int)EmControlType.DDL)
                {
                    aValueIdPameter.Value = Converter.ToDDLSqlInt32(row[columnClrDto.ColumnName]);
                }
                else if (columnClrDto.UicontrolType == (int)EmControlType.DATE)
                {

                    aValueDatePameter.Value = Converter.ToSqlDateTime(row[columnClrDto.ColumnName]);
                }
                else if (columnClrDto.UicontrolType == (int)EmControlType.CHKBOX)
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
                }
                else
                {

                    aValueTextPameter.Value = Converter.ToString(row[columnClrDto.ColumnName]);
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


                using (SqlCommand cmdInsertCell = new SqlCommand())
                {
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
        private static string GetOnePrimaryKeyColumnValue(DataRow row, List<PdmEntityColumnClrDto> userDefineColumns)
        {
            string keyCombineValue = string.Empty;
            List<string> keyColumnName = userDefineColumns.Where(o => o.IsPrimaryKey.HasValue && o.IsPrimaryKey.Value).Select(o => o.ColumnName).ToList();

            foreach (string pkColumn in keyColumnName)
            {
                keyCombineValue = keyCombineValue + row[pkColumn].ToString();
            }
            return keyCombineValue;
        }





        private static List<string> GetExistingExDatabaseTableNameAndStructure(List<string> exChangeDbTableList, Dictionary<string, List<PdmEntityColumnClrDto>> dictExchangeTableColumnDto)
        {
            using (SqlConnection conn = new SqlConnection(PLM_ExChangeDatabase_ConnectionString))
            {
                conn.Open();

                string QueryExchangeTable = string.Format(@" select distinct Left(  sysobj .Name , 50 ) AS [ExChangeTableName] from sysObjects  as sysobj 
            inner join dbo.sysColumns  as sysColumn on sysColumn.ID =  sysobj .ID
            where   sysobj .name like '{0}%' and sysColumn.Name in ( '{1}', '{2}' ,'{3}' ,'{4}' ,'{5}')",
                                                                                           PLMConstantString.EX_PLM_Import_Prefix,
                                                                                           PLMConstantString.ExchangeRowDataERPFlagColumn,
                                                                                           PLMConstantString.ExchangeRowDataPLMFlagColumn,
                                                                                           PLMConstantString.ExchangeRowDataERPExportDateTimeColumn,
                                                                                           PLMConstantString.ExchangeRowDataPLMImportDateTimeColumn,
                                                                                           PLMConstantString.ExchangeRowDataPLMPrimayKeyColumn

                                                                                           );

                CLROutput.Output("QueryExchangeTable=" + QueryExchangeTable);

                exChangeDbTableList = DataAcessHelper.GetDataTableQueryResult(conn, QueryExchangeTable).AsEnumerable().Select(o => (o["ExChangeTableName"] as string).Trim().ToLowerInvariant()).ToList();



                foreach (string exchangeTable in exChangeDbTableList)
                {

                    List<PdmEntityColumnClrDto> exEntituyColumn = PLMSEntityClrBL.GetExchangeDatabaseTableColumnDto(conn, exchangeTable);
                    dictExchangeTableColumnDto.Add(exchangeTable, exEntituyColumn);



                }


            }
            return exChangeDbTableList;
        }

        private static void AddNewExchangeTableToPLMSUserDefineEntity(List<string> exChangeDbTableList, Dictionary<string, List<PdmEntityColumnClrDto>> dictExchangeTableColumnDto)
        {
            using (SqlConnection conn = new SqlConnection(PLMSDWStoredProcedures.PLM_APP_ConnectionString))
            {
                conn.Open();

                string queryGetImportFolderId = " SELECT SetupValue FROM pdmsetup WHERE setupCode='ImportEntityFolder'";
                SqlCommand cmdGetFodlerId = new SqlCommand(queryGetImportFolderId, conn);
                string folderId = cmdGetFodlerId.ExecuteScalar().ToString();

                string querExistingImportTableInPLm = @" select EntityCode  from pdmentity where  IsImport =1  and EntityID > 3000    and EntityCode like 'EX_%'   ";
                SqlCommand cmdGetExistingTable = new SqlCommand(querExistingImportTableInPLm, conn);
                // var existingEntityCode = DataAcessHelper.GetDataTableQueryResult(conn, querExistingImportTableInPLm).AsEnumerable().Select(o => o["EntityCode"]); 
                List<string> existingEntityList = DataAcessHelper.GetDataTableQueryResult(conn, querExistingImportTableInPLm).AsEnumerable().Select(o => o.Field<string>("EntityCode").Trim().ToLowerInvariant()).ToList();

                //  existingEntityList.ForEach(o => CLROutput.Output("PLmexistingEntityList=" + o));

                //exChangeDbTableList.ForEach(o => CLROutput.Output("exChangeDbTableList=" + o));


                var newEntityList = exChangeDbTableList.Except(existingEntityList);

                foreach (string newEntity in newEntityList)
                {
                    object newEntityID = InsertNewExportUserDefineEntity(folderId, conn, newEntity);
                    // need to insert table as well

                    List<PdmEntityColumnClrDto> columnDtoList = dictExchangeTableColumnDto[newEntity];

                    foreach (PdmEntityColumnClrDto pdmEntityColumnClrDto in columnDtoList)
                    {
                        pdmEntityColumnClrDto.EntityId = (int)newEntityID;
                        InsertPdmUserDefineColunmDto(pdmEntityColumnClrDto, conn);

                    }


                }
            }
        }

        private static object InsertNewExportUserDefineEntity(string folderId, SqlConnection conn, string newEntityCode)
        {

            string sqlInsertPdmEntity = string.Format(@" 

                         Insert into    dbo.PdmEntity
                                  (
                                        EntityCode,
                                        FolderID,
                                        IsImport
                                   )  " +
                      @" values 
                                (
                                 '{0}',{1},{2}
 
                                )
                                    ", newEntityCode, folderId, 1);

            using (SqlCommand insertNewentity = new SqlCommand(sqlInsertPdmEntity, conn))
            {
                CLROutput.Output("sqlInsertPdmEntity" + sqlInsertPdmEntity);
                insertNewentity.ExecuteNonQuery();
            }

            string newEntityIdQuery = @"select EntityID  from pdmEntity  where entitycode='" + newEntityCode + "' and EntityID > 3000 ";
            using (SqlCommand cmdGetNewEntityId = new SqlCommand(newEntityIdQuery, conn))
            {
                return cmdGetNewEntityId.ExecuteScalar(); ;
            }


        }

        private static void InsertPdmUserDefineColunmDto(PdmEntityColumnClrDto pdmEntityColumnClrDto, SqlConnection conn)
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


            string sqlInsertPdmEntityColumn = string.Format(@" 

                         Insert into    dbo.pdmUserDefineEntityColumn
                                  (
                                        EntityID,
                                        ColumnName,
                                        IsPrimaryKey,
                                        UIControlType,
                                        NBDecimal
                                        
                                   )  " +
                      @" values 
                                (
                                 {0},'{1}',{2},{3},{4}
 
                                )
                                    ", pdmEntityColumnClrDto.EntityId, pdmEntityColumnClrDto.ColumnName, bitPrimaryKey, pdmEntityColumnClrDto.UicontrolType, NBDecimal);

            using (SqlCommand insertNewentity = new SqlCommand(sqlInsertPdmEntityColumn, conn))
            {
                CLROutput.Output("sqlInsertPdmEntity" + sqlInsertPdmEntityColumn);
                insertNewentity.ExecuteNonQuery();
            }




        }
    }



}

