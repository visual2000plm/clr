using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTabBlockDal 
    {
        					
        public static  string QueryAll = @" SELECT  TabBlockID ,  TabID ,  BlockID   FROM [dbo].[PdmTabBlock] ";
			
		public static List< PdmTabBlockClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmTabBlockClrDto> listDto = new List<PdmTabBlockClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmTabBlockConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 