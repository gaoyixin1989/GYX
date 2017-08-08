using System;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.IO;

namespace Common.Excel
{
    /// <summary>
    /// 导出Excel操作类，行或列的起始闰都是从0开始计算
    /// </summary>
    public class ExcelOparete
    {
        private const string LETTER = "ABCDEFGHIJKLMNOPQRSTUVWSYZ";

        private string GetLetterHeader(int index)
        {
            char[] letters = LETTER.ToCharArray();
            if (index < 26)
                return letters[index].ToString();
            else
            {
                int m = index / 26;
                int n = index % 26;
                return letters[m].ToString() + letters[n].ToString();
            }
        }

        #region 变量
        private HSSFWorkbook _workbook;
        private ISheet _currentSheet;
        //private int _sheetRowCount;
        private ExcelCellStyle _defaultExcelCellStyle;
        //private int _currentSheetIndex;
        private short _defualtColor = NPOI.HSSF.Util.HSSFColor.Black.Index;//默认颜色
        #endregion 变量

        /// <summary>
        /// 创建一个新工作表对象，默认空表格
        /// </summary>
        public ExcelOparete()
        {
            _workbook = new HSSFWorkbook();
            _currentSheet = null;
            //_sheetRowCount = 0;
            _defaultExcelCellStyle = new ExcelCellStyle();
        }

        /// <summary>
        /// 创建一个新的工作表对象，默认有一个表格
        /// </summary>
        /// <param name="sheetName">工作表格名</param>
        public ExcelOparete(string sheetName):this()
        {
            _currentSheet = _workbook.CreateSheet(sheetName);
        }

        /// <summary>
        /// 创建一个现有的工作表对象
        /// </summary>
        /// <param name="fullFileName">工作表完整路径</param>
        /// <param name="sheetName">指定当前活动表格的表名，如果为空或NULL值，则默认为第一个表格。</param>
        public ExcelOparete(string fullFileName,string sheetName)
        {
            FileStream file = new FileStream(fullFileName, FileMode.Open, FileAccess.Read);
            this._workbook = new HSSFWorkbook(file);
            file.Close();
            file.Dispose();
            if (string.IsNullOrEmpty(sheetName))
            {
                this._currentSheet = this._workbook.GetSheetAt(0);
            }
            else
                this._currentSheet = this._workbook.GetSheet(sheetName); 
        }
        /// <summary>
        /// 打开一个现有的工作表对象
        /// </summary>
        /// <param name="fullFilename"></param>
        public ExcelOparete(string fullFilename,int readType)
        {
            FileStream file = new FileStream(fullFilename, FileMode.Open, FileAccess.Read);
            this._workbook = new HSSFWorkbook(file);
            file.Close();
            file.Dispose();
            this._currentSheet = this._workbook.GetSheetAt(0);
        }

        public ExcelOparete(Stream stream)
        {
            this._workbook = new HSSFWorkbook(stream);
            this._currentSheet = this._workbook.GetSheetAt(0);
        }
        #region 属性

        public ISheet this[int index]
        {
            get { return _workbook.GetSheetAt(index); }
        }

        public ISheet this[string sheetName]
        {
            get { return _workbook.GetSheet(sheetName); }
        }

        /// <summary>
        /// 获到工作表
        /// </summary>
        public HSSFWorkbook WorkBook
        { get { return _workbook; } }

        /// <summary>
        /// 获取或设置当前活动表
        /// </summary>
        public ISheet Current
        {
            get { return _currentSheet; }
            set { _currentSheet = value; }
        }

        /// <summary>
        /// 表的行数,取行Excel的最大行数
        /// </summary>
        public int SheetRowCount
        {
            get { return this._currentSheet.PhysicalNumberOfRows; }
        }

        /// <summary>
        /// 获取或设置工作表默认的格式
        /// </summary>
        public ExcelCellStyle DefaultExcelCellStyle
        {
            get { return _defaultExcelCellStyle; }
            set { _defaultExcelCellStyle = value; }
        }

        /// <summary>
        /// 获取表格数量
        /// </summary>
        public int SheetCount
        {
            get { return _workbook.NumberOfSheets; }
        }

        /// <summary>
        /// 获取当前活动表序号
        /// </summary>
        public int CurrentSheetIndex
        {
            get { return _workbook.GetSheetIndex(_currentSheet); }     
        }

        #endregion 属性

        #region 公用方法

        /// <summary>
        /// 当表活动表转化为DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable ConvertToDataTable()
        {
            
            DataTable dt = new DataTable(this._currentSheet.SheetName);
            if (this.SheetRowCount > 0)
            {
                IRow row1 = this.GetRow(0);
                int maxColumn = row1.LastCellNum;
                for (int i = 0; i <= maxColumn; i++)
                {
                    dt.Columns.Add(GetLetterHeader(i), typeof(string));
                }
                System.Collections.IEnumerator rows = this._currentSheet.GetRowEnumerator();
                while (rows.MoveNext())
                {
                    IRow row = (HSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                         ICell cell =row.GetCell(i);
                         if (cell == null)
                             dr[i] = null;
                         else
                             dr[i] = cell.ToString();
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        /// <summary>
        /// 增加一个新的表格，并将新增的表格作为活动表
        /// </summary>
        /// <param name="sheetName">表名称</param>
        public void AddSheet(string sheetName)
        {
            _currentSheet = _workbook.CreateSheet(sheetName); 
        }

        /// <summary>
        /// 设置标题值，自动合并单元格
        /// </summary>
        /// <param name="titleString">标题文本</param>
        /// <param name="firstRowIndex">开始行位置</param>
        /// <param name="lastRowIndex">结束行的位置</param>
        /// <param name="firstColumnIndex">开始列的位置</param>
        /// <param name="lastColumnIndex">结束列的位置</param>
        /// <param name="titleExceCellStyle">单元格样式，如果为NULL，则默认（12号字体、加粗）</param>
        public void SetTitleValue(string titleString, int firstRowIndex, int lastRowIndex, int firstColumnIndex, int lastColumnIndex, ExcelCellStyle titleExceCellStyle)
        {
            if (titleExceCellStyle == null)
            {
                titleExceCellStyle = new ExcelCellStyle(12, FontBoldWeight.Bold);
                titleExceCellStyle.HorizontalAlignment = HorizontalAlignment.Center;
            }
            for (int i = firstRowIndex; i <= lastRowIndex; i++)
            {
                CreateRow(i,firstColumnIndex,lastColumnIndex,titleExceCellStyle);
            }
            IRow row = this._currentSheet.GetRow(firstRowIndex);
            
            if(lastColumnIndex>firstColumnIndex)
                this._currentSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(firstRowIndex, lastRowIndex, firstColumnIndex, lastColumnIndex));
            row.GetCell(firstColumnIndex).SetCellValue(titleString);
        }

        /// <summary>
        /// 设置标题值，自动合并单元格，默认（12号字体、加粗）
        /// </summary>
        /// <param name="titleString">标题文本</param>
        /// <param name="firstRowIndex">开始行位置</param>
        /// <param name="lastRowIndex">结束行位置</param>
        /// <param name="firstColumnIndex">开始的列位置</param>
        /// <param name="lastColumnIndex">结束的列位置</param>

        public void SetTitleValue(string titleString, int firstRowIndex, int lastRowIndex, int firstColumnIndex, int lastColumnIndex)
        {
            SetTitleValue(titleString, firstRowIndex, lastRowIndex, firstColumnIndex, lastColumnIndex, null);
        }

        /// <summary>
        /// 设置一个对象的值,自动合并列,单元格格为工作表默认的值
        /// </summary>
        /// <param name="value">对象</param>
        /// <param name="rowIndex">行位置</param>
        /// <param name="firstColumnIndex">起始列</param>
        /// <param name="lastColumnIndex">结束列</param>
        /// <param name="cellStyle">如果为NULL，则为工作表默认的值</param>
        public void SetObjectValue(object value, int rowIndex, int firstColumnIndex, int lastColumnIndex,ExcelCellStyle cellStyle)
        {
            IRow row = CreateRow(rowIndex, firstColumnIndex, lastColumnIndex, cellStyle);
            if (lastColumnIndex > firstColumnIndex)
                this._currentSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, firstColumnIndex, lastColumnIndex));
            SetCellValue(value,row,firstColumnIndex);
        }

        /// <summary>
        /// 设置一个对象的值,自动合并列,单元格格为工作表默认的值
        /// </summary>
        /// <param name="value">对象</param>
        /// <param name="rowIndex">行位置</param>
        /// <param name="firstColumnIndex">起始列</param>
        /// <param name="lastColumnIndex">结束列</param>
        public void SetObjectValue(object value, int rowIndex, int firstColumnIndex, int lastColumnIndex)
        {
            SetObjectValue(value, rowIndex, firstColumnIndex, lastColumnIndex, null);
        }

        /// <summary>
        /// 将ExcelTableRows中的单元格值装载入当前工作表中。
        /// </summary>
        /// <param name="excelTable">数据源</param>
        /// <param name="firstRowIndex">起始行号</param>
        /// <param name="firstColumnIndex">起始列号</param>
        public void SetExcelTableRows(ExcelTable excelTable,int firstRowIndex,int firstColumnIndex)
        {
            if (excelTable == null)
                throw new ArgumentNullException("excelTable参数不能为NULL.");
            SetColumnWithByColumns(excelTable.Columns, firstColumnIndex);
            int curRowIndex = firstRowIndex;
            for (int i = 0; i < excelTable.RowCount; i++)
            {
                int curColumnIndex = firstColumnIndex;
                curRowIndex = firstRowIndex +i;
                IRow row = CreateRow(curRowIndex, firstColumnIndex, excelTable.Columns.RealCount + firstColumnIndex - 1, null);
                
                foreach (ExcelCell cell in excelTable.Rows[i].Cells)
                {
                    for (int cellCount = curColumnIndex; cellCount < cell.OwningColumn.ColumnCount+curColumnIndex; cellCount++)
                    {
                        if (row.GetCell(cellCount) == null)
                            row.CreateCell(cellCount);
                        row.GetCell(cellCount).CellStyle
                            = cell.OwningColumn.DefaultExcelCellStyle == null ? this._defaultExcelCellStyle.GetCellStyle(this.WorkBook):cell.OwningColumn.DefaultExcelCellStyle .GetCellStyle(this.WorkBook);
                    }
                    if (cell.OwningColumn.ColumnCount > 1)
                    {
                        this._currentSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(curRowIndex,curRowIndex,curColumnIndex ,curColumnIndex + cell.OwningColumn.ColumnCount-1));
                    }
                    
                    SetCellValue(cell.Value,row,curColumnIndex);
                    curColumnIndex += cell.OwningColumn.ColumnCount;
                }
            }
            
        }

        /// <summary>
        /// 将ExcelColumnCollection表头值装载到当前工作表中
        /// </summary>
        /// <param name="columns">数据源</param>
        /// <param name="rowIndex">当前行号</param>
        /// <param name="firstColumnIndex">起始列号</param>
        public void SetColumnName(ExcelColumnCollection columns, int rowIndex, int firstColumnIndex)
        {
            int curColumnIndex = firstColumnIndex;
            SetColumnWithByColumns(columns, firstColumnIndex);
            IRow row = CreateRow(rowIndex, curColumnIndex, curColumnIndex + columns.RealCount - 1, null);
            for(int i =0;i<columns.Count;i++)
            {
                for (int columnIndex = curColumnIndex; columnIndex < columns[i].ColumnCount + curColumnIndex; columnIndex++)
                {
                    if (row.GetCell(columnIndex) == null)
                    {
                        row.CreateCell(columnIndex);
                    }
                    row.GetCell(columnIndex).CellStyle
                        = columns[i].ColumnExcelCellStyle == null ? this._defaultExcelCellStyle.GetCellStyle(this.WorkBook) : columns[i].ColumnExcelCellStyle.GetCellStyle(this.WorkBook);
                    int kk = this.WorkBook.NumCellStyles;
                }
                if (columns[i].ColumnCount > 1)
                {
                    this._currentSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex,rowIndex,curColumnIndex,curColumnIndex+columns[i].ColumnCount -1));
                }
                row.GetCell(curColumnIndex).SetCellValue(columns[i].ColumnName);
                curColumnIndex += columns[i].ColumnCount;
            }
        }

        /// <summary>
        /// 将DataRow[]的值装载到当前工作表中
        /// </summary>
        /// <param name="columns">数据源对应工作表中的列头集合</param>
        /// <param name="dataRows">数据源</param>
        /// <param name="rowIndex">起始行号</param>
        /// <param name="firstColumnIndex">起始列号</param>
        public void SetColumnValue(ExcelColumnCollection columns, DataRow[] dataRows, int rowIndex, int firstColumnIndex)
        {
            if (dataRows == null || dataRows.Length <= 0)
                return;
            //SetColumnWithByColumns(columns, firstColumnIndex);
            int curColumnIndex = 0;
            int pageCount = 1;
            string firstSheetName = _currentSheet.SheetName;
            for (int i = 0; i < dataRows.Length; i++, rowIndex++)
            {
                //if (i > 65000)
                //    return;
                curColumnIndex = firstColumnIndex;
                IRow row = CreateRow(rowIndex, firstColumnIndex, columns.RealCount + firstColumnIndex - 1, null);
                for (int cIndex = 0; cIndex < columns.Count; cIndex++)
                {
                    for (int columnIndex = curColumnIndex; columnIndex < columns[cIndex].ColumnCount + curColumnIndex; columnIndex++)
                    {
                        if (row.GetCell(columnIndex) == null)
                            row.CreateCell(columnIndex);
                        row.GetCell(columnIndex).CellStyle
                            = columns[cIndex].DefaultExcelCellStyle == null ? this._defaultExcelCellStyle.GetCellStyle(this.WorkBook) : columns[cIndex].DefaultExcelCellStyle.GetCellStyle(this.WorkBook);
                        int kk = this.WorkBook.NumCellStyles;
                    }
                    if (columns[cIndex].ColumnCount > 1)
                    {
                        this._currentSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, curColumnIndex, curColumnIndex + columns[cIndex].ColumnCount - 1));
                    }
                    SetCellValue(dataRows[i][columns[cIndex].DataPropertyName], row, curColumnIndex);
                    curColumnIndex += columns[cIndex].ColumnCount;
                }
                if (this._currentSheet.PhysicalNumberOfRows > 65000)
                {
                    AddSheet(firstSheetName + pageCount.ToString());

                    SetColumnName(columns, 0, firstColumnIndex);
                    pageCount += 1;
                    rowIndex = 0;
                }
            }
        }

        /// <summary>
        /// 保存工作表
        /// </summary>
        /// <param name="fileName">工作表的文件名称</param>
        /// <returns>保存后，返回工作表的完整路径</returns>
        public string SaveExcel(string fileName)
        {
            string path1 = @"C:\temp";
            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }
            string fname1 = path1 + @"\" + fileName + @".xls";
            FileStream fs1 = new FileStream(fname1, FileMode.Create);
            this.WorkBook.Write(fs1);
            fs1.Close();
            return fname1;
        }

        /// <summary>
        /// 将文件写到内存流中
        /// </summary>
        /// <param name="file"></param>

        public void WriteToMemoryStream(MemoryStream file)
        {
            this.WorkBook.Write(file);
        }

        /// <summary>
        /// 保存工作表
        /// </summary>
        /// <param name="fullFileName">工作表的完整路径+文件名</param>
        public void SaveExcelByFullFileName(string fullFileName)
        {
            FileStream fs1 = new FileStream(fullFileName, FileMode.Create);
            this.WorkBook.Write(fs1);
            
            fs1.Close();     
        }

        /// <summary>
        /// 设置列宽
        /// </summary>
        /// <param name="columnIndex">列序号</param>
        /// <param name="with">列宽</param>
        public void SetCellWith(int columnIndex, int with)
        {
            this._currentSheet.SetColumnWidth(columnIndex, with*256);
        }

        /// <summary>
        /// 设置行高
        /// </summary>
        /// <param name="rowIndex">行号</param>
        /// <param name="height">高</param>
        public void SetRowHeight(int rowIndex, short height)
        {
            this._currentSheet.GetRow(rowIndex).Height = (short)(height * 20);
        }

        /// <summary>
        /// 设置格式
        /// </summary>
        /// <param name="firstRowIndex"></param>
        /// <param name="lastRowIndex"></param>
        /// <param name="firstColumnIndex"></param>
        /// <param name="lastColumnIndex"></param>
        /// <param name="cellStyle"></param>
        public void SetRowCellStyle(int firstRowIndex, int lastRowIndex, int firstColumnIndex, int lastColumnIndex, ExcelCellStyle cellStyle)
        {
            for (int rIndex = firstRowIndex; rIndex <= lastRowIndex; rIndex++)
            {
                IRow row = GetRow(rIndex);
                for (int cIndex = firstColumnIndex; cIndex <= lastColumnIndex; cIndex++)
                {
                    row.GetCell(cIndex).CellStyle = cellStyle.GetCellStyle(this.WorkBook);
                }
            }
        }

        /// <summary>
        /// 获取行对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IRow GetRow(int index)
        {
            return _currentSheet.GetRow(index);
        }

        /// <summary>
        /// 设置当前活动表格
        /// </summary>
        /// <param name="sheetIndex"></param>
        public void SetActiveSheet(int sheetIndex)
        {
            _currentSheet = _workbook.GetSheetAt(sheetIndex);
        }
        /// <summary>
        /// 设置当前活动表
        /// </summary>
        /// <param name="sheetName"></param>
        public void SetActiveSheet(string sheetName)
        {
            _currentSheet = _workbook.GetSheet(sheetName);
        }

        #endregion 公共方法

        #region 私有方法

        /// <summary>
        /// 设置列的宽
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="firstColumnIndex"></param>
        private void SetColumnWithByColumns(ExcelColumnCollection columns, int firstColumnIndex)
        {
            if (columns == null)
                return;

            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].IsSetWith)
                {
                    for (int cellIndex = 0; cellIndex < columns[i].ColumnCount; cellIndex++)
                    {
                        this._currentSheet.SetColumnWidth(cellIndex + firstColumnIndex, columns[i].RealityWith);
                    }
                }
                firstColumnIndex += columns[i].ColumnCount;
            }
        }

        /// <summary>
        /// 更新ExcelCollection对象中的单元格格式，如果不NULL，则为当前默认值。
        /// </summary>
        /// <param name="columns"></param>
        private void UpdateExcelColumnsOfCellStycle(ExcelColumnCollection columns)
        {
            foreach (ExcelColumn column in columns)
            {
                if (column.DefaultExcelCellStyle == null)
                    column.DefaultExcelCellStyle = this._defaultExcelCellStyle;
                if (column.ColumnExcelCellStyle == null)
                    column.ColumnExcelCellStyle = this._defaultExcelCellStyle;
            }
        }
        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        private void SetCellValue(object value, IRow row, int columnIndex)
        {
            if (value != null && value != DBNull.Value)
            {
                Type valueType = value.GetType();
                if (valueType == typeof(decimal) || valueType == typeof(int) || valueType == typeof(float) || valueType == typeof(double))
                {
                    row.GetCell(columnIndex).SetCellValue(double.Parse(value.ToString()));
                }
                else if (valueType == typeof(DateTime))
                {
                    row.GetCell(columnIndex).SetCellValue(DateTime.Parse(value.ToString()));
                }
                else if (valueType == typeof(bool) || valueType == typeof(Boolean))
                {
                    row.GetCell(columnIndex).SetCellValue(bool.Parse(value.ToString()));
                }
                else
                    row.GetCell(columnIndex).SetCellValue(value.ToString());
                
            }
        }

        private IRow CreateRow(int index, int firstColumnIndex, int lastColumnIndex, ExcelCellStyle cellStyle)
        {
            IRow row = _currentSheet.GetRow(index);
            //ExcelCellStyle tempStyle = cellStyle == null ? _defaultExcelCellStyle : cellStyle;
            if (row == null)
            {

                row = this._currentSheet.CreateRow(index);
                 
                for (int i = firstColumnIndex; i <= lastColumnIndex; i++)
                {
                    row.CreateCell(i);
                    row.GetCell(i).CellStyle = cellStyle == null ? _defaultExcelCellStyle.GetCellStyle(this.WorkBook) : cellStyle.GetCellStyle(this.WorkBook);
                   
                }
            }
            else
            {
                for (int i = firstColumnIndex; i <= lastColumnIndex; i++)
                {
                    if (row.GetCell(i) == null)
                    {
                        row.CreateCell(i);
                    }
                    row.GetCell(i).CellStyle = cellStyle == null ? _defaultExcelCellStyle.GetCellStyle(this.WorkBook) : cellStyle.GetCellStyle(this.WorkBook);
                }
            }
            int kk = this.WorkBook.NumCellStyles;
            return row;
        }
        #endregion 私有方法

        #region 设置边框的方法，注意：如果设置的边框的单元格总数超过4000，则会报表超过最大样式的异常。

        /// <summary>
        /// 设置指定区域的四周边框
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderStyle"></param>
        /// <param name="color"></param>
        public void SetEnclosedBorderOfRegion(int firstRow, int lastRow, int firstColumn, int lastColumn,BorderStyle borderStyle,short color)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetEnclosedBorderOfRegion(new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, firstColumn, lastColumn), borderStyle, color);
            
        }

        /// <summary>
        /// 设置指定区域的四周边框
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderSytyle"></param>
        public void SetEnclosedBorderOfRegion(int firstRow, int lastRow, int firstColumn, int lastColumn, BorderStyle borderSytyle)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetEnclosedBorderOfRegion(new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, firstColumn, lastColumn), borderSytyle,_defualtColor );
        }

        /// <summary>
        /// 设置指定区域的下边柜
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderSytle"></param>
        /// <param name="color"></param>
        public void SetBorderBottomOfRegion(int rowIndex, int firstColumn, int lastColumn, BorderStyle borderSytle, short color)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetBorderBottomOfRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, firstColumn, lastColumn), borderSytle, color);
        }

        /// <summary>
        /// 设置指定区域的下边柜
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderSytle"></param>
        public void SetBorderBottomOfRegion(int rowIndex, int firstColumn, int lastColumn, BorderStyle borderSytle)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetBorderBottomOfRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, firstColumn, lastColumn), borderSytle, _defualtColor);
        }

        /// <summary>
        /// 设置指定区域的上边柜
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderSytle"></param>
        /// <param name="color"></param>
        public void SetBorderTopOfRegion(int rowIndex, int firstColumn, int lastColumn, BorderStyle borderSytle, short color)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetBorderTopOfRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, firstColumn, lastColumn), borderSytle, color);
        }

        /// <summary>
        /// 设置指定区域的上边柜
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderSytle"></param>
        public void SetBorderTopOfRegion(int rowIndex, int firstColumn, int lastColumn, BorderStyle borderSytle)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetBorderTopOfRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, firstColumn, lastColumn), borderSytle, _defualtColor);
        }

        /// <summary>
        /// 设置指定区域的左边柜
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="ColumnIndex"></param>
        /// <param name="borderSytle"></param>
        /// <param name="color"></param>
        public void SetBorderLeftOfRegion(int firstRow, int lastRow, int ColumnIndex, BorderStyle borderSytle, short color)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetBorderLeftOfRegion(new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, ColumnIndex, ColumnIndex), borderSytle, color);
        }

        /// <summary>
        /// 设置指定区域的左边柜
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="ColumnIndex"></param>
        /// <param name="borderSytle"></param>
        public void SetBorderLeftOfRegion(int firstRow, int lastRow, int ColumnIndex, BorderStyle borderSytle)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetBorderLeftOfRegion(new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, ColumnIndex, ColumnIndex), borderSytle, _defualtColor);
        }
        /// <summary>
        /// 设置指定区域的右边柜
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="ColumnIndex"></param>
        /// <param name="borderSytle"></param>
        /// <param name="color"></param>
        public void SetBorderRightOfRegion(int firstRow, int lastRow, int ColumnIndex, BorderStyle borderSytle, short color)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetBorderRightOfRegion(new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, ColumnIndex, ColumnIndex), borderSytle, color);
        }

        /// <summary>
        /// 设置指定区域的右边柜
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="ColumnIndex"></param>
        /// <param name="borderSytle"></param>
        /// <param name="?"></param>
        public void SetBorderRightOfRegion(int firstRow, int lastRow, int ColumnIndex, BorderStyle borderSytle)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            s.SetBorderRightOfRegion(new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, ColumnIndex, ColumnIndex), borderSytle, _defualtColor);
        }

        /// <summary>
        /// 设置指定区域的内部边框
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderStyle"></param>
        /// <param name="color"></param>
        public void SetInternalBorderOfRegion(int firstRow, int lastRow, int firstColumn, int lastColumn, BorderStyle borderStyle, short color)
        {
            HSSFSheet s = (HSSFSheet)(_currentSheet);
            for (int i = firstRow; i < lastRow; i++)//设置水平边线
            {
                s.SetBorderBottomOfRegion(new NPOI.SS.Util.CellRangeAddress(i, i, firstColumn, lastColumn), borderStyle, color);
            }
            for (int i = firstColumn; i < lastColumn; i++)//设置垂直边线
            {
                s.SetBorderRightOfRegion(new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, i, i), borderStyle, color);
            }
        }
        /// <summary>
        /// 设置指定区域的内部边框
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderStyle"></param>
        public void SetInternalBorderOfRegion(int firstRow, int lastRow, int firstColumn, int lastColumn, BorderStyle borderStyle)
        {
            SetInternalBorderOfRegion(firstRow, lastRow, firstColumn, lastColumn, borderStyle, _defualtColor);
        }
        /// <summary>
        /// 设置指定区域所有的边框
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderStyle"></param>
        public void SetAllBorderOfRegion(int firstRow, int lastRow, int firstColumn, int lastColumn, BorderStyle borderStyle)
        {
            SetEnclosedBorderOfRegion(firstRow, lastRow, firstColumn, lastColumn, borderStyle);
            SetInternalBorderOfRegion(firstRow, lastRow, firstColumn, lastColumn, borderStyle);
        }

        /// <summary>
        /// 设置指定区域所有的边框
        /// </summary>
        /// <param name="firstRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="firstColumn"></param>
        /// <param name="lastColumn"></param>
        /// <param name="borderStyle"></param>
        /// <param name="color"></param>
        public void SetAllBorderOfRegion(int firstRow, int lastRow, int firstColumn, int lastColumn, BorderStyle borderStyle,short color)
        {
            SetEnclosedBorderOfRegion(firstRow, lastRow, firstColumn, lastColumn, borderStyle, color);
            SetInternalBorderOfRegion(firstRow, lastRow, firstColumn, lastColumn, borderStyle, color);
        }
        #endregion

        #region 打印设置方法

        /// <summary>
        /// 设置当工作表所有表格的打印方向和纸张
        /// </summary>
        /// <param name="isLandscape">是否横向</param>
        /// <param name="paperSize">纸张（9=A4）</param>
        public void SetWorkbookPrint(bool isLandscape, short paperSize)
        {
            if (this._workbook != null)
            {
                if (_workbook.NumberOfSheets > 0)
                {
                    for (int i = 0; i < _workbook.NumberOfSheets; i++)
                    {
                        SetSheetPrint(_workbook.GetSheetAt(i), isLandscape, paperSize);
                    }
                }
            }
        }

        /// <summary>
        /// 设置当工作表所有表格的打印方向和纸张,默认为A4纸
        /// </summary>
        /// <param name="isLandscape">是否横向</param>
        public void SetWorkbookPrint(bool isLandscape)
        {
            SetWorkbookPrint(isLandscape, 9);
        }

        /// <summary>
        /// 设置所有工作表显示页码
        /// </summary>
        public void SetPrintPageNum()
        {
            if (this._workbook != null)
            {
                if (_workbook.NumberOfSheets > 0)
                {
                    for (int i = 0; i < _workbook.NumberOfSheets; i++)
                    {
                        PrintPageNum(_workbook.GetSheetAt(i));
                    }
                }
            }
        }

        /// <summary>
        /// 设置工作表的打印方向，纸张
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="isLandscape">是否横向</param>
        /// <param name="paperSize">纸张（9=A4纸）</param>
        private void SetSheetPrint(ISheet sheet, bool isLandscape, short paperSize)
        {
            sheet.PrintSetup.Landscape = isLandscape;
            sheet.PrintSetup.PaperSize = paperSize;
           // sheet.PrintSetup.Scale = 100;
        }

        /// <summary>
        /// 设置当工作表所有表格的打印方向,纸张默认为A4
        /// </summary>
        /// <param name="isLandscape">是否横向</param>
        public void SetSheetPrint(bool isLandscape)
        {
            _currentSheet.PrintSetup.Landscape = isLandscape;
            _currentSheet.PrintSetup.PaperSize = 9;
            //_currentSheet.PrintSetup.Scale = 100;
            
        }
        /// <summary>
        /// 设置当工作表所有表格的打印方向,纸张大小
        /// </summary>
        /// <param name="isLandscape">是否横向打印</param>
        /// <param name="paperSize">纸张大小</param>

        public void SetSheetPrint(bool isLandscape,short paperSize)
        {
            _currentSheet.PrintSetup.Landscape = isLandscape;
            _currentSheet.PrintSetup.PaperSize = paperSize;
        }
        /// <summary>
        /// 设置工作表打印显示页码,页码显示在右上角,格式：(页码：1/1)
        /// </summary>
        /// <param name="sheet"></param>
        public void PrintPageNum(ISheet sheet)
        {
            sheet.Header.Right = "页码：" + HSSFHeader.Page + @"/" + HSSFHeader.NumPages;
        }

        /// <summary>
        /// 设置页边距
        /// </summary>
        /// <param name="leftMargin"></param>
        /// <param name="topMargin"></param>
        /// <param name="rightMargin"></param>
        /// <param name="bottomMargin"></param>
        public void SetPrintMargin(double leftMargin, double topMargin, double rightMargin, double bottomMargin)
        {
            this._currentSheet.SetMargin(MarginType.LeftMargin, leftMargin);
            this._currentSheet.SetMargin(MarginType.TopMargin, topMargin);
            this._currentSheet.SetMargin(MarginType.RightMargin, rightMargin);
            this._currentSheet.SetMargin(MarginType.BottomMargin, bottomMargin);
        }

        /// <summary>
        /// 设置页眉边距
        /// </summary>
        /// <param name="headerMargin"></param>
        public void SetPrintHeaderMargin(double headerMargin)
        {
            this._currentSheet.PrintSetup.HeaderMargin = headerMargin;
        }

        /// <summary>
        /// 设置页脚边距
        /// </summary>
        /// <param name="footerMargin"></param>
        public void SetPrintFooterMarging(double footerMargin)
        {
            this._currentSheet.PrintSetup.FooterMargin = footerMargin;
        }

        /// <summary>
        /// 设置按比例打印
        /// </summary>
        /// <param name="scale"></param>
        public void SetPrintScale(short scale)
        {
            this._currentSheet.PrintSetup.FitWidth = 0;
            this._currentSheet.PrintSetup.FitHeight = 0;
            this._currentSheet.PrintSetup.Scale = scale;
        }

        /// <summary>
        /// 设置按比例打印
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="scale"></param>
        public void SetPrintScale(int sheetIndex, short scale)
        {
            ISheet s = this._workbook.GetSheetAt(sheetIndex);
            s.PrintSetup.FitWidth = 0;
            s.PrintSetup.FitHeight = 0;
            s.PrintSetup.Scale = scale;
        }
        /// <summary>
        /// 设置打印区域
        /// </summary>
        /// <param name="firstRowIndex"></param>
        /// <param name="lastRowIndex"></param>
        /// <param name="firstColumnIndex"></param>
        /// <param name="lastColumnIndex"></param>
        public void SetPrintArea(int firstRowIndex, int lastRowIndex, int firstColumnIndex, int lastColumnIndex)
        {
            this._workbook.SetPrintArea(_workbook.GetSheetIndex(_currentSheet),firstColumnIndex, lastColumnIndex, firstRowIndex,lastRowIndex );
        }
        /// <summary>
        /// 设置打印区域
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="firstRowIndex"></param>
        /// <param name="lastRowIndex"></param>
        /// <param name="firstColumnIndex"></param>
        /// <param name="lastColumnIndex"></param>
        public void SetPrintArea(int sheetIndex, int firstRowIndex, int lastRowIndex, int firstColumnIndex, int lastColumnIndex)
        {
            this._workbook.SetPrintArea(sheetIndex, firstColumnIndex, lastColumnIndex, firstRowIndex, lastRowIndex);
        }

        /// <summary>
        /// 设置重复打印区域
        /// </summary>
        /// <param name="firstRowIndex"></param>
        /// <param name="lastRowIndex"></param>
        /// <param name="firstColumnIndex"></param>
        /// <param name="lastColumnIndex"></param>
        public void SetPrintRepeatRange(int firstRowIndex, int lastRowIndex, int firstColumnIndex, int lastColumnIndex)
        {
            //this.WorkBook.SetRepeatingRowsAndColumns(_workbook.GetSheetIndex(_currentSheet), firstColumnIndex, lastColumnIndex, firstRowIndex, lastRowIndex);
        }

        /// <summary>
        /// 设置重复打印区域
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="firstRowIndex"></param>
        /// <param name="lastRowIndex"></param>
        /// <param name="firstColumnIndex"></param>
        /// <param name="lastColumnIndex"></param>
        public void SetPrintRepeatRange(int sheetIndex, int firstRowIndex, int lastRowIndex, int firstColumnIndex, int lastColumnIndex)
        {
            //HSSFSheet s = (HSSFSheet)(_currentSheet);
            // this._currentSheet.setr SetRepeatingColumns
            //this. SetRepeatingRowsAndColumns(sheetIndex, firstColumnIndex, lastColumnIndex, firstRowIndex, lastRowIndex);
        }
        #endregion 打印设置方法
    }
}
