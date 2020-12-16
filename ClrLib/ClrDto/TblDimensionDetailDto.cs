using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class TblDimensionDetailClrDto 
    {
        public TblDimensionDetailClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 DimensionDetailId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DimensionId
        {
            get ;
            set ;
        }

     
        public  System.String DimDetailCode
        {
            get ;
            set ;
        }

     
        public  System.String DimDetailDesc
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DimDetailSort
        {
            get ;
            set ;
        }

     
        public  System.Byte[] SystemTimeStamp
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SystemSort
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
 

		
		

       
        public  TblDimensionClrDto ForeignTblDimensionClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

