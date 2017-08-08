
namespace Common.Excel
{
    /// <summary>
    /// 单元格的元素类，被ExcelColumn、ExcelRow类继承
    /// </summary>
    public class ExcelElement
    {
        private ExcelTable _table;
        private int _index = -1;
        // private bool _isRow = false;

        /// <summary>
        /// 创建一个工作表元素对象
        /// </summary>
        public ExcelElement()
        {
            _table = null;
        }
        /// <summary>
        /// 获取工作表元素对象所属的工作表表格
        /// </summary>
        public ExcelTable ExcelTable
        {
            get { return _table; }
        }

        internal ExcelTable ExcelTableInternal
        {
            set
            {
                if (_table != value)
                {
                    _table = value;
                    OnExcelTableChanged();
                }
            }
        }

        /// <summary>
        /// 获取行或列的序号，如果没有关系工作表表格，则为-1
        /// </summary>
        public int Index
        {
            get
            {
                return _index;
            }
        }

        internal int IndexInternal
        {
            set { _index = value; }
        }

        protected virtual void OnExcelTableChanged()
        {

        }
    }
}
