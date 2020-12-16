using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityDivUserGroupMemberConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmSecurityDivUserGroupMemberClrDto ConvertDataRowDto(DataRow row)
        {
            PdmSecurityDivUserGroupMemberClrDto aDto = new  PdmSecurityDivUserGroupMemberClrDto();
			aDto.DivisionUserGroupId =(System.Int32)row["DivisionUserGroupID"];
			aDto.DivisionUserId =(System.Int32)row["DivisionUserID"];
			aDto.GroupId =(System.Int32)row["GroupID"];
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 