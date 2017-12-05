using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GYX.Core;
using GYX.Core.Caching;
using GYX.Data;
using GYX.Data.Domain.System;
using System.Linq.Expressions;
using GYX.Service.IServiceManger.System;

namespace GYX.Service.ServiceManger.System
{
    public partial class SysDictService : BaseService<SysDict>, ISysDictService
    {
        /// <summary>
        /// 根据父节点ID获取子节点
        /// </summary>
        /// <param name="ParentID">ParentID</param>
        /// <param name="withSelf">是否包含自身,默认false</param>
        /// <returns></returns>
        public List<SysDict> GetSonByParentID(Guid ParentId, bool withSelf = false)
        {
            if (withSelf)
                return this._entityStore.Table.Where(u => (u.ParentId == ParentId || u.Id == ParentId) && (u.DataState ?? 0) != 1).OrderBy(u => u.OrderId).ToList();
            else
                return this._entityStore.Table.Where(u => u.ParentId == ParentId && (u.DataState ?? 0) != 1).OrderBy(u => u.OrderId).ToList();

        }

        /// <summary>
        /// 根据父节点ID获取所有子节点
        /// </summary>
        /// <param name="ParentID">ParentID</param>
        /// <param name="isSelf">是否包含自身,默认false</param>
        /// <returns></returns>
        public List<SysDict> GetAllSonByParentID(Guid ParentId, bool withSelf = false)
        {
            List<SysDict> ResultList = new List<SysDict>();
            List<SysDict> firstSonList = new List<SysDict>();
            if (withSelf)
                firstSonList = this._entityStore.Table.Where(u => u.Id == ParentId && (u.DataState ?? 0) != 1).ToList();
            else
                firstSonList = this._entityStore.Table.Where(u => u.ParentId == ParentId && (u.DataState ?? 0) != 1).ToList();

            ResultList.AddRange(firstSonList);
            foreach (var item in firstSonList)
            {
                ResultList.AddRange(GetAllSonByParentID(item.Id, false));
            }

            return ResultList;
        }

        /// <summary>
        /// 根据Code获取子节点数据
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public List<SysDict> GetSonByCode(string Code)
        {
            var result = this._entityStore.Table.Where(u => u.Parent.DictCode == Code && (u.DataState ?? 0) != 1 && u.IsUse == true).ToList();
            return result;
        }
    }
}