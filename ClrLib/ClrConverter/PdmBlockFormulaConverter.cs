using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmBlockFormulaConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmBlockFormulaClrDto ConvertDataRowDto(DataRow row)
        {
            PdmBlockFormulaClrDto aDto = new  PdmBlockFormulaClrDto();
			aDto.BlockFormulaId =(System.Int32)row["BlockFormulaID"];
			aDto.BlockId =row["BlockID"]  as System.Int32 ? ;
			aDto.EntityId =row["EntityID"]  as System.Int32 ? ;
			aDto.GridId =row["GridID"]  as System.Int32 ? ;
			aDto.CaculationFlowSort =row["CaculationFlowSort"]  as System.Int32 ? ;
			aDto.FormulaExpression =(System.String)row["FormulaExpression"];
			aDto.WarningMessage =row["WarningMessage"]as System.String	 ;
			aDto.FunctionType =row["FunctionType"]  as System.Int32 ? ;
			aDto.OperationType =row["OperationType"]  as System.Int32 ? ;
			aDto.ConditionDcuid =row["ConditionDCUID"]  as System.Int32 ? ;
			aDto.SwitchTrueFalseType =row["SwitchTrueFalseType"]  as System.Boolean ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 