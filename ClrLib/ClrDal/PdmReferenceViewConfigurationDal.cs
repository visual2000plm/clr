using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmReferenceViewConfigurationDal 
    {
        					
        public static  string QueryAll = @" SELECT  ReferenceViewConfigurationID ,  ReferenceTypeID ,  ConfigurationTypeID ,  SecurityWebUserID ,  SecurityUserGroupID ,  ReferenceViewID ,  SearchTemplateID ,  SystemTimeStamp   FROM [dbo].[pdmReferenceViewConfiguration] ";
			
		public static List< PdmReferenceViewConfigurationClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmReferenceViewConfigurationClrDto> listDto = new List<PdmReferenceViewConfigurationClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmReferenceViewConfigurationConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 