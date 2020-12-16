using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmDwrequireTabAndGridDal 
    {
        					
        public static  string QueryAll = @" SELECT  RequireTabGridID ,  TabID ,  BlockID ,  GridID   FROM [dbo].[pdmDWRequireTabAndGrid] ";
			
		public static List< PdmDwrequireTabAndGridClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmDwrequireTabAndGridClrDto> listDto = new List<PdmDwrequireTabAndGridClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmDwrequireTabAndGridConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 