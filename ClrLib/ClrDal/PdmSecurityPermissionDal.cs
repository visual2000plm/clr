using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityPermissionDal 
    {
        					
        public static  string QueryAll = @" SELECT  PermissionID ,  Name ,  Description ,  InternalCode ,  SystemTimeStamp   FROM [dbo].[pdmSecurityPermission] ";
			
		public static List< PdmSecurityPermissionClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmSecurityPermissionClrDto> listDto = new List<PdmSecurityPermissionClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmSecurityPermissionConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 