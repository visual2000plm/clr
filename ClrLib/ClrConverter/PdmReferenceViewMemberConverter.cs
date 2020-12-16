using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmReferenceViewMemberConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmReferenceViewMemberClrDto ConvertDataRowDto(DataRow row)
        {
            PdmReferenceViewMemberClrDto aDto = new  PdmReferenceViewMemberClrDto();
			aDto.ReferenceViewMemberId =(System.Int32)row["ReferenceViewMemberID"];
			aDto.ReferenceViewId =(System.Int32)row["ReferenceViewID"];
			aDto.SecurityWebUserId =row["SecurityWebUserID"]  as System.Int32 ? ;
			aDto.SecurityUserGroupId =row["SecurityUserGroupID"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 