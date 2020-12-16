using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace PLMCLRTools
{
    public static class PLMConstantString
    {
        // public static string ExchangeTableExChangeActionByColumn = "ExChangeActionBy";    //PLM,ERP

        public static readonly string EX_PLM_Import_Prefix = "EX_ERP_PLM_";
        //public static readonly string EX_PLM_Export_Prefix = "EX_PLM_Export_";
        public static string ExchangeRowDataERPFlagColumn = "ExchangeRowDataERPFlag"; // // 1:IsNew 2:IsModified 3: IsDeleted ,4:NoChanged
        public static string ExchangeRowDataPLMFlagColumn = "ExchangeRowDataPLMFlag"; // // 1:IsNew 2:IsModified 3: IsDeleted ,4:NoChanged
        public static string ExchangeRowDataERPExportDateTimeColumn = "ExchangeRowDataERPExportDateTime";
        public static string ExchangeRowDataPLMImportDateTimeColumn = "ExchangeRowDataPLMImportDateTime";
        public static string ExchangeRowDataPLMPrimayKeyColumn = "ExchangeRowDataPLMPrimayKey";
        public static string ExchangeRowDataSourceOfOriginalColumn = "ExchangeRowDataSourceOfOriginal";
        public static string ExchangeRowDataChangeTimeStamp = "ChangeTimeStamp";
        public static string PLM = "PLM";
        public static string ERP = "ERP";


        // need to add SQL DataType here Foreign Table mapping

        public static string ExchangeRowDataForeignKeyColumn = "ExchangeRowDataForeignKeyColumn";
        public static string ExchangeRowDataPrimaryKeyColumn = "ExchangeRowDataPrimaryKeyColumn";

        public static string FkTableExchangeRowDataPLMPrimayKeyColumn = "FkTable" + PLMConstantString.ExchangeRowDataPLMPrimayKeyColumn;
        


        // Tech Pack
        public static readonly string ConvertValueTextToDateTime = @"dbo.ConvertValueTextToDateTime";
        public static readonly string ConvertValueTextToDecimal = @"dbo.ConvertValueTextToDecimal";
        public static readonly string ConvertValueTextToInt = @"dbo.ConvertValueTextToInt";
        public static readonly string PdmSearchSimpleDcuValue = @" dbo.PdmSearchSimpleDcuValue ";
        public static readonly string pdmSearchComplexColumnValue = @" dbo.pdmSearchComplexColumnValue ";

        
        public static readonly string SETTRANSACTIONISOLATIONLEVELREAD_UNCOMMITTED = @" SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED  ";
        public static readonly string SETTRANSACTIONISOLATIONLEVELREAD_COMMITTED = @"   SET TRANSACTION ISOLATION LEVEL READ COMMITTED  ";

        public static readonly string EntityIdDisplayPrefix = @"_D8EFFB23_46ED_418C_AFF1";
        public static readonly string EntityMappingDisplay = @"_Display";


        // DW Table constatn string


        public static readonly string PLM_DW_TabGridScripContainerTable = "pdmDWTabGridScriptSetting";
        public static readonly string PLM_DW_TabPreifxTableName = "PLM_DW_Tab_";
        public static readonly string PLM_DW_GridPreifxTableName = "PLM_DW_Grid_";
        public static readonly string PLM_DW_UserDefineTablePrefix = "PLM_DW_UD_";
        public static readonly string PLM_DW_SchemeTemDBMS = "PLM_DW_SchemeTemDBMS";
        public static readonly string PLM_DW_REPLACEFROM = "PLM_DW_REPLACEFROM";

        public static readonly string PLM_ERP_DataEX_TableName = "PLM_ERP_DataEX_Tab";



     

        public static readonly string PLM_APP_ConnectionString = string.Empty;
        public static readonly string PLM_DW_ConnectionString = string.Empty;
        public static readonly string PLM_DW_SwapDBFilePath = string.Empty;
        public static readonly string PLM_DW_SchemeTemDBMSConnectionString = string.Empty;
        public static readonly string PLM_Report_View = "PLM_Report_View";

        public static readonly Dictionary<string, PdmSetupClrDto> DictPdmSetup;
       

        //List< PdmSetupClrDto>


        public static readonly string PLM_ExChangeDatabase_ConnectionString = string.Empty;

        private static string _PLM_ExChangeDatabase_ServerAndDatabaseName = string.Empty;
        public static string PLM_ExChangeDatabase_ServerAndDatabaseName
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


        static PLMConstantString()
        {
            // Initianize context FROM PLM App
            using (SqlConnection conn = new SqlConnection("context connection=true"))
            {
                  
                conn.Open();

                ;

                //PLMAPPDBConnection

                string aqueryDwConn = " SELECT SetupValue FROM pdmsetup WHERE setupCode='PLMDWDBConnection'";
                System.Data.DataTable resultTabelPLMDWDB =  DataAcessHelper.GetDataTableQueryResult(conn, aqueryDwConn);

                if (resultTabelPLMDWDB.Rows.Count > 0)
                {
                    PLM_DW_ConnectionString = resultTabelPLMDWDB.Rows[0]["SetupValue"].ToString();
                }

                CLROutput.OutputDebug("PLM_DW_ConnectionString=" + PLM_DW_ConnectionString);




                //PLMAPPDBConnection
                string aqueryAppConn = " SELECT SetupValue FROM pdmsetup WHERE setupCode='PLMAPPDBConnection'";
                System.Data.DataTable resultTabelPLMAPPDB = DataAcessHelper.GetDataTableQueryResult(conn, aqueryAppConn);


                if (resultTabelPLMAPPDB.Rows.Count > 0)
                {
                    PLM_APP_ConnectionString  = resultTabelPLMAPPDB.Rows[0]["SetupValue"].ToString();
                }

                CLROutput.OutputDebug("PLM_APP_ConnectionString=" + PLM_APP_ConnectionString);




                //Swap_DBConnectin string

                var regex = new Regex("Catalog=" + conn.Database, RegexOptions.IgnoreCase);
                PLM_DW_SchemeTemDBMSConnectionString = regex.Replace(PLM_APP_ConnectionString, "Catalog=" + PLM_DW_SchemeTemDBMS);





                // PLM_DW_SwapDBFilePathConn
                string PLM_DW_SwapDBFilePathConn = " SELECT SetupValue FROM pdmsetup WHERE setupCode='PLMDWSwapDBFilePath'";
                System.Data.DataTable resultTabelPLMDWSwapDBFilePath = DataAcessHelper.GetDataTableQueryResult(conn, PLM_DW_SwapDBFilePathConn);


                if (resultTabelPLMDWSwapDBFilePath.Rows.Count > 0)
                {
                    PLM_DW_SwapDBFilePath = resultTabelPLMDWSwapDBFilePath.Rows[0]["SetupValue"].ToString();
                }
                CLROutput.OutputDebug("PLM_DW_SwapDBFilePath=" + PLM_DW_SwapDBFilePath);

               

            

                ////Ex-chagne-dB
               string exChangeDBConn = " SELECT SetupValue FROM pdmsetup WHERE setupCode='PLMExChangeDatabaseConnection'";
               System.Data.DataTable resultTabelexChangeDBConn = DataAcessHelper.GetDataTableQueryResult(conn, exChangeDBConn);

               if (resultTabelexChangeDBConn.Rows.Count > 0)
               {
                   PLM_ExChangeDatabase_ConnectionString = resultTabelexChangeDBConn.Rows[0]["SetupValue"].ToString();
                  
               }
               CLROutput.OutputDebug("PLM_ExChangeDatabase_ConnectionString=" + PLM_ExChangeDatabase_ConnectionString);


                //

               DictPdmSetup = PdmSetupDal.GetAllList(conn).Where(o => !string.IsNullOrEmpty(o.SetupCode)).ToDictionary(o => o.SetupCode, o => o);
                
            }

           
        }


        private static string _PLM_DWServerAndDatabaseName = string.Empty;

        public static string PLM_DWServerAndDatabaseName
        {
            get
            {
                if (_PLM_DWServerAndDatabaseName == string.Empty)
                {
                    string plmDWServer = string.Empty;
                    string plmDWDataBase = string.Empty;

                    using (SqlConnection conn = new SqlConnection(PLM_DW_ConnectionString))
                    {
                        conn.Open();
                        plmDWServer = conn.DataSource;
                        plmDWDataBase = conn.Database;
                    }

                    _PLM_DWServerAndDatabaseName = "[" + plmDWServer + "]." + "[" + plmDWDataBase + "]." + "dbo.";
                }

                return _PLM_DWServerAndDatabaseName;
            }
        }
    }

   
  

    public class GridColumnConstantName
    {

        public static readonly string TabName = "TabName";
        public static readonly string ProductReferenceID = "ProductReferenceID";
        public static readonly string RowValueGUID = "RowValueGUID";
        public static readonly string GridID = "GridID";
        public static readonly string BlockID = "BlockID";
        public static readonly string TabID = "TabID";
     //   public static readonly string RowID = "RowID";
        public static readonly string Sort = "Sort";

        public static readonly string GridColumnIDPrefix = "GridColumnID_";
        public static readonly string SubItemIDPrefix = "SubItemID_";

        public static readonly string GridColumnID = "GridColumnID";
        public static readonly string SubItemID = "SubItemID";

        public static readonly string ValueText = "ValueText";
        public static readonly string ValueID = "ValueID";
        public static readonly string CountPrefix = "Count_";
        public static readonly string CopyTabReferenceID = "CopyTabReferenceID";



    }
}
