using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Net;
using System.IO;
using System.Collections;

namespace GYX.Web
{
    /// <summary>
    /// 公用方法
    /// </summary>
    public class FunBase
    {
        #region 将Ajax提交到后台的数据自动绑定到对象object
        /// <summary>
        /// 将Ajax提交到后台的数据自动绑定到对象【注意：未经同意不允许修改】，by 熊卫华 2012.11.05
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="request">request对象</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T autoBindRequest<T>(HttpRequest request, T obj)
        {
            Type objType = obj.GetType();
            PropertyInfo[] objPropertiesArray = objType.GetProperties();
            foreach (PropertyInfo objProperty in objPropertiesArray)
            {
                if (request[objProperty.Name] != null)
                    objProperty.SetValue(obj, request[objProperty.Name].ToString(), null);
            }

            return obj;
        }
        #endregion

        #region 对象Object和Json的转换
        /// <summary>
        /// 对象转换为 Json
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>JSON字符串</returns>
        public static string ObjectToJson(object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            return serializer.Serialize(obj);
        }
        public object JsonToObject(string jsonString)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            return serializer.DeserializeObject(jsonString);
        }
        public T JsonToObject<T>(string jsonString)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(jsonString);
        }
        #endregion

        #region 将DataTable类型数据转成JSON数据
        /// <summary>
        /// JSon 序列化,将DataTable序列化为Json字符【注意：未经同意不允许修改】，by 熊卫华 2012.11.05
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="intRowsCount">总记录数</param>
        /// <returns></returns>
        public static string CreateToJson(DataTable dt, int intRowsCount)
        {
            if (dt == null) return "";
            List<object> list = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> diRow = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    diRow.Add(dc.ColumnName, dr[dc].ToString());
                }
                list.Add(diRow);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string result = serializer.Serialize(list);
            string json = @"{""rows"":" + result + @",""total"":""" + intRowsCount + @"""}";
            return json;
        }
        /// <summary>
        /// DataTable 转换为 Json 字符串
        /// </summary>
        /// <param name="dtSource">数据集</param>
        /// <returns>JSON 字符串</returns>
        public static string DataTableToJson(DataTable dtSource)
        {
            if (dtSource == null) return "";
            List<object> listRows = new List<object>();
            foreach (DataRow dr in dtSource.Rows)
            {
                Dictionary<string, object> diRow = new Dictionary<string, object>();
                foreach (DataColumn dc in dtSource.Columns)
                {
                    diRow.Add(dc.ColumnName, dr[dc].ToString());
                }
                listRows.Add(diRow);
            }
            return ObjectToJson(listRows);
        }
        /// <summary>
        /// DataRow 装换为 Json 字符串
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static string DataRowToJson(DataRow dr)
        {
            Dictionary<string, object> diRow = new Dictionary<string, object>();
            foreach (DataColumn dc in dr.Table.Columns)
            {
                diRow.Add(dc.ColumnName, dr[dc].ToString());
            }
            return ObjectToJson(diRow);
        }
        /// <summary>
        /// 转换为标准原生JSON 自动检索完成可识别 胡方扬 2012-11-28
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string CreateMIMUJson(DataTable dt)
        {
            StringBuilder JsonString = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonString.Append("[ ");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{ ");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            string value = "";
                            if (dt.Rows[i][j].GetType().FullName == "System.DateTime")
                            {
                                value = Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                            {
                                value = dt.Rows[i][j].ToString();
                            }

                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + value + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            string value = "";
                            if (dt.Rows[i][j].GetType().FullName == "System.DateTime")
                            {
                                value = Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                            {
                                value = dt.Rows[i][j].ToString();
                            }
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + value + "\"");
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("} ");
                    }
                    else
                    {
                        JsonString.Append("}, ");
                    }
                }
                JsonString.Append("]");

                return JsonString.ToString();
            }
            else
            {
                return "[]";
            }

        }

        #endregion

        #region 将JSON 序列化为DataTable数据 Create By 胡方扬 2013-02-01
        /// <summary>
        /// 将JSON 序列化为DataTable数据 Create By 胡方扬 2013-02-01
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string strJson)
        {
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;            //去除表名              
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));
            //获取数据               
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split(',');
                //创建表                   
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        var dc = new DataColumn();
                        string[] strCell = str.Split(':');
                        dc.ColumnName = strCell[0];
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }
                //增加内容                   
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split(':')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }
            return tb;
        }

        /// <summary>
        /// 把JSON字符串转换成DATATABLE。基于胡方扬的方法，但去除列名上的双引号
        /// create by 钟杰华
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable JSONToDataTable2(string json)
        {
            if (!string.IsNullOrEmpty(json) && json != "[]")
            {
                DataTable dt = JsonToDataTable(json);
                foreach (DataColumn dc in dt.Columns)
                {
                    dc.ColumnName = dc.ColumnName.Replace("\"", "");
                }
                return dt;
            }
            else
            {
                return new DataTable();
            }
        }
        #endregion

        #region 将集合类转换成DataTable 
        /// <summary>    
        /// 将集合类转换成DataTable    
        /// </summary>    
        /// <param name="list">集合</param>    
        /// <returns></returns>    
        public static DataTable ListToDataTable(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();

                foreach (PropertyInfo pi in propertys)
                {
                    //result.Columns.Add(pi.Name, pi.PropertyType);
                    result.Columns.Add(pi.Name);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
        #endregion

        #region 根据父子关系获取树形list
        /// <summary>
        /// 将数据转为树结构list
        /// </summary>
        /// <param name="sourceData">源数据</param>
        /// <param name="parentData">开始的父节点</param>
        /// <param name="keyAttr">主键</param>
        /// <param name="parentAttr">父级键</param>
        /// <param name="valueAttr">输出值</param>
        /// <param name="textAttr">输出文本</param>
        /// <param name="showAttributes">是否添加更多的属性值</param>
        /// <param name="attributesAttr">需要显示的属性值，没有值则默认全部</param>
        /// <returns></returns>
        public static List<object> SetDataToTree(List<object> sourceData, string keyAttr, string parentAttr, string valueAttr, string textAttr, bool showAttributes = false, string[] attributesAttr = null)
        {
            return SetDataToTree(ListToDataTable(sourceData), keyAttr, parentAttr, valueAttr, textAttr, showAttributes, attributesAttr);
        }
        /// <summary>
        /// 将数据转为树结构list
        /// </summary>
        /// <param name="sourceData">源数据</param>
        /// <param name="parentData">开始的父节点</param>
        /// <param name="keyAttr">主键</param>
        /// <param name="parentAttr">父级键</param>
        /// <param name="valueAttr">输出值</param>
        /// <param name="textAttr">输出文本</param>
        /// <param name="showAttributes">是否添加更多的属性值</param>
        /// <param name="attributesAttr">需要显示的属性值，没有值则默认全部</param>
        /// <returns></returns>
        public static List<object> SetDataToTree(DataTable sourceData, string keyAttr, string parentAttr, string valueAttr, string textAttr, bool showAttributes = false, string[] attributesAttr = null)
        {
            List<object> listResult = new List<object>();
            if (!(sourceData.Rows.Count > 0
                && !string.IsNullOrEmpty(keyAttr)
                && !string.IsNullOrEmpty(parentAttr)
                && !string.IsNullOrEmpty(valueAttr)
                && !string.IsNullOrEmpty(textAttr)
                && sourceData.Columns.Contains(keyAttr)
                && sourceData.Columns.Contains(parentAttr)
                && sourceData.Columns.Contains(valueAttr)
                && sourceData.Columns.Contains(textAttr)))
                return listResult;

            DataTable parentData = sourceData.Clone();

            //List<string> startKeyValue = new List<string>();

            DataRow[] drlist;
            List<string> listKeyValue = sourceData.AsEnumerable().Select(r => r[keyAttr].ToString()).ToList();
            drlist = sourceData.Select(string.Format("{0} is null or {0} not in ('{1}')", parentAttr, string.Join("','", listKeyValue)));
            if (drlist != null && drlist.Count() > 0)
                parentData = drlist.CopyToDataTable();

            return SetDataToTreeByParent(sourceData, parentData, keyAttr, parentAttr, valueAttr, textAttr, showAttributes, attributesAttr);
        }
        /// <summary>
        /// 将数据转为树结构list
        /// </summary>
        /// <param name="sourceData">源数据</param>
        /// <param name="parentData">开始的父节点</param>
        /// <param name="keyAttr">主键</param>
        /// <param name="parentAttr">父级键</param>
        /// <param name="valueAttr">输出值</param>
        /// <param name="textAttr">输出文本</param>
        /// <param name="showAttributes">是否添加更多的属性值</param>
        /// <param name="attributesAttr">需要显示的属性值，没有值则默认全部</param>
        /// <returns></returns>
        private static List<object> SetDataToTreeByParent(DataTable sourceData, DataTable parentData, string keyAttr, string parentAttr, string valueAttr, string textAttr, bool showAttributes = false, string[] attributesAttr = null)
        {
            List<object> listResult = new List<object>();
            List<object> listChildren = new List<object>();

            if (parentData != null)
            {
                foreach (DataRow item in parentData.Rows)
                {
                    Dictionary<string, object> itemAttr = new Dictionary<string, object>();
                    if (showAttributes)
                    {
                        itemAttr = DataRowToDict(item);
                        if (attributesAttr != null && attributesAttr.Length > 0)
                            itemAttr = itemAttr.Where(u => attributesAttr.Contains(u.Key)).ToDictionary(u => u.Key, u => u.Value);
                    }

                    DataTable nextParentData = null;
                    DataRow[] temp = sourceData.Select(string.Format("{0}='{1}'", parentAttr, item[keyAttr].ToString()));
                    if (temp != null && temp.Count() > 0)
                        nextParentData = temp.CopyToDataTable();
                    listChildren = SetDataToTreeByParent(sourceData, nextParentData, keyAttr, parentAttr, valueAttr, textAttr, showAttributes, attributesAttr);
                    if (listChildren.Count > 0)
                        listResult.Add(new { id = item[valueAttr], text = item[textAttr], attributes = itemAttr, children = listChildren });//attributes = item,
                    else
                        listResult.Add(new { id = item[valueAttr], text = item[textAttr], attributes = itemAttr });//attributes = item 
                }
            }

            return listResult;
        }

        /// <summary>
        /// 将DataRow数据转换为Dictionary格式
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <returns></returns>
        public static Dictionary<string, object> DataRowToDict(DataRow dr)
        {
            Dictionary<string, object> dictRow = new Dictionary<string, object>();
            foreach (DataColumn dc in dr.Table.Columns)
            {
                dictRow.Add(dc.ColumnName, dr[dc]);
            }
            return dictRow;
        }
        #endregion

        #region Cookies 设置  唐锋 2015-08-03

        /// <summary>
        /// Cookies赋值
        /// </summary>
        /// <param name="strName">主键</param>
        /// <param name="strValue">键值</param>
        /// <param name="strDay">有效天数</param>
        /// <returns>true / false</returns>
        public static bool setCookie(string strName, string strValue, DateTime ExpTime)
        {
            try
            {
                HttpCookie Cookie = new HttpCookie(strName);
                //Cookie.Domain = ".xxx.com";//当要跨域名访问的时候,给cookie指定域名即可,格式为.xxx.com
                //Cookie.Expires = DateTime.Now.AddDays(strDay);
                Cookie.Expires = ExpTime;
                Cookie.Value = strValue;
                HttpContext.Current.Response.Cookies.Add(Cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 读取Cookies
        /// </summary>
        /// <param name="strName">主键</param>
        /// <returns>string / null</returns>

        public static string getCookie(string strName)
        {
            HttpCookie Cookie = System.Web.HttpContext.Current.Request.Cookies[strName];
            if (Cookie != null)
            {
                return Cookie.Value.ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 删除Cookies
        /// </summary>
        /// <param name="strName">主键</param>
        /// <returns>true / false</returns>
        public static bool delCookie(string strName)
        {
            try
            {
                HttpCookie Cookie = new HttpCookie(strName);
                //Cookie.Domain = ".xxx.com";//当要跨域名访问的时候,给cookie指定域名即可,格式为.xxx.com
                Cookie.Expires = DateTime.Now.AddDays(-1);
                System.Web.HttpContext.Current.Response.Cookies.Add(Cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 获取远程网页内容  唐锋 2015-08-12
        /// <summary>
        /// 获取远程网页内容
        /// </summary>
        /// <param name="Url">页面地址</param>
        /// <param name="Encoding">编码：GB2312、UTF-8</param>
        /// <returns></returns>
        public static string GetWebContent(string Url, string coding)
        {
            string strWebContent = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                //声明一个HttpWebRequest请求    
                request.Timeout = 30000;
                //设置连接超时时间
                request.Headers.Set("Pragma", "no-cache");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                Encoding encoding = Encoding.GetEncoding(coding);
                StreamReader streamReader = new StreamReader(streamReceive, encoding);
                strWebContent = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return strWebContent;
        }
        #endregion

        #region 将对象值填充至对象    唐锋   2015-08-16
        /// <summary>
        /// 将对象的值填充至对象
        /// </summary>
        /// <param name="obj">数据对象</param>
        /// <param name="dataObj">转值至对象</param>
        public static object BindObjectToObject(object obj, object dataObj)
        {
            return AutoBinder.BindObjectToObjectWithNoNullValues(obj, dataObj, ConstValues.SpecialCharacter.EmptyValuesFillChar);
        }
        #endregion

        #region DataTable转换成实体类  唐锋  2015-08-16

        /// <summary>
        /// DataTable 转换为 实体类对象实例
        /// </summary>
        /// <typeparam name="T">对象名</typeparam>
        /// <param name="dt">数据表</param>
        /// <returns>List<User> userList = TableToEntity<User>(YourDataTable);</returns>
        public static List<T> TableToEntity<T>(DataTable dt) where T : class, new()
        {
            Type type = typeof(T);
            List<T> list = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                PropertyInfo[] pArray = type.GetProperties();
                T entity = new T();
                foreach (PropertyInfo p in pArray)
                {
                    //判断DataRow 列是否存在
                    if (row.Table.Columns.Contains(p.Name))
                    {
                        object objValue = row[p.Name];
                        string strValue = "";
                        if (objValue is Int64)
                        {
                            p.SetValue(entity, Convert.ToInt32(objValue), null);
                            continue;
                        }
                        if (objValue != null)
                        {
                            strValue = objValue.ToString();
                            if (string.IsNullOrEmpty(strValue))
                                strValue = ConstValues.SpecialCharacter.EmptyValuesFillChar;
                        }
                        else
                        {
                            strValue = ConstValues.SpecialCharacter.EmptyValuesFillChar;
                        }
                        p.SetValue(entity, strValue, null);
                    }
                }
                list.Add(entity);
            }
            return list;
        }
        #endregion

        #region 判断对象修改后于原数据对比返回 DataTable(KEYNAME,VALUE,NEWVALUE)    2015-08-16  唐锋
        /// <summary>
        /// 判断对象修改后于原数据对比返回 DataTable(KEYNAME,VALUE,NEWVALUE)    2015-08-16  唐锋
        /// </summary>
        /// <param name="obj">原对象数据</param>
        /// <param name="newObj">更新后对象数据</param>
        /// <param name="ArrayParams">不需要对比属性</param>
        public static DataTable ObjectCompareObjectReturnDataTable(object obj, object newObj, params string[] ArrayParams)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("KEYNAME", Type.GetType("System.String"));
            dt.Columns.Add("VALUE", Type.GetType("System.String"));
            dt.Columns.Add("NEWVALUE", Type.GetType("System.String"));
            if (obj == null && newObj == null)
            {
                return null;
            }
            Type dataObjype = newObj.GetType();
            PropertyInfo[] dataObjypePropertiesArray = dataObjype.GetProperties();
            foreach (PropertyInfo dataObjProperty in dataObjypePropertiesArray)
            {
                //判断是否需要对比
                for (int i = 0; i < ArrayParams.Length; i++)
                {
                    if (dataObjProperty.Name.ToUpper().Trim() == ArrayParams[i].ToUpper().Trim())
                    {
                        break;
                    }
                }

                object objKeyValue = AutoBinder.GetPropertyValue(obj, dataObjProperty.Name);
                object newObjKeyValue = AutoBinder.GetPropertyValue(newObj, dataObjProperty.Name);
                objKeyValue = objKeyValue == null ? "" : objKeyValue;
                //newObjKeyValue = newObjKeyValue == null ? "" : newObjKeyValue;
                //等于 ### 的字段是不需要更新
                if (newObjKeyValue != null)
                {
                    if (newObjKeyValue.ToString() != "")
                    {
                        objKeyValue = string.IsNullOrEmpty(objKeyValue.ToString()) ? ConstValues.SpecialCharacter.EmptyValuesFillChar : objKeyValue;
                        if (objKeyValue.ToString() != newObjKeyValue.ToString())
                        {
                            dt.Rows.Add(new object[] { dataObjProperty.Name, objKeyValue, newObjKeyValue });
                        }
                    }
                }
            }
            return dt;
        }
        #endregion




    }
}