using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityUserGroupPermissionDal 
    {
        					
        public static  string QueryAll = @" SELECT  UserGroupPermissionID ,  GroupID ,  PermissionID ,  SystemTimeStamp   FROM [dbo].[pdmSecurityUserGroupPermission] ";
			
		public static List< PdmSecurityUserGroupPermissionClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmSecurityUserGroupPermissionClrDto> listDto = new List<PdmSecurityUserGroupPermissionClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmSecurityUserGroupPermissionConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 