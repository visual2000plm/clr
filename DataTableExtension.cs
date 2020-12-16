using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using PLMCLRTools;

namespace System.Data
{
    public static class Extensions
    {

        public static List<int> GetDistinctOneColumnValueIds(this DataTable dataTable, string columnName)
        {
            List<int> toRetun = new List<int>();
            foreach (DataRow row in dataTable.Rows)
            {               
                int outvalue;
                if (int.TryParse(row[columnName].ToString(), out outvalue))
                {
                    toRetun.Add(outvalue);
                }

            }
            return toRetun.Distinct ().ToList ();
        }


        public static DataTable SortDataTable(this DataTable dataTable, string sortColumn)
        {
           
            dataTable.DefaultView.Sort = sortColumn; //+ " " + direction;
            return dataTable.DefaultView.ToTable();
           
        }

        public static DataTable SortDataTable(this DataTable dataTable, Dictionary<string,string> dictSortColumnNameAndDirection)
        {
            string sortColumn = string.Empty;
            foreach (var pair in dictSortColumnNameAndDirection)
            {
                sortColumn = sortColumn + pair.Key + " " + pair.Value + ",";
            }
            if (sortColumn != string.Empty)
            {

                sortColumn = sortColumn.Substring(0, sortColumn.Length - 1);
            }
        
            dataTable.DefaultView.Sort = sortColumn; //+ " " + direction;
            return dataTable.DefaultView.ToTable();
         
            // desc
        }

        public static List<string> GetDistinctOneColumnStringValue(this DataTable dataTable, string columnName)
        {
            List<string> toRetun = new List<string>();
            foreach (DataRow row in dataTable.Rows)
            {
                string id = row[columnName] as string;
                if (id !=null)
                {
                    toRetun.Add(id);
                }

            }
            return toRetun.Distinct().ToList();
        }

        public static List<DataRow > AsDataRowEnumerable(this DataTable dataTable)
        {
            List<DataRow> list = new List<DataRow>();
            foreach (DataRow row in dataTable.Rows)
            {
                list.Add(row);
            }
            return list;
        }

        public static DataTable DeepClone(this DataTable dtOld)
        {
            DataTable dtNew = new DataTable(dtOld.TableName);

            Extensions.CreatingColumns(dtOld, dtNew);

            Extensions.CreatingRows(dtOld, dtNew);

            return dtNew;
        }
        private static void CreatingColumns(DataTable dtOld, DataTable dtNew)
        {
            DataColumn[] arr = new DataColumn[] { new DataColumn("Head") }
                .ToList()
                .Union<DataColumn>(dtOld.Rows.Cast<DataRow>()
                    .ToList()
                    .Select(row => new DataColumn(Convert.ToString(row[0])))
                    ).ToArray();

            dtNew.Columns.AddRange(arr);

        }
        private static void CreatingRows(DataTable dtOld, DataTable dtNew)
        {
            dtOld.Columns.Cast<DataColumn>()
                .ToList()
                .ForEach(a =>
                    dtNew.Rows.Add(new string[] { a.ColumnName }.Union(
                        dtOld.Rows.Cast<DataRow>()
                        .ToList()
                        .Select(row => row[a.ColumnName])).ToArray()));


        }

    }
}