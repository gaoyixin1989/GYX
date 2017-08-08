using System;
using System.Collections;

namespace Common.Excel
{
    /// <summary>
    /// 列头集合类
    /// </summary>
    public class ExcelColumnCollection : ICollection, IEnumerable
    {
        private int _realCount;
        ArrayList _list;

        //public ExcelColumn[] ToArray()
        //{
        //    return (ExcelColumn[])(_list.ToArray(typeof(ExcelColumn)));
        //}

        /// <summary>
        /// 创建一个列头集合类对象
        /// </summary>
        public ExcelColumnCollection()
        {
            _list = new ArrayList();
            _realCount = 0;
        }

        /// <summary>
        /// 获取一个列头对象
        /// </summary>
        /// <param name="index">序号</param>
        /// <returns></returns>
        public ExcelColumn this[int index]
        {
            get { return (ExcelColumn)_list[index]; }
        }

        ///// <summary>
        ///// 获取一个列头对象
        ///// </summary>
        ///// <param name="dataPropertyName">数据属性名称</param>
        ///// <returns></returns>
        //public ExcelColumn this[string dataPropertyName]
        //{
        //    get
        //    {
        //        foreach (ExcelColumn column in _list)
        //        {
        //            if (column.DataPropertyName.Equals(dataPropertyName))
        //            {
        //                return column;
        //            }
        //        }
        //        return null;
        //    }
        //}

        
        private void OnIndexChanged(int index)
        {
            for (int i = index; i < _list.Count; i++)
            {
                this[i].IndexInternal = i;
            }
        }

        private void ChangeExcelCell(int index, bool isAdd)
        {
            if (index >= 0 && index < _list.Count)
            {
                if (this[0].ExcelTable != null)
                {
                    foreach (ExcelRow row in this[index].ExcelTable.Rows)
                    {
                        if (isAdd)
                        {
                            ExcelCell cell = new ExcelCell();
                            cell.OwningRowInternal = row;
                            cell.OwningColumnInternal = this[index];
                            row.Cells.Add(cell);
                        }
                        else
                            row.Cells.RemoveAt(index);
                    }
                }
            }
        }

        private void ClearAllExcelCell()
        {
            if (_list.Count > 0)
            {
                this[0].ExcelTable.Rows.Clear();
            }
        }

        /// <summary>
        /// 新增一个列头对象
        /// </summary>
        /// <param name="column">列头对象</param>
        public void Add(ExcelColumn column)
        {
            int index = _list.Count;
            _list.Add(column);
            OnIndexChanged(index);
            ChangeExcelCell(index, true);
            _realCount += column.ColumnCount;
        }
        /// <summary>
        /// 新增一个列头对象
        /// </summary>       
        /// <param name="columnName">列头名</param>
        public void Add(string columnName)
        {
            int index = _list.Count;
            ExcelColumn column = new ExcelColumn(columnName);
            _list.Add(column);
            OnIndexChanged(index);
            ChangeExcelCell(index, true);
            _realCount += column.ColumnCount;
        }

        /// <summary>
        ///  新增一个列头对象
        /// </summary>       
        /// <param name="columnName">列头名</param>
        /// <param name="dataPropertyName">列头绑定的数据属性名</param>
        public void Add(string columnName,string dataPropertyName)
        {
            int index = _list.Count;
            ExcelColumn column = new ExcelColumn(columnName, dataPropertyName);
            _list.Add(column);
            OnIndexChanged(index);
            ChangeExcelCell(index, true);
            _realCount += column.ColumnCount;
        }
        /// <summary>
        ///  新增一个列头对象
        /// </summary>       
        /// <param name="columnName">列头名</param>
        /// <param name="dataPropertyName">列头绑定的数据属性名</param>
        /// <param name="with">列宽</param>
        public void Add(string columnName, string dataPropertyName, int with)
        {
            int index = _list.Count;
            ExcelColumn column = new ExcelColumn(columnName, dataPropertyName, with);
            _list.Add(column);
            OnIndexChanged(index);
            ChangeExcelCell(index, true);
            _realCount += column.ColumnCount;
        }
        /// <summary>
        /// 新增一组列头对象
        /// </summary>
        /// <param name="columns"></param>
        public void AddRange(ExcelColumn[] columns)
        {
            int index = _list.Count;
            _list.AddRange(columns);
            OnIndexChanged(index);
            for (int i = index; i < _list.Count; i++)
            {
                ChangeExcelCell(i, true);
            }
            for (int i = 0; i < columns.Length; i++)
            {
                _realCount += columns[i].ColumnCount;
            }
        }

        public void Remove(ExcelColumn column)
        {
            if (_list.Contains(column))
            {
                int index = _list.Count;
                _list.Remove(column);
                OnIndexChanged(index);
                ChangeExcelCell(index, false);
                _realCount -= column.ColumnCount;
            }
        }

        public void RemoveAt(int index)
        {
            _realCount -= this[index].ColumnCount;
            _list.RemoveAt(index);
            OnIndexChanged(index);
            ChangeExcelCell(index, false); 
        }
        public void Clear()
        {
            _list.Clear();
            ClearAllExcelCell();
            _realCount = 0;
        }

        public bool Contains(string dataProperteyName)
        {
            foreach (ExcelColumn c in _list)
            {
                if (c.DataPropertyName.Equals(dataProperteyName))
                {
                    return true;
                }
            }
            return false;
        }

        public int IndexOf(ExcelColumn column)
        {
            return _list.IndexOf(column);
        }
        public void CopyTo(Array array, int index)
        {
            _list.CopyTo(array, index);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// 列实际在工作表中占的列数
        /// </summary>
        public int RealCount
        {
            get { return _realCount; }
        }
        
        public bool IsSynchronized
        {
            get { return _list.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return _list.SyncRoot; }
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
