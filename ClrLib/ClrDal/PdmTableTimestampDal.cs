using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTableTimestampDal 
    {
        					
        public static  string QueryAll = @" SELECT  SysTimeStamp ,  TableName ,  LastScanTime ,  TableTimeStampID   FROM [dbo].[pdmTableTimestamp] ";
			
		public static List< PdmTableTimestampClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmTableTimestampClrDto> listDto = new List<PdmTableTimestampClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmTableTimestampConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 