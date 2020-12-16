using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmGridMetaColumnClrDto 
    {


        public string EntityCode
        {
            get;
            set;
        }

        public PdmEntityBlClrDto EntityBlClrDto
        {
            get;
            set;
        }

        public bool IsSpecialColumnType
        {
            get
            {
                if (SpecialColumnType == (int)EmSpecialColumnType.CurrentRefKeyDCUColumn
                    || SpecialColumnType == (int)EmSpecialColumnType.CurrentRefSimpleDCU
                    || SpecialColumnType == (int)EmSpecialColumnType.DynamicMatrixKeyColumn
                    || SpecialColumnType == (int)EmSpecialColumnType.ForeignKeyDependentColumn
                    || SpecialColumnType == (int)EmSpecialColumnType.PointToExternalDCUDataSourceKeyColumn
                    || SpecialColumnType == (int)EmSpecialColumnType.ProductGridKeyDCUColumn
                    || SpecialColumnType == (int)EmSpecialColumnType.ProductGridSimpleDCU)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsReadOnlyOfColumnTypeId
        {
            get
            {
                if (IsSpecialColumnType)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsEntityRelatedColumnType
        {
            get
            {
                if (ColumnTypeId == (int)EmControlType.DDL
                    || ColumnTypeId == (int)EmControlType.File
                    || ColumnTypeId == (int)EmControlType.Image
                    || IsSpecialColumnType)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public int SpecialColumnType
        {
            get
            {
                if ((!DcucolumnId.HasValue) && (!Dcuid.HasValue) && (!MasterEntityColumnId.HasValue))
                {
                    return (int)EmSpecialColumnType.RegularColumn;
                }
                else if (object.Equals(IsDynamicMatrixKey, true)
                    && DcucolumnId.HasValue
                    && DcucolumnBlockId.HasValue)
                {
                    return (int)EmSpecialColumnType.DynamicMatrixKeyColumn;
                }
                else if (object.Equals(IsDcuforProductGridRef, true) && (Dcuid.HasValue))
                {
                    return (int)EmSpecialColumnType.ProductGridSimpleDCU;
                }
                else if (MasterDcucolumnId.HasValue && DcucolumnId.HasValue)
                {
                    return (int)EmSpecialColumnType.ForeignKeyDependentColumn;
                }
                else if (object.Equals(IsDcuforProductGridRef, true)
                    && (!MasterDcucolumnId.HasValue)
                    && DcucolumnId.HasValue
                    && DcucolumnBlockId.HasValue)
                {
                    return (int)EmSpecialColumnType.ProductGridKeyDCUColumn;
                }
                else if ((!object.Equals(IsDcuforProductGridRef, true)) && Dcuid.HasValue)
                {
                    return (int)EmSpecialColumnType.CurrentRefSimpleDCU;
                }
                else if (PublishSimpleDcuid.HasValue
                    && PublishSimpleDcutxRefType.HasValue
                    && CurrentRefSubscribeSimpleDcuid.HasValue
                    && DcucolumnId.HasValue
                    && DcucolumnBlockId.HasValue)
                {
                    return (int)EmSpecialColumnType.PointToExternalDCUDataSourceKeyColumn;
                }
                else if ((!object.Equals(IsDcuforProductGridRef, true))
                    && (!object.Equals(IsDynamicMatrixKey, true))
                    && (!MasterDcucolumnId.HasValue)
                    && DcucolumnId.HasValue
                    && DcucolumnBlockId.HasValue
                    && (!PublishSimpleDcuid.HasValue)
                    && (!PublishSimpleDcutxRefType.HasValue)
                    && (!CurrentRefSubscribeSimpleDcuid.HasValue))
                {
                    return (int)EmSpecialColumnType.CurrentRefKeyDCUColumn;
                }
                else
                {
                    return (int)EmSpecialColumnType.UnknownTypeColumn;
                }
            }
            set
            {

            }
        }

	
		        
    }
}

