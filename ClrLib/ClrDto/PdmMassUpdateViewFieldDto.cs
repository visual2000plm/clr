using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmMassUpdateViewFieldClrDto 
    {
        public PdmMassUpdateViewFieldClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 MassUpdateViewFieldId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> MassUpdateViewId
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

     
        public  Nullable<System.Boolean> IsReadonly
        {
            get ;
            set ;
        }

     
        public  System.String FriendName
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> Sort
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> Width
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

     
        public  Nullable<System.Boolean> IsHide
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ProductFieldId
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
 

		
		

       
        public  PdmBlockSubItemClrDto ForeignPdmBlockSubItemClrDto
        {
          get;
		  set;
        }

       
        public  PdmMassUpdateViewClrDto ForeignPdmMassUpdateViewClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

