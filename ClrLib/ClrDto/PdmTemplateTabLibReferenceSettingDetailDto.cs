using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmTemplateTabLibReferenceSettingDetailClrDto 
    {
        public PdmTemplateTabLibReferenceSettingDetailClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 DetailId
        {
            get ;
            set ;
        }

     
        public  System.Int32 TemplateTabLibRefId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> FiledSubitmeId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> MappingGridColumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> MappingType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> UserDefineEntityColumnId
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
 

		
		

       
        public  PdmTemplateTabLibReferenceSettingClrDto ForeignPdmTemplateTabLibReferenceSettingClrDto
        {
          get;
		  set;
        }

       
        public  PdmUserDefineEntityColumnClrDto ForeignPdmUserDefineEntityColumnClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

