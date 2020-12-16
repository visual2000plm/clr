using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTabBlockCaculationFlowDal 
    {
        					
        public static  string QueryAll = @" SELECT  CaculationFlowID ,  TabID ,  BlockID ,  Priority ,  SystemTimeStamp   FROM [dbo].[pdmTabBlockCaculationFlow] ";
			
		public static List< PdmTabBlockCaculationFlowClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmTabBlockCaculationFlowClrDto> listDto = new List<PdmTabBlockCaculationFlowClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmTabBlockCaculationFlowConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 