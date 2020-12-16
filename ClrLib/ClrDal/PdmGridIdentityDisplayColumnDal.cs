using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmGridIdentityDisplayColumnDal 
    {
        					
        public static  string QueryAll = @" SELECT  IdentityDisplayColumnID ,  IdentityColumnID ,  DisplayColumnID   FROM [dbo].[pdmGridIdentityDisplayColumn] ";
			
		public static List< PdmGridIdentityDisplayColumnClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmGridIdentityDisplayColumnClrDto> listDto = new List<PdmGridIdentityDisplayColumnClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmGridIdentityDisplayColumnConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 