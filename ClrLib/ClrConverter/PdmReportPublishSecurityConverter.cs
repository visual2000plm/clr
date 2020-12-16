using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmReportPublishSecurityConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmReportPublishSecurityClrDto ConvertDataRowDto(DataRow row)
        {
            PdmReportPublishSecurityClrDto aDto = new  PdmReportPublishSecurityClrDto();
			aDto.ReportSecurityId =(System.Int32)row["ReportSecurityID"];
			aDto.ReportId =(System.Int32)row["ReportID"];
			aDto.SecurityWebUserId =row["SecurityWebUserID"]  as System.Int32 ? ;
			aDto.SecurityUserGroupId =row["SecurityUserGroupID"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 