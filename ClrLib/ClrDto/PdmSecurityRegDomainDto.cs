using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityRegDomainClrDto 
    {
        public PdmSecurityRegDomainClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 DomainId
        {
            get ;
            set ;
        }

     
        public  System.String DomainCode
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> PersonType
        {
            get ;
            set ;
        }

     
        public  System.String Description
        {
            get ;
            set ;
        }

     
        public  System.String DefaultPage
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
 
       
       public  List<PdmSecurityRegDomainListMenuClrDto> PdmSecurityRegDomainListMenuList
        {
          get;
		  set;
        }

		
		
	
		
		
		
		
	
		        
    }
}

