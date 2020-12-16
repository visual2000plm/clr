using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmUserDefineEntityColumnDal 
    {
        					
        public static  string QueryAll = @" SELECT  UserDefineEntityColumnID ,  EntityID ,  ColumnName ,  DataType ,  UsedByDropDownList ,  DataRowSort ,  MappingValueKey ,  IsPrimaryKey ,  IsIdentity ,  IsExtendColumn ,  SystemTableColumnName ,  FKEntityID ,  UIControlType ,  NBDecimal ,  SystemTimeStamp   FROM [dbo].[pdmUserDefineEntityColumn] ";
			
		public static List< PdmUserDefineEntityColumnClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmUserDefineEntityColumnClrDto> listDto = new List<PdmUserDefineEntityColumnClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmUserDefineEntityColumnConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 