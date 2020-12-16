using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmMassUpdateViewMemberClrDto 
    {
        public PdmMassUpdateViewMemberClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 MassUpdateViewMemberId
        {
            get ;
            set ;
        }

     
        public  System.Int32 MassUpdateViewId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SecurityWebUserId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SecurityUserGroupId
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
 

		
		

       
        public  PdmMassUpdateViewClrDto ForeignPdmMassUpdateViewClrDto
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

