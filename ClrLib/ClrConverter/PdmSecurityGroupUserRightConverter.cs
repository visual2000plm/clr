using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityGroupUserRightConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmSecurityGroupUserRightClrDto ConvertDataRowDto(DataRow row)
        {
            PdmSecurityGroupUserRightClrDto aDto = new  PdmSecurityGroupUserRightClrDto();
			aDto.SecurityRightId =(System.Int32)row["SecurityRightID"];
			aDto.GroupId =row["GroupID"]  as System.Int32 ? ;
			aDto.UserId =row["UserID"]  as System.Int32 ? ;
			aDto.BlockId =row["BlockID"]  as System.Int32 ? ;
			aDto.RefTxType =row["RefTxType"]  as System.Int32 ? ;
			aDto.TabId =row["TabID"]  as System.Int32 ? ;
			aDto.TemplateId =row["TemplateID"]  as System.Int32 ? ;
			aDto.IsInVisible =row["IsInVisible"]  as System.Boolean ? ;
			aDto.IsUnSaveAble =row["IsUnSaveAble"]  as System.Boolean ? ;
			aDto.IsSpecialPermission =row["IsSpecialPermission"]  as System.Boolean ? ;
			aDto.GridId =row["GridID"]  as System.Int32 ? ;
			aDto.GridMetaColumnId =row["GridMetaColumnID"]  as System.Int32 ? ;
			aDto.SubItemId =row["SubItemID"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 