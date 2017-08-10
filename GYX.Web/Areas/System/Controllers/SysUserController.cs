using GYX.Data.Domain.System;
using GYX.Service.IServiceManger.System;
using GYX.Service.ServiceManger.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static GYX.Data.QueryBuilder;

/// <summary>
/// 用户
/// </summary>
namespace GYX.Web.Areas.System.Controllers
{
    public class SysUserController : BaseController
    {
        ISysUserService _userService = new SysUserService();
        // GET: SysUser
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            ViewData["id"] = id;
            return View();
        }


        #region 数据查询
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDataById(Guid id)
        {
            var obj = _userService.FindById(id);
            return BackData(obj);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public JsonResult GetDataList(int pageIndex = 1, int pageSize = int.MaxValue, SysUserQueryBuilder query = null)
        {
            //query.IsUse = true;
            //query.DataState = new List<int>() { 0 };
            if (!string.IsNullOrEmpty(query.UserName)) query.UserName.Trim();
            if (!string.IsNullOrEmpty(query.RealName)) query.RealName.Trim();
            int count = 0;
            List<SysUser> listData = _userService.GetForPaging(out count, query, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (SysUser)u).ToList();

            return BackData(new
            {
                total = count,
                rows = listData
            });
        }

        /// <summary>
        /// 获取数据用于下拉框列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDataForCombo()
        {
            object query = new
            {
                DataState = 0,
                IsUse = true
            };
            int count = 0;
            var listData = _userService.GetForPaging(out count, query).Select(u => (SysUser)u).OrderBy(u => u.RealName).Select(t => new
            {
                value = t.Id,
                text = t.RealName
            }).ToList();
            return BackData(listData);
        }
        #endregion

        #region 数据编辑
        /// <summary>
        /// 保存数据
        /// </summary>
        public JsonResult Save(SysUser model)
        {
            if (model.Id == Guid.Empty)
                return Create(model);
            else
                return Update(model);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        private JsonResult Create(SysUser model)
        {
            SystemResult result = new SystemResult();
            model.Id = Guid.NewGuid();
            model.CreateTime = DateTime.Now;
            model.UpdateTime = DateTime.Now;
            model.IsUse = model.IsUse ?? true;
            model.DataState = model.DataState ?? 0;
            try
            {
                if (_userService.Insert(model))
                    result.isSuccess = true;
                else
                {
                    result.isSuccess = false;
                    result.message = "新增失败";
                }
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.message = ex.Message;
            }
            return BackData(result);
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        private JsonResult Update(SysUser model)
        {
            SystemResult result = new SystemResult();
            var oldData = _userService.FindById(model.Id);
            oldData.UserName = model.UserName;
            oldData.RealName = model.RealName;
            oldData.IsUse = model.IsUse;
            oldData.Remark = model.Remark;
            oldData.UpdateTime = DateTime.Now;

            try
            {
                if (_userService.Update(oldData))
                    result.isSuccess = true;
                else
                {
                    result.isSuccess = false;
                    result.message = "保存失败";
                }
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.message = ex.Message;
            }

            return BackData(result);
        }

        /// <summary>
        /// 删除——软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult Delete(Guid[] ids)
        {
            SystemResult result = new SystemResult();
            int intSuccess = 0;
            int intError = 0;
            //Delete
            try
            {
                var objs = _userService.List().Where(u => ids.Contains(u.Id)).ToList();
                if (objs.Count > 0)
                {
                    foreach (var obj in objs)
                    {
                        obj.DataState = 1;
                        obj.UpdateTime = DateTime.Now;
                        if (_userService.Update(obj))
                            intSuccess++;
                        else
                            intError++;
                    }
                    if (intSuccess > 0)
                    {
                        result.isSuccess = true;
                        result.message = string.Format("成功删除{0}条数据，失败{1}条", intSuccess, intError);
                    }
                    else
                    {
                        result.isSuccess = false;
                        result.message = "删除数据失败";
                    }

                }
                else
                {
                    result.isSuccess = false;
                    result.message = "没有可删除的数据";
                }
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.message = ex.Message;
            }
            return BackData(result);
        }

        /// <summary>
        /// 启用禁用
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public ActionResult ResetIsUse(Guid[] ids, bool isUse = true)
        {
            SystemResult result = new SystemResult();
            var objs = _userService.List().Where(u => ids.Contains(u.Id)).ToList();
            foreach (var item in objs)
            {
                item.IsUse = isUse;
                item.UpdateTime = DateTime.Now;
            }
            result.isSuccess = _userService.UpdateByList(objs);
            return BackData(result);
        }

        #endregion

    }
}