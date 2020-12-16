using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class TblSizeRunDal 
    {
        					
        public static  string QueryAll = @" SELECT  SizeRunId ,  SizeRunCode ,  Description ,  NbSize ,  SizeDesc1 ,  SizeDesc2 ,  SizeDesc3 ,  SizeDesc4 ,  SizeDesc5 ,  SizeDesc6 ,  SizeDesc7 ,  SizeDesc8 ,  SizeDesc9 ,  SizeDesc10 ,  SizeDesc11 ,  SizeDesc12 ,  SizeDesc13 ,  SizeDesc14 ,  SizeDesc15 ,  SizeDesc16 ,  SizeDesc17 ,  SizeDesc18 ,  SizeDesc19 ,  SizeDesc20 ,  SystemTimeStamp ,  PublishedMode ,  SizeNRFCode1 ,  SizeNRFCode2 ,  SizeNRFCode3 ,  SizeNRFCode4 ,  SizeNRFCode5 ,  SizeNRFCode6 ,  SizeNRFCode7 ,  SizeNRFCode8 ,  SizeNRFCode9 ,  SizeNRFCode10 ,  SizeNRFCode11 ,  SizeNRFCode12 ,  SizeNRFCode13 ,  SizeNRFCode14 ,  SizeNRFCode15 ,  SizeNRFCode16 ,  SizeNRFCode17 ,  SizeNRFCode18 ,  SizeNRFCode19 ,  SizeNRFCode20 ,  Divisible1 ,  Divisible2 ,  Divisible3 ,  Divisible4 ,  Divisible5 ,  Divisible6 ,  Divisible7 ,  Divisible8 ,  Divisible9 ,  Divisible10 ,  Divisible11 ,  Divisible12 ,  Divisible13 ,  Divisible14 ,  Divisible15 ,  Divisible16 ,  Divisible17 ,  Divisible18 ,  Divisible19 ,  Divisible20 ,  ERPID ,  IntegrationId   FROM [dbo].[tblSizeRun] ";
			
		public static List< TblSizeRunClrDto> GetAllList(SqlConnection conn)
        {
            List<TblSizeRunClrDto> listDto = new List<TblSizeRunClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(TblSizeRunConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 