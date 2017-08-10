using GYX.Data.Domain.System;
using GYX.Service.IServiceManger.System;
using GYX.Service.ServiceManger.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 字典
/// </summary>
namespace GYX.Web.Areas.System.Controllers
{
    public class SysDictController : BaseController
    {
        ISysDictService _dictService = new SysDictService();
        // GET: SysDict
        //treegrid
        public ActionResult Index()
        {
            return View();
        }

        #region 查询数据
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetById(Guid id)
        {
            var obj = _dictService.FindById(id);
            return BackData(obj);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetData()
        {
            var condition = new { DataState = 0 };//查询条件
            int intTotal = 0;
            List<SysDict> listData = _dictService.GetForPaging(out intTotal, condition).Select(u => (SysDict)u).ToList();
            listData = listData.OrderBy(u => u.OrderId).ToList();
            return BackData(listData);
        }

        /// <summary>
        /// 获取菜单数据：easyui-tree数据格式
        /// </summary>
        /// <param name="rootCode">指定根节点</param>
        /// <param name="withRoot">是否包含根节点，rootCode值存在时才有效</param>
        /// <returns>easyui-tree数据格式json</returns>
        public JsonResult GetTreeData(string rootCode, bool withRoot = true)
        {
            List<object> listResult = new List<object>();
            var condition = new { DataState = 0 };//查询条件
            int intTotal = 0;
            List<SysDict> listData = _dictService.GetForPaging(out intTotal, condition).Select(u => (SysDict)u).ToList();
            listData = listData.OrderBy(u => u.OrderId).ToList();
            //listResult = FunBase.SetDataToTree(listData.ToList<object>(), "Id", "ParentId", "Id", "DictText"
            //    , true, new string[] { });
            var rootData = new List<SysDict>();
            if (!string.IsNullOrEmpty(rootCode))
            {
                List<Guid> listIds = listData.Where(u => u.DictCode == rootCode).Select(u => u.Id).ToList();
                if (withRoot)
                    rootData = listData.Where(u => listIds.Contains(u.Id)).ToList();
                else
                {
                    rootData = listData.Where(u => listIds.Contains(u.ParentId ?? Guid.Empty)).ToList();
                }
                listResult = SetDataIntoTree(listData, rootData);
            }
            else
            {
                listResult = SetDataIntoTree(listData, null);
            }
            return BackData(listResult);

        }
        /// <summary>
        /// 将数据转换为树结构
        /// </summary>
        /// <param name="source">源数据</param>
        /// <param name="rootData">根节点（为null时自动取没有父节点的数据作为根节点）</param>
        /// <returns></returns>
        private List<object> SetDataIntoTree(List<SysDict> source, List<SysDict> rootData)
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
                        text = item.DictText,
                        state = "closed",
                        attributes = item,
                        children = listChildren
                    });
                else
                    listResult.Add(new
                    {
                        id = item.Id,
                        text = item.DictText,
                        attributes = item
                    });

            }

            return listResult;
        }

        /// <summary>
        /// 根据字典编码获取其下属字典内容
        /// </summary>
        /// <param name="strCode">字典编码</param>
        /// <returns></returns>
        public ActionResult GetDictByCode(string strCode)
        {
            List<object> listResult = new List<object>();
            if (string.IsNullOrEmpty(strCode))
                return BackData(listResult);

            int intTotal = 0;
            var parents = _dictService.GetForPaging(out intTotal, new { DataState = 0, IsUse = true, DictCode = strCode })
                .Select(u => (SysDict)u).Where(u => u.DictCode == strCode).ToList();

            foreach (var itemP in parents)
            {
                var curDicts = _dictService.GetForPaging(out intTotal, new { DataState = 0, IsUse = true, ParentId = itemP.Id }).Select(u => (SysDict)u).ToList();
                curDicts = curDicts.OrderBy(u => u.OrderId ?? 9999).ToList();
                foreach (var itemD in curDicts)
                {
                    listResult.Add(new
                    {
                        DictCode = itemD.DictCode,
                        DictText = itemD.DictText,
                        IsDefalut = itemD.IsDefalut ?? false
                    });
                }
            }


            return BackData(listResult);
        }

        /// <summary>
        /// 获取菜单数据：easyui-tree数据格式
        /// </summary>
        /// <param name="strCode">指定根节点</param>
        /// <returns>easyui-tree数据格式json</returns>
        public JsonResult GetDictTreeByCode(string strCode)
        {
            List<object> listResult = new List<object>();
            var condition = new { DataState = 0 };//查询条件
            int intTotal = 0;
            List<SysDict> listData = _dictService.GetForPaging(out intTotal, condition).Select(u => (SysDict)u).ToList();
            listData = listData.OrderBy(u => u.OrderId).ToList();
            //listResult = FunBase.SetDataToTree(listData.ToList<object>(), "Id", "ParentId", "Id", "DictText"
            //    , true, new string[] { });
            var rootData = new List<SysDict>();
            List<Guid> listIds = listData.Where(u => u.DictCode == strCode).Select(u => u.Id).ToList();
            rootData = listData.Where(u => listIds.Contains(u.ParentId ?? Guid.Empty)).ToList();

            listResult = SetDataCodeIntoTree(listData, rootData);
            return BackData(listResult);

        }
        /// <summary>
        /// 将数据转换为树结构
        /// </summary>
        /// <param name="source">源数据</param>
        /// <param name="rootData">根节点（为null时自动取没有父节点的数据作为根节点）</param>
        /// <returns></returns>
        private List<object> SetDataCodeIntoTree(List<SysDict> source, List<SysDict> rootData)
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
                listChildren = SetDataCodeIntoTree(source, source.Where(u => (u.ParentId ?? Guid.Empty) == item.Id).ToList());
                if (listChildren.Count > 0)
                    listResult.Add(new
                    {
                        id = item.DictCode,
                        text = item.DictText,
                        state = "closed",
                        @checked = item.IsDefalut ?? false,
                        children = listChildren
                    });
                else
                    listResult.Add(new
                    {
                        id = item.DictCode,
                        text = item.DictText,
                        @checked = item.IsDefalut ?? false
                    });

            }

            return listResult;
        }
        #endregion

        #region 编辑数据
        /// <summary>
        /// 保存数据前设置对象的默认值
        /// </summary>
        private void SetDefaultDataBeforeSave(SysDict model)
        {
            _dictService.TrimObj(model);
            model.CreateTime = model.CreateTime ?? DateTime.Now;
            model.UpdateTime = model.UpdateTime ?? DateTime.Now;

            model.DataState = model.DataState ?? 0;
            model.IsDefalut = model.IsDefalut ?? false;
            model.IsUse = model.IsUse ?? true;

            model.ParentId = model.ParentId ?? Guid.Empty;
            if (!model.OrderId.HasValue)
            {
                var sonObjs = _dictService.List().Where(u => u.ParentId == model.ParentId).Select(u => u.OrderId);
                if (sonObjs.Count() > 0)
                    model.OrderId = (sonObjs.Max() ?? 0) + 1;
                else
                    model.OrderId = 1;

            }

        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public JsonResult Save(SysDict model)
        {
            if (model.Id == Guid.Empty)
                return Create(model);
            else
                return Update(model);
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        private JsonResult Create(SysDict model)
        {
            SystemResult obj = new SystemResult();
            model.Id = Guid.NewGuid();
            SetDefaultDataBeforeSave(model);
            obj.isSuccess = _dictService.Insert(model);
            if (obj.isSuccess)
            {
                obj.data = model;
                ResetDefault(model);
            }
            return BackData(obj);
        }
        /// <summary>
        /// 修改菜单
        /// </summary>
        private JsonResult Update(SysDict model)
        {
            SystemResult obj = new SystemResult();
            var oldData = _dictService.FindById(model.Id);
            oldData.DictCode = model.DictCode;
            oldData.DictText = model.DictText;
            oldData.ParentId = model.ParentId;
            oldData.IsUse = model.IsUse;
            oldData.IsDefalut = model.IsDefalut;
            oldData.Remark = model.Remark;
            model = oldData;

            SetDefaultDataBeforeSave(model);
            obj.isSuccess = _dictService.Update(model);
            if (obj.isSuccess)
            {
                obj.data = model;
                ResetDefault(model);
            }

            return BackData(obj);
        }

        /// <summary>
        /// 设置默认值
        /// </summary>
        /// <param name="model"></param>
        private void ResetDefault(SysDict model)
        {
            if (model.IsDefalut ?? false)//当前字典为默认值时，将其他字典设置为非默认值
            {
                int intTotal = 0;
                var condition = new
                {
                    ParentId = model.ParentId,
                    IsDefalut = true
                };
                List<SysDict> listData = _dictService.GetForPaging(out intTotal, condition).Select(u => (SysDict)u).ToList();
                foreach (var item in listData)
                {
                    if (item.Id != model.Id)
                    {
                        item.IsDefalut = false;
                        _dictService.Update(item);
                    }
                }
            }
        }

        /// <summary>
        /// 删除菜单_软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="withSon">是否包含子元素，默认不包含</param>
        /// <returns></returns>
        public JsonResult Delete(Guid[] ids, bool withSon = false)
        {
            bool boolResult = false;
            int successCount = 0;
            int errorCount = 0;
            var objList = _dictService.List().Where(u => ids.Contains(u.Id)).ToList();
            if (withSon)
            {
                List<SysDict> list = new List<SysDict>();
                foreach (var item in objList)
                {
                    list.AddRange(_dictService.GetAllSonByParentID(item.Id));
                }
                objList.AddRange(list);
                objList = objList.Distinct().ToList();
            }
            foreach (var curObj in objList)
            {
                curObj.DataState = 1;
                if (_dictService.Update(curObj))
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
            List<SysDict> list = new List<SysDict>();
            for (int i = 0; i < arrChildIds.Length; i++)
            {
                SysDict model = new SysDict();
                model = _dictService.FindById(arrChildIds[i]);
                model.ParentId = parentId;
                model.OrderId = i;
                list.Add(model);
            }
            boolResult = _dictService.UpdateByList(list);
            return BackData(new { Result = boolResult });
        }
        #endregion
    }
}