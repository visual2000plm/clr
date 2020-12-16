using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class TblDimensionClrDto 
    {
        public TblDimensionClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 DimensionId
        {
            get ;
            set ;
        }

     
        public  System.String DimensionCode
        {
            get ;
            set ;
        }

     
        public  System.String DimensionDesc
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> PublishedMode
        {
            get ;
            set ;
        }

     
        public  System.Byte[] SystemTimeStamp
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
 
       
       public  List<TblDimensionDetailClrDto> TblDimensionDetailList
        {
          get;
		  set;
        }

		
		
	
		
		
		
		
	
		        
    }
}

