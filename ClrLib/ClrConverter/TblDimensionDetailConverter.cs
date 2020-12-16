using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class TblDimensionDetailConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static TblDimensionDetailClrDto ConvertDataRowDto(DataRow row)
        {
            TblDimensionDetailClrDto aDto = new  TblDimensionDetailClrDto();
			aDto.DimensionDetailId =(System.Int32)row["DimensionDetailID"];
			aDto.DimensionId =row["DimensionID"]  as System.Int32 ? ;
			aDto.DimDetailCode =row["DimDetailCode"]as System.String	 ;
			aDto.DimDetailDesc =row["DimDetailDesc"]as System.String	 ;
			aDto.DimDetailSort =row["DimDetailSort"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =row["SystemTimeStamp"]as System.Byte[]	 ;
			aDto.SystemSort =row["SystemSort"]  as System.Int32 ? ;
			aDto.Erpid =row["ERPID"]as System.String	 ;
			aDto.IntegrationId =row["IntegrationId"]as System.String	 ;
            return aDto;
        }
      		
     
		        
    }
}

 