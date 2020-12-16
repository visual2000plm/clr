using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTemplateReportPrintConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmTemplateReportPrintClrDto ConvertDataRowDto(DataRow row)
        {
            PdmTemplateReportPrintClrDto aDto = new  PdmTemplateReportPrintClrDto();
			aDto.ReportPrintId =(System.Int32)row["ReportPrintID"];
			aDto.TemplateId =row["TemplateID"]  as System.Int32 ? ;
			aDto.ReportFileName =row["ReportFileName"]as System.String	 ;
			aDto.ReportEngineType =row["ReportEngineType"]  as System.Int32 ? ;
			aDto.IsDefault =row["IsDefault"]  as System.Boolean ? ;
			aDto.PrintName =row["PrintName"]as System.String	 ;
			aDto.Description =row["Description"]as System.String	 ;
			aDto.ReportId =(System.Int32)row["ReportId"];
			aDto.ReferenceViewId =row["ReferenceViewID"]  as System.Int32 ? ;
            return aDto;
        }
      		
     
		        
    }
}

 