using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYX.Data.Domain.Assets
{
    public partial class HumanContact
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 人
        /// </summary>
        public string Person { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Money { get; set; }
        /// <summary>
        /// 事件
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? EventDate { get; set; }
        public string Remark { get; set; }
    }
}
