using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmTabBlockClrDto 
    {
        public PdmTabBlockClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 TabBlockId
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

