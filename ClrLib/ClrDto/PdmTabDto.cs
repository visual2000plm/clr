using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmTabClrDto 
    {
        public PdmTabClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 TabId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> TabGroupId
        {
            get ;
            set ;
        }

     
        public  System.String TabName
        {
            get ;
            set ;
        }

     
        public  System.String TabDescription
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> CreatedBy
        {
            get ;
            set ;
        }

     
        public  Nullable<System.DateTime> CreateDate
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ModifydBy
        {
            get ;
            set ;
        }

     
        public  Nullable<System.DateTime> ModifyDate
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Byte> ManagementLevel
        {
            get ;
            set ;
        }

     
        public  System.String InternalCode
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsTemplateHeaderTab
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsProductDependent
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsMasterReferenceHeaderTab
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ParentId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsHideMasterHeader
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ProductReferenceId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsHideTemplateHeader
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsUsedForUpdate
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SearchUpdateRefTxScope
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsUsedForSearch
        {
            get ;
            set ;
        }

     
        public  System.Int32 FolderId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ProductCopyTabSort
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ProductCopyTabRootTabId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SiblingCopyTabId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsTabHeader
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> TabHeaderId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsTemplateHeaderCollpase
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsMasterRefHeaderCollapse
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsTabHeaderCollapse
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> CopyTabValueHolderProductReferenceId
        {
            get ;
            set ;
        }

     
        public  System.Byte[] SystemTimeStamp
        {
            get ;
            set ;
        }

     
        public  System.Byte[] Uilayout
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Guid> RowIdentity
        {
            get ;
            set ;
        }

     
        public  System.Byte[] PrintUilayout
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> PrintTabHight
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> PrintTabWidth
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsAllowProductTabCopy
        {
            get ;
            set ;
        }
        
        #endregion
 
       
       public  List<PdmDwrequireTabAndGridClrDto> PdmDwrequireTabAndGridList
        {
          get;
		  set;
        }

       
       public  List<PdmMassUpdateViewClrDto> PdmMassUpdateViewList
        {
          get;
		  set;
        }

       
       public  List<PdmSearchTemplateClrDto> PdmSearchTemplateList
        {
          get;
		  set;
        }

       
       public  List<PdmTabClrDto> ChildPdmTabList
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

       
       public  List<PdmTabBlockSubItemExtraInfoClrDto> PdmTabBlockSubItemExtraInfoList
        {
          get;
		  set;
        }

       
       public  List<PdmTabGridMetaColumnClrDto> PdmTabGridMetaColumnList
        {
          get;
		  set;
        }

       
       public  List<PdmTemplateTabClrDto> PdmTemplateTabList
        {
          get;
		  set;
        }

       
       public  List<PdmTemplateTabLibReferenceSettingClrDto> PdmTemplateTabLibReferenceSettingList
        {
          get;
		  set;
        }

		
		

       
        public  PdmTabClrDto ForeignParentPdmTabClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

