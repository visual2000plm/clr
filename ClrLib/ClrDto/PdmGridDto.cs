using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmGridClrDto 
    {
        public PdmGridClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 GridId
        {
            get ;
            set ;
        }

     
        public  System.String GridName
        {
            get ;
            set ;
        }

     
        public  System.String InternalCode
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> RowHight
        {
            get ;
            set ;
        }

     
        public  System.String Description
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsFixedTableLayout
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> NewRowViewPosition
        {
            get ;
            set ;
        }

     
        public  System.Int32 GridType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ShareSimpleDcuid
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ShareTxRefType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SubscribeSimpleDcuid
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> FolderId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SearchTemplateId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsNeedDefualtRow
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsAllowToDeleteRow
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsAllowToEditRow
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsAllowToAddNewRow
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsAllowEmptyRow
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ConceptualTemplateId
        {
            get ;
            set ;
        }

     
        public  System.Byte[] SystemTimeStamp
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> LinePlanningBlockId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> CreatedById
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ModifiedBy
        {
            get ;
            set ;
        }

     
        public  Nullable<System.DateTime> ModifiedDate
        {
            get ;
            set ;
        }

     
        public  Nullable<System.DateTime> CreatedDate
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

       
       public  List<PdmDwrequireTabAndGridClrDto> PdmDwrequireTabAndGridList
        {
          get;
		  set;
        }

       
       public  List<PdmGridMetaColumnClrDto> PdmGridMetaColumnList
        {
          get;
		  set;
        }

       
       public  List<PdmMassUpdateViewClrDto> PdmMassUpdateViewList
        {
          get;
		  set;
        }

		
		

       
        public  PdmBlockClrDto ForeignPdmBlockClrDto
        {
          get;
		  set;
        }

       
        public  PdmBlockSubItemClrDto ForeignShareSimpleDcuidPdmBlockSubItemClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

