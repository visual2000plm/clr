using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmSearchTemplateReferenceViewClrDto 
    {
        public PdmSearchTemplateReferenceViewClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 SearchTemplateViewId
        {
            get ;
            set ;
        }

     
        public  System.Int32 SearchTemplateId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ReferenceViewId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ViewFilterColumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> MassUpdateViewId
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

       
        public  PdmReferenceViewClrDto ForeignPdmReferenceViewClrDto
        {
          get;
		  set;
        }

       
        public  PdmReferenceViewColumnClrDto ForeignPdmReferenceViewColumnClrDto
        {
          get;
		  set;
        }

       
        public  PdmSearchTemplateClrDto ForeignPdmSearchTemplateClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

