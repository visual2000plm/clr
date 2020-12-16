using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmGridMetaColumnAggFunctionDal 
    {
        					
        public static  string QueryAll = @" SELECT  GridMetaColumnFunctionID ,  GridColumnID ,  AggregationFunctionType ,  SystemTimeStamp   FROM [dbo].[pdmGridMetaColumnAggFunction] ";
			
		public static List< PdmGridMetaColumnAggFunctionClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmGridMetaColumnAggFunctionClrDto> listDto = new List<PdmGridMetaColumnAggFunctionClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmGridMetaColumnAggFunctionConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 