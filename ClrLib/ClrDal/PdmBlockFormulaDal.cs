using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmBlockFormulaDal 
    {
        					
        public static  string QueryAll = @" SELECT  BlockFormulaID ,  BlockID ,  EntityID ,  GridID ,  CaculationFlowSort ,  FormulaExpression ,  WarningMessage ,  FunctionType ,  OperationType ,  ConditionDCUID ,  SwitchTrueFalseType ,  SystemTimeStamp   FROM [dbo].[pdmBlockFormula] ";
			
		public static List< PdmBlockFormulaClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmBlockFormulaClrDto> listDto = new List<PdmBlockFormulaClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmBlockFormulaConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 