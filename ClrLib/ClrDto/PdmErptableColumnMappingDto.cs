using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmErptableColumnMappingClrDto 
    {
        public PdmErptableColumnMappingClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 ColumnMappingId
        {
            get ;
            set ;
        }

     
        public  System.Int32 MappingId
        {
            get ;
            set ;
        }

     
        public  System.String Plmcolumn
        {
            get ;
            set ;
        }

     
        public  System.String Erpcolumn
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> FkentityMappingId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ExdataType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ExdataLength
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> PlmdataType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> PlmdataLength
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> IsAllowNull
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> UserDefineEntityColumnId
        {
            get ;
            set ;
        }
        
        #endregion
 

		
		

       
        public  PdmErptableMappingClrDto ForeignPdmErptableMapping_ClrDto
        {
          get;
		  set;
        }

       
        public  PdmErptableMappingClrDto ForeignPdmErptableMappingClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

