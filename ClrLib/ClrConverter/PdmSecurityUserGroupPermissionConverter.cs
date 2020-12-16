using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityUserGroupPermissionConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmSecurityUserGroupPermissionClrDto ConvertDataRowDto(DataRow row)
        {
            PdmSecurityUserGroupPermissionClrDto aDto = new  PdmSecurityUserGroupPermissionClrDto();
			aDto.UserGroupPermissionId =(System.Int32)row["UserGroupPermissionID"];
			aDto.GroupId =(System.Int32)row["GroupID"];
			aDto.PermissionId =(System.Int32)row["PermissionID"];
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 