using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSearchTemplateReferenceViewDal 
    {
        					
        public static  string QueryAll = @" SELECT  SearchTemplateViewID ,  SearchTemplateID ,  ReferenceViewID ,  ReferenceIDViewFilterColumnID ,  MassUpdateViewID   FROM [dbo].[pdmSearchTemplateReferenceView] ";
			
		public static List< PdmSearchTemplateReferenceViewClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmSearchTemplateReferenceViewClrDto> listDto = new List<PdmSearchTemplateReferenceViewClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmSearchTemplateReferenceViewConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 