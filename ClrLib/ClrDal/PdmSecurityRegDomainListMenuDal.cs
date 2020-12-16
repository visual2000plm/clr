using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityRegDomainListMenuDal 
    {
        					
        public static  string QueryAll = @" SELECT  DomainMenuID ,  MenuID ,  DomainID ,  SystemTimeStamp   FROM [dbo].[pdmSecurityRegDomainListMenu] ";
			
		public static List< PdmSecurityRegDomainListMenuClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmSecurityRegDomainListMenuClrDto> listDto = new List<PdmSecurityRegDomainListMenuClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmSecurityRegDomainListMenuConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 