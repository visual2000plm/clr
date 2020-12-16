using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmRgbcolorReferenceTypeDal 
    {
        					
        public static  string QueryAll = @" SELECT  ColorReferenceTypeID ,  ReferenceGroup ,  ReferenceDescription   FROM [dbo].[pdmRGBColorReferenceType] ";
			
		public static List< PdmRgbcolorReferenceTypeClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmRgbcolorReferenceTypeClrDto> listDto = new List<PdmRgbcolorReferenceTypeClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmRgbcolorReferenceTypeConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 