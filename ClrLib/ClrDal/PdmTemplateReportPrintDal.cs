using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTemplateReportPrintDal 
    {
        					
        public static  string QueryAll = @" SELECT  ReportPrintID ,  TemplateID ,  ReportFileName ,  ReportEngineType ,  IsDefault ,  PrintName ,  Description ,  ReportId ,  ReferenceViewID   FROM [dbo].[pdmTemplateReportPrint] ";
			
		public static List< PdmTemplateReportPrintClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmTemplateReportPrintClrDto> listDto = new List<PdmTemplateReportPrintClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmTemplateReportPrintConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 