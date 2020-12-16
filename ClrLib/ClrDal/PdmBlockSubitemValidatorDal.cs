using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmBlockSubitemValidatorDal 
    {
        					
        public static  string QueryAll = @" SELECT  SubitemValidatorID ,  SubItemID ,  ValidatorType ,  WarningMessage ,  MaximumValue ,  MinimumValue ,  RegularExpression ,  SystemTimeStamp   FROM [dbo].[pdmBlockSubitemValidator] ";
			
		public static List< PdmBlockSubitemValidatorClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmBlockSubitemValidatorClrDto> listDto = new List<PdmBlockSubitemValidatorClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmBlockSubitemValidatorConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 