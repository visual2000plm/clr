using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmEntityEnumValueClrDto 
    {
        public PdmEntityEnumValueClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 EnumValueId
        {
            get ;
            set ;
        }

     
        public  System.Int32 EntityId
        {
            get ;
            set ;
        }

     
        public  System.Int32 EnumKey
        {
            get ;
            set ;
        }

     
        public  System.String EnumValue
        {
            get ;
            set ;
        }
        
        #endregion
 

		
		

       
        public  PdmEntityBlClrDto ForeignPdmEntityBlClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

