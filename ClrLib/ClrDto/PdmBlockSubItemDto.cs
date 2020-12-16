using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmBlockSubItemClrDto 
    {
        public PdmBlockSubItemClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 SubItemId
        {
            get ;
            set ;
        }

     
        public  System.Int32 BlockId
        {
            get ;
            set ;
        }

     
        public  System.String SubItemName
        {
            get ;
            set ;
        }

     
        public  System.Int32 ControlType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DataType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DefaultValueId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.DateTime> DefaultValueDate
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> EntityId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> GridId
        {
            get ;
            set ;
        }

     
        public  System.String InternalCode
        {
            get ;
            set ;
        }

     
        public  System.String DefaultText
        {
            get ;
            set ;
        }

     
        public  System.String ErpMappingName
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> NeedValidator
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ValidatorType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> Nbdecimal
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> MasterEntityControlId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> UserDefineEntityColumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SubscribeSourceId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SortOrder
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> MaxCharLegnth
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DdlparentLevelId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> AutoIncrementSeed
        {
            get ;
            set ;
        }

     
        public  System.String AutoIncrementPrefix
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> AutoIncrementLastId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SubscribeGridBlockId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SubscribeGridColumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SubscribeColumnAggFuntionId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsUniqueForOneRefTxType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsNeedLog
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ReferenceStaticFiledId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsAllowEmpty
        {
            get ;
            set ;
        }

     
        public  System.String ToolTip
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> HorizontalAlignment
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsConvertToUpperCase
        {
            get ;
            set ;
        }

     
        public  System.String SysDefineEntityColumnName
        {
            get ;
            set ;
        }

     
        public  System.Byte[] SystemTimeStamp
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DdlparentIntermediateEntityId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsPluginAi
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsForeignKeyCscading
        {
            get ;
            set ;
        }
        
        #endregion
 
       
       public  List<PdmBlockFormulaClrDto> ConditionDcuPdmBlockFormulaList
        {
          get;
		  set;
        }

       
       public  List<PdmBlockSubItemClrDto> SubscribedPdmBlockSubItemList
        {
          get;
		  set;
        }

       
       public  List<PdmBlockSubItemClrDto> CascadingChildrenPdmBlockSubItemList
        {
          get;
		  set;
        }

       
       public  List<PdmBlockSubitemValidatorClrDto> PdmBlockSubitemValidatorList
        {
          get;
		  set;
        }

       
       public  List<PdmBlockSubItemValueClrDto> PdmBlockSubItemValueList
        {
          get;
		  set;
        }

       
       public  List<PdmGridClrDto> PdmGrid_List
        {
          get;
		  set;
        }

       
       public  List<PdmGridMetaColumnClrDto> PdmGridMetaColumn__List
        {
          get;
		  set;
        }

       
       public  List<PdmGridMetaColumnClrDto> PdmGridMetaColumn_List
        {
          get;
		  set;
        }

       
       public  List<PdmMassUpdateViewFieldClrDto> PdmMassUpdateViewFieldList
        {
          get;
		  set;
        }

       
       public  List<PdmReferenceViewColumnClrDto> PdmReferenceViewColumnList
        {
          get;
		  set;
        }

       
       public  List<PdmSearchTemplateDcuClrDto> PdmSearchTemplateDcuList
        {
          get;
		  set;
        }

       
       public  List<PdmTabBlockSubItemExtraInfoClrDto> PdmTabBlockSubItemExtraInfoList
        {
          get;
		  set;
        }

		
		

       
        public  PdmBlockClrDto ForeignSubsribebeGridPdmBlockClrDto
        {
          get;
		  set;
        }

       
        public  PdmBlockClrDto ForeignPdmBlockClrDto
        {
          get;
		  set;
        }

       
        public  PdmBlockSubItemClrDto ForeignMasterEntityPdmBlockSubItemClrDto
        {
          get;
		  set;
        }

       
        public  PdmBlockSubItemClrDto ForeignSubsribeSourcePdmBlockSubItemClrDto
        {
          get;
		  set;
        }

       
        public  PdmBlockSubItemClrDto ForeignDdlparentlevelPdmBlockSubItemClrDto
        {
          get;
		  set;
        }

       
        public  PdmEntityBlClrDto ForeignPdmEntityBlClrDto
        {
          get;
		  set;
        }

       
        public  PdmEntityBlClrDto ForeignPdmEntityBl_ClrDto
        {
          get;
		  set;
        }

       
        public  PdmGridClrDto ForeignPdmGridClrDto
        {
          get;
		  set;
        }

       
        public  PdmGridMetaColumnClrDto ForeignPdmGridMetaColumnClrDto
        {
          get;
		  set;
        }

       
        public  PdmGridMetaColumnAggFunctionClrDto ForeignPdmGridMetaColumnAggFunctionClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

