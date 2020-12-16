using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmGridIdentityDisplayColumnClrDto 
    {
        public PdmGridIdentityDisplayColumnClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 IdentityDisplayColumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> IdentityColumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DisplayColumnId
        {
            get ;
            set ;
        }
        
        #endregion
 

		
		

       
        public  PdmGridMetaColumnClrDto ForeignPdmGridMetaColumn_ClrDto
        {
          get;
		  set;
        }

       
        public  PdmGridMetaColumnClrDto ForeignPdmGridMetaColumnClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

