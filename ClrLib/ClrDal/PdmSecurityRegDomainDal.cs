using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityRegDomainDal 
    {
        					
        public static  string QueryAll = @" SELECT  DomainID ,  DomainCode ,  PersonType ,  Description ,  DefaultPage ,  SystemTimeStamp   FROM [dbo].[pdmSecurityRegDomain] ";
			
		public static List< PdmSecurityRegDomainClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmSecurityRegDomainClrDto> listDto = new List<PdmSecurityRegDomainClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmSecurityRegDomainConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 