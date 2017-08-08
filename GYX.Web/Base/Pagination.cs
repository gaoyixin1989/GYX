using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GYX.Web.Base
{
    public partial class Pagination
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int? pageIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int? pageSize { get; set; }

        /// <summary>
        /// 排序列，多表以逗号隔开
        /// </summary>
        public string sort { get; set; }

        /// <summary>
        /// asc,desc
        /// </summary>
        public string order { get; set; }


        public int Index
        {
            get { return (pageIndex ?? 0) < 1 ? 1 : pageIndex.Value; }
        }
        public int Size
        {
            get { return (pageSize ?? 0) < 1 ? int.MaxValue : pageSize.Value; }
        }
        
    }
}