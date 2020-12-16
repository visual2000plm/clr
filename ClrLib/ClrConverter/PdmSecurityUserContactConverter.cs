using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSecurityUserContactConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmSecurityUserContactClrDto ConvertDataRowDto(DataRow row)
        {
            PdmSecurityUserContactClrDto aDto = new  PdmSecurityUserContactClrDto();
			aDto.ContactId =(System.Int32)row["ContactID"];
			aDto.UserId =row["UserID"]  as System.Int32 ? ;
			aDto.ContactType =row["ContactType"]  as System.Int32 ? ;
			aDto.ContactFormat =row["ContactFormat"]as System.String	 ;
			aDto.AdditionalContactInfo =row["AdditionalContactInfo"]as System.String	 ;
			aDto.Comments =row["Comments"]as System.String	 ;
            return aDto;
        }
      		
     
		        
    }
}

 