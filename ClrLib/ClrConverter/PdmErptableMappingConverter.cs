using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmErptableMappingConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmErptableMappingClrDto ConvertDataRowDto(DataRow row)
        {
            PdmErptableMappingClrDto aDto = new  PdmErptableMappingClrDto();
			aDto.MappingId =(System.Int32)row["MappingID"];
			aDto.PlmtableName =row["PLMTableName"]as System.String	 ;
			aDto.PlmprimaryKeyColumn =row["PLMPrimaryKeyColumn"]as System.String	 ;
			aDto.PlmlogicalUniqueColumn =row["PLMLogicalUniqueColumn"]as System.String	 ;
			aDto.ErptableName =row["EXTableName"]as System.String	 ;
			aDto.ErpprimaryKeyColumn =row["EXPrimaryKeyColumn"]as System.String	 ;
			aDto.ErplogicalUniqueColumn =row["EXLogicalUniqueColumn"]as System.String	 ;
			aDto.ExstartRootPkidusedByPlm =row["EXStartRootPKIDUsedByPLM"]  as System.Int32 ? ;
			aDto.ExchangeMode =row["EXChangeMode"]  as System.Int32 ? ;
			aDto.PlmlastTimeStamp =row["LastReadPLMTableTimeStamp"]as System.Byte[]	 ;
			aDto.ErplastTimeStamp =row["LastReadExchangeTableTimeStamp"]as System.Byte[]	 ;
			aDto.ExchangeTableTimeStampColumnName =row["ExchangeTableTimeStampColumnName"]as System.String	 ;
			aDto.PlmTableTimeStampColumnName =row["PlmTableTimeStampColumnName"]as System.String	 ;
			aDto.UserDefineEntityId =row["UserDefineEntityID"]  as System.Int32 ? ;
			aDto.IsMappingToUserDefineEntity =row["IsMappingToUserDefineEntity"]  as System.Boolean ? ;
            return aDto;
        }
      		
     
		        
    }
}

 