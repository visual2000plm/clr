using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmBlockConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmBlockClrDto ConvertDataRowDto(DataRow row)
        {
            PdmBlockClrDto aDto = new  PdmBlockClrDto();
			aDto.BlockId =(System.Int32)row["BlockID"];
			aDto.Name =(System.String)row["Name"];
			aDto.CategoryId =row["CategoryID"]  as System.Int32 ? ;
			aDto.CreatedById =row["CreatedByID"]  as System.Int32 ? ;
			aDto.CreatedDate =row["CreatedDate"]  as System.DateTime ? ;
			aDto.ModifiedBy =row["ModifiedBy"]  as System.Int32 ? ;
			aDto.ModifiedDate =row["ModifiedDate"]  as System.DateTime ? ;
			aDto.InternalCode =row["InternalCode"]as System.String	 ;
			aDto.UseStandardControl =row["UseStandardControl"]  as System.Boolean ? ;
			aDto.SyncToPdmproduct =row["SyncToPDMProduct"]  as System.Boolean ? ;
			aDto.Description =row["Description"]as System.String	 ;
			aDto.ApproveRoleId =row["ApproveRoleID"]  as System.Int32 ? ;
			aDto.NotifyVersionChange =row["NotifyVersionChange"]  as System.Boolean ? ;
			aDto.IsUseDrillDownControl =row["IsUseDrillDownControl"]  as System.Boolean ? ;
			aDto.IsUseByMerchPlan =row["IsUseByMerchPlan"]  as System.Boolean ? ;
			aDto.IsAllowProductTabCopy =row["IsAllowProductTabCopy"]  as System.Boolean ? ;
			aDto.IsMerchPlanCopyAble =row["IsMerchPlanCopyAble"]  as System.Boolean ? ;
			aDto.FolderId =row["FolderID"]  as System.Int32 ? ;
			aDto.IsMasterSynToChild =row["IsMasterSynToChild"]  as System.Boolean ? ;
			aDto.IsReferenceStaticFiledControl =row["IsReferenceStaticFiledControl"]  as System.Boolean ? ;
			aDto.IsAllowCopyValueInRefSaveAsAndTabCopy =row["IsAllowCopyValueInRefSaveAs"]  as System.Boolean ? ;
			aDto.ErpMappingName =row["ERPMappingName"]as System.String	 ;
			aDto.IsForcedCreateFirstVersion =row["IsForcedCreateFirstVersion"]  as System.Boolean ? ;
			aDto.IsSynFromQuoteSampleToProduct =row["IsSynFromQuoteSampleToProduct"]  as System.Boolean ? ;
			aDto.IsUsedForAutoGeneration =row["IsUsedForAutoGeneration"]  as System.Boolean ? ;
			aDto.SpecialUserDefineType =row["SpecialUserDefineType"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
			aDto.IsAllowCopyValueInTabCopy =row["IsAllowCopyValueInTabCopy"]  as System.Boolean ? ;
			aDto.RowIdentity =row["RowIdentity"]  as System.Guid ? ;
            return aDto;
        }
      		
     
		        
    }
}

 