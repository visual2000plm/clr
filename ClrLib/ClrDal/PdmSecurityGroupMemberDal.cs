using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityGroupMemberDal 
    {
        					
        public static  string QueryAll = @" SELECT  RoleMemberID ,  GroupID ,  UserID ,  IsDefault ,  SystemTimeStamp   FROM [dbo].[pdmSecurityGroupMember] ";
			
		public static List< PdmSecurityGroupMemberClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmSecurityGroupMemberClrDto> listDto = new List<PdmSecurityGroupMemberClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmSecurityGroupMemberConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 