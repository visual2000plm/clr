  using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace PLMCLRTools
{
    public partial class TestPLMSysTableExchnage
    {
      //  [Microsoft.SqlServer.Server.SqlProcedure]
        public static void TestSysDefineInCommingConnection()
        {
            //step1: Get all Sys mapping Tables from ex-Chang Dababae

            List<PdmERPTableMappingDto> mappingDtoList = GetPdmERPMappingDtoList();
            foreach (PdmERPTableMappingDto mappingTable in mappingDtoList)
            {
                PullNewRecordFromExTableToPLM(mappingTable);
            }
            // need to update FK depden column  

            foreach (PdmERPTableMappingDto mappingTable in mappingDtoList)
            {
                foreach (PdmERPTableColumnMappingDto fkColumn in mappingTable.MappingColumnList)
                {
                    if (fkColumn.FKEntityMappingID.HasValue)
                    {
                        UpdateForeignKeyColumnValueFromExchangeTables(mappingTable, fkColumn);
                    }
                
                }

            }


        }



      //  [Microsoft.SqlServer.Server.SqlProcedure]
        public static void TestSysDefineOutCommingConnection()
        {
            //step1: Get all Sys mapping Tables from ex-Chang Dababae

            List<PdmERPTableMappingDto> mappingDtoList = GetPdmERPMappingDtoList();
            foreach (PdmERPTableMappingDto mappingTable in mappingDtoList)
            {
                PushNewRecordFromPLMExTable(mappingTable); 
            }
            // need to update FK depden column

            //foreach (PdmERPTableMappingDto mappingTable in mappingDtoList)
            //{
            //    foreach (PdmERPTableColumnMappingDto fkColumn in mappingTable.MappingColumnList)
            //    {
            //        if (fkColumn.FKEntityMappingID.HasValue)
            //        {
            //            UpdateForeignKeyColumnValueFromExchangeTables(mappingTable, fkColumn);
            //        }

            //    }

            //}


        }

        private static void PushNewRecordFromPLMExTable(PdmERPTableMappingDto mappingTable)
        {
            string erpTable = "[" + mappingTable.EXTableName + "]";
           

            // Get ERP Data

            string getExTablePKAndPLMPrimayKeyMappingValue = "  SELECT " + mappingTable.EXPrimaryKeyColumn  + ", " + PLMConstantString.ExchangeRowDataPLMPrimayKeyColumn   + " FROM " + erpTable;

           // string erpWhereClause = string.Empty;
            string erpWhereClause = string.Format(@"        where    ExchangeRowDataSourceOfOriginal='{0}'   ",   PLMConstantString.PLM    );


          

            DataTable erpExchangeResult;
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
            {
                conn.Open();
                erpExchangeResult = DataAcessHelper.GetDataTableQueryResult(conn, getExTablePKAndPLMPrimayKeyMappingValue);
                CLROutput.SendDataTable(erpExchangeResult);
            }

            // Get Logical unique column, we can use logical uniq column to do batch insert (
            // unfortunenately , we cannot use BulkCopyDatatable becaseu we need  PK mapping key between ERP and PLM system

            //Dictionary<string,string> dataTableBulkcopyMappingcolumn = new Dictionary<string,string> ();
            //foreach (var columnDto in mappingTable.MappingColumnList)
            //{
            //    dataTableBulkcopyMappingcolumn.Add(columnDto.PLMColumn, columnDto.PLMColumn);
            //}
            //DataTableUtility.BulkCopyDatatableToDatabaseTable(conn, erpExchangeResult, mappingTable.PLMTableName, dataTableMappingcolumn);



            // need to Check ERP Data and PLM Data logical unique column
            if (!string.IsNullOrEmpty(mappingTable.EXLogicalUniqueColumn))
            {


            }


            Dictionary<object, DataRow> dictNewRowIdDataRow = new Dictionary<object, DataRow>();
            if (erpExchangeResult != null && erpExchangeResult.Rows.Count > 0)
            {
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                {
                    conn.Open();

                    foreach (DataRow row in erpExchangeResult.Rows)
                    {

                        object rowId = InsertDataRowErpRecordToPlmTable(mappingTable, row, conn);
                        //  CLROutput.Output("rowid=" + rowId); 
                        if (rowId != null)
                        {
                            dictNewRowIdDataRow.Add(rowId, row);
                        }
                    }

                }

                UpdateEx_PLM_ERP_ChangeTableMappingKeyAndLogStatus(mappingTable, dictNewRowIdDataRow);
            }
        }



        private static void UpdateForeignKeyColumnValueFromExchangeTables(PdmERPTableMappingDto mappingTable, PdmERPTableColumnMappingDto fkColumn)
        {


            string queryCurrentExchangeTableFkColumnAndPLMSPrimaryData = GetCurrentExchangeTableFkColumnAndPLMSPrimaryDataQuery(mappingTable, fkColumn);


            string getForeignKeyExchangeTableDataQuery = GetFKTablePrimaryKeyAndPLMSPrimaryColumnMappingQuery(fkColumn);



            DataTable erpExchangeResult;
            DataTable erpForeignExchangeResult;
            DataTable mappingCurrentTableFkTable=null;

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
            {
                conn.Open();
                erpExchangeResult = DataAcessHelper.GetDataTableQueryResult(conn, queryCurrentExchangeTableFkColumnAndPLMSPrimaryData);
                erpForeignExchangeResult = DataAcessHelper.GetDataTableQueryResult(conn, getForeignKeyExchangeTableDataQuery); 
                CLROutput.SendDataTable(erpExchangeResult);
                CLROutput.SendDataTable(erpForeignExchangeResult);
            }

            if (erpExchangeResult != null && erpForeignExchangeResult != null)
            {

                mappingCurrentTableFkTable = DataTableUtility.Join(erpExchangeResult, erpForeignExchangeResult, PLMConstantString.ExchangeRowDataForeignKeyColumn, PLMConstantString.ExchangeRowDataPrimaryKeyColumn);
                 CLROutput.SendDataTable(mappingCurrentTableFkTable);
              
            
            }


            if (mappingCurrentTableFkTable != null )
            {
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                {
                    conn.Open();

                    foreach (DataRow row in mappingCurrentTableFkTable.Rows)
                    {
                        string updateFKTableCmd = string.Format(
                                        @" update  {0} 
                                        set {1}={2}
                                        where {3}={4} ",
                                        mappingTable.PLMTableName,
                                        fkColumn.PLMColumn, row[PLMConstantString.FkTableExchangeRowDataPLMPrimayKeyColumn],
                                        mappingTable.PLMPrimaryKeyColumn, row[PLMConstantString.ExchangeRowDataPLMPrimayKeyColumn]
                                        
                                        );

                        CLROutput.OutputDebug("updateFKTableCmd=" + updateFKTableCmd);
                        SqlCommand upateCmd = new SqlCommand(updateFKTableCmd, conn);
                        upateCmd.ExecuteNonQuery(); 
                     
                    }

                }

               
            }
        }

        private static string GetCurrentExchangeTableFkColumnAndPLMSPrimaryDataQuery(PdmERPTableMappingDto mappingTable, PdmERPTableColumnMappingDto fkColumn)
        {
            string erpTable = "[" + mappingTable.EXTableName + "]";
            string mappingPlmPrimaryKeyColumn = "[" + PLMConstantString.ExchangeRowDataPLMPrimayKeyColumn + "] ";

            string currentTableForeignColumn = "[" + fkColumn.EXColumn + "] as [" + PLMConstantString.ExchangeRowDataForeignKeyColumn + "]";


            // Get ERP Data

            string getErpDataQuery = "  SELECT " + mappingPlmPrimaryKeyColumn + ", " + currentTableForeignColumn + " FROM " + erpTable;

            // CLROutput.Output("eroColumns=" + eroColumns);

            string erpWhereClause = string.Empty;
            //            string erpWhereClause = string.Format(@"        where
            //                                                                    {0}={1}
            //                                                                    and  ExchangeRowDataSourceOfOriginal='{2}'
            //                                                                    ",
            //                                                                    PLMConstantString.ExchangeRowDataERPFlagColumn, (int)EmExChangeActionType.New,
            //                                                                    PLMConstantString.ERP
            //                                                                    );

            getErpDataQuery = getErpDataQuery + erpWhereClause;
            return getErpDataQuery;
        }

        private static string GetFKTablePrimaryKeyAndPLMSPrimaryColumnMappingQuery(PdmERPTableColumnMappingDto fkColumn)
        {
            PdmERPTableMappingDto FkMappingTable = fkColumn.FKPdmERPTableMappingDto;

            string fkTAbleEpTable = "[" + FkMappingTable.EXTableName + "]";
     

            string FkTableExchangeRowDataPLMPrimayKeyColumnSelectField = "[" + PLMConstantString.ExchangeRowDataPLMPrimayKeyColumn + "] as [" + PLMConstantString.FkTableExchangeRowDataPLMPrimayKeyColumn  + "]";
            string fkTableMappingPLmColumns = "[" + FkMappingTable.EXPrimaryKeyColumn + "]  as [" + PLMConstantString.ExchangeRowDataPrimaryKeyColumn + "]";


            // Get ERP foregin TAble Data

            string getfkErpDataQuery = "  SELECT " + FkTableExchangeRowDataPLMPrimayKeyColumnSelectField + ", " + fkTableMappingPLmColumns + " FROM " + fkTAbleEpTable;
            return getfkErpDataQuery;
        }


        private static void PullNewRecordFromExTableToPLM(PdmERPTableMappingDto mappingTable)
        {
            string erpTable = "[" + mappingTable.EXTableName + "]";
            string mappingPlmPrimaryKeyColumn = "[" + mappingTable.EXPrimaryKeyColumn + "] as [" + mappingTable.PLMPrimaryKeyColumn + "]" ;

            string mappingPLmColumns = string.Empty;
            foreach (var columnDto in mappingTable.MappingColumnList)
            {
                mappingPLmColumns = mappingPLmColumns + "[" + columnDto.EXColumn + "] as [" + columnDto .PLMColumn + "],";
            }
            if (mappingPLmColumns != string.Empty)
            {
                mappingPLmColumns = mappingPLmColumns.Substring(0, mappingPLmColumns.Length - 1);
            }

            // Get ERP Data

            string getErpDataQuery = "  SELECT " + mappingPlmPrimaryKeyColumn + ", " + mappingPLmColumns + " FROM " + erpTable;

            // CLROutput.Output("eroColumns=" + eroColumns);

            string erpWhereClause = string.Format(@"        where
                                                                    {0}={1}
                                                                    and  ExchangeRowDataSourceOfOriginal='{2}'
                                                                    ",
                                                                    PLMConstantString.ExchangeRowDataERPFlagColumn, (int)EmExChangeActionType.New,
                                                                    PLMConstantString.ERP
                                                                    );

            getErpDataQuery = getErpDataQuery + erpWhereClause;

            DataTable erpExchangeResult;
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
            {
                conn.Open();
                erpExchangeResult = DataAcessHelper.GetDataTableQueryResult(conn, getErpDataQuery);
               CLROutput.SendDataTable(erpExchangeResult);
            }

            // Get Logical unique column, we can use logical uniq column to do batch insert (
            // unfortunenately , we cannot use BulkCopyDatatable becaseu we need  PK mapping key between ERP and PLM system

            //Dictionary<string,string> dataTableBulkcopyMappingcolumn = new Dictionary<string,string> ();
            //foreach (var columnDto in mappingTable.MappingColumnList)
            //{
            //    dataTableBulkcopyMappingcolumn.Add(columnDto.PLMColumn, columnDto.PLMColumn);
            //}
            //DataTableUtility.BulkCopyDatatableToDatabaseTable(conn, erpExchangeResult, mappingTable.PLMTableName, dataTableMappingcolumn);



            // need to Check ERP Data and PLM Data logical unique column
            if(! string.IsNullOrEmpty (mappingTable.EXLogicalUniqueColumn))
            {
                           

            }


            Dictionary<object, DataRow> dictNewRowIdDataRow = new Dictionary<object, DataRow>();
            if (erpExchangeResult != null && erpExchangeResult.Rows.Count > 0)
            {
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                {
                    conn.Open();

                    foreach (DataRow row in erpExchangeResult.Rows)
                    {

                        object rowId = InsertDataRowErpRecordToPlmTable( mappingTable, row, conn);
                        //  CLROutput.Output("rowid=" + rowId); 
                        if (rowId != null)
                        {
                            dictNewRowIdDataRow.Add(rowId, row);
                        }
                    }
                    
                }

                UpdateEx_PLM_ERP_ChangeTableMappingKeyAndLogStatus( mappingTable,  dictNewRowIdDataRow);
            }
        }

        private static void UpdateEx_PLM_ERP_ChangeTableMappingKeyAndLogStatus(PdmERPTableMappingDto mappingTable, Dictionary<object, DataRow> dictNewRowIdDataRow)
        {

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
            {
                conn.Open();

              
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
                                       mappingTable.EXTableName,
                                        PLMConstantString.ExchangeRowDataERPFlagColumn, (int)EmExChangeActionType.NoChange,
                                       PLMConstantString.ExchangeRowDataPLMImportDateTimeColumn,
                                       PLMConstantString.ExchangeRowDataPLMPrimayKeyColumn, rowId,
                                       mappingTable.EXPrimaryKeyColumn, row[mappingTable.PLMPrimaryKeyColumn],
                                       PLMConstantString.ExchangeRowDataERPFlagColumn, (int)EmExChangeActionType.New);

                    CLROutput.OutputDebug("updateExchangeStatus=" + updateExchangeStatus);
                    SqlCommand cmdUpdateExTableFromNewToNochnage = new SqlCommand(updateExchangeStatus, conn);
                    cmdUpdateExTableFromNewToNochnage.ExecuteNonQuery();
                }
            }
        
        }

        private static object InsertDataRowErpRecordToPlmTable(PdmERPTableMappingDto mappingTable, DataRow row, SqlConnection conn)
        {

          

            if ( string.IsNullOrEmpty(mappingTable.PLMPrimaryKeyColumn) || string.IsNullOrEmpty(mappingTable.EXPrimaryKeyColumn) )
                return null;



            //PdmEntityColumnClrDto afirstPdmEntityColumnClrDto = userDefineColumns[0];
            CLROutput.OutputDebug(" insert new ErpEchange TAble " + mappingTable.EXTableName + "   insert new plm TAble " + mappingTable.PLMTableName );


            // so assum all , need to set all FK entiyt null in orde to do batch
            var dictFKcolumns = mappingTable.MappingColumnList.Where(o => (o.FKEntityMappingID.HasValue)).ToDictionary(o=>o.PLMColumn,o=>"INT") ;

            DataAcessHelper.SetTableColumnIsNull(conn, mappingTable.PLMTableName, dictFKcolumns);
            




            var columnWithoutFkcolumns = mappingTable.MappingColumnList.Where(o => (!o.FKEntityMappingID.HasValue)).ToList(); 


            // Get keyValue
            string keyCombineValue = row[mappingTable.PLMPrimaryKeyColumn].ToString ();


            string mappingPLmColumns = string.Empty;
            string values = string.Empty;

            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            foreach (var columnDto in columnWithoutFkcolumns)
            {
                mappingPLmColumns = mappingPLmColumns + "[" + columnDto.PLMColumn + "] ,";
                values = values + "@" + columnDto.PLMColumn + ",";

                SqlParameter paramtercolumn = new SqlParameter("@" + columnDto.PLMColumn, row[columnDto.PLMColumn]);
                sqlParameterList.Add(paramtercolumn);
            }
            if (mappingPLmColumns != string.Empty)
            {   
                mappingPLmColumns = mappingPLmColumns.Substring(0, mappingPLmColumns.Length - 1);
                values = values.Substring(0, values.Length - 1);
            }


          

            string sqlInsertpdmEntityRow = string.Format(@" 
            
                                     Insert into    [dbo].[{0}]
                                              (
                                                            {1}
                                                    
                                                    
                                               )  " +
                                     @" values 
                                            (
                                                             
                                                             {2}
                                            )" ,

                                             mappingTable.PLMTableName  ,
                                             mappingPLmColumns,
                                             values
                                               
                                               );




            CLROutput.OutputDebug(" sqlInsertpdmEntityRow = " + sqlInsertpdmEntityRow);


            object rowID;
            using (SqlCommand cmdInsertNewentity = new SqlCommand())
            {

                foreach (var paramter in sqlParameterList)
                {
                    cmdInsertNewentity.Parameters.Add(paramter);
                }

                rowID = InsertNewEntityWithReturnNewIdentityId(conn, sqlInsertpdmEntityRow, cmdInsertNewentity);
               // InsertRowCellValue(row, conn, aPdmEntityClrDto, rowID);

            }


            return rowID;

          // throw new NotImplementedException();
        }

        private static List<PdmERPTableMappingDto> GetPdmERPMappingDtoList()
        {
            string queryllMappingTable = @"SELECT  [MappingID]
                      ,[PLMTableName]
                      ,[PLMPrimaryKeyColumn]
                      ,[PLMLogicalUniqueColumn]
                      ,[EXTableName]
                      ,[EXChangeMode]
                      ,[EXPrimaryKeyColumn]
                      ,[EXLogicalUniqueColumn],
                      [EXStartRootPKIDUsedByPLM]
                      FROM [pdmERPTableMapping] ";

            string queryllMappingTableColumn = @"SELECT
                [ColumnMappingID]
                ,[MappingID]
                ,[PLMColumn]
                ,[EXColumn]
                ,[FKEntityMappingID]
                ,[EXDataType]
                ,[EXDataLength]
                ,[PLMDataType]
                ,[PLMDataLength]
               ,[LastReadPLMTableTimeStamp]
               ,[LastReadExchangeTableTimeStamp]
               ,[ExchangeTableTimeStampColumnName]


               FROM [pdmERPTableColumnMapping]";

            List<PdmERPTableMappingDto> mappingTablelist = new List<PdmERPTableMappingDto>();
            List<PdmERPTableColumnMappingDto> mappingcolumnlist = new List<PdmERPTableColumnMappingDto>();

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                DataTable dtMappingTableResult = DataAcessHelper.GetDataTableQueryResult(conn, queryllMappingTable);

                foreach (DataRow row in dtMappingTableResult.Rows)
                {
                    PdmERPTableMappingDto aPdmERPTableMappingDto = PLMDataRowConverter.ConvertPdmErpMappingTableToErpMappingTableDto(row);
                    mappingTablelist.Add(aPdmERPTableMappingDto);
                }

                DataTable dtMappingTableColumnResult = DataAcessHelper.GetDataTableQueryResult(conn, queryllMappingTableColumn);

                foreach (DataRow row in dtMappingTableColumnResult.Rows)
                {
                    PdmERPTableColumnMappingDto aPdmERPTableMappingColumnDto = PLMDataRowConverter.ConvertPdmERPTableColumnMappingToDto(row);
                    mappingcolumnlist.Add(aPdmERPTableMappingColumnDto);
                }
            }

            var groupappingcolumnlist = from row in mappingcolumnlist
                                        group row by new
                                        {
                                            mappingId = row.MappingID
                                        } into grp

                                        select new
                                        {
                                            MappingID = grp.Key.mappingId,
                                            Mappingcolumnlist = grp.Select(r => r).ToList(),
                                        };

            Dictionary<int, List<PdmERPTableColumnMappingDto>> dictMappingEntityandColumn = groupappingcolumnlist.ToDictionary(o => o.MappingID, o => o.Mappingcolumnlist);

            foreach (PdmERPTableMappingDto mappingTable in mappingTablelist)
            {
                if (dictMappingEntityandColumn.ContainsKey(mappingTable.MappingID))
                {
                    mappingTable.MappingColumnList = dictMappingEntityandColumn[mappingTable.MappingID];
                    foreach (PdmERPTableColumnMappingDto columnDto in mappingTable.MappingColumnList)
                    {
                        if (columnDto.FKEntityMappingID.HasValue)
                        {

                            columnDto.FKPdmERPTableMappingDto = mappingTablelist.FirstOrDefault(o => o.MappingID == columnDto.FKEntityMappingID.Value);
                        }
 
                    
                    }
                }
            }

            foreach (PdmERPTableMappingDto mappingTable in mappingTablelist)
            {
                CLROutput.OutputDebug("mappingTable.PLMTableName" + mappingTable.PLMTableName);
                foreach (var column in mappingTable.MappingColumnList)
                {
                    CLROutput.OutputDebug("mappingTable.MappingColumnList PLMColumn" + column.PLMColumn);
                    CLROutput.OutputDebug("mappingTable.MappingColumnList ERPColumn" + column.EXColumn);
                    CLROutput.OutputDebug("mappingTable.MappingColumnList FKEntityMappingID" + column.FKEntityMappingID);

                    if (column.FKEntityMappingID.HasValue)
                    {

                        CLROutput.OutputDebug("FKEntityMappingID table ???" + column.FKPdmERPTableMappingDto.EXTableName );
                    }
                }
            }

            return mappingTablelist;
        }

        public static void TestUserDefineExchangeDBConnection()
        {
            //step1: Get all Ex-change Tables from ex-Chang Dababae

            Dictionary<string, List<PdmEntityColumnClrUserDefineDto>> dictErpExchangeTableColumnDto = new Dictionary<string, List<PdmEntityColumnClrUserDefineDto>>();

            List<string> erpExChangeDbTableList = GetErpExDatabaseTableNameAndStructure(dictErpExchangeTableColumnDto);

            //  SqlContext.Pipe.Send("Aggregate TAble erpExChangeDbTableList:" + erpExChangeDbTableList.Aggregate((i, j) => i + ";" + j));

            Dictionary<string, List<string>> dictErpTableMutipleKey = CheckErpTableHasMutiplePrimayKey(erpExChangeDbTableList);
            if (dictErpTableMutipleKey.Count > 0)
            {
                SqlContext.Pipe.Send("some ERP tables have mutiple primary Key, need to add surrogate key ");
                foreach (string erptable in dictErpTableMutipleKey.Keys)
                {
                    SqlContext.Pipe.Send(erptable + "Primary key columns:" + dictErpTableMutipleKey[erptable].Aggregate((i, j) => i + ";" + j));
                }

                return;
            }

            List<string> commonEntityBetwennErpAndPlmList = new List<string>();

            // step2-2 Get common table

            CheckErpAndPLMStrcutureAndAddNewExEntity(erpExChangeDbTableList, commonEntityBetwennErpAndPlmList, dictErpExchangeTableColumnDto);
            SqlContext.Pipe.Send("Add new table structure done");

            //!!!! Get all Refresh Userdefinetable strcute from PLMS

            string queryllEntityImport = @"  select  EntityID from pdmEntity where IsImport =1 and   EntityType = " + (int)EmEntityType.UserDefineTable;

           
            List<int> enityIds;
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                DataTable dtresult = DataAcessHelper.GetDataTableQueryResult(conn, queryllEntityImport);

                 enityIds = dtresult.GetDistinctOneColumnValueIds("EntityID");

               // plmImportEntityDtoList = PLMSEntityClrBL.GetEntityAndColumnStrcutureInfoList(enityIds, conn);
            }

            List<PdmClrEntitySimpleStructureDto> plmImportEntityDtoList = PLMSEntityClrBL.GetEntityAndColumnStrcutureInfoList(enityIds);

            var dictPlmImportedTableColumnDto = plmImportEntityDtoList.ToDictionary(o => o.EntityCode.ToLowerInvariant(), o => o.Columns);

            // SqlContext.Pipe.Send("done:after check common tables structure ");

            //step3: Get commonTab different strcuture

            List<string> notSameStructureList = new List<string>();
            foreach (string commtable in commonEntityBetwennErpAndPlmList)
            {
                bool isSamestrut = IsSameStrcuture(commtable, dictErpExchangeTableColumnDto, dictPlmImportedTableColumnDto);

                if (!isSamestrut)
                {
                    notSameStructureList.Add(commtable);
                }
            }

            SqlContext.Pipe.Send("done:check common tables structure ");

            // need to transfer Data block

            if (notSameStructureList.Count > 0)
            {
                SqlContext.Pipe.Send("Erp and PLM  structure not match ");

                foreach (string changestrcuture in notSameStructureList)
                {
                    SqlContext.Pipe.Send(changestrcuture + " structure is changed ");
                }
            }

            else
            {
                //foreach (var exh in plmImportEntityDtoList)
                //{
                //    CLROutput.Output("exchangeTableNameList=" + exh);
                //}

                //Step4 Transfer NewRecord FromERP To PLM
                TransferNewRecordFromERPToPLM(plmImportEntityDtoList);

                SqlContext.Pipe.Send(" done: Transfer NewRecord To PLM ");

                ////Step5; upadte modfied fileds
                UpdateModifyRecordFromERPToPLM(plmImportEntityDtoList);

                SqlContext.Pipe.Send(" done:  Update Modify Record To PLM");

                //ste5: Delete--toDO

                //Step 6: update FK relation Ship
                UpdateForeignKeyRelationFromERPToPLM(plmImportEntityDtoList);
                SqlContext.Pipe.Send(" done:  Update ForeignKey Relation");
            }
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

            List<string> exchangeTableNameList = plmImportEntityDtoList.Select(o => o.EntityCode).ToList();

            Dictionary<string, PdmClrEntitySimpleStructureDto> dictAllImportEntity = plmImportEntityDtoList.ToDictionary(o => o.EntityCode.ToLowerInvariant(), o => o);

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

                PdmClrEntitySimpleStructureDto ChildEntityWithFKEntityDto = dictAllImportEntity[childEntityEntityName];

                //  CLROutput.Output("after EntityWithFKEntityDto=" + EntityWithFKEntityDto.EntityCode);

                Dictionary<string, string> dictErpChildEntityPKAndFKInExTableValue = null;
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
                {
                    conn.Open();

                    //   string pkColumn = EntityWithFKEntityDto.Columns.Where(o => o.IsPrimaryKey.HasValue && o.IsPrimaryKey.Value).First ().ColumnName ;
                    //
                    //ToDO: need to imporve mutiple Primay in one Table
                    var column = ChildEntityWithFKEntityDto.PrimaryKeyColumn;

                    //  SqlContext.Pipe.Send("ChildEntityWithFKEntityDto=" + ChildEntityWithFKEntityDto.EntityCode );

                    if (column != null)
                    {
                        string pkColumn = column[0].ColumnName;

                        CLROutput.OutputDebug("rowCount=" + rowCount + "childEntityEntityName=" + childEntityEntityName + " Childtable PK Column=" + pkColumn + " childFkColumnName=" + childFkColumnName + " masterEntityTableName=" + masterEntityTableName + " masterKeyName=" + masterKeyName);
                        rowCount++;

                        string querytchildEntityPKAndFkValue = string.Format(@" select {0},{1} from {2} ", pkColumn, childFkColumnName, ChildEntityWithFKEntityDto.EntityCode);

                        // SqlContext.Pipe.Send("querytchildEntityPKAndFkValue=" + querytchildEntityPKAndFkValue);

                        DataTable ErpChildEntityPKAndFKInExTable = DataAcessHelper.GetDataTableQueryResult(conn, querytchildEntityPKAndFkValue);

                        dictErpChildEntityPKAndFKInExTableValue = ErpChildEntityPKAndFKInExTable.AsDataRowEnumerable().ToDictionary(o => o[pkColumn].ToString(), o => o[childFkColumnName].ToString());
                    }
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
                    Dictionary<string, int> dictMasterEntityErpPrimayKeyAsValueTextAndRowID = MasterEntityErpPrimayKeyDataTable.AsDataRowEnumerable().Where(o => ((o[constValueText] as string) != null))
                        .ToDictionary(o => (string)o[constValueText], o => (int)o[constRowID]);

                    foreach (var childFKColumn in ChildEntityWithFKEntityDto.Columns)
                    {
                        if (childFKColumn.ColumnName.ToLowerInvariant() == childFkColumnName.ToLowerInvariant())
                        {
                            string getchildentityFkUserDefineValue = string.Format(@"select {0},{1}  from pdmUserDefineEntityRow where EntityID ={2}",
                            constValueText, constRowID, ChildEntityWithFKEntityDto.EntityId);

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
                    foreach (var childFKColumn in ChildEntityWithFKEntityDto.Columns)
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

                    InsertEntityWithFKRelaship(ChildEntityWithFKEntityDto, childFkColumnName, masterEntityEntityDto, conn);
                }
            }
        }

        //?????????? TODO list....
        private static void InsertEntityWithFKRelaship(PdmClrEntitySimpleStructureDto EntityWithFKEntityDto, string EntityWithFKEntityForeighKeyColumn, PdmClrEntitySimpleStructureDto masterEntityEntityDto, SqlConnection conn)
        {
            // EntityWithFKEntityDto.EntityId, Both PrimaryKeyColumn.UserDefineEntityColumnID and MasterEntityColumn.UserDefineEntityColumnID are in the same EntityWithFKEntityDto

            string masterEntityTableName = masterEntityEntityDto.EntityCode;

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

        private static void TransferNewRecordFromERPToPLM(List<PdmClrEntitySimpleStructureDto> plmImportEntityDtoList)
        {
            foreach (PdmClrEntitySimpleStructureDto aPdmEntityClrDto in plmImportEntityDtoList)
            {
                string exchangeTableName = aPdmEntityClrDto.EntityCode;
                DataTable newRecordsTable = null;

                //Get Ex-change new Row Data
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
                {
                    conn.Open();

                    string queryERPNewRecord = string.Format(@" select * from  {0}
                                                                    where  {1}={2}
                                                                    and  ExchangeRowDataSourceOfOriginal='{3}'
                                                                    ", aPdmEntityClrDto.EntityCode,
                                                                     PLMConstantString.ExchangeRowDataERPFlagColumn,
                                                                     (int)EmExChangeActionType.New,
                                                                     PLMConstantString.ERP
                                                                     );

                    newRecordsTable = DataAcessHelper.GetDataTableQueryResult(conn, queryERPNewRecord);

                    //  CLROutput.SendDataTable(newRecordsTable);
                }

                // insert to PLM
                Dictionary<object, DataRow> dictNewRowIdDataRow = new Dictionary<object, DataRow>();
                List<PdmEntityColumnClrUserDefineDto> userDefineColumns = aPdmEntityClrDto.Columns;
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                {
                    conn.Open();
                    foreach (DataRow row in newRecordsTable.Rows)
                    {
                        object rowId = InsertPdmUserDefineEntityRowAndCellValue(aPdmEntityClrDto, row, conn);

                        //  CLROutput.Output("rowid=" + rowId);
                        if (rowId != null)
                        {
                            dictNewRowIdDataRow.Add(rowId, row);
                        }
                    }
                }

                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
                {
                    conn.Open();

                    var erpPKColumnDto = aPdmEntityClrDto.Columns.Where(o => o.IsPrimaryKey.HasValue && o.IsPrimaryKey.Value).FirstOrDefault();
                    if (erpPKColumnDto == null)
                    {
                        return;
                    }

                    string erpPKColumn = erpPKColumnDto.ColumnName;
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

                        CLROutput.OutputDebug("updateExchangeStatus=" + updateExchangeStatus);
                        SqlCommand cmdUpdateExTableFromNewToNochnage = new SqlCommand(updateExchangeStatus, conn);
                        cmdUpdateExTableFromNewToNochnage.ExecuteNonQuery();
                    }
                }

                // Update Ex-change Tabl
            }

            //select CURRENT_TIMESTAMP
            // SELECT {fn NOW()}
            //  CURRENT_TIMESTAMP
            // SELECT GETDATE() ",

            //System.InvalidOperationException: Sequence contains no elements
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
            string keyCombineValue = GetOnePrimaryKeyColumnValue(row, userDefineColumns);

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
            List<string> keyColumnName = userDefineColumns.Where(o => o.IsPrimaryKey.HasValue && o.IsPrimaryKey.Value).Select(o => o.ColumnName).ToList();

            foreach (string pkColumn in keyColumnName)
            {
                keyCombineValue = keyCombineValue + row[pkColumn].ToString();
            }
            return keyCombineValue;
        }

       

        private static List<string> GetErpExDatabaseTableNameAndStructure(Dictionary<string, List<PdmEntityColumnClrUserDefineDto>> dictExchangeTableColumnDto)
        {
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
            {
                conn.Open();

                string QueryExchangeTable = string.Format(@" select distinct Left(  sysobj .Name , 50 ) AS [ExChangeTableName] from sysObjects  as sysobj
            inner join dbo.sysColumns  as sysColumn on sysColumn.ID =  sysobj .ID
            where    ( sysobj .name not like '%sysdiagrams%' and   sysobj .name like '{0}%' ) and sysColumn.Name in ( '{1}', '{2}' ,'{3}' ,'{4}' ,'{5}')",
                                                                                           PLMConstantString.EX_PLM_Import_Prefix,
                                                                                           PLMConstantString.ExchangeRowDataERPFlagColumn,
                                                                                           PLMConstantString.ExchangeRowDataPLMFlagColumn,
                                                                                           PLMConstantString.ExchangeRowDataERPExportDateTimeColumn,
                                                                                           PLMConstantString.ExchangeRowDataPLMImportDateTimeColumn,
                                                                                           PLMConstantString.ExchangeRowDataPLMPrimayKeyColumn

                                                                                           );

                // CLROutput.Output("QueryExchangeTable=" + QueryExchangeTable);

                DataTable exchangeResult = DataAcessHelper.GetDataTableQueryResult(conn, QueryExchangeTable);

                List<string> exChangeDbTableList = exchangeResult.AsDataRowEnumerable().Select(o => (o["ExChangeTableName"] as string).Trim().ToLowerInvariant()).ToList();

                foreach (string exchangeTable in exChangeDbTableList)
                {
                    List<PdmEntityColumnClrUserDefineDto> exEntituyColumn = PLMSEntityClrBL.GetErpExchangeDatabaseTableColumnDto(conn, exchangeTable);

                    var pkColumn = exEntituyColumn.Where(o => o.IsPrimaryKey.HasValue && o.IsPrimaryKey.Value).FirstOrDefault();
                    if (pkColumn != null)
                    {
                        dictExchangeTableColumnDto.Add(exchangeTable, exEntituyColumn);
                    }
                }
            }
            return dictExchangeTableColumnDto.Keys.ToList();
        }

        private static void CheckErpAndPLMStrcutureAndAddNewExEntity(List<string> erpExChangeDbTableList, List<string> commonEntityBetwennErpAndPlmList, Dictionary<string, List<PdmEntityColumnClrUserDefineDto>> dictExchangeTableColumnDto)
        {
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                string folderId = GetImportDefaultFolderId(conn);

                List<string> plmExistingEntityList = GetPLMExistingImportTable(conn);

                commonEntityBetwennErpAndPlmList.AddRange(erpExChangeDbTableList.Intersect(plmExistingEntityList));

                var newEntityList = erpExChangeDbTableList.Except(plmExistingEntityList);

                foreach (string newEntity in newEntityList)
                {
                    object newEntityID = InsertNewExportUserDefineEntity(folderId, conn, newEntity);

                    // need to insert table as well

                    List<PdmEntityColumnClrUserDefineDto> columnDtoList = dictExchangeTableColumnDto[newEntity];

                    foreach (PdmEntityColumnClrUserDefineDto pdmEntityColumnClrDto in columnDtoList)
                    {
                        pdmEntityColumnClrDto.EntityId = (int)newEntityID;
                        InsertPdmUserDefineColunmDto(pdmEntityColumnClrDto, conn);
                    }
                }
            }
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
            string querExistingImportTableInPLm = string.Format(@" select EntityCode  from pdmentity where  IsImport =1  and EntityType = " + (int)EmEntityType.UserDefineTable + "    and EntityCode like '{0}%'   ", PLMConstantString.EX_PLM_Import_Prefix);
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

        private static object InsertNewExportUserDefineEntity(string folderId, SqlConnection conn, string newEntityCode)
        {
            string sqlInsertPdmEntity = string.Format(@"

                         Insert into    dbo.PdmEntity
                                  (
                                        EntityCode,
                                        FolderID,
                                        IsImport,
                                        EntityType
                                   )  " +
                      @" values
                                (
                                 '{0}',{1},{2},{3}

                                )
                                    ", newEntityCode, folderId, 1, (int)EmEntityType.UserDefineTable);

            using (SqlCommand insertNewentity = new SqlCommand(sqlInsertPdmEntity, conn))
            {
                CLROutput.OutputDebug("sqlInsertPdmEntity" + sqlInsertPdmEntity);
                insertNewentity.ExecuteNonQuery();
            }
            //EntityType = "+(int) EmEntityType.UserDefineTable +"
            string newEntityIdQuery = @"select EntityID  from pdmEntity  where entitycode='" + newEntityCode + "' and EntityType = " + (int)EmEntityType.UserDefineTable;
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
                                         UsedByDropDownList

                                   )  " +
                      @" values
                                (
                                 {0},'{1}',{2},{3},{4},{5}

                                )
                                    ", pdmEntityColumnClrDto.EntityId, pdmEntityColumnClrDto.ColumnName, bitPrimaryKey, pdmEntityColumnClrDto.UicontrolType, NBDecimal, bitsedByDropDownList);

            using (SqlCommand insertNewentity = new SqlCommand(sqlInsertPdmEntityColumn, conn))
            {
                CLROutput.OutputDebug("sqlInsertPdmEntity" + sqlInsertPdmEntityColumn);
                insertNewentity.ExecuteNonQuery();
            }
        }
    }
}