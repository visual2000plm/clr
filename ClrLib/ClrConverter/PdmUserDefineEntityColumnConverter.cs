using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmUserDefineEntityColumnConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmUserDefineEntityColumnClrDto ConvertDataRowDto(DataRow row)
        {
            PdmUserDefineEntityColumnClrDto aDto = new  PdmUserDefineEntityColumnClrDto();
			aDto.UserDefineEntityColumnId =(System.Int32)row["UserDefineEntityColumnID"];
			aDto.EntityId =(System.Int32)row["EntityID"];
			aDto.ColumnName =(System.String)row["ColumnName"];
			aDto.DataType =row["DataType"]  as System.Int32 ? ;
			aDto.UsedByDropDownList =row["UsedByDropDownList"]  as System.Boolean ? ;
			aDto.DataRowSort =row["DataRowSort"]  as System.Int32 ? ;
			aDto.MappingValueKey =row["MappingValueKey"]  as System.Int32 ? ;
			aDto.IsPrimaryKey =row["IsPrimaryKey"]  as System.Boolean ? ;
			aDto.IsIdentity =row["IsIdentity"]  as System.Boolean ? ;
			aDto.IsExtendColumn =row["IsExtendColumn"]  as System.Boolean ? ;
			aDto.SystemTableColumnName =row["SystemTableColumnName"]as System.String	 ;
			aDto.FkentityId =row["FKEntityID"]  as System.Int32 ? ;
			aDto.UicontrolType =row["UIControlType"]  as System.Int32 ? ;
			aDto.Nbdecimal =row["NBDecimal"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 