using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmBlockSubItemValueClrDto 
    {
        public PdmBlockSubItemValueClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 SubItemValueId
        {
            get ;
            set ;
        }

     
        public  System.Int32 SubItemId
        {
            get ;
            set ;
        }

     
        public  System.Int32 VersionId
        {
            get ;
            set ;
        }

     
        public  System.Byte ValueShareType
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
 

		
		

       
        public  PdmBlockSubItemClrDto ForeignPdmBlockSubItemClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

