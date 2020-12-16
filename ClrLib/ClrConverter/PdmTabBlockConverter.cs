using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTabBlockConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmTabBlockClrDto ConvertDataRowDto(DataRow row)
        {
            PdmTabBlockClrDto aDto = new  PdmTabBlockClrDto();
			aDto.TabBlockId =(System.Int32)row["TabBlockID"];
			aDto.TabId =(System.Int32)row["TabID"];
			aDto.BlockId =(System.Int32)row["BlockID"];
            return aDto;
        }
      		
     
		        
    }
}

 