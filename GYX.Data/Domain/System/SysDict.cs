using GYX.Core;
using System;
using System.Collections.Generic;
/// <summary>
/// 字典表
/// </summary>
namespace GYX.Data.Domain.System
{
    public partial class SysDict
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 字典代号
        /// </summary>
        public string DictCode { get; set; }
        /// <summary>
        /// 字典名称
        /// </summary>
        public string DictText { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? OrderId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 是否默认值，该值来自字典
        /// </summary>
        public bool? IsDefalut { get; set; }
        /// <summary>
        /// 是否启用，该值来自字典
        /// </summary>
        public bool? IsUse { get; set; }
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
