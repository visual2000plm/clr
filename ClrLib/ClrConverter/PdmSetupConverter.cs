using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSetupConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmSetupClrDto ConvertDataRowDto(DataRow row)
        {
            PdmSetupClrDto aDto = new  PdmSetupClrDto();
			aDto.SetupId =(System.Int32)row["SetupID"];
			aDto.SetupCode =(System.String)row["setupCode"];
			aDto.SetupValue =(System.String)row["SetupValue"];
			aDto.Description =row["Description"]as System.String	 ;
			aDto.EntityId =row["EntityID"]  as System.Int32 ? ;
			aDto.UsageType =row["UsageType"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 