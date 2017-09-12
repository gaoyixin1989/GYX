using GYX.Core;
using System;
using System.Collections.Generic;
/// <summary>
/// 信用卡取现记录
/// </summary>
namespace GYX.Data.Domain.Assets
{
    public partial class CreditCardTakeRecord
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 信用卡id
        /// </summary>
        public Guid? CardId { get; set; }
        /// <summary>
        /// 取现日期
        /// </summary>
        public DateTime? TakeDate { get; set; }
        /// <summary>
        /// 取现金额
        /// </summary>
        public decimal? TakeMoney { get; set; }
        /// <summary>
        /// 手续费
        /// </summary>
        public decimal? Fee { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否已经还款
        /// </summary>
        public bool? HasReturn { get; set; }

        /// <summary>
        /// 还款日期
        /// </summary>
        public DateTime? ReturnDate { get; set; }

        //外键
        public virtual CreditCardInfo CardObj { get; set; }//归属信用卡

    }
}
