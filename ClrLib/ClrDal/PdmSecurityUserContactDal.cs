using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityUserContactDal 
    {
        					
        public static  string QueryAll = @" SELECT  ContactID ,  UserID ,  ContactType ,  ContactFormat ,  AdditionalContactInfo ,  Comments   FROM [dbo].[pdmSecurityUserContact] ";
			
		public static List< PdmSecurityUserContactClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmSecurityUserContactClrDto> listDto = new List<PdmSecurityUserContactClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmSecurityUserContactConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 