using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmSearchTemplateDcuClrDto 
    {
        public PdmSearchTemplateDcuClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 SearchTemplateDcuid
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SearchTemplateId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SubitemId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> GridColumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> EmStaticSearchControl
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> Sort
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DcucolumnBlockId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> PositionRow
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> PositionColumn
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> OperationId
        {
            get ;
            set ;
        }

     
        public  System.String DisplayText
        {
            get ;
            set ;
        }

     
        public  System.Boolean IsVisible
        {
            get ;
            set ;
        }

     
        public  System.String DefaultValue
        {
            get ;
            set ;
        }

     
        public  System.Boolean IsReadOnly
        {
            get ;
            set ;
        }

     
        public  System.Boolean IsAutoPopulate
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ParentDcuid
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsLoadOnDemand
        {
            get ;
            set ;
        }

     
        public  System.String SysTableFiledPath
        {
            get ;
            set ;
        }

     
        public  System.Byte[] SystemTimeStamp
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ControlType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> EntityId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DataType
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

       
        public  PdmBlockSubItemClrDto ForeignPdmBlockSubItemClrDto
        {
          get;
		  set;
        }

       
        public  PdmGridMetaColumnClrDto ForeignPdmGridMetaColumnClrDto
        {
          get;
		  set;
        }

       
        public  PdmSearchTemplateClrDto ForeignPdmSearchTemplateClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

