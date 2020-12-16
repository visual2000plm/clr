using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSearchTemplateDcuConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmSearchTemplateDcuClrDto ConvertDataRowDto(DataRow row)
        {
            PdmSearchTemplateDcuClrDto aDto = new  PdmSearchTemplateDcuClrDto();
			aDto.SearchTemplateDcuid =(System.Int32)row["SearchTemplateDCUID"];
			aDto.SearchTemplateId =row["SearchTemplateID"]  as System.Int32 ? ;
			aDto.SubitemId =row["SubitemID"]  as System.Int32 ? ;
			aDto.GridColumnId =row["GridColumnID"]  as System.Int32 ? ;
			aDto.EmStaticSearchControl =row["EmStaticSearchControl"]  as System.Int32 ? ;
			aDto.Sort =row["Sort"]  as System.Int32 ? ;
			aDto.DcucolumnBlockId =row["DCUColumnBlockID"]  as System.Int32 ? ;
			aDto.PositionRow =row["PositionRow"]  as System.Int32 ? ;
			aDto.PositionColumn =row["PositionColumn"]  as System.Int32 ? ;
			aDto.OperationId =row["OperationID"]  as System.Int32 ? ;
			aDto.DisplayText =row["DisplayText"]as System.String	 ;
			aDto.IsVisible =(System.Boolean)row["IsVisible"];
			aDto.DefaultValue =row["DefaultValue"]as System.String	 ;
			aDto.IsReadOnly =(System.Boolean)row["IsReadOnly"];
			aDto.IsAutoPopulate =(System.Boolean)row["IsAutoPopulate"];
			aDto.ParentDcuid =row["ParentDCUID"]  as System.Int32 ? ;
			aDto.IsLoadOnDemand =row["IsLoadOnDemand"]  as System.Boolean ? ;
			aDto.SysTableFiledPath =row["SysTableFiledPath"]as System.String	 ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
			aDto.ControlType =row["ControlType"]  as System.Int32 ? ;
			aDto.EntityId =row["EntityID"]  as System.Int32 ? ;
			aDto.DataType =row["DataType"]  as System.Int32 ? ;
            return aDto;
        }
      		
     
		        
    }
}

 