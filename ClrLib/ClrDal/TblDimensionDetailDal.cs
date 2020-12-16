using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class TblDimensionDetailDal 
    {
        					
        public static  string QueryAll = @" SELECT  DimensionDetailID ,  DimensionID ,  DimDetailCode ,  DimDetailDesc ,  DimDetailSort ,  SystemTimeStamp ,  SystemSort ,  ERPID ,  IntegrationId   FROM [dbo].[tblDimensionDetail] ";
			
		public static List< TblDimensionDetailClrDto> GetAllList(SqlConnection conn)
        {
            List<TblDimensionDetailClrDto> listDto = new List<TblDimensionDetailClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(TblDimensionDetailConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 