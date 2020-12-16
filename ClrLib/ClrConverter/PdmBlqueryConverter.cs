using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmBlqueryConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmBlqueryClrDto ConvertDataRowDto(DataRow row)
        {
            PdmBlqueryClrDto aDto = new  PdmBlqueryClrDto();
			aDto.BlqueryId =(System.Int32)row["BLQueryID"];
			aDto.QueryName =row["QueryName"]as System.String	 ;
			aDto.QueryDescription =row["QueryDescription"]as System.String	 ;
			aDto.QueryText =row["QueryText"]as System.String	 ;
			aDto.SearchScopeType =row["SearchScopeType"]  as System.Int32 ? ;
            return aDto;
        }
      		
     
		        
    }
}

 