using System;

namespace Common.Excel
{
    /// <summary>
    /// 单元格类
    /// </summary>
    public class ExcelCell
    {
        object _value;
        private ExcelRow _owningRow;
        private ExcelColumn _owningColumn;
        
        /// <summary>
        /// 创建一个单元对象
        /// </summary>
        public ExcelCell()
        {
            _value = null;
            _owningRow = null;
            _owningColumn = null;
        }

        /// <summary>
        /// 单元格值
        /// </summary>
        public Object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// 单元格关联的列头对象，如果还没有建立关系列，则为NULL
        /// </summary>
        public ExcelColumn OwningColumn
        {
            get { return _owningColumn; }
        }

        /// <summary>

        /// 单元格关联的行对象，如果还没有建立关系行，则为NULL
        /// </summary>
        public ExcelRow OwningRow
        { get { return _owningRow; } }

        internal ExcelColumn OwningColumnInternal
        {
            set { _owningColumn = value; }
        }

        internal ExcelRow OwningRowInternal
        {
            set { _owningRow = value; }
        }

        /// <summary>
        /// 相对列位置，如果还没有建立与列的关系，则为-1
        /// </summary>
        public int ColumnIndex
        { 
            get
            {
                if (_owningColumn == null)
                    return -1;
                else
                    return _owningColumn.Index;
            } 
        }

        /// <summary>
        /// 相对行位置，如果还没有建立与行的关系，则为-1
        /// </summary>
        public int RowIndex
        { 
            get 
            {
                if (_owningRow == null)
                    return -1;
                else
                    return _owningRow.Index;
            }
        }
    }
}

