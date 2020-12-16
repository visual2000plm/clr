using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class TblDimensionConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static TblDimensionClrDto ConvertDataRowDto(DataRow row)
        {
            TblDimensionClrDto aDto = new  TblDimensionClrDto();
			aDto.DimensionId =(System.Int32)row["DimensionID"];
			aDto.DimensionCode =row["DimensionCode"]as System.String	 ;
			aDto.DimensionDesc =row["DimensionDesc"]as System.String	 ;
			aDto.PublishedMode =row["PublishedMode"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
			aDto.Erpid =row["ERPID"]as System.String	 ;
			aDto.IntegrationId =row["IntegrationId"]as System.String	 ;
            return aDto;
        }
      		
     
		        
    }
}

 