using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmBlockSubItemDal 
    {
        					
        public static  string QueryAll = @" SELECT  SubItemID ,  BlockID ,  SubItemName ,  ControlType ,  DataType ,  DefaultValueID ,  DefaultValueDate ,  EntityID ,  GridID ,  InternalCode ,  DefaultText ,  ERPMappingName ,  NeedValidator ,  ValidatorType ,  NBDecimal ,  MasterEntityControlID ,  UserDefineEntityColumnID ,  SubscribeSourceID ,  SortOrder ,  MaxCharLegnth ,  DDLParentLevelID ,  AutoIncrementSeed ,  AutoIncrementPrefix ,  AutoIncrementLastID ,  SubscribeGridBlockID ,  SubscribeGridColumnID ,  SubscribeColumnAggFuntionID ,  IsUniqueForOneRefTxType ,  IsNeedLog ,  ReferenceStaticFiledID ,  IsAllowEmpty ,  ToolTip ,  HorizontalAlignment ,  IsConvertToUpperCase ,  SysDefineEntityColumnName ,  SystemTimeStamp ,  DDLParentIntermediateEntityId ,  IsPluginAI ,  IsForeignKeyCscading   FROM [dbo].[pdmBlockSubItem] ";
			
		public static List< PdmBlockSubItemClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmBlockSubItemClrDto> listDto = new List<PdmBlockSubItemClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmBlockSubItemConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 