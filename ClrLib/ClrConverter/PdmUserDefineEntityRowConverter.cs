using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmUserDefineEntityRowConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmUserDefineEntityRowClrDto ConvertDataRowDto(DataRow row)
        {
            PdmUserDefineEntityRowClrDto aDto = new  PdmUserDefineEntityRowClrDto();
			aDto.EntityRowId =(System.Int32)row["RowID"];
			aDto.EntityId =(System.Int32)row["EntityID"];
			aDto.TextValue =row["TextValue"]as System.String	 ;
			aDto.Value1 =row["Value1"]as System.String	 ;
			aDto.Value2 =row["Value2"]as System.String	 ;
			aDto.Value3 =row["Value3"]as System.String	 ;
			aDto.Value4 =row["Value4"]as System.String	 ;
			aDto.Value5 =row["Value5"]as System.String	 ;
			aDto.Value6 =row["Value6"]as System.String	 ;
			aDto.Value7 =row["Value7"]as System.String	 ;
			aDto.Value8 =row["Value8"]as System.String	 ;
			aDto.Value9 =row["Value9"]as System.String	 ;
			aDto.Value10 =row["Value10"]as System.String	 ;
			aDto.SortOrder =row["SortOrder"]  as System.Int32 ? ;
			aDto.ValueId =row["ValueID"]  as System.Int32 ? ;
			aDto.ValueDate =row["ValueDate"]  as System.DateTime ? ;
			aDto.ValueText =row["ValueText"]as System.String	 ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 