using GYX.Core;
using System;
using System.Collections.Generic;
/// <summary>
/// 账本表
/// </summary>
namespace GYX.Data.Domain.Assets
{
    public partial class AccountBook
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 归属账本（账本名称代号）
        /// </summary>
        public string BookName { get; set; }
        /// <summary>
        /// 账单收支类型：支出、收入
        /// </summary>
        public string BillType { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { get; set; }
        /// <summary>
        /// 收入类型
        /// </summary>
        public string BillTypeIncome { get; set; }
        /// <summary>
        /// 支出类型
        /// </summary>
        public string BillTypeOutput { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Money { get; set; }
        /// <summary>
        /// 货币类型
        /// </summary>
        public string CurrencyType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 数据状态：0正常，1删除
        /// </summary>
        public int? DataState { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
