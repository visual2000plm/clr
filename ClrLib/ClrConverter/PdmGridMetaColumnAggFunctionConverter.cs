using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmGridMetaColumnAggFunctionConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmGridMetaColumnAggFunctionClrDto ConvertDataRowDto(DataRow row)
        {
            PdmGridMetaColumnAggFunctionClrDto aDto = new  PdmGridMetaColumnAggFunctionClrDto();
			aDto.GridMetaColumnFunctionId =(System.Int32)row["GridMetaColumnFunctionID"];
			aDto.GridColumnId =(System.Int32)row["GridColumnID"];
			aDto.AggregationFunctionType =(System.Int32)row["AggregationFunctionType"];
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 