using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmMassUpdateViewMemberConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmMassUpdateViewMemberClrDto ConvertDataRowDto(DataRow row)
        {
            PdmMassUpdateViewMemberClrDto aDto = new  PdmMassUpdateViewMemberClrDto();
			aDto.MassUpdateViewMemberId =(System.Int32)row["MassUpdateViewMemberID"];
			aDto.MassUpdateViewId =(System.Int32)row["MassUpdateViewID"];
			aDto.SecurityWebUserId =row["SecurityWebUserID"]  as System.Int32 ? ;
			aDto.SecurityUserGroupId =row["SecurityUserGroupID"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 