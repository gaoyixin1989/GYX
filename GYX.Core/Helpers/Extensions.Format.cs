using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYX.Core.Helpers
{
    /// <summary>
    /// 验证扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTime value, string strFormat = "yyyy-MM-dd HH:mm:ss")
        {
            return value.ToString(strFormat);
        }

        public static string ToDateString(this DateTime? value, string strFormat = "yyyy-MM-dd HH:mm:ss")
        {
            return value.HasValue ? value.Value.ToString(strFormat) : null;
        }
    }
}
