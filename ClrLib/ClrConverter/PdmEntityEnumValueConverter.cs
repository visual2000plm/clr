using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmEntityEnumValueConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmEntityEnumValueClrDto ConvertDataRowDto(DataRow row)
        {
            PdmEntityEnumValueClrDto aDto = new  PdmEntityEnumValueClrDto();
			aDto.EnumValueId =(System.Int32)row["EnumValueID"];
			aDto.EntityId =(System.Int32)row["EntityID"];
			aDto.EnumKey =(System.Int32)row["EnumKey"];
			aDto.EnumValue =(System.String)row["EnumValue"];
            return aDto;
        }
      		
     
		        
    }
}

 