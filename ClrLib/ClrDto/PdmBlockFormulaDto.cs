using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmBlockFormulaClrDto 
    {
        public PdmBlockFormulaClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 BlockFormulaId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> BlockId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> EntityId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> GridId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> CaculationFlowSort
        {
            get ;
            set ;
        }

     
        public  System.String FormulaExpression
        {
            get ;
            set ;
        }

     
        public  System.String WarningMessage
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> FunctionType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> OperationType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ConditionDcuid
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> SwitchTrueFalseType
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
 

		
		

       
        public  PdmBlockClrDto ForeignPdmBlockClrDto
        {
          get;
		  set;
        }

       
        public  PdmBlockSubItemClrDto ForeignConditionDcuPdmBlockSubItemClrDto
        {
          get;
		  set;
        }

       
        public  PdmEntityBlClrDto ForeignPdmEntityBlClrDto
        {
          get;
		  set;
        }

       
        public  PdmGridClrDto ForeignPdmGridClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

