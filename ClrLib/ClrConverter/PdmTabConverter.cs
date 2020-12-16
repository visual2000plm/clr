using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTabConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmTabClrDto ConvertDataRowDto(DataRow row)
        {
            PdmTabClrDto aDto = new  PdmTabClrDto();
			aDto.TabId =(System.Int32)row["TabID"];
			aDto.TabGroupId =row["TabGroupID"]  as System.Int32 ? ;
			aDto.TabName =(System.String)row["TabName"];
			aDto.TabDescription =row["TabDescription"]as System.String	 ;
			aDto.CreatedBy =row["CreatedBy"]  as System.Int32 ? ;
			aDto.CreateDate =row["CreateDate"]  as System.DateTime ? ;
			aDto.ModifydBy =row["ModifydBy"]  as System.Int32 ? ;
			aDto.ModifyDate =row["ModifyDate"]  as System.DateTime ? ;
			aDto.ManagementLevel =row["ManagementLevel"]  as System.Byte ? ;
			aDto.InternalCode =row["InternalCode"]as System.String	 ;
			aDto.IsTemplateHeaderTab =row["IsTemplateHeaderTab"]  as System.Boolean ? ;
			aDto.IsProductDependent =row["IsProductDependent"]  as System.Boolean ? ;
			aDto.IsMasterReferenceHeaderTab =row["IsMasterReferenceHeaderTab"]  as System.Boolean ? ;
			aDto.ParentId =row["ParentID"]  as System.Int32 ? ;
			aDto.IsHideMasterHeader =row["IsHideMasterHeader"]  as System.Boolean ? ;
			aDto.ProductReferenceId =row["ProductReferenceID"]  as System.Int32 ? ;
			aDto.IsHideTemplateHeader =row["IsHideTemplateHeader"]  as System.Boolean ? ;
			aDto.IsUsedForUpdate =row["IsUsedForUpdate"]  as System.Boolean ? ;
			aDto.SearchUpdateRefTxScope =row["SearchUpdateRefTxScope"]  as System.Int32 ? ;
			aDto.IsUsedForSearch =row["IsUsedForSearch"]  as System.Boolean ? ;
			aDto.FolderId =(System.Int32)row["FolderID"];
			aDto.ProductCopyTabSort =row["ProductCopyTabSort"]  as System.Int32 ? ;
			aDto.ProductCopyTabRootTabId =row["ProductCopyTabRootTabID"]  as System.Int32 ? ;
			aDto.SiblingCopyTabId =row["SiblingCopyTabID"]  as System.Int32 ? ;
			aDto.IsTabHeader =row["IsTabHeader"]  as System.Boolean ? ;
			aDto.TabHeaderId =row["TabHeaderID"]  as System.Int32 ? ;
			aDto.IsTemplateHeaderCollpase =row["IsTemplateHeaderCollpase"]  as System.Boolean ? ;
			aDto.IsMasterRefHeaderCollapse =row["IsMasterRefHeaderCollapse"]  as System.Boolean ? ;
			aDto.IsTabHeaderCollapse =row["IsTabHeaderCollapse"]  as System.Boolean ? ;
			aDto.CopyTabValueHolderProductReferenceId =row["CopyTabValueHolderProductReferenceID"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
			aDto.Uilayout =row["UILayout"]as System.Byte[]	 ;
			aDto.RowIdentity =row["RowIdentity"]  as System.Guid ? ;
			aDto.PrintUilayout =row["PrintUILayout"]as System.Byte[]	 ;
			aDto.PrintTabHight =row["PrintTabHight"]  as System.Int32 ? ;
			aDto.PrintTabWidth =row["PrintTabWidth"]  as System.Int32 ? ;
			aDto.IsAllowProductTabCopy =row["IsAllowProductTabCopy"]  as System.Boolean ? ;
            return aDto;
        }
      		
     
		        
    }
}

 