using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmUserDefineEntityRowValueConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmUserDefineEntityRowValueClrDto ConvertDataRowDto(DataRow row)
        {
            PdmUserDefineEntityRowValueClrDto aDto = new  PdmUserDefineEntityRowValueClrDto();
			aDto.EntityRowValueId =(System.Int32)row["EntityRowValueID"];
			aDto.RowId =row["RowID"]  as System.Int32 ? ;
			aDto.UserDefineEntityColumnId =row["UserDefineEntityColumnID"]  as System.Int32 ? ;
			aDto.ValueId =row["ValueID"]  as System.Int32 ? ;
			aDto.ValueDate =row["ValueDate"]  as System.DateTime ? ;
			aDto.ValueText =row["ValueText"]as System.String	 ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 