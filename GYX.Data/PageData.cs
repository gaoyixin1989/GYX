using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYX.Data
{
    public class PageList<T>
    {
        public PageList(int _count, T _items)
        {
            this.count = _count;
            this.items = _items;
        }
        public int count { get; set; }
        public T items { get; set; }
    }

    public class Log
    {
        public int uid { get; set; }
        public string opStartTime { get; set; }
        public string portalID { get; set; }
        public string description { get; set; }
        public string exceptionName { get; set; }
        public string operationName { get; set; }
    }
}
