using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTimeZoneInfoDal 
    {
        					
        public static  string QueryAll = @" SELECT  TimeZoneID ,  Display ,  Bias ,  StdBias ,  DltBias ,  StdMonth ,  StdDayOfWeek ,  StdWeek ,  StdHour ,  DltMonth ,  DltDayOfWeek ,  DltWeek ,  DltHour   FROM [dbo].[pdmTimeZoneInfo] ";
			
		public static List< PdmTimeZoneInfoClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmTimeZoneInfoClrDto> listDto = new List<PdmTimeZoneInfoClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmTimeZoneInfoConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 