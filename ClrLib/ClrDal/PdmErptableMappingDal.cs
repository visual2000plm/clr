using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmErptableMappingDal 
    {
        					
        public static  string QueryAll = @" SELECT  MappingID ,  PLMTableName ,  PLMPrimaryKeyColumn ,  PLMLogicalUniqueColumn ,  EXTableName ,  EXPrimaryKeyColumn ,  EXLogicalUniqueColumn ,  EXStartRootPKIDUsedByPLM ,  EXChangeMode ,  LastReadPLMTableTimeStamp ,  LastReadExchangeTableTimeStamp ,  ExchangeTableTimeStampColumnName ,  PlmTableTimeStampColumnName ,  UserDefineEntityID ,  IsMappingToUserDefineEntity   FROM [dbo].[pdmERPTableMapping] ";
			
		public static List< PdmErptableMappingClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmErptableMappingClrDto> listDto = new List<PdmErptableMappingClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmErptableMappingConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 