using GYX.Data.Domain.System;
using GYX.Service.IServiceManger.System;
using GYX.Service.ServiceManger.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 菜单
/// </summary>
namespace GYX.Web.Areas.System.Controllers
{
    public class SysMenuController : BaseController
    {
        ISysMenuService _menuService = new SysMenuService();
        // GET: SysMenu
        public ActionResult Index()
        {
            return View();
        }

        #region 查询数据
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetById(int id)
        {
            var obj = _menuService.FindById(id);
            return BackData(obj);
        }

        /// <summary>
        /// 获取菜单数据：easyui-tree数据格式
        /// </summary>
        /// <returns>easyui-tree数据格式json</returns>
        public ActionResult GetTreeData(bool? IsUse = null, bool? IsShow = null)
        {
            List<object> listResult = new List<object>();
            var condition = new
            {
                DataState = 0,
                IsUse = IsUse,
                IsShow = IsShow
            };//查询条件
            int intTotal = 0;
            List<SysMenu> listData = _menuService.GetForPaging(out intTotal, condition).Select(u => (SysMenu)u).ToList();
            listData = listData.OrderBy(u => u.OrderId).ToList();

            //listResult = FunBase.SetDataToTree(listData.ToList<object>(), "Id", "ParentId", "Id", "MenuText"
            //    , true, new string[] { });
            listResult = SetDataIntoTree(listData, null);
            return BackData(listResult);

        }

        /// <summary>
        /// 将数据转换为树结构
        /// </summary>
        /// <param name="source">源数据</param>
        /// <param name="rootData">更节点（为null时自动取没有父节点的数据作为根节点）</param>
        /// <returns></returns>
        private List<object> SetDataIntoTree(List<SysMenu> source, List<SysMenu> rootData)
        {
            List<object> listResult = new List<object>();
            List<object> listChildren = new List<object>();
            if (source == null || source.Count < 1)
                return listResult;
            if (rootData == null)
            {
                List<Guid> listIds = source.Select(u => u.Id).ToList();
                rootData = source.Where(u => (!u.ParentId.HasValue) || (!listIds.Contains(u.ParentId.Value))).ToList();
            }
            foreach (var item in rootData)
            {
                listChildren = SetDataIntoTree(source, source.Where(u => (u.ParentId ?? Guid.Empty) == item.Id).ToList());
                if (listChildren.Count > 0)
                    listResult.Add(new
                    {
                        id = item.Id,
                        text = item.MenuText,
                        state = "closed",
                        attributes = item,
                        children = listChildren
                    });
                else
                    listResult.Add(new
                    {
                        id = item.Id,
                        text = item.MenuText,
                        attributes = item
                    });

            }

            return listResult;
        }

        #endregion

        #region 编辑数据
        /// <summary>
        /// 保存数据前设置对象的默认值
        /// </summary>
        private void SetDefaultDataBeforeSave(SysMenu model)
        {
            _menuService.TrimObj(model);
            model.CreateTime = model.CreateTime ?? DateTime.Now;
            model.UpdateTime = model.UpdateTime ?? DateTime.Now;
            model.MenuType = string.IsNullOrEmpty(model.MenuType) ? "menu" : model.MenuType;
            model.DataState = model.DataState ?? 0;
            model.IsShow = model.IsShow ?? true;
            model.IsUse = model.IsUse ?? true;
            model.ParentId = model.ParentId ?? null;
            if (!model.OrderId.HasValue)
            {
                var sonObjs = _menuService.List().Where(u => u.ParentId == model.ParentId).Select(u => u.OrderId);
                if (sonObjs.Count() > 0)
                    model.OrderId = (sonObjs.Max() ?? 0) + 1;
                else
                    model.OrderId = 1;

            }

        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public JsonResult Save(SysMenu model)
        {
            if (model.Id == Guid.Empty)
                return Create(model);
            else
                return Update(model);
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        private JsonResult Create(SysMenu model)
        {
            SetDefaultDataBeforeSave(model);
            bool boolState = _menuService.Insert(model);
            return BackData(new { Result = boolState });
        }
        /// <summary>
        /// 修改菜单
        /// </summary>
        private JsonResult Update(SysMenu model)
        {
            var oldData = _menuService.FindById(model.Id);
            oldData.ParentId = model.ParentId;
            oldData.MenuType = model.MenuType;
            oldData.MenuText = model.MenuText;
            oldData.MenuUrl = model.MenuUrl;
            oldData.IsUse = model.IsUse;
            oldData.IsShow = model.IsShow;
            oldData.Remark = model.Remark;
            model = oldData;

            SetDefaultDataBeforeSave(model);
            bool boolState = _menuService.Update(model);

            return BackData(new { Result = boolState });
        }
        /// <summary>
        /// 删除菜单_软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="withSon">是否包含子元素,默认不包含</param>
        /// <returns></returns>
        public JsonResult Delete(Guid[] ids, bool withSon = false)
        {
            bool boolResult = false;
            int successCount = 0;
            int errorCount = 0;
            var objList = _menuService.List().Where(u => ids.Contains(u.Id)).ToList();
            if (withSon)
            {
                List<SysMenu> list = new List<SysMenu>();
                foreach (var item in objList)
                {
                    list.AddRange(_menuService.GetAllSonByParentID(item.Id));
                }
                objList.AddRange(list);
                objList = objList.Distinct().ToList();
            }
            foreach (var curObj in objList)
            {
                curObj.DataState = 1;
                if (_menuService.Update(curObj))
                    successCount++;//成功+1
                else
                    errorCount++;
            }
            if (successCount > 0)
                boolResult = true;
            return BackData(new { Result = boolResult, successCount = successCount, errorCount = errorCount });

        }

        /// <summary>
        /// 根据父节点ID和子节点ID重置父子关系和排序
        /// </summary>
        /// <param name="parentId">父节点Id</param>
        /// <param name="arrChildIds">子节点Id</param>
        /// <returns>Result:true/false</returns>
        public JsonResult ResetParnetAndSort(Guid parentId, Guid[] arrChildIds)
        {
            bool boolResult = true;
            List<SysMenu> list = new List<SysMenu>();
            for (int i = 0; i < arrChildIds.Length; i++)
            {
                SysMenu model = new SysMenu();
                model = _menuService.FindById(arrChildIds[i]);
                model.ParentId = parentId;
                model.OrderId = i;
                list.Add(model);
            }
            boolResult = _menuService.UpdateByList(list);
            return BackData(new { Result = boolResult });
        }
        #endregion

    }
}