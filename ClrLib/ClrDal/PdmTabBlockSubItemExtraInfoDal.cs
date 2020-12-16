using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTabBlockSubItemExtraInfoDal 
    {

        public static readonly string QueryAll = @" SELECT  BlockSubItemExtraInfoID ,  SubItemID ,  TabID ,  AliasName ,  ExternalMappingName, Width ,  Height ,  Visible   FROM [dbo].[pdmTabBlockSubItemExtraInfo] ";

        public static readonly Dictionary<int, Dictionary<int, string>> DictTabSubitemExternapMappingName = null;


        static PdmTabBlockSubItemExtraInfoDal()
        {

            using (SqlConnection conn = new SqlConnection(PLMConstantString.PLM_APP_ConnectionString))
            {
                conn.Open();

               List< PdmTabBlockSubItemExtraInfoClrDto> result   = GetAllList( conn);

               DictTabSubitemExternapMappingName = result.GroupBy (o=>o.TabId).ToDictionary (o=>o.Key,o=>o.ToDictionary (p=>p.SubItemId ,p=>p.ExternalMappingName));  


            }
        
          //  DictTabSubitemExternapMappingName = 
        }

  			
	private static List< PdmTabBlockSubItemExtraInfoClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmTabBlockSubItemExtraInfoClrDto> listDto = new List<PdmTabBlockSubItemExtraInfoClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmTabBlockSubItemExtraInfoConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }


        private partial class PdmTabBlockSubItemExtraInfoConverter
        {
            //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
            public static PdmTabBlockSubItemExtraInfoClrDto ConvertDataRowDto(DataRow row)
            {
                PdmTabBlockSubItemExtraInfoClrDto aDto = new PdmTabBlockSubItemExtraInfoClrDto();
                aDto.BlockSubItemExtraInfoId = (System.Int32)row["BlockSubItemExtraInfoID"];
                aDto.SubItemId = (System.Int32)row["SubItemID"];
                aDto.TabId = (System.Int32)row["TabID"];
                aDto.AliasName = row["AliasName"] as System.String;
                aDto.ExternalMappingName = row["ExternalMappingName"] as System.String;
                aDto.Width = row["Width"] as System.Int32?;
                aDto.Height = row["Height"] as System.Int32?;
                aDto.Visible = (System.Boolean)row["Visible"];
                return aDto;
            }

        }
		        
    }


}

 