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

        public static void  CreateDataBaseTableFromQuery(string query, SqlConnection conn, string datbaseTableName)
        {

            string sqlCrateTableStament = CreateSQlCreateTableStatmentFromQuery(query, conn, datbaseTableName);

            string dropTablecommand = DataAcessHelper.GetSQLDropTableCommand(datbaseTableName);

            sqlCrateTableStament = dropTablecommand + System.Environment.NewLine + sqlCrateTableStament;
            using (SqlCommand cmd = new SqlCommand(sqlCrateTableStament, conn))
            {
                cmd.ExecuteNonQuery();
            }
            

        }


        public static string CreateSQlCreateTableStatmentFromQuery(string query, SqlConnection conn, string datbaseTableName)
        {

            DataTable schemeTable = GetQueryScheme(query, conn);
          
            return GenerateSQlCreateStatmentFromSchmeTable(datbaseTableName, schemeTable, null);
            
        }


        public static DataTable GetQueryScheme(string query, SqlConnection conn)
        {

           
                string newQuery = "select top 1 * from   ( " + query + " ) as SchemeTable";
                using (SqlCommand cmd = new SqlCommand(newQuery, conn))
                {
                    // //There is already an open DataReader associated with this Command which must be closed first.
                    // otherwise it iwll thorwexception like 
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        return rd.GetSchemaTable();
                    }
                    
                }
                
            
        }


    
        public static string GenerateSQlCreateStatmentFromSchmeTable(string tableName, DataTable schema, int[] primaryKeys)
        {
            string sql = "CREATE TABLE [" + tableName + "] (\n";

            // columns
            foreach (DataRow column in schema.Rows)
            {
                bool? isHidden = column["IsHidden"] as bool?;
                if (! (isHidden.HasValue && isHidden.Value))
                {
                    sql += "\t[" + column["ColumnName"].ToString() + "] " + SQLGetType(column);

                    bool? isAllowDBNull = column["AllowDBNull"] as bool?;
                    if (!(isAllowDBNull.HasValue && isAllowDBNull.Value))
                    {
                        sql += " NOT NULL";
                    }

                                        

                    sql += ",\n";
                
                }
               

            }
            sql = sql.TrimEnd(new char[] { ',', '\n' }) + "\n";

            // primary keys
            string pk = ", CONSTRAINT PK_" + tableName + " PRIMARY KEY CLUSTERED (";
            bool hasKeys = (primaryKeys != null && primaryKeys.Length > 0);
            if (hasKeys)
            {
                // user defined keys
                foreach (int key in primaryKeys)
                {
                    pk += schema.Rows[key]["ColumnName"].ToString() + ", ";
                }
            }
            else
            {
                // check schema for keys
                string keys = string.Join(", ", GetPrimaryKeys(schema));
                pk += keys;
                hasKeys = keys.Length > 0;
            }
            pk = pk.TrimEnd(new char[] { ',', ' ', '\n' }) + ")\n";
            if (hasKeys) sql += pk;

            sql += ")";

            CLROutput.OutputDebug("SQl" + sql);
            return sql;
        }

        public static string GetCreateFromDataTableSQL(string tableName, DataTable table)
        {
            string sql = "CREATE TABLE [" + tableName + "] (\n";
            // columns
            foreach (DataColumn column in table.Columns)
            {
                sql += "[" + column.ColumnName + "] " + SQLGetType(column) + ",\n";
            }
            sql = sql.TrimEnd(new char[] { ',', '\n' }) + "\n";
            // primary keys
            if (table.PrimaryKey.Length > 0)
            {
                sql += "CONSTRAINT [PK_" + tableName + "] PRIMARY KEY CLUSTERED (";
                foreach (DataColumn column in table.PrimaryKey)
                {
                    sql += "[" + column.ColumnName + "],";
                }
                sql = sql.TrimEnd(new char[] { ',' }) + "))\n";
            }

            //if not ends with ")"
            if ((table.PrimaryKey.Length == 0) && (!sql.EndsWith(")")))
            {
                sql += ")";
            }

            return sql;
        }

        public static string[] GetPrimaryKeys(DataTable schema)
        {
            List<string> keys = new List<string>();

            foreach (DataRow column in schema.Rows)
            {
                bool? IsKey = column["IsKey"] as bool?;


                if (IsKey.HasValue && IsKey.Value)
                {
                    keys.Add(column["ColumnName"].ToString());
                }
                
            }

            return keys.ToArray();
        }

        // Return T-SQL data type definition, based on schema definition for a column
        public static string SQLGetType(object type, int columnSize, int numericPrecision, int numericScale)
        {
            switch (type.ToString())
            {
                case "System.String":
                    return "VARCHAR(" + ((columnSize == -1) ? "255" : (columnSize > 8000) ? "MAX" : columnSize.ToString()) + ")";

                case "System.Decimal":
                    if (numericScale > 0)
                        return "REAL";
                    else if (numericPrecision > 10)
                        return "BIGINT";
                    else
                        return "INT";

                case "System.Double":
                case "System.Single":
                    return "REAL";

                case "System.Int64":
                    return "BIGINT";

                case "System.Int16":
                case "System.Int32":
                    return "INT";

                case "System.DateTime":
                    return "DATETIME";

                case "System.Boolean":
                    return "BIT";

                case "System.Byte":
                    return "TINYINT";

                case "System.Guid":
                    return "UNIQUEIDENTIFIER";

                case "System.Byte[]":
                    if (columnSize == 8)
                    {
                        return "timestamp";
                    }
                    else
                    {
                        return "VARBinary(MAX)";
                    }

                default:
                    throw new Exception(type.ToString() + " not implemented.");
            }
        }

        // Overload based on row from schema table
        public static string SQLGetType(DataRow schemaRow)
        {
            return SQLGetType(schemaRow["DataType"],
                                int.Parse(schemaRow["ColumnSize"].ToString()),
                                int.Parse(schemaRow["NumericPrecision"].ToString()),
                                int.Parse(schemaRow["NumericScale"].ToString()));
        }

        // Overload based on DataColumn from DataTable type
        public static string SQLGetType(DataColumn column)
        {
            return SQLGetType(column.DataType, column.MaxLength, 10, 2);
        }


        public static string GenerateSqlInserts(List<string> aryColumns, DataTable dtTable, string sTargetTableName)
        {
            string sSqlInserts = string.Empty;
            StringBuilder sbSqlStatements = new StringBuilder(string.Empty);
            StringBuilder sbColumns = new StringBuilder(string.Empty);

            // create the columns portion of the INSERT statement						            
            foreach (string colname in aryColumns)
            {
                if (sbColumns.ToString() != string.Empty)
                    sbColumns.Append(", ");

                sbColumns.Append("[" + colname + "]");
            }

            // loop thru each record of the datatable
            foreach (DataRow drow in dtTable.Rows)
            {
                // loop thru each column, and include the value if the column is in the array
                StringBuilder sbValues = new StringBuilder(string.Empty);
                foreach (string col in aryColumns)
                {
                    if (sbValues.ToString() != string.Empty)
                        sbValues.Append(", ");

                    // need to do a case to check the column-value types(quote strings(check for dups first), convert bools)
                    string sType = string.Empty;
                    try
                    {
                        sType = drow[col].GetType().ToString();
                        switch (sType.Trim().ToLower())
                        {
                            case "system.boolean":
                                sbValues.Append((Convert.ToBoolean(drow[col]) == true ? "1" : "0"));
                                break;

                            case "system.string":
                                sbValues.Append(string.Format("'{0}'", DataAcessHelper.QuoteSQLString(drow[col])));
                                break;

                            case "system.datetime":
                                string sDateTime = DataAcessHelper.QuoteSQLString(drow[col]);
                                if (Validation.IsDateTime(sDateTime) == true)
                                    sDateTime = System.DateTime.Parse(sDateTime).ToString("yyyy-MM-dd HH:mm:ss");
                                else
                                    sDateTime = string.Empty;
                                sbValues.Append(string.Format("'{0}'", sDateTime));
                                break;

                            case "system.byte[]":
                                sbValues.Append(string.Format("'{0}'", Convert.ToBase64String((byte[])drow[col])));
                                break;

                            default:
                                if (drow[col] == System.DBNull.Value)
                                    sbValues.Append("NULL");
                                else
                                    sbValues.Append(Convert.ToString(drow[col]));
                                break;
                        }
                    }
                    catch
                    {
                        sbValues.Append(string.Format("'{0}'", DataAcessHelper.QuoteSQLString(drow[col])));
                    }
                }

                //   INSERT INTO Tabs(Name) 
                //      VALUES('Referrals')
                // write the insert line out to the stringbuilder
                string snewsql = string.Format("INSERT INTO [{0}]({1}) ", sTargetTableName, sbColumns.ToString());
                sbSqlStatements.Append(snewsql);
                sbSqlStatements.AppendLine();
                sbSqlStatements.Append('\t');
                snewsql = string.Format("VALUES({0});", sbValues.ToString());
                sbSqlStatements.Append(snewsql);
                sbSqlStatements.AppendLine();
                sbSqlStatements.AppendLine();
            }

            sSqlInserts = sbSqlStatements.ToString();
            return sSqlInserts;
        }

        public static string GenerateSqlUpdates(List<string> aryColumns, List<string> aryWhereColumns, DataTable dtTable, string sTargetTableName)
        {
            string sSqlUpdates = string.Empty;
            StringBuilder sbSqlStatements = new StringBuilder(string.Empty);
            StringBuilder sbColumns = new StringBuilder(string.Empty);

            // UPDATE table SET col1 = 3, col2 = 4 WHERE (select cols)
            // loop thru each record of the datatable
            foreach (DataRow drow in dtTable.Rows)
            {
                // VALUES clause:  loop thru each column, and include the value if the column is in the array
                StringBuilder sbValues = new StringBuilder(string.Empty);

                foreach (string col in aryColumns)
                {
                    StringBuilder sbNewValue = new StringBuilder("[" + col + "] = ");
                    if (sbValues.ToString() != string.Empty)
                        sbValues.Append(", ");

                    // need to do a case to check the column-value types(quote strings(check for dups first), convert bools)
                    string sType = string.Empty;
                    try
                    {
                        sType = drow[col].GetType().ToString();
                        switch (sType.Trim().ToLower())
                        {
                            case "system.boolean":
                                sbNewValue.Append((Convert.ToBoolean(drow[col]) == true ? "1" : "0"));
                                break;

                            case "system.string":
                                sbNewValue.Append(string.Format("'{0}'", DataAcessHelper.QuoteSQLString(drow[col])));
                                break;

                            case "system.datetime":
                                string sDateTime = DataAcessHelper.QuoteSQLString(drow[col]);
                                if (Validation.IsDateTime(sDateTime) == true)
                                    sDateTime = System.DateTime.Parse(sDateTime).ToString("yyyy-MM-dd HH:mm:ss");
                                else
                                    sDateTime = string.Empty;
                                sbNewValue.Append(string.Format("'{0}'", sDateTime));
                                break;

                            case "system.byte[]":
                                sbNewValue.Append(string.Format("'{0}'", Convert.ToBase64String((byte[])drow[col])));
                                break;

                            default:
                                if (drow[col] == System.DBNull.Value)
                                    sbNewValue.Append("NULL");
                                else
                                    sbNewValue.Append(Convert.ToString(drow[col]));
                                break;
                        }
                    }
                    catch
                    {
                        sbNewValue.Append(string.Format("'{0}'", DataAcessHelper.QuoteSQLString(drow[col])));
                    }

                    sbValues.Append(sbNewValue.ToString());
                }

                // WHERE clause:  loop thru each column, and include the value if the column is in the array
                StringBuilder sbWhereValues = new StringBuilder(string.Empty);
                foreach (string col in aryWhereColumns)
                {
                    StringBuilder sbNewValue = new StringBuilder("[" + col + "] = ");
                    if (sbWhereValues.ToString() != string.Empty)
                        sbWhereValues.Append(" AND ");

                    // need to do a case to check the column-value types(quote strings(check for dups first), convert bools)
                    string sType = string.Empty;
                    try
                    {
                        sType = drow[col].GetType().ToString();
                        switch (sType.Trim().ToLower())
                        {
                            case "system.boolean":
                                sbNewValue.Append((Convert.ToBoolean(drow[col]) == true ? "1" : "0"));
                                break;

                            case "system.string":
                                sbNewValue.Append(string.Format("'{0}'", DataAcessHelper.QuoteSQLString(drow[col])));
                                break;

                            case "system.datetime":
                                string sDateTime = DataAcessHelper.QuoteSQLString(drow[col]);
                                if (Validation.IsDateTime(sDateTime) == true)
                                    sDateTime = System.DateTime.Parse(sDateTime).ToString("yyyy-MM-dd HH:mm:ss");
                                else
                                    sDateTime = string.Empty;
                                sbNewValue.Append(string.Format("'{0}'", sDateTime));
                                break;

                            case "system.byte[]":
                                sbNewValue.Append(string.Format("'{0}'", Convert.ToBase64String((byte[])drow[col])));
                                break;

                            default:
                                if (drow[col] == System.DBNull.Value)
                                    sbNewValue.Append("NULL");
                                else
                                    sbNewValue.Append(Convert.ToString(drow[col]));
                                break;
                        }
                    }
                    catch
                    {
                        sbNewValue.Append(string.Format("'{0}'", DataAcessHelper.QuoteSQLString(drow[col])));
                    }

                    sbWhereValues.Append(sbNewValue.ToString());
                }

                // UPDATE table SET col1 = 3, col2 = 4 WHERE (select cols)
                // write the line out to the stringbuilder
                string snewsql = string.Format("UPDATE [{0}] SET {1} WHERE {2};", sTargetTableName, sbValues.ToString(), sbWhereValues.ToString());
                sbSqlStatements.Append(snewsql);
                sbSqlStatements.AppendLine();
                sbSqlStatements.AppendLine();
            }

            sSqlUpdates = sbSqlStatements.ToString();
            return sSqlUpdates;
        }

        public static string GenerateSqlDeletes(List<string> aryColumns, DataTable dtTable, string sTargetTableName)
        {
            string sSqlDeletes = string.Empty;
            StringBuilder sbSqlStatements = new StringBuilder(string.Empty);

            // loop thru each record of the datatable
            foreach (DataRow drow in dtTable.Rows)
            {
                // loop thru each column, and include the value if the column is in the array
                StringBuilder sbValues = new StringBuilder(string.Empty);
                foreach (string col in aryColumns)
                {
                    StringBuilder sbNewValue = new StringBuilder("[" + col + "] = ");

                    if (sbValues.ToString() != string.Empty)
                        sbValues.Append(" AND ");

                    // need to do a case to check the column-value types(quote strings(check for dups first), convert bools)
                    string sType = string.Empty;
                    try
                    {
                        sType = drow[col].GetType().ToString();
                        switch (sType.Trim().ToLower())
                        {
                            case "system.boolean":
                                sbNewValue.Append((Convert.ToBoolean(drow[col]) == true ? "1" : "0"));
                                break;

                            case "system.string":
                                sbNewValue.Append(string.Format("'{0}'", DataAcessHelper.QuoteSQLString(drow[col])));
                                break;

                            case "system.datetime":
                                string sDateTime = DataAcessHelper.QuoteSQLString(drow[col]);
                                if (Validation.IsDateTime(sDateTime) == true)
                                    sDateTime = System.DateTime.Parse(sDateTime).ToString("yyyy-MM-dd HH:mm:ss");
                                else
                                    sDateTime = string.Empty;
                                sbNewValue.Append(string.Format("'{0}'", sDateTime));
                                break;

                            default:
                                if (drow[col] == System.DBNull.Value)
                                    sbNewValue.Append("NULL");
                                else
                                    sbNewValue.Append(Convert.ToString(drow[col]));
                                break;
                        }
                    }
                    catch
                    {
                        sbNewValue.Append(string.Format("'{0}'", DataAcessHelper.QuoteSQLString(drow[col])));
                    }

                    sbValues.Append(sbNewValue.ToString());
                }

                // DELETE FROM table WHERE col1 = 3 AND col2 = '4'
                // write the line out to the stringbuilder
                string snewsql = string.Format("DELETE FROM [{0}] WHERE {1};", sTargetTableName, sbValues.ToString());
                sbSqlStatements.Append(snewsql);
                sbSqlStatements.AppendLine();
                sbSqlStatements.AppendLine();
            }

            sSqlDeletes = sbSqlStatements.ToString();
            return sSqlDeletes;
        }

     
     

    }

}
