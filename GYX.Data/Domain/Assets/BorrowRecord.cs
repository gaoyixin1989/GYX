using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 借还记录
/// </summary>
namespace GYX.Data.Domain.Assets
{
    public partial class BorrowRecord
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
        /// 借款类型：借出、借入
        /// </summary>
        public string BorrowType { get; set; }
        /// <summary>
        /// 借款日期
        /// </summary>
        public DateTime? BorrowDate { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Money { get; set; }
        /// <summary>
        /// 是否归还
        /// </summary>
        public bool? HasReturn { get; set; }
        /// <summary>
        /// 归还日期
        /// </summary>
        public DateTime? ReturnDate { get; set; }
        public string Remark { get; set; }

    }
}
