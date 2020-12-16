using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmUserDefineEntityColumnClrDto 
    {
        public PdmUserDefineEntityColumnClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 UserDefineEntityColumnId
        {
            get ;
            set ;
        }

     
        public  System.Int32 EntityId
        {
            get ;
            set ;
        }

     
        public  System.String ColumnName
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DataType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> UsedByDropDownList
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DataRowSort
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> MappingValueKey
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsPrimaryKey
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsIdentity
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsExtendColumn
        {
            get ;
            set ;
        }

     
        public  System.String SystemTableColumnName
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> FkentityId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> UicontrolType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> Nbdecimal
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
 
       
       public  List<PdmEntityBlClrDto> PdmEntityBl_List
        {
          get;
		  set;
        }

       
       public  List<PdmEntityBlClrDto> PdmEntityBlList
        {
          get;
		  set;
        }

       
       public  List<PdmTemplateTabLibReferenceSettingDetailClrDto> PdmTemplateTabLibReferenceSettingDetailList
        {
          get;
		  set;
        }

		
		

       
        public  PdmEntityBlClrDto ForeignFkpdmEntityBlClrDto
        {
          get;
		  set;
        }

       
        public  PdmEntityBlClrDto ForeignPdmEntityClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

