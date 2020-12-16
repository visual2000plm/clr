using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmTableTimestampClrDto 
    {
        public PdmTableTimestampClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Byte[] SysTimeStamp
        {
            get ;
            set ;
        }

     
        public  System.String TableName
        {
            get ;
            set ;
        }

     
        public  Nullable<System.DateTime> LastScanTime
        {
            get ;
            set ;
        }

     
        public  System.Int32 TableTimeStampId
        {
            get ;
            set ;
        }
        
        #endregion
 

		
		
	
		
		
		
		
	
		        
    }
}

