using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTimeZoneInfoConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmTimeZoneInfoClrDto ConvertDataRowDto(DataRow row)
        {
            PdmTimeZoneInfoClrDto aDto = new  PdmTimeZoneInfoClrDto();
			aDto.TimeZoneId =(System.Int32)row["TimeZoneID"];
			aDto.Display =(System.String)row["Display"];
			aDto.Bias =(System.Int16)row["Bias"];
			aDto.StdBias =(System.Int16)row["StdBias"];
			aDto.DltBias =(System.Int16)row["DltBias"];
			aDto.StdMonth =(System.Int16)row["StdMonth"];
			aDto.StdDayOfWeek =(System.Int16)row["StdDayOfWeek"];
			aDto.StdWeek =(System.Int16)row["StdWeek"];
			aDto.StdHour =(System.Int16)row["StdHour"];
			aDto.DltMonth =(System.Int16)row["DltMonth"];
			aDto.DltDayOfWeek =(System.Int16)row["DltDayOfWeek"];
			aDto.DltWeek =(System.Int16)row["DltWeek"];
			aDto.DltHour =(System.Int16)row["DltHour"];
            return aDto;
        }
      		
     
		        
    }
}

 