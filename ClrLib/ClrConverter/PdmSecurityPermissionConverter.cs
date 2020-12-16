using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityPermissionConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmSecurityPermissionClrDto ConvertDataRowDto(DataRow row)
        {
            PdmSecurityPermissionClrDto aDto = new  PdmSecurityPermissionClrDto();
			aDto.PermissionId =(System.Int32)row["PermissionID"];
			aDto.Name =(System.String)row["Name"];
			aDto.Description =row["Description"]as System.String	 ;
			aDto.InternalCode =(System.Int32)row["InternalCode"];
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 