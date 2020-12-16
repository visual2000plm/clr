using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmDwrequireTabAndGridClrDto 
    {
        public PdmDwrequireTabAndGridClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 RequireTabGridId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> TabId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> BlockId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> GridId
        {
            get ;
            set ;
        }
        
        #endregion
 

		
		

       
        public  PdmGridClrDto ForeignPdmGridClrDto
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

