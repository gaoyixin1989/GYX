using GYX.Core;
using GYX.Data.Domain.System;
using System;
using System.Collections.Generic;
/// <summary>
/// 信用卡信息表
/// </summary>
namespace GYX.Data.Domain.Assets
{
    public partial class CreditCardInfo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 归属用户ID
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 卡名称
        /// </summary>
        public string CardName { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 额度
        /// </summary>
        public decimal? LimitMoney { get; set; }
        /// <summary>
        /// 账单日
        /// </summary>
        public int? BillDay { get; set; }

        /// <summary>
        /// 还款日
        /// </summary>
        public int? RepaymentDay { get; set; }

        /// <summary>
        /// 是否启用、有效
        /// </summary>
        public bool? IsUse { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        //外键
        public virtual SysUser UserObj { get; set; }//归属用户
    }
}
