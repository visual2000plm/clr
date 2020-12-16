using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTemplateConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmTemplateClrDto ConvertDataRowDto(DataRow row)
        {
            PdmTemplateClrDto aDto = new  PdmTemplateClrDto();
			aDto.TemplateId =(System.Int32)row["TemplateID"];
			aDto.TemplateName =row["TemplateName"]as System.String	 ;
			aDto.Description =row["Description"]as System.String	 ;
			aDto.GroupId =row["GroupID"]  as System.Int32 ? ;
			aDto.IsRef =row["IsRef"]  as System.Byte ? ;
			aDto.ParentId =row["ParentID"]  as System.Int32 ? ;
			aDto.CreatedBy =row["CreatedBy"]  as System.Int32 ? ;
			aDto.CreatedDate =row["CreatedDate"]  as System.DateTime ? ;
			aDto.ModifyBy =row["ModifyBy"]  as System.Int32 ? ;
			aDto.ModifyDate =row["ModifyDate"]  as System.DateTime ? ;
			aDto.ManagementLevel =row["ManagementLevel"]  as System.Byte ? ;
			aDto.InternalCode =row["InternalCode"]as System.String	 ;
			aDto.RefTxType =row["RefTxType"]  as System.Int32 ? ;
			aDto.FolderId =row["FolderID"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 