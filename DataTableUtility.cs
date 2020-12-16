using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{
    public partial  class DataTableUtility
    {
        // The Project operator is used to "project" columns from a table.
        //In TSQL, this equates to explicitly naming the column/s we want in a Select clause.

        // Create a copy of the table
        //Prepare array for column names to remove.
        //Find Columns to remove and add to array.
        //Loop through array and remove from table
        //Return Table.

        public static DataTable Project(DataTable Table, DataColumn[] Columns, bool Include)
        {
            //     but the new DataTable created by the System.Data.DataTable.Clone()
            ////     method does not contain any DataRows.
            DataTable table = Table.Copy();

            table.TableName = "Project";

            int columns_to_remove = Include ? (Table.Columns.Count - Columns.Length) : Columns.Length;

            string[] columns = new String[columns_to_remove];

            int z = 0;

            for (int i = 0; i < table.Columns.Count; i++)
            {
                string column_name = table.Columns[i].ColumnName;

                bool is_in_list = false;

                for (int x = 0; x < Columns.Length; x++)
                {
                    if (column_name == Columns[x].ColumnName)
                    {
                        is_in_list = true;

                        break;
                    }
                }

                if (is_in_list ^ Include)

                    columns[z++] = column_name;
            }

            foreach (string s in columns)
            {
                table.Columns.Remove(s);
            }

            return table;
        }

        public static DataTable Project(DataTable Table, DataColumn[] Columns)
        {
            return Project(Table, Columns, true);
        }

        public static DataTable Project(DataTable Table, params     string[] Columns)
        {
            DataColumn[] columns = new DataColumn[Columns.Length];

            for (int i = 0; i < Columns.Length; i++)
            {
                columns[i] = Table.Columns[Columns[i]];
            }

            return Project(Table, columns, true);
        }

        public static DataTable Project(DataTable Table, bool Include, params string[] Columns)
        {
            DataColumn[] columns = new DataColumn[Columns.Length];

            for (int i = 0; i < Columns.Length; i++)
            {
                columns[i] = Table.Columns[Columns[i]];
            }

            return Project(Table, columns, Include);
        }

        //The implementation of the UNION operator is equivalent to the TSQL expresion UNION ALL
        //      Create new empty table
        //Add columns to empty table.
        // Loop through First table and add rows
        // Loop through Second table and add rows
        // Return Table.

        public static DataTable Union(DataTable First, DataTable Second)
        {
            //Result table

            DataTable table = new DataTable("Union");

            //Build new columns

            DataColumn[] newcolumns = new DataColumn[First.Columns.Count];

            for (int i = 0; i < First.Columns.Count; i++)
            {
                newcolumns[i] = new DataColumn(First.Columns[i].ColumnName, First.Columns[i].DataType);
            }

            //add new columns to result table

            table.Columns.AddRange(newcolumns);

            table.BeginLoadData();

            //Load data from first table

            foreach (DataRow row in First.Rows)
            {
                table.LoadDataRow(row.ItemArray, true);
            }

            //Load data from second table

            foreach (DataRow row in Second.Rows)
            {
                table.LoadDataRow(row.ItemArray, true);
            }

            table.EndLoadData();

            return table;
        }

        //The Product Method is the equivalent of the CROSS JOIN expression in TSQL
        //      Create new empty table
        //Add columns from First table to empty table.
        //Add columns from Secondtable to empty table. Rename if necessary
        //Loop through First table and for each row loop through Second table and add rows via array manipulation.
        //Return Table.

        public static DataTable Product(DataTable First, DataTable Second)
        {
            DataTable table = new DataTable("Product");

            //Add Columns from First

            for (int i = 0; i < First.Columns.Count; i++)
            {
                table.Columns.Add(new DataColumn(First.Columns[i].ColumnName, First.Columns[i].DataType));
            }

            //Add Columns from Second
            int columnCount = First.Columns.Count;

            for (int i = 0; i < Second.Columns.Count; i++)
            {
                //Beware Duplicates
                columnCount++;

                if (!table.Columns.Contains(Second.Columns[i].ColumnName))

                    table.Columns.Add(new DataColumn(Second.Columns[i].ColumnName, Second.Columns[i].DataType));

                else

                    table.Columns.Add(new DataColumn(Second.Columns[i].ColumnName + columnCount.ToString(), Second.Columns[i].DataType));
            }

            table.BeginLoadData();

            foreach (DataRow parentrow in First.Rows)
            {
                object[] firstarray = parentrow.ItemArray;

                foreach (DataRow childrow in Second.Rows)
                {
                    object[] secondarray = childrow.ItemArray;

                    object[] productarray = new object[firstarray.Length + secondarray.Length];

                    Array.Copy(firstarray, 0, productarray, 0, firstarray.Length);

                    Array.Copy(secondarray, 0, productarray, firstarray.Length, secondarray.Length);

                    table.LoadDataRow(productarray, true);
                }
            }

            table.EndLoadData();

            return table;
        }

        //        The DIFFERENCE Method has no equivalent in TSQL. // not in query
        //It is also refered to as MINUS and is simply all the rows that are in the First table but not the Second.

        public static DataTable Difference(DataTable First, DataTable Second)
        {
            //Create Empty Table

            DataTable table = new DataTable("Difference");

            //Must use a Dataset to make use of a DataRelation object

            using (DataSet ds = new DataSet())
            {
                //Add tables

                ds.Tables.AddRange(new DataTable[] { First.Copy(), Second.Copy() });

                //Get Columns for DataRelation

                DataColumn[] firstcolumns = new DataColumn[ds.Tables[0].Columns.Count];

                for (int i = 0; i < firstcolumns.Length; i++)
                {
                    firstcolumns[i] = ds.Tables[0].Columns[i];
                }

                DataColumn[] secondcolumns = new DataColumn[ds.Tables[1].Columns.Count];

                for (int i = 0; i < secondcolumns.Length; i++)
                {
                    secondcolumns[i] = ds.Tables[1].Columns[i];
                }

                //Create DataRelation

                DataRelation r = new DataRelation(string.Empty, firstcolumns, secondcolumns, false);

                ds.Relations.Add(r);

                //Create columns for return table

                for (int i = 0; i < First.Columns.Count; i++)
                {
                    table.Columns.Add(First.Columns[i].ColumnName, First.Columns[i].DataType);
                }

                //If First Row not in Second, Add to return table.

                table.BeginLoadData();

                foreach (DataRow parentrow in ds.Tables[0].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r);

                    if (childrows == null || childrows.Length == 0)

                        table.LoadDataRow(parentrow.ItemArray, true);
                }

                table.EndLoadData();
            }

            return table;
        }

        //This JOIN method is equivalent to the TSQL INNER JOIN expression using equality.
        //        Create new empty table
        //Create a DataSet and add tables.
        //Get a reference to Join columns
        //Create a DataRelation
        //Construct JOIN table columns
        //Using the DataRelation add rows with matching related rows using array manipulation
        //Return table

        //FJC = First Join Column

        //SJC = Second Join Column

        public static DataTable Join(DataTable First, DataTable Second, DataColumn[] FJC, DataColumn[] SJC)
        {
            //Create Empty Table

            DataTable table = new DataTable("Join");

            // Use a DataSet to leverage DataRelation

            using (DataSet ds = new DataSet())
            {
                //Add Copy of Tables

                ds.Tables.AddRange(new DataTable[] { First.Copy(), Second.Copy() });

                //Identify Joining Columns from First

                DataColumn[] parentcolumns = new DataColumn[FJC.Length];

                for (int i = 0; i < parentcolumns.Length; i++)
                {
                    parentcolumns[i] = ds.Tables[0].Columns[FJC[i].ColumnName];
                }

                //Identify Joining Columns from Second

                DataColumn[] childcolumns = new DataColumn[SJC.Length];

                for (int i = 0; i < childcolumns.Length; i++)
                {
                    childcolumns[i] = ds.Tables[1].Columns[SJC[i].ColumnName];
                }

                //Create DataRelation

                DataRelation r = new DataRelation(string.Empty, parentcolumns, childcolumns, false);

                ds.Relations.Add(r);

                //Create Columns for JOIN table

                for (int i = 0; i < First.Columns.Count; i++)
                {
                    table.Columns.Add(First.Columns[i].ColumnName, First.Columns[i].DataType);
                }

                int columnCount = First.Columns.Count;

                for (int i = 0; i < Second.Columns.Count; i++)
                {
                    //Beware Duplicates
                    columnCount++;
                    if (!table.Columns.Contains(Second.Columns[i].ColumnName))

                        table.Columns.Add(Second.Columns[i].ColumnName, Second.Columns[i].DataType);

                    else // if

                        //table.Columns.Add(Second.Columns[i].ColumnName + "_Second", Second.Columns[i].DataType);
                        table.Columns.Add(Second.Columns[i].ColumnName + columnCount.ToString(), Second.Columns[i].DataType);
                }

                //Loop through First table

                table.BeginLoadData();

                foreach (DataRow firstrow in ds.Tables[0].Rows)
                {
                    //Get "joined" rows, i need left join

                    DataRow[] childrows = firstrow.GetChildRows(r);

                    if (childrows != null && childrows.Length > 0)
                    {
                        object[] parentarray = firstrow.ItemArray;

                        foreach (DataRow secondrow in childrows)
                        {
                            object[] secondarray = secondrow.ItemArray;

                            object[] joinarray = new object[parentarray.Length + secondarray.Length];

                            Array.Copy(parentarray, 0, joinarray, 0, parentarray.Length);

                            Array.Copy(secondarray, 0, joinarray, parentarray.Length, secondarray.Length);

                            table.LoadDataRow(joinarray, true);
                        }
                    }
                    else // left join
                    {
                        object[] parentarray = firstrow.ItemArray;

                        object[] secondarray = Second.NewRow().ItemArray;

                        object[] joinarray = new object[parentarray.Length + secondarray.Length];

                        Array.Copy(parentarray, 0, joinarray, 0, parentarray.Length);

                        Array.Copy(secondarray, 0, joinarray, parentarray.Length, secondarray.Length);

                        table.LoadDataRow(joinarray, true);
                    }
                }

                table.EndLoadData();
            }

            return table;
        }

        // it always take left ( keep first table all rows)
        public static DataTable Join(DataTable First, DataTable Second, DataColumn FJC, DataColumn SJC)
        {
            return DataTableUtility.Join(First, Second, new DataColumn[] { FJC }, new DataColumn[] { SJC });
        }

        public static DataTable Join(DataTable First, DataTable Second, string FJC, string SJC)
        {
            return DataTableUtility.Join(First, Second, new DataColumn[] { First.Columns[FJC] }, new DataColumn[] { Second.Columns[SJC] });
        }

        //String SQL =
        //       "select o.customerID, c.CompanyName, p.productName, sum(od.quantity) as Qty " +
        //       " from orders o " +
        //       " inner join [order details] od on o.orderID = od.orderID " +
        //       " inner join Products p on od.ProductID = p.ProductID " +
        //       " inner join Customers c  on o.CustomerID = c.CustomerID " +
        //       " group by o.customerID, c.CompanyName, p.ProductName " +
        //       " order by o.customerID";
        //   conn = new SqlConnection("Server=(local);Database=Northwind;uid=sa;pwd=sa1234");
        //   conn.Open();
        //   com = new SqlCommand(SQL, conn);
        //   try
        //   {
        //       dg.DataSource  = Pivot(com.ExecuteReader(), "CustomerID", "ProductName", "Qty");
        //   }

        public static DataTable Pivot(IDataReader dataValues, string keyColumn, string pivotNameColumn, string pivotValueColumn)
        {
            DataTable tmp = new DataTable();

            DataRow r;

            string LastKey = "//dummy//";

            int i, pValIndex, pNameIndex;

            string s;

            bool FirstRow = true;

            // Add non-pivot columns to the data table:

            pValIndex = dataValues.GetOrdinal(pivotValueColumn);

            pNameIndex = dataValues.GetOrdinal(pivotNameColumn);

            for (i = 0; i <= dataValues.FieldCount - 1; i++)

                if (i != pValIndex && i != pNameIndex)

                    tmp.Columns.Add(dataValues.GetName(i), dataValues.GetFieldType(i));

            r = tmp.NewRow();

            // now, fill up the table with the data:

            while (dataValues.Read())
            {
                // see if we need to start a new row

                if (dataValues[keyColumn].ToString() != LastKey)
                {
                    // if this isn't the very first row, we need to add the last one to the table

                    if (!FirstRow)

                        tmp.Rows.Add(r);

                    r = tmp.NewRow();

                    FirstRow = false;

                    // Add all non-pivot column values to the new row:

                    for (i = 0; i <= dataValues.FieldCount - 3; i++)

                        r[i] = dataValues[tmp.Columns[i].ColumnName];

                    LastKey = dataValues[keyColumn].ToString();
                }

                // assign the pivot values to the proper column; add new columns if needed:

                s = dataValues[pNameIndex].ToString();

                //    if (!tmp.Columns.Contains(s))

                //        tmp.Columns.Add(s, dataValues.GetFieldType(pValIndex));

                //    if (!tmp.Columns.Contains(s))
                //        tmp.Columns.Add(s, dataValues.GetFieldType(pValIndex));

                //To:

                if (!tmp.Columns.Contains(s))
                {
                    DataColumn c = tmp.Columns.Add(s, dataValues.GetFieldType(pValIndex));
                    // set the index so that it is sorted properly:
                    int newOrdinal = c.Ordinal;
                    for (i = newOrdinal - 1; i >= dataValues.FieldCount - 2; i--)
                        if (c.ColumnName.CompareTo(tmp.Columns[i].ColumnName) < 0)
                            newOrdinal = i;
                    c.SetOrdinal(newOrdinal);
                }

                r[s] = dataValues[pValIndex];
            }

            // add that final row to the datatable:

            tmp.Rows.Add(r);

            // Close the DataReader

            dataValues.Close();

            // and that's it!

            return tmp;
        }

        //
                    //var rowcellValueList = from row in reference.CellValueList
                    //                       group row by new
                    //                       {
                    //                           RowId = row.RowId,

                    //                           Sort = row.Sort,
                    //                           RowValueGUId = row.RowValueGUId,
                    //                       } into rowGroup

                    //                       select new
                    //                       {
                    //                           RowId = rowGroup.Key.RowId,
                    //                           Sort = rowGroup.Key.Sort,
                    //                           RowValueGUId = rowGroup.Key.RowValueGUId,
                    //                           RowCellValueList = rowGroup.Select
                    //                           (
                    //                               row => new
                    //                               {
                    //                                   GridColumnId = row.GridColumnId,
                    //                                   ValueText = row.ValueText
                    //                               }

                    //                           ),

                    //                           Concanation = rowGroup.Select (o=>o.ValueText).Distinct ().Aggregate((current, next) => current + ", " + next)
                    //                       };

                    //foreach (var gridRow in rowcellValueList)
                    //{
                    //    SimpleGridProductRow aGridProductRowDto = new SimpleGridProductRow();
                    //    listDto.Add(aGridProductRowDto);
                    //    aGridProductRowDto.RowId = gridRow.RowId;
                    //    aGridProductRowDto.RowValueGuId = gridRow.RowValueGUId;
                    //    aGridProductRowDto.Sort = gridRow.Sort.HasValue ? gridRow.Sort.Value : 0;
                    //    foreach (var cellValue in gridRow.RowCellValueList)
                    //    {
                    //        object value = ControlTypeValueConverter.ConvertValueToObject(cellValue.ValueText, dictColumnIdControType[cellValue.GridColumnId]);
                    //        aGridProductRowDto.Add(cellValue.GridColumnId, value);
                    //    }
                    //}

        public static DataTable GetInversedDataTable(DataTable table,
            String columnUseAsX,
            String columnUseAsY,
            String AggegationColumnZ,
            String AggegationFuc,
            String nullValueForEmptyAggregation
            )
        {
            //Create a DataTable to Return
            DataTable returnTable = new DataTable();

            if (columnUseAsX == string.Empty)
                columnUseAsX = table.Columns[0].ColumnName;

            //Add a Column at the beginning of the table
            returnTable.Columns.Add(columnUseAsY);

            List<string> columnXValues = new List<string>();

            //Read all DISTINCT values from columnX Column in the provided DataTale
            foreach (DataRow dr in table.Rows)
            {
                String columnXTemp = dr[columnUseAsX].ToString();
                if (!columnXValues.Contains(columnXTemp))
                {
                    columnXValues.Add(columnXTemp);
                    returnTable.Columns.Add(columnXTemp);
                }
            }

            //  Verify if Y and Z Axis columns re provided

            if (columnUseAsY != string.Empty && AggegationColumnZ != string.Empty)
            {
                List<string> columnYValues = new List<string>();
                foreach (DataRow dr in table.Rows)
                {
                    if (!columnYValues.Contains(dr[columnUseAsY].ToString()))
                    {
                        columnYValues.Add(dr[columnUseAsY].ToString());
                    }
                }

                //Loop all Column Y Distinct Value

                foreach (string columnYValue in columnYValues)
                {
                    DataRow drReturn = returnTable.NewRow();
                    drReturn[0] = columnYValue;

                    // foreach column Y value, The rows are selected distincted
                    DataRow[] rows = table.Select((columnUseAsY + "='") + columnYValue + "'");

                    // group Yvalue ( Row value)
                    foreach (DataRow dr in rows)
                    {
                        String rowColumnTitle = dr[columnUseAsX].ToString();

                        //Read each column to fill the DataTable
                        foreach (DataColumn dc in returnTable.Columns)
                        {
                            if (dc.ColumnName == rowColumnTitle)
                            {
                                //'If Sum of Values is True it try to perform a Sum
                                //'If sum is not possible due to value types, the value displayed is the last one read
                                if (AggegationFuc != string.Empty)
                                {
                                    // if Aggeation= string.Contantion,Sum,Max,Min,First Row,

                                    try
                                    {
                                        drReturn[rowColumnTitle] = Convert.ToDecimal(drReturn[rowColumnTitle]) + Convert.ToDecimal(dr[AggegationColumnZ]);
                                    }
                                    catch
                                    {
                                        drReturn[rowColumnTitle] = dr[AggegationColumnZ];
                                    }
                                }

                                else
                                    drReturn[rowColumnTitle] = dr[AggegationColumnZ];
                            }
                        }
                    }

                    returnTable.Rows.Add(drReturn);
                }
            }

            if (nullValueForEmptyAggregation != string.Empty)
            {
                foreach (DataRow dr in returnTable.Rows)
                    foreach (DataColumn dc in returnTable.Columns)
                        if (dr[dc.ColumnName].ToString() == string.Empty)
                        {
                            dr[dc.ColumnName] = nullValueForEmptyAggregation;
                        }
            }

            return returnTable;
        }



        //dataBaseTableName must exsits first
        public static void BulkCopyDatatableToDatabaseTable(SqlConnection connection, DataTable sourceDataTable, string destinationDataBaseTableName, Dictionary<string, string> dictDataTableAndDatabaseTableMappingColumn)
        {

            try
            {
                SqlBulkCopy bulkCopy =
                    new SqlBulkCopy
                    (
                    connection,
                    SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.FireTriggers |
                    SqlBulkCopyOptions.UseInternalTransaction,
                    null
                    );

                if (dictDataTableAndDatabaseTableMappingColumn != null && dictDataTableAndDatabaseTableMappingColumn.Count > 0)
                {
                    foreach (var pair in dictDataTableAndDatabaseTableMappingColumn)
                    {

                        bulkCopy.ColumnMappings.Add(pair.Key, pair.Value);
                    }
                }

                // bulkCopy.BatchSize = 50;
                bulkCopy.DestinationTableName = destinationDataBaseTableName;
                bulkCopy.WriteToServer(sourceDataTable);
            }
            catch (Exception ex)
            {

                CLROutput.OutputDebug("connection" + connection.Database);
                CLROutput.OutputDebug("Exception" + ex.ToString());
            }

        }

        // filter token
        public static string FilterSQLDBInvalidChar(string tabGridColumnName)
        {
            return tabGridColumnName.
                Replace(' ', '_')
                .Replace('(', '_')
                .Replace(')', '_')
                .Replace('-', '_')
                .Replace('&', '_')
                .Replace("'", "")
                .Replace("#", "")
                .Replace("/", "_")
                .Replace('\\', '_')
                .Replace('.', '_')
                .Replace('$', '_')
                .Replace('*', '_')
                .Replace('%', '_')
                .Replace(',', '_')
                .Replace('+', '_')
                .Replace("delete", "DLT_")
                .Replace("FROM", "FRM")
                .Replace("SELECT", "SLCT"); ;
        }



        public static DataTable ConvertSimpleGridProductRowToDataTable(IEnumerable<SimpleGridProductRow> rowList, List<int> columnIds, DataTable table)
        {
           
            //

         

            Dictionary<int,int> dictGridColumnIdAndType =  PdmCacheManager.GetMutiplePdmGridMetaColumnEntityFromCache(columnIds).ToDictionary(o => o.GridColumnId, o => o.ColumnTypeId);    

            foreach (var gridRow in rowList)
            {
                DataRow dataRow = table.NewRow();

              dataRow[GridColumnConstantName.ProductReferenceID]=gridRow.ProductReferenceId;
              dataRow[GridColumnConstantName.RowValueGUID] = gridRow.RowValueGuId;
              dataRow[GridColumnConstantName.Sort] = gridRow.Sort;

              foreach (int columnId in columnIds)
                {
                    object value =ControlTypeValueConverter.ConvertValueToObject(gridRow[columnId], dictGridColumnIdAndType[columnId]);


                    if (value == null)
                    {
                        dataRow[columnId.ToString()] = DBNull.Value;
                    }
                    else
                    {
                        //  CLROutput.Output("convervalue=" + value);
                        dataRow[columnId.ToString()] = value;

                        //  CLROutput.Output("dataRow[subitem.id.ToString()]" + dataRow[subitem.id.ToString()]);
                    }

                  // need to remove !!!! !
                   // dataRow[columnId.ToString()] = value;
                    

                }

                table.Rows.Add(dataRow);

            }

            return table;



        }


        public static DataTable ConvertSimpleGridProductRowToDataTable(IEnumerable<SimpleGridProductRow> rowList)
        {
            DataTable table = new DataTable();

            List<int> columnIds = new List<int>();

            foreach (var row in rowList)
            {
                columnIds.AddRange(row.DictColumnCellValue.Keys);


            }

            // 





            table.Columns.Add(GridColumnConstantName.ProductReferenceID);

            table.Columns.Add(GridColumnConstantName.RowValueGUID);
            foreach (int columnId in columnIds.Distinct())
            {
                table.Columns.Add(columnId.ToString());

            }



            foreach (var gridRow in rowList)
            {
                DataRow dataRow = table.NewRow();
                dataRow[GridColumnConstantName.ProductReferenceID] = gridRow.ProductReferenceId;
                dataRow[GridColumnConstantName.RowValueGUID] = gridRow.RowValueGuId;

                foreach (int columnId in columnIds)
                {
                  //  string columnId = column.ColumnName;

                   // int gridRowcolumnId = int.Parse(columnId);
                    dataRow[columnId.ToString()] = gridRow[columnId];

                }

                table.Rows.Add(dataRow);

            }

            return table;



        }






    }

}
