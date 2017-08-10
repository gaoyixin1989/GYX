using GYX.Data.Domain.Assets;
using GYX.Service.IServiceManger.Assets;
using GYX.Service.ServiceManger.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static GYX.Data.QueryBuilder;

/// <summary>
/// 账本
/// </summary>
namespace GYX.Web.Areas.Assets.Controllers
{

    public class AccountBookController : BaseController
    {
        IAccountBookService _accountBookService = new AccountBookService();
        // GET: AccountBook

        /// <summary>
        /// 记账列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 记账编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        public JsonResult GetDataById(Guid? id)
        {
            var obj = _accountBookService.FindById(id);
            return BackData(obj);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public JsonResult GetDataList(int pageIndex = 1, int pageSize = int.MaxValue, AccountBookQueryBuilder query = null)
        {
            int count = 0;
            var listData = _accountBookService.GetForPaging(out count, query, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (AccountBook)u).ToList();

            return BackData(new
            {
                total = count,
                rows = listData
            });
        }

        #endregion

        #region 数据编辑
        /// <summary>
        /// 保存数据
        /// </summary>
        public JsonResult Save(AccountBook model)
        {
            if (model.Id == Guid.Empty)
                return Create(model);
            else
                return Update(model);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        private JsonResult Create(AccountBook model)
        {
            SystemResult result = new SystemResult();
            model.CreateTime = DateTime.Now;
            model.DataState = model.DataState ?? 0;

            try
            {
                if (_accountBookService.Insert(model))
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
        private JsonResult Update(AccountBook model)
        {
            SystemResult result = new SystemResult();
            model.DataState = model.DataState ?? 0;

            if (_accountBookService.Update(model))
                result.isSuccess = true;
            else
            {
                result.isSuccess = false;
                result.message = "更新失败";
            }

            return BackData(result);
        }

        /// <summary>
        /// 删除
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
                var objs = _accountBookService.List().Where(u => ids.Contains(u.Id)).ToList();
                if (objs.Count > 0)
                {
                    foreach (var obj in objs)
                    {
                        obj.DataState = 1;
                        if (_accountBookService.Update(obj))
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
        #endregion

    }
}