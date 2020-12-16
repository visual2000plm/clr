using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class TblSizeRunConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static TblSizeRunClrDto ConvertDataRowDto(DataRow row)
        {
            TblSizeRunClrDto aDto = new  TblSizeRunClrDto();
			aDto.SizeRunId =(System.Int32)row["SizeRunId"];
			aDto.SizeRunCode =row["SizeRunCode"]as System.String	 ;
			aDto.Description =row["Description"]as System.String	 ;
			aDto.NbSize =row["NbSize"]  as System.Byte ? ;
			aDto.SizeDesc1 =row["SizeDesc1"]as System.String	 ;
			aDto.SizeDesc2 =row["SizeDesc2"]as System.String	 ;
			aDto.SizeDesc3 =row["SizeDesc3"]as System.String	 ;
			aDto.SizeDesc4 =row["SizeDesc4"]as System.String	 ;
			aDto.SizeDesc5 =row["SizeDesc5"]as System.String	 ;
			aDto.SizeDesc6 =row["SizeDesc6"]as System.String	 ;
			aDto.SizeDesc7 =row["SizeDesc7"]as System.String	 ;
			aDto.SizeDesc8 =row["SizeDesc8"]as System.String	 ;
			aDto.SizeDesc9 =row["SizeDesc9"]as System.String	 ;
			aDto.SizeDesc10 =row["SizeDesc10"]as System.String	 ;
			aDto.SizeDesc11 =row["SizeDesc11"]as System.String	 ;
			aDto.SizeDesc12 =row["SizeDesc12"]as System.String	 ;
			aDto.SizeDesc13 =row["SizeDesc13"]as System.String	 ;
			aDto.SizeDesc14 =row["SizeDesc14"]as System.String	 ;
			aDto.SizeDesc15 =row["SizeDesc15"]as System.String	 ;
			aDto.SizeDesc16 =row["SizeDesc16"]as System.String	 ;
			aDto.SizeDesc17 =row["SizeDesc17"]as System.String	 ;
			aDto.SizeDesc18 =row["SizeDesc18"]as System.String	 ;
			aDto.SizeDesc19 =row["SizeDesc19"]as System.String	 ;
			aDto.SizeDesc20 =row["SizeDesc20"]as System.String	 ;
			aDto.SystemTimeStamp =row["SystemTimeStamp"]as System.Byte[]	 ;
			aDto.PublishedMode =row["PublishedMode"]  as System.Int32 ? ;
			aDto.SizeNrfcode1 =row["SizeNRFCode1"]  as System.Int32 ? ;
			aDto.SizeNrfcode2 =row["SizeNRFCode2"]  as System.Int32 ? ;
			aDto.SizeNrfcode3 =row["SizeNRFCode3"]  as System.Int32 ? ;
			aDto.SizeNrfcode4 =row["SizeNRFCode4"]  as System.Int32 ? ;
			aDto.SizeNrfcode5 =row["SizeNRFCode5"]  as System.Int32 ? ;
			aDto.SizeNrfcode6 =row["SizeNRFCode6"]  as System.Int32 ? ;
			aDto.SizeNrfcode7 =row["SizeNRFCode7"]  as System.Int32 ? ;
			aDto.SizeNrfcode8 =row["SizeNRFCode8"]  as System.Int32 ? ;
			aDto.SizeNrfcode9 =row["SizeNRFCode9"]  as System.Int32 ? ;
			aDto.SizeNrfcode10 =row["SizeNRFCode10"]  as System.Int32 ? ;
			aDto.SizeNrfcode11 =row["SizeNRFCode11"]  as System.Int32 ? ;
			aDto.SizeNrfcode12 =row["SizeNRFCode12"]  as System.Int32 ? ;
			aDto.SizeNrfcode13 =row["SizeNRFCode13"]  as System.Int32 ? ;
			aDto.SizeNrfcode14 =row["SizeNRFCode14"]  as System.Int32 ? ;
			aDto.SizeNrfcode15 =row["SizeNRFCode15"]  as System.Int32 ? ;
			aDto.SizeNrfcode16 =row["SizeNRFCode16"]  as System.Int32 ? ;
			aDto.SizeNrfcode17 =row["SizeNRFCode17"]  as System.Int32 ? ;
			aDto.SizeNrfcode18 =row["SizeNRFCode18"]  as System.Int32 ? ;
			aDto.SizeNrfcode19 =row["SizeNRFCode19"]  as System.Int32 ? ;
			aDto.SizeNrfcode20 =row["SizeNRFCode20"]  as System.Int32 ? ;
			aDto.Divisible1 =row["Divisible1"]  as System.Int32 ? ;
			aDto.Divisible2 =row["Divisible2"]  as System.Int32 ? ;
			aDto.Divisible3 =row["Divisible3"]  as System.Int32 ? ;
			aDto.Divisible4 =row["Divisible4"]  as System.Int32 ? ;
			aDto.Divisible5 =row["Divisible5"]  as System.Int32 ? ;
			aDto.Divisible6 =row["Divisible6"]  as System.Int32 ? ;
			aDto.Divisible7 =row["Divisible7"]  as System.Int32 ? ;
			aDto.Divisible8 =row["Divisible8"]  as System.Int32 ? ;
			aDto.Divisible9 =row["Divisible9"]  as System.Int32 ? ;
			aDto.Divisible10 =row["Divisible10"]  as System.Int32 ? ;
			aDto.Divisible11 =row["Divisible11"]  as System.Int32 ? ;
			aDto.Divisible12 =row["Divisible12"]  as System.Int32 ? ;
			aDto.Divisible13 =row["Divisible13"]  as System.Int32 ? ;
			aDto.Divisible14 =row["Divisible14"]  as System.Int32 ? ;
			aDto.Divisible15 =row["Divisible15"]  as System.Int32 ? ;
			aDto.Divisible16 =row["Divisible16"]  as System.Int32 ? ;
			aDto.Divisible17 =row["Divisible17"]  as System.Int32 ? ;
			aDto.Divisible18 =row["Divisible18"]  as System.Int32 ? ;
			aDto.Divisible19 =row["Divisible19"]  as System.Int32 ? ;
			aDto.Divisible20 =row["Divisible20"]  as System.Int32 ? ;
			aDto.Erpid =row["ERPID"]as System.String	 ;
			aDto.IntegrationId =row["IntegrationId"]as System.String	 ;
            return aDto;
        }
      		
     
		        
    }
}

 