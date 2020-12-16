using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmReferenceViewColumnConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmReferenceViewColumnClrDto ConvertDataRowDto(DataRow row)
        {
            PdmReferenceViewColumnClrDto aDto = new  PdmReferenceViewColumnClrDto();
			aDto.ReferenceViewColumnId =(System.Int32)row["ReferenceViewColumnID"];
			aDto.ReferenceViewId =(System.Int32)row["ReferenceViewID"];
			aDto.SubItemId =row["SubItemID"]  as System.Int32 ? ;
			aDto.GridColumnId =row["GridColumnID"]  as System.Int32 ? ;
			aDto.ProductFieldId =row["ProductFieldID"]  as System.Int32 ? ;
			aDto.IsVisible =(System.Boolean)row["IsVisible"];
			aDto.DisplayText =row["DisplayText"]as System.String	 ;
			aDto.Sort =row["Sort"]  as System.Int32 ? ;
			aDto.LabelColumn =row["LabelColumn"]  as System.Int32 ? ;
			aDto.LabelRow =row["LabelRow"]  as System.Int32 ? ;
			aDto.LabelRowSpan =row["LabelRowSpan"]  as System.Int32 ? ;
			aDto.LabelColSpan =row["LabelColSpan"]  as System.Int32 ? ;
			aDto.LabelIsVisible =row["LabelIsVisible"]  as System.Boolean ? ;
			aDto.ValueColumn =row["ValueColumn"]  as System.Int32 ? ;
			aDto.ValueRow =row["ValueRow"]  as System.Int32 ? ;
			aDto.ValueColSpan =row["ValueColSpan"]  as System.Int32 ? ;
			aDto.ValueRowSpan =row["ValueRowSpan"]  as System.Int32 ? ;
			aDto.IsUpdatable =(System.Boolean)row["IsUpdatable"];
			aDto.ValueWidth =row["ValueWidth"]as System.String	 ;
			aDto.ValueHeight =row["ValueHeight"]as System.String	 ;
			aDto.SysTableFiledPath =row["SysTableFiledPath"]as System.String	 ;
			aDto.ControlType =row["ControlType"]  as System.Int32 ? ;
			aDto.EntityId =row["EntityID"]  as System.Int32 ? ;
			aDto.DataType =row["DataType"]  as System.Int32 ? ;
			aDto.IsGroupBy =row["IsGroupBy"]  as System.Boolean ? ;
			aDto.GroupByLevel =row["GroupByLevel"]  as System.Int32 ? ;
			aDto.AggregationFunctionType =row["AggregationFunctionType"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 