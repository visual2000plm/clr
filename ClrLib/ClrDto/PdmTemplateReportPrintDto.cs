using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmTemplateReportPrintClrDto 
    {
        public PdmTemplateReportPrintClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 ReportPrintId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> TemplateId
        {
            get ;
            set ;
        }

     
        public  System.String ReportFileName
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ReportEngineType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsDefault
        {
            get ;
            set ;
        }

     
        public  System.String PrintName
        {
            get ;
            set ;
        }

     
        public  System.String Description
        {
            get ;
            set ;
        }

     
        public  System.Int32 ReportId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ReferenceViewId
        {
            get ;
            set ;
        }
        
        #endregion
 

		
		

       
        public  PdmReferenceViewClrDto ForeignPdmReferenceViewClrDto
        {
          get;
		  set;
        }

       
        public  PdmTemplateClrDto ForeignPdmTemplateClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

