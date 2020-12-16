using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityDivUserGroupMemberClrDto 
    {
        public PdmSecurityDivUserGroupMemberClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 DivisionUserGroupId
        {
            get ;
            set ;
        }

     
        public  System.Int32 DivisionUserId
        {
            get ;
            set ;
        }

     
        public  System.Int32 GroupId
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
 

		
		

       
        public  PdmSecurityUserGroupClrDto ForeignPdmSecurityUserGroupClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

