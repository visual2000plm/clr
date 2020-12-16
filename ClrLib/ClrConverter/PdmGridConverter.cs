using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmGridConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmGridClrDto ConvertDataRowDto(DataRow row)
        {
            PdmGridClrDto aDto = new  PdmGridClrDto();
			aDto.GridId =(System.Int32)row["GridID"];
			aDto.GridName =(System.String)row["GridName"];
			aDto.InternalCode =row["InternalCode"]as System.String	 ;
			aDto.RowHight =row["RowHight"]  as System.Int32 ? ;
			aDto.Description =row["Description"]as System.String	 ;
			aDto.IsFixedTableLayout =row["IsFixedTableLayout"]  as System.Boolean ? ;
			aDto.NewRowViewPosition =row["NewRowViewPosition"]  as System.Int32 ? ;
			aDto.GridType =(System.Int32)row["GridType"];
			aDto.ShareSimpleDcuid =row["ShareSimpleDCUID"]  as System.Int32 ? ;
			aDto.ShareTxRefType =row["ShareTxRefType"]  as System.Int32 ? ;
			aDto.SubscribeSimpleDcuid =row["SubscribeSimpleDCUID"]  as System.Int32 ? ;
			aDto.FolderId =row["FolderID"]  as System.Int32 ? ;
			aDto.SearchTemplateId =row["SearchTemplateID"]  as System.Int32 ? ;
			aDto.IsNeedDefualtRow =row["IsNeedDefualtRow"]  as System.Boolean ? ;
			aDto.IsAllowToDeleteRow =row["IsAllowToDeleteRow"]  as System.Boolean ? ;
			aDto.IsAllowToEditRow =row["IsAllowToEditRow"]  as System.Boolean ? ;
			aDto.IsAllowToAddNewRow =row["IsAllowToAddNewRow"]  as System.Boolean ? ;
			aDto.IsAllowEmptyRow =row["IsAllowEmptyRow"]  as System.Boolean ? ;
			aDto.ConceptualTemplateId =row["ConceptualTemplateID"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
			aDto.LinePlanningBlockId =row["LinePlanningBlockID"]  as System.Int32 ? ;
			aDto.CreatedById =row["CreatedByID"]  as System.Int32 ? ;
			aDto.ModifiedBy =row["ModifiedBy"]  as System.Int32 ? ;
			aDto.ModifiedDate =row["ModifiedDate"]  as System.DateTime ? ;
			aDto.CreatedDate =row["CreatedDate"]  as System.DateTime ? ;
            return aDto;
        }
      		
     
		        
    }
}

 