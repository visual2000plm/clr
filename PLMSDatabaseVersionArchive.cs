using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace PLMCLRTools
{
    public partial class PLMSDatabaseVersionArchive
    {

        public static readonly string PLM_DataVersionLog_ConnectionString = string.Empty;
        private static string _PLM_DataVersionLog_ServerAndDatabaseName = string.Empty;

        private static string PLM_DataVersionLog_ServerAndDatabaseName
        {
            get
            {
                if (_PLM_DataVersionLog_ServerAndDatabaseName == string.Empty)
                {
                    string plmVersionLogServer = string.Empty;
                    string plmVersionLogDataBase = string.Empty;

                    using (SqlConnection conn = new SqlConnection(PLM_DataVersionLog_ConnectionString))
                    {
                        conn.Open();
                        plmVersionLogServer = conn.DataSource;
                        plmVersionLogDataBase = conn.Database;
                    }

                    _PLM_DataVersionLog_ServerAndDatabaseName = "[" + plmVersionLogServer + "]." + "[" + plmVersionLogDataBase + "]." + "dbo.";
                }

                return _PLM_DataVersionLog_ServerAndDatabaseName;
            }
        }

        static PLMSDatabaseVersionArchive()
        {
            string versionLogConn = " SELECT SetupValue FROM pdmsetup WHERE setupCode='PLMDataVersionLogConnection'";

            using (SqlConnection plmconn_from_context = new SqlConnection("context connection=true"))
            {
                SqlCommand cmd = new SqlCommand(versionLogConn, plmconn_from_context);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable resultTabel = new DataTable();
                adapter.Fill(resultTabel);

                if (resultTabel.Rows.Count > 0)
                {
                    PLM_DataVersionLog_ConnectionString = resultTabel.Rows[0]["SetupValue"].ToString();
                }
            }

        }

        #region --------------Database Version Archive

     //   [Microsoft.SqlServer.Server.SqlProcedure]
        public static void DoPLMVersionLogFirstTime(int BackwardDaysFromToday)
        {
            GenerateVerlogDatabaseTableScheme();

            SqlContext.Pipe.Send("Don't stop it running");
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                string doLogVersiom =
              " ALTER TABLE pdmProductVersion NOCHECK CONSTRAINT ALL " + Environment.NewLine +
              "  ALTER TABLE pdmBlockSubItemValue NOCHECK CONSTRAINT ALL " + Environment.NewLine +
              "  ALTER TABLE pdmGridProductRow NOCHECK CONSTRAINT ALL " + Environment.NewLine +
              "  ALTER TABLE pdmGridProductValue NOCHECK CONSTRAINT ALL " + Environment.NewLine +
              "  ALTER TABLE pdmProductSnapShotVersion NOCHECK CONSTRAINT ALL " + Environment.NewLine +


              "   declare @VersionCreateDate datetime " + Environment.NewLine +
              "   SELECT @VersionCreateDate =  DATEADD(day,   " + BackwardDaysFromToday + "*-1, GetDate()) " + Environment.NewLine +

              //--print @VersionCreateDate

           "  declare      @blockVersionTable table( id int identity,  productreferenceid int, blockid int,versionCode int) " + Environment.NewLine +
            "   insert into     @blockVersionTable(productreferenceid,blockid,versionCode) " +
            "   SELECT     ProductReferenceID, BlockID,  MAX(VersionCode) AS versioncode " +
            "   FROM         dbo.pdmProductVersion " +
            "   GROUP BY ProductReferenceID, BlockID " +
            "   having count(VersionID)>1 " + Environment.NewLine +

              "   declare @maxiumID int " + Environment.NewLine +
              " select  @maxiumID = max(id) from    @blockVersionTable" + Environment.NewLine +

              //--select * from @blockVersionTable

              "   declare @countID int " + Environment.NewLine +

              " set @countID=1 " + Environment.NewLine +

              " while ( @countID<=@maxiumID) " +

              " begin " +
              " declare @productReferenceid int " +
              " declare @blockid int " +
              " declare @versionCode int " +
              " declare @versionId int " +

              "SET NOCOUNT  ON  " +
              "select @productReferenceid=productreferenceid, @blockid = blockid,@versionCode=versionCode from    @blockVersionTable where id=@countID " +

              //-- need to declare Version Table
              "declare @versionIDTable Table( versionID int) " +

              "insert into @versionIDTable " +
              "select VersionID from pdmProductVersion    where productreferenceid=@productReferenceid and blockid=@blockid and versioncode<@versionCode and CreateDate<@VersionCreateDate " +

              "declare @VersionCount int " +
              "select  @VersionCount=count(*) from  @versionIDTable " +

              "if( @VersionCount > 0) " +
              "begin " +

              //--print  @VersionCount
                    //--select versionID from  @versionIDTable

              "insert into " + PLM_DataVersionLog_ServerAndDatabaseName + "pdmProductVersion " +
              "select *  from pdmProductVersion    where versionID in ( select versionID from  @versionIDTable) " +

              //--Transfer into [pdmProductVersionLog]
              " insert into  " + PLM_DataVersionLog_ServerAndDatabaseName + "[pdmProductVersionLog] " +
              " select *  from  [pdmProductVersionLog]     where versionID in ( select versionID from  @versionIDTable) " +

              //-- Transfer into [pdmBlockSubItemValue]
              "insert into  " + PLM_DataVersionLog_ServerAndDatabaseName + "[pdmBlockSubItemValue] " +
              "select *  from  [pdmBlockSubItemValue]     where versionID in ( select versionID from  @versionIDTable) " +

              //---- [pdmProductSnapShot]

              //---- Transfer into [pdmProductSnapShot]
              "insert into  " + PLM_DataVersionLog_ServerAndDatabaseName + "[pdmProductSnapShotVersion] " +
              "select *  from  [pdmProductSnapShotVersion]      where versionID in ( select versionID from  @versionIDTable) " +

              //---- [pdmGridProductRow]

              //---- Transfer into [pdmGridProductRow]
              "insert into  " + PLM_DataVersionLog_ServerAndDatabaseName + "[pdmGridProductRow] " +
              "select *  from  [pdmGridProductRow]     where versionID in ( select versionID from  @versionIDTable) " +

              //--print 'pdmGridProductRow is done'

              //---- Transfer into [pdmGridProductValue]
              "insert into  " + PLM_DataVersionLog_ServerAndDatabaseName + "[pdmGridProductValue] " +
              "select *  from  [pdmGridProductValue]     where  " +
              "RowID in  ( select RowID  from  [pdmGridProductRow]     where versionID in ( select versionID from  @versionIDTable) ) " +

              //--print 'pdmGridProductValue is done'

              //---- Transfer into [pdmNotification]


              //--print 'delete pdmNotification is done'
              "delete  from  [pdmProductVersionLog]          where versionID in ( select versionID from  @versionIDTable) " +

              //--print 'delete [pdmProductVersionLog] is done'

              "delete  from  [pdmProductSnapShotVersion]     where versionID in ( select versionID from  @versionIDTable) " +

              //--print 'delete [[pdmProductSnapShotVersion]] is done'

              //--- can not use cscading delete here ??
              "delete  from  [pdmBlockSubItemValue]     where versionID in ( select versionID from  @versionIDTable) " +

              //--print 'delete [[[pdmBlockSubItemValue]]] is done'

              "delete from  [pdmGridProductValue]     where  " +
              "RowID in  ( select RowID  from  [pdmGridProductRow]     where versionID in ( select versionID from  @versionIDTable) ) " +

              //--print 'delete [[[[pdmGridProductValue]]]] is done'

              //---- Transfer into [pdmGridProductRow]
              "delete  from  [pdmGridProductRow]     where versionID in ( select versionID from  @versionIDTable) " +
                    //--print 'delete [[[[[pdmGridProductRow]]]]] is done'

              "delete from    [pdmProductVersion]           where versionID in ( select versionID from  @versionIDTable) " +

              //--print 'delete [[[[pdmProductVersion]]]] is done'

              //-- end row count > 0

              //--- for the need to clear @versionIDTable ASAP,
              "delete @versionIDTable " +
              "end  " +

              "select @countID=@countID+1 " +

              //-- end Block  @blockVersionTable
              "end " + Environment.NewLine +

              "ALTER TABLE pdmProductVersion CHECK CONSTRAINT ALL " + Environment.NewLine +
              "ALTER TABLE pdmBlockSubItemValue CHECK CONSTRAINT ALL " + Environment.NewLine +
              "ALTER TABLE pdmGridProductRow CHECK CONSTRAINT ALL " + Environment.NewLine +
              "ALTER TABLE pdmGridProductValue CHECK CONSTRAINT ALL " + Environment.NewLine +
              "ALTER TABLE pdmProductSnapShotVersion CHECK CONSTRAINT ALL " + Environment.NewLine +
              "ALTER TABLE pdmNotification CHECK CONSTRAINT ALL " + Environment.NewLine;

                SqlCommand cmd = new SqlCommand(doLogVersiom, conn);
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
        }

        private static void GenerateVerlogDatabaseTableScheme()
        {
            using (SqlConnection conn = new SqlConnection(PLM_DataVersionLog_ConnectionString))
            {
                conn.Open();
                string aquery = SetupDropTableCommand("pdmBlockSubItemValue") + System.Environment.NewLine +
                    SetupDropTableCommand("pdmGridProductRow") + System.Environment.NewLine +
                    SetupDropTableCommand("pdmGridProductValue") + System.Environment.NewLine +
                    SetupDropTableCommand("pdmNotification") + System.Environment.NewLine +
                    SetupDropTableCommand("pdmProductSnapShotVersion") + System.Environment.NewLine +
                    SetupDropTableCommand("pdmProductVersion") + System.Environment.NewLine +
                    SetupDropTableCommand("pdmProductVersionLog") + System.Environment.NewLine +
                    SetupDropTableCommand("pdmVersionLogRunHistory") + System.Environment.NewLine +

                     @" CREATE TABLE [dbo].[pdmBlockSubItemValue](
	                    [SubItemValueID] [int] NOT NULL,
	                    [SubItemID] [int] NOT NULL,
	                    [VersionID] [int] NOT NULL,
	                    [ValueShareType] [tinyint] NOT NULL,
	                    [ValueID] [int] NULL,
	                    [ValueDate] [datetime] NULL,
	                    [ValueText] [nvarchar](4000) NULL,
                     CONSTRAINT [PK_pdmBlockSubItemValue] PRIMARY KEY CLUSTERED
                    (
	                    [SubItemValueID] ASC
                    )
                    ) ON [PRIMARY] " + System.Environment.NewLine +

                    @" CREATE TABLE [dbo].[pdmGridProductRow](
	                    [RowID] [int] NOT NULL,
	                    [GridID] [int] NOT NULL,
	                    [VersionID] [int] NULL,
	                    [ValueShareType] [tinyint] NOT NULL,
	                    [Sort] [int] NULL,
	                    [ValueID] [int] NULL,
	                    [DynamicControlID] [int] NULL,
	                    [ValueText] [nvarchar](800) NULL,
	                    [RowValueGUID] [uniqueidentifier] NULL,
                     CONSTRAINT [PK_pdmGridProductRow] PRIMARY KEY CLUSTERED
                    (
	                    [RowID] ASC
                    )
                    ) ON [PRIMARY] " + System.Environment.NewLine +

                    @"CREATE TABLE [dbo].[pdmGridProductValue](
	                    [GridValueID] [int] NOT NULL,
	                    [RowID] [int] NOT NULL,
	                    [GridColumnID] [int] NULL,
	                    [ProductColumnID] [int] NULL,
	                    [ValueID] [int] NULL,
	                    [ValueDate] [datetime] NULL,
	                    [ValueText] [nvarchar](800) NULL,
                     CONSTRAINT [PK_pdmGridProductValue] PRIMARY KEY CLUSTERED
                    (
	                    [GridValueID] ASC
                    )
                    ) ON [PRIMARY] " + System.Environment.NewLine +



                    @"CREATE TABLE [dbo].[pdmProductSnapShotVersion](
	                    [SnapShotVersionID] [int] NOT NULL,
	                    [ProductSnapShotID] [int] NULL,
	                    [VersionID] [int] NULL,
                     CONSTRAINT [PK_pdmProductSnapShotVersion] PRIMARY KEY CLUSTERED
                    (
	                    [SnapShotVersionID] ASC
                    )
                    ) ON [PRIMARY] " + System.Environment.NewLine +

                    @"CREATE TABLE [dbo].[pdmProductVersion](
	                    [VersionID] [int] NOT NULL,
	                    [ProductReferenceID] [int] NOT NULL,
	                    [VersionCode] [int] NOT NULL,
	                    [TabID] [int] NULL,
	                    [BlockID] [int] NULL,
	                    [CreateDate] [datetime] NULL,
	                    [CreatedByID] [int] NULL,
	                    [UpdateDate] [datetime] NULL,
	                    [UpdateByID] [int] NULL,
	                    [VersionDesc] [nvarchar](800) NULL,
	                    [IsApproved] [bit] NULL,
	                    [Label] [nvarchar](50) NULL,
	                    [IsPassValidation] [bit] NULL,
	                    [VersionStatusType] [int] NULL,
	                    [StatusChangedBy] [int] NULL,
	                    [Reason] [nvarchar](800) NULL,
                     CONSTRAINT [PK_pdmProductVersion] PRIMARY KEY CLUSTERED
                    (
	                    [VersionID] ASC
                    )
                    ) ON [PRIMARY]" + System.Environment.NewLine +

                    @"CREATE TABLE [dbo].[pdmProductVersionLog](
	                    [VersionLogID] [int] NOT NULL,
	                    [VersionID] [int] NOT NULL,
	                    [RowIdentityName] [nvarchar](500) NULL,
	                    [FiledName] [nvarchar](200) NULL,
	                    [ModifiedValueBefore] [nvarchar](500) NULL,
	                    [ModifiedValueAfter] [nvarchar](500) NULL,
	                    [Action] [nvarchar](200) NULL,
                     CONSTRAINT [PK_pdmProductVersionLog] PRIMARY KEY CLUSTERED
                    (
	                    [VersionLogID] ASC
                    )
                    ) ON [PRIMARY] " + System.Environment.NewLine +

                    @"CREATE TABLE [dbo].[pdmVersionLogRunHistory](
	                    [VersionLogID] [int] IDENTITY(1,1) NOT NULL,
	                    [VersionLogStartDateTime] [datetime] NULL,
	                    [VersionLogEndDateTime] [datetime] NULL,
                     CONSTRAINT [PK_pdmVersionLogRunHistory] PRIMARY KEY CLUSTERED
                    (
	                    [VersionLogID] ASC
                    )
                    ) ON [PRIMARY] " + System.Environment.NewLine;

                SqlCommand aCmd = new SqlCommand(aquery, conn);
                aCmd.ExecuteNonQuery();
            }
        }

        private static string SetupDropTableCommand(string tableName)
        {
            //string droptableFullName = dbmsName + ".[dbo].[" + tableName + "] ";
            string dropDWTable = " IF  EXISTS (SELECT * FROM  sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + tableName + "]') AND type in (N'U')) " +
                   " DROP TABLE " + tableName + System.Environment.NewLine;
            return dropDWTable;
        }

        #endregion

    };

}

