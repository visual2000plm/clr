using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityUserGroupPermissionClrDto 
    {
        public PdmSecurityUserGroupPermissionClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 UserGroupPermissionId
        {
            get ;
            set ;
        }

     
        public  System.Int32 GroupId
        {
            get ;
            set ;
        }

     
        public  System.Int32 PermissionId
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
 

		
		

       
        public  PdmSecurityPermissionClrDto ForeignPdmSecurityPermissionClrDto
        {
          get;
		  set;
        }

       
        public  PdmSecurityUserGroupClrDto ForeignPdmSecurityUserGroupClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

