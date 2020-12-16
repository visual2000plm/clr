using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmTemplateTabLibReferenceSettingClrDto 
    {
        public PdmTemplateTabLibReferenceSettingClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 TemplateTabLibRefId
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

     
        public  Nullable<System.Int32> TemplateId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> TabId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> LibReferenceId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> GridBlockId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> Type
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> EntityId
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
 
       
       public  List<PdmTemplateTabLibReferenceSettingDetailClrDto> PdmTemplateTabLibReferenceSettingDetailList
        {
          get;
		  set;
        }

		
		

       
        public  PdmBlockClrDto ForeignPdmBlockClrDto
        {
          get;
		  set;
        }

       
        public  PdmEntityBlClrDto ForeignPdmEntityBlClrDto
        {
          get;
		  set;
        }

       
        public  PdmTabClrDto ForeignPdmTabClrDto
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

