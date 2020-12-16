   using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
namespace PLMCLRTools
{
     public  class  BunchObject 
        {
            public object o1
            {
                get;set;
            }
             public object o2
            {
                get;set;
            }

             public object o3
            {
                get;set;
            }
        }

    #region------------- DTO help class

   
    public class PdmClrEntitySimpleStructureDto
    {
        #region Entity Dto Properties

        public int EntityId
        {
            get;
            set;
        }

        public System.String EntityCode
        {
            get;
            set;
        }

        public int? EntityType
        {
            get;
            set;
        }

        public string SysTableName
        {
            get;
            set;
        }

        public List<PdmEntityColumnClrUserDefineDto> Columns
        {
            get;
            set;
        }

        public List<PdmEntityColumnClrUserDefineDto> PrimaryKeyColumn
        {
            get
            {
                return Columns.Where(o => o.IsPrimaryKey.HasValue && o.IsPrimaryKey.Value).ToList();
            }
        }

        // For userDefine Table

        public string DWUserDefineTableName
        {
            get
            {
                return PLMConstantString.PLM_DW_UserDefineTablePrefix + EntityCode + "_" + EntityId;
            }
        }

        public string SQLSelect
        {
            get;
            set;
        }

        #endregion Entity Dto Properties
    }

    public class PdmEntityColumnClrUserDefineDto
    {
        public PdmEntityColumnClrUserDefineDto()
        {
        }

        public System.Int32 EntityId
        {
            get;
            set;
        }

        public int UserDefineEntityColumnID
        {
            get;
            set;
        }

        public System.String ColumnName
        {
            get;
            set;
        }

        //public int? DataType
        //{
        //    get;
        //    set;
        //}
        // int,varchar,char,etcc
        public string DataBaseDataType
        {
            get;
            set;
        }

        public bool DataBaseIsNullAble
        {
            get;
            set;
        }

        public bool? UsedByDropDownList
        {
            get;
            set;
        }

        public int? DataRowSort
        {
            get;
            set;
        }

        public bool? IsPrimaryKey
        {
            get;
            set;
        }

        public bool? IsIdentity
        {
            get;
            set;
        }

        public String SystemTableColumnName
        {
            get;
            set;
        }

        public int? FkentityId
        {
            get;
            set;
        }

        public int? UicontrolType
        {
            get;
            set;
        }

        public int? Nbdecimal
        {
            get;
            set;
        }
    }

    internal class TabGridScriptDTO
    {
        public int DWScriptID
        {
            get;
            set;
        }

        public SqlInt32 TabID
        {
            get;
            set;
        }

        public string TabName
        {
            get;
            set;
        }

        public SqlInt32 GridID
        {
            get;
            set;
        }

        public SqlInt32 EntityID
        {
            get;
            set;
        }

        public string GridName
        {
            get;
            set;
        }

        public string InserIntoSQLScript
        {
            get;
            set;
        }

        public string RootLevelSelectSQLScript
        {
            get;
            set;
        }

        public string DWTableSchemeScript
        {
            get;
            set;
        }

        public string DWTableName
        {
            get;
            set;
        }

        public bool IsPassValidation
        {
            get;
            set;
        }
    }

    public class GridColumnClrUserDefineDto
    {
        public int GridColumnID
        {
            get;
            set;
        }

        public int ColumnTypeId
        {
            get;
            set;
        }

        public string ColumnName
        {
            get;
            set;
        }

        public int? EntityID
        {
            get;
            set;
        }

        public string EntityCode
        {
            get;
            set;
        }

        //public bool IsSpecialColumnType
        //{
        //    get
        //    {
        //        if (SpecialColumnTypeDto == (int)EmSpecialColumnType.CurrentRefKeyDCUColumn
        //            || SpecialColumnTypeDto == (int)EmSpecialColumnType.CurrentRefSimpleDCU
        //            || SpecialColumnTypeDto == (int)EmSpecialColumnType.DynamicMatrixKeyColumn
        //            || SpecialColumnTypeDto == (int)EmSpecialColumnType.ForeignKeyDependentColumn
        //            || SpecialColumnTypeDto == (int)EmSpecialColumnType.PointToExternalDCUDataSourceKeyColumn
        //            || SpecialColumnTypeDto == (int)EmSpecialColumnType.ProductGridKeyDCUColumn
        //            || SpecialColumnTypeDto == (int)EmSpecialColumnType.ProductGridSimpleDCU)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        //public bool IsReadOnlyOfColumnTypeId
        //{
        //    get
        //    {
        //        if (IsSpecialColumnType)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        //public bool IsEntityRelatedColumnType
        //{
        //    get
        //    {
        //        if (ColumnTypeId == (int)EmGridColumnType.DDL
        //            || ColumnTypeId == (int)EmGridColumnType.File
        //            || ColumnTypeId == (int)EmGridColumnType.Image
        //            || IsSpecialColumnType)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        //public int SpecialColumnTypeDto
        //{
        //    get
        //    {
        //        if ((!DcucolumnId.HasValue) && (!Dcuid.HasValue) && (!MasterEntityColumnId.HasValue))
        //        {
        //            return (int)EmSpecialColumnType.RegularColumn;
        //        }
        //        else if (object.Equals(IsDynamicMatrixKey, true)
        //            && DcucolumnId.HasValue
        //            && DcucolumnBlockId.HasValue)
        //        {
        //            return (int)EmSpecialColumnType.DynamicMatrixKeyColumn;
        //        }
        //        else if (object.Equals(IsDcuforProductGridRef, true) && (Dcuid.HasValue))
        //        {
        //            return (int)EmSpecialColumnType.ProductGridSimpleDCU;
        //        }
        //        else if (MasterDcucolumnId.HasValue && DcucolumnId.HasValue)
        //        {
        //            return (int)EmSpecialColumnType.ForeignKeyDependentColumn;
        //        }
        //        else if (object.Equals(IsDcuforProductGridRef, true)
        //            && (!MasterDcucolumnId.HasValue)
        //            && DcucolumnId.HasValue
        //            && DcucolumnBlockId.HasValue)
        //        {
        //            return (int)EmSpecialColumnType.ProductGridKeyDCUColumn;
        //        }
        //        else if ((!object.Equals(IsDcuforProductGridRef, true)) && Dcuid.HasValue)
        //        {
        //            return (int)EmSpecialColumnType.CurrentRefSimpleDCU;
        //        }
        //        else if (PublishSimpleDcuid.HasValue
        //            && PublishSimpleDcutxRefType.HasValue
        //            && CurrentRefSubscribeSimpleDcuid.HasValue
        //            && DcucolumnId.HasValue
        //            && DcucolumnBlockId.HasValue)
        //        {
        //            return (int)EmSpecialColumnType.PointToExternalDCUDataSourceKeyColumn;
        //        }
        //        else if ((!object.Equals(IsDcuforProductGridRef, true))
        //            && (!object.Equals(IsDynamicMatrixKey, true))
        //            && (!MasterDcucolumnId.HasValue)
        //            && DcucolumnId.HasValue
        //            && DcucolumnBlockId.HasValue
        //            && (!PublishSimpleDcuid.HasValue)
        //            && (!PublishSimpleDcutxRefType.HasValue)
        //            && (!CurrentRefSubscribeSimpleDcuid.HasValue))
        //        {
        //            return (int)EmSpecialColumnType.CurrentRefKeyDCUColumn;
        //        }
        //        else
        //        {
        //            return (int)EmSpecialColumnType.UnknownTypeColumn;
        //        }
        //    }
        //    set { SetValue(() => SpecialColumnTypeDto, value); }
        //}
    }

    public class BlockSubitemClrUserDefineDto
    {
        public int SubItemID
        {
            get;
            set;
        }

        public int ControlType
        {
            get;
            set;
        }

        public string SubItemName
        {
            get;
            set;
        }

        public string SubItemFullPathName
        {
            get;
            set;
        }

        public int? EntityID
        {
            get;
            set;
        }

        public string EntityCode
        {
            get;
            set;
        }
    }



    public class LookupItemDto
    {
        public object Id
        {
            get;
            set;
        }

        public string Display
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.Display;
        }

        public string FirstField
        {
            get
            {
                if (!String.IsNullOrEmpty(Display))
                {
                    string[] tokens = Display.Split(new Char[] { '|' });

                    if (tokens.Length == 1)
                    {
                        return Display.Trim();
                    }
                    else
                    {
                        return tokens[0].Trim();
                    }
                }

                return string.Empty;
            }
        }
    }

    public enum UserDefineEntityColumnDataType { Text = 1, Number = 2, DateTime = 3 }

    public class PdmERPTableMappingDto
    {
        public int MappingID
        {
            get;
            set;
        }

        public string PLMTableName
        {
            get;
            set;
        }

        public string PLMPrimaryKeyColumn
        {
            get;
            set;
        }

        public string PLMLogicalUniqueColumn
        {
            get;
            set;
        }

        public string EXTableName
        {
            get;
            set;
        }

        public string EXPrimaryKeyColumn
        {
            get;
            set;
        }

        public string EXLogicalUniqueColumn
        {
            get;
            set;
        }

        public int EXChangeMode
        {
            get;
            set;
        }

        public int? EXStartRootPKIDUsedByPLM
        {
            get;
            set;
        }

        public List<PdmERPTableColumnMappingDto> MappingColumnList
        {
            get;
            set;
        }

        public byte[] LastReadPLMTableTimeStamp
        {
            get;
            set;
        }

        public byte[] LastReadExchangeTableTimeStamp
        {
            get;
            set;
        }

        public string ExchangeTableTimeStampColumnName
        {
            get;
            set;
        }

        public string PlmTableTimeStampColumnName
        {
            get;
            set;
        }
    }

    public class PdmERPTableColumnMappingDto
    {
        public int ColumnMappingID
        {
            get;
            set;
        }

        public int MappingID
        {
            get;
            set;
        }

        public string PLMColumn
        {
            get;
            set;
        }

        public string EXColumn
        {
            get;
            set;
        }

        public int? FKEntityMappingID
        {
            get;
            set;
        }

        public int? EXDataType
        {
            get;
            set;
        }

        public int? EXDataLength
        {
            get;
            set;
        }

        public int? PLMDataType
        {
            get;
            set;
        }

        public int? PLMDataLength
        {
            get;
            set;
        }

        public PdmERPTableMappingDto FKPdmERPTableMappingDto
        {
            get;
            set;
        }
    }

    #endregion
}