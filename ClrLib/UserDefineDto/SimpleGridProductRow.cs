using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{
    public class SimpleGridProductRow
    {
        private Dictionary<int, object> _DictColumnCellValue = new Dictionary<int, object>();

        public SimpleGridProductRow()
        {
        }

        public Dictionary<int, object> DictColumnCellValue
        {
            get
            {
                return _DictColumnCellValue;
            }
        }

        // must Add First !
        public void Add(int columnId, object value)
        {
            if (!DictColumnCellValue.ContainsKey(columnId))
            {
                DictColumnCellValue.Add(columnId, value);
            }
        }

        public object this[int columnId]
        {
            get
            {
                if (DictColumnCellValue.ContainsKey(columnId))
                    return DictColumnCellValue[columnId];

                else
                {
                    return null;
                }
            }

            set // can update 
            {

                if (DictColumnCellValue.ContainsKey(columnId))
                {
                    DictColumnCellValue[columnId] = value;
                }

            }
        }

        public bool ContainColumnId(int columnId)
        {
            return DictColumnCellValue.ContainsKey(columnId);
        }

        public int ProductReferenceId
        {
            get;
            set;
        }


        //public int RowId
        //{
        //    get;
        //    set;
        //}

        public int Sort
        {
            get;
            set;
        }
        //
        public Guid? RowValueGuId
        {
            get;
            set;
        }


        // public  DataTable 
    }
    public class SimpleUserDefineEntityRow
    {
        private Dictionary<int, object> _DictColumnCellValue = new Dictionary<int, object>();

        public SimpleUserDefineEntityRow()
        {
        }

        private Dictionary<int, object> DictColumnCellValue
        {
            get
            {
                return _DictColumnCellValue;
            }
        }

        public void Add(int columnId, object value)
        {
            if (!DictColumnCellValue.ContainsKey(columnId))
            {
                DictColumnCellValue.Add(columnId, value);
            }
        }

        public object this[int columnId]
        {
            get
            {
                if (_DictColumnCellValue.ContainsKey(columnId))
                {
                    return _DictColumnCellValue[columnId];
                }


                else
                {
                    return null;
                }
            }
        }

        public bool ContainColumnId(int columnId)
        {
            return _DictColumnCellValue.ContainsKey(columnId);
        }

        public int RowId
        {
            get;
            set;
        }

        public int SortOrder
        {
            get;
            set;
        }

        public  int EntityId
        {
            get;
            set;
        }
    }
}

