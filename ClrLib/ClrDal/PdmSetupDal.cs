using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSetupDal 
    {
        					
        public static  string QueryAll = @" SELECT  SetupID ,  setupCode ,  SetupValue ,  Description ,  EntityID ,  UsageType ,  SystemTimeStamp   FROM [dbo].[pdmSetup] ";
			
		public static List< PdmSetupClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmSetupClrDto> listDto = new List<PdmSetupClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmSetupConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 