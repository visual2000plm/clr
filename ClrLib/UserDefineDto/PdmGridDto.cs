using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmGridClrDto 
    {
         public static readonly string ProductGridProductReferenceIDColumnName = "ProductReferenceID";


         public PdmGridMetaColumnClrDto this[int gridColumnId]
         {
             get
             {
                 return this.PdmGridMetaColumnList.Where(o => (int)o.GridColumnId  == gridColumnId).FirstOrDefault();
             }
         }

         public PdmGridMetaColumnClrDto this[string internalCode]
         {
             get
             {
                 return this.PdmGridMetaColumnList.Where(o => o.InternalCode == internalCode).FirstOrDefault();
             }
         }


        public PdmGridMetaColumnClrDto ProductGridProductReferenceColumn
        {
            get
            {
                if (this.GridType == (int)EmGridType.ProductBasedGrid)
                {
                    foreach (var aColumn in this.PdmGridMetaColumnList)
                    {
                        if (!string.IsNullOrEmpty(aColumn.InternalCode))
                        {
                            if (aColumn.InternalCode.ToLowerInvariant() == ProductGridProductReferenceIDColumnName.ToLowerInvariant())
                                return aColumn;
                        }
                    }

                }	

                return null;
            }
        }

        private List<PdmGridMetaColumnClrDto> _keyMetaColumns;

        public List<PdmGridMetaColumnClrDto> KeyMetaColumns
        {
            get
            {
                if (_keyMetaColumns == null)
                {
                    _keyMetaColumns = new List<PdmGridMetaColumnClrDto>();
                    foreach (var aColumn in this.PdmGridMetaColumnList)
                    {
                        if (aColumn.IsPrimaryKey.HasValue && aColumn.IsPrimaryKey.Value)
                        {
                            _keyMetaColumns.Add(aColumn);
                        }
                    }
                }

                return _keyMetaColumns;
            }
        }

        private List<PdmGridMetaColumnClrDto> _ProductGridKeyDCUColumns;

        public List<PdmGridMetaColumnClrDto> ProductGridForeignKeyColumns
        {
            get
            {
                if (_ProductGridKeyDCUColumns == null)
                {
                    _ProductGridKeyDCUColumns = new List<PdmGridMetaColumnClrDto>();

                    foreach (PdmGridMetaColumnClrDto aGridMetaColumn in PdmGridMetaColumnList)
                    {
                        if (aGridMetaColumn.SpecialColumnType == (int)EmSpecialColumnType.ProductGridKeyDCUColumn)
                        {
                            _ProductGridKeyDCUColumns.Add(aGridMetaColumn);
                        }
                    }
                }

                return _ProductGridKeyDCUColumns;
            }
        }


        private Dictionary<int, List<PdmGridMetaColumnClrDto>> _ForeignKeyDepdedentColumns;

        public Dictionary<int, List<PdmGridMetaColumnClrDto>> ForeignKeyDepdedentColumns
        {
            get
            {
                if (_ForeignKeyDepdedentColumns == null)
                {
                    _ForeignKeyDepdedentColumns = new Dictionary<int, List<PdmGridMetaColumnClrDto>>();

                    foreach (var productGridKeyColumn in ProductGridForeignKeyColumns)
                    {
                        var list = PdmGridMetaColumnList.Where(c => c.MasterDcucolumnId == productGridKeyColumn.GridColumnId && c.GridId == productGridKeyColumn.GridId).ToList();
                        if (list.Count > 0)
                        {
                            _ForeignKeyDepdedentColumns.Add(productGridKeyColumn.GridColumnId, list);
                        }

                       
                    }
                }

                return _ForeignKeyDepdedentColumns;
            }
        }


        private List<PdmGridMetaColumnClrDto> _ProductGridSimpleDCUColumn;

        public List<PdmGridMetaColumnClrDto> ProductGridSimpleDCUColumn
        {
            get
            {
                if (_ProductGridSimpleDCUColumn == null)
                {
                    _ProductGridSimpleDCUColumn = new List<PdmGridMetaColumnClrDto>();

                    foreach (PdmGridMetaColumnClrDto aGridMetaColumn in PdmGridMetaColumnList)
                    {
                        if (aGridMetaColumn.SpecialColumnType == (int)EmSpecialColumnType.ProductGridSimpleDCU)
                        {
                            _ProductGridSimpleDCUColumn.Add(aGridMetaColumn);
                        }
                    }
                }

                return _ProductGridSimpleDCUColumn;
            }
        }



        private List<PdmGridMetaColumnClrDto> _CurrentRefSimpleDCUColumns;

        public List<PdmGridMetaColumnClrDto> CurrentRefSimpleDCUColumns
        {
            get
            {
                if (_CurrentRefSimpleDCUColumns == null)
                {
                    _CurrentRefSimpleDCUColumns = new List<PdmGridMetaColumnClrDto>();

                    foreach (var aColumn in PdmGridMetaColumnList)
                    {
                        if (aColumn.SpecialColumnType == (int)EmSpecialColumnType.CurrentRefSimpleDCU)
                        {
                            _CurrentRefSimpleDCUColumns.Add(aColumn);
                        }
                    }
                }

                return _CurrentRefSimpleDCUColumns;
            }
        }


        // like current product colorID and dynamic key
        private List<PdmGridMetaColumnClrDto> _CurrentRefKeyDCUColumn;

        public List<PdmGridMetaColumnClrDto> CurrentRefRegularColumnKeyAndDynamicMatrixKeyDCUColumn
        {
            get
            {
                if (_CurrentRefKeyDCUColumn == null)
                {
                    _CurrentRefKeyDCUColumn = new List<PdmGridMetaColumnClrDto>();

                    foreach (var aColumn in PdmGridMetaColumnList)
                    {
                        if (
                              aColumn.SpecialColumnType == (int)EmSpecialColumnType.CurrentRefKeyDCUColumn
                            || aColumn.SpecialColumnType == (int)EmSpecialColumnType.DynamicMatrixKeyColumn)
                        {
                            _CurrentRefKeyDCUColumn.Add(aColumn);
                        }
                    }
                }

                return _CurrentRefKeyDCUColumn;
            }
        }


        private List<PdmGridMetaColumnClrDto> _CurrentRefKeyDCUColumnWithoutMatrixKeyDCUColumn;

        public List<PdmGridMetaColumnClrDto> CurrentRefKeyDCUColumnWithoutMatrixKeyDCUColumn
        {
            get
            {
                if (_CurrentRefKeyDCUColumnWithoutMatrixKeyDCUColumn == null)
                {
                    _CurrentRefKeyDCUColumnWithoutMatrixKeyDCUColumn = new List<PdmGridMetaColumnClrDto>();

                    foreach (var aColumn in this.PdmGridMetaColumnList)
                    {
                        if (aColumn.SpecialColumnType == (int)EmSpecialColumnType.CurrentRefKeyDCUColumn)
                        {
                            _CurrentRefKeyDCUColumnWithoutMatrixKeyDCUColumn.Add(aColumn);
                        }
                    }
                }

                return _CurrentRefKeyDCUColumnWithoutMatrixKeyDCUColumn;
            }
        }


        private Dictionary<int, List<PdmGridMetaColumnClrDto>> _CurrentRefRegularColumnKeyAndDynamicMatrixKeyDCUColumnDepdedentColumns;

        public Dictionary<int, List<PdmGridMetaColumnClrDto>> CurrentRefRegularColumnKeyAndDynamicMatrixKeyDCUColumnDepdedentColumns
        {
            get
            {
                if (_CurrentRefRegularColumnKeyAndDynamicMatrixKeyDCUColumnDepdedentColumns == null)
                {
                    _CurrentRefRegularColumnKeyAndDynamicMatrixKeyDCUColumnDepdedentColumns = new Dictionary<int, List<PdmGridMetaColumnClrDto>>();

                    foreach (var productGridKeyColumn in CurrentRefRegularColumnKeyAndDynamicMatrixKeyDCUColumn)
                    {
                        var list = PdmGridMetaColumnList.Where(c => c.MasterDcucolumnId == productGridKeyColumn.GridColumnId && c.GridId == productGridKeyColumn.GridId).ToList();
                        if (list.Count > 0)
                        {
                            _CurrentRefRegularColumnKeyAndDynamicMatrixKeyDCUColumnDepdedentColumns.Add(productGridKeyColumn.GridColumnId, list);
                        }
                    }
                }

                return _CurrentRefRegularColumnKeyAndDynamicMatrixKeyDCUColumnDepdedentColumns;
            }
        }


        #region --- MasterEntitycolumn

        private Dictionary<int, List<PdmGridMetaColumnClrDto>> _MasterEntityDepdentColumn;

        public Dictionary<int, List<PdmGridMetaColumnClrDto>> MasterEntityDepdentColumn
        {
            get
            {
                if (_MasterEntityDepdentColumn == null)
                {
                    _MasterEntityDepdentColumn = new Dictionary<int, List<PdmGridMetaColumnClrDto>>();

                    foreach (var productGridKeyColumn in this.MasterEntityColumn)
                    {
                        var list = PdmGridMetaColumnList.Where(c => c.MasterEntityColumnId == productGridKeyColumn.GridColumnId && c.GridId == productGridKeyColumn.GridId).ToList();
                        if (list.Count > 0)
                        {
                            _MasterEntityDepdentColumn.Add(productGridKeyColumn.GridColumnId, list);
                        }
                    }
                }

                return _MasterEntityDepdentColumn;
            }
        }

        private List<PdmGridMetaColumnClrDto> _masterEntitycolumn;

        public List<PdmGridMetaColumnClrDto> MasterEntityColumn
        {
            get
            {
                if (_masterEntitycolumn == null)
                {
                    _masterEntitycolumn = new List<PdmGridMetaColumnClrDto>();

                    var masterEntityIds = PdmGridMetaColumnList.Where(col => col.MasterEntityColumnId.HasValue)
                                          .Select(col => col.MasterEntityColumnId.Value).ToList();

                    var masterEntityColumn = PdmGridMetaColumnList.Where(col => masterEntityIds.Contains(col.GridColumnId)).ToList();
                    if (masterEntityColumn.Count() > 0)
                    {
                        _masterEntitycolumn.AddRange(masterEntityColumn);
                    }
                }

                return _masterEntitycolumn;
            }
        }

        #endregion

		        
    }
}

