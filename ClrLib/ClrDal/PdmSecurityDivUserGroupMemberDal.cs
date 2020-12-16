using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityDivUserGroupMemberDal 
    {
        					
        public static  string QueryAll = @" SELECT  DivisionUserGroupID ,  DivisionUserID ,  GroupID ,  SystemTimeStamp   FROM [dbo].[pdmSecurityDivUserGroupMember] ";
			
		public static List< PdmSecurityDivUserGroupMemberClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmSecurityDivUserGroupMemberClrDto> listDto = new List<PdmSecurityDivUserGroupMemberClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmSecurityDivUserGroupMemberConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 