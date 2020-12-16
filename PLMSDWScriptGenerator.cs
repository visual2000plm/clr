using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace PLMCLRTools
{
  public  class PLMSDWScriptGenerator
    {
        //public static readonly string ConvertValueTextToDateTime = @"dbo.ConvertValueTextToDateTime";
        //public static readonly string ConvertValueTextToDecimal = @"dbo.ConvertValueTextToDecimal";
        //public static readonly string ConvertValueTextToInt = @"dbo.ConvertValueTextToInt";

        #region-------------- Generate Tab Dataware housing

        // Generate ATb tab
        internal  static void GenerateTabDWScript()
        {
            Dictionary<int, string> aRootLevelTabIds = new Dictionary<int, string>();

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                //SqlContext.Pipe.Send(PLM_APP_ConnectionString);
                conn.Open();

                string qeuryTabBlockSubitem = @"   SELECT distinct TabID,TabName FROM pdmtab where TabID in 
                     (
 
                     select  tabID from pdmDWRequireTabAndGrid where blockID is null and GridID is null
 
                     ) ";

               
                SqlCommand cmd = new SqlCommand(qeuryTabBlockSubitem, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                System.Data.DataTable resultTabel = new DataTable();
                adapter.Fill(resultTabel);

                foreach (DataRow aRow in resultTabel.Rows)
                {
                    aRootLevelTabIds.Add((Int32)aRow["TabID"], aRow["TabName"].ToString());
                }
            }

            foreach (int rootTabID in aRootLevelTabIds.Keys)
            {
                string tabName = aRootLevelTabIds[rootTabID];

                InsertTabTableToPlaceHolderTableWithNewTablLevlCaseQuery(rootTabID, tabName);
            }
        }

        internal static void InsertTabTableToPlaceHolderTableWithNewTablLevlCaseQuery(int mainTabID, string tabName)
        {
            string DWTableName = string.Empty;
            string SQLRootLevelSelect = string.Empty;

            SqlString aREsult = GenerateMainTabInsertIntoWithCaseQuery(mainTabID, ref DWTableName, ref SQLRootLevelSelect);

            DWTableName = DWTableName.Trim();
            SQLRootLevelSelect = SQLRootLevelSelect.Trim();

            if (!aREsult.IsNull && aREsult.Value.Trim() != string.Empty)
            {
                using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
                {
                    conn.Open();

                    string qeuryDeleteTabTableInPlaceHolder = @" delete " + PLMConstantString.PLM_DW_TabGridScripContainerTable + "   where  TabID= " + mainTabID;

                    SqlCommand cmdDelete = new SqlCommand(qeuryDeleteTabTableInPlaceHolder, conn);
                    cmdDelete.ExecuteNonQuery();

                    // re-insert the insertinon string
                    string qeuryTabBlockSubitem = @" insert into " + PLMConstantString.PLM_DW_TabGridScripContainerTable + " ( TabID,TabName,InserIntoSQLScript,DWTableName,RootLevelSelectSQLScript)    values (@rootTabID,@TabName,@queryResult,@DWTableName,@SQLRootLevelSelect)";
                    SqlCommand cmd = new SqlCommand(qeuryTabBlockSubitem, conn);
                    cmd.Parameters.Add(new SqlParameter("@rootTabID", mainTabID));
                    cmd.Parameters.Add(new SqlParameter("@TabName", tabName));
                    cmd.Parameters.Add(new SqlParameter("@queryResult", aREsult));
                    cmd.Parameters.Add(new SqlParameter("@DWTableName", DWTableName));
                    cmd.Parameters.Add(new SqlParameter("@SQLRootLevelSelect", SQLRootLevelSelect));

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public static SqlString GenerateMainTabInsertIntoWithCaseQuery(int mainTabID, ref string dWTableName, ref  string sQLRootLevelSelect)
        {
            dWTableName = GetDWMainTabTableName(mainTabID);
            sQLRootLevelSelect = CreateOneMainTabLevelCaseQueryForAllRefs(mainTabID);
            if (sQLRootLevelSelect == string.Empty)
            {
                return string.Empty;
            }
            string insertIntoTab = " insert  into " + dWTableName;

            StringBuilder aTabLevelQuery = new StringBuilder();

            string rootleveInserInto = insertIntoTab + " " + sQLRootLevelSelect;
            aTabLevelQuery.Append(rootleveInserInto);




            return aTabLevelQuery.ToString();
        }



        internal static List<BlockSubitemClrUserDefineDto> GetOneMainTabBlockSubitemFullPathNameAndControlType(string anytabID, SqlConnection conn)
        {
            string qeuryTabBlockSubitem = string.Format(@"SELECT DISTINCT
                      TOP (100) PERCENT dbo.pdmBlockSubItem.SubItemID, dbo.pdmBlockSubItem.ControlType, dbo.pdmBlockSubItem.SubItemName, dbo.pdmBlockSubItem.EntityID,
                      dbo.pdmEntity.EntityCode, dbo.pdmEntity.SysTableName
FROM         dbo.pdmBlockSubItem INNER JOIN
                      dbo.pdmBlock ON dbo.pdmBlockSubItem.BlockID = dbo.pdmBlock.BlockID INNER JOIN
                      dbo.PdmTabBlock ON dbo.pdmBlock.BlockID = dbo.PdmTabBlock.BlockID INNER JOIN
                      dbo.pdmTab ON dbo.PdmTabBlock.TabID = dbo.pdmTab.TabID LEFT OUTER JOIN
                      dbo.pdmEntity ON dbo.pdmBlockSubItem.EntityID = dbo.pdmEntity.EntityID
WHERE     (dbo.pdmTab.TabID = {0}) AND (dbo.pdmBlockSubItem.ControlType IN (1, 2, 3, 4, 5, 7, 8, 9, 13, 15, 19, 20, 21, 23))
ORDER BY dbo.pdmBlockSubItem.SubItemID", anytabID);

            return RetriveBlockSubitemFullPathNameAndControlType(conn, qeuryTabBlockSubitem);
        }

        internal static List<BlockSubitemClrUserDefineDto> RetriveBlockSubitemFullPathNameAndControlType(SqlConnection conn, string qeuryTabBlockSubitem)
        {
            List<BlockSubitemClrUserDefineDto> aDictSubitemID_Name = new List<BlockSubitemClrUserDefineDto>();
            SqlCommand cmd = new SqlCommand(qeuryTabBlockSubitem, conn);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            System.Data.DataTable resultTabel = new DataTable();
            adapter.Fill(resultTabel);

            foreach (DataRow aRow in resultTabel.Rows)
            {
                int subItemID =(int) aRow["SubItemID"];
                string aStringSubItemId = subItemID.ToString();
                string subItemName = DataTableUtility.FilterSQLDBInvalidChar(aRow["SubItemName"].ToString()) + "_" + aStringSubItemId;

                //dbo.pdmBlockSubItem.EntityID dbo.pdmEntity.EntityCode, dbo.pdmEntity.SysTableName

                int? entityID = aRow["EntityID"] as int?;
           
                if (entityID.HasValue)
                {
                    string entityCode = DataTableUtility.FilterSQLDBInvalidChar(aRow["EntityCode"].ToString());
                    string systemTable = aRow["SysTableName"].ToString();

                    PdmEntityBlClrDto aEntityBlClrDto = PdmCacheManager.DictPdmEntityBlEntity[entityID.Value];

                    if (aEntityBlClrDto.EntityType == (int)EmEntityType.UserDefineTable)
                    {
                        subItemName += "_FK_" + PLMConstantString.PLM_DW_UserDefineTablePrefix + entityCode + "_" + entityID;
                    }
                    else
                    {
                        subItemName += "_FK_" + systemTable;
                    }
                }

                // need to

                int controlType = int.Parse(aRow["ControlType"].ToString());



                BlockSubitemClrUserDefineDto aDto = new BlockSubitemClrUserDefineDto();
                aDto.SubItemID = subItemID;
                aDto.SubItemName = subItemName;
                aDto.EntityID = entityID;
                aDto.ControlType  = controlType;

                //!!!!!!!!!!!!!!!!!!!!!!!


                aDictSubitemID_Name.Add(aDto);
            }

            return aDictSubitemID_Name;
        }

        internal static string GetDWMainTabTableName(int rootabID)
        {
            string aRootTabName = string.Empty;
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();
                string qeuryTabBlockSubitem = " SELECT TabName FROM pdmtab  WHERE tabid = " + rootabID;
                SqlCommand cmd = new SqlCommand(qeuryTabBlockSubitem, conn);
                aRootTabName = cmd.ExecuteScalar().ToString();
            }

            // remove  Space,  ( )

            aRootTabName = DataTableUtility.FilterSQLDBInvalidChar(aRootTabName);
            return string.Format("{0}" + aRootTabName + "_" + rootabID + "", PLMConstantString.PLM_DW_TabPreifxTableName);
        }

        // [Microsoft.SqlServer.Server.SqlProcedure]TODO 2012-11-07
        public static string CreateOneMainTabLevelCaseQueryForAllRefs(int anytabID)
        {
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                List<BlockSubitemClrUserDefineDto> listSubitems = GetOneMainTabBlockSubitemFullPathNameAndControlType(anytabID.ToString(), conn);

                if (listSubitems.Count == 0)
                    return string.Empty;

                StringBuilder aTabLevelQuery = new StringBuilder();
                aTabLevelQuery.Append(" SELECT TabID, ProductReferenceID,");

                string tabLevelSubItemInClause = string.Empty;

                //WHERE  SubItemID in (4583,4584)
                foreach (BlockSubitemClrUserDefineDto aSubitem in listSubitems)
                {
                    string aSubItemId = aSubitem.SubItemID.ToString();

                    tabLevelSubItemInClause += aSubItemId + ",";

                    string aCaseQuery = string.Empty;
                    if (aSubitem.ControlType == (int)EmControlType.DDL || aSubitem.ControlType == (int)EmControlType.Image)
                    {
                        aCaseQuery = string.Format("  {0}   (dbo.Concatenate ( case  when    subitemid={1} then ValueText    end )) as '{2}'  ,", PLMConstantString.ConvertValueTextToInt, aSubItemId, aSubitem.SubItemName);
                    }
                    else if (aSubitem.ControlType == (int)EmControlType.Date)
                    {
                        aCaseQuery = string.Format("  {0}   (dbo.Concatenate ( case  when    subitemid={1} then ValueText    end )) as '{2}'  ,", PLMConstantString.ConvertValueTextToDateTime, aSubItemId, aSubitem.SubItemName);
                    }

                    else if (
                                 aSubitem.ControlType  == (int)EmControlType.Numeric

                            )
                    {
                        aCaseQuery = string.Format("  {0}   ( dbo.Concatenate ( case  when    subitemid={1} then ValueText    end )) as '{2}'  ,", PLMConstantString.ConvertValueTextToDecimal, aSubItemId, aSubitem.SubItemName);
                    }
                    else
                    {
                        aCaseQuery = string.Format("   dbo.Concatenate ( case  when    subitemid={0} then ValueText    end ) as '{1}'  ,", aSubItemId, aSubitem.SubItemName);
                    }

                    aTabLevelQuery.Append(aCaseQuery);
                }

                aTabLevelQuery.Remove(aTabLevelQuery.Length - 1, 1);

                if (tabLevelSubItemInClause != string.Empty)
                {
                    tabLevelSubItemInClause = tabLevelSubItemInClause.Substring(0, tabLevelSubItemInClause.Length - 1);
                    tabLevelSubItemInClause = "  WHERE  SubItemID in ( " + tabLevelSubItemInClause + " ) " + " and TabID=" + anytabID;
                }

                aTabLevelQuery.Append(@" FROM PLM_DW_SimpleDCUValue " + tabLevelSubItemInClause + "  group by TabID, ProductReferenceID    ");



                return aTabLevelQuery.ToString();
            }
        }

        #endregion


        #region----------- Generate Grid level

        internal static void GenerateGridDWscript()
        {
            Dictionary<int, string> aGridList = new Dictionary<int, string>();

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

 //                SELECT distinct GridID,GridName FROM pdmGrid  WHERE GridName is not null and GridName<>''
 
 //and GridID in (select  GridID from pdmDWRequireTabAndGrid where blockID is not null and GridID is not null)
 //  order by GridName 

                string qeuryTabBlockSubitem = @" SELECT distinct GridID,GridName FROM pdmGrid  WHERE
                     GridID in (select  GridID from pdmDWRequireTabAndGrid ) and 
                   GridName is not null and GridName<>''  order by GridName  ";

                SqlCommand cmd = new SqlCommand(qeuryTabBlockSubitem, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                System.Data.DataTable resultTabel = new DataTable();
                adapter.Fill(resultTabel);

                foreach (DataRow aRow in resultTabel.Rows)
                {
                    aGridList.Add((Int32)aRow["GridID"], aRow["GridName"].ToString());
                }
            }

            foreach (int gridID in aGridList.Keys)
            {
                string gridName = aGridList[gridID];

                InsertGridTableToPlaceHolderTable(gridID, gridName);
            }
        }

        public static void InsertGridTableToPlaceHolderTable(int gridID, string gridName)
        {
            string DWTableName = string.Empty;
            string SQLRootLevelSelect = string.Empty;
            SqlString aREsult = GenerateGridLevelCaseQuery(gridID, ref DWTableName, ref SQLRootLevelSelect);

            DWTableName = DWTableName.Trim();
            SQLRootLevelSelect = SQLRootLevelSelect.Trim();

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                string qeuryDeleteGridTableInPlaceHolder = @" delete " + PLMConstantString.PLM_DW_TabGridScripContainerTable + "   where  GridID= " + gridID;

                SqlCommand cmdDelete = new SqlCommand(qeuryDeleteGridTableInPlaceHolder, conn);
                cmdDelete.ExecuteNonQuery();

                string qeuryTabBlockSubitem = @" insert into " + PLMConstantString.PLM_DW_TabGridScripContainerTable + " ( GridID,GridName,InserIntoSQLScript,DWTableName,RootLevelSelectSQLScript)    values (@gridID,@GridName,@queryResult,@DWTableName,@SQLRootLevelSelect)";
                SqlCommand cmd = new SqlCommand(qeuryTabBlockSubitem, conn);
                cmd.Parameters.Add(new SqlParameter("@gridID", gridID));
                cmd.Parameters.Add(new SqlParameter("@GridName", gridName));
                cmd.Parameters.Add(new SqlParameter("@queryResult", aREsult));
                cmd.Parameters.Add(new SqlParameter("@DWTableName", DWTableName));
                cmd.Parameters.Add(new SqlParameter("@SQLRootLevelSelect", SQLRootLevelSelect));

                cmd.ExecuteNonQuery();
            }
        }

        //[return: SqlFacet(MaxSize = -1)]
        //[SqlFunction(DataAccess = DataAccessKind.Read, SystemDataAccess = SystemDataAccessKind.Read)]
        public static SqlString GenerateGridLevelCaseQuery(int gridID, ref string tableName, ref string rootLevelSelect)
        {
            List<GridColumnClrUserDefineDto> aGridColumns = GetGridColumnIDAndName(gridID);

            tableName = GetDWGridIntoTalbeName(gridID);
            rootLevelSelect = GenerateGridDWQuery(gridID, aGridColumns);

            string IntoDWTableName = " insert  into  " + tableName + " ";

            StringBuilder aGridWithOutCopyTabQuery = new StringBuilder();
            aGridWithOutCopyTabQuery.Append(IntoDWTableName + rootLevelSelect);



            return aGridWithOutCopyTabQuery.ToString();

            //return string.Empty;
        }

      //Need to add TabID back as old one
        private static string GenerateGridDWQuery(int gridID, List<GridColumnClrUserDefineDto> aGridColumns)
        {
            StringBuilder aQuery = GenerateGridColumnCaseStatmentBeforeWhereClause(aGridColumns);

            aQuery.Append(@" WHERE Gridid=" + gridID);
            aQuery.Append(@" group by  ProductReferenceID ,BlockID,GridID,RowID,RowValueGUID,Sort ");
            return aQuery.ToString();
        }
        ////Need to add TabID back as old one
        private static StringBuilder GenerateGridColumnCaseStatmentBeforeWhereClause(List<GridColumnClrUserDefineDto> aGridColumns)
        {
            StringBuilder aQuery = new StringBuilder();
            aQuery.Append(" SELECT  ProductReferenceID,BlockID, GridID,  RowID, cast ( RowValueGUID as varchar(36)) as RowValueGUID ,Sort,");

            foreach (GridColumnClrUserDefineDto aGridColumnDTO in aGridColumns)
            {
                int aColumnID = aGridColumnDTO.GridColumnID;
                string columnName = aGridColumnDTO.ColumnName;

                string aCaseQuery = string.Empty;

                if (aGridColumnDTO.ColumnTypeId == (int)EmControlType.DDL || aGridColumnDTO.ColumnTypeId == (int)EmControlType.Image)
                {
                    aCaseQuery = string.Format(" {0}   ( dbo.Concatenate ( case  when    GridColumnID={1} then ValueText    end )) as '{2}'  ,", PLMConstantString.ConvertValueTextToInt, aColumnID, columnName);
                }
                else if (aGridColumnDTO.ColumnTypeId == (int)EmControlType.Date)
                {
                    aCaseQuery = string.Format(" {0}   ( dbo.Concatenate ( case  when    GridColumnID={1} then ValueText    end )) as '{2}'  ,", PLMConstantString.ConvertValueTextToDateTime, aColumnID, columnName);
                }

                else if (
                             aGridColumnDTO.ColumnTypeId == (int)EmControlType.Numeric

                        )
                {
                    aCaseQuery = string.Format(" {0}   ( dbo.Concatenate ( case  when    GridColumnID={1} then ValueText    end )) as '{2}'  ,", PLMConstantString.ConvertValueTextToDecimal, aColumnID, columnName);
                }
                else
                {
                    aCaseQuery = string.Format(" dbo.Concatenate ( case  when    GridColumnID={0} then ValueText    end ) as '{1}'  ,", aColumnID, columnName);
                }

                aQuery.Append(aCaseQuery);
            }

            aQuery.Remove(aQuery.Length - 1, 1);

            aQuery.Append(@" FROM PLM_DW_ComplexColumnValue ");



            return aQuery;
        }


        public static List<GridColumnClrUserDefineDto> GetGridColumnIDAndName(int gridID)
        {
            List<GridColumnClrUserDefineDto> aGridColumns = new List<GridColumnClrUserDefineDto>();

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                // call  udf function RetriveProductTabCopyRootTabID
                string qeuryGridColumn = string.Format(@"SELECT     PdmGridMetaColumn.GridColumnID, PdmGridMetaColumn.ColumnName, PdmGridMetaColumn.ColumnTypeID , pdmEntity.EntityCode, pdmEntity.EntityID, pdmEntity.SysTableName
                                                        FROM      PdmGridMetaColumn LEFT OUTER JOIN     pdmEntity ON PdmGridMetaColumn.EntityID = pdmEntity.EntityID WHERE GridID={0} ", gridID);

                SqlCommand cmd = new SqlCommand(qeuryGridColumn, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                System.Data.DataTable resultTabel = new DataTable();
                adapter.Fill(resultTabel);

                foreach (DataRow aRow in resultTabel.Rows)
                {
                    int aColumnID = (int)aRow["GridColumnID"];

                    string colName = DataTableUtility.FilterSQLDBInvalidChar(aRow["ColumnName"].ToString() + "_" + aColumnID);

                    int ? entityID = aRow["EntityID"] as int ?;
                    string entityCode = DataTableUtility.FilterSQLDBInvalidChar(aRow["EntityCode"].ToString());
                    string systemTable = aRow["SysTableName"].ToString();
                    if (entityID.HasValue )
                    {
                        PdmEntityBlClrDto aEntityBlClrDto = PdmCacheManager.DictPdmEntityBlEntity[entityID.Value];
                       if (aEntityBlClrDto.EntityType == (int)EmEntityType.UserDefineTable)
                        {
                            colName += "_FK_" + PLMConstantString.PLM_DW_UserDefineTablePrefix + entityCode + "_" + entityID;
                        }
                        else
                        {
                            colName += "_FK_" + systemTable;
                        }
                    }

                    int controlType = int.Parse(aRow["ColumnTypeID"].ToString());

                    GridColumnClrUserDefineDto aDto = new GridColumnClrUserDefineDto();
                    aDto.GridColumnID = aColumnID;
                    aDto.ColumnName = colName;
                    aDto.EntityID = entityID;
                    aDto.ColumnTypeId = controlType;

                    aGridColumns.Add(aDto);
                }
            }

            return aGridColumns;
        }

        internal static string GetDWGridIntoTalbeName(int gridID)
        {
            string aRootGridName = string.Empty;
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();
                string qeuryTabBlockSubitem = " SELECT GridName FROM pdmGrid  WHERE GridID = " + gridID;
                SqlCommand cmd = new SqlCommand(qeuryTabBlockSubitem, conn);
                aRootGridName = cmd.ExecuteScalar().ToString();
            }

            // remove  Space,  ( )

            aRootGridName = DataTableUtility.FilterSQLDBInvalidChar(aRootGridName); //.Replace(' ', '_').Replace('(', '_').Replace(')', '_').Replace('-', '_');
            return string.Format(" {0}" + aRootGridName + "_" + gridID + " ", PLMConstantString.PLM_DW_GridPreifxTableName);
            // return IntoDWTableName;
        }

        #endregion


        #region -----  Generate UserDefinTable Script

        //TODO list--------
        public static void GenerateUserDefinTableScript()
        {

            List<int> userDefineEntityIds = null;
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                int userDefineEntityType = (int)EmEntityType.UserDefineTable;
                string qeuryUserDefineEntity = @" SELECT EntityID FROM pdmentity WHERE EntityType =  " + userDefineEntityType;
                DataTable resultTabel = DataAcessHelper.GetDataTableQueryResult(conn, qeuryUserDefineEntity);
                 userDefineEntityIds = resultTabel.GetDistinctOneColumnValueIds("EntityID").ToList();
            }


           // List<PdmEntityClrUserDefineDto> listPdmEntityClrDto = null;

            List<PdmClrEntitySimpleStructureDto> listPdmEntityClrDto = PLMSEntityClrBL.GetEntityAndColumnStrcutureInfoList(userDefineEntityIds);

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

         

                string aEntiValueScript = string.Empty;
                foreach (PdmClrEntitySimpleStructureDto aEntityDTO in listPdmEntityClrDto)
                {

                    //  int entityId = aEntityDTO.EntityId; 
                    //  string entityCode = ;
                    aEntityDTO.EntityCode = DataTableUtility.FilterSQLDBInvalidChar(aEntityDTO.EntityCode);

                    GenetateEntityDTOQuery(aEntityDTO, conn);

                    string deleteOldEntityTable = @"delete  [pdmDWTabGridScriptSetting] where EntityID="+aEntityDTO.EntityId;


                    // string sqlSelect = aEntityDTO.SQLSelect;
                    string IntoUseDefineTablName = " insert   into " + aEntityDTO.DWUserDefineTableName + aEntityDTO.SQLSelect;


                    // save to dbms

                    string deleteAndInsert =   deleteOldEntityTable + System.Environment.NewLine  + @" insert into " + PLMConstantString.PLM_DW_TabGridScripContainerTable + " ( EntityID,EntityName,InserIntoSQLScript,DWTableName,RootLevelSelectSQLScript)    values (@EntityID,@EntityName,@queryResult,@DWTableName,@SQLRootLevelSelect)";
                    SqlCommand cmd = new  SqlCommand(deleteAndInsert, conn);
                    cmd.Parameters.Add(new SqlParameter("@EntityID", aEntityDTO.EntityId));
                    cmd.Parameters.Add(new SqlParameter("@EntityName", DataTableUtility.FilterSQLDBInvalidChar(aEntityDTO.EntityCode)));
                    cmd.Parameters.Add(new SqlParameter("@queryResult", IntoUseDefineTablName));
                    cmd.Parameters.Add(new SqlParameter("@DWTableName", aEntityDTO.DWUserDefineTableName));
                    cmd.Parameters.Add(new SqlParameter("@SQLRootLevelSelect", aEntityDTO.SQLSelect));
                    CLROutput.OutputDebug("aEntityDTO.entityCode" + aEntityDTO.EntityCode);
                    CLROutput.OutputDebug("aEntityDTO.SQLSelect" + aEntityDTO.SQLSelect);

                    cmd.ExecuteNonQuery();
                } // end of for eahc entity
            }
        }

        //TODO
        public static void GenetateEntityDTOQuery(PdmClrEntitySimpleStructureDto aEntityDTO, SqlConnection conn)
        {

            List<PdmEntityColumnClrUserDefineDto> listColumnDto = aEntityDTO.Columns;

            if (listColumnDto.Count == 0)
            {
                aEntityDTO.SQLSelect = " invalid column (column no exist !)";
                return;
            }


            StringBuilder aUserDefineTableQuery = new StringBuilder();
            aUserDefineTableQuery.Append(" SELECT RowID as ValueID , SortOrder ,");

            string userDefineColumnIdInClause = string.Empty;

            //WHERE  UserDefineEntityColumnID in (4583,4584)
            foreach (var columnClrDto in listColumnDto)
            {

             //   columnClrDto.ColumnName = DataTableUtility.FilterSQLDBInvalidChar(columnClrDto.ColumnName + "_" + columnClrDto.UserDefineEntityColumnID);

                columnClrDto.ColumnName = DataTableUtility.FilterSQLDBInvalidChar(columnClrDto.ColumnName );
                string aUserDefineEntityColumnID = columnClrDto.UserDefineEntityColumnID.ToString();

                userDefineColumnIdInClause += aUserDefineEntityColumnID + ",";

                string aCaseQuery = string.Empty;
                if (columnClrDto.UicontrolType == (int)EmControlType.DDL || columnClrDto.UicontrolType == (int)EmControlType.Image)
                {
                    aCaseQuery = string.Format("  {0}   (dbo.Concatenate ( case  when    UserDefineEntityColumnID={1} then ValueText    end )) as '{2}'  ,", PLMConstantString.ConvertValueTextToInt, aUserDefineEntityColumnID, columnClrDto.ColumnName);
                }
                else if (columnClrDto.UicontrolType == (int)EmControlType.Date)
                {
                    aCaseQuery = string.Format("  {0}   (dbo.Concatenate ( case  when    UserDefineEntityColumnID={1} then ValueText    end )) as '{2}'  ,", PLMConstantString.ConvertValueTextToDateTime, aUserDefineEntityColumnID, columnClrDto.ColumnName);
                }

                else if (
                             columnClrDto.UicontrolType == (int)EmControlType.Numeric

                        )
                {
                    aCaseQuery = string.Format("  {0}   ( dbo.Concatenate ( case  when    UserDefineEntityColumnID={1} then ValueText    end )) as '{2}'  ,", PLMConstantString.ConvertValueTextToDecimal, aUserDefineEntityColumnID, columnClrDto.ColumnName);
                }
                else
                {
                    aCaseQuery = string.Format("   dbo.Concatenate ( case  when    UserDefineEntityColumnID={0} then ValueText    end ) as '{1}'  ,", aUserDefineEntityColumnID, columnClrDto.ColumnName);
                }

                aUserDefineTableQuery.Append(aCaseQuery);
            }

            aUserDefineTableQuery.Remove(aUserDefineTableQuery.Length - 1, 1);

            if (userDefineColumnIdInClause != string.Empty)
            {
                userDefineColumnIdInClause = userDefineColumnIdInClause.Substring(0, userDefineColumnIdInClause.Length - 1);
                userDefineColumnIdInClause = "  WHERE  UserDefineEntityColumnID in ( " + userDefineColumnIdInClause + " ) " + " and EntityID=" + aEntityDTO.EntityId;
                aUserDefineTableQuery.Append(@" FROM  " + PLMSEntityClrBL.PdmUserDefineEntityRowCellValueView + userDefineColumnIdInClause + "  group by RowID, SortOrder    ");

                aEntityDTO.SQLSelect = aUserDefineTableQuery.ToString();
            }
            else
            {

                aEntityDTO.SQLSelect = "invalid column ";
            }

            // return aEntityDTO;
        }

        #endregion


    }
}
