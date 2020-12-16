using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityGroupUserRightDal 
    {
        					
        public static  string QueryAll = @" SELECT  SecurityRightID ,  GroupID ,  UserID ,  BlockID ,  RefTxType ,  TabID ,  TemplateID ,  IsInVisible ,  IsUnSaveAble ,  IsSpecialPermission ,  GridID ,  GridMetaColumnID ,  SubItemID ,  SystemTimeStamp   FROM [dbo].[pdmSecurityGroupUserRight] ";
			
		public static List< PdmSecurityGroupUserRightClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmSecurityGroupUserRightClrDto> listDto = new List<PdmSecurityGroupUserRightClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmSecurityGroupUserRightConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 