using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace GYX.Helpers
{
    public class HtmHelper
    {
        public static string HtmlEncode(string str)
        {
            if (str == null || str == "")
                return "";
            str.Replace("<", "<");
            str.Replace(">", ">");
            str.Replace(" ", "&nbsp;");
            str.Replace("　", "&nbsp;&nbsp;");
            str.Replace("/'", "'");
            str.Replace("/n", "<br/>");
            return str;
        }


        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="Htmlstring">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Htmlstring.Replace("\r\n", "");
            //Htmlstring = Regex.Replace(Htmlstring, @"<script.*?</script>", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"<style.*?</style>", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"<.*?>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>[\s\S]*?<\/script>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<style[^>]*?>[\s\S]*?<\/style>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<(.|\n)+?>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Htmlstring.Replace("<", "");
            Htmlstring = Htmlstring.Replace(">", "");
            Htmlstring = Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }

        /// <summary>
        /// 获取外部网站页面内容
        /// </summary>
        /// <param name="Url">页面地址</param>
        /// <param name="strEncoding">页面编码 utf-8,GB2312</param>
        /// <returns></returns>
        public static string GetWebContent(string Url, string strEncoding)
        {
            string strResult = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                //声明一个HttpWebRequest请求    
                request.Timeout = 30000;
                //设置连接超时时间    
                request.Headers.Set("Pragma", "no-cache");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                Encoding encoding = Encoding.GetEncoding(strEncoding);
                StreamReader streamReader = new StreamReader(streamReceive, encoding);
                strResult = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return strResult;
        }

        /// <summary>
        /// 获取符合条件的第一笔数据
        /// </summary>
        /// <param name="strInfo">需要进行查找的字符串</param>
        /// <param name="stMark">开始标识</param>
        /// <param name="edMark">结束标识</param>
        /// <param name="isSelf">是否包含开始结束标识</param>
        /// <returns>返回结果</returns>
        public static string GetFirstString(string strInfo, string stMark, string edMark, bool isSelf)
        {
            string strResult = "";
            int startIndex = strInfo.IndexOf(stMark);
            if (startIndex > -1)
            {
                strInfo = strInfo.Substring(startIndex + stMark.Length);
                int endIndex = strInfo.IndexOf(edMark);
                if (endIndex > -1)
                {
                    strInfo = strInfo.Substring(0, endIndex);
                    if (isSelf)
                        strResult = stMark + strInfo + edMark;
                }
            }
            return strResult;
        }

        /// <summary>
        /// 获取符合条件的所有最小数据
        /// </summary>
        /// <param name="strInfo">需要进行查找的字符串</param>
        /// <param name="stMark">开始标识</param>
        /// <param name="edMark">结束标识</param>
        /// <param name="isSelf">是否包含开始结束标识</param>
        /// <returns>开始与结束标识之间的内容</returns>
        public static List<string> GetStringList(string strInfo, string stMark, string edMark, bool isSelf)
        {
            CompareInfo Compare = CultureInfo.InvariantCulture.CompareInfo;
            List<string> strList = new List<string>();
            int index = 0;
            int endIndex = 0;
            while (index > -1 && strInfo.Length > 0)
            {
                index = Compare.IndexOf(strInfo, stMark, CompareOptions.IgnoreCase);//忽略大小写
                if (index > -1)
                {
                    if (isSelf)
                    {
                        strInfo = strInfo.Substring(index);
                        endIndex = Compare.IndexOf(strInfo, edMark, stMark.Length, CompareOptions.IgnoreCase);//忽略大小写
                    }
                    else
                    {
                        strInfo = strInfo.Substring(index + stMark.Length);
                        endIndex = Compare.IndexOf(strInfo, edMark, CompareOptions.IgnoreCase);//忽略大小写
                    }
                    if (endIndex > -1)
                    {
                        if (isSelf)
                            strList.Add(strInfo.Substring(0, endIndex + edMark.Length));
                        else
                            strList.Add(strInfo.Substring(0, endIndex));
                        strInfo = strInfo.Substring(endIndex + edMark.Length);
                    }
                    index = endIndex;
                }
            }
            return strList;
        }
    }
}
