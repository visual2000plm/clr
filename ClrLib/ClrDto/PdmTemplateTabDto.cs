using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmTemplateTabClrDto 
    {
        public PdmTemplateTabClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 TemplateTabId
        {
            get ;
            set ;
        }

     
        public  System.Int32 TemplateId
        {
            get ;
            set ;
        }

     
        public  System.Int32 TabId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int16> Sort
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> HeaderTabCaculationFlow
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
 

		
		

       
        public  PdmTabClrDto ForeignPdmTabClrDto
        {
          get;
		  set;
        }

       
        public  PdmTemplateClrDto ForeignPdmTemplateClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

