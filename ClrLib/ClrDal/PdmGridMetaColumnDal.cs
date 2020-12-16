using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmGridMetaColumnDal 
    {
        					
        public static  string QueryAll = @" SELECT  GridColumnID ,  GridID ,  ColumnName ,  ColumnWidth ,  ColumnTypeID ,  DataType ,  ColumnOrder ,  EntityID ,  InternalCode ,  Hidden ,  IsUsedToLockRow ,  IsUsedToDisplayProductGridRowInfo ,  NBDecimal ,  DCUColumnID ,  DCUID ,  ERPMappingName ,  DCUColumnBlockID ,  DisplayProductGridRowInfoOrder ,  MasterEntityColumnID ,  ChildColumnID ,  CascadingParentID ,  ProductColorSizeDepdentType ,  DisplayFormat ,  DepdentProductColorSizeDepdentTypeColumnID ,  IsPrimaryKey ,  IsNeedLog ,  IsReadOnly ,  MasterDCUColumnID ,  PublishSimpleDCUID ,  PublishSimpleDCUTxRefType ,  CurrentRefSubscribeSimpleDCUID ,  IsDynamicMatrixKey ,  IsDCUForProductGridRef ,  IsAutoKeyColumn ,  IsAutoKeyDisplayColumn ,  IsLogicalDisplayForVersionChange ,  IsAllowNull ,  DefaultValue ,  HorizontalAlignment ,  IsGroupBy ,  GroupByLevel ,  RangePlanColumnMapToDcuID ,  RangePlanColumnMapToAggegationDcuID ,  IsRangePlanRowLevelKey ,  RangePlanMapingDcuAggegationType ,  SystemTimeStamp ,  CurrentRefRowLevelSubscribeSimpleDCUID ,  CurrentRefRowLevelPublishSimpleDCUID   FROM [dbo].[PdmGridMetaColumn] ";
			
		public static List< PdmGridMetaColumnClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmGridMetaColumnClrDto> listDto = new List<PdmGridMetaColumnClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmGridMetaColumnConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 