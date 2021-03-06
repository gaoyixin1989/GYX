﻿using GYX.Data.Domain.System;
using System;
using GYX.Core;
using System.Collections.Generic;

namespace GYX.Service.IServiceManger.System
{
    public interface ISysMenuService : IBaseService<SysMenu>
    {
        /// <summary>
        /// 根据父节点ID获取子节点
        /// </summary>
        /// <param name="ParentID">ParentID</param>
        /// <param name="withSelf">是否包含自身,默认false</param>
        /// <returns></returns>
        List<SysMenu> GetSonByParentID(Guid ParentId, bool withSelf=false);

        /// <summary>
        /// 根据父节点ID获取所有子节点
        /// </summary>
        /// <param name="ParentID">ParentID</param>
        /// <param name="isSelf">是否包含自身,默认false</param>
        /// <returns></returns>
        List<SysMenu> GetAllSonByParentID(Guid ParentId, bool withSelf = false);
    }
}