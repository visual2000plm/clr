using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmSearchTemplateClrDto 
    {
        public PdmSearchTemplateClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 SearchTemplateId
        {
            get ;
            set ;
        }

     
        public  System.String Name
        {
            get ;
            set ;
        }

     
        public  System.String Description
        {
            get ;
            set ;
        }

     
        public  System.String Urllink
        {
            get ;
            set ;
        }

     
        public  System.Int32 Type
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsBuiltIn
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> OutputTabId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> WhereUsedSearchTemplateResultId
        {
            get ;
            set ;
        }

     
        public  System.Int32 ReferenceViewId
        {
            get ;
            set ;
        }

     
        public  System.Boolean IsDefault
        {
            get ;
            set ;
        }

     
        public  System.Boolean IsAutoExecute
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> BlqueryId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> CatalogId
        {
            get ;
            set ;
        }

     
        public  System.Byte[] SystemTimeStamp
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DefaultMassUpdateViewId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsNoSecuirty
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> TechPackTypeId
        {
            get ;
            set ;
        }
        
        #endregion
 
       
       public  List<PdmReferenceViewConfigurationClrDto> PdmReferenceViewConfigurationList
        {
          get;
		  set;
        }

       
       public  List<PdmSearchTemplateDcuClrDto> PdmSearchTemplateDcuList
        {
          get;
		  set;
        }

       
       public  List<PdmSearchTemplateReferenceViewClrDto> PdmSearchTemplateReferenceViewList
        {
          get;
		  set;
        }

		
		

       
        public  PdmBlqueryClrDto ForeignPdmBlqueryClrDto
        {
          get;
		  set;
        }

       
        public  PdmMassUpdateViewClrDto ForeignPdmMassUpdateViewClrDto
        {
          get;
		  set;
        }

       
        public  PdmReferenceViewClrDto ForeignPdmReferenceViewClrDto
        {
          get;
		  set;
        }

       
        public  PdmTabClrDto ForeignPdmTabClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

