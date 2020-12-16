using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityRegDomainListMenuConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmSecurityRegDomainListMenuClrDto ConvertDataRowDto(DataRow row)
        {
            PdmSecurityRegDomainListMenuClrDto aDto = new  PdmSecurityRegDomainListMenuClrDto();
			aDto.DomainMenuId =(System.Int32)row["DomainMenuID"];
			aDto.MenuId =(System.Int32)row["MenuID"];
			aDto.DomainId =(System.Int32)row["DomainID"];
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 