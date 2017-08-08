using System;

namespace Common.Excel
{
    /// <summary>
    /// 表格类
    /// </summary>

    public class ExcelTable
    {
        ExcelColumnCollection _columns;
        ExcelRowCollection _rows;

        /// <summary>
        /// 创建一个新的表格
        /// </summary>
        public ExcelTable()
        {
            _columns = new ExcelColumnCollection();
            _rows = new ExcelRowCollection();
        }

        /// <summary>
        /// 获取一个单元格
        /// </summary>
        /// <param name="rowIndex">行序号</param>
        /// <param name="columnIndex">列序号</param>
        /// <returns></returns>
        public object this[int rowIndex,int columnIndex]
        {
            get{return this.Rows[rowIndex].Cells[columnIndex].Value;}
        }

        //public object this[int rowIndex, string datapropertyName]
        //{
        //    get { return this.Rows[rowIndex].Cells[datapropertyName].Value; }
        //}

        /// <summary>
        /// 列头集合对象
        /// </summary>

        public ExcelColumnCollection Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// 行集合对象
        /// </summary>

        public ExcelRowCollection Rows
        {
            get { return _rows; }
        }

        /// <summary>
        /// 创建一个新的行对象
        /// </summary>
        /// <returns></returns>
        public ExcelRow NewRow()
        {
            ExcelRow row = new ExcelRow();
            foreach (ExcelColumn column in _columns)
            {
                ExcelCell cell = new ExcelCell();
                cell.OwningColumnInternal = column;
                cell.OwningRowInternal = row;
                row.Cells.Add(cell);
            }
            return row;
        }

        /// <summary>
        /// 新增一个行对象
        /// </summary>
        /// <param name="values">行中的单元格的值</param>

        public void AddRow(object[] values)
        {
            if (values.Length != Columns.Count)
            {
                throw new ArgumentOutOfRangeException("values的长度与列数不相等。");
            }
            ExcelRow row = new ExcelRow();
            for (int i = 0; i < Columns.Count; i++)
            {
                ExcelCell cell = new ExcelCell();
                cell.OwningColumnInternal = Columns[i];
                cell.Value = values[i];
                cell.OwningRowInternal = row;
                row.Cells.Add(cell);
            }
            Rows.Add(row);
        }

        public int RowCount
        {
            get 
            {
                return _rows.Count; 
            }
        }
    }

}