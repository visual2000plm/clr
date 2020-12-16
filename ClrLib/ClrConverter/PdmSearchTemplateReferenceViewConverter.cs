using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSearchTemplateReferenceViewConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmSearchTemplateReferenceViewClrDto ConvertDataRowDto(DataRow row)
        {
            PdmSearchTemplateReferenceViewClrDto aDto = new  PdmSearchTemplateReferenceViewClrDto();
			aDto.SearchTemplateViewId =(System.Int32)row["SearchTemplateViewID"];
			aDto.SearchTemplateId =(System.Int32)row["SearchTemplateID"];
			aDto.ReferenceViewId =row["ReferenceViewID"]  as System.Int32 ? ;
			aDto.ViewFilterColumnId =row["ReferenceIDViewFilterColumnID"]  as System.Int32 ? ;
			aDto.MassUpdateViewId =row["MassUpdateViewID"]  as System.Int32 ? ;
            return aDto;
        }
      		
     
		        
    }
}

 