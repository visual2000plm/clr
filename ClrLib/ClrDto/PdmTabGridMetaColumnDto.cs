using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmTabGridMetaColumnClrDto 
    {
        public PdmTabGridMetaColumnClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 TabGridMetalColumnId
        {
            get ;
            set ;
        }

     
        public  System.Int32 TabId
        {
            get ;
            set ;
        }

     
        public  System.Int32 GridColumnId
        {
            get ;
            set ;
        }

     
        public  System.String AliasName
        {
            get ;
            set ;
        }

        public System.String ExternalMappingName
        {
            get ;
            set ;
        }

        

     
        public  Nullable<System.Int32> BlockId
        {
            get ;
            set ;
        }

     
        public  System.Byte[] SystemTimeStamp
        {
            get ;
            set ;
        }

     
        public  System.Boolean Visible
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

       
        public  PdmGridMetaColumnClrDto ForeignPdmGridMetaColumnClrDto
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

