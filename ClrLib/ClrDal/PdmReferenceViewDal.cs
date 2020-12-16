using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmReferenceViewDal 
    {
        					
        public static  string QueryAll = @" SELECT  ReferenceViewID ,  Name ,  Description ,  TypeID ,  NoSecurity ,  GridOutputMode ,  Options ,  ViewType ,  ColumnCount ,  RowPerPage ,  IsFilterByFolderSecurity ,  MainTabID ,  GridBlockID ,  GridID ,  UpdateType ,  LineSheetTemplateID ,  IsFilterByCurrentDiv ,  IsFilterByCurrentUser ,  SystemTimeStamp ,  UILayout ,  ItemWidth ,  ItemHight ,  BLQueryID ,  CatalogID ,  IsUpdatable   FROM [dbo].[pdmReferenceView] ";
			
		public static List< PdmReferenceViewClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmReferenceViewClrDto> listDto = new List<PdmReferenceViewClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmReferenceViewConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 