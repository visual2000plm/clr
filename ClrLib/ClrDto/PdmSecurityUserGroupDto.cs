using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityUserGroupClrDto 
    {
        public PdmSecurityUserGroupClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 GroupId
        {
            get ;
            set ;
        }

     
        public  System.String GroupName
        {
            get ;
            set ;
        }

     
        public  System.String Description
        {
            get ;
            set ;
        }

     
        public  System.String LoginEvent
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ParentId
        {
            get ;
            set ;
        }

     
        public  System.String InternalCode
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsBuiltIn
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
 
       
       public  List<PdmMassUpdateViewMemberClrDto> PdmMassUpdateViewMemberList
        {
          get;
		  set;
        }

       
       public  List<PdmReportPublishSecurityClrDto> PdmReportPublishSecurityList
        {
          get;
		  set;
        }

       
       public  List<PdmSecurityDivUserGroupMemberClrDto> PdmSecurityDivUserGroupMemberList
        {
          get;
		  set;
        }

       
       public  List<PdmSecurityGroupMemberClrDto> PdmSecurityGroupMemberList
        {
          get;
		  set;
        }

       
       public  List<PdmSecurityGroupUserRightClrDto> PdmSecurityGroupUserRightList
        {
          get;
		  set;
        }

       
       public  List<PdmSecurityUserGroupClrDto> PdmSecurityUserGroup_List
        {
          get;
		  set;
        }

       
       public  List<PdmSecurityUserGroupPermissionClrDto> PdmSecurityUserGroupPermissionList
        {
          get;
		  set;
        }

		
		

       
        public  PdmSecurityUserGroupClrDto ForeignPdmSecurityUserGroupClrDto
        {
          get;
		  set;
        }	
		
		
		
		
	
		        
    }
}

