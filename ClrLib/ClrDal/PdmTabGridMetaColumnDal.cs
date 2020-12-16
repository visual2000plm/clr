using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTabGridMetaColumnDal 
    {

        public static string QueryAll = @" SELECT  TabGridMetalColumnID ,  TabID ,  GridColumnID ,  AliasName , ExternalMappingName,  BlockID ,  SystemTimeStamp ,  Visible   FROM [dbo].[pdmTabGridMetaColumn] ";
			
		public static List< PdmTabGridMetaColumnClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmTabGridMetaColumnClrDto> listDto = new List<PdmTabGridMetaColumnClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmTabGridMetaColumnConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }

        private partial class PdmTabGridMetaColumnConverter
        {


            //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
            public static PdmTabGridMetaColumnClrDto ConvertDataRowDto(DataRow row)
            {
                PdmTabGridMetaColumnClrDto aDto = new PdmTabGridMetaColumnClrDto();
                aDto.TabGridMetalColumnId = (System.Int32)row["TabGridMetalColumnID"];
                aDto.TabId = (System.Int32)row["TabID"];
                aDto.GridColumnId = (System.Int32)row["GridColumnID"];
                aDto.AliasName = row["AliasName"] as System.String;
                aDto.ExternalMappingName = row["ExternalMappingName"] as System.String;
                aDto.BlockId = row["BlockID"] as System.Int32?;
                aDto.SystemTimeStamp = (System.Byte[])row["SystemTimeStamp"];
                aDto.Visible = (System.Boolean)row["Visible"];
                return aDto;
            }



        }
		        
    }
}

 