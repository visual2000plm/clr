using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmMassUpdateViewMemberDal 
    {
        					
        public static  string QueryAll = @" SELECT  MassUpdateViewMemberID ,  MassUpdateViewID ,  SecurityWebUserID ,  SecurityUserGroupID ,  SystemTimeStamp   FROM [dbo].[pdmMassUpdateViewMember] ";
			
		public static List< PdmMassUpdateViewMemberClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmMassUpdateViewMemberClrDto> listDto = new List<PdmMassUpdateViewMemberClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmMassUpdateViewMemberConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 