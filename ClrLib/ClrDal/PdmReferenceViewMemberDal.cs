using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmReferenceViewMemberDal 
    {
        					
        public static  string QueryAll = @" SELECT  ReferenceViewMemberID ,  ReferenceViewID ,  SecurityWebUserID ,  SecurityUserGroupID ,  SystemTimeStamp   FROM [dbo].[pdmReferenceViewMember] ";
			
		public static List< PdmReferenceViewMemberClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmReferenceViewMemberClrDto> listDto = new List<PdmReferenceViewMemberClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmReferenceViewMemberConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 