using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmBlockDal 
    {
        					
        public static  string QueryAll = @" SELECT  BlockID ,  Name ,  CategoryID ,  CreatedByID ,  CreatedDate ,  ModifiedBy ,  ModifiedDate ,  InternalCode ,  UseStandardControl ,  SyncToPDMProduct ,  Description ,  ApproveRoleID ,  NotifyVersionChange ,  IsUseDrillDownControl ,  IsUseByMerchPlan ,  IsAllowProductTabCopy ,  IsMerchPlanCopyAble ,  FolderID ,  IsMasterSynToChild ,  IsReferenceStaticFiledControl ,  IsAllowCopyValueInRefSaveAs ,  ERPMappingName ,  IsForcedCreateFirstVersion ,  IsSynFromQuoteSampleToProduct ,  IsUsedForAutoGeneration ,  SpecialUserDefineType ,  SystemTimeStamp ,  IsAllowCopyValueInTabCopy ,  RowIdentity   FROM [dbo].[pdmBlock] ";
			
		public static List< PdmBlockClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmBlockClrDto> listDto = new List<PdmBlockClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmBlockConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 