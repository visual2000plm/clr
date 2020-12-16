using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmMassUpdateViewConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmMassUpdateViewClrDto ConvertDataRowDto(DataRow row)
        {
            PdmMassUpdateViewClrDto aDto = new  PdmMassUpdateViewClrDto();
			aDto.MassUpdateViewId =(System.Int32)row["MassUpdateViewID"];
			aDto.Name =row["Name"]as System.String	 ;
			aDto.Description =row["Description"]as System.String	 ;
			aDto.MainTabId =row["MainTabID"]  as System.Int32 ? ;
			aDto.GridBlockId =row["GridBlockID"]  as System.Int32 ? ;
			aDto.GridId =row["GridID"]  as System.Int32 ? ;
			aDto.UpdateType =row["UpdateType"]  as System.Int32 ? ;
			aDto.IsUsedForSearch =row["IsUsedForSearch"]  as System.Boolean ? ;
			aDto.IsActive =row["IsActive"]  as System.Boolean ? ;
			aDto.FreezeNumber =row["FreezeNumber"]  as System.Int32 ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
			aDto.TechPackTypeId =row["TechPackTypeID"]  as System.Int32 ? ;
            return aDto;
        }
      		
     
		        
    }
}

 