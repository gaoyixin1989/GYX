using System;
using System.Collections;

namespace Common.Excel
{
    /// <summary>
    /// 单元格集合类
    /// </summary>
    public class ExcelCellCollection : ICollection, IEnumerable
    {
        ArrayList _list;

        /// <summary>
        /// 创建一个单元格集合对象
        /// </summary>
        public ExcelCellCollection()
        {
            _list = new ArrayList();
        }

       

        /// <summary>
        /// 获取单元格对象
        /// </summary>
        /// <param name="index">序号</param>
        /// <returns>返回单元格对象</returns>
        public ExcelCell this[int index]
        {
            get { return (ExcelCell)_list[index]; }
        }

        /// <summary>
        /// 获取单元格对象
        /// </summary>
        /// <param name="DataPropertyName">数据的属性名称</param>
        /// <returns></returns>
        public ExcelCell this[string DataPropertyName]
        {
            get
            {
                foreach (ExcelCell cell in _list)
                {
                    if (cell.OwningColumn.DataPropertyName.Equals(DataPropertyName))
                    {
                        return cell;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 新增一个单元格对象
        /// </summary>
        /// <param name="cell">单元格对象</param>
        public void Add(ExcelCell cell)
        {
            _list.Add(cell);
        }

        /// <summary>
        /// 新增一个单元格对象
        /// </summary>
        /// <param name="cells">单元格集合</param>
        public void AddRange(ExcelCell[] cells)
        {
            _list.AddRange(cells);
        }

        /// <summary>
        /// 移除一个单元格
        /// </summary>
        /// <param name="cell">单元格对象</param>
        public void Remove(ExcelCell cell)
        {
            if (_list.Contains(cell))
                _list.Remove(cell);
        }

        /// <summary>
        /// 移除一个单元格
        /// </summary>
        /// <param name="index">序号</param>
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        /// <summary>
        /// 移除所有单元格对象
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }

        public void CopyTo(Array array, int index)
        {
            _list.CopyTo(array, index);
        }

        /// <summary>
        /// 包含单元格数量
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
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
