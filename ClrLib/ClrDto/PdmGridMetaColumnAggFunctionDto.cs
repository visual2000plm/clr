using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmGridMetaColumnAggFunctionClrDto 
    {
        public PdmGridMetaColumnAggFunctionClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 GridMetaColumnFunctionId
        {
            get ;
            set ;
        }

     
        public  System.Int32 GridColumnId
        {
            get ;
            set ;
        }

     
        public  System.Int32 AggregationFunctionType
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
 
       
       public  List<PdmBlockSubItemClrDto> PdmBlockSubItemSubscribeList
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

