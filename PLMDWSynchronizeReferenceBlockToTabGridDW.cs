using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace PLMCLRTools
{
   public class PLMDWSynchronizeReferenceBlockToTabGridDW
    {

       // need to run this scrip in PLMS
       string PdmDWRequireTabAndGridGeneration = @"DELETE FROM pdmDWRequireTabAndGrid 

INSERT INTO pdmDWRequireTabAndGrid (Tabid)
SELECT distinct tabid FROM pdmTemplateTab

INSERT INTO pdmDWRequireTabAndGrid (TabID, GridID)
SELECT distinct pdmTabBlock.TabID, PdmGrid.GridID
FROM PdmGrid inner join pdmBlockSubItem on PdmGrid.GridID = pdmBlockSubItem.GridID
                  inner join pdmBlock     on    pdmBlockSubItem.BlockID = pdmBlock.BlockID
                  
                  inner join pdmTabBlock on pdmTabBlock.BlockID  = pdmBlock.BlockID
WHERE pdmTabBlock.TabID in (SELECT distinct tabid FROM pdmTemplateTab)

GO ";

        #region----------- Synchonized

   

        // [Microsoft.SqlServer.Server.SqlProcedure]
        public static void DoSynchronizeReferenceBlockToTabGridDWTo(string refids, string blockids)
        {
            if (string.IsNullOrEmpty(refids) || string.IsNullOrEmpty(blockids))
                return;

            #region------------ do Tab sych

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

                //InsertException(conn, "refids=" + refids + " blockids=" + blockids);
                CLROutput.OutputDebug("refids=" + refids + " blockids=" + blockids, conn); 



                string processTab = string.Format(@"
                      SELECT DISTINCT pdmTabBlock.TabID
                    FROM         pdmTabBlock INNER JOIN
                     pdmBlock  on pdmTabBlock.BlockID = pdmBlock.BlockID 
                     inner join 
                      pdmDWRequireTabAndGrid ON pdmDWRequireTabAndGrid.TabID = pdmTabBlock.TabID INNER JOIN
                      pdmBlockSubItem ON pdmTabBlock.BlockID = pdmBlockSubItem.BlockID
                    WHERE     (pdmBlockSubItem.GridID IS NULL) and    (pdmTabBlock.BlockID IN ({0}))", blockids);

                CLROutput.OutputDebug("processTabBlock=" + processTab , conn); 
               

                SqlCommand cmd = new SqlCommand(processTab, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                System.Data.DataTable resultTabel = new DataTable();
                adapter.Fill(resultTabel);

                foreach (DataRow aRow in resultTabel.Rows)
                {
                    //begin root leve

                    SqlInt32 aTabID = Converter.ToDDLSqlInt32(aRow["TabID"]);
                    ProcessSingleTab(aTabID, conn, refids);
                }
            }

            #endregion

            #region-----------  do Grid Transfer

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

//                string processGrid = string.Format(@"
// SELECT DISTINCT   dbo.pdmBlockSubItem.BlockID, dbo.pdmBlockSubItem.GridID, dbo.pdmTabField.TabID, dbo.pdmTab.ProductCopyTabRootTabID
//                                                                    FROM         dbo.pdmBlockSubItem INNER JOIN
//                                                                    dbo.pdmItem ON dbo.pdmBlockSubItem.BlockID = dbo.pdmItem.BlockID INNER JOIN
//                                                                    dbo.pdmTabField ON dbo.pdmItem.FieldID = dbo.pdmTabField.FieldID INNER JOIN
//                                                                    dbo.pdmTab ON dbo.pdmTabField.TabID = dbo.pdmTab.TabID
//                                                                     INNER JOIN  pdmDWRequireTabAndGrid on pdmDWRequireTabAndGrid.GridID = dbo.pdmBlockSubItem.GridID and pdmDWRequireTabAndGrid.TabID=dbo.pdmTab.TabID
//                                                                WHERE     (dbo.pdmBlockSubItem.GridID IS NOT NULL) and  dbo.pdmTab.ProductCopyTabRootTabID is null
//                                                                  AND      ( dbo.pdmBlockSubItem.BlockID IN ({0}))", blockids);



                string processGrid = string.Format(@"
 SELECT DISTINCT pdmBlockSubItem.BlockID, pdmBlockSubItem.GridID, PdmTabBlock.TabID
FROM         pdmDWRequireTabAndGrid INNER JOIN
                      pdmBlockSubItem ON pdmDWRequireTabAndGrid.GridID = pdmBlockSubItem.GridID INNER JOIN
                      PdmTabBlock ON pdmBlockSubItem.BlockID = PdmTabBlock.BlockID AND pdmDWRequireTabAndGrid.TabID = PdmTabBlock.TabID
               WHERE     (dbo.pdmBlockSubItem.GridID IS NOT NULL)   AND      ( dbo.pdmBlockSubItem.BlockID IN ({0}))", blockids);
           

                SqlCommand cmd = new SqlCommand(processGrid, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                System.Data.DataTable resultTabel = new DataTable();
                adapter.Fill(resultTabel);

                foreach (DataRow aRow in resultTabel.Rows)
                {
                    //dbo.pdmTab.TabID, dbo.pdmTab.ProductCopyTabRootTabID, dbo.pdmTab.ProductReferenceID

                    SqlInt32 aGridID = Converter.ToDDLSqlInt32(aRow["GridID"]);
                    SqlInt32 aTabID = Converter.ToDDLSqlInt32(aRow["TabID"]);
                    SqlInt32 aBlockID = Converter.ToDDLSqlInt32(aRow["BlockID"]);

                    string gridTablename = string.Empty;
                    if (PLMSDWStoredProcedures.GridTableName.ContainsKey(aGridID.Value))
                    {
                        gridTablename = PLMSDWStoredProcedures.GridTableName[aGridID.Value];

                        CLROutput.OutputDebug("GridTableName=" + gridTablename, conn); 

            

                    }

                    if (gridTablename != string.Empty)
                    {
                        //TabID,ProductReferenceID, GridID

                        string deleteOLDValue = string.Format(" delete {0} WHERE TabID={1} and ProductReferenceID in ({2}) and GridID={3} and BlockID = {4} ", gridTablename, aTabID, refids, aGridID, aBlockID);

                        try
                        {
                            SqlCommand deletcmd = new SqlCommand(deleteOLDValue, conn);
                            deletcmd.ExecuteNonQuery();
                        }
                        catch
                        {

                            CLROutput.OutputDebug("deleteGridFieldOLDValue=" + deleteOLDValue, conn); 

                          

                        }

                        string replaceWHERE = string.Format(" where TabID={0} and ProductReferenceID in ({1})   and BlockID = {2} and ", aTabID, refids, aBlockID);

                        string insertIntoGridTable = string.Empty;

                        string getRootGridSelectQuery = @"select top 1 RootlevelselectSQLscript from PLM_Dw_tabgridscript where gridid=" + aGridID + " and tabid is null";

                        SqlCommand cmdRootGrid = new SqlCommand(getRootGridSelectQuery, conn);
                        string rootLevelquery = cmdRootGrid.ExecuteScalar() as string;

                        //WHERE Gridid=7 group by TabID ,ProductReferenceID ,BlockID,GridID,RowID,RowValueGUID,Sort
                        insertIntoGridTable = " insert  into  " + gridTablename + " " + rootLevelquery.Replace("WHERE", replaceWHERE);

                        try
                        {
                            // SqlContext.Pipe.Send("exception insertcmd=" + insertIntoGridTable);
                            SqlCommand insertcmd = new SqlCommand(insertIntoGridTable, conn);

                            insertcmd.ExecuteNonQuery();


                            CLROutput.OutputDebug("insertIntoGridTable=" + insertIntoGridTable, conn); 

                         

                        }
                        catch (Exception ex)
                        {
                            // SqlContext.Pipe.Send("exception insertcmd=" + insertIntoGridTable);


                            CLROutput.OutputDebug("exception insertcmd failed=" + insertIntoGridTable +ex.ToString () , conn);

      
                        }
                    }
                }
            }

            #endregion
        }

        private static void ProcessSingleTab(SqlInt32 aTabID, SqlConnection conn, string refids)
        {
            string tabQuery = @"select top 1 RootlevelselectSQLscript  from PLM_Dw_tabgridscript where tabID=" + aTabID + " and gridid is null";

            SqlCommand cmdQueryTabNameAndSelectRootlevel = new SqlCommand(tabQuery, conn);
            string sQLTabLevelSelect = cmdQueryTabNameAndSelectRootlevel.ExecuteScalar() as string;

            CLROutput.OutputDebug ( "tabQuery=" + tabQuery,conn);
            CLROutput.OutputDebug("sQLTabLevelSelect=" + sQLTabLevelSelect, conn);
         
       


            string tabTablename = PLMSDWStoredProcedures.TabTableName[aTabID.Value];

      

            if (tabTablename != string.Empty)
            {
                string deleteOLDValue = string.Format(" delete {0} WHERE TabID={1} and ProductReferenceID in ({2}) ", tabTablename, aTabID, refids);

                SqlCommand deletcmd = new SqlCommand(deleteOLDValue, conn);


                CLROutput.OutputDebug("before deleteOLDValue=" + deleteOLDValue,conn);



                try
                {
                    deletcmd.ExecuteNonQuery();

                    CLROutput.OutputDebug( "after delete ok deleteOLDValue=" + deleteOLDValue, conn);
                 


                }
                catch (Exception ex)
                {

                      CLROutput.OutputDebug("delete failed deleteOLDValue=" + deleteOLDValue + ex.ToString(), conn);

                }

                string addReplaceAndRef = string.Format(" where  ProductReferenceID in ({0}) and  ", refids);

                string inserIntoSelect = " insert  into " + tabTablename + "  " + sQLTabLevelSelect.Replace("WHERE", addReplaceAndRef); ;

                CLROutput.OutputDebug("before inserIntoSelect=" + inserIntoSelect, conn);

         

                try
                {
                    SqlCommand insertcmd = new SqlCommand(inserIntoSelect, conn);
                    insertcmd.ExecuteNonQuery();

                 
                   CLROutput.OutputDebug( "after insert ok   inserIntoSelect=" + inserIntoSelect , conn);

                }
                catch (Exception ex)
                {

                    CLROutput.OutputDebug("exception insertcmd failed=" + inserIntoSelect + ex.ToString(), conn);

                }
            }
        }



      
        public static void DoSynchronizeUserDefineTableToDW(int entityID)
        {
            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();


                string tabQuery = @"select  RootlevelselectSQLscript from " +  PLMConstantString . PLM_DW_TabGridScripContainerTable + " where EntityID=" + entityID + " and IsPassValidation =1";

                SqlCommand cmdQueryTabNameAndSelectRootlevel = new SqlCommand(tabQuery, conn);
                string sQLTabLevelSelect = cmdQueryTabNameAndSelectRootlevel.ExecuteScalar() as string;





                string tabTablename = PLMSDWStoredProcedures.EntityTableName[entityID];



                if (tabTablename != string.Empty)
                {
                    string deleteOLDValue = string.Format(" delete {0}  ", tabTablename);

                    SqlCommand deletcmd = new SqlCommand(deleteOLDValue, conn);


                    CLROutput.OutputDebug("before delete Entity OLDValue=" + deleteOLDValue, conn);



                    try
                    {
                        deletcmd.ExecuteNonQuery();

                        CLROutput.OutputDebug("after delete ok deleteOLDValue=" + deleteOLDValue, conn);



                    }
                    catch (Exception ex)
                    {

                        CLROutput.OutputDebug("delete failed deleteOLDValue=" + deleteOLDValue + ex.ToString(), conn);

                    }

                 

                    string inserIntoSelect = " insert  into " + tabTablename + "  " + sQLTabLevelSelect ;

                    CLROutput.OutputDebug("before inserIntoSelect=" + inserIntoSelect, conn);



                    try
                    {
                        SqlCommand insertcmd = new SqlCommand(inserIntoSelect, conn);
                        insertcmd.ExecuteNonQuery();


                        CLROutput.OutputDebug("after insert ok   inserIntoSelect=" + inserIntoSelect, conn);

                    }
                    catch (Exception ex)
                    {

                        CLROutput.OutputDebug("exception insertcmd failed=" + inserIntoSelect + ex.ToString(), conn);

                    }
                }
            }
        }

        #endregion

    }
}


