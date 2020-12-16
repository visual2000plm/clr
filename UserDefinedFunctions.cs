using System;

using System.Text;

using System.IO;

using Microsoft.SqlServer.Server;
using System.Data.Sql;
using System.Data.SqlClient;

using System.Data.SqlTypes;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

namespace PLMCLRTools
{
    #region----- SQlUserdefine Function---
    public partial class UserDefinedFunctions
    {

        // internal static readonly string Into " + DWTableName +


        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlDouble ConvertValueTextToDecimal(SqlString valueText)
        {
            if (valueText.IsNull)
            {
                return SqlDouble.Null;
            }
            else
            {


                Double result;


                if (Double.TryParse(valueText.Value.Trim(), out result))
                {
                    try
                    {
                        // default it is uncheck
                        Double fresult = checked(result);
                        return fresult;
                    }
                    catch //()
                    {
                        //retu SqlDouble.Null;
                        //Console.WriteLine("Error caught: {0}", e);
                    }




                }


            }

            return SqlDouble.Null;




        }


        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlDateTime ConvertValueTextToDateTime(SqlString valueText)
        {
            if (valueText.IsNull)
            {
                return SqlDateTime.Null;
            }
            else
            {


                DateTime result;

                if (DateTime.TryParse(valueText.Value.Trim(), out result))
                {

                    return result;

                }


            }

            return SqlDateTime.Null;




        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static SqlInt32 ConvertValueTextToInt(SqlString valueText)
        {
            if (valueText.IsNull)
            {
                return SqlInt32.Null;
            }
            else
            {

                if (valueText.Value.Trim().Length > 64)
                    return SqlInt32.Null;

                int result;

                if (Int32.TryParse(valueText.Value.Trim(), out result))
                {

                    return result;

                }


            }

            return SqlInt32.Null;




        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static string ConvertValueTextToShortDate(SqlString valueText)
        {
            if (valueText.IsNull)
            {
                return string.Empty;
            }
            else
            {


                DateTime result;

                if (DateTime.TryParse(valueText.Value.Trim(), out result))
                {

                    return result.ToShortDateString();

                }


            }

            return string.Empty;




        }

        [Microsoft.SqlServer.Server.SqlFunction]
        public static string RemoveString(SqlString orgString, SqlString removeS)
        {
            if (orgString.IsNull || orgString.Value.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                // if(removeS

                return orgString.Value.Replace(removeS.Value, string.Empty);


            }

            // return string.Empty;




        }


        [Microsoft.SqlServer.Server.SqlFunction]
        public static DateTime ConvertUTCToClientDateTime(DateTime  utcDateTime, string clientTimeZonekey)
        {

            if (string.IsNullOrEmpty(clientTimeZonekey))
            {
                return utcDateTime;
            }

             TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(clientTimeZonekey);

             return TimeZoneInfo.ConvertTime(utcDateTime, TimeZoneInfo.Utc, zone);
        }

       

        #region  INCH_CM CM convert

        private static readonly double INCH_CM = 2.54;

        // private static readonly double ONE_OF_THIRTYTWO_INCH = 0.03125;
        private static readonly string[] special32 = { " 1/16", " 1/8",  " 3/16", " 1/4",  " 5/16", 
														" 3/8",  " 7/16", " 1/2",  " 9/16", " 5/8", 
														" 11/16"," 3/4",  " 13/16"," 7/8",  " 15/16" };

        private static readonly double ONE_OF_SIXTYFOUR_INCH = 0.015625;
        private static readonly string[] special64 = {  " 1/32", " 1/16", " 3/32",  " 1/8",
														" 5/32", " 3/16", " 7/32",  " 1/4", 
														" 9/32", " 5/16", " 11/32", " 3/8",  
														" 13/32"," 7/16", " 15/32", " 1/2",
														" 17/32"," 9/16", " 19/32", " 5/8",  
														" 21/32"," 11/16"," 23/32", " 3/4",  
														" 25/32"," 13/16"," 27/32", " 7/8",  
														" 29/32"," 15/16"," 31/32", };


        private static string Inchs(int inch)
        {
            int index, rem;
            index = Math.DivRem(inch, 2, out rem) - 1;
            if (rem == 0 && 0 <= index && index < 31)
                return special64[index];
            else
                return (" " + inch.ToString() + "/64");

        }

        private static decimal CentimeterToInch(decimal dec)
        {
            double d = decimal.ToDouble(dec) / INCH_CM;
            return Convert.ToDecimal(d);
        }

        //SELECT  dbo.ConvertCentimeterToInch([38_FinalSpec])   FROM PLM_DW_Grid_SpecFitGrid_5
        [Microsoft.SqlServer.Server.SqlFunction]
        public static string ConvertCentimeterToInch(string stringCM)
        {
            decimal decimalCM;

            if (!Decimal.TryParse(stringCM, out decimalCM))
                return string.Empty;
            decimal orgCm = decimalCM;

            decimalCM = Math.Abs(decimalCM);


            string toReturn = "0";

            decimal dec1 = CentimeterToInch(decimalCM);

            if (dec1 != 0)
            {
                if (dec1.ToString().IndexOf('.') > 0)
                {
                    string strinch = dec1.ToString();
                    string[] str = strinch.Split(".".ToCharArray(), 2);

                    double d = Convert.ToDouble("0." + str[1]);

                    int inch = (int)Math.Round((double)d / ONE_OF_SIXTYFOUR_INCH);
                    //					int inch = (int)Math.Round( (double) d / ONE_OF_THIRTYTWO_INCH );

                    if (str[0] != "0")
                    {
                        if (inch != 0)
                        {
                            if (inch == 64) // 32)
                            {
                                int approx = int.Parse(str[0]) + 1;
                                toReturn = approx.ToString();
                            }
                            else
                                toReturn = str[0] + Inchs(inch);

                        }
                        else
                            toReturn = str[0];
                    }
                    else
                    {
                        if (inch == 64) // 32)
                            toReturn= "1";
                        else
                            toReturn = Inchs(inch);
                    }
                }
                else
                    toReturn = dec1.ToString();

            }


            if (orgCm < 0)
            {

                toReturn = "-" + toReturn;



            }
            return toReturn;



        }


        #endregion


    };
    #endregion

}
