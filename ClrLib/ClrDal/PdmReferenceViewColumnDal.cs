using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmReferenceViewColumnDal 
    {
        					
        public static  string QueryAll = @" SELECT  ReferenceViewColumnID ,  ReferenceViewID ,  SubItemID ,  GridColumnID ,  ProductFieldID ,  IsVisible ,  DisplayText ,  Sort ,  LabelColumn ,  LabelRow ,  LabelRowSpan ,  LabelColSpan ,  LabelIsVisible ,  ValueColumn ,  ValueRow ,  ValueColSpan ,  ValueRowSpan ,  IsUpdatable ,  ValueWidth ,  ValueHeight ,  SysTableFiledPath ,  ControlType ,  EntityID ,  DataType ,  IsGroupBy ,  GroupByLevel ,  AggregationFunctionType ,  SystemTimeStamp   FROM [dbo].[pdmReferenceViewColumn] ";
			
		public static List< PdmReferenceViewColumnClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmReferenceViewColumnClrDto> listDto = new List<PdmReferenceViewColumnClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmReferenceViewColumnConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 