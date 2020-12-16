using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmGridMetaColumnConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmGridMetaColumnClrDto ConvertDataRowDto(DataRow row)
        {
            PdmGridMetaColumnClrDto aDto = new  PdmGridMetaColumnClrDto();
			aDto.GridColumnId =(System.Int32)row["GridColumnID"];
			aDto.GridId =(System.Int32)row["GridID"];
			aDto.ColumnName =(System.String)row["ColumnName"];
			aDto.ColumnWidth =row["ColumnWidth"]  as System.Int32 ? ;
			aDto.ColumnTypeId =(System.Int32)row["ColumnTypeID"];
			aDto.DataType =(System.Int32)row["DataType"];
			aDto.ColumnOrder =row["ColumnOrder"]  as System.Int32 ? ;
			aDto.EntityId =row["EntityID"]  as System.Int32 ? ;
			aDto.InternalCode =row["InternalCode"]as System.String	 ;
			aDto.Hidden =row["Hidden"]  as System.Boolean ? ;
			aDto.IsUsedToLockRow =row["IsUsedToLockRow"]  as System.Boolean ? ;
			aDto.IsUsedToDisplayProductGridRowInfo =row["IsUsedToDisplayProductGridRowInfo"]  as System.Boolean ? ;
			aDto.Nbdecimal =row["NBDecimal"]  as System.Int32 ? ;
			aDto.DcucolumnId =row["DCUColumnID"]  as System.Int32 ? ;
			aDto.Dcuid =row["DCUID"]  as System.Int32 ? ;
			aDto.ErpMappingName =row["ERPMappingName"]as System.String	 ;
			aDto.DcucolumnBlockId =row["DCUColumnBlockID"]  as System.Int32 ? ;
			aDto.DisplayProductGridRowInforOrder =row["DisplayProductGridRowInfoOrder"]  as System.Int32 ? ;
			aDto.MasterEntityColumnId =row["MasterEntityColumnID"]  as System.Int32 ? ;
			aDto.ChildColumnId =row["ChildColumnID"]  as System.Int32 ? ;
			aDto.CascadingParentId =row["CascadingParentID"]  as System.Int32 ? ;
			aDto.ProductColorSizeDepdentType =row["ProductColorSizeDepdentType"]  as System.Int32 ? ;
			aDto.DisplayFormat =row["DisplayFormat"]as System.String	 ;
			aDto.DepdentProductColorSizeDepdentTypeColumnId =row["DepdentProductColorSizeDepdentTypeColumnID"]  as System.Int32 ? ;
			aDto.IsPrimaryKey =row["IsPrimaryKey"]  as System.Boolean ? ;
			aDto.IsNeedLog =row["IsNeedLog"]  as System.Boolean ? ;
			aDto.IsReadOnly =row["IsReadOnly"]  as System.Boolean ? ;
			aDto.MasterDcucolumnId =row["MasterDCUColumnID"]  as System.Int32 ? ;
			aDto.PublishSimpleDcuid =row["PublishSimpleDCUID"]  as System.Int32 ? ;
			aDto.PublishSimpleDcutxRefType =row["PublishSimpleDCUTxRefType"]  as System.Int32 ? ;
			aDto.CurrentRefSubscribeSimpleDcuid =row["CurrentRefSubscribeSimpleDCUID"]  as System.Int32 ? ;
			aDto.IsDynamicMatrixKey =row["IsDynamicMatrixKey"]  as System.Boolean ? ;
			aDto.IsDcuforProductGridRef =row["IsDCUForProductGridRef"]  as System.Boolean ? ;
			aDto.IsAutoKeyColumn =row["IsAutoKeyColumn"]  as System.Boolean ? ;
			aDto.IsAutoKeyDisplayColumn =row["IsAutoKeyDisplayColumn"]  as System.Boolean ? ;
			aDto.IsLogicalDisplayForVersionChange =row["IsLogicalDisplayForVersionChange"]  as System.Boolean ? ;
			aDto.IsAllowNull =row["IsAllowNull"]  as System.Boolean ? ;
			aDto.DefaultValue =row["DefaultValue"]as System.String	 ;
			aDto.HorizontalAlignment =row["HorizontalAlignment"]  as System.Int32 ? ;
			aDto.IsGroupBy =row["IsGroupBy"]  as System.Boolean ? ;
			aDto.GroupByLevel =row["GroupByLevel"]  as System.Int32 ? ;
			aDto.RangePlanColumnMapToDcuId =row["RangePlanColumnMapToDcuID"]  as System.Int32 ? ;
			aDto.RangePlanColumnMapToAggegationDcuId =row["RangePlanColumnMapToAggegationDcuID"]  as System.Int32 ? ;
			aDto.IsRangePlanRowLevelKey =row["IsRangePlanRowLevelKey"]  as System.Boolean ? ;
			aDto.RangePlanMapingDcuAggegationType =row["RangePlanMapingDcuAggegationType"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
			aDto.CurrentRefRowLevelSubscribeSimpleDcuid =row["CurrentRefRowLevelSubscribeSimpleDCUID"]  as System.Int32 ? ;
			aDto.CurrentRefRowLevelPublishSimpleDcuid =row["CurrentRefRowLevelPublishSimpleDCUID"]  as System.Int32 ? ;
            return aDto;
        }
      		
     
		        
    }
}

 