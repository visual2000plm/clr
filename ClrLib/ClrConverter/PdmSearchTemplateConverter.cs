using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSearchTemplateConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmSearchTemplateClrDto ConvertDataRowDto(DataRow row)
        {
            PdmSearchTemplateClrDto aDto = new  PdmSearchTemplateClrDto();
			aDto.SearchTemplateId =(System.Int32)row["SearchTemplateID"];
			aDto.Name =(System.String)row["Name"];
			aDto.Description =row["Description"]as System.String	 ;
			aDto.Urllink =row["URLLink"]as System.String	 ;
			aDto.Type =(System.Int32)row["Type"];
			aDto.IsBuiltIn =row["IsBuiltIn"]  as System.Boolean ? ;
			aDto.OutputTabId =row["OutputTabID"]  as System.Int32 ? ;
			aDto.WhereUsedSearchTemplateResultId =row["WhereUsedSearchTemplateResultID"]  as System.Int32 ? ;
			aDto.ReferenceViewId =(System.Int32)row["ReferenceViewID"];
			aDto.IsDefault =(System.Boolean)row["IsDefault"];
			aDto.IsAutoExecute =(System.Boolean)row["IsAutoExecute"];
			aDto.BlqueryId =row["BLQueryID"]  as System.Int32 ? ;
			aDto.CatalogId =row["CatalogID"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
			aDto.DefaultMassUpdateViewId =row["DefaultMassUpdateViewID"]  as System.Int32 ? ;
			aDto.IsNoSecuirty =row["IsNoSecuirty"]  as System.Boolean ? ;
			aDto.TechPackTypeId =row["TechPackTypeID"]  as System.Int32 ? ;
            return aDto;
        }
      		
     
		        
    }
}

 