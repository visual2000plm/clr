using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class TblSizeRunRotateConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static TblSizeRunRotateClrDto ConvertDataRowDto(DataRow row)
        {
            TblSizeRunRotateClrDto aDto = new  TblSizeRunRotateClrDto();
			aDto.SizeRunRotateId =(System.Int32)row["SizeRunRotateID"];
			aDto.SizeRunId =(System.Int32)row["SizeRunId"];
			aDto.SizeOrder =(System.Byte)row["SizeOrder"];
			aDto.SystemTimeStamp =row["SystemTimeStamp"]as System.Byte[]	 ;
			aDto.SizeName =row["SizeName"]as System.String	 ;
			aDto.Erpid =row["ERPID"]as System.String	 ;
			aDto.IntegrationId =row["IntegrationId"]as System.String	 ;
            return aDto;
        }
      		
     
		        
    }
}

 