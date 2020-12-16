using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSearchTemplateDcuDal 
    {
        					
        public static  string QueryAll = @" SELECT  SearchTemplateDCUID ,  SearchTemplateID ,  SubitemID ,  GridColumnID ,  EmStaticSearchControl ,  Sort ,  DCUColumnBlockID ,  PositionRow ,  PositionColumn ,  OperationID ,  DisplayText ,  IsVisible ,  DefaultValue ,  IsReadOnly ,  IsAutoPopulate ,  ParentDCUID ,  IsLoadOnDemand ,  SysTableFiledPath ,  SystemTimeStamp ,  ControlType ,  EntityID ,  DataType   FROM [dbo].[pdmSearchTemplateDCU] ";
			
		public static List< PdmSearchTemplateDcuClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmSearchTemplateDcuClrDto> listDto = new List<PdmSearchTemplateDcuClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmSearchTemplateDcuConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 