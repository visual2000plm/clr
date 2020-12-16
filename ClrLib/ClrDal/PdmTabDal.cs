using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmTabDal 
    {
        					
        public static  string QueryAll = @" SELECT  TabID ,  TabGroupID ,  TabName ,  TabDescription ,  CreatedBy ,  CreateDate ,  ModifydBy ,  ModifyDate ,  ManagementLevel ,  InternalCode ,  IsTemplateHeaderTab ,  IsProductDependent ,  IsMasterReferenceHeaderTab ,  ParentID ,  IsHideMasterHeader ,  ProductReferenceID ,  IsHideTemplateHeader ,  IsUsedForUpdate ,  SearchUpdateRefTxScope ,  IsUsedForSearch ,  FolderID ,  ProductCopyTabSort ,  ProductCopyTabRootTabID ,  SiblingCopyTabID ,  IsTabHeader ,  TabHeaderID ,  IsTemplateHeaderCollpase ,  IsMasterRefHeaderCollapse ,  IsTabHeaderCollapse ,  CopyTabValueHolderProductReferenceID ,  SystemTimeStamp ,  UILayout ,  RowIdentity ,  PrintUILayout ,  PrintTabHight ,  PrintTabWidth ,  IsAllowProductTabCopy   FROM [dbo].[pdmTab] ";
			
		public static List< PdmTabClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmTabClrDto> listDto = new List<PdmTabClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmTabConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 