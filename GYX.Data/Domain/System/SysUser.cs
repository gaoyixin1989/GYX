using GYX.Core;
using System;
using System.Collections.Generic;
/// <summary>
/// 用户表
/// </summary>
namespace GYX.Data.Domain.System
{
    public partial class SysUser
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 数据状态：0正常，1删除
        /// </summary>
        public int? DataState { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsUse { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

    }
}
