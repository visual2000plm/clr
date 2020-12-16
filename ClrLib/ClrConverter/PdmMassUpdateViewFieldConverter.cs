using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmMassUpdateViewFieldConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmMassUpdateViewFieldClrDto ConvertDataRowDto(DataRow row)
        {
            PdmMassUpdateViewFieldClrDto aDto = new  PdmMassUpdateViewFieldClrDto();
			aDto.MassUpdateViewFieldId =(System.Int32)row["MassUpdateViewFieldID"];
			aDto.MassUpdateViewId =row["MassUpdateViewID"]  as System.Int32 ? ;
			aDto.SubItemId =row["SubItemID"]  as System.Int32 ? ;
			aDto.GridColumnId =row["GridColumnID"]  as System.Int32 ? ;
			aDto.IsReadonly =row["IsReadonly"]  as System.Boolean ? ;
			aDto.FriendName =row["FriendName"]as System.String	 ;
			aDto.Sort =row["Sort"]  as System.Int32 ? ;
			aDto.Width =row["Width"]  as System.Int32 ? ;
			aDto.IsGroupBy =row["IsGroupBy"]  as System.Boolean ? ;
			aDto.GroupByLevel =row["GroupByLevel"]  as System.Int32 ? ;
			aDto.AggregationFunctionType =row["AggregationFunctionType"]  as System.Int32 ? ;
			aDto.IsHide =row["IsHide"]  as System.Boolean ? ;
			aDto.ProductFieldId =row["ProductFieldId"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 