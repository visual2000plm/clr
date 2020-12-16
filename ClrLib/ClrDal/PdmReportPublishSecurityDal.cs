using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmReportPublishSecurityDal 
    {
        					
        public static  string QueryAll = @" SELECT  ReportSecurityID ,  ReportID ,  SecurityWebUserID ,  SecurityUserGroupID ,  SystemTimeStamp   FROM [dbo].[pdmReportPublishSecurity] ";
			
		public static List< PdmReportPublishSecurityClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmReportPublishSecurityClrDto> listDto = new List<PdmReportPublishSecurityClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmReportPublishSecurityConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 