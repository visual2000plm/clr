using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTemplateTabLibReferenceSettingDetailConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmTemplateTabLibReferenceSettingDetailClrDto ConvertDataRowDto(DataRow row)
        {
            PdmTemplateTabLibReferenceSettingDetailClrDto aDto = new  PdmTemplateTabLibReferenceSettingDetailClrDto();
			aDto.DetailId =(System.Int32)row["DetailID"];
			aDto.TemplateTabLibRefId =(System.Int32)row["TemplateTabLibRefID"];
			aDto.FiledSubitmeId =row["FiledSubitmeID"]  as System.Int32 ? ;
			aDto.MappingGridColumnId =row["MappingGridColumnID"]  as System.Int32 ? ;
			aDto.MappingType =row["MappingType"]  as System.Int32 ? ;
			aDto.UserDefineEntityColumnId =row["UserDefineEntityColumnID"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 