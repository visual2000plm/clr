using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityGroupMemberClrDto 
    {
        public PdmSecurityGroupMemberClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 RoleMemberId
        {
            get ;
            set ;
        }

     
        public  System.Int32 GroupId
        {
            get ;
            set ;
        }

     
        public  System.Int32 UserId
        {
            get ;
            set ;
        }

     
        public  System.Boolean IsDefault
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

