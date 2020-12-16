using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmReferenceViewConfigurationConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmReferenceViewConfigurationClrDto ConvertDataRowDto(DataRow row)
        {
            PdmReferenceViewConfigurationClrDto aDto = new  PdmReferenceViewConfigurationClrDto();
			aDto.ReferenceViewConfigurationId =(System.Int32)row["ReferenceViewConfigurationID"];
			aDto.ReferenceTypeId =(System.Int32)row["ReferenceTypeID"];
			aDto.ConfigurationTypeId =(System.Int32)row["ConfigurationTypeID"];
			aDto.SecurityWebUserId =row["SecurityWebUserID"]  as System.Int32 ? ;
			aDto.SecurityUserGroupId =row["SecurityUserGroupID"]  as System.Int32 ? ;
			aDto.ReferenceViewId =row["ReferenceViewID"]  as System.Int32 ? ;
			aDto.SearchTemplateId =row["SearchTemplateID"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 