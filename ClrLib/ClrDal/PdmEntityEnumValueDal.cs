using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmEntityEnumValueDal 
    {
        					
        public static  string QueryAll = @" SELECT  EnumValueID ,  EntityID ,  EnumKey ,  EnumValue   FROM [dbo].[pdmEntityEnumValue] ";
			
		public static List< PdmEntityEnumValueClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmEntityEnumValueClrDto> listDto = new List<PdmEntityEnumValueClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmEntityEnumValueConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 