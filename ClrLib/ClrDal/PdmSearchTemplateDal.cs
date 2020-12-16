using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmSearchTemplateDal 
    {
        					
        public static  string QueryAll = @" SELECT  SearchTemplateID ,  Name ,  Description ,  URLLink ,  Type ,  IsBuiltIn ,  OutputTabID ,  WhereUsedSearchTemplateResultID ,  ReferenceViewID ,  IsDefault ,  IsAutoExecute ,  BLQueryID ,  CatalogID ,  SystemTimeStamp ,  DefaultMassUpdateViewID ,  IsNoSecuirty ,  TechPackTypeID   FROM [dbo].[pdmSearchTemplate] ";
			
		public static List< PdmSearchTemplateClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmSearchTemplateClrDto> listDto = new List<PdmSearchTemplateClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmSearchTemplateConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 