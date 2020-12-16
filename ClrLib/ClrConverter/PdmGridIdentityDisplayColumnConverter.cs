using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmGridIdentityDisplayColumnConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmGridIdentityDisplayColumnClrDto ConvertDataRowDto(DataRow row)
        {
            PdmGridIdentityDisplayColumnClrDto aDto = new  PdmGridIdentityDisplayColumnClrDto();
			aDto.IdentityDisplayColumnId =(System.Int32)row["IdentityDisplayColumnID"];
			aDto.IdentityColumnId =row["IdentityColumnID"]  as System.Int32 ? ;
			aDto.DisplayColumnId =row["DisplayColumnID"]  as System.Int32 ? ;
            return aDto;
        }
      		
     
		        
    }
}

 