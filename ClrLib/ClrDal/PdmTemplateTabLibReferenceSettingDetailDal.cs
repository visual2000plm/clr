using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTemplateTabLibReferenceSettingDetailDal 
    {
        					
        public static  string QueryAll = @" SELECT  DetailID ,  TemplateTabLibRefID ,  FiledSubitmeID ,  MappingGridColumnID ,  MappingType ,  UserDefineEntityColumnID ,  SystemTimeStamp   FROM [dbo].[pdmTemplateTabLibReferenceSettingDetail] ";
			
		public static List< PdmTemplateTabLibReferenceSettingDetailClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmTemplateTabLibReferenceSettingDetailClrDto> listDto = new List<PdmTemplateTabLibReferenceSettingDetailClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmTemplateTabLibReferenceSettingDetailConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 