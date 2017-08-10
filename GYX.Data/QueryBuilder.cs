using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 查询条件类
/// </summary>
namespace GYX.Data
{
    public class QueryBuilder
    {
        /// <summary>
        /// 用户信息查询条件
        /// </summary>
        public class SysUserQueryBuilder
        {
            public SysUserQueryBuilder()
            {
                this.DataState = new List<int?>() { 0 };
            }
            public string UserName { get; set; }//用户名
            public string RealName { get; set; }//真实姓名
            public List<int?> DataState { get; set; }//数据状态：0正常，1删除，默认0
            public bool? IsUse { get; set; }//是否启用
        }

        /// <summary>
        /// 信用卡信息查询条件
        /// </summary>
        public class CreditCardInfoQueryBuilder
        {
            public List<int?> UserId { get; set; }//归属用户ID
            public string CardName { get; set; }//卡名称
            public string CardNo { get; set; }//卡号

            public List<bool?> IsUse { get; set; }// 是否启用、有效
        }

        /// <summary>
        /// 信用卡取现记录查询条件
        /// </summary>
        public class CreditCardTakeRecordQueryBuilder
        {
            public List<int?> CardId { get; set; }//信用卡
            public DateTime? TakeDate_start { get; set; }//取现日期
            public DateTime? TakeDate_end { get; set; }
        }

        /// <summary>
        /// 账本记账查询条件
        /// </summary>
        public class AccountBookQueryBuilder
        {
            public AccountBookQueryBuilder()
            {
                this.DataState = new List<int?>() { 0 };
            }
            public List<int?> DataState { get; set; }//数据状态：0正常，1删除，默认0
            public List<string> BookName { get; set; }//归属账本（账本名称代号）
            public List<string> BillType { get; set; }//账单收支类型：支出、收入
            public DateTime? PayTime_start { get; set; }//支付时间
            public DateTime? PayTime_end { get; set; }//支付时间
            public List<string> BillTypeIncome { get; set; }//收入类型
            public List<string> BillTypeOutput { get; set; }//支出类型
        }

    }
}
