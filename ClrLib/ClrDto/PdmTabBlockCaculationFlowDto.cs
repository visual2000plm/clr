using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmTabBlockCaculationFlowClrDto 
    {
        public PdmTabBlockCaculationFlowClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 CaculationFlowId
        {
            get ;
            set ;
        }

     
        public  System.Int32 TabId
        {
            get ;
            set ;
        }

     
        public  System.Int32 BlockId
        {
            get ;
            set ;
        }

     
        public  System.Int32 Priority
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
 

		
		

       
        public  PdmBlockClrDto ForeignPdmBlockClrDto
        {
          get;
		  set;
        }

       
        public  PdmTabClrDto ForeignPdmTabClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

