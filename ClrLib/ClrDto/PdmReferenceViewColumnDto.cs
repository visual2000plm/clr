using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmReferenceViewColumnClrDto 
    {
        public PdmReferenceViewColumnClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 ReferenceViewColumnId
        {
            get ;
            set ;
        }

     
        public  System.Int32 ReferenceViewId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SubItemId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> GridColumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ProductFieldId
        {
            get ;
            set ;
        }

     
        public  System.Boolean IsVisible
        {
            get ;
            set ;
        }

     
        public  System.String DisplayText
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> Sort
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> LabelColumn
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> LabelRow
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> LabelRowSpan
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> LabelColSpan
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> LabelIsVisible
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ValueColumn
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ValueRow
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ValueColSpan
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ValueRowSpan
        {
            get ;
            set ;
        }

     
        public  System.Boolean IsUpdatable
        {
            get ;
            set ;
        }

     
        public  System.String ValueWidth
        {
            get ;
            set ;
        }

     
        public  System.String ValueHeight
        {
            get ;
            set ;
        }

     
        public  System.String SysTableFiledPath
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ControlType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> EntityId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DataType
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

     
        public  Nullable<System.Int32> AggregationFunctionType
        {
            get ;
            set ;
        }

     
        public  System.Byte[] SystemTimeStamp
        {
            get ;
            set ;
        }
        
        #endregion
 
       
       public  List<PdmSearchTemplateReferenceViewClrDto> PdmSearchTemplateReferenceViewList
        {
          get;
		  set;
        }

		
		

       
        public  PdmBlockSubItemClrDto ForeignPdmBlockSubItemClrDto
        {
          get;
		  set;
        }

       
        public  PdmGridMetaColumnClrDto ForeignPdmGridMetaColumnClrDto
        {
          get;
		  set;
        }

       
        public  PdmReferenceViewClrDto ForeignPdmReferenceViewClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

