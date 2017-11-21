using GYX.Data.Domain.System;
using System;
using GYX.Core;
using System.Collections.Generic;

namespace GYX.Service.IServiceManger.System
{
    public interface ISysDictService : IBaseService<SysDict>
    {
        /// <summary>
        /// 根据父节点ID获取子节点
        /// </summary>
        /// <param name="ParentID">ParentID</param>
        /// <param name="withSelf">是否包含自身,默认false</param>
        /// <returns></returns>
        List<SysDict> GetSonByParentID(Guid ParentId, bool withSelf = false);

        /// <summary>
        /// 根据父节点ID获取所有子节点
        /// </summary>
        /// <param name="ParentID">ParentID</param>
        /// <param name="isSelf">是否包含自身,默认false</param>
        /// <returns></returns>
        List<SysDict> GetAllSonByParentID(Guid ParentId, bool withSelf = false);

        /// <summary>
        /// 根据Code获取子节点数据
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        List<SysDict> GetSonByCode(string Code);
    }
}