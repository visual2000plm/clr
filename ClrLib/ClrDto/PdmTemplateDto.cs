using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmTemplateClrDto 
    {
        public PdmTemplateClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 TemplateId
        {
            get ;
            set ;
        }

     
        public  System.String TemplateName
        {
            get ;
            set ;
        }

     
        public  System.String Description
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> GroupId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Byte> IsRef
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ParentId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> CreatedBy
        {
            get ;
            set ;
        }

     
        public  Nullable<System.DateTime> CreatedDate
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ModifyBy
        {
            get ;
            set ;
        }

     
        public  Nullable<System.DateTime> ModifyDate
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Byte> ManagementLevel
        {
            get ;
            set ;
        }

     
        public  System.String InternalCode
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> RefTxType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> FolderId
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
 
       
       public  List<PdmTemplateReportPrintClrDto> PdmTemplateReportPrintList
        {
          get;
		  set;
        }

       
       public  List<PdmTemplateTabClrDto> PdmTemplateTabList
        {
          get;
		  set;
        }

       
       public  List<PdmTemplateTabLibReferenceSettingClrDto> PdmTemplateTabLibReferenceSettingList
        {
          get;
		  set;
        }

		
		
	
		
		
		
		
	
		        
    }
}

