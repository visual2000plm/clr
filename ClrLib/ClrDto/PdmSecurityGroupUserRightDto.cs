using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityGroupUserRightClrDto 
    {
        public PdmSecurityGroupUserRightClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 SecurityRightId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> GroupId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> UserId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> BlockId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> RefTxType
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> TabId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> TemplateId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsInVisible
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsUnSaveAble
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsSpecialPermission
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> GridId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> GridMetaColumnId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> SubItemId
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
 

		
		

       
        public  PdmGridClrDto ForeignPdmGridClrDto
        {
          get;
		  set;
        }

       
        public  PdmGridMetaColumnClrDto ForeignPdmGridMetaColumnClrDto
        {
          get;
		  set;
        }

       
        public  PdmSecurityUserGroupClrDto ForeignPdmSecurityUserGroupClrDto
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

