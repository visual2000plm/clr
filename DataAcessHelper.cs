using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using Microsoft.SqlServer.Server;
using System.Net;
using System.IO;

namespace PLMCLRTools
{
    public   class  DataAcessHelper
    {
        public static string GenerateColumnInClauseWithAndCondition(IEnumerable<int> ids, string IDColumnName, bool isInsertAnd)
        {
            string inclause = string.Empty;

            if (ids != null)
            {
                foreach (object pid in ids)
                {
                    inclause += pid + ",";
                }
            }

            if (inclause != string.Empty)
            {
                inclause = inclause.Substring(0, inclause.Length - 1);
                if (isInsertAnd)
                {
                    inclause = "  and  " + IDColumnName + " in ( " + inclause + " ) ";
                }
                else
                {
                    inclause = "  " + IDColumnName + " in ( " + inclause + " ) ";
                }
            }
            return inclause;
        }

        public static string GenerateColumnInClauseWithAndCondition(IEnumerable<string> ids, string IDColumnName, bool isInsertAnd)
        {
            string inclause = string.Empty;

            if (ids != null)
            {
                foreach (string pid in ids)
                {
                    inclause += "'"+pid + "',";
                }
            }

            if (inclause != string.Empty)
            {
                inclause = inclause.Substring(0, inclause.Length - 1);
                if (isInsertAnd)
                {
                    inclause = "  and  " + IDColumnName + " in ( " + inclause + " ) ";
                }
                else
                {
                    inclause = "  " + IDColumnName + " in ( " + inclause + " ) ";
                }
            }
            return inclause;
        }

        public static System.Data.DataTable GetDataTableQueryResult(SqlConnection conn, string queryString, List<SqlParameter> listParamters=null)
        {
            SqlCommand cmd = new SqlCommand(queryString, conn);

            if (listParamters != null)
            {
                foreach (var parameter in listParamters)
                {
                    cmd.Parameters.Add(parameter);
                }
            }

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            System.Data.DataTable resultTabel = new DataTable();
            adapter.Fill(resultTabel);

            // need to update FKEntity Name

            return resultTabel;


          

        }

        public static SqlDataReader GetDataReaderResult(SqlConnection conn, string queryString)
        {
            SqlCommand cmd = new SqlCommand(queryString, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            //if (reader.HasRows)
            //{
            //    while (reader.Read())
            //    {
            //        Console.WriteLine("{0}\t{1}", reader.GetInt32(0),
            //        reader.GetString(1));
            //    }
            //}

            return reader;

        }


        public static void ExecuteNonQuery(SqlConnection conn, string queryString)
        {
            using (SqlCommand cmd = new SqlCommand(queryString, conn))
            {
                cmd.ExecuteNonQuery();
            
            }
           

        }


        // user  as int?  or string to cast the final data Type
        public static object RetriveSigleValue(SqlConnection conn, string queryString, List<SqlParameter> listParamters)
        {
            using (SqlCommand cmd = new SqlCommand(queryString, conn))
            {
                if (listParamters != null)
                {
                    foreach (var parameter in listParamters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }
                return cmd.ExecuteScalar();

            }


        }

        //public static System.Data.DataTable GetDataReaderQueryResult(SqlConnection conn, string queryString)
        //{
        //    SqlCommand cmd = new SqlCommand(queryString, conn);
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //      //  reader[
        //        //Console.WriteLine(String.Format("{0}", reader[0]));
        //    }
          

        //}

        // queryCommand need to set paramter and quetest
        public static System.Data.DataTable GetDataTableQueryResult(SqlCommand queryCommand)
        {
           // SqlCommand cmd = new SqlCommand(queryString, conn);

            SqlDataAdapter adapter = new SqlDataAdapter(queryCommand);
            System.Data.DataTable resultTabel = new DataTable();
            adapter.Fill(resultTabel);

            // need to update FKEntity Name

            return resultTabel;
        }



        public static System.Data.DataSet GetDataSetQueryResult(SqlConnection conn, Dictionary<string, string> dictTableNameAndQuery)
        {
            DataSet resultResutlSet = new DataSet();
            foreach (var pair in dictTableNameAndQuery)
            {
                SqlDataAdapter adapter = new SqlDataAdapter(pair.Value, conn);
                adapter.Fill(resultResutlSet, pair.Key);
            }

            return resultResutlSet;
        }


        public static void SetTableColumnIsNull(SqlConnection conn, string TableName, Dictionary<string,string>  dictColumnType )
        {
            // in plms all Fk Id user interger
            foreach ( string column in dictColumnType.Keys )
            {
                string alterTable = string.Format("ALTER TABLE [{0}] ALTER COLUMN [{1}]  {2}  NULL ", TableName, column, dictColumnType[column]);
                SqlCommand comd = new SqlCommand(alterTable, conn);
                comd.ExecuteNonQuery();

            
            }
          
        
        }

        public static void SetTableColumnIsNotNull(SqlConnection conn, string TableName, Dictionary<string, string> dictColumnType)
        {
            // in plms all Fk Id user interger
            foreach (string column in dictColumnType.Keys)
            {
                string alterTable = string.Format("ALTER TABLE [{0}] ALTER COLUMN [{1}] {2} Not  NULL ", TableName, column, dictColumnType[column]);
                SqlCommand comd = new SqlCommand(alterTable, conn);
                comd.ExecuteNonQuery();  


            }


        }

        public static string GetSQLDropTableCommand(string tableName)
        {
            //string droptableFullName = dbmsName + ".[dbo].[" + tableName + "] ";
            string dropDWTable = " IF  EXISTS (SELECT * FROM  sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + tableName + "]') AND type in (N'U')) " +
                   " DROP TABLE " + tableName + System.Environment.NewLine;
            return dropDWTable;
        }
        public static int GetUniqueInterID()
        {
            int i = Guid.NewGuid().GetHashCode();
            return i;
        }

        public static void ExecuteReadUnCommmited(SqlConnection conn)
        {
            SqlCommand excuteSETTRANSACTIONISOLATIONLEVELREAD_UNCOMMITTED = new SqlCommand(PLMConstantString.SETTRANSACTIONISOLATIONLEVELREAD_UNCOMMITTED, conn);
            excuteSETTRANSACTIONISOLATIONLEVELREAD_UNCOMMITTED.ExecuteNonQuery();
        }

        public static void ExecuteReadCommmited(SqlConnection conn)
        {
            SqlCommand excuteSETTRANSACTIONISOLATIONLEVELREAD_COMMITTED = new SqlCommand(PLMConstantString.SETTRANSACTIONISOLATIONLEVELREAD_COMMITTED, conn);
            excuteSETTRANSACTIONISOLATIONLEVELREAD_COMMITTED.ExecuteNonQuery();
        }

   
         public static string QuoteSQLString(string str)
         {
             return str.Replace("'", "''");
         }

         public static string QuoteSQLString(object ostr)
         {
             return ostr.ToString().Replace("'", "''");
         }


         // need to add Newtonsoft.Json dll to the reference
         public static DataTable GetRestJSonResult(string serviceUrl, Dictionary<string, string> dictHeaderParamter, string requestMethod = "GET")
         {
             DataTable aDataTable = new DataTable();
             HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
             webrequest.Method = requestMethod;
             webrequest.ContentType = "application/json";
             foreach (var keyValue in dictHeaderParamter)
             {
                 webrequest.Headers.Add(keyValue.Key, keyValue.Value);
             }

             HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
             Stream responseStream = webresponse.GetResponseStream();

             StreamReader reader = new StreamReader(responseStream);
             string text = reader.ReadToEnd();

          //   DataTable db = DBInteractionBase.ConvertJasonToDataTable(text);

             return aDataTable;
         }


         //public static DataTable ConvertJasonToDataTable(string jasonString)
         //{
         //    // StringReader theReader = new StringReader(jasonString);
         //    var table = JsonConvert.DeserializeObject<DataTable>(jasonString);
         //    return table;


         //}

    }

    public class Validation
    {
        /// <summary>
        /// Returns true if the given object is a valid number, or false if it's not
        /// </summary>
        /// <param name="sDateTime"></param>
        /// <returns></returns>
        public static bool IsNumeric(Object objValue)
        {
            bool _Valid = false;

            try
            {
                double y = Convert.ToDouble(objValue);
                _Valid = true;
                return _Valid;
            }
            catch
            {
                _Valid = false;
            }

            try
            {
                int x = Convert.ToInt32(objValue);
                _Valid = true;
                return _Valid;
            }
            catch
            {
                _Valid = false;
            }

            return _Valid;
        }

        /// <summary>
        /// Returns true if the given string is a valid date string, or false if it's not
        /// </summary>
        /// <param name="sDateTime"></param>
        /// <returns></returns>
        public static bool IsDateTime(string sDateTime)
        {
            bool bIsDateTime = false;

            try
            {
                System.DateTime.Parse(sDateTime);
                bIsDateTime = true;
            }
            catch
            {
                bIsDateTime = false;
            }

            return bIsDateTime;
        }

        //public static IEnumerable<TankReading> ConvertToTankReadings(DataTable dataTable)
        //{
        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        yield return new TankReading
        //        {
        //            TankReadingsID = Convert.ToInt32(row["TRReadingsID"]),
        //            TankID = Convert.ToInt32(row["TankID"]),
        //            ReadingDateTime = Convert.ToDateTime(row["ReadingDateTime"]),
        //            ReadingFeet = Convert.ToInt32(row["ReadingFeet"]),
        //            ReadingInches = Convert.ToInt32(row["ReadingInches"]),
        //            MaterialNumber = row["MaterialNumber"].ToString(),
        //            EnteredBy = row["EnteredBy"].ToString(),
        //            ReadingPounds = Convert.ToDecimal(row["ReadingPounds"]),
        //            MaterialID = Convert.ToInt32(row["MaterialID"]),
        //            Submitted = Convert.ToBoolean(row["Submitted"]),
        //        };
        //    }

        //}
    }
}
