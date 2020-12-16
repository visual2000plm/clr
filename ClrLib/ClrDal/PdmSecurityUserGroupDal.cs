using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityUserGroupDal 
    {
        					
        public static  string QueryAll = @" SELECT  GroupID ,  GroupName ,  Description ,  LoginEvent ,  ParentID ,  InternalCode ,  IsBuiltIn ,  SystemTimeStamp   FROM [dbo].[pdmSecurityUserGroup] ";
			
		public static List< PdmSecurityUserGroupClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmSecurityUserGroupClrDto> listDto = new List<PdmSecurityUserGroupClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmSecurityUserGroupConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 