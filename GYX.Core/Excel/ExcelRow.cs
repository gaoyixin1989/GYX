using System;

namespace Common.Excel
{
    /// <summary>
    /// 表格列类，构造函数修饰为interal，外部要通过ExcelTable.NewRow()去创建表格列对象。
    /// </summary>
    public class ExcelRow : ExcelElement
    {
        private ExcelCellCollection _cells;

        /// <summary>
        /// 创建一个新的表格列对象,内部使用，外部要通过ExcelTable.NewRow()去创建表格列对象。
        /// </summary>
        internal ExcelRow()
        {
            _cells = new ExcelCellCollection();
        }

        /// <summary>
        /// 设置表格列中的单元格集合的值
        /// </summary>
        /// <param name="values">值的集合，值的顺序要与单元格集合的顺序对应</param>
        public void SetRowValue(object[] values)
        {
            if (values.Length != _cells.Count)
            {
                throw new ArgumentException("values的列數不對.");
            }
            for (int i = 0; i < _cells.Count; i++)
            {
                _cells[i].Value = values[i];
            }
        }

        /// <summary>
        /// 获取单元格集合对象
        /// </summary>
        public ExcelCellCollection Cells
        {
            get { return _cells; }
        }

        internal ExcelCellCollection CellsInternal
        {
            set
            {
                _cells = value;
            }
        }

        protected override void OnExcelTableChanged()
        {
            base.OnExcelTableChanged();
        }
    }
}
