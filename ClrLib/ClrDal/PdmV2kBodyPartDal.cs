using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmV2kBodyPartDal 
    {
        					
        public static  string QueryAll = @" SELECT  BodyPartID ,  BodyPartName ,  Code ,  GroupID ,  ProductReferenceID ,  Description ,  MeasureInstruction ,  InitialSpecValue ,  Tolerance ,  GradingPlusValue ,  GradingMinuValue ,  IsHight ,  FolderID ,  IsImport ,  SketchID ,  MeasureOfUnitID ,  DimensionID ,  DimensionDetailID ,  IsNeedToApplyGradingRule ,  SystemTimeStamp   FROM [dbo].[pdmV2kBodyPart] ";
			
		public static List< PdmV2kBodyPartClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmV2kBodyPartClrDto> listDto = new List<PdmV2kBodyPartClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmV2kBodyPartConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 