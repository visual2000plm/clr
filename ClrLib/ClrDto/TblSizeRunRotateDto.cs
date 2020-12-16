using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class TblSizeRunRotateClrDto 
    {
        public TblSizeRunRotateClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 SizeRunRotateId
        {
            get ;
            set ;
        }

     
        public  System.Int32 SizeRunId
        {
            get ;
            set ;
        }

     
        public  System.Byte SizeOrder
        {
            get ;
            set ;
        }

     
        public  System.Byte[] SystemTimeStamp
        {
            get ;
            set ;
        }

     
        public  System.String SizeName
        {
            get ;
            set ;
        }

     
        public  System.String Erpid
        {
            get ;
            set ;
        }

     
        public  System.String IntegrationId
        {
            get ;
            set ;
        }
        
        #endregion
 

		
		

       
        public  TblSizeRunClrDto ForeignTblSizeRunClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

