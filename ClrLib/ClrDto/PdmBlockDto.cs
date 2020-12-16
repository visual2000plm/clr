using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmBlockClrDto 
    {
        public PdmBlockClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 BlockId
        {
            get ;
            set ;
        }

     
        public  System.String Name
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> CategoryId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> CreatedById
        {
            get ;
            set ;
        }

     
        public  Nullable<System.DateTime> CreatedDate
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ModifiedBy
        {
            get ;
            set ;
        }

     
        public  Nullable<System.DateTime> ModifiedDate
        {
            get ;
            set ;
        }

     
        public  System.String InternalCode
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> UseStandardControl
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> SyncToPdmproduct
        {
            get ;
            set ;
        }

     
        public  System.String Description
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ApproveRoleId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> NotifyVersionChange
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsUseDrillDownControl
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsUseByMerchPlan
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsAllowProductTabCopy
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsMerchPlanCopyAble
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> FolderId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsMasterSynToChild
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsReferenceStaticFiledControl
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsAllowCopyValueInRefSaveAsAndTabCopy
        {
            get ;
            set ;
        }

     
        public  System.String ErpMappingName
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsForcedCreateFirstVersion
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsSynFromQuoteSampleToProduct
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsUsedForAutoGeneration
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SpecialUserDefineType
        {
            get ;
            set ;
        }

     
        public  System.Byte[] SystemTimeStamp
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsAllowCopyValueInTabCopy
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Guid> RowIdentity
        {
            get ;
            set ;
        }
        
        #endregion
 
       
       public  List<PdmBlockFormulaClrDto> PdmBlockFormulaList
        {
          get;
		  set;
        }

       
       public  List<PdmBlockSubItemClrDto> PdmBlockSubItemList
        {
          get;
		  set;
        }

       
       public  List<PdmGridClrDto> PdmGridList
        {
          get;
		  set;
        }

       
       public  List<PdmGridMetaColumnClrDto> PdmGridMetaColumnList
        {
          get;
		  set;
        }

       
       public  List<PdmMassUpdateViewClrDto> PdmMassUpdateViewList
        {
          get;
		  set;
        }

       
       public  List<PdmSearchTemplateDcuClrDto> PdmSearchTemplateDcuList
        {
          get;
		  set;
        }

       
       public  List<PdmSecurityGroupUserRightClrDto> PdmSecurityGroupUserRightList
        {
          get;
		  set;
        }

       
       public  List<PdmTabBlockClrDto> PdmTabBlockList
        {
          get;
		  set;
        }

       
       public  List<PdmTabBlockCaculationFlowClrDto> PdmTabBlockCaculationFlowList
        {
          get;
		  set;
        }

       
       public  List<PdmTabGridMetaColumnClrDto> PdmTabGridMetaColumnList
        {
          get;
		  set;
        }

       
       public  List<PdmTemplateTabLibReferenceSettingClrDto> PdmTemplateTabLibReferenceSettingList
        {
          get;
		  set;
        }

		
		
	
		
		
		
		
	
		        
    }
}

