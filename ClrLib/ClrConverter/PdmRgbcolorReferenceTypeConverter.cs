using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmRgbcolorReferenceTypeConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmRgbcolorReferenceTypeClrDto ConvertDataRowDto(DataRow row)
        {
            PdmRgbcolorReferenceTypeClrDto aDto = new  PdmRgbcolorReferenceTypeClrDto();
			aDto.ColorReferenceTypeId =(System.Int32)row["ColorReferenceTypeID"];
			aDto.ReferenceGroup =(System.String)row["ReferenceGroup"];
			aDto.ReferenceDescription =row["ReferenceDescription"]as System.String	 ;
            return aDto;
        }
      		
     
		        
    }
}

 