using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmBlqueryDal 
    {
        					
        public static  string QueryAll = @" SELECT  BLQueryID ,  QueryName ,  QueryDescription ,  QueryText ,  SearchScopeType   FROM [dbo].[pdmBLQuery] ";
			
		public static List< PdmBlqueryClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmBlqueryClrDto> listDto = new List<PdmBlqueryClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmBlqueryConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 