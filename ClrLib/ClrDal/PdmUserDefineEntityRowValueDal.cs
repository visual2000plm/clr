using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmUserDefineEntityRowValueDal 
    {
        					
        public static  string QueryAll = @" SELECT  EntityRowValueID ,  RowID ,  UserDefineEntityColumnID ,  ValueID ,  ValueDate ,  ValueText ,  SystemTimeStamp   FROM [dbo].[pdmUserDefineEntityRowValue] ";
			
		public static List< PdmUserDefineEntityRowValueClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmUserDefineEntityRowValueClrDto> listDto = new List<PdmUserDefineEntityRowValueClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmUserDefineEntityRowValueConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 