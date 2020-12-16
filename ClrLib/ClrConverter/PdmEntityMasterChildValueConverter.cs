using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmEntityMasterChildValueConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmEntityMasterChildValueClrDto ConvertDataRowDto(DataRow row)
        {
            PdmEntityMasterChildValueClrDto aDto = new  PdmEntityMasterChildValueClrDto();
			aDto.MasterChildValueId =(System.Int32)row["MasterChildValueID"];
			aDto.RelationEntityId =(System.Int32)row["RelationEntityID"];
			aDto.MasterValueId =(System.Int32)row["MasterValueID"];
			aDto.ChildValueId =(System.Int32)row["ChildValueID"];
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 