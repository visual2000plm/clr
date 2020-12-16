using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmMassUpdateViewFieldDal 
    {
        					
        public static  string QueryAll = @" SELECT  MassUpdateViewFieldID ,  MassUpdateViewID ,  SubItemID ,  GridColumnID ,  IsReadonly ,  FriendName ,  Sort ,  Width ,  IsGroupBy ,  GroupByLevel ,  AggregationFunctionType ,  IsHide ,  ProductFieldId ,  SystemTimeStamp   FROM [dbo].[pdmMassUpdateViewField] ";
			
		public static List< PdmMassUpdateViewFieldClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmMassUpdateViewFieldClrDto> listDto = new List<PdmMassUpdateViewFieldClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmMassUpdateViewFieldConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 