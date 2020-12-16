using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PLMCLRTools
{
   public static  class PLMDataRowConverter
    {
        //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmClrEntitySimpleStructureDto ConvertRowToPdmEntityDto(DataRow row)
        {
            PdmClrEntitySimpleStructureDto aPdmEntityDto = new PdmClrEntitySimpleStructureDto();
            aPdmEntityDto.EntityId = (int)row["EntityID"];
            aPdmEntityDto.EntityCode = (string)row["EntityCode"];
            aPdmEntityDto.EntityType = row["EntityType"] as int?;
            aPdmEntityDto.SysTableName = row["SysTableName"] as string;

            return aPdmEntityDto;
        }




        //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmEntityColumnClrUserDefineDto ConvertUserDefineEntityColumnDataRowToPdmEntityColumnDto(DataRow row)
        {
            PdmEntityColumnClrUserDefineDto aPdmEntityDto = new PdmEntityColumnClrUserDefineDto();
            aPdmEntityDto.UserDefineEntityColumnID = (int)row["UserDefineEntityColumnID"];
            aPdmEntityDto.EntityId = (int)row["EntityID"];
            aPdmEntityDto.ColumnName = row["ColumnName"] as string;
            // aPdmEntityDto.DataType = row["ColumnName"] as int?;
            aPdmEntityDto.UsedByDropDownList = row["UsedByDropDownList"] as bool?;
            aPdmEntityDto.DataRowSort = row["DataRowSort"] as int?;
            aPdmEntityDto.IsPrimaryKey = row["IsPrimaryKey"] as bool?;
            aPdmEntityDto.IsIdentity = row["IsIdentity"] as bool?;
            aPdmEntityDto.SystemTableColumnName = row["SystemTableColumnName"] as string;
            aPdmEntityDto.FkentityId = row["FkentityId"] as int?;
            aPdmEntityDto.UicontrolType = row["UicontrolType"] as int?;
            aPdmEntityDto.Nbdecimal = row["Nbdecimal"] as int?;
            return aPdmEntityDto;
        }

        //select EntityID ,EntityCode ,EntityType,SysTableName  from pdmEntity
        public static PdmEntityColumnClrUserDefineDto ConvertErpExchangeDatabaseTableColumnDataRowToPdmEntityColumnDto(DataRow row)
        {
            PdmEntityColumnClrUserDefineDto aPdmEntityDto = new PdmEntityColumnClrUserDefineDto();
            // aPdmEntityDto.UserDefineEntityColumnID = (int)row["UserDefineEntityColumnID"];
            // aPdmEntityDto.EntityId = (int)row["EntityID"];
            aPdmEntityDto.ColumnName = row["ColumnName"] as string;
            aPdmEntityDto.DataBaseDataType = row["DataType"] as string;
            aPdmEntityDto.IsPrimaryKey = row["PrimaryKey"] as bool?;
            aPdmEntityDto.UicontrolType =PLMDataRowConverter. ConvertDataBaseDataTypeToUicontrolType(aPdmEntityDto.DataBaseDataType);
            if (aPdmEntityDto.DataBaseDataType == "float"

                     || aPdmEntityDto.DataBaseDataType == "real"

                     || aPdmEntityDto.DataBaseDataType == "decimal")
            {

                aPdmEntityDto.Nbdecimal = row["Scale"] as int?;
            }

            // aPdmEntityDto.Nbdecimal 
            //aPdmEntityDto.UicontrolType 
            // aPdmEntityDto.IsIdentity   = row["PrimaryKey"] as bool?;



            //aPdmEntityDto.UsedByDropDownList = row["UsedByDropDownList"] as bool?;
            //aPdmEntityDto.DataRowSort = row["DataRowSort"] as int?;
            //aPdmEntityDto.IsPrimaryKey = row["IsPrimaryKey"] as bool?;
            //aPdmEntityDto.IsIdentity = row["IsIdentity"] as bool?;
            //aPdmEntityDto.SystemTableColumnName = row["SystemTableColumnName"] as string;
            //aPdmEntityDto.FkentityId = row["FkentityId"] as int?;
            // aPdmEntityDto.UicontrolType = row["UicontrolType"] as int?;
            // aPdmEntityDto.Nbdecimal = row["Nbdecimal"] as int?;
            return aPdmEntityDto;
        }

        private static int ConvertDataBaseDataTypeToUicontrolType(string dataBaseType)
        {
            // Nbdecimal = 0;
            dataBaseType = dataBaseType.Trim().ToLowerInvariant();

            if (dataBaseType == "datetime")
            {
                return (int)EmControlType.Date;

            }
            else if (dataBaseType == "bit")
            {
                return (int)EmControlType.CheckBox;

            }
            else if (
                      dataBaseType == "int"
                     || dataBaseType == "float"
                     || dataBaseType == "tinyint"
                     || dataBaseType == "real"
                     || dataBaseType == "bigint"
                     || dataBaseType == "decimal"




                )
            {
                return (int)EmControlType.Numeric;

            }


            return (int)EmControlType.TextBox;

        }


        internal static PdmERPTableMappingDto ConvertPdmErpMappingTableToErpMappingTableDto(DataRow row)
        {
            PdmERPTableMappingDto aPdmEntityDto = new PdmERPTableMappingDto();
            aPdmEntityDto.MappingID = (int)row["MappingID"];
            aPdmEntityDto.PLMTableName = row["PLMTableName"] as string; ;
            aPdmEntityDto.PLMPrimaryKeyColumn = row["PLMPrimaryKeyColumn"] as string;
           aPdmEntityDto.PLMLogicalUniqueColumn = row["PLMLogicalUniqueColumn"] as string; ;

           aPdmEntityDto.EXTableName = row["EXTableName"] as string; ;
           aPdmEntityDto.EXPrimaryKeyColumn = row["EXPrimaryKeyColumn"] as string; ;
           aPdmEntityDto.EXLogicalUniqueColumn = row["EXLogicalUniqueColumn"] as string; ;
           aPdmEntityDto.EXStartRootPKIDUsedByPLM = row["EXStartRootPKIDUsedByPLM"] as int?;
           aPdmEntityDto.LastReadExchangeTableTimeStamp = row["LastReadExchangeTableTimeStamp"] as byte[];
           aPdmEntityDto.LastReadPLMTableTimeStamp = row["LastReadPLMTableTimeStamp"] as byte[];
           aPdmEntityDto.ExchangeTableTimeStampColumnName = row["ExchangeTableTimeStampColumnName"] as string; ;
           aPdmEntityDto.PlmTableTimeStampColumnName = row["PlmTableTimeStampColumnName"] as string; ;



           int? exchangeMode = row["EXChangeMode"] as int?;
            if(exchangeMode.HasValue )
            {
                aPdmEntityDto.EXChangeMode = exchangeMode.Value ; 

            }
            else
            {
                aPdmEntityDto.EXChangeMode = (int)EmExChangeMode.OneWay; 
            }

          

            
        
            return aPdmEntityDto;
        }

        internal static PdmERPTableColumnMappingDto ConvertPdmERPTableColumnMappingToDto(DataRow row)
        {
            PdmERPTableColumnMappingDto aPdmEntityDto = new PdmERPTableColumnMappingDto();
            aPdmEntityDto.ColumnMappingID = (int)row["ColumnMappingID"];
            aPdmEntityDto.MappingID = (int)row["MappingID"];
            aPdmEntityDto.PLMColumn = row["PLMColumn"] as string; ;
            aPdmEntityDto.EXColumn = row["EXColumn"] as string;
            aPdmEntityDto.FKEntityMappingID = row["FKEntityMappingID"] as int?;
            aPdmEntityDto.EXDataType = row["EXDataType"] as int?;
            aPdmEntityDto.EXDataLength = row["EXDataLength"] as int?;
            aPdmEntityDto.PLMDataType = row["PLMDataType"] as int?;
            aPdmEntityDto.PLMDataLength = row["PLMDataLength"] as int ?;

          
          
            return aPdmEntityDto;
        }
    }
}
