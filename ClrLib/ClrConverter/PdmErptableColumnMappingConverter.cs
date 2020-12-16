using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmErptableColumnMappingConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmErptableColumnMappingClrDto ConvertDataRowDto(DataRow row)
        {
            PdmErptableColumnMappingClrDto aDto = new  PdmErptableColumnMappingClrDto();
			aDto.ColumnMappingId =(System.Int32)row["ColumnMappingID"];
			aDto.MappingId =(System.Int32)row["MappingID"];
			aDto.Plmcolumn =row["PLMColumn"]as System.String	 ;
			aDto.Erpcolumn =row["EXColumn"]as System.String	 ;
			aDto.FkentityMappingId =row["FKEntityMappingID"]  as System.Int32 ? ;
			aDto.ExdataType =row["EXDataType"]  as System.Int32 ? ;
			aDto.ExdataLength =row["EXDataLength"]  as System.Int32 ? ;
			aDto.PlmdataType =row["PLMDataType"]  as System.Int32 ? ;
			aDto.PlmdataLength =row["PLMDataLength"]  as System.Int32 ? ;
			aDto.IsAllowNull =row["IsAllowNull"]  as System.Int32 ? ;
			aDto.UserDefineEntityColumnId =row["UserDefineEntityColumnID"]  as System.Int32 ? ;
            return aDto;
        }
      		
     
		        
    }
}

 