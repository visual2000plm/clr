using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTemplateTabLibReferenceSettingConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmTemplateTabLibReferenceSettingClrDto ConvertDataRowDto(DataRow row)
        {
            PdmTemplateTabLibReferenceSettingClrDto aDto = new  PdmTemplateTabLibReferenceSettingClrDto();
			aDto.TemplateTabLibRefId =(System.Int32)row["TemplateTabLibRefID"];
			aDto.Name =row["Name"]as System.String	 ;
			aDto.Description =row["Description"]as System.String	 ;
			aDto.TemplateId =row["TemplateID"]  as System.Int32 ? ;
			aDto.TabId =row["TabID"]  as System.Int32 ? ;
			aDto.LibReferenceId =row["LibReferenceID"]  as System.Int32 ? ;
			aDto.GridBlockId =row["GridBlockID"]  as System.Int32 ? ;
			aDto.Type =row["Type"]  as System.Int32 ? ;
			aDto.EntityId =row["EntityID"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 