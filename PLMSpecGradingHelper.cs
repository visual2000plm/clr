using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;

namespace PLMCLRTools
{
   public class PLMSpecGradingHelper
    {

       [Microsoft.SqlServer.Server.SqlProcedure]
       public static void GetSpecSize(int referenceId)
       {
           // Initianize context FROM PLM App,
           using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
           {
               //SqlContext.
               conn.Open();

              DataAcessHelper. ExecuteReadUnCommmited(conn);

               // Get SubitemID
              int baseSizeIndexFromZero = 0;
              int totalSizeNumber = 0;
              DataTable returnDataTble = GetSpecSizeRunTable(referenceId, conn, out baseSizeIndexFromZero, out totalSizeNumber);

               CLROutput.SendDataTable(returnDataTble);

               CLROutput.OutputDebug("baseSizeIndex=" + baseSizeIndexFromZero);
               CLROutput.OutputDebug("totalSizeNumber=" + totalSizeNumber);

               

               DataAcessHelper.ExecuteReadCommmited(conn);

           }
       }
  
       [Microsoft.SqlServer.Server.SqlProcedure]
       public static void GetSpecQcSize(int referenceId, int mainQcTabId)
       {
           // Initianize context FROM PLM App,
           using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
           {
               //SqlContext.
               conn.Open();

               DataAcessHelper.ExecuteReadUnCommmited(conn);

               // Get SubitemID
               //int baseSizeIndexFromZero = 0;
               //int totalSizeNumber = 0;
               DataTable returnDataTble = GetOneReferenceSpecQcSelectedSizeTable(referenceId, conn, mainQcTabId);

               CLROutput.SendDataTable(returnDataTble);

               //CLROutput.OutputDebug("baseSizeIndex=" + baseSizeIndexFromZero);
               //CLROutput.OutputDebug("totalSizeNumber=" + totalSizeNumber);



               DataAcessHelper.ExecuteReadCommmited(conn);

           }
       }

       [Microsoft.SqlServer.Server.SqlProcedure]
       public static void GetGradingSizeValue(int tabId, int currentGridBlockId, int referenceId)
       {
           //if (string.IsNullOrEmpty(referenceId))
           //    return;

           PdmBlockClrDto dmBlockClrDto = PdmCacheManager.DictBlockCache[currentGridBlockId];
           if (dmBlockClrDto.BlockPdmGridDto == null)
               return;

           bool IsGetAliasname =true;
           DataTable gridcolumnResultDataTable = PLMSGetGridValueHeler.LoadVariousGridColumnValue(tabId, currentGridBlockId, referenceId.ToString(), true, dmBlockClrDto, false, true, IsGetAliasname);

           int baseSizeIndexFromZero = 0;
           int totalSizeNumber = 0;



           using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
           {
               //SqlContext.  
               conn.Open();
               DataAcessHelper.ExecuteReadUnCommmited(conn);
               PLMSpecGradingHelper.GetSpecSizeRunTable(referenceId, conn, out baseSizeIndexFromZero, out totalSizeNumber);
               CLROutput.OutputDebug("baseSizeIndexFromZero" + baseSizeIndexFromZero.ToString() +"totalSizeNumber" + totalSizeNumber.ToString());

               DataAcessHelper.ExecuteReadCommmited(conn);

           }

           PdmGridClrDto aPdmGridClrDto = dmBlockClrDto.BlockPdmGridDto;

           int baseSizeColumnId = aPdmGridClrDto[GridRegister.GridSpecGrading.GradingBaseSize].GridColumnId;

           List<DataColumn> gradingSizeNameList = new List<DataColumn>();
           DataColumn baseSizeColumn = null;

           foreach (DataColumn column in gridcolumnResultDataTable.Columns)
           {
               if (column.ColumnName.StartsWith(GridRegister.GridSpecGrading.GradingSize))
               {
                   gradingSizeNameList.Add(column);
               }
               if (column.ColumnName.EndsWith("_" + baseSizeColumnId))
               {
                   baseSizeColumn = column;
               }

           }


           //GridRegister.GridSpecGrading.


           foreach (DataRow dataRow in gridcolumnResultDataTable.Rows)
           {
               List<double> gradingValue = new List<double>();
               List<String> columnName = new List<string>();
               double basesize = ControlTypeValueConverter.ConvertValueToDoubleWithDefautZero(dataRow[baseSizeColumn.ColumnName]);

               for (int i = 1; i <= totalSizeNumber; i++)
               {
                   foreach (DataColumn sizecolumn in gradingSizeNameList)
                   {
                       string baseSizeColumnName = GridRegister.GridSpecGrading.GradingSize + i.ToString() + "_";
                       if (sizecolumn.ColumnName.StartsWith(baseSizeColumnName))
                       {
                           double value = ControlTypeValueConverter.ConvertValueToDoubleWithDefautZero(dataRow[sizecolumn.ColumnName]);

                           gradingValue.Add(value);
                           columnName.Add(sizecolumn.ColumnName);
                       }
                   }

               }

               List<double> newValueList = PLMSpecGradingHelper.CaculateSizeValueWithGradingValue(gradingValue, basesize, baseSizeIndexFromZero);
               for (int i = 0; i < totalSizeNumber; i++)
               {
                   string columnNmae = columnName[i];
                   string value = newValueList[i].ToString();

                   dataRow[columnNmae] = value;


               }



           }


           List<DataColumn> needConvertColumn = new List<DataColumn>();


           foreach (DataColumn column in gridcolumnResultDataTable.Columns)
           {

               int baseSizeId = aPdmGridClrDto[GridRegister.GridSpecGrading.GradingBaseSize].GridColumnId;

               if (column.ColumnName.EndsWith("_" + baseSizeColumnId))
               {
                   needConvertColumn.Add(column);
               }

               int tolColumnId = aPdmGridClrDto[GridRegister.GridSpecGrading.Tolerance].GridColumnId;

               if (column.ColumnName.EndsWith("_" + tolColumnId))
               {
                   needConvertColumn.Add(column);
               }

               for (int i = 1; i <= 20; i++)
               {
                   int sizeColumnIdId = aPdmGridClrDto[GridRegister.GridSpecGrading.GradingSize + i.ToString()].GridColumnId;
                   if (column.ColumnName.EndsWith("_" + sizeColumnIdId))
                   {
                       needConvertColumn.Add(column);
                   }


               }



           }


           // need to get the bock size and unit of mesaure;
           //DefaultPOMUnitOfMeasure	1:INch, 2: Cm

           int? pomUnitOfmeasure = GetReferencePomOfUnitMeasure(referenceId);
           if (pomUnitOfmeasure.HasValue && pomUnitOfmeasure.Value == 1)
           {
               ConvertCMDatatableToInch(gridcolumnResultDataTable, needConvertColumn);
           }
           else // NO VALUE, NEED TO CHECK DEFAULT VALUE
           {
               if (PLMConstantString.DictPdmSetup["DefaultPOMUnitOfMeasure"].SetupValue == "1")
               {
                   ConvertCMDatatableToInch(gridcolumnResultDataTable, needConvertColumn);

               }
           
           }

        


           CLROutput.SendDataTable(gridcolumnResultDataTable);



       }

       private static void ConvertCMDatatableToInch(DataTable gridcolumnResultDataTable, List<DataColumn> needConvertColumn)
       {
           foreach (DataRow dataRow in gridcolumnResultDataTable.Rows)
           {

               foreach (DataColumn column in needConvertColumn)
               {
                   string cm = dataRow[column] as string;
                   dataRow[column] = UserDefinedFunctions.ConvertCentimeterToInch(cm);

                   // if(column.ColumnName.EndsWith(  
               }



           }
       }


       //   [Microsoft.SqlServer.Server.SqlProcedure]
       public static void GetSpecQcSizeAllCopySelectedSize(int referenceId, int mainQcTabId)
       {
           // Initianize context FROM PLM App,
           using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
           {
               //SqlContext.
               conn.Open();

               DataAcessHelper.ExecuteReadUnCommmited(conn);

               // Get SubitemID
               //int baseSizeIndexFromZero = 0;
               //int totalSizeNumber = 0;
               DataTable returnDataTble = GetSpecQcSelectedSizeForAllCopyTabsTable(referenceId, conn, mainQcTabId);

               CLROutput.SendDataTable(returnDataTble);

               //CLROutput.OutputDebug("baseSizeIndex=" + baseSizeIndexFromZero);
               //CLROutput.OutputDebug("totalSizeNumber=" + totalSizeNumber);

               DataAcessHelper.ExecuteReadCommmited(conn);

           }
       }


       private static int? GetReferencePomOfUnitMeasure(int referenceId)
       {
           string query = @"select PomUnitOfMeasure from pdmProduct where ProductReferenceID =@ProductReferenceID";

       
            List<SqlParameter> paraList = new List<SqlParameter> ();
           paraList.Add(new SqlParameter("@ProductReferenceID",referenceId));


           using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
           {
               //SqlContext.
               conn.Open();
              object value=  DataAcessHelper.RetriveSigleValue(conn, query, paraList);

             return ControlTypeValueConverter.ConvertValueToInt(value);
             

           }


       }


       public static DataTable GetOneReferenceSpecQcSelectedSizeTable(int referenceId, SqlConnection conn, int mainQcTabId)
       {
           Dictionary<string, int> dictSubitemInterlCodeAndId = SysDefinBlockHelper.GetDictInteralCodeSubItemId(conn, BlockRegister.SpecQCBlock.SpecQCBlockName);

 


           // Get  QC seelct Size
           //  string productInclause = DataAcessHelper.GenerateColumnInClauseWithAndCondition(allReferenceIds, "ProductReferenceID", true);
           string querySelectSize = @" select ProductReferenceID,SizeRunRotateID   from PdmProductQcSize where TabID =  " + mainQcTabId + " and ProductReferenceID= " + referenceId;

           DataTable selectSizeDataTable = DataAcessHelper.GetDataTableQueryResult(conn, querySelectSize);

          //  CLROutput.SendDataTable(selectSizeDataTable);

           Dictionary<int, List<int>> dictRefereneSelectSize = selectSizeDataTable.AsDataRowEnumerable().GroupBy(o => (int)o["ProductReferenceID"]).ToDictionary(g => g.Key, g => g.Select(r => (int)r["SizeRunRotateID"]).ToList());

           DataTable selectedSizeRotateTable = GetSpecQCBlockSelectSizeRotateTable(referenceId, conn);

          // CLROutput.SendDataTable(selectedSizeRotateTable);

        

           // Create Return table 
           DataTable returnDataTble = new DataTable();
           DataColumn tabNamecolumn = new DataColumn("ProductReferenceID", typeof(int));
           returnDataTble.Columns.Add(tabNamecolumn);

           for (int i = 1; i <= 20; i++)
           {
               DataColumn column = new DataColumn("Size" + i.ToString(), typeof(string));
               returnDataTble.Columns.Add(column);

           }

           if (dictRefereneSelectSize.ContainsKey(referenceId))
           {

               DataRow aRow = returnDataTble.NewRow();

               aRow["ProductReferenceID"] = referenceId;

               List<int> selectedSizeRotatedIds = dictRefereneSelectSize[referenceId];



               int rowCount = 1;
               foreach (DataRow row in selectedSizeRotateTable.Rows)
               {
                   int rizeRotateId = (int)row["SizeRunRotateID"];
                   if (selectedSizeRotatedIds.Contains(rizeRotateId))
                   {
                       aRow["Size" + rowCount.ToString()] = row["SizeName"];


                   }

                   rowCount++;
               }

               returnDataTble.Rows.Add(aRow);

           }


           return returnDataTble;
       }

       // will return base size
       //GetSpecQcSizeRunTable

       public static DataTable GetSpecQcSelectedSizeForAllCopyTabsTable(int mainReferenceId, SqlConnection conn ,int mainQcTabId)
       {
           Dictionary<string, int> dictSubitemInterlCodeAndId = SysDefinBlockHelper.GetDictInteralCodeSubItemId(conn, BlockRegister.SpecQCBlock.SpecQCBlockName);

           int sizeRunSubitemId = dictSubitemInterlCodeAndId[BlockRegister.SpecQCBlock.SpecFitSizeRun];
          // int selecdSizeRunSubitemId = dictSubitemInterlCodeAndId[BlockRegister.SpecQCBlock.SpecSelectedSize];
         //  int SpecFitBaseSizeSubitemId = dictSubitemInterlCodeAndId[BlockRegister.SpecQCBlock.SpecFitBaseSize];



           // Get MainTab SizeRunId
           string searchSubitemValue = @" select ValueText  from PdmSearchSimpleDcuValue where
                                                ( SubItemID =@sizeRunSubitemId  and ProductReferenceID = @ProductReferenceID  )    ";


           SqlCommand cmd = new SqlCommand(searchSubitemValue, conn);
           SqlParameter pararselecdSizeRunSubitemId = new SqlParameter("@sizeRunSubitemId", sizeRunSubitemId);
           cmd.Parameters.Add(pararselecdSizeRunSubitemId);
           SqlParameter paraMainReferenceId = new SqlParameter("@ProductReferenceID", mainReferenceId);
           cmd.Parameters.Add(paraMainReferenceId);

           DataTable mainTabSizeRun = DataAcessHelper.GetDataTableQueryResult(cmd);

           if (mainTabSizeRun.Rows.Count == 0)
               return new DataTable();

           int? sizeRunId = ControlTypeValueConverter.ConvertValueToInt(mainTabSizeRun.Rows[0]["ValueText"]);
           if (! sizeRunId.HasValue)
               return new DataTable ();
         

           // Get Copy Tab
           string queryCopyTab = @"SELECT     CopyTabID, MainTabID, CopyTabReferenceID, Name, Description, Sort, MainReferenceID
                FROM         pdmCopyTabReference
                WHERE     (MainReferenceID = @MainReferenceID) AND (MainTabID =@MainTabID)";

           List<SqlParameter> parameters = new List<SqlParameter> ();
           parameters.Add ( new SqlParameter ("@MainReferenceID",mainReferenceId));
           parameters.Add ( new SqlParameter ("@MainTabID",mainQcTabId));
           DataTable copyTabTable = DataAcessHelper.GetDataTableQueryResult(conn, queryCopyTab, parameters);

           List<int> allReferenceIds = new List<int>();
           allReferenceIds.Add(mainReferenceId);


           // refere copy tab mapping
            Dictionary<int, string> dictReferenceTabName = new Dictionary<int, string>();

             var pdmTabClrDto = PdmCacheManager.DictTabCache[mainQcTabId] ;
             dictReferenceTabName.Add(mainReferenceId, pdmTabClrDto.TabName);
          //   foreach ( DataRow row in 

           foreach (DataRow row in copyTabTable.Rows)
           {
               int copyTabRefId = (int)row["CopyTabReferenceID"];
               allReferenceIds.Add(copyTabRefId);
               dictReferenceTabName.Add(copyTabRefId, row["Name"].ToString ());
           }


           // Get  QC seelct Size
           string productInclause = DataAcessHelper.GenerateColumnInClauseWithAndCondition(allReferenceIds, "ProductReferenceID", true);
           string querySelectSize = @" select ProductReferenceID,SizeRunRotateID   from PdmProductQcSize where TabID =  " + mainQcTabId + productInclause;

           DataTable selectSizeDataTable = DataAcessHelper.GetDataTableQueryResult(conn, querySelectSize);

         //  CLROutput.SendDataTable(selectSizeDataTable);

           Dictionary<int, List<int>>  dictRefereneSelectSize = selectSizeDataTable.AsDataRowEnumerable().GroupBy(o => (int)o["ProductReferenceID"]).ToDictionary(g => g.Key, g => g.Select(r =>(int) r["SizeRunRotateID"]).ToList() );


           // get Size run table
         // DataTable sizeRotateTable = GetSizeRotateTableWithSizerunId(conn, sizeRunId.Value );


          DataTable sizeRotateTable = GetSpecGradingBlockSelectSizeRotateTable(mainReferenceId, conn);

                   


           // Create Return table 
           DataTable returnDataTble = new DataTable();
           DataColumn tabNamecolumn = new DataColumn("TabName", typeof(string));
           returnDataTble.Columns.Add(tabNamecolumn);
         
           for (int i = 1; i <= 20; i++)
           {
               DataColumn column = new DataColumn("Size" + i.ToString(), typeof(string));
               returnDataTble.Columns.Add(column);

           }

           foreach ( int refId in dictReferenceTabName.Keys )
           {
               DataRow aRow = returnDataTble.NewRow();
               aRow["TabName"] = dictReferenceTabName[refId];



               if (dictRefereneSelectSize.ContainsKey(refId))
               {

                   List<int> selectedSizeRotatedIds = dictRefereneSelectSize[refId];

                   int rowCount = 1;
                   foreach (DataRow row in sizeRotateTable.Rows)
                   {
                       int rizeRotateId = (int)row["SizeRunRotateID"];
                       if (selectedSizeRotatedIds.Contains(rizeRotateId))
                       {
                           aRow["Size" + rowCount.ToString()] = row["SizeName"];
                       

                       }

                       rowCount++;
                   }

               }


               returnDataTble.Rows.Add(aRow);

           
           }
           
           
           
           return returnDataTble;
       }

       public static DataTable GetSpecSizeRunTable(int referenceId, SqlConnection conn,  out int  baseSizeIndexFromZero, out int totalSizeNumber)
       {
           Dictionary<string, int> dictSubitemInterlCodeAndId = SysDefinBlockHelper.GetDictInteralCodeSubItemId(conn, BlockRegister.SpecGradingBlock.SpecGradingBlockName);
           int sizeRunSubitemId = dictSubitemInterlCodeAndId[BlockRegister.SpecGradingBlock.SpecFitSizeRun];
           int selecdSizeRunSubitemId = dictSubitemInterlCodeAndId[BlockRegister.SpecGradingBlock.SpecSelectedSize];
           int SpecFitBaseSizeSubitemId = dictSubitemInterlCodeAndId[BlockRegister.SpecGradingBlock.SpecFitBaseSize];

        

           // Get ValueText
           string SubitemInClause = @" select SubItemID ,ValueText  from PdmSearchSimpleDcuValue where
                                                (SubItemID = @sizeRunSubitemId or SubItemID=@selecdSizeRunSubitemId or   SubItemID=@SpecFitBaseSizeSubitemId )   and ProductReferenceID =@ProductReferenceID  ";

           SqlParameter paraSizeRunSubitemId = new SqlParameter("@sizeRunSubitemId", sizeRunSubitemId);
           SqlParameter pararselecdSizeRunSubitemId = new SqlParameter("@selecdSizeRunSubitemId", selecdSizeRunSubitemId);
           SqlParameter pararSpecFitBaseSizeSubitemId = new SqlParameter("@SpecFitBaseSizeSubitemId", SpecFitBaseSizeSubitemId);
           SqlParameter pararProductReferenceId = new SqlParameter("@ProductReferenceID", referenceId);

           SqlCommand cmd = new SqlCommand(SubitemInClause, conn);
           cmd.Parameters.Add(paraSizeRunSubitemId);
           cmd.Parameters.Add(pararselecdSizeRunSubitemId);
           cmd.Parameters.Add(pararSpecFitBaseSizeSubitemId);
           cmd.Parameters.Add(pararProductReferenceId);

           Dictionary<int, string> dictSubitemIdandValueText = DataAcessHelper.GetDataTableQueryResult(cmd).AsDataRowEnumerable().ToDictionary(row => (int)row["SubItemID"], row => row["ValueText"] as string);


           baseSizeIndexFromZero = 0;
           totalSizeNumber = 0;

           if (dictSubitemIdandValueText.Count == 0)
           {
               // need to get all size

               return new DataTable ();
           
           }

           string baseSizeId = string.Empty;

           if (dictSubitemIdandValueText.ContainsKey(SpecFitBaseSizeSubitemId))
           {
               baseSizeId = dictSubitemIdandValueText[SpecFitBaseSizeSubitemId];
           }


           DataTable returnDataTble = new DataTable();
           for (int i = 1; i <= 20; i++)
           {
               DataColumn column = new DataColumn("Size" + i.ToString(), typeof(string));
               returnDataTble.Columns.Add(column);

           }
           DataRow aRow = returnDataTble.NewRow();
           returnDataTble.Rows.Add(aRow);


           // Get Size run
           

           if (dictSubitemIdandValueText.Count > 0)
           {

               if (dictSubitemIdandValueText.ContainsKey(sizeRunSubitemId))
               {
                   string sizeRunId = dictSubitemIdandValueText[sizeRunSubitemId];
                   if (!string.IsNullOrEmpty(sizeRunId))
                   {
                       int sizerunValueId = int.Parse(sizeRunId);


                       DataTable sizeRotateTable = GetSizeRotateTableWithSizerunId(sizerunValueId);


                       string selectsizeRunId = dictSubitemIdandValueText[selecdSizeRunSubitemId];

                       if (!string.IsNullOrEmpty(selectsizeRunId) && selectsizeRunId.Length > 0)
                       {
                           ProcessSelectSized(ref baseSizeIndexFromZero, ref totalSizeNumber, baseSizeId, aRow, sizeRotateTable, ref selectsizeRunId);
                       }
                       
                       else //  NO seelcted Size , it show all size run tble
                       {
                           int rowCount = 1;
                           foreach (DataRow row in sizeRotateTable.Rows)
                           {

                               aRow["Size" + rowCount.ToString()] = row["SizeName"];

                               int rizeRotateId = (int)row["SizeRunRotateID"];

                               if (rizeRotateId.ToString() == baseSizeId)
                               {
                                   baseSizeIndexFromZero = rowCount-1;
                               }

                               rowCount++;

                           }

                           totalSizeNumber = sizeRotateTable.Rows.Count ;

                       }



                   }

               }

           }
           return returnDataTble;
       }


       public static DataTable GetSpecQCBlockSelectSizeRotateTable(int referenceId, SqlConnection conn)
       {
           Dictionary<string, int> dictSubitemInterlCodeAndId = SysDefinBlockHelper.GetDictInteralCodeSubItemId(conn, BlockRegister.SpecQCBlock.SpecQCBlockName);
           int sizeRunSubitemId = dictSubitemInterlCodeAndId[BlockRegister.SpecQCBlock.SpecFitSizeRun];
           int selecdSizeRunSubitemId = dictSubitemInterlCodeAndId[BlockRegister.SpecQCBlock.SpecSelectedSize];
           int SpecFitBaseSizeSubitemId = dictSubitemInterlCodeAndId[BlockRegister.SpecQCBlock.SpecFitBaseSize];


           DataTable returnDataTble = GetSelectSizeDataTable(referenceId, conn, sizeRunSubitemId, selecdSizeRunSubitemId, SpecFitBaseSizeSubitemId);
           return returnDataTble;
       }


       public static DataTable GetSpecGradingBlockSelectSizeRotateTable(int referenceId, SqlConnection conn)
       {
           Dictionary<string, int> dictSubitemInterlCodeAndId = SysDefinBlockHelper.GetDictInteralCodeSubItemId(conn, BlockRegister.SpecGradingBlock.SpecGradingBlockName);
           int sizeRunSubitemId = dictSubitemInterlCodeAndId[BlockRegister.SpecGradingBlock.SpecFitSizeRun];
           int selecdSizeRunSubitemId = dictSubitemInterlCodeAndId[BlockRegister.SpecGradingBlock.SpecSelectedSize];
           int SpecFitBaseSizeSubitemId = dictSubitemInterlCodeAndId[BlockRegister.SpecGradingBlock.SpecFitBaseSize];
           

           DataTable returnDataTble = GetSelectSizeDataTable(referenceId, conn, sizeRunSubitemId, selecdSizeRunSubitemId, SpecFitBaseSizeSubitemId);
           return returnDataTble;
       }

       private static DataTable GetSelectSizeDataTable(int referenceId, SqlConnection conn, int sizeRunSubitemId, int selecdSizeRunSubitemId, int SpecFitBaseSizeSubitemId)
       {
           // Get ValueText
           string SubitemInClause = @" select SubItemID ,ValueText  from PdmSearchSimpleDcuValue where
                                                (SubItemID = @sizeRunSubitemId or SubItemID=@selecdSizeRunSubitemId or   SubItemID=@SpecFitBaseSizeSubitemId )   and ProductReferenceID =@ProductReferenceID  ";

           SqlParameter paraSizeRunSubitemId = new SqlParameter("@sizeRunSubitemId", sizeRunSubitemId);
           SqlParameter pararselecdSizeRunSubitemId = new SqlParameter("@selecdSizeRunSubitemId", selecdSizeRunSubitemId);
           SqlParameter pararSpecFitBaseSizeSubitemId = new SqlParameter("@SpecFitBaseSizeSubitemId", SpecFitBaseSizeSubitemId);
           SqlParameter pararProductReferenceId = new SqlParameter("@ProductReferenceID", referenceId);

           SqlCommand cmd = new SqlCommand(SubitemInClause, conn);
           cmd.Parameters.Add(paraSizeRunSubitemId);
           cmd.Parameters.Add(pararselecdSizeRunSubitemId);
           cmd.Parameters.Add(pararSpecFitBaseSizeSubitemId);
           cmd.Parameters.Add(pararProductReferenceId);

           Dictionary<int, string> dictSubitemIdandValueText = DataAcessHelper.GetDataTableQueryResult(cmd).AsDataRowEnumerable().ToDictionary(row => (int)row["SubItemID"], row => row["ValueText"] as string);




           //if (dictSubitemIdandValueText.Count == 0)
           //{
           //    // need to get all size

           //    return new DataTable();

           //}


           DataTable returnDataTble = new DataTable();

           string baseSizeId = string.Empty;

           if (dictSubitemIdandValueText.ContainsKey(SpecFitBaseSizeSubitemId))
           {
               baseSizeId = dictSubitemIdandValueText[SpecFitBaseSizeSubitemId];
           }




           // Get Size run




           if (dictSubitemIdandValueText.Count > 0)
           {


               if (dictSubitemIdandValueText.ContainsKey(sizeRunSubitemId))
               {
                   string sizeRunId = dictSubitemIdandValueText[sizeRunSubitemId];
                   if (!string.IsNullOrEmpty(sizeRunId))
                   {
                       int sizerunValueId = int.Parse(sizeRunId);


                       returnDataTble = GetSizeRotateTableWithSizerunId( sizerunValueId);


                       string selectsizeRunId = dictSubitemIdandValueText[selecdSizeRunSubitemId];

                       if (!string.IsNullOrEmpty(selectsizeRunId) && selectsizeRunId.Length > 0)
                       {
                           selectsizeRunId = selectsizeRunId.Trim();
                           string[] selectSizeIds = selectsizeRunId.Split("|".ToCharArray());

                           List<int> selectedSizeRotatedIds = selectSizeIds.Select(o => int.Parse(o)).ToList();

                           var NootInRow = returnDataTble.AsDataRowEnumerable()
                              .Where(r => !selectedSizeRotatedIds.Contains((int)r["SizeRunRotateID"]));

                           foreach (DataRow aRow in NootInRow)
                           {
                               returnDataTble.Rows.Remove(aRow);

                           }


                       }




                   }

               }

           }
           return returnDataTble;
       }

       private static void ProcessSelectSized(ref int baseSizeIndexFromZero, ref int totalSizeNumber, string baseSizeId, DataRow aRow, DataTable sizeRotateTable, ref string selectsizeRunId)
       {
           selectsizeRunId = selectsizeRunId.Trim();
           string[] selectSizeIds = selectsizeRunId.Split("|".ToCharArray());



           if (selectSizeIds != null && selectSizeIds.Length > 0)
           {


               List<int> selectedSizeRotatedIds = selectSizeIds.Select(o => int.Parse(o)).ToList();

               int rowCount = 1;
               foreach (DataRow row in sizeRotateTable.Rows)
               {
                   int rizeRotateId = (int)row["SizeRunRotateID"];
                   if (selectedSizeRotatedIds.Contains(rizeRotateId))
                   {
                       aRow["Size" + rowCount.ToString()] = row["SizeName"];

                       // need to find base size index
                       if (rizeRotateId.ToString() == baseSizeId)
                       {
                           baseSizeIndexFromZero = rowCount - 1;
                       }

                       rowCount++;

                   }

               }

               totalSizeNumber = selectSizeIds.Length;

           }
           else //  NO seelcted Size , it show all size run tble
           {
               int rowCount = 1;

               //  totalSizeNumber = sizeRotateTable.Rows.Count ;
               foreach (DataRow row in sizeRotateTable.Rows)
               {

                   aRow["Size" + rowCount.ToString()] = row["SizeName"];

                   int rizeRotateId = (int)row["SizeRunRotateID"];

                   if (rizeRotateId.ToString() == baseSizeId)
                   {
                       baseSizeIndexFromZero = rowCount - 1;
                   }

                   rowCount++;

               }

               totalSizeNumber = sizeRotateTable.Rows.Count ;


           }
       }


       // Big size 
       private static DataTable GetSizeRotateTableWithSizerunId(int sizerunValueId)
       {

           var sizeEntity = PdmCacheManager.DictCodeKeyPdmEntityBlCache[EmEntityCode.SizeRunDetail.ToString() ];

           string connectionInfo = PLMSEntityClrBL.GetConnectionInfoWithCode(sizeEntity.DataSourceFrom);

           string querySizeRotate = @" select SizeRunRotateID ,SizeOrder  ,SizeName  from tblSizeRunRotate where SizeRunId = @sizeRunId order by SizeOrder ";
        
           // need to filter
           using (SqlConnection conn = new SqlConnection(connectionInfo))
           {
               //SqlContext.
               conn.Open();
               SqlCommand cmdSize = new SqlCommand(querySizeRotate, conn);
               SqlParameter paraSizeRunrunIdValueId = new SqlParameter("@sizeRunId", sizerunValueId);
               cmdSize.Parameters.Add(paraSizeRunrunIdValueId);
               DataTable sizeRotateTable = DataAcessHelper.GetDataTableQueryResult(cmdSize);
               return sizeRotateTable;
           }


        
       }


       public static List<double> CaculateSizeValueWithGradingValue(List<double> oneRowGradingValueList, double baseSizeValue, int baseIndexFromZero)
       {
           List<double> toRetrunSizeValueList = new List<double>();
           if (baseIndexFromZero == -1)
               return toRetrunSizeValueList;

           for (int i = 0; i < oneRowGradingValueList.Count; i++)
           {
               toRetrunSizeValueList.Add(0);
           }
           toRetrunSizeValueList[baseIndexFromZero] = baseSizeValue;

           //left
           for (int i = baseIndexFromZero; i >= 1; i--)
           {
               toRetrunSizeValueList[i - 1] = toRetrunSizeValueList[i] - oneRowGradingValueList[i - 1];
           } // right side
           for (int i = baseIndexFromZero; i < oneRowGradingValueList.Count - 1; i++)
           {
               toRetrunSizeValueList[i + 1] = toRetrunSizeValueList[i] + oneRowGradingValueList[i + 1];
           }

           return toRetrunSizeValueList;
       }

   
       //blockPdmGridDto, gridcolumnResultDataTable


       public static void SetupBodypartName(PdmGridClrDto aPdmGridClrDto, DataTable gridcolumnResultDataTable)
       {



           if (string.IsNullOrEmpty(aPdmGridClrDto.InternalCode))
               return;

           // get all colunName

         

           if (!(
                aPdmGridClrDto.InternalCode == GridRegister.GridSpecGrading.SpecGradingGrid
                || aPdmGridClrDto.InternalCode == GridRegister.GridSpecFit.SpecFitGrid
                || aPdmGridClrDto.InternalCode == GridRegister.GridSpecQC.SpecQCGrid

               ))
               return;


           string PomSketchIDColumnName = "PomSketchID";

           gridcolumnResultDataTable.Columns.Add(PomSketchIDColumnName);

           DataColumn bodypartIdColumn=null;
           DataColumn bodypartCodeColumn = null;
           DataColumn bodypartNameColumn = null;
           DataColumn bodypartDescColumn = null;
           DataColumn bodypartHowToColumn = null;


     

           foreach (DataColumn column in gridcolumnResultDataTable.Columns)
           {

               int idColumnId = aPdmGridClrDto[GridRegister.GridSpecFit.BodyPartDetailIDWDimDetailID].GridColumnId;

               if (column.ColumnName.EndsWith("_" + idColumnId))
               {
                   bodypartIdColumn = column;
               }



               int codeColumnId = aPdmGridClrDto[GridRegister.GridSpecFit.BodyPartCustomerCode].GridColumnId;

               if (column.ColumnName.EndsWith("_" + codeColumnId))
               {
                   bodypartCodeColumn = column;
               }



               int nameColumnId = aPdmGridClrDto[GridRegister.GridSpecFit.BodyPartName].GridColumnId;

               if (column.ColumnName.EndsWith("_" + nameColumnId))
               {
                   bodypartNameColumn = column;
               }


               int descColumnId = aPdmGridClrDto[GridRegister.GridSpecFit.BodyPartDesc].GridColumnId;

               if (column.ColumnName.EndsWith("_" + descColumnId))
               {
                   bodypartDescColumn = column;
               }


               int howtoColumnId = aPdmGridClrDto[GridRegister.GridSpecFit.HowToMeasure].GridColumnId;

               if (column.ColumnName.EndsWith("_" + howtoColumnId))
               {
                   bodypartHowToColumn = column;
               }
              
           }

          // gridcolumnResultDataTable.AsDataRowEnumerable ().Select (o=>o

           Dictionary<DataRow, int> dictRowAndBodypartIdForColumnResultDataTable = new Dictionary<DataRow, int>();
           List<int> bodypartIds   = new List<int> ();
           foreach (DataRow aRow in gridcolumnResultDataTable.Rows)
           {
               int? bodyparid = ControlTypeValueConverter.ConvertValueToInt(aRow[bodypartIdColumn]);
               if(bodyparid.HasValue )
               {
                   bodypartIds.Add (bodyparid.Value );
                   dictRowAndBodypartIdForColumnResultDataTable.Add(aRow,bodyparid.Value );
               }
                     
           }

           if (bodypartIds.Count > 0)
           {

               string query = @" select BodyPartID , Code , BodyPartName ,Description ,MeasureInstruction,SketchID from pdmV2kBodyPart ";
               string inclause = DataAcessHelper.GenerateColumnInClauseWithAndCondition(bodypartIds.Distinct(), "BodyPartID", false);
               query = query + " where " + inclause;

               DataTable bodypartDataTable = null;
               using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
               {

                   conn.Open();
                   bodypartDataTable = DataAcessHelper.GetDataTableQueryResult(conn, query);


               }

               Dictionary<int, DataRow> dictbodyRowAndId = new Dictionary<int, DataRow>();
               foreach (DataRow aDataRow in bodypartDataTable.Rows)
               {
                   dictbodyRowAndId.Add((int)aDataRow["BodyPartID"], aDataRow);


               }

               
//           EnablePomCodeSynchonizeWithProduct
//EnablePomNameSynchonizeWithProduct
//EnablePomDescriptionSynchonizeWithProduct
//EnablePomMeasureInstrcutionSynchonizeWithProduct

               bool isEnableCode = PLMConstantString.DictPdmSetup["EnablePomCodeSynchonizeWithProduct"].SetupValue.ToLower() == "true";
               bool isEnableName = PLMConstantString.DictPdmSetup["EnablePomNameSynchonizeWithProduct"].SetupValue.ToLower() == "true";
               bool isEnableDesc = PLMConstantString.DictPdmSetup["EnablePomDescriptionSynchonizeWithProduct"].SetupValue.ToLower() == "true";
               bool isEnableMeasure= PLMConstantString.DictPdmSetup["EnablePomMeasureInstrcutionSynchonizeWithProduct"].SetupValue.ToLower() == "true";


               foreach (DataRow gridRow in dictRowAndBodypartIdForColumnResultDataTable.Keys)
               {
                   int bodyId = dictRowAndBodypartIdForColumnResultDataTable[gridRow];
                   if (dictbodyRowAndId.ContainsKey(bodyId))
                   {

                       DataRow bodyparRow = dictbodyRowAndId[bodyId];
                       if (isEnableCode)
                       {
                           gridRow[bodypartCodeColumn] = bodyparRow["Code"] as string;
                       }

                       if (isEnableName)
                       {
                           gridRow[bodypartNameColumn] = bodyparRow["BodyPartName"] as string;
                       }

                       if (isEnableDesc)
                       {
                           gridRow[bodypartDescColumn] = bodyparRow["Description"] as string;
                       }

                       if (isEnableMeasure)
                       {
                           gridRow[bodypartHowToColumn] = bodyparRow["MeasureInstruction"] as string;
                       }


                        gridRow[PomSketchIDColumnName] = bodyparRow["SketchID"] as int?;
                       



                   }

               }
           
           }

           


       
       }
       public static void CheckSpecialColumnToInchOrCM(PdmGridClrDto aPdmGridClrDto, DataTable gridcolumnResultDataTable, int referenceId)
       {

           List<DataColumn> needConvertColumn = null;

           if (string.IsNullOrEmpty(aPdmGridClrDto.InternalCode))
               return;



           if (aPdmGridClrDto.InternalCode == GridRegister.GridSpecGrading.SpecGradingGrid)
           {
             needConvertColumn=  CollectiontGradingColumn(aPdmGridClrDto, gridcolumnResultDataTable);
           }
           else if (aPdmGridClrDto.InternalCode == GridRegister.GridSpecFit.SpecFitGrid)
           {
             needConvertColumn=  CollectiontFitColumn(aPdmGridClrDto, gridcolumnResultDataTable);
           }

           else if (aPdmGridClrDto.InternalCode == GridRegister.GridSpecQC.SpecQCGrid)
           {
               needConvertColumn = CollectiontQcColumn(aPdmGridClrDto, gridcolumnResultDataTable);
           }

           else
           {
               return;
           
           }

           
           // need to get the bock size and unit of mesaure;
           //DefaultPOMUnitOfMeasure	1:INch, 2: Cm

           int? pomUnitOfmeasure = GetReferencePomOfUnitMeasure(referenceId);
           if (pomUnitOfmeasure.HasValue && pomUnitOfmeasure.Value == 1)
           {
               ConvertCMDatatableToInch(gridcolumnResultDataTable, needConvertColumn);
           }
           else // NO VALUE, NEED TO CHECK DEFAULT VALUE
           {
               if (PLMConstantString.DictPdmSetup["DefaultPOMUnitOfMeasure"].SetupValue == "1")
               {
                   ConvertCMDatatableToInch(gridcolumnResultDataTable, needConvertColumn);

               }

           }

          

       }


       private static List<DataColumn> CollectiontFitColumn(PdmGridClrDto aPdmGridClrDto, DataTable gridcolumnResultDataTable)
       {
           List<DataColumn> needConvertColumn = new List<DataColumn>();


           foreach (DataColumn column in gridcolumnResultDataTable.Columns)
           {

               int initialSpecColumnId = aPdmGridClrDto[GridRegister.GridSpecFit.InitiaSpec].GridColumnId;

               if (column.ColumnName.EndsWith("_" + initialSpecColumnId))
               {
                   needConvertColumn.Add(column);
               }


               int initialSampleSpecColumnId = aPdmGridClrDto[GridRegister.GridSpecFit.SampleInitiaSpec].GridColumnId;

               if (column.ColumnName.EndsWith("_" + initialSampleSpecColumnId))
               {
                   needConvertColumn.Add(column);
               }



               int finalSpecColumnId = aPdmGridClrDto[GridRegister.GridSpecFit.FinalSpec].GridColumnId;

               if (column.ColumnName.EndsWith("_" + finalSpecColumnId))
               {
                   needConvertColumn.Add(column);
               }




               int tolColumnId = aPdmGridClrDto[GridRegister.GridSpecFit.Tolerance ].GridColumnId;

               if (column.ColumnName.EndsWith("_" + tolColumnId))
               {
                   needConvertColumn.Add(column);
               }

               for (int i = 1; i <= 6; i++)
               {
                   int revisedColumnId = aPdmGridClrDto[GridRegister.GridSpecFit.Revise + i.ToString()].GridColumnId;
                   if (column.ColumnName.EndsWith("_" + revisedColumnId))
                   {
                       needConvertColumn.Add(column);
                   }

                   int sampleColumnId = aPdmGridClrDto[GridRegister.GridSpecFit.Sample + i.ToString()].GridColumnId;
                   if (column.ColumnName.EndsWith("_" + sampleColumnId))
                   {
                       needConvertColumn.Add(column);
                   }

                   int samplesampleColumnId = aPdmGridClrDto[GridRegister.GridSpecFit.Sample + i.ToString() + i.ToString()].GridColumnId;
                   if (column.ColumnName.EndsWith("_" + samplesampleColumnId))
                   {
                       needConvertColumn.Add(column);
                   }


                   int diffColumnId = aPdmGridClrDto[GridRegister.GridSpecFit.Difference + i.ToString()].GridColumnId;
                   if (column.ColumnName.EndsWith("_" + diffColumnId))
                   {
                       needConvertColumn.Add(column);
                   }

                   int diffdiffColumnId = aPdmGridClrDto[GridRegister.GridSpecFit.Difference + i.ToString() + i.ToString()].GridColumnId;
                   if (column.ColumnName.EndsWith("_" + diffdiffColumnId))
                   {
                       needConvertColumn.Add(column);
                   }



                   //int diffSampleColumnId = aPdmGridClrDto[GridRegister.GridSpecFit.DiffSample + i.ToString()].GridColumnId;
                   //if (column.ColumnName.EndsWith("_" + diffSampleColumnId))
                   //{
                   //    needConvertColumn.Add(column);
                   //}
               }


           }
           return needConvertColumn;
       }

       private static List<DataColumn> CollectiontQcColumn(PdmGridClrDto aPdmGridClrDto, DataTable gridcolumnResultDataTable)
       {
           List<DataColumn> needConvertColumn = new List<DataColumn>();

           foreach (DataColumn column in gridcolumnResultDataTable.Columns)
           {

               int baseSizeColumnId = aPdmGridClrDto[GridRegister.GridSpecQC.GradingBaseSize].GridColumnId;

               if (column.ColumnName.EndsWith("_" + baseSizeColumnId))
               {
                   needConvertColumn.Add(column);
               }

               int tolColumnId = aPdmGridClrDto[GridRegister.GridSpecQC.Tolerance].GridColumnId;

               if (column.ColumnName.EndsWith("_" + tolColumnId))
               {
                   needConvertColumn.Add(column);
               }

               for (int i = 1; i <= 20; i++)
               {
                   int sizeColumnIdId = aPdmGridClrDto[GridRegister.GridSpecQC.GradingSize + i.ToString()].GridColumnId;
                   if (column.ColumnName.EndsWith("_" + sizeColumnIdId))
                   {
                       needConvertColumn.Add(column);
                   }

                   int qcColumnIdId = aPdmGridClrDto[GridRegister.GridSpecQC.QCSize + i.ToString()].GridColumnId;
                   if (column.ColumnName.EndsWith("_" + qcColumnIdId))
                   {
                       needConvertColumn.Add(column);
                   }

                   int qcdiffColumnIdId = aPdmGridClrDto[GridRegister.GridSpecQC.Difference + i.ToString()].GridColumnId;
                   if (column.ColumnName.EndsWith("_" + qcdiffColumnIdId))
                   {
                       needConvertColumn.Add(column);
                   }

               }


           }
       
           return needConvertColumn;
       }


       private static List<DataColumn> CollectiontGradingColumn(PdmGridClrDto aPdmGridClrDto, DataTable gridcolumnResultDataTable)
       {
           List<DataColumn> needConvertColumn = new List<DataColumn>();


           foreach (DataColumn column in gridcolumnResultDataTable.Columns)
           {

               int baseSizeColumnId = aPdmGridClrDto[GridRegister.GridSpecGrading.GradingBaseSize].GridColumnId;

               if (column.ColumnName.EndsWith("_" + baseSizeColumnId))
               {
                   needConvertColumn.Add(column);
               }

               int tolColumnId = aPdmGridClrDto[GridRegister.GridSpecGrading.Tolerance].GridColumnId;

               if (column.ColumnName.EndsWith("_" + tolColumnId))
               {
                   needConvertColumn.Add(column);
               }

               for (int i = 1; i <= 20; i++)
               {
                   int sizeColumnIdId = aPdmGridClrDto[GridRegister.GridSpecGrading.GradingSize + i.ToString()].GridColumnId;
                   if (column.ColumnName.EndsWith("_" + sizeColumnIdId))
                   {
                       needConvertColumn.Add(column);
                   }

               }


           }
           return needConvertColumn;
       }

    }
}
