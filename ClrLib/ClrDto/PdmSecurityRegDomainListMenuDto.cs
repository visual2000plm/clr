using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityRegDomainListMenuClrDto 
    {
        public PdmSecurityRegDomainListMenuClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 DomainMenuId
        {
            get ;
            set ;
        }

     
        public  System.Int32 MenuId
        {
            get ;
            set ;
        }

     
        public  System.Int32 DomainId
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
 

		
		

       
        public  PdmSecurityRegDomainClrDto ForeignPdmSecurityRegDomainClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

