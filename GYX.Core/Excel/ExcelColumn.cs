
namespace Common.Excel
{
    /// <summary>
    /// 列头信息类,
    /// 注意：是否设置宽（ExcelColumn.IsSetWith)默认值为否。
    /// </summary>
    public class ExcelColumn : ExcelElement
    {
        #region 变量
        private string _columnName;//列名
        private int _with;//列宽
        private bool _isSetWith;//是否设置宽
        //private int _columnWith;//列的宽度，_columnWith = _with*256;
        private string _dataPropertyName;//数据属性名称
        private ExcelCellStyle _defaultExcelCellStyle;//单元格样式
        private ExcelCellStyle _columnExcelCellStyle;//表头样式
        private int _columnCount;//列数

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建一个新列头对象
        /// </summary>
        private ExcelColumn()
        {
            _isSetWith = false;
            _columnCount = 1;
            _columnName = string.Empty;
            _dataPropertyName = string.Empty;
            _with = 20;
            _defaultExcelCellStyle =null;// new ExcelCellStyle();
            _columnExcelCellStyle = null;//new ExcelCellStyle();
            //_warpText = false;
            //_index = 0;
        }

        /// <summary>
        /// 创建一个新列头对象
        /// </summary>
        /// <param name="columnName">列头名称</param>
        public ExcelColumn(string columnName)
            : this()
        {
            _columnName = columnName;
        }

        /// <summary>
        /// 创建一个新列头对象
        /// </summary>
        /// <param name="columnName">列头名称</param>
        /// <param name="dataPropertyName">数据属性名称</param>
        public ExcelColumn(string columnName, string dataPropertyName)
            : this(columnName)
        {
            _dataPropertyName = dataPropertyName;
        }

        /// <summary>
        /// 创建一个新列头对象
        /// </summary>
        /// <param name="columnName">列头名称</param>
        /// <param name="dataPropertyName">数据属性名称</param>
        /// <param name="with">列宽</param>
        public ExcelColumn(string columnName, string dataPropertyName, int with)
            : this(columnName, dataPropertyName)
        {
            _with = with;
            _isSetWith = true;
        }

        /// <summary>
        ///  创建一个新列头对象
        /// </summary>
        /// <param name="columnName">列头名称</param>
        /// <param name="dataPropertyName">数据属性名称</param>
        /// <param name="with">列宽</param>
        /// <param name="columnExcelCellStyle">列头的单元格式</param>
        ///<param name="defaultExcelCellStyle">列值的单元格式</param>
        public ExcelColumn(string columnName, string dataPropertyName, int with, ExcelCellStyle columnExcelCellStyle,ExcelCellStyle defaultExcelCellStyle)
            : this(columnName, dataPropertyName, with)
        {
            _defaultExcelCellStyle = defaultExcelCellStyle ;
            _columnExcelCellStyle = columnExcelCellStyle;
        }

        /// <summary>
        /// 创建一个新列头对象
        /// </summary>
        /// <param name="columnCount">列数</param>
        /// <param name="columnName">列头名称</param>
        /// <param name="dataPropertyName">数据的属性名称</param>
        public ExcelColumn(int columnCount, string columnName, string dataPropertyName)
            : this(columnName, dataPropertyName)
        {
            this._columnCount = columnCount;
        }
        #endregion

        #region 属性

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName
        {
            get
            {
                return _columnName;
            }
            set { _columnName = value; }
        }

        /// <summary>
        /// 数据属性名称
        /// </summary>
        public string DataPropertyName
        {
            get { return _dataPropertyName; }
            set { _dataPropertyName = value; }
        }

        /// <summary>
        /// 列宽(With=1即一个字符的宽度）,且IsSetWith为true时，设置才有效
        /// </summary>
        public int With
        {
            get { return _with; }
            set
            {
                if (_with != value)
                {
                    _with = value;
                }
            }
        }

        /// <summary>
        /// 列的实际宽度
        /// </summary>
        public int RealityWith
        {
            get { return _with * 256; }
        }

        /// <summary>
        /// 单元格样式
        /// </summary>
        public ExcelCellStyle DefaultExcelCellStyle
        {
            get { return _defaultExcelCellStyle; }
            set { _defaultExcelCellStyle = value; }
        }

        /// <summary>
        /// 表头样式
        /// </summary>
        public ExcelCellStyle ColumnExcelCellStyle
        {
            get { return _columnExcelCellStyle; }
            set { _columnExcelCellStyle = value; }
        }

        /// <summary>
        /// 获取或设置列在工作表中占的列数
        /// </summary>
        public int ColumnCount
        {
            get { return _columnCount; }
            set { _columnCount = value; }
        }

        //internal ICellStyle DefaultCellStyle
        //{
        //    get { return _defaultCellStyle; }
        //    set { _defaultCellStyle = value; }
        //}

        //internal ICellStyle ColumnCellStyle
        //{
        //    get { return _columnCellStyle; }
        //    set { _columnCellStyle = value; }
        //}

        /// <summary>
        /// 获取或设置是否设置了列宽
        /// </summary>
        public bool IsSetWith
        {
            get { return _isSetWith; }
            set { _isSetWith = value; }
        }
        #endregion
    }
}
