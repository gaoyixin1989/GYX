using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GYX.Helpers
{
    public class PageHelper
    {
        /// <summary>
        /// 页面分页
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pagesize">页的大小</param>
        /// <param name="count">总数</param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string getPageHtml(int page, int pagesize, int count, string url)
        {
            if (CheckUrl(url))   //调用CheckUrl
                url += "&";
            int s = count / pagesize;
            int pages = s * pagesize < count ? ++s : s;
            StringBuilder html = new StringBuilder();

            html.Append("<script type=\"text/javascript\">");
            html.Append("function Check(page){if(page<=1)page=1;if(page>=" + pages + ")page=" + pages + ";return page;}");
            html.Append("function CheckFun(){document.formup.goPage.value=Check(document.formup.goPage.value);return true;}");
            html.Append("function GoPage(page) {document.formup.goPage.value = Check(page);document.formup.submit();}");
            html.Append("</script>");
            //html.Append("<form id=\"formup\" name=\"formup\" method=\"post\" action=\"" + url + "\"><table class=\"pageer\">");
            html.Append("<form id=\"formup\" name=\"formup\" method=\"post\" action=\"" + url + "\" onsubmit=\"return CheckFun()\"><table class=\"pageer\">");
            html.Append("<tr><td clss=\"pageli\">");
            html.Append("<a href=\"#\" onclick=\"GoPage(1)\">首页</a> ");
            html.Append("<a href=\"#\"id=\"LastPage\"  onclick=\"GoPage(" + (page - 1) + ")\">上一页</a> ");
            html.Append("<a href=\"#\"id=\"NextPage\" onclick=\"GoPage(" + (page + 1) + ")\">下一页</a> ");
            html.Append("<a href=\"#\" onclick=\"GoPage(" + pages + ")\">尾页</a> ");
            html.Append("　页码:<span id=\"Span1\">" + page + "</span>/" + pages + " 共:" + count + "条　");
            html.Append("转到第 <input type=\"text\" id=\"goPage\" name=\"goPage\" value=" + page + " size=\"3\" maxlength=\"6\" /> 页");
            html.Append("　<input type=\"submit\" id=\"Submit1\" value=\"确定\" /> ");
            html.Append("</td></tr></table></form>");
            return html.ToString();

        }
        /// <summary>
        /// 判断url是否带参数，是返回true
        /// </summary>
        /// <param name="url"></param>
        /// <returns>带参数返回true，否则返回false</returns>
        public static bool CheckUrl(string url)
        {
            char[] charArray = url.ToCharArray();
            foreach (var i in charArray)
            {
                if (i == '?') return true;
            }

            return false;
        }

        public static void CheckPage(string str, out int page)
        {
            int a;
            if (!Int32.TryParse(str, out a))  //page为空，a便为0；
            {
                page = 1;
            }
            else
            {
                page = a;
            }
        }
    }
}
