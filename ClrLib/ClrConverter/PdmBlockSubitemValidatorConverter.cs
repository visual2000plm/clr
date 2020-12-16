using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmBlockSubitemValidatorConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmBlockSubitemValidatorClrDto ConvertDataRowDto(DataRow row)
        {
            PdmBlockSubitemValidatorClrDto aDto = new  PdmBlockSubitemValidatorClrDto();
			aDto.SubitemValidatorId =(System.Int32)row["SubitemValidatorID"];
			aDto.SubItemId =(System.Int32)row["SubItemID"];
			aDto.ValidatorType =(System.Int32)row["ValidatorType"];
			aDto.WarningMessage =row["WarningMessage"]as System.String	 ;
			aDto.MaximumValue =row["MaximumValue"]  as System.Double ? ;
			aDto.MinimumValue =row["MinimumValue"]  as System.Double ? ;
			aDto.RegularExpression =row["RegularExpression"]as System.String	 ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 