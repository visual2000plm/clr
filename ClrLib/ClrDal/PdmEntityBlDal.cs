using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmEntityBlDal 
    {
        					
        public static  string QueryAll = @" SELECT  EntityID ,  EntityCode ,  EntityGroupID ,  IsBuiltInTable ,  ServiceUrl ,  Description ,  ServiceHeaderDict ,  IsSimpleColumn ,  IsImport ,  EntityType ,  SysTableName ,  IsRelationEntity ,  MasterEntityID ,  ChildEntityID ,  FolderID ,  UILayout ,  SystemTimeStamp ,  EntityWithFKEntityID ,  MasterEntityColumnID ,  ChildEntityColumnID ,  DataSourceFrom ,  RequestMethod   FROM [dbo].[pdmEntity] ";
			
		public static List< PdmEntityBlClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmEntityBlClrDto> listDto = new List<PdmEntityBlClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmEntityBlConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 