using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmReferenceViewConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmReferenceViewClrDto ConvertDataRowDto(DataRow row)
        {
            PdmReferenceViewClrDto aDto = new  PdmReferenceViewClrDto();
			aDto.ReferenceViewId =(System.Int32)row["ReferenceViewID"];
			aDto.Name =(System.String)row["Name"];
			aDto.Description =row["Description"]as System.String	 ;
			aDto.TypeId =(System.Int32)row["TypeID"];
			aDto.NoSecurity =(System.Boolean)row["NoSecurity"];
			aDto.GridOutputMode =(System.Int32)row["GridOutputMode"];
			aDto.Options =(System.Int32)row["Options"];
			aDto.ViewType =(System.Int32)row["ViewType"];
			aDto.ColumnCount =(System.Int32)row["ColumnCount"];
			aDto.RowPerPage =(System.Int32)row["RowPerPage"];
			aDto.IsFilterByFolderSecurity =(System.Boolean)row["IsFilterByFolderSecurity"];
			aDto.MainTabId =row["MainTabID"]  as System.Int32 ? ;
			aDto.GridBlockId =row["GridBlockID"]  as System.Int32 ? ;
			aDto.GridId =row["GridID"]  as System.Int32 ? ;
			aDto.UpdateType =row["UpdateType"]  as System.Int32 ? ;
			aDto.LineSheetTemplateId =row["LineSheetTemplateID"]  as System.Int32 ? ;
			aDto.IsFilterByCurrentDiv =row["IsFilterByCurrentDiv"]  as System.Boolean ? ;
			aDto.IsFilterByCurrentUser =row["IsFilterByCurrentUser"]  as System.Boolean ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
			aDto.Uilayout =row["UILayout"]as System.Byte[]	 ;
			aDto.ItemWidth =row["ItemWidth"]  as System.Int32 ? ;
			aDto.ItemHight =row["ItemHight"]  as System.Int32 ? ;
			aDto.BlqueryId =row["BLQueryID"]  as System.Int32 ? ;
			aDto.CatalogId =row["CatalogID"]  as System.Int32 ? ;
			aDto.IsUpdatable =row["IsUpdatable"]  as System.Boolean ? ;
            return aDto;
        }
      		
     
		        
    }
}

 