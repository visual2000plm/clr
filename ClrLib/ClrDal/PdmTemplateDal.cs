using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTemplateDal 
    {
        					
        public static  string QueryAll = @" SELECT  TemplateID ,  TemplateName ,  Description ,  GroupID ,  IsRef ,  ParentID ,  CreatedBy ,  CreatedDate ,  ModifyBy ,  ModifyDate ,  ManagementLevel ,  InternalCode ,  RefTxType ,  FolderID ,  SystemTimeStamp   FROM [dbo].[pdmTemplate] ";
			
		public static List< PdmTemplateClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmTemplateClrDto> listDto = new List<PdmTemplateClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmTemplateConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 