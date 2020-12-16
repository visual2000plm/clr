using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmReferenceViewConfigurationClrDto 
    {
        public PdmReferenceViewConfigurationClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 ReferenceViewConfigurationId
        {
            get ;
            set ;
        }

     
        public  System.Int32 ReferenceTypeId
        {
            get ;
            set ;
        }

     
        public  System.Int32 ConfigurationTypeId
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

     
        public  Nullable<System.Int32> ReferenceViewId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SearchTemplateId
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
 

		
		

       
        public  PdmReferenceViewClrDto ForeignPdmReferenceViewClrDto
        {
          get;
		  set;
        }

       
        public  PdmSearchTemplateClrDto ForeignPdmSearchTemplateClrDto
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

