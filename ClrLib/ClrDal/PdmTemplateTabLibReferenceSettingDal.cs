using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTemplateTabLibReferenceSettingDal 
    {
        					
        public static  string QueryAll = @" SELECT  TemplateTabLibRefID ,  Name ,  Description ,  TemplateID ,  TabID ,  LibReferenceID ,  GridBlockID ,  Type ,  EntityID ,  SystemTimeStamp   FROM [dbo].[pdmTemplateTabLibReferenceSetting] ";
			
		public static List< PdmTemplateTabLibReferenceSettingClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmTemplateTabLibReferenceSettingClrDto> listDto = new List<PdmTemplateTabLibReferenceSettingClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmTemplateTabLibReferenceSettingConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 