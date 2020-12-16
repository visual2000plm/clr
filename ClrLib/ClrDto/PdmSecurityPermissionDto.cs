using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityPermissionClrDto 
    {
        public PdmSecurityPermissionClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 PermissionId
        {
            get ;
            set ;
        }

     
        public  System.String Name
        {
            get ;
            set ;
        }

     
        public  System.String Description
        {
            get ;
            set ;
        }

     
        public  System.Int32 InternalCode
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
 
       
       public  List<PdmSecurityUserGroupPermissionClrDto> PdmSecurityUserGroupPermissionList
        {
          get;
		  set;
        }

		
		
	
		
		
		
		
	
		        
    }
}

