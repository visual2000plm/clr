using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityRegDomainConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmSecurityRegDomainClrDto ConvertDataRowDto(DataRow row)
        {
            PdmSecurityRegDomainClrDto aDto = new  PdmSecurityRegDomainClrDto();
			aDto.DomainId =(System.Int32)row["DomainID"];
			aDto.DomainCode =(System.String)row["DomainCode"];
			aDto.PersonType =row["PersonType"]  as System.Int32 ? ;
			aDto.Description =row["Description"]as System.String	 ;
			aDto.DefaultPage =row["DefaultPage"]as System.String	 ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 