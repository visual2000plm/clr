using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmMassUpdateViewClrDto 
    {
        public PdmMassUpdateViewClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 MassUpdateViewId
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

     
        public  Nullable<System.Int32> MainTabId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> GridBlockId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> GridId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> UpdateType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsUsedForSearch
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsActive
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> FreezeNumber
        {
            get ;
            set ;
        }

     
        public  System.Byte[] SystemTimeStamp
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
 
       
       public  List<PdmMassUpdateViewFieldClrDto> PdmMassUpdateViewFieldList
        {
          get;
		  set;
        }

       
       public  List<PdmMassUpdateViewMemberClrDto> PdmMassUpdateViewMemberList
        {
          get;
		  set;
        }

       
       public  List<PdmSearchTemplateClrDto> PdmSearchTemplateList
        {
          get;
		  set;
        }

       
       public  List<PdmSearchTemplateReferenceViewClrDto> PdmSearchTemplateReferenceViewList
        {
          get;
		  set;
        }

		
		

       
        public  PdmBlockClrDto ForeignPdmBlockClrDto
        {
          get;
		  set;
        }

       
        public  PdmGridClrDto ForeignPdmGridClrDto
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

