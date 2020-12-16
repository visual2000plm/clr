using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmBlockSubItemValueDal 
    {
        					
        public static  string QueryAll = @" SELECT  SubItemValueID ,  SubItemID ,  VersionID ,  ValueShareType ,  ValueID ,  ValueDate ,  ValueText ,  SystemTimeStamp   FROM [dbo].[pdmBlockSubItemValue] ";
			
		public static List< PdmBlockSubItemValueClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmBlockSubItemValueClrDto> listDto = new List<PdmBlockSubItemValueClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmBlockSubItemValueConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 