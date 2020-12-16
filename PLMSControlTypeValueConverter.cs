using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Data;
using System.Globalization;
using System.Reflection;
namespace PLMCLRTools
{
    public static class ControlTypeValueConverter
    {
        public static List<LookupItemDto> GenerateLookupList<T>()
           where T : struct
        {
            var result = new List<LookupItemDto>();

            Type enumType = typeof(T);

            foreach (var item in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                Enum key = item.GetValue(null) as Enum;

                var enumId = Convert.ToInt32(item.GetRawConstantValue());

                string display = key.ToString();

                result.Add(new LookupItemDto { Id = enumId, Display = display });
            }

            return result.OrderBy(o => o.Display).ToList();
        }

        public static object ConvertValueToObject(object sourceValue, int aControlType)
        {
            if (aControlType == (int)EmControlType.Date)
            {
                return ConvertValueToDate(sourceValue);
            }
            else if (aControlType == (int)EmControlType.CheckBox)
            {
                return ConvertValueToBoolean(sourceValue);
            }

            else if (aControlType == (int)EmControlType.DDL
                || aControlType == (int)EmControlType.File
                || aControlType == (int)EmControlType.Image)
            {
                return ConvertValueToInt(sourceValue);
            }

            else if (aControlType == (int)EmControlType.Numeric)
            {
                //return ConvertValueToDouble(sourceValue);
                return ConvertValueToDoubleWithDefautZero(sourceValue);
            }
            else if (aControlType == (int)EmControlType.RGBColorDisplay)
            {
                if (sourceValue == null || sourceValue.ToString() == string.Empty)
                    return "255|255|0";

                return sourceValue.ToString();
            }
            else
            {
                if (sourceValue == null || sourceValue.ToString() == string.Empty)
                    return string.Empty;
                return sourceValue.ToString();
            }
        }

        // if the ControlType is Numeric, need to check Nbdecimal , if decimal is zero it is integer, else it is decial
        public static Type GetDataTypeByControlType(int aControlType)
        {
            if (aControlType == (int)EmControlType.Date)
            {
                return typeof(DateTime);
            }
            else if (aControlType == (int)EmControlType.CheckBox)
            {
                return typeof(bool);
            }

            else if (aControlType == (int)EmControlType.DDL
                || aControlType == (int)EmControlType.File
                || aControlType == (int)EmControlType.Image)
            {
                return typeof(int);
            }

            else if (aControlType == (int)EmControlType.Numeric)
            {
                return typeof(Double);
            }

            else
            {
                return typeof(string);
            }
        }

        public static double? ConvertValueToDouble(object sourceValue)
        {
            if (sourceValue == null) return null;

            double outvalue;
            if (double.TryParse(sourceValue.ToString(), out outvalue))
            {
                return outvalue;
            }

            return null;
        }

        public static DateTime? ConvertValueToDate(object sourceValue)
        {
            if (sourceValue == null) return null;

            DateTime outvalue;
            if (DateTime.TryParse(sourceValue.ToString(), out outvalue))
            {
                return outvalue;
            }

            return null;
        }

        public static Boolean? ConvertValueToBoolean(object sourceValue)
        {
            if (sourceValue == null) return null;

            Boolean outvalue;
            if (Boolean.TryParse(sourceValue.ToString(), out outvalue))
            {
                return outvalue;
            }

            return null;
        }

        public static int? ConvertValueToInt(object sourceValue)
        {
            if (sourceValue == null) return null;

            int outvalue;
            if (int.TryParse(sourceValue.ToString(), out outvalue))
            {
                return outvalue;
            }

            return null;
        }

        public static decimal? ConvertValueToDecimal(object sourceValue)
        {
            if (sourceValue == null) return null;

            decimal outvalue;
            if (decimal.TryParse(sourceValue.ToString(), out outvalue))
            {
                return outvalue;
            }
            return null;
        }

        public static double ConvertValueToDoubleWithDefautZero(object sourceValue)
        {
            if (sourceValue == null) return 0;

            double outvalue;

           // bool b = Decimal.TryParse("0.1", NumberStyles.Any, new CultureInfo("en-US"), out value)
          //  CLROutput.Output ("sourceValue"+ sourceValue.ToString ());
            if (double.TryParse(sourceValue.ToString(), out outvalue))
            {
               // CLROutput.Output("outvalue" + outvalue.ToString());
                return outvalue;
            }
            return 0;
        }

        public static string ConvertValueToStringWithDefaultEmptyString(object sourceValue)
        {
            if (sourceValue == null)
                return string.Empty;

            return sourceValue.ToString();
        }

       

        public static bool IsObjectNaNOrInfinity(object value)
        {

            if (value is double)
            {
                return double.IsNaN((double)value);
            }

            else if (value is float)
            {
                return float.IsNaN((float)value) || float.IsInfinity((float)value);
            }


            return false;
        }


    }

}
