using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmBlockSubItemValueConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmBlockSubItemValueClrDto ConvertDataRowDto(DataRow row)
        {
            PdmBlockSubItemValueClrDto aDto = new  PdmBlockSubItemValueClrDto();
			aDto.SubItemValueId =(System.Int32)row["SubItemValueID"];
			aDto.SubItemId =(System.Int32)row["SubItemID"];
			aDto.VersionId =(System.Int32)row["VersionID"];
			aDto.ValueShareType =(System.Byte)row["ValueShareType"];
			aDto.ValueId =row["ValueID"]  as System.Int32 ? ;
			aDto.ValueDate =row["ValueDate"]  as System.DateTime ? ;
			aDto.ValueText =row["ValueText"]as System.String	 ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 