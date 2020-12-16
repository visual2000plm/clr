using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmErptableColumnMappingDal 
    {
        					
        public static  string QueryAll = @" SELECT  ColumnMappingID ,  MappingID ,  PLMColumn ,  EXColumn ,  FKEntityMappingID ,  EXDataType ,  EXDataLength ,  PLMDataType ,  PLMDataLength ,  IsAllowNull ,  UserDefineEntityColumnID   FROM [dbo].[pdmERPTableColumnMapping] ";
			
		public static List< PdmErptableColumnMappingClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmErptableColumnMappingClrDto> listDto = new List<PdmErptableColumnMappingClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmErptableColumnMappingConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 