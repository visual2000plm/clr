using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTemplateTabDal 
    {
        					
        public static  string QueryAll = @" SELECT  TemplateTabID ,  TemplateID ,  TabID ,  Sort ,  HeaderTabCaculationFlow ,  SystemTimeStamp   FROM [dbo].[pdmTemplateTab] ";
			
		public static List< PdmTemplateTabClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmTemplateTabClrDto> listDto = new List<PdmTemplateTabClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmTemplateTabConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 