using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class TblDimensionDal 
    {
        					
        public static  string QueryAll = @" SELECT  DimensionID ,  DimensionCode ,  DimensionDesc ,  PublishedMode ,  SystemTimeStamp ,  ERPID ,  IntegrationId   FROM [dbo].[tblDimension] ";
			
		public static List< TblDimensionClrDto> GetAllList(SqlConnection conn)
        {
            List<TblDimensionClrDto> listDto = new List<TblDimensionClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(TblDimensionConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 