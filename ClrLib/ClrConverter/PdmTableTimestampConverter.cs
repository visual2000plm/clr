using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTableTimestampConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmTableTimestampClrDto ConvertDataRowDto(DataRow row)
        {
            PdmTableTimestampClrDto aDto = new  PdmTableTimestampClrDto();
			aDto.SysTimeStamp =row["SysTimeStamp"]as System.Byte[]	 ;
			aDto.TableName =(System.String)row["TableName"];
			aDto.LastScanTime =row["LastScanTime"]  as System.DateTime ? ;
			aDto.TableTimeStampId =(System.Int32)row["TableTimeStampID"];
            return aDto;
        }
      		
     
		        
    }
}

 