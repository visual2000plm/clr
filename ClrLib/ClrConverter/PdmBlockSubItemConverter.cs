using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmBlockSubItemConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmBlockSubItemClrDto ConvertDataRowDto(DataRow row)
        {
            PdmBlockSubItemClrDto aDto = new  PdmBlockSubItemClrDto();
			aDto.SubItemId =(System.Int32)row["SubItemID"];
			aDto.BlockId =(System.Int32)row["BlockID"];
			aDto.SubItemName =(System.String)row["SubItemName"];
			aDto.ControlType =(System.Int32)row["ControlType"];
			aDto.DataType =row["DataType"]  as System.Int32 ? ;
			aDto.DefaultValueId =row["DefaultValueID"]  as System.Int32 ? ;
			aDto.DefaultValueDate =row["DefaultValueDate"]  as System.DateTime ? ;
			aDto.EntityId =row["EntityID"]  as System.Int32 ? ;
			aDto.GridId =row["GridID"]  as System.Int32 ? ;
			aDto.InternalCode =row["InternalCode"]as System.String	 ;
			aDto.DefaultText =row["DefaultText"]as System.String	 ;
			aDto.ErpMappingName =row["ERPMappingName"]as System.String	 ;
			aDto.NeedValidator =row["NeedValidator"]  as System.Boolean ? ;
			aDto.ValidatorType =row["ValidatorType"]  as System.Int32 ? ;
			aDto.Nbdecimal =row["NBDecimal"]  as System.Int32 ? ;
			aDto.MasterEntityControlId =row["MasterEntityControlID"]  as System.Int32 ? ;
			aDto.UserDefineEntityColumnId =row["UserDefineEntityColumnID"]  as System.Int32 ? ;
			aDto.SubscribeSourceId =row["SubscribeSourceID"]  as System.Int32 ? ;
			aDto.SortOrder =row["SortOrder"]  as System.Int32 ? ;
			aDto.MaxCharLegnth =row["MaxCharLegnth"]  as System.Int32 ? ;
			aDto.DdlparentLevelId =row["DDLParentLevelID"]  as System.Int32 ? ;
			aDto.AutoIncrementSeed =row["AutoIncrementSeed"]  as System.Int32 ? ;
			aDto.AutoIncrementPrefix =row["AutoIncrementPrefix"]as System.String	 ;
			aDto.AutoIncrementLastId =row["AutoIncrementLastID"]  as System.Int32 ? ;
			aDto.SubscribeGridBlockId =row["SubscribeGridBlockID"]  as System.Int32 ? ;
			aDto.SubscribeGridColumnId =row["SubscribeGridColumnID"]  as System.Int32 ? ;
			aDto.SubscribeColumnAggFuntionId =row["SubscribeColumnAggFuntionID"]  as System.Int32 ? ;
			aDto.IsUniqueForOneRefTxType =row["IsUniqueForOneRefTxType"]  as System.Boolean ? ;
			aDto.IsNeedLog =row["IsNeedLog"]  as System.Boolean ? ;
			aDto.ReferenceStaticFiledId =row["ReferenceStaticFiledID"]  as System.Int32 ? ;
			aDto.IsAllowEmpty =row["IsAllowEmpty"]  as System.Boolean ? ;
			aDto.ToolTip =row["ToolTip"]as System.String	 ;
			aDto.HorizontalAlignment =row["HorizontalAlignment"]  as System.Int32 ? ;
			aDto.IsConvertToUpperCase =row["IsConvertToUpperCase"]  as System.Boolean ? ;
			aDto.SysDefineEntityColumnName =row["SysDefineEntityColumnName"]as System.String	 ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
			aDto.DdlparentIntermediateEntityId =row["DDLParentIntermediateEntityId"]  as System.Int32 ? ;
			aDto.IsPluginAi =row["IsPluginAI"]  as System.Boolean ? ;
			aDto.IsForeignKeyCscading =row["IsForeignKeyCscading"]  as System.Boolean ? ;
            return aDto;
        }
      		
     
		        
    }
}

 