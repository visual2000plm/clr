using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmEntityMasterChildValueClrDto 
    {
        public PdmEntityMasterChildValueClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 MasterChildValueId
        {
            get ;
            set ;
        }

     
        public  System.Int32 RelationEntityId
        {
            get ;
            set ;
        }

     
        public  System.Int32 MasterValueId
        {
            get ;
            set ;
        }

     
        public  System.Int32 ChildValueId
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
 

		
		

       
        public  PdmEntityBlClrDto ForeignPdmEntityClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

