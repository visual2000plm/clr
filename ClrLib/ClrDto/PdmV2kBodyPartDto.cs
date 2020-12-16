using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmV2kBodyPartClrDto 
    {
        public PdmV2kBodyPartClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 BodyPartId
        {
            get ;
            set ;
        }

     
        public  System.String BodyPartName
        {
            get ;
            set ;
        }

     
        public  System.String Code
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> GroupId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ProductReferenceId
        {
            get ;
            set ;
        }

     
        public  System.String Description
        {
            get ;
            set ;
        }

     
        public  System.String MeasureInstruction
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Decimal> InitialSpecValue
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Decimal> Tolerance
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Decimal> GradingPlusValue
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Decimal> GradingMinuValue
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsHight
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> FolderId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsImport
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SketchId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> MeasureOfUnitId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DimensionId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DimensionDetailId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsNeedToApplyGradingRule
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
 

		
		
	
		
		
		
		
	
		        
    }
}

