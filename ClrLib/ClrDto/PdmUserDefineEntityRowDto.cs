using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmUserDefineEntityRowClrDto 
    {
        public PdmUserDefineEntityRowClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 EntityRowId
        {
            get ;
            set ;
        }

     
        public  System.Int32 EntityId
        {
            get ;
            set ;
        }

     
        public  System.String TextValue
        {
            get ;
            set ;
        }

     
        public  System.String Value1
        {
            get ;
            set ;
        }

     
        public  System.String Value2
        {
            get ;
            set ;
        }

     
        public  System.String Value3
        {
            get ;
            set ;
        }

     
        public  System.String Value4
        {
            get ;
            set ;
        }

     
        public  System.String Value5
        {
            get ;
            set ;
        }

     
        public  System.String Value6
        {
            get ;
            set ;
        }

     
        public  System.String Value7
        {
            get ;
            set ;
        }

     
        public  System.String Value8
        {
            get ;
            set ;
        }

     
        public  System.String Value9
        {
            get ;
            set ;
        }

     
        public  System.String Value10
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SortOrder
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ValueId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.DateTime> ValueDate
        {
            get ;
            set ;
        }

     
        public  System.String ValueText
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
 
       
       public  List<PdmUserDefineEntityRowValueClrDto> PdmUserDefineEntityRowValueList
        {
          get;
		  set;
        }

		
		

       
        public  PdmEntityBlClrDto ForeignPdmEntityClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

