using GYX.Core;
using System;
using System.Collections.Generic;
/// <summary>
/// 资产表
/// </summary>
namespace GYX.Data.Domain.Assets
{
    public partial class AssetsTable
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 总额
        /// </summary>
        public decimal? Total { get; set; }
        /// <summary>
        /// 统计日期
        /// </summary>
        public DateTime? StatisticsDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 数据状态：0正常，1删除
        /// </summary>
        public int? DataState { get; set; }

        //外键
        public virtual List<AssetsDetail> DetailList { get; set; }

    }
}
