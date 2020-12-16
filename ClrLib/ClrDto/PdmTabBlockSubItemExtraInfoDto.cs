using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmTabBlockSubItemExtraInfoClrDto 
    {
        public PdmTabBlockSubItemExtraInfoClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 BlockSubItemExtraInfoId
        {
            get ;
            set ;
        }

     
        public  System.Int32 SubItemId
        {
            get ;
            set ;
        }

     
        public  System.Int32 TabId
        {
            get ;
            set ;
        }

        public System.String ExternalMappingName
        {
            get;
            set;
        }

        public  System.String AliasName
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> Width
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> Height
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
 

		
		

       
        public  PdmBlockSubItemClrDto ForeignPdmBlockSubItemClrDto
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

