using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmV2kBodyPartConverter 
    {
       
		
		   //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmV2kBodyPartClrDto ConvertDataRowDto(DataRow row)
        {
            PdmV2kBodyPartClrDto aDto = new  PdmV2kBodyPartClrDto();
			aDto.BodyPartId =(System.Int32)row["BodyPartID"];
			aDto.BodyPartName =(System.String)row["BodyPartName"];
			aDto.Code =row["Code"]as System.String	 ;
			aDto.GroupId =row["GroupID"]  as System.Int32 ? ;
			aDto.ProductReferenceId =row["ProductReferenceID"]  as System.Int32 ? ;
			aDto.Description =row["Description"]as System.String	 ;
			aDto.MeasureInstruction =row["MeasureInstruction"]as System.String	 ;
			aDto.InitialSpecValue =row["InitialSpecValue"]  as System.Decimal ? ;
			aDto.Tolerance =row["Tolerance"]  as System.Decimal ? ;
			aDto.GradingPlusValue =row["GradingPlusValue"]  as System.Decimal ? ;
			aDto.GradingMinuValue =row["GradingMinuValue"]  as System.Decimal ? ;
			aDto.IsHight =row["IsHight"]  as System.Boolean ? ;
			aDto.FolderId =row["FolderID"]  as System.Int32 ? ;
			aDto.IsImport =row["IsImport"]  as System.Boolean ? ;
			aDto.SketchId =row["SketchID"]  as System.Int32 ? ;
			aDto.MeasureOfUnitId =row["MeasureOfUnitID"]  as System.Int32 ? ;
			aDto.DimensionId =row["DimensionID"]  as System.Int32 ? ;
			aDto.DimensionDetailId =row["DimensionDetailID"]  as System.Int32 ? ;
			aDto.IsNeedToApplyGradingRule =row["IsNeedToApplyGradingRule"]  as System.Boolean ? ;
			aDto.SystemTimeStamp =(System.Byte[])row["SystemTimeStamp"];
            return aDto;
        }
      		
     
		        
    }
}

 