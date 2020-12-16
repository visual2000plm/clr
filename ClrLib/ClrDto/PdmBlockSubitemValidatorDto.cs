using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmBlockSubitemValidatorClrDto 
    {
        public PdmBlockSubitemValidatorClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 SubitemValidatorId
        {
            get ;
            set ;
        }

     
        public  System.Int32 SubItemId
        {
            get ;
            set ;
        }

     
        public  System.Int32 ValidatorType
        {
            get ;
            set ;
        }

     
        public  System.String WarningMessage
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Double> MaximumValue
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Double> MinimumValue
        {
            get ;
            set ;
        }

     
        public  System.String RegularExpression
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

