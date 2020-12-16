using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmGridDal 
    {
        					
        public static  string QueryAll = @" SELECT  GridID ,  GridName ,  InternalCode ,  RowHight ,  Description ,  IsFixedTableLayout ,  NewRowViewPosition ,  GridType ,  ShareSimpleDCUID ,  ShareTxRefType ,  SubscribeSimpleDCUID ,  FolderID ,  SearchTemplateID ,  IsNeedDefualtRow ,  IsAllowToDeleteRow ,  IsAllowToEditRow ,  IsAllowToAddNewRow ,  IsAllowEmptyRow ,  ConceptualTemplateID ,  SystemTimeStamp ,  LinePlanningBlockID ,  CreatedByID ,  ModifiedBy ,  ModifiedDate ,  CreatedDate   FROM [dbo].[pdmGrid] ";
			
		public static List< PdmGridClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmGridClrDto> listDto = new List<PdmGridClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmGridConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 