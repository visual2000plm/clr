using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmEntityBlClrDto 
    {
        public PdmEntityBlClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public string ConnectInfo
        {
            get;
            set;
        }

        public string LastScanCheckSum
        {
            get;
            set;
        }

        public  System.Int32 EntityId
        {
            get ;
            set ;
        }

     
        public  System.String EntityCode
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> EntityGroupId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsBuiltInTable
        {
            get ;
            set ;
        }

     
        public  System.String ManageUrl
        {
            get ;
            set ;
        }

     
        public  System.String Description
        {
            get ;
            set ;
        }

     
        public  System.String ServiceHeaderDict
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsSimpleColumn
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsImport
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> EntityType
        {
            get ;
            set ;
        }

     
        public  System.String SysTableName
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsRelationEntity
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> MasterEntityId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ChildEntityId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> FolderId
        {
            get ;
            set ;
        }

     
        public  System.String Uilayout
        {
            get ;
            set ;
        }

     
        public  System.Byte[] SystemTimeStamp
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> EntityWithFkentityId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> MasterEntityColumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ChildEntityColumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> DataSourceFrom
        {
            get ;
            set ;
        }

     
        public  System.String RequestMethod
        {
            get ;
            set ;
        }
        
        #endregion
 
       
       public  List<PdmBlockFormulaClrDto> PdmBlockFormulaList
        {
          get;
		  set;
        }

       
       public  List<PdmBlockSubItemClrDto> PdmBlockSubItemList
        {
          get;
		  set;
        }

       
       public  List<PdmBlockSubItemClrDto> PdmBlockSubItem_List
        {
          get;
		  set;
        }

       
       public  List<PdmEntityBlClrDto> PdmEntityBl__List
        {
          get;
		  set;
        }

       
       public  List<PdmEntityEnumValueClrDto> PdmEntityEnumValueList
        {
          get;
		  set;
        }

       
       public  List<PdmEntityMasterChildValueClrDto> PdmEntityMasterChildValueList
        {
          get;
		  set;
        }

       
       public  List<PdmGridMetaColumnClrDto> PdmGridMetaColumnList
        {
          get;
		  set;
        }

       
       public  List<PdmTemplateTabLibReferenceSettingClrDto> PdmTemplateTabLibReferenceSettingList
        {
          get;
		  set;
        }

       
       public  List<PdmUserDefineEntityColumnClrDto> PdmUserDefineEntityColumnList
        {
          get;
		  set;
        }

       
       public  List<PdmUserDefineEntityRowClrDto> PdmUserDefineEntityRowList
        {
          get;
		  set;
        }

		
		

       
        public  PdmEntityBlClrDto ForeignMasterPdmEntityClrDto
        {
          get;
		  set;
        }

       
        public  PdmEntityBlClrDto ForeignChildPdmEntityClrDto
        {
          get;
		  set;
        }

       
        public  PdmEntityBlClrDto ForeignPdmEntityBlClrDto
        {
          get;
		  set;
        }

       
        public  PdmUserDefineEntityColumnClrDto ForeignPdmUserDefineEntityColumn__ClrDto
        {
          get;
		  set;
        }

       
        public  PdmUserDefineEntityColumnClrDto ForeignPdmUserDefineEntityColumn_ClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

