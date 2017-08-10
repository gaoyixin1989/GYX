using GYX.Core;
using System;
using System.Collections.Generic;
/// <summary>
/// 菜单表
/// </summary>
namespace GYX.Data.Domain.System
{
    public partial class SysMenu
    {
        /// <summary>
        /// ID编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 菜单显示名称
        /// </summary>
        public string MenuText { get; set; }
        /// <summary>
        /// 菜单URL
        /// </summary>
        public string MenuUrl { get; set; }
        /// <summary>
        /// 菜单类型：menu、菜单；item、菜单项，该值来自字典
        /// </summary>
        public string MenuType { get; set; }
        /// <summary>
        /// 父级菜单ID
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? OrderId { get; set; }
        /// <summary>
        /// 菜单小图标URL
        /// </summary>
        public string ImgUrl_Small { get; set; }
        /// <summary>
        /// 菜单大图标URL
        /// </summary>
        public string ImgUrl_Big { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public Nullable<DateTime> CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 是否显示，该值来自字典
        /// </summary>
        public bool? IsShow { get; set; }
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
