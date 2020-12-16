using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmUserDefineEntityRowDal 
    {
        					
        public static  string QueryAll = @" SELECT  RowID ,  EntityID ,  TextValue ,  Value1 ,  Value2 ,  Value3 ,  Value4 ,  Value5 ,  Value6 ,  Value7 ,  Value8 ,  Value9 ,  Value10 ,  SortOrder ,  ValueID ,  ValueDate ,  ValueText ,  SystemTimeStamp   FROM [dbo].[pdmUserDefineEntityRow] ";
			
		public static List< PdmUserDefineEntityRowClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmUserDefineEntityRowClrDto> listDto = new List<PdmUserDefineEntityRowClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmUserDefineEntityRowConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 