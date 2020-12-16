using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmBlqueryClrDto 
    {
        public PdmBlqueryClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 BlqueryId
        {
            get ;
            set ;
        }

     
        public  System.String QueryName
        {
            get ;
            set ;
        }

     
        public  System.String QueryDescription
        {
            get ;
            set ;
        }

     
        public  System.String QueryText
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SearchScopeType
        {
            get ;
            set ;
        }
        
        #endregion
 
       
       public  List<PdmReferenceViewClrDto> PdmReferenceViewList
        {
          get;
		  set;
        }

       
       public  List<PdmSearchTemplateClrDto> PdmSearchTemplateList
        {
          get;
		  set;
        }

		
		
	
		
		
		
		
	
		        
    }
}

