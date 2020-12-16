using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmEntityMasterChildValueDal 
    {
        					
        public static  string QueryAll = @" SELECT  MasterChildValueID ,  RelationEntityID ,  MasterValueID ,  ChildValueID ,  SystemTimeStamp   FROM [dbo].[pdmEntityMasterChildValue] ";
			
		public static List< PdmEntityMasterChildValueClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmEntityMasterChildValueClrDto> listDto = new List<PdmEntityMasterChildValueClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmEntityMasterChildValueConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 