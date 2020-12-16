using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Text;
using Microsoft.SqlServer.Server;

namespace PLMCLRTools
{
  
    public static class  TabGridEntityStoredProcedureHelper
    {
        #region ------- PLM_DW_Scripts_PlaceHolderTable

        public static void CheckPLM_DW_Scripts_PlaceHolderTable()
        {
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                string createPLMTable = @"

                       IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + PLMConstantString.PLM_DW_TabGridScripContainerTable + @"]') AND type in (N'U')) " +
                       " DROP TABLE [dbo].[" + PLMConstantString.PLM_DW_TabGridScripContainerTable + @"]" + System.Environment.NewLine +

                        @"CREATE TABLE [dbo].[" + PLMConstantString.PLM_DW_TabGridScripContainerTable + @"](
	                    [DWScriptID] [int] IDENTITY(1,1) NOT NULL,
	                    [TabID] [int] NULL,
	                    [TabName] [nvarchar](200) NULL,
	                    [GridID] [int] NULL,
	                    [GridName] [nchar](200) NULL,
	                    [EntityID] [int] NULL,
	                    [EntityName] [nchar](200) NULL,
	                    [InserIntoSQLScript] [nvarchar](max) NULL,
	                    [IsPassValidation] [bit] NULL,
	                    [IsNeedToGenerateDWTable] [bit] NULL,
	                    [DWTableName] [nvarchar](800) NULL,
	                    [RootLevelSelectSQLScript] [nvarchar](max) NULL,
	                    [DWTableSchemeScript] [nvarchar](max) NULL
                    ) ON [PRIMARY]";

                SqlCommand cmd = new SqlCommand(createPLMTable, conn);
                cmd.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
