using GYX.Core.Helpers;
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
/// 资产管理
/// </summary>
namespace GYX.Web.Areas.Assets.Controllers
{
    public class AssetsController : BaseController
    {
        IAssetsService _assetsService = new AssetsService();
        IAssetsDetailService _detailService = new AssetsDetailService();
        // GET: Assets
        /// <summary>
        /// 资产统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }

        /// <summary>
        /// 统计编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid? id)
        {
            ViewData["id"] = id;
            return View();
        }

        /// <summary>
        /// 统计明细编辑页面
        /// </summary>
        /// <param name="AssetsId">资产统计id</param>
        /// <param name="DetailId">明细id</param>
        /// <returns></returns>
        public ActionResult EditDetail(Guid? AssetsId, Guid? DetailId)
        {
            ViewData["AssetsId"] = AssetsId;
            ViewData["DetailId"] = DetailId;
            return View();
        }

        #region 统计情况
        #region 数据查询
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDataById_Assets(Guid? id)
        {
            var obj = _assetsService.FindById(id);
            return BackData(obj);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public JsonResult GetDataList_Assets(int pageIndex = 1, int pageSize = int.MaxValue, AssetsQueryBuilder query = null)
        {
            if (query.StatisticsDate_start.HasValue)
                query.StatisticsDate_start = query.StatisticsDate_start.Value.Date;
            if (query.StatisticsDate_end.HasValue)
                query.StatisticsDate_end = query.StatisticsDate_end.Value.AddDays(1).Date.AddSeconds(-1);
            int count = 0;
            var listData = _assetsService.GetForPaging(out count, query, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (AssetsTable)u).Select(u => new
            {
                u.Id,
                StatisticsDate = u.StatisticsDate.ToDateString("yyyy-MM-dd"),
                Total = u.DetailList.Sum(m => (decimal)(m.Money ?? 0)),
                u.Remark
            }).ToList();

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
        public JsonResult Save_Assets(AssetsTable model)
        {
            if (model.Id == Guid.Empty)
                return Create_Assets(model);
            else
                return Update_Assets(model);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        private JsonResult Create_Assets(AssetsTable model)
        {
            SystemResult result = new SystemResult();
            model.Id = Guid.NewGuid();
            model.CreateTime = DateTime.Now;
            model.UpdateTime = DateTime.Now;
            model.DataState = model.DataState ?? 0;

            try
            {
                if (_assetsService.Insert(model))
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
        private JsonResult Update_Assets(AssetsTable model)
        {
            SystemResult result = new SystemResult();
            model.UpdateTime = DateTime.Now;
            model.DataState = model.DataState ?? 0;

            if (_assetsService.Update(model))
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
        public JsonResult Delete_Assets(Guid[] ids)
        {
            SystemResult result = new SystemResult();
            int intSuccess = 0;
            int intError = 0;
            //Delete
            try
            {
                var objs = _assetsService.List().Where(u => ids.Contains(u.Id)).ToList();
                if (objs.Count > 0)
                {
                    foreach (var obj in objs)
                    {
                        obj.DataState = 1;
                        obj.UpdateTime = DateTime.Now;
                        if (_assetsService.Update(obj))
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
        #endregion

        #region 统计明细
        

        #region 数据查询
        /// <summary>
        /// 根据id查询明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetDataById_Detail(Guid? id)
        {
            var obj = _detailService.FindById(id);
            return BackData(obj);
        }

        /// <summary>
        /// 根据统计id查询对应明细列表
        /// </summary>
        /// <param name="AssetsId"></param>
        /// <returns></returns>
        public JsonResult GetDataList_Detail(Guid? AssetsId)
        {
            var listData = _assetsService.FindById(AssetsId)?.DetailList;
            return BackData(new
            {
                total = listData.Count(),
                rows = listData
            });
        }

        #endregion
        #region 数据编辑
        /// <summary>
        /// 保存数据
        /// </summary>
        public JsonResult Save_Detail(AssetsDetail model)
        {
            if (model.Id == Guid.Empty)
                return Create_Detail(model);
            else
                return Update_Detail(model);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        private JsonResult Create_Detail(AssetsDetail model)
        {
            SystemResult result = new SystemResult();
            model.Id = Guid.NewGuid();

            try
            {
                if (_detailService.Insert(model))
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
        private JsonResult Update_Detail(AssetsDetail model)
        {
            SystemResult result = new SystemResult();

            if (_detailService.Update(model))
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
        public JsonResult Delete_Detail(Guid[] ids)
        {
            SystemResult result = new SystemResult();
            int intSuccess = 0;
            int intError = 0;
            //Delete
            try
            {
                var objs = _detailService.List().Where(u => ids.Contains(u.Id)).ToList();
                if (objs.Count > 0)
                {
                    foreach (var obj in objs)
                    {
                        if (_detailService.Delete(obj))
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
        #endregion
    }
}