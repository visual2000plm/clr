using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmUserDefineEntityRowValueClrDto 
    {
        public PdmUserDefineEntityRowValueClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 EntityRowValueId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> RowId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> UserDefineEntityColumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ValueId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.DateTime> ValueDate
        {
            get ;
            set ;
        }

     
        public  System.String ValueText
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
 

		
		

       
        public  PdmUserDefineEntityRowClrDto ForeignPdmUserDefineEntityRowClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

