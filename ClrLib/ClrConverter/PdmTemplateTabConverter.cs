using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTemplateTabConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmTemplateTabClrDto ConvertDataRowDto(DataRow row)
        {
            PdmTemplateTabClrDto aDto = new  PdmTemplateTabClrDto();
			aDto.TemplateTabId =(System.Int32)row["TemplateTabID"];
			aDto.TemplateId =(System.Int32)row["TemplateID"];
			aDto.TabId =(System.Int32)row["TabID"];
			aDto.Sort =row["Sort"]  as System.Int16 ? ;
			aDto.HeaderTabCaculationFlow =row["HeaderTabCaculationFlow"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 