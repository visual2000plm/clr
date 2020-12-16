using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityUserContactClrDto 
    {
        public PdmSecurityUserContactClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 ContactId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> UserId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ContactType
        {
            get ;
            set ;
        }

     
        public  System.String ContactFormat
        {
            get ;
            set ;
        }

     
        public  System.String AdditionalContactInfo
        {
            get ;
            set ;
        }

     
        public  System.String Comments
        {
            get ;
            set ;
        }
        
        #endregion
 

		
		
	
		
		
		
		
	
		        
    }
}

