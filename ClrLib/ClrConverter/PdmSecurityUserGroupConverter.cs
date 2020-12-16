using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityUserGroupConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmSecurityUserGroupClrDto ConvertDataRowDto(DataRow row)
        {
            PdmSecurityUserGroupClrDto aDto = new  PdmSecurityUserGroupClrDto();
			aDto.GroupId =(System.Int32)row["GroupID"];
			aDto.GroupName =(System.String)row["GroupName"];
			aDto.Description =row["Description"]as System.String	 ;
			aDto.LoginEvent =row["LoginEvent"]as System.String	 ;
			aDto.ParentId =row["ParentID"]  as System.Int32 ? ;
			aDto.InternalCode =row["InternalCode"]as System.String	 ;
			aDto.IsBuiltIn =row["IsBuiltIn"]  as System.Boolean ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 