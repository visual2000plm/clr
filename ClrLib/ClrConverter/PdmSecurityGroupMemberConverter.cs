using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityGroupMemberConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmSecurityGroupMemberClrDto ConvertDataRowDto(DataRow row)
        {
            PdmSecurityGroupMemberClrDto aDto = new  PdmSecurityGroupMemberClrDto();
			aDto.RoleMemberId =(System.Int32)row["RoleMemberID"];
			aDto.GroupId =(System.Int32)row["GroupID"];
			aDto.UserId =(System.Int32)row["UserID"];
			aDto.IsDefault =(System.Boolean)row["IsDefault"];
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 