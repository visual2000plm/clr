using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmReferenceViewClrDto 
    {
        public PdmReferenceViewClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 ReferenceViewId
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

     
        public  System.Int32 TypeId
        {
            get ;
            set ;
        }

     
        public  System.Boolean NoSecurity
        {
            get ;
            set ;
        }

     
        public  System.Int32 GridOutputMode
        {
            get ;
            set ;
        }

     
        public  System.Int32 Options
        {
            get ;
            set ;
        }

     
        public  System.Int32 ViewType
        {
            get ;
            set ;
        }

     
        public  System.Int32 ColumnCount
        {
            get ;
            set ;
        }

     
        public  System.Int32 RowPerPage
        {
            get ;
            set ;
        }

     
        public  System.Boolean IsFilterByFolderSecurity
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

     
        public  Nullable<System.Int32> LineSheetTemplateId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsFilterByCurrentDiv
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsFilterByCurrentUser
        {
            get ;
            set ;
        }

     
        public  System.Byte[] SystemTimeStamp
        {
            get ;
            set ;
        }

     
        public  System.Byte[] Uilayout
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ItemWidth
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ItemHight
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

     
        public  Nullable<System.Boolean> IsUpdatable
        {
            get ;
            set ;
        }
        
        #endregion
 
       
       public  List<PdmReferenceViewColumnClrDto> PdmReferenceViewColumnList
        {
          get;
		  set;
        }

       
       public  List<PdmReferenceViewConfigurationClrDto> PdmReferenceViewConfigurationList
        {
          get;
		  set;
        }

       
       public  List<PdmReferenceViewMemberClrDto> PdmReferenceViewMemberList
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

       
       public  List<PdmTemplateReportPrintClrDto> PdmTemplateReportPrintList
        {
          get;
		  set;
        }

		
		

       
        public  PdmBlqueryClrDto ForeignPdmBlqueryClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

