using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmErptableMappingClrDto 
    {
        public PdmErptableMappingClrDto()
        {        
        }
	 
        #region  Entity Dto Properties 
 
     
        public  System.Int32 MappingId
        {
            get ;
            set ;
        }

     
        public  System.String PlmtableName
        {
            get ;
            set ;
        }

     
        public  System.String PlmprimaryKeyColumn
        {
            get ;
            set ;
        }

     
        public  System.String PlmlogicalUniqueColumn
        {
            get ;
            set ;
        }

     
        public  System.String ErptableName
        {
            get ;
            set ;
        }

     
        public  System.String ErpprimaryKeyColumn
        {
            get ;
            set ;
        }

     
        public  System.String ErplogicalUniqueColumn
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ExstartRootPkidusedByPlm
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> ExchangeMode
        {
            get ;
            set ;
        }

     
        public  System.Byte[] PlmlastTimeStamp
        {
            get ;
            set ;
        }

     
        public  System.Byte[] ErplastTimeStamp
        {
            get ;
            set ;
        }

     
        public  System.String ExchangeTableTimeStampColumnName
        {
            get ;
            set ;
        }

     
        public  System.String PlmTableTimeStampColumnName
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Int32> UserDefineEntityId
        {
            get ;
            set ;
        }

     
        public  Nullable<System.Boolean> IsMappingToUserDefineEntity
        {
            get ;
            set ;
        }
        
        #endregion
 
       
       public  List<PdmErptableColumnMappingClrDto> PdmErptableColumnMapping_List
        {
          get;
		  set;
        }

       
       public  List<PdmErptableColumnMappingClrDto> PdmErptableColumnMappingList
        {
          get;
		  set;
        }

		
		
	
		
		
		
		
	
		        
    }
}

