using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmDwrequireTabAndGridConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmDwrequireTabAndGridClrDto ConvertDataRowDto(DataRow row)
        {
            PdmDwrequireTabAndGridClrDto aDto = new  PdmDwrequireTabAndGridClrDto();
			aDto.RequireTabGridId =(System.Int32)row["RequireTabGridID"];
			aDto.TabId =row["TabID"]  as System.Int32 ? ;
			aDto.BlockId =row["BlockID"]  as System.Int32 ? ;
			aDto.GridId =row["GridID"]  as System.Int32 ? ;
            return aDto;
        }
      		
     
		        
    }
}

 