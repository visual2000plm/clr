using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTabBlockCaculationFlowConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmTabBlockCaculationFlowClrDto ConvertDataRowDto(DataRow row)
        {
            PdmTabBlockCaculationFlowClrDto aDto = new  PdmTabBlockCaculationFlowClrDto();
			aDto.CaculationFlowId =(System.Int32)row["CaculationFlowID"];
			aDto.TabId =(System.Int32)row["TabID"];
			aDto.BlockId =(System.Int32)row["BlockID"];
			aDto.Priority =(System.Int32)row["Priority"];
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 