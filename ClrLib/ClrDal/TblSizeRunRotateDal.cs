using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class TblSizeRunRotateDal 
    {
        					
        public static  string QueryAll = @" SELECT  SizeRunRotateID ,  SizeRunId ,  SizeOrder ,  SystemTimeStamp ,  SizeName ,  ERPID ,  IntegrationId   FROM [dbo].[tblSizeRunRotate] ";
			
		public static List< TblSizeRunRotateClrDto> GetAllList(SqlConnection conn)
        {
            List<TblSizeRunRotateClrDto> listDto = new List<TblSizeRunRotateClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(TblSizeRunRotateConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 