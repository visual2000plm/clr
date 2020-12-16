using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using Microsoft.SqlServer.Server;

namespace PLMCLRTools
{
    public static class CLROutput
    {
        public static void SendDataSet(DataSet ds)
        {
            if (ds == null)
            {
                throw new ArgumentException("SendDataSet requires a non-null data set.");
            }
            else
            {
                foreach (DataTable dt in ds.Tables)
                {
                    SendDataTable(dt);
                }
            }
        }

        public static void SendDebugDataTable(DataTable dt)
        {
# if DEBUG

            bool[] coerceToString;
            // Do we need to coerce this column to string?
            // if could not find the exact type, force to(coece to string)
            SqlMetaData[] metaData = ExtractDataTableColumnMetaData(dt, out coerceToString);

            SqlDataRecord record = new SqlDataRecord(metaData);
            SqlPipe pipe = SqlContext.Pipe;
            pipe.SendResultsStart(record);
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    for (int index = 0; index < record.FieldCount; index++)
                    {
                        object value = row[index];
                        if (null != value && coerceToString[index])
                            value = value.ToString();
                        record.SetValue(index, value);
                    }

                    pipe.SendResultsRow(record);
                }
            }
            finally
            {
                pipe.SendResultsEnd();
            }

# endif

        }

        public static void SendDataTable(DataTable dt)
        {
            bool[] coerceToString;
            // Do we need to coerce this column to string?
            // if could not find the exact type, force to(coece to string)
            SqlMetaData[] metaData = ExtractDataTableColumnMetaData(dt, out coerceToString);

            SqlDataRecord record = new SqlDataRecord(metaData);
            SqlPipe pipe = SqlContext.Pipe;
            pipe.SendResultsStart(record);
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    for (int index = 0; index < record.FieldCount; index++)
                    {
                        object value = row[index];
                        if (null != value && coerceToString[index])
                            value = value.ToString();
                        record.SetValue(index, value);
                    }

                    pipe.SendResultsRow(record);
                }
            }
            finally
            {
                pipe.SendResultsEnd();
            }
        }

        public static void OutputDebug(string message, SqlConnection conn = null)
        {
# if DEBUG
            if (message.Length <= 4000)
            {
                SqlContext.Pipe.Send(message);
            }
            else
            {
                string trucatedmsg1 = message.Substring(0, 3950) + "Line1";
                SqlContext.Pipe.Send(trucatedmsg1);


                string trucatedmsg2 = message.Substring(message.Length - 4000);

                SqlContext.Pipe.Send(trucatedmsg2);


            }


            if (conn != null)
            {

                InsertException(conn, message);
            }



# endif
        }

        public static void OutputDisplayErrorMsg(string message, SqlConnection conn = null)
        {

            if (message.Length < 4000)
            {
                SqlContext.Pipe.Send(message);
            }
            else
            {

                message = message.Substring(0, 4000 - 10);

                SqlContext.Pipe.Send(message);
            }







        }

        private static void InsertException(SqlConnection conn, string message)
        {
            message = message.Replace("'", "''");
            string insertIntoGridTable = @" INSERT [dbo].pdmException
                (
	                ExceptionReport
                )
                      VALUES
                (
	                ' " + message + @" '
                )";


            using (SqlCommand insertcmd = new SqlCommand(insertIntoGridTable, conn))
            {
                insertcmd.ExecuteNonQuery(); ;
            }

        }

        private static SqlMetaData[] ExtractDataTableColumnMetaData(DataTable dt, out bool[] coerceToString)
        {
            SqlMetaData[] metaDataResult = new SqlMetaData[dt.Columns.Count];
            coerceToString = new bool[dt.Columns.Count];
            for (int index = 0; index < dt.Columns.Count; index++)
            {
                DataColumn column = dt.Columns[index];
                metaDataResult[index] = SqlMetaDataFromColumn(column, out coerceToString[index]);
            }

            return metaDataResult;
        }

        private static Exception InvalidDataTypeCode(TypeCode code)
        {
            return new ArgumentException("Invalid type: " + code);
        }

        private static Exception UnknownDataType(Type clrType)
        {
            return new ArgumentException("Unknown type: " + clrType);
        }

        private static SqlMetaData SqlMetaDataFromColumn(DataColumn column, out bool coerceToString)
        {
            coerceToString = false;
            SqlMetaData smd = null;
            Type clrType = column.DataType;
            string name = column.ColumnName;
            switch (Type.GetTypeCode(clrType))
            {
                case TypeCode.Boolean: smd = new SqlMetaData(name, SqlDbType.Bit); break;
                case TypeCode.Byte: smd = new SqlMetaData(name, SqlDbType.TinyInt); break;
                case TypeCode.Char: smd = new SqlMetaData(name, SqlDbType.NVarChar, 1); break;
                case TypeCode.DateTime: smd = new SqlMetaData(name, SqlDbType.DateTime); break;
                case TypeCode.DBNull: throw InvalidDataTypeCode(TypeCode.DBNull);
                case TypeCode.Decimal: smd = new SqlMetaData(name, SqlDbType.Decimal, 18, 5); break;
                case TypeCode.Double: smd = new SqlMetaData(name, SqlDbType.Float); break;
                case TypeCode.Empty: throw InvalidDataTypeCode(TypeCode.Empty);
                case TypeCode.Int16: smd = new SqlMetaData(name, SqlDbType.SmallInt); break;
                case TypeCode.Int32: smd = new SqlMetaData(name, SqlDbType.Int); break;
                case TypeCode.Int64: smd = new SqlMetaData(name, SqlDbType.BigInt); break;
                case TypeCode.SByte: throw InvalidDataTypeCode(TypeCode.SByte);
                case TypeCode.Single: smd = new SqlMetaData(name, SqlDbType.Real); break;
                case TypeCode.String:
                    if (column.MaxLength == -1)
                    {
                        smd = new SqlMetaData(name, SqlDbType.NVarChar, 4000);
                    }
                    else
                    {
                        smd = new SqlMetaData(name, SqlDbType.NVarChar, column.MaxLength);

                    }

                    break;
                case TypeCode.UInt16: throw InvalidDataTypeCode(TypeCode.UInt16);
                case TypeCode.UInt32: throw InvalidDataTypeCode(TypeCode.UInt32);
                case TypeCode.UInt64: throw InvalidDataTypeCode(TypeCode.UInt64);
                case TypeCode.Object:
                    smd = SqlMetaDataFromObjectColumn(name, column, clrType);
                    if (smd == null)
                    {
                        // Unknown type, try to treat it as string;
                        smd = new SqlMetaData(name, SqlDbType.NVarChar, column.MaxLength);
                        coerceToString = true;
                    }
                    break;

                default: throw UnknownDataType(clrType);
            }

            return smd;
        }

        private static SqlMetaData SqlMetaDataFromObjectColumn(string name, DataColumn column, Type clrType)
        {
            SqlMetaData smd = null;
            if (clrType == typeof(System.Byte[]) || clrType == typeof(SqlBinary) || clrType == typeof(SqlBytes) ||
        clrType == typeof(System.Char[]) || clrType == typeof(SqlString) || clrType == typeof(SqlChars))
                smd = new SqlMetaData(name, SqlDbType.VarBinary, column.MaxLength);
            else if (clrType == typeof(System.Guid))
                smd = new SqlMetaData(name, SqlDbType.UniqueIdentifier);
            else if (clrType == typeof(System.Object))
                smd = new SqlMetaData(name, SqlDbType.Variant);
            else if (clrType == typeof(SqlBoolean))
                smd = new SqlMetaData(name, SqlDbType.Bit);
            else if (clrType == typeof(SqlByte))
                smd = new SqlMetaData(name, SqlDbType.TinyInt);
            else if (clrType == typeof(SqlDateTime))
                smd = new SqlMetaData(name, SqlDbType.DateTime);
            else if (clrType == typeof(SqlDouble))
                smd = new SqlMetaData(name, SqlDbType.Float);
            else if (clrType == typeof(SqlGuid))
                smd = new SqlMetaData(name, SqlDbType.UniqueIdentifier);
            else if (clrType == typeof(SqlInt16))
                smd = new SqlMetaData(name, SqlDbType.SmallInt);
            else if (clrType == typeof(SqlInt32))
                smd = new SqlMetaData(name, SqlDbType.Int);
            else if (clrType == typeof(SqlInt64))
                smd = new SqlMetaData(name, SqlDbType.BigInt);
            else if (clrType == typeof(SqlMoney))
                smd = new SqlMetaData(name, SqlDbType.Money);
            else if (clrType == typeof(SqlDecimal))
                smd = new SqlMetaData(name, SqlDbType.Decimal, SqlDecimal.MaxPrecision, 0);
            else if (clrType == typeof(SqlSingle))
                smd = new SqlMetaData(name, SqlDbType.Real);
            else if (clrType == typeof(SqlXml))
                smd = new SqlMetaData(name, SqlDbType.Xml);
            else
                smd = null;

            return smd;
        }

    }

    //
    public class Converter
    {
        public static readonly string EmptyKeyValue = "-1";

        public static SqlByte ToSqlByte(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                try
                {
                    return SqlByte.Parse(obj.ToString());
                }
                catch { return SqlByte.Null; }
            }
            return SqlByte.Null;
        }

        public static SqlInt16 ToSqlInt16(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                try
                {
                    return SqlInt16.Parse(obj.ToString());
                }
                catch { return SqlInt16.Null; }
            }
            return SqlInt16.Null;
        }

        public static SqlInt32 ToSqlInt32(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                try
                {
                    return SqlInt32.Parse(obj.ToString());
                }
                catch { return SqlInt32.Null; }
            }
            return SqlInt32.Null;
        }

        public static SqlGuid ToSqlGuid(object obj)
        {
            //SqlGuid.tr

            if (obj != null && obj.ToString() != string.Empty)
            {
                try
                {
                    return SqlGuid.Parse(obj.ToString());
                }
                catch { return SqlGuid.Null; }
            }
            return SqlGuid.Null;
        }

        public static SqlInt32 ToDDLSqlInt32(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty && obj.ToString() != EmptyKeyValue)
            {
                try
                {
                    return SqlInt32.Parse(obj.ToString());
                }
                catch { return SqlInt32.Null; }
            }
            return SqlInt32.Null;
        }

        public static SqlInt16 ToDDLSqlInt16(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty && obj.ToString() != EmptyKeyValue)
            {
                try
                {
                    return SqlInt16.Parse(obj.ToString());
                }
                catch { return SqlInt16.Null; }
            }
            return SqlInt16.Null;
        }

        public static SqlByte ToDDLSqlByte(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty && obj.ToString() != EmptyKeyValue)
            {
                try
                {
                    return SqlByte.Parse(obj.ToString());
                }
                catch { return SqlByte.Null; }
            }
            return SqlByte.Null;
        }

        public static SqlDouble ToSqlDouble(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                try
                {
                    return SqlDouble.Parse(obj.ToString());
                }
                catch { return SqlDouble.Null; }
            }
            return SqlDouble.Null;
        }

        public static SqlDecimal ToSqlDecimal(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                try
                {
                    return SqlDecimal.Parse(obj.ToString());
                }
                catch { return SqlDecimal.Null; }
            }
            return SqlDecimal.Null;
        }

        public static SqlBoolean ToSqlBoolean(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                try
                {
                    return SqlBoolean.Parse(obj.ToString());
                }
                catch { return SqlBoolean.Null; }
            }
            return SqlBoolean.Null;
        }

        public static SqlDateTime ToSqlDateTime(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                try
                {
                    if (obj is DateTime)
                        return (DateTime)obj;
                    else
                        return Convert.ToDateTime(obj.ToString());
                }
                catch { return SqlDateTime.Null; }
            }
            return SqlDateTime.Null;
        }

        public static string ToString(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                return obj.ToString();
            }
            return null;
        }

        public static SqlString ToSqlString(object obj)
        {
            if (obj != null && obj.ToString() != string.Empty)
            {
                return obj.ToString();
            }
            return SqlString.Null;
        }

        public static DateTime? ConvertUTCToClientDateTime(DateTime? utcDateTime, string clientTimeZonekey)
        {
            if (!utcDateTime.HasValue)
                return null;

            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(clientTimeZonekey);

            //System.DateTime.

            return TimeZoneInfo.ConvertTime(utcDateTime.Value, TimeZoneInfo.Utc, zone);
        }

    }


    public class SqlTableCreator
    {
        #region Instance Variables

        private SqlConnection _connection;

        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        private SqlTransaction _transaction;

        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        private string _tableName;

        public string DestinationTableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        #endregion Instance Variables

        #region Constructor

        public SqlTableCreator() { }

        public SqlTableCreator(SqlConnection connection) : this(connection, null) { }

        public SqlTableCreator(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        #endregion Constructor

        #region Instance Methods

        public object Create(DataTable schema, string sqlConnString)
        {
            return Create(schema, null, sqlConnString);
        }

        public object Create(DataTable schema, int numKeys, string sqlConnString)
        {
            int[] primaryKeys = new int[numKeys];
            for (int i = 0; i < numKeys; i++)
            {
                primaryKeys[i] = i;
            }
            return Create(schema, primaryKeys, sqlConnString);
        }

        public object Create(DataTable schema, int[] primaryKeys, string sqlConnString)
        {
            string sql = GetCreateSQL(_tableName, schema, primaryKeys);

            using (SqlConnection con = new SqlConnection(sqlConnString))
            {
                con.Open();
                var cmd = new SqlCommand(sql, con);

                return cmd.ExecuteNonQuery();
            }
        }

        public object CreateFromDataTable(DataTable table, string sqlConnString)
        {
            string sql = GetCreateFromDataTableSQL(_tableName, table);

            SqlCommand cmd;
            //if (_transaction != null && _transaction.Connection != null)
            //    cmd = new SqlCommand(sql, _connection, _transaction);
            //else
            //    cmd = new SqlCommand(sql, _connection);

            using (SqlConnection con = new SqlConnection(sqlConnString))
            {
                con.Open();
                cmd = new SqlCommand(sql, con);

                return cmd.ExecuteNonQuery();
            }
        }

        #endregion Instance Methods

        #region Static Methods

        public static string GetCreateSQL(string tableName, DataTable schema, int[] primaryKeys)
        {
            string sql = "CREATE TABLE [" + tableName + "] (\n";

            // columns
            foreach (DataRow column in schema.Columns)
            {
                if (!(schema.Columns.Contains("IsHidden") && (bool)column["IsHidden"]))
                {
                    sql += "\t[" + column["ColumnName"].ToString() + "] " + SQLGetType(column);

                    if (schema.Columns.Contains("AllowDBNull") && (bool)column["AllowDBNull"] == false)
                        sql += " NOT NULL";

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
                if (schema.Columns.Contains("IsKey") && (bool)column["IsKey"])
                    keys.Add(column["ColumnName"].ToString());
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

        #endregion Static Methods
    }

}