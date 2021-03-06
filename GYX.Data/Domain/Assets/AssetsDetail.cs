﻿using GYX.Core;
using System;
using System.Collections.Generic;
/// <summary>
/// 资产明细表
/// </summary>
namespace GYX.Data.Domain.Assets
{
    public partial class AssetsDetail
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 资产表ID
        /// </summary>
        public Guid? AssetsId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Money { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
