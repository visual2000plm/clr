using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;
using System.Linq; 

namespace PLMCLRTools
{
    //  Error	1	CREATE ASSEMBLY failed because method 'InitConnectionString'
    //  on type 'PLMCLRTools.StoredProcedures'  in safe assembly 'PDMUDF' is storing to a static field.
    //Storing to a static field is not allowed in safe assemblies.	PDMUDF

    // step1  ALTER DATABASE devlppdm SET TRUSTWORTHY ON
    // step2   set permission level External to CLR Project levle

            //update        pdmSetup   set setupvalue ='Data Source=srv-spider;Initial Catalog=PLMS_Devlp;User ID=sa;Password=visualdev' where 
            //          setupCode ='PLMAPPDBConnection'
                      
            //           update        pdmSetup   set setupvalue ='Data Source=srv-spider;Initial Catalog=PLMSDW_Devlp;User ID=sa;Password=visualdev' where 
            //          setupCode ='PLMDWDBConnection'


// SELECT DATEADD(Second,  -60 *   60, SYSUTCDATETIME())
 
//SELECT DATEADD(Second,  -60 *   60, GETUTCDATE())

//declare @SinceWhen datetime

//SELECT @SinceWhen= DATEADD(Second,   24* -60 *   60, GETDATE()) 

//print @SinceWhen
 

    public class PLMSDWStoredProcedures
    {
        private static Dictionary<int, string> _TabTableName = new Dictionary<int, string>();
        private static Dictionary<int, string> _GridTableName = new Dictionary<int, string>();
        private static Dictionary<int, string> _EntityTableName = new Dictionary<int, string>();
        private static readonly string SinceWhenPara = "@sinceWhen";


        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void PLMDW_CreateTable()
        {
            SqlContext.Pipe.Send("PLMDB=" + PLMConstantString.PLM_APP_ConnectionString);
            SqlContext.Pipe.Send("PLMDWDB=" + PLMConstantString.PLM_DW_ConnectionString);
            SqlContext.Pipe.Send("PLMDSwapDB=" + PLMConstantString.PLM_DW_SchemeTemDBMSConnectionString);

            DropPLM_DW_Table();

            TabGridEntityStoredProcedureHelper.CheckPLM_DW_Scripts_PlaceHolderTable();
            PLMSDWScriptGenerator.GenerateTabDWScript();
            PLMSDWScriptGenerator.GenerateGridDWscript();
            PLMSDWScriptGenerator.GenerateUserDefinTableScript();

            CreateTableStructure();

          
        }

          [Microsoft.SqlServer.Server.SqlProcedure]
        public static void PLMDW_CreateUserDefineTable()
        {


            DropTableByTablPrefixName(PLMConstantString.PLM_DW_UserDefineTablePrefix);
            PLMSDWScriptGenerator.GenerateUserDefinTableScript();
            CreateAllUserDefineTableStructure();


        }


         [Microsoft.SqlServer.Server.SqlProcedure]
        public static void PLMDW_CreateTabGridDWTableSnapshot( DateTime sinceWhen, string refTxtype )
        {

            PLMDW_CreateTable();
            string productInclasue = GenerateProductInClauseBySinceWhenAndRefTxtype(sinceWhen, refTxtype);

            List<TabGridScriptDTO> tabGridSciptDTOS = PopulateTabGridDTO();
            TransferDataToDWDatabase(tabGridSciptDTOS, productInclasue, sinceWhen);


            AddIndexToDWTable();
        }

         private static void AddIndexToDWTable()
         {

             string dropOldIndex = @"DECLARE @ownername sysname
                DECLARE @tablename sysname
                DECLARE @indexname sysname
                DECLARE @sql NVARCHAR(4000)
                DECLARE dropindexes CURSOR FOR

                SELECT sysindexes.name, sysobjects.name, sysusers.name
                FROM sysindexes
                JOIN sysobjects ON sysindexes.id = sysobjects.id
                JOIN sysusers ON sysobjects.uid = sysusers.uid
                WHERE indid > 0 
                  AND indid < 255 
                  AND INDEXPROPERTY(sysobjects.id, sysindexes.name, 'IsStatistics') = 0
                  AND sysobjects.type = N'U'
                  AND NOT EXISTS (SELECT 1 FROM sysobjects where sysobjects.name = sysindexes.name)
                  And sysobjects.name in (SELECT TABLE_NAME FROM   INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE' and TABLE_NAME like 'PLM_DW_%')
                ORDER BY sysindexes.id, indid DESC


                OPEN dropindexes
                FETCH NEXT FROM dropindexes INTO @indexname, @tablename, @ownername
                WHILE @@fetch_status = 0
                BEGIN
                  SET @sql = N'DROP INDEX '+QUOTENAME(@ownername)+'.'+QUOTENAME(@tablename)+'.'+QUOTENAME(@indexname)
                  PRINT @sql
                EXEC sp_executesql @sql  

                  FETCH NEXT FROM dropindexes INTO @indexname, @tablename, @ownername

                END
                CLOSE dropindexes
                DEALLOCATE dropindexes";

             string addPLM_DWUDClusterIndex = @"DECLARE @sql NVARCHAR(4000)

                DECLARE execmyquery CURSOR FOR
                SELECT 'ALTER TABLE '+ TABLE_NAME + ' ADD CONSTRAINT [PK_' + TABLE_NAME + '] PRIMARY KEY (ValueID)' FROM   INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE' and TABLE_NAME like 'PLM_DW_UD%' order by TABLE_NAME

                OPEN execmyquery

                FETCH NEXT FROM execmyquery INTO @sql
                WHILE @@fetch_status = 0
                BEGIN
                  PRINT @sql
                  EXEC sp_executesql @sql  
                  FETCH NEXT FROM execmyquery INTO @sql
                END

                CLOSE execmyquery
                DEALLOCATE execmyquery";


             string add_PLM_DW_Grid_ClusterIndex = @"DECLARE @sql NVARCHAR(4000)

                DECLARE execmyquery CURSOR FOR
                SELECT 'CREATE CLUSTERED INDEX [ix_rowID] ON [dbo].'+ TABLE_NAME + '([RowID])' FROM   INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE' and TABLE_NAME like 'PLM_DW_Grid_%' order by TABLE_NAME

                OPEN execmyquery

                FETCH NEXT FROM execmyquery INTO @sql
                WHILE @@fetch_status = 0
                BEGIN
                  PRINT @sql
                  EXEC sp_executesql @sql  
                  FETCH NEXT FROM execmyquery INTO @sql
                END

                CLOSE execmyquery
                DEALLOCATE execmyquery ";

             string addNoclusterGridIndex = @"DECLARE @sql NVARCHAR(4000)

                    DECLARE execmyquery CURSOR FOR
                    SELECT 'CREATE NONCLUSTERED INDEX [ix_refId] ON [dbo].'+ TABLE_NAME + '([ProductReferenceID])' FROM   INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE' and TABLE_NAME like 'PLM_DW_Grid_%' order by TABLE_NAME

                    OPEN execmyquery

                    FETCH NEXT FROM execmyquery INTO @sql
                    WHILE @@fetch_status = 0
                    BEGIN
                    PRINT @sql
                    EXEC sp_executesql @sql  
                    FETCH NEXT FROM execmyquery INTO @sql
                    END

                    CLOSE execmyquery
                    DEALLOCATE execmyquery";


                    string addTabClusterIndex = @"DECLARE @sql NVARCHAR(4000)

                    DECLARE execmyquery CURSOR FOR
                    SELECT 'CREATE CLUSTERED INDEX [ix_refID] ON [dbo].'+ TABLE_NAME + '([ProductReferenceID])' FROM   INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE' and TABLE_NAME like 'PLM_DW_Tab%' order by TABLE_NAME

                    OPEN execmyquery

                    FETCH NEXT FROM execmyquery INTO @sql
                    WHILE @@fetch_status = 0
                    BEGIN
                      PRINT @sql
                      EXEC sp_executesql @sql  
                      FETCH NEXT FROM execmyquery INTO @sql
                    END

                    CLOSE execmyquery
                    DEALLOCATE execmyquery";



             using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_DW_ConnectionString))
             {
                 conn.Open();
                 SqlCommand cmd = new SqlCommand(add_PLM_DW_Grid_ClusterIndex, conn);
                 cmd.ExecuteNonQuery();

                 cmd = new SqlCommand(addNoclusterGridIndex, conn);
                 cmd.ExecuteNonQuery();

                 cmd = new SqlCommand(addTabClusterIndex, conn);
                 cmd.ExecuteNonQuery();

                 cmd = new SqlCommand(addPLM_DWUDClusterIndex, conn);
                 cmd.ExecuteNonQuery();





             }
         
         
         }


      

        #region--------- need to cache Tab, Grid, UserDefine Entity  table  Name
        internal static Dictionary<int, string> TabTableName
        {
            get
            {
                if (_TabTableName.Count == 0)
                {
                    using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                    {
                        string aqueryTabAndGridTableName = @" SELECT   distinct   TabID, DWTableName      FROM       " + PLMConstantString.PLM_DW_TabGridScripContainerTable + " where tabID is not null ";

                        SqlCommand cmd = new SqlCommand(aqueryTabAndGridTableName, conn);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        System.Data.DataTable resultTabel = new DataTable();
                        adapter.Fill(resultTabel);

                        foreach (DataRow aRow in resultTabel.Rows)
                        {
                            //dbo.pdmTab.TabID, dbo.pdmTab.ProductCopyTabRootTabID, dbo.pdmTab.ProductReferenceID

                            SqlInt32 aTabID = Converter.ToDDLSqlInt32(aRow["TabID"].ToString());

                            string tabTablename = aRow["DWTableName"].ToString();

                            //SqlContext.Pipe.Send("tabTablename=" + tabTablename);

                            tabTablename = PLMConstantString.PLM_DWServerAndDatabaseName + tabTablename;
                            _TabTableName.Add(aTabID.Value, tabTablename);
                        }
                    }
                }

                return _TabTableName;
            }
        }

        internal static Dictionary<int, string> GridTableName
        {
            get
            {
                if (_GridTableName.Count == 0)
                {
                    using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                    {
                        string aqueryTabAndGridTableName = @" SELECT     GridID, DWTableName
                                                      FROM         dbo.pdmDWTabGridScriptSetting " + PLMConstantString.PLM_DW_TabGridScripContainerTable + " where GridID is not null ";

                        SqlCommand cmd = new SqlCommand(aqueryTabAndGridTableName, conn);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        System.Data.DataTable resultTabel = new DataTable();
                        adapter.Fill(resultTabel);

                        foreach (DataRow aRow in resultTabel.Rows)
                        {
                            SqlInt32 aGridID = Converter.ToDDLSqlInt32(aRow["GridID"].ToString());
                            string tabTablename = aRow["DWTableName"].ToString();

                            tabTablename = PLMConstantString.PLM_DWServerAndDatabaseName + tabTablename;
                            _GridTableName.Add(aGridID.Value, tabTablename);
                        }
                    }
                }

                return _GridTableName;
            }
        }

        internal static Dictionary<int, string> EntityTableName
        {
            get
            {
                if (_EntityTableName.Count == 0)
                {
                    using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                    {
                        string aqueryTabAndGridTableName = @" SELECT  distinct  EntityID, DWTableName
                                                           FROM   " + PLMConstantString.PLM_DW_TabGridScripContainerTable + " where EntityID is not null ";

                        SqlCommand cmd = new SqlCommand(aqueryTabAndGridTableName, conn);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        System.Data.DataTable resultTabel = new DataTable();
                        adapter.Fill(resultTabel);

                        foreach (DataRow aRow in resultTabel.Rows)
                        {
                            SqlInt32 aEntityID = Converter.ToDDLSqlInt32(aRow["EntityID"].ToString());
                            string tabTablename = aRow["DWTableName"].ToString();

                            tabTablename = PLMConstantString.PLM_DWServerAndDatabaseName + tabTablename;
                            _EntityTableName.Add(aEntityID.Value, tabTablename);
                        }
                    }
                }

                return _EntityTableName;
            }
        }
        #endregion

      
        #region ---------- delete RefId , TabID or GridID value from dataWS
     //  [Microsoft.SqlServer.Server.SqlProcedure]
        public static void PLMDW_SynchronizeDeleteReferenceToDWTabGridDW(int refID)
        {
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                string processTab = string.Format(@"
                    select TabID,DWTableName from pdmDWTabGridScriptSetting where  tabID in
                    (
                        select distinct tabID from PLM_DW_SimpleDCUValue where ProductReferenceID={0}
                      )", refID);

                // SqlContext.Pipe.Send("processTab=" + processTab);

                SqlCommand cmd = new SqlCommand(processTab, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                System.Data.DataTable resultTabel = new DataTable();
                adapter.Fill(resultTabel);

                foreach (DataRow aRow in resultTabel.Rows)
                {
                    //dbo.pdmTab.TabID, dbo.pdmTab.ProductCopyTabRootTabID, dbo.pdmTab.ProductReferenceID

                    SqlInt32 aTabID = Converter.ToDDLSqlInt32(aRow["TabID"]);

                    string tabTablename = string.Empty;

                    if (TabTableName.ContainsKey(aTabID.Value))
                        tabTablename = TabTableName[aTabID.Value];

                    if (tabTablename != string.Empty)
                    {
                        string deleteOLDValue = string.Format(" delete {0} WHERE  ProductReferenceID={1}", tabTablename, refID);

                        //  SqlContext.Pipe.Send("SynchronizeDeleteReferenceToDWTabGridDW=" + deleteOLDValue);

                        SqlCommand deletcmd = new SqlCommand(deleteOLDValue, conn);
                        deletcmd.ExecuteNonQuery();
                    }
                }
            }

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                string processGrid = string.Format(@"
                    select GridID,DWTableName from pdmDWTabGridScriptSetting where  GridID in
                    (
                       select distinct GridID from PLM_DW_ComplexColumnValue where ProductReferenceID={0}

                      )", refID);

                // SqlContext.Pipe.Send("processTab=" + processTab);

                SqlCommand cmd = new SqlCommand(processGrid, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                System.Data.DataTable resultTabel = new DataTable();
                adapter.Fill(resultTabel);

                foreach (DataRow aRow in resultTabel.Rows)
                {
                    //dbo.pdmTab.TabID, dbo.pdmTab.ProductCopyTabRootTabID, dbo.pdmTab.ProductReferenceID

                    SqlInt32 aGridID = Converter.ToDDLSqlInt32(aRow["GridID"]);

                    string gridablename = string.Empty;

                    if (GridTableName.ContainsKey(aGridID.Value))
                        gridablename = GridTableName[aGridID.Value];

                    if (gridablename != string.Empty)
                    {
                        string deleteOLDValue = string.Format(" delete {0} WHERE  ProductReferenceID={1}", gridablename, refID);

                        SqlContext.Pipe.Send("SynchronizeDeleteReferenceToDWTabGridDW=" + deleteOLDValue);

                        SqlCommand deletcmd = new SqlCommand(deleteOLDValue, conn);
                        deletcmd.ExecuteNonQuery();
                    }
                }
            }
        }

     //   [Microsoft.SqlServer.Server.SqlProcedure]
        public static void PLMDW_SynchronizeDeleteTabToDWTabGridDW(int tabID)
        {
            // case1: clear tab data

            #region---------------- clear tab

            string tableName = string.Empty;


            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                

               
                    string processTab = string.Format(@"
                    select TabID,DWTableName from pdmDWTabGridScriptSetting where  tabID ={0}", tabID);

                    // SqlContext.Pipe.Send("processTab=" + processTab);

                    SqlCommand cmd = new SqlCommand(processTab, conn);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    System.Data.DataTable resultTabel = new DataTable();
                    adapter.Fill(resultTabel);

                    if (resultTabel.Rows.Count > 0)
                    {
                        tableName = resultTabel.Rows[0]["DWTableName"].ToString();
                    }
               
            }

            if (tableName != string.Empty )
            {
                DropDataWareHousingTable(tableName);
            }

            #endregion

            #region-----------  clear grid value

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                string processGrid = string.Format(@" SELECT DISTINCT dbo.pdmBlockSubItem.GridID, dbo.pdmTabField.TabID, dbo.pdmTab.ProductCopyTabRootTabID
                                                                    FROM         dbo.pdmBlockSubItem INNER JOIN
                                                                    dbo.pdmItem ON dbo.pdmBlockSubItem.BlockID = dbo.pdmItem.BlockID INNER JOIN
                                                                    dbo.pdmTabField ON dbo.pdmItem.FieldID = dbo.pdmTabField.FieldID INNER JOIN
                                                                    dbo.pdmTab ON dbo.pdmTabField.TabID = dbo.pdmTab.TabID
                                                                WHERE     (dbo.pdmBlockSubItem.GridID IS NOT NULL)
                                                                  AND      ( dbo.pdmTab.TabID={0})", tabID);

                //SqlContext.Pipe.Send("processTab=" + processTab);

                SqlCommand cmd = new SqlCommand(processGrid, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                System.Data.DataTable resultTabel = new DataTable();
                adapter.Fill(resultTabel);

                foreach (DataRow aRow in resultTabel.Rows)
                {
                    //dbo.pdmTab.TabID, dbo.pdmTab.ProductCopyTabRootTabID, dbo.pdmTab.ProductReferenceID

                    SqlInt32 aGridID = Converter.ToDDLSqlInt32(aRow["GridID"]);
                    SqlInt32 aTabID = Converter.ToDDLSqlInt32(aRow["TabID"]);
                    SqlInt32 rootLevelTabID = Converter.ToDDLSqlInt32(aRow["ProductCopyTabRootTabID"]); //ProductCopyTabRootTabID
                    string gridTablename = string.Empty;
                    if (GridTableName.ContainsKey(aGridID.Value))
                    {
                        gridTablename = GridTableName[aGridID.Value];
                    }

                    if (gridTablename != string.Empty)
                    {
                        //TabID,ProductReferenceID, GridID

                        string deleteOLDValue = string.Format(" delete {0} WHERE TabID={1}  and GridID={2} ", gridTablename, aTabID, aGridID);

                        try
                        {
                            SqlCommand deletcmd = new SqlCommand(deleteOLDValue, conn);
                            deletcmd.ExecuteNonQuery();
                        }
                        catch
                        {
                            SqlContext.Pipe.Send("exception delete deleteGridOLDValue=" + deleteOLDValue);
                        }
                    }
                }
            }

            #endregion
        }

      //  [Microsoft.SqlServer.Server.SqlProcedure]
        public static void PLMDW_SynchronizeDeleteGridToDWTabGridDW(int gridID)
        {
            string tableName = string.Empty;
            if (GridTableName.ContainsKey(gridID))
            {
                tableName = GridTableName[gridID];
            }
            else
            {
                return;
            }

            if (tableName != string.Empty)
            {
                DropDataWareHousingTable(tableName);
            }
        }

        private static void DropDataWareHousingTable(string tableName)
        {
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_DW_ConnectionString))
            {
                conn.Open();

                try
                {
                    string dropDWTable =DataAcessHelper.GetSQLDropTableCommand(tableName);

                    SqlCommand cmd = new SqlCommand(dropDWTable, conn);
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                }
            }
        }

        #endregion
         
        #region------- recreate  

       [Microsoft.SqlServer.Server.SqlProcedure]
        public static void PLMDW_ReCreateTabSchemeToDWTable(int tabID , DateTime sinceWhen, string refTxtype )
        {
            // step1: chec if this tab exist, if yes, need  to delete this tab and re-inset  to pdmDWTabGridScriptSetting



            string tabName = RePopulateTabToPLMDWTabGridScriptPlaceHolderWithNewTablLevlCaseQuery(tabID);
            if (tabName == string.Empty)
                return;


         //  string [] reftxtp = refTxtype.Split (

            // step2: find DW table exist and drop

// declare @yesterdayDay datetime
//select @yesterdayDay= DATEADD(dd, -1, GETDATE())

//exec PLMDW_ReCreateTabSchemeToDWTable 1,@yesterdayDay,'1'

            string productInclasue = GenerateProductInClauseBySinceWhenAndRefTxtype(sinceWhen, refTxtype);

             

          

            DropTabDataWSTableByTabID(tabID);

            #region------------- create swap DB for DW scheme

            SetupSwapDBForDWScheme();

            #endregion

            #region---------- populate Tab GridScriptDTO

            List<TabGridScriptDTO> AlltabGridSciptDTOS = PopulateTabGridDTO();

            List<TabGridScriptDTO> oneTabGridSciptDTOS = new List<TabGridScriptDTO>();

            string dataWsTableName = string.Empty;
            foreach (TabGridScriptDTO aDto in AlltabGridSciptDTOS)
            {
                if (!aDto.TabID.IsNull)
                {
                    if (aDto.TabID.Value == tabID)
                    {
                        oneTabGridSciptDTOS.Add(aDto);
                        dataWsTableName = aDto.DWTableName;

                        break;
                    }
                }
            }

            #endregion

            #region---------- generate Dw schme table in swap dbms

            // SqlContext.Pipe.Send(" PLM_APP_ConnectionString" + PLM_APP_ConnectionString);
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();
                foreach (TabGridScriptDTO aDto in oneTabGridSciptDTOS)
                {
                    GenerateDWTableSchemeInSwapDBPLMAppConnection(conn, aDto);
                }
            }

            #endregion

            #region------- Collect Swap DB scheme  Push  into DW databe

            CollectSwapDBSchemeAndPushIntoDWDatabase(PLMConstantString.PLM_DW_SchemeTemDBMSConnectionString);

            #endregion

            #region------------ transfer Data to DW dabase

            TransferDataToDWDatabase(oneTabGridSciptDTOS, productInclasue, sinceWhen);

            // need to upate pdmDWTabGridScriptSetting script
            PLMSDWScriptGenerator.InsertTabTableToPlaceHolderTableWithNewTablLevlCaseQuery(tabID, tabName);

            #endregion

            #region--- Drop Swap Database

            DropSwapDatabase();



            string addTabClusterIndex = @"DECLARE @sql NVARCHAR(4000)

                    DECLARE execmyquery CURSOR FOR
                    SELECT 'CREATE CLUSTERED INDEX [ix_refID] ON [dbo].'+ TABLE_NAME + '([ProductReferenceID])' FROM   INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE' and TABLE_NAME = '"+dataWsTableName+@"' order by TABLE_NAME

                    OPEN execmyquery

                    FETCH NEXT FROM execmyquery INTO @sql
                    WHILE @@fetch_status = 0
                    BEGIN
                      PRINT @sql
                      EXEC sp_executesql @sql  
                      FETCH NEXT FROM execmyquery INTO @sql
                    END

                    CLOSE execmyquery
                    DEALLOCATE execmyquery";

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_DW_ConnectionString))
            {
                conn.Open();


                SqlCommand cmd = new SqlCommand(addTabClusterIndex, conn);
                cmd.ExecuteNonQuery();

             



            }


            #endregion
        }

       private static string GenerateProductInClauseBySinceWhenAndRefTxtype(DateTime sinceWhen, string refTxtype)
       {


           string queryProductID = @"select ProductReferenceId from PdmProduct where PdmProduct.LastRevisedDate >" + SinceWhenPara + "  and RefTxType in ( " + refTxtype + ")";

           string productInclasue = @" ProductReferenceId in ( " + queryProductID + " ) ";


           return productInclasue;
       }


       // private static List<int> GetListReferenceIdsWithRefTxtypeAndSincewhen(string refTxtype, DateTime sinceWhen)
       //{
       //     string queryProductID = @"select ProductReferenceId from PdmProduct where PdmProduct.LastRevisedDate > '" + sinceWhen + "' and RefTxType in ( " + refTxtype + ")";

       //      using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_ExChangeDatabase_ConnectionString))
       //     {
       //         conn.Open();
       //         DataTable result = DataAcessHelper.GetDataTableQueryResult(conn, queryProductID);

       //         return result.AsDataRowEnumerable().Select(row => (int)row["ProductReferenceId"]).ToList ();

       //         //  CLROutput.SendDataTable(foreignKeyTable);
       //     }
       // }


   
       [Microsoft.SqlServer.Server.SqlProcedure]
       public static void PLMDW_ReCreateGridSchemeToDWTable(int gridID, DateTime sinceWhen, string refTxtype)
        {
            // step1: chec if this tab exist, if yes, need  to delete this tab and re-inset  to pdmDWTabGridScriptSetting
            string gridName = RePopulateGridToPLMDWTabGridScriptPlaceHolder(gridID);
            if (gridName == string.Empty)
                return;

            // step2: find DW table exist and drop

            string productInclasue = GenerateProductInClauseBySinceWhenAndRefTxtype(sinceWhen, refTxtype);

            DropGridDataWSTableByGridID(gridID);

            #region------------- create swap DB for DW scheme

            SetupSwapDBForDWScheme();

            #endregion

            #region---------- populate Tab GridScriptDTO

            List<TabGridScriptDTO> AlltabGridSciptDTOS = PopulateTabGridDTO();

            List<TabGridScriptDTO> oneTabGridSciptDTOS = new List<TabGridScriptDTO>();

            string dwTablename = string.Empty;
            foreach (TabGridScriptDTO aDto in AlltabGridSciptDTOS)
            {
                if (!aDto.GridID.IsNull)
                {
                    if (aDto.GridID.Value == gridID)
                    {
                        dwTablename = aDto.DWTableName;
                        oneTabGridSciptDTOS.Add(aDto);

                        break;
                    }
                }
            }

            #endregion

            #region---------- generate Dw schme table in swap dbms

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();
                foreach (TabGridScriptDTO aDto in oneTabGridSciptDTOS)
                {
                    GenerateDWTableSchemeInSwapDBPLMAppConnection(conn, aDto);
                }
            }

            #endregion

            #region------- Collect Swap DB scheme  Push  into DW databe

            CollectSwapDBSchemeAndPushIntoDWDatabase(PLMConstantString.PLM_DW_SchemeTemDBMSConnectionString);

            #endregion

            #region------------ transfer Data to DW dabase

            CLROutput.OutputDebug("start to transferproductInclasue "+productInclasue);

            TransferDataToDWDatabase(oneTabGridSciptDTOS, productInclasue, sinceWhen);

            //???
            PLMSDWScriptGenerator.InsertGridTableToPlaceHolderTable(gridID, gridName);

            #endregion

            #region--- Drop Swap Database

            DropSwapDatabase();


            if (dwTablename == string.Empty)
                return;

            string add_PLM_DW_Grid_ClusterIndex = @"DECLARE @sql NVARCHAR(4000)

                DECLARE execmyquery CURSOR FOR
                SELECT 'CREATE CLUSTERED INDEX [ix_rowID] ON [dbo].'+ TABLE_NAME + '([RowID])' FROM   INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE'  and TABLE_NAME = '" + dwTablename + @"' order by TABLE_NAME

                OPEN execmyquery

                FETCH NEXT FROM execmyquery INTO @sql
                WHILE @@fetch_status = 0
                BEGIN
                  PRINT @sql
                  EXEC sp_executesql @sql  
                  FETCH NEXT FROM execmyquery INTO @sql
                END

                CLOSE execmyquery
                DEALLOCATE execmyquery ";

            string addNoclusterGridIndex = @"DECLARE @sql NVARCHAR(4000)

                    DECLARE execmyquery CURSOR FOR
                    SELECT 'CREATE NONCLUSTERED INDEX [ix_refId] ON [dbo].'+ TABLE_NAME + '([ProductReferenceID])' FROM   INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE' and TABLE_NAME = '" + dwTablename + @"' order by TABLE_NAME

                    OPEN execmyquery

                    FETCH NEXT FROM execmyquery INTO @sql
                    WHILE @@fetch_status = 0
                    BEGIN
                    PRINT @sql
                    EXEC sp_executesql @sql  
                    FETCH NEXT FROM execmyquery INTO @sql
                    END

                    CLOSE execmyquery
                    DEALLOCATE execmyquery";

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_DW_ConnectionString))
            {
                conn.Open();


                SqlCommand cmd = new SqlCommand(addNoclusterGridIndex, conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand(add_PLM_DW_Grid_ClusterIndex, conn);
                cmd.ExecuteNonQuery();





            }

            #endregion
        }

       #endregion

       #region----------- Refresh All Tab and Grid

       [Microsoft.SqlServer.Server.SqlProcedure]
       public static void PLMDW_RefreshTabDataToDWTable(int tabID, DateTime sinceWhen, string refTxtype)
       {
         

           string productInclasue = GenerateProductInClauseBySinceWhenAndRefTxtype(sinceWhen, refTxtype);

           List<TabGridScriptDTO> AlltabGridSciptDTOS = PopulateTabGridDTO();

           List<TabGridScriptDTO> oneTabGridSciptDTOS = new List<TabGridScriptDTO>();

           // need to clear DW table Datab First
           foreach (TabGridScriptDTO aDto in AlltabGridSciptDTOS)
           {
               if (!aDto.TabID.IsNull)
               {
                   if (aDto.TabID.Value == tabID)
                   {
                       oneTabGridSciptDTOS.Add(aDto);

                       break;
                   }
               }
           }

           TransferDataToDWDatabase(oneTabGridSciptDTOS, productInclasue, sinceWhen);
       }

       //pdmBlockSubItemValuedw

     //  [Microsoft.SqlServer.Server.SqlProcedure]
       public static void RefreshPdmBlockSubItemValue(DateTime sinceWhen, string refTxtype)
       {

           string productInclasue = GenerateProductInClauseBySinceWhenAndRefTxtype(sinceWhen, refTxtype);


           string deleteOldReference = @" delete FROM  pdmBlockSubItemValuedw  WHERE " + productInclasue + System.Environment.NewLine;

           string insertDataScript = "insert into   pdmBlockSubItemValuedw  select * from pdmSearchSimpleDcuFilter WITH (NOLOCK)  WHERE " + productInclasue + System.Environment.NewLine;

          string sqlsDtsSchemcript = deleteOldReference + insertDataScript;



        
       }

        [Microsoft.SqlServer.Server.SqlProcedure]
       public static void RefreshPdmGridProductValuedw(DateTime sinceWhen, string refTxtype)
       {

           string productInclasue = GenerateProductInClauseBySinceWhenAndRefTxtype(sinceWhen, refTxtype);


           string deleteOldReference = @" delete FROM  pdmGridProductValuedw  WHERE " + productInclasue + System.Environment.NewLine;

           string insertDataScript = "insert into   pdmGridProductValuedw  select * from pdmSearchComplexColumnValue WITH (NOLOCK)  WHERE " + productInclasue + System.Environment.NewLine;

           string sqlsDtsSchemcript = deleteOldReference + insertDataScript;



        
       }

        



       [Microsoft.SqlServer.Server.SqlProcedure]
       public static void PLMDW_RefreshGridDataToDWTable(int gridID, DateTime sinceWhen, string refTxtype)
        {
         
            string productInclasue = GenerateProductInClauseBySinceWhenAndRefTxtype(sinceWhen, refTxtype);

            List<TabGridScriptDTO> AlltabGridSciptDTOS = PopulateTabGridDTO();

            List<TabGridScriptDTO> oneTabGridSciptDTOS = new List<TabGridScriptDTO>();

            // need to clear DW table Datab First
            foreach (TabGridScriptDTO aDto in AlltabGridSciptDTOS)
            {
                if (!aDto.GridID.IsNull)
                {
                    if (aDto.GridID.Value == gridID)
                    {
                        oneTabGridSciptDTOS.Add(aDto);

                        break;
                    }
                }
            }

            TransferDataToDWDatabase(oneTabGridSciptDTOS, productInclasue,sinceWhen);
        }



       [Microsoft.SqlServer.Server.SqlProcedure]
       public static void PLMDW_RefreshALLUserDefineTableToDWTable()
       {

           List<int> entityIDList = new List<int>();


           using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
           {
               conn.Open();
               string GetAllUserDefinEntity = string.Format(@"  select EntityID from pdmDWTabGridScriptSetting where EntityID is not null ");

               SqlCommand cmd = new SqlCommand(GetAllUserDefinEntity, conn);

               SqlDataAdapter adapter = new SqlDataAdapter(cmd);
               System.Data.DataTable resultTabel = new DataTable();
               adapter.Fill(resultTabel);

               foreach (DataRow aRow in resultTabel.Rows)
               {
                   entityIDList.Add((int)aRow[0]);



               }
           }


           foreach (int entityId in entityIDList)
           {
               PLMDWSynchronizeReferenceBlockToTabGridDW.DoSynchronizeUserDefineTableToDW(entityId);
           }
       }
       

        #endregion ----- end refresh


       #region-----------   PLMDW_RefreshLastModifiedTabToDWTable    Refresh All Tab and Grid

   //    [Microsoft.SqlServer.Server.SqlProcedure]
       public static void PLMDW_RefreshLastModifiedTabToDWTable(int tabID, int lastModifiedMinutesFromNow, string refTxtype)
       {

           DateTime sinceWhen = System.DateTime.UtcNow.AddMinutes(-1 * lastModifiedMinutesFromNow);
           PLMDW_RefreshTabDataToDWTable(tabID, sinceWhen, refTxtype);
          
       }

        

    //   [Microsoft.SqlServer.Server.SqlProcedure]
       public static void PLMDW_RefreshLastModifiedGridDataToDWTable(int gridID, int lastModifiedMinutesFromNow, string refTxtype)
       {

           DateTime sinceWhen = System.DateTime.UtcNow.AddMinutes(-1 * lastModifiedMinutesFromNow);

           PLMDW_RefreshGridDataToDWTable(gridID, sinceWhen, refTxtype);
          
       }



       #endregion ----- end refresh

       #region---------- help method

        //chec if this tab exist, if yes, need  to delete this tab and re-inset  to pdmDWTabGridScriptSetting
        private static string RePopulateTabToPLMDWTabGridScriptPlaceHolderWithNewTablLevlCaseQuery(int tabID)
        {
            string tabName = string.Empty;
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                string qeuryTab = @" SELECT top 1 TabID,TabName FROM pdmtab WHERE TabID =" + tabID;

                SqlCommand cmd = new SqlCommand(qeuryTab, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                System.Data.DataTable resultTabel = new DataTable();
                adapter.Fill(resultTabel);

                if (resultTabel.Rows.Count > 0)
                {
                    tabName = resultTabel.Rows[0]["TabName"].ToString();
                }
            }

            if (tabName == string.Empty)
            {
                SqlContext.Pipe.Send("Can not find this tab (tabID=)" + tabID);
                // return;
            }

            try
            {
                PLMSDWScriptGenerator.InsertTabTableToPlaceHolderTableWithNewTablLevlCaseQuery(tabID, tabName);
            }
            catch { SqlContext.Pipe.Send("Can insert the Tab table to " + tabName + " " + PLMConstantString.PLM_DW_TabGridScripContainerTable); }

            return tabName;
        }

        //chec if this tab exist, if yes, need  to delete this tab and re-inset  to pdmDWTabGridScriptSetting
        private static string RePopulateGridToPLMDWTabGridScriptPlaceHolder(int gridID)
        {
            string gridName = string.Empty;
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                string qeuryGrid = @" SELECT top 1 GridID,GridName FROM pdmGrid WHERE GridID=" + gridID;

                SqlCommand cmd = new SqlCommand(qeuryGrid, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                System.Data.DataTable resultTabel = new DataTable();
                adapter.Fill(resultTabel);

                if (resultTabel.Rows.Count > 0)
                {
                    gridName = resultTabel.Rows[0]["GridName"].ToString();
                }
            }

            if (gridName == string.Empty)
            {
                SqlContext.Pipe.Send("Can not find this Grid (gridID=)" + gridID);
                // return;
            }

            try
            {
               PLMSDWScriptGenerator. InsertGridTableToPlaceHolderTable(gridID, gridName);
            }
            catch { SqlContext.Pipe.Send("Can insert the Grid table to " + gridName + " " + PLMConstantString.PLM_DW_TabGridScripContainerTable); }

            return gridName;
        }

        private static void DropTabDataWSTableByTabID(int tabID)
        {
            string dwTabTableName = GetTabDataWSTableName(tabID);

            // need to drop this tabTable
            if (dwTabTableName != string.Empty)
            {
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_DW_ConnectionString))
                {
                    conn.Open();

                    try
                    {
                        //SELECT * FROM  devlppdmDW.sys.objects
                        string dropDWTable = DataAcessHelper.GetSQLDropTableCommand(dwTabTableName);

                        SqlCommand cmd = new SqlCommand(dropDWTable, conn);
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static void DropGridDataWSTableByGridID(int gridID)
        {
            string dwGridTableName = GetGridDataWSTableName(gridID);

            // need to drop this tabTable
            if (dwGridTableName != string.Empty)
            {
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_DW_ConnectionString))
                {
                    conn.Open();

                    try
                    {
                        //SELECT * FROM  devlppdmDW.sys.objects
                        string dropDWTable =DataAcessHelper. GetSQLDropTableCommand(dwGridTableName);

                        SqlCommand cmd = new SqlCommand(dropDWTable, conn);
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static void TruancateGridDataWSTableByGridID(int gridID)
        {
            string dwGridTableName = GetGridDataWSTableName(gridID);

            // need to drop this tabTable
            TruncateDWTableByTablName(dwGridTableName);
        }

        private static void TruancateTabDataWSTableByTabID(int tabID)
        {
            string dwTabTableName = GetTabDataWSTableName(tabID);

            // need to drop this tabTable
            TruncateDWTableByTablName(dwTabTableName);
        }

        private static void TruncateDWTableByTablName(string dwTableName)
        {
            if (dwTableName != string.Empty)
            {
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_DW_ConnectionString))
                {
                    conn.Open();

                    try
                    {
                        //SELECT * FROM  devlppdmDW.sys.objects
                        string truncateDWTable = string.Format(" truncate table dbo.[{0}]", dwTableName);

                        SqlCommand cmd = new SqlCommand(truncateDWTable, conn);
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static string GetTabDataWSTableName(int tabID)
        {
            string aTabTablmane = string.Empty;
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                string sqlTabPLMTable = string.Format(@" select top 1 DWTableName from {0} where TabID={1}", PLMConstantString.PLM_DW_TabGridScripContainerTable, tabID);

                SqlCommand cmd = new SqlCommand(sqlTabPLMTable, conn);
                SqlDataReader aReader = cmd.ExecuteReader();
                if (aReader.HasRows)
                {
                    while (aReader.Read())
                    {
                        aTabTablmane = aReader["DWTableName"].ToString();
                    }
                }
            }

            return aTabTablmane;
        }

        private static string GetGridDataWSTableName(int gridID)
        {
            string aTabTablmane = string.Empty;
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                string sqlTabPLMTable = string.Format(@" select top 1 DWTableName from {0} where GridID={1}", PLMConstantString.PLM_DW_TabGridScripContainerTable, gridID);

                SqlCommand cmd = new SqlCommand(sqlTabPLMTable, conn);
                SqlDataReader aReader = cmd.ExecuteReader();
                if (aReader.HasRows)
                {
                    while (aReader.Read())
                    {
                        aTabTablmane = aReader["DWTableName"].ToString();
                    }
                }
            }

            return aTabTablmane;
        }

        #endregion
   
       #region----------- Synchonized

     //   [Microsoft.SqlServer.Server.SqlProcedure]
        public static void PLMDW_SynchronizeReferenceBlockToTabGridDW(string refids, string blockids)
        {
            PLMDWSynchronizeReferenceBlockToTabGridDW.DoSynchronizeReferenceBlockToTabGridDWTo(refids, blockids);

           
        }


     

       [Microsoft.SqlServer.Server.SqlProcedure]
        public static void PLMDW_SynchronizeUserDefineTableToDW(int entityID)
        {
            PLMDWSynchronizeReferenceBlockToTabGridDW.DoSynchronizeUserDefineTableToDW(entityID);
        }

        #endregion

       #region-------- View and store pocere

        // [Microsoft.SqlServer.Server.SqlProcedure]
        public static void PLMDW_ReoprtViewDataSourceFindAndReplace(string findWhat, string replaceWith)
        {
            using (SqlConnection contextConnection = new SqlConnection("context connection=true"))
            {
                //SqlContext.
                contextConnection.Open();

                //PLMAPPDBConnection

                string aqueryView = string.Format(@" SELECT DISTINCT so.name FROM syscomments sc INNER JOIN sysobjects
                             so ON sc.id=so.id WHERE   xtype='v'");
                //   and so.name like '{0}%'", PLM_Report_View);

                SqlCommand cmd = new SqlCommand(aqueryView, contextConnection);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                System.Data.DataTable resultTabel = new DataTable();

                adapter.Fill(resultTabel);

                foreach (DataRow aRow in resultTabel.Rows)
                {
                    string viewName = aRow["name"].ToString();
                    SqlContext.Pipe.Send("viewBame=" + viewName);

                    string viewText = GetViewContent(contextConnection, viewName);

                    var regex = new Regex("CREATE ", RegexOptions.IgnoreCase);
                    string alterView = regex.Replace(viewText, "ALTER ");

                    var regexFindAndReplace = new Regex(findWhat, RegexOptions.IgnoreCase);
                    alterView = regexFindAndReplace.Replace(alterView, replaceWith);

                    SqlCommand cmdExcuteAlterView = new SqlCommand(alterView, contextConnection);

                    try
                    {
                        cmdExcuteAlterView.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        if (alterView.Length >= 3900)
                        {
                            alterView = alterView.Substring(0, 3900);
                        }

                        SqlContext.Pipe.Send("  Failed  View Alter: " + alterView);
                    }

                    // SqlContext.Pipe.Send("viewText=" + viewText);

                    //SqlContext.Pipe.Send("alterView=" + alterView);
                }
            }
        }

        private static string GetViewContent(SqlConnection contextConnection, string viewName)
        {
            // string getViewTextQuery = string.Format(@"sp_helptext 'dbo.{0}'", viewName);

            //SqlContext.Pipe.Send("view name" + viewName);

            // store procedure call
            //SqlCommand getViewCmd = new SqlCommand(getViewTextQuery, contextConnection);
            //getViewCmd.CommandType = CommandType.StoredProcedure;
            //getViewCmd.CommandText = "sp_helptext";
            //SqlParameter viewNameParameter = new SqlParameter();
            //viewNameParameter.ParameterName = "@objname";
            //viewNameParameter.Value = viewName;
            //getViewCmd.Parameters.Add(viewNameParameter);

            // query

            // getViewCmd.ExecuteReader();

            string queryViewText = string.Format(@" select text from syscomments  where id = OBJECT_id('{0}')", viewName);
            SqlCommand getViewCmd = new SqlCommand(queryViewText, contextConnection);

            SqlDataAdapter getViewAdapter = new SqlDataAdapter(getViewCmd);
            System.Data.DataTable getViewresultTabel = new DataTable();

            getViewAdapter.Fill(getViewresultTabel);

            string viewText = getViewresultTabel.Rows[0][0].ToString();

            return viewText;

            //SqlContext.Pipe.Send(getViewresultTabel.Rows[0][0].ToString() + getViewresultTabel.Rows.Count.ToString () );
        }

        #endregion

        #region------------- Drop DW table

        private static void DropPLM_DW_Table()
        {
            DropTableByTablPrefixName(PLMConstantString.PLM_DW_TabPreifxTableName);
            DropTableByTablPrefixName(PLMConstantString.PLM_DW_GridPreifxTableName);
            DropTableByTablPrefixName(PLMConstantString.PLM_DW_UserDefineTablePrefix);
        }

        private static void DropTableByTablPrefixName(string aprefixName)
        {
            List<string> plmDWTableName = new List<string>();

            // using  PLM_DW_ConnectionString
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_DW_ConnectionString))
            {
                // SqlContext.Pipe.Send("Starting..." + PLM_DW_ConnectionString);

                conn.Open();

                string qeuryTabBlockSubitem = string.Format(@" SELECT  Name FROM sys.objects WHERE name like '{0}%'", aprefixName);

                SqlCommand cmd = new SqlCommand(qeuryTabBlockSubitem, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                System.Data.DataTable resultTabel = new DataTable();
                adapter.Fill(resultTabel);
                string mechPrefixTable = "PLM_DW_MerchPlan";
                foreach (DataRow aRow in resultTabel.Rows)
                {
                    string objectName = aRow["Name"].ToString();
                    if (!objectName.Contains(mechPrefixTable))
                    {
                        plmDWTableName.Add(objectName);
                    }
                }
            }

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_DW_ConnectionString))
            {
                conn.Open();
                foreach (string tableName in plmDWTableName)
                {
                    try
                    {
                        //SELECT * FROM  devlppdmDW.sys.objects
                        string dropDWTable = DataAcessHelper.GetSQLDropTableCommand(tableName);

                        SqlCommand cmd = new SqlCommand(dropDWTable, conn);
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                    }
                }
            }
        }

       

        #endregion
   
        #region----------excute All Script

        private static void CreateTableStructure()
        {
            #region------------- create swap DB for DW scheme

            SetupSwapDBForDWScheme();

            #endregion

            #region---------- populate Tab GridScriptDTO

            List<TabGridScriptDTO> tabGridSciptDTOS = PopulateTabGridDTO();

            #endregion

            #region---------- generate Dw schme table in swap dbms

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();
                foreach (TabGridScriptDTO aDto in tabGridSciptDTOS)
                {
                    GenerateDWTableSchemeInSwapDBPLMAppConnection(conn, aDto);
                }
            }

            #endregion

            #region------- Collect Swap DB scheme  Push  into DW databe

            CollectSwapDBSchemeAndPushIntoDWDatabase(PLMConstantString.PLM_DW_SchemeTemDBMSConnectionString);

            #endregion


            #region--- Drop Swap Database

            DropSwapDatabase();

            #endregion




        }



        private static void CreateAllUserDefineTableStructure()
        {
            #region------------- create swap DB for DW scheme

            SetupSwapDBForDWScheme();

            #endregion

            #region---------- populate Tab GridScriptDTO

            List<TabGridScriptDTO> tabGridSciptDTOS = PopulateUserDefineTableScriptdDTO();

            #endregion

            #region---------- generate Dw schme table in swap dbms

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();
                foreach (TabGridScriptDTO aDto in tabGridSciptDTOS)
                {
                    GenerateDWTableSchemeInSwapDBPLMAppConnection(conn, aDto);
                }
            }

            #endregion

            #region------- Collect Swap DB scheme  Push  into DW databe

            CollectSwapDBSchemeAndPushIntoDWDatabase(PLMConstantString.PLM_DW_SchemeTemDBMSConnectionString);

            #endregion


            #region--- Drop Swap Database

            DropSwapDatabase();

            #endregion




        }


     

        #endregion

        private static void TransferDataToDWDatabase(List<TabGridScriptDTO> tabGridSciptDTOS, string  productReferenceInClasue, DateTime? sinceWhen )
        {
            string plmDWServer;
            string plmDWDataBase;

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_DW_ConnectionString))
            {
                plmDWServer = conn.DataSource;
                plmDWDataBase = conn.Database;
            }

          //  CLROutput.OutputDebug("start loop tabGridSciptDTOS  transferproductInclasue and tabGridSciptDTOS " + tabGridSciptDTOS.Count.ToString() + "  inclause ??" + productReferenceInClasue);

            foreach (TabGridScriptDTO aDto in tabGridSciptDTOS)
            {
             
                // big improve for each big sql command , need to use connection for each command !
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                {
                    conn.Open();


                   //begin 
                    DataAcessHelper.ExecuteReadUnCommmited(conn);

                    try
                    {
                         //string  aDto.DWTableName 

                        // insert  into PLM_DW_Tab_2_3159  SELECT TabID, ProductReferenceID, dbo.Concatenate ( case  when    subitemid=3244 then ValueText    end ) as 'Attachment_3244'  , dbo.Concatenate ( case  when    subitemid=3245 then ValueText    end ) as 'Notes_3245'  , dbo.Concatenate ( case  when    subitemid=3246 then ValueText    end ) as 'Sketch_3246'  , dbo.Concatenate ( case  when    subitemid=3243 then ValueText    end ) as 'Style_Info_3243'   FROM PLM_DW_SimpleDCUValue  
                        // WHERE  SubItemID in ( 3244,3245,3246,3243 )  and TabID=3159  group by TabID, ProductReferenceID

                   
                   

                       string   sqlsDtsSchemcript = aDto.InserIntoSQLScript;

                    //   CLROutput.OutputDebug("sqlsDtsSchemcript???? " + sqlsDtsSchemcript);


                        string replaceTempFullName = "[" + plmDWServer + "].[" + plmDWDataBase + "]." + "dbo.[" + aDto.DWTableName + "]";
                        sqlsDtsSchemcript = sqlsDtsSchemcript.Replace(aDto.DWTableName, replaceTempFullName);


                      //  CLROutput.OutputDebug(sqlsDtsSchemcript);

                        // not user define table
                        if (! aDto.DWTableName.StartsWith(PLMConstantString.PLM_DW_UserDefineTablePrefix))
                        {
                            if (! string.IsNullOrEmpty(productReferenceInClasue))
                            {
                                string deleteOldReference = @" delete FROM  " + replaceTempFullName + " WHERE " + productReferenceInClasue + System.Environment.NewLine;

                                string insertDataScript = sqlsDtsSchemcript.Replace("WHERE", " WHERE " + productReferenceInClasue + " And ");

                                sqlsDtsSchemcript = deleteOldReference + insertDataScript;

                                CLROutput.OutputDebug(sqlsDtsSchemcript);


                            }
                        
                        }




                        CLROutput.OutputDebug(sqlsDtsSchemcript);
                        // 
                        SqlCommand cmd = new SqlCommand(sqlsDtsSchemcript, conn);

                     
                        if (sinceWhen.HasValue)
                        {
                            cmd.Parameters.Add(SinceWhenPara, sinceWhen);  
                        }


                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                       // throw new Exception(ex.ToString());

                       // SqlContext.Pipe.Send(" Data Transfer" + aDto.DWTableName + ex.ToString());
                    }

                    DataAcessHelper.ExecuteReadCommmited(conn);
                }
            }

            SqlContext.Pipe.Send(" Data  Transfer Was done ! ");
        }

        private static List<TabGridScriptDTO> PopulateTabGridDTO()
        {
            List<TabGridScriptDTO> tabGridSciptDTOS = new List<TabGridScriptDTO>();
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                string qeuryTabBlockSubitem = @" SELECT  *  FROM " + PLMConstantString.PLM_DW_TabGridScripContainerTable + " order by TabName, GridName ";

                SqlCommand cmd = new SqlCommand(qeuryTabBlockSubitem, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);  
                System.Data.DataTable resultTabel = new DataTable();
                adapter.Fill(resultTabel);

                foreach (DataRow aRow in resultTabel.Rows)
                {
                    TabGridScriptDTO aDto = new TabGridScriptDTO();

                    aDto.DWTableName = aRow["DWTableName"].ToString();
                    aDto.DWScriptID = int.Parse(aRow["DWScriptID"].ToString());
                    aDto.TabID = Converter.ToDDLSqlInt32(aRow["TabID"]);
                    aDto.GridID = Converter.ToDDLSqlInt32(aRow["GridID"]);
                    aDto.EntityID = Converter.ToDDLSqlInt32(aRow["EntityID"]);
                    aDto.InserIntoSQLScript = aRow["InserIntoSQLScript"].ToString();
                    aDto.RootLevelSelectSQLScript = aRow["RootLevelSelectSQLScript"].ToString();
                    tabGridSciptDTOS.Add(aDto);
                }
            }
            return tabGridSciptDTOS;
        }


        private static List<TabGridScriptDTO> PopulateUserDefineTableScriptdDTO()
        {
            List<TabGridScriptDTO> tabGridSciptDTOS = new List<TabGridScriptDTO>();
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                string qeuryTabBlockSubitem = @" SELECT  *  FROM " + PLMConstantString.PLM_DW_TabGridScripContainerTable + " where EntityID is not null   ";

                SqlCommand cmd = new SqlCommand(qeuryTabBlockSubitem, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                System.Data.DataTable resultTabel = new DataTable();
                adapter.Fill(resultTabel);

                foreach (DataRow aRow in resultTabel.Rows)
                {
                    TabGridScriptDTO aDto = new TabGridScriptDTO();

                    aDto.DWTableName = aRow["DWTableName"].ToString();
                    aDto.DWScriptID = int.Parse(aRow["DWScriptID"].ToString());
                    aDto.TabID = Converter.ToDDLSqlInt32(aRow["TabID"]);
                    aDto.GridID = Converter.ToDDLSqlInt32(aRow["GridID"]);
                    aDto.EntityID = Converter.ToDDLSqlInt32(aRow["EntityID"]);
                    aDto.InserIntoSQLScript = aRow["InserIntoSQLScript"].ToString();
                    aDto.RootLevelSelectSQLScript = aRow["RootLevelSelectSQLScript"].ToString();
                    tabGridSciptDTOS.Add(aDto);
                }
            }
            return tabGridSciptDTOS;
        }

        private static void DropSwapDatabase()
        {
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                //                string setOffline = string.Format(@" if  exists(SELECT * FROM sys.databases WHERE name = '{0}')
                //                                                     begin
                //                                                     ALTER DATABASE {0}
                //                                                     SET OFFLINE
                //                                                     WITH ROLLBACK IMMEDIATE
                //                                                     end
                //                                                     ", PLM_DW_SchemeTemDBMS);

                //                SqlCommand offlinecmd = new SqlCommand(setOffline, conn);

                //                try
                //                {
                //                    offlinecmd.ExecuteNonQuery();
                //                }
                //                catch { }

                string dropDb = string.Format(@" if  exists(SELECT * FROM sys.databases WHERE name = '{0}')
                                                 drop database {1} ", PLMConstantString.PLM_DW_SchemeTemDBMS, PLMConstantString.PLM_DW_SchemeTemDBMS);

                SqlCommand cmd = new SqlCommand(dropDb, conn);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch { }
            }
        }

        private static void CollectSwapDBSchemeAndPushIntoDWDatabase(string PLM_DW_SchemeTemDBMSConnection)
        {
            Dictionary<string, string> tableName_SchmeCreate = new Dictionary<string, string>();
            using (SqlConnection conn = new SqlConnection(PLM_DW_SchemeTemDBMSConnection))
            {
                conn.Open();
                string queryTablName = @" SELECT    Name  AS [TableName] FROM sysObjects   WHERE   sysObjects.name like 'PLM_DW_%'  ";

                SqlCommand cmd = new SqlCommand(queryTablName, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                System.Data.DataTable resultTabel = new DataTable();
                adapter.Fill(resultTabel);

                foreach (DataRow aRow in resultTabel.Rows)
                {
                    string tablName = aRow["TableName"].ToString();

                    string storRe = GetOneTableCreatetionSchemeByUsingGeneraTableCreateScriptStoreProc(conn, tablName);

                    tableName_SchmeCreate.Add(tablName, storRe);
                }

                SqlConnection.ClearPool(conn);
            }

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                foreach (string aTableName in tableName_SchmeCreate.Keys)
                {
                    try
                    {
                        string updateTablName = string.Format(@"  update {0} set DWTableSchemeScript = '{1}'  WHERE DWTableName='{2}' ", PLMConstantString.PLM_DW_TabGridScripContainerTable, tableName_SchmeCreate[aTableName], aTableName);
                        SqlCommand cmd = new SqlCommand(updateTablName, conn);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //SqlContext.Pipe.Send(" Create Sche Faile" + ex + tableName_SchmeCreate[atableName]);
                    }
                }
            }

            // update scheme to plm need to drop swap database

            SqlContext.Pipe.Send("Scheme Creation is done ");

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_DW_ConnectionString))
            {
                conn.Open();

                foreach (string atableName in tableName_SchmeCreate.Keys)
                {
                    try
                    {
                        string createTablName = tableName_SchmeCreate[atableName];
                        SqlCommand cmd = new SqlCommand(createTablName, conn);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        CLROutput.OutputDebug(" Create Scheme Faild" + ex + tableName_SchmeCreate[atableName], conn);
                       CLROutput.OutputDisplayErrorMsg(" Create Scheme Faild" + tableName_SchmeCreate[atableName]);
                        //SqlContext.Pipe.Send(" Create Scheme Faild" + ex + tableName_SchmeCreate[atableName]);
                    }
                }
            }

            //  SqlContext.Pipe.Send("C new table PLM_DW Database  ");
        }

        private static void GenerateDWTableSchemeInSwapDBPLMAppConnection(SqlConnection plmConn, TabGridScriptDTO aDto)
        {
            try
            {
                // using SELECT top 0 and SELECT into to generate Dw  table schemen

                string sqlsSchemcript = aDto.RootLevelSelectSQLScript;

                string replaceTempFullName = "into [" + PLMConstantString.PLM_DW_SchemeTemDBMS + "].[dbo]." + aDto.DWTableName + " FROM ";

                sqlsSchemcript = sqlsSchemcript.Replace("FROM", replaceTempFullName).Replace("SELECT", " SELECT top 0 ");

                //sqlsSchemcript.Replace("SELECT", " SELECT top 0 ")

                // // SELECT dbo.pdmUserDefineEntityValue.RowID AS ValueID, TextValue as UOM  into FROM    dbo.pdmUserDefineEntityValue   WHERE EntityID=3009

                //// tab script to to ch
                if (!aDto.TabID.IsNull || !aDto.GridID.IsNull)
                {
                    string filterGroup = " and ProductReferenceID=-1000  group ";

                    sqlsSchemcript = sqlsSchemcript.Replace("group", filterGroup);
                }

                SqlCommand cmd = new SqlCommand(sqlsSchemcript, plmConn);

                cmd.ExecuteNonQuery();

                string updateTrue = " update  pdmDWTabGridScriptSetting set IsPassValidation=1 WHERE DWSCriptID= " + aDto.DWScriptID;
                SqlCommand updateCmd = new SqlCommand(updateTrue, plmConn);
                updateCmd.ExecuteNonQuery();
            }
            catch
            {
                string updateFalse = " update  pdmDWTabGridScriptSetting set IsPassValidation=0 WHERE DWSCriptID= " + aDto.DWScriptID;
                SqlCommand updateCmd = new SqlCommand(updateFalse, plmConn);
                updateCmd.ExecuteNonQuery();
            }
        }

        private static void SetupSwapDBForDWScheme()
        {
            string plmAppDBName = string.Empty;

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                //SqlContext.Pipe.Send(" open PLM_app" + PLM_APP_ConnectionString);

                plmAppDBName = conn.Database;

                //string clearDBuser = string.Format(" exec cleardbusers '{0}'", PLM_DW_SchemeTemDBMS);
                //SqlCommand cmdClearUser = new SqlCommand(clearDBuser, conn);
                //cmdClearUser.ExecuteNonQuery();

                string dropDb = string.Format(@" if  exists(SELECT * FROM sys.databases WHERE name = '{0}')
                                     drop database {1} ", PLMConstantString.PLM_DW_SchemeTemDBMS, PLMConstantString.PLM_DW_SchemeTemDBMS);

                SqlCommand cmd = new SqlCommand(dropDb, conn);
                cmd.ExecuteNonQuery();

                SqlCommand createDB = new SqlCommand(string.Format(@" create database {0}", PLMConstantString.PLM_DW_SchemeTemDBMS), conn);

                if (!string.IsNullOrEmpty(PLMConstantString.PLM_DW_SwapDBFilePath))
                {
                    string createDBWithpath = string.Format(@"
                    CREATE DATABASE PLM_DW_SchemeTemDBMS
                    ON
                    ( NAME = PLM_DW_SchemeTemDBMS,
                        FILENAME = '{0}\PLM_DW_SchemeTemDBMS.mdf',
                        SIZE = 10,
                        MAXSIZE = 50,
                        FILEGROWTH = 5
                    )

                     LOG ON
                    (
                        NAME = Sales_log,
                        FILENAME = '{1}\PLM_DW_SchemeTemDBMS.ldf',
                        SIZE = 5MB,
                        MAXSIZE = 25MB,
                        FILEGROWTH = 5MB
                     )", PLMConstantString.PLM_DW_SwapDBFilePath, PLMConstantString.PLM_DW_SwapDBFilePath);

                    createDB = new SqlCommand(createDBWithpath, conn);
                }

                createDB.ExecuteNonQuery();

                // SqlContext.Pipe.Send(" Create PLM_DW_SwapDBFilePath Command:" + createDB.CommandText);
            }

            // SqlContext.Pipe.Send(" plmAppDBName" + plmAppDBName);
            //SqlContext.Pipe.Send(" PLM_DW_SchemeTemDBMS" + PLM_DW_SchemeTemDBMS);

            //  string PLM_DW_SchemeTemDBMSConnection = PLM_APP_ConnectionString.Replace(plmAppDBName, PLM_DW_SchemeTemDBMS);

            // string PLM_DW_SchemeTemDBMSConnection = PLM_APP_ConnectionString.Replace(string.Format("atalog={0}", plmAppDBName), string.Format("atalog={0}", PLM_DW_SchemeTemDBMS));

            // SqlContext.Pipe.Send(" PLM_DW_SchemeTemDBMSConnection" + PLM_DW_SchemeTemDBMSConnection);

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_DW_SchemeTemDBMSConnectionString))
            {
                conn.Open();

                string craeteproc = CreateTableStoreProcedure();

                SqlCommand creteStorProc = new SqlCommand(craeteproc, conn);
                creteStorProc.ExecuteNonQuery();

                SqlConnection.ClearPool(conn);

                //  SqlContext.Pipe.Send(" CCreateTableStoreProcedure" + craeteproc);
            }
            // return PLM_DW_SchemeTemDBMSConnection + "  Pooling = False ";

            // SqlContext.Pipe.Send(" PLM_DW_SchemeTemDBMSConnection" + PLM_DW_SchemeTemDBMSConnection);
            //return PLM_DW_SchemeTemDBMSConnection;
        }

        private static string GetOneTableCreatetionSchemeByUsingGeneraTableCreateScriptStoreProc(SqlConnection conn, string tablName)
        {
            // string execStor = string.Format( @"  GenerateTableCreateScript '{0}' ",tablName);
            SqlCommand cmdToExecute = new SqlCommand();
            cmdToExecute.CommandText = "dbo.[GenerateTableCreateScript]";
            cmdToExecute.CommandType = CommandType.StoredProcedure;

            // Use base class' connection object
            cmdToExecute.Connection = conn;

            cmdToExecute.Parameters.Add(new SqlParameter("@tableName", tablName));

            SqlDataAdapter astordapter = new SqlDataAdapter(cmdToExecute);
            System.Data.DataTable storresultTabel = new DataTable();
            astordapter.Fill(storresultTabel);

            string storRe = storresultTabel.Rows[0][0].ToString();
            return storRe;
        }

        //NVARCHAR(MAX)

        private static string CreateTableStoreProcedure()
        {
            string craeteproc = @" Create Procedure GenerateTableCreateScript (
                                @tableName varchar(500))
                                as
                                If exists (Select * FROM Information_Schema.COLUMNS WHERE Table_Name= @tableName)
                                Begin
                                declare @sql NVARCHAR(MAX)
                                declare @table varchar(200)
                                declare @cols table (datatype varchar(50))
                                insert into @cols values('bit')
                                insert into @cols values('binary')
                                insert into @cols values('bigint')
                                insert into @cols values('int')
                                insert into @cols values('float')
                                insert into @cols values('datetime')
                                insert into @cols values('text')
                                insert into @cols values('image')
                                insert into @cols values('uniqueidentifier')
                                insert into @cols values('smalldatetime')
                                insert into @cols values('tinyint')
                                insert into @cols values('smallint')
                                insert into @cols values('sql_variant')

                                set @sql=''
                                Select @sql=@sql+
                                case when charindex('(',@sql,1)<=0 then '(' else '' end + '[' + Column_Name + ']'+' ' +Data_Type +
                                case when Data_Type in (Select datatype FROM @cols) then '' else  '(' end+
                                case when data_type in ('real','money','decimal','numeric')  then cast(isnull(numeric_precision,'') as varchar)+','+
                                case when data_type in ('real','money','decimal','numeric') then cast(isnull(Numeric_Scale,'') as varchar) end
                                when data_type in ('char','nvarchar','varchar','nchar') then cast(isnull(Character_Maximum_Length,'') as varchar)       else '' end+
                                case when Data_Type in (Select datatype FROM @cols)then '' else  ')' end+
                                case when Is_Nullable='No' then ' Not null,' else ' null,' end
                                FROM Information_Schema.COLUMNS WHERE Table_Name=@tableName
                                SELECT  @table=  'Create table ' + table_Name FROM Information_Schema.COLUMNS WHERE table_Name=@tableName
                                SELECT @sql=@table + substring(@sql,1,len(@sql)-1) +' )'
                                SELECT @sql  as DDL
                                End Else  Select 'The table '+@tableName + ' does not exist'  " + System.Environment.NewLine;

            return craeteproc;
        }

     

  


     
    }
}


// how to output 8000 char 

// [Microsoft.SqlServer.Server.SqlFunction]

//[return:SqlFacet(MaxSize=-1)]
//public static SqlString Function1([SqlFacet(MaxSize=-1)] SqlString s)
//{
//Int32 l = s.Value.Length;
//String retval = l.ToString();
//retval += new String('a', 20000);
//return new SqlString(retval);
//}

 

//you can invoke it from T-SQL:

//DECLARE @s VARCHAR(MAX)
//SET @s = REPLICATE(CAST('N' AS VARCHAR(MAX)), 100000)
//select dbo.Function1(@s)
