using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System;

namespace PLMCLRTools
{
    public class PLMUtilityStorcProcedures
    {
       

        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void RestartClr()
        {
            try
            {
               // using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                using (SqlConnection conn = new SqlConnection("context connection=true"))
                {
                    //SqlContext.
                    conn.Open();

                    string restcmd = @"EXEC SP_CONFIGURE 'clr enabled' , '0'
                                    RECONFIGURE;
                                    EXEC SP_CONFIGURE 'clr enabled' , '1';
                                    RECONFIGURE;
                                   ";

                    try
                    {
                        DataAcessHelper.ExecuteNonQuery(conn, restcmd);
                        //PLMSCacheSystem.TouchCache();
                    }
                    catch { };
                }
            }
            catch { }

            //PLMSDWStoredProcedures.GenerateUserDefinTableScript();
        }

      
    

      //  [Microsoft.SqlServer.Server.SqlProcedure]
        public static void TestCLR()
        {
            //            PLMSReferenceValueRetrieveBL.GetMutipleBlockGridCellValueTable(new[] { 9625 }, new[] { 5807 }, new[] { 4407,
            //4408,
            //4410,
            //4411,
            //4412,
            //4413,
            //4414,
            //4415,
            //4416});

            var result =
           PLMSReferenceValueRetrieveBL.RetrieveBlocksReferencesGridColumnsRowList(new[] { 9625 }, new[] { 5807 }, new[] { 4407,
4408,
4410,
4411,
4412,
4413,
4414,
4415,
4416});

            //CLROutput.Output(result.Count.ToString () );

            foreach (var pair in result)
            {
                CLROutput.OutputDebug(pair.Key.ToString());

                var dictValue1 = pair.Value;

                foreach (var pair2 in dictValue1)
                {
                    CLROutput.OutputDebug(pair2.Key.ToString());

                    var dictvalue3 = pair2.Value;

                    foreach (var row in dictvalue3)
                    {
                        // row.RowId

                        //CLROutput.Output(pair3.Key.ToString());

                        //var dictvalue4 = pair3.Value;

                        //foreach (var pair4 in dictvalue4)
                        //{
                        //    CLROutput.Output(pair4.Key.ToString());

                        //    var dictvalue5 = pair4.Value;

                        //}
                    }
                }
            }

            // CLROutput.SendDataTable(gridcolumnResultDataTable);

            //using (SqlConnection conn = new SqlConnection("Data Source=srv-spider;Initial Catalog=PLMS_Devlp;User ID=sa;Password=visualdev"))
            //{
            //    //SqlContext.
            //    conn.Open();

            //    // TestDatatableScheme(conn);
            //    //  DataTableUtility.BulkCopyDatatableToDatabaseTable(conn, result, "FromDataTale", dictDataTableAndDatabaseTableMappingColumn);

            //    TestExcelExport(conn);
            //}

            //PLMSDWStoredProcedures.GenerateUserDefinTableScript();
        }

        public static void TestExcelExport(SqlConnection conn)
        {
            DataSet exportData = new DataSet();

            Dictionary<string, string> dictTabQuery = new Dictionary<string, string>();

            //string queryTab = "select * from pdmTab";

            //dictTabQuery.Add ("PDMTab", queryTab);

            string querygrid = "select * from pdmgrid";

            dictTabQuery.Add("pdmgrid", querygrid);

            exportData = DataAcessHelper.GetDataSetQueryResult(conn, dictTabQuery);

            //check for empty parameters

            //if (procName.Value == string.Empty)
            //    throw new Exception("Procedure name value is missing.");

            //if (filePath.Value == string.Empty)
            //    throw new Exception("Missing file path location.");

            //if (fileName.Value == string.Empty)
            //    throw new Exception("Missing name of file.");

            //using (SqlConnection conn = new SqlConnection("context connection=true"))
            //{
            //    SqlCommand getOutput = new SqlCommand();

            //    getOutput.CommandText = procName.ToString(); ;
            //    getOutput.CommandType = CommandType.StoredProcedure;
            //    getOutput.CommandTimeout = 120;

            //    //To allow for multiple parameters, xml is used
            //    //and must then be parsed to set up the paramaters
            //    //for the command object.
            //    using (XmlReader parms = xmlParams.CreateReader())
            //    {
            //        while(parms.Read())
            //        {
            //            if (parms.Name == "param")
            //            {
            //                string paramName;
            //                paramName = parms.GetAttribute("name");

            //                string paramValue;
            //                paramValue = parms.GetAttribute("value");

            //                getOutput.Parameters.AddWithValue(paramName, paramValue);
            //            }
            //        }
            //    }
            //    //srv-SpiderD:\MSSQLServer\Temp
            //    getOutput.Connection = conn;

            //    conn.Open();
            //    SqlDataAdapter da = new SqlDataAdapter(getOutput);
            //    da.Fill(exportData);
            //    conn.Close();
            //}

            // //srv-SpiderD:\MSSQLServer\Temp
            ExcelExportUtility exportUtil = new ExcelExportUtility("TestExcelexport" + System.Guid.NewGuid().ToString(), @"D:\MSSQLServer\Temp\");

            //This allows for flexible naming of the tabs in the workbook
            exportUtil.SheetNameColumnOrdinal = 0;
            exportUtil.Export(exportData);
        }

        private static void TestDatatableScheme(SqlConnection conn)
        {
            string aQuery = "select TabID,TabName,FolderID from pdmtab ";

            DataTable result = DataAcessHelper.GetDataTableQueryResult(conn, aQuery);
            Dictionary<string, string> dictDataTableAndDatabaseTableMappingColumn = new Dictionary<string, string>();
            dictDataTableAndDatabaseTableMappingColumn.Add("TabID", "TabID");
            dictDataTableAndDatabaseTableMappingColumn.Add("TabName", "TabName");
            dictDataTableAndDatabaseTableMappingColumn.Add("FolderID", "FolderID");

            aQuery = "select * from pdmgrid ";

            DataTableUtility.CreateDataBaseTableFromQuery(aQuery, conn, "testpdmgrid");
        }

        }
}