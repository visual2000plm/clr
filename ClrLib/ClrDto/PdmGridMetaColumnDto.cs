using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmGridMetaColumnClrDto 
    {
        public PdmGridMetaColumnClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 GridColumnId
        {
            get ;
            set ;
        }

     
        public  System.Int32 GridId
        {
            get ;
            set ;
        }

     
        public  System.String ColumnName
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ColumnWidth
        {
            get ;
            set ;
        }

     
        public  System.Int32 ColumnTypeId
        {
            get ;
            set ;
        }

     
        public  System.Int32 DataType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ColumnOrder
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> EntityId
        {
            get ;
            set ;
        }

     
        public  System.String InternalCode
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> Hidden
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsUsedToLockRow
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsUsedToDisplayProductGridRowInfo
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> Nbdecimal
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DcucolumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> Dcuid
        {
            get ;
            set ;
        }

     
        public  System.String ErpMappingName
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DcucolumnBlockId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DisplayProductGridRowInforOrder
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> MasterEntityColumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ChildColumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> CascadingParentId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ProductColorSizeDepdentType
        {
            get ;
            set ;
        }

     
        public  System.String DisplayFormat
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DepdentProductColorSizeDepdentTypeColumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsPrimaryKey
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsNeedLog
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsReadOnly
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> MasterDcucolumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> PublishSimpleDcuid
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> PublishSimpleDcutxRefType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> CurrentRefSubscribeSimpleDcuid
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsDynamicMatrixKey
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsDcuforProductGridRef
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsAutoKeyColumn
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsAutoKeyDisplayColumn
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsLogicalDisplayForVersionChange
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsAllowNull
        {
            get ;
            set ;
        }

     
        public  System.String DefaultValue
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> HorizontalAlignment
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsGroupBy
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> GroupByLevel
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> RangePlanColumnMapToDcuId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> RangePlanColumnMapToAggegationDcuId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsRangePlanRowLevelKey
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> RangePlanMapingDcuAggegationType
        {
            get ;
            set ;
        }

     
        public  System.Byte[] SystemTimeStamp
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> CurrentRefRowLevelSubscribeSimpleDcuid
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> CurrentRefRowLevelPublishSimpleDcuid
        {
            get ;
            set ;
        }
        
        #endregion
 
       
       public  List<PdmBlockSubItemClrDto> SubscribePdmBlockSubItemList
        {
          get;
		  set;
        }

       
       public  List<PdmGridIdentityDisplayColumnClrDto> PdmGridIdentityDisplayColumn_List
        {
          get;
		  set;
        }

       
       public  List<PdmGridIdentityDisplayColumnClrDto> PdmGridIdentityDisplayColumnList
        {
          get;
		  set;
        }

       
       public  List<PdmGridMetaColumnAggFunctionClrDto> PdmGridMetaColumnAggFunctionList
        {
          get;
		  set;
        }

       
       public  List<PdmSearchTemplateDcuClrDto> PdmSearchTemplateDcuList
        {
          get;
		  set;
        }

       
       public  List<PdmTabGridMetaColumnClrDto> PdmTabGridMetaColumnList
        {
          get;
		  set;
        }

		
		

       
        public  PdmBlockClrDto ForeignDcuColumnPdmBlockClrDto
        {
          get;
		  set;
        }

       
        public  PdmBlockSubItemClrDto ForeignPdmBlockSubItem_ClrDto
        {
          get;
		  set;
        }

       
        public  PdmBlockSubItemClrDto ForeignPdmBlockSubItemClrDto
        {
          get;
		  set;
        }

       
        public  PdmEntityBlClrDto ForeignPdmEntityBlClrDto
        {
          get;
		  set;
        }

       
        public  PdmGridClrDto ForeignPdmGridClrDto
        {
          get;
		  set;
        }

       
        public  PdmGridMetaColumnClrDto ForeignMasterentityPdmGridMetaColumnClrDto
        {
          get;
		  set;
        }

       
        public  PdmUserDefineEntityColumnClrDto ForeignPdmUserDefineEntityColumnClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

