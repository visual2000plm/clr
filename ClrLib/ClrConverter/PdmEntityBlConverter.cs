using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmEntityBlConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmEntityBlClrDto ConvertDataRowDto(DataRow row)
        {
            PdmEntityBlClrDto aDto = new  PdmEntityBlClrDto();
			aDto.EntityId =(System.Int32)row["EntityID"];
			aDto.EntityCode =(System.String)row["EntityCode"];
			aDto.EntityGroupId =row["EntityGroupID"]  as System.Int32 ? ;
			aDto.IsBuiltInTable =row["IsBuiltInTable"]  as System.Boolean ? ;
			aDto.ManageUrl =row["ServiceUrl"]as System.String	 ;
			aDto.Description =row["Description"]as System.String	 ;
			aDto.ServiceHeaderDict =row["ServiceHeaderDict"]as System.String	 ;
			aDto.IsSimpleColumn =row["IsSimpleColumn"]  as System.Boolean ? ;
			aDto.IsImport =row["IsImport"]  as System.Boolean ? ;
			aDto.EntityType =row["EntityType"]  as System.Int32 ? ;
			aDto.SysTableName =row["SysTableName"]as System.String	 ;
			aDto.IsRelationEntity =row["IsRelationEntity"]  as System.Boolean ? ;
			aDto.MasterEntityId =row["MasterEntityID"]  as System.Int32 ? ;
			aDto.ChildEntityId =row["ChildEntityID"]  as System.Int32 ? ;
			aDto.FolderId =row["FolderID"]  as System.Int32 ? ;
			aDto.Uilayout =row["UILayout"]as System.String	 ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
			aDto.EntityWithFkentityId =row["EntityWithFKEntityID"]  as System.Int32 ? ;
			aDto.MasterEntityColumnId =row["MasterEntityColumnID"]  as System.Int32 ? ;
			aDto.ChildEntityColumnId =row["ChildEntityColumnID"]  as System.Int32 ? ;
			aDto.DataSourceFrom =row["DataSourceFrom"]  as System.Int32 ? ;
			aDto.RequestMethod =row["RequestMethod"]as System.String	 ;
            return aDto;
        }
      		
     
		        
    }
}

 