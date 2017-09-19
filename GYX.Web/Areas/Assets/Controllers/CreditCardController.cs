using GYX.Data.Domain.Assets;
using GYX.Service.IServiceManger.Assets;
using GYX.Service.ServiceManger.Assets;
using GYX.Web.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GYX.Core.Helpers;
using static GYX.Data.QueryBuilder;

/// <summary>
/// 信用卡管理
/// </summary>
namespace GYX.Web.Areas.Assets.Controllers
{

    public class CreditCardController : BaseController
    {
        ICreditCardInfoService _cardInfoService = new CreditCardInfoService();
        ICreditCardTakeRecordService _takeRecordService = new CreditCardTakeRecordService();
        /// <summary>
        /// 信用卡信息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 信用卡信息编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid? id)
        {
            ViewData["id"] = id;
            return View();
        }

        /// <summary>
        /// 取现记录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult TakeRecordIndex()
        {
            return View();
        }

        /// <summary>
        /// 取现记录编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult TakeRecordEdit(Guid? id)
        {
            ViewData["id"] = id;
            return View();
        }


        #region 信用卡信息
        #region 数据查询
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDataById_CardInfo(Guid id)
        {
            var obj = _cardInfoService.FindById(id);
            return BackData(obj);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public JsonResult GetDataList_CardInfo(int pageIndex = 1, int pageSize = int.MaxValue, CreditCardInfoQueryBuilder query = null)
        {
            int count = 0;
            var listData = _cardInfoService.GetForPaging(out count, query, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (CreditCardInfo)u).Select(u => new
            {
                u.Id,
                u.UserId,
                u.CardName,
                u.CardNo,
                u.LimitMoney,
                u.BillDay,
                u.RepaymentDay,
                u.IsUse,
                u.Remark,
                u.CreateTime,
                u.UpdateTime,
                UserObj = new { u.UserObj.Id, u.UserObj.UserName, u.UserObj.RealName }
            }).ToList();

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
        public JsonResult GetDataForCombo_CardInfo()
        {
            object query = new
            {
                IsUse = true
            };
            int count = 0;
            var listData = _cardInfoService.GetForPaging(out count, query).Select(u => (CreditCardInfo)u).OrderBy(u => u.UserObj.RealName).ThenBy(u => u.CardName).Select(t => new
            {
                value = t.Id,
                text = t.UserObj.RealName + "_" + t.CardName
            }).ToList();
            return BackData(listData);
        }
        #endregion
        #region 数据编辑
        /// <summary>
        /// 保存数据
        /// </summary>
        public JsonResult Save_CardInfo(CreditCardInfo model)
        {
            if (model.Id == Guid.Empty)
                return Create_CardInfo(model);
            else
                return Update_CardInfo(model);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        private JsonResult Create_CardInfo(CreditCardInfo model)
        {
            SystemResult result = new SystemResult();
            model.Id = Guid.NewGuid();
            model.CreateTime = DateTime.Now;
            model.UpdateTime = DateTime.Now;
            model.IsUse = model.IsUse ?? true;

            try
            {
                if (_cardInfoService.Insert(model))
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
        private JsonResult Update_CardInfo(CreditCardInfo model)
        {
            SystemResult result = new SystemResult();
            model.UpdateTime = DateTime.Now;
            model.IsUse = model.IsUse ?? true;

            if (_cardInfoService.Update(model))
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
        public JsonResult Delete_CardInfo(Guid[] ids)
        {
            SystemResult result = new SystemResult();
            int intSuccess = 0;
            int intError = 0;
            //Delete
            try
            {
                var objs = _cardInfoService.List().Where(u => ids.Contains(u.Id)).ToList();
                if (objs.Count > 0)
                {
                    foreach (var obj in objs)
                    {
                        if (_cardInfoService.Delete(obj))
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
        public ActionResult ResetIsUse_CardInfo(Guid[] ids, bool isUse = true)
        {
            SystemResult result = new SystemResult();
            var objs = _cardInfoService.List().Where(u => ids.Contains(u.Id)).ToList();
            foreach (var item in objs)
            {
                item.IsUse = isUse;
                item.UpdateTime = DateTime.Now;
            }
            result.isSuccess = _cardInfoService.UpdateByList(objs);
            return BackData(result);
        }

        #endregion
        #endregion

        #region 信用卡刷卡记录信息
        #region 数据查询
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDataById_TakeRecord(Guid? id)
        {
            var obj = _takeRecordService.FindById(id);
            return BackData(obj);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public JsonResult GetDataList_TakeRecord(int pageIndex = 1, int pageSize = int.MaxValue, CreditCardTakeRecordQueryBuilder query = null)
        {
            if (query.TakeDate_start.HasValue)
                query.TakeDate_start = query.TakeDate_start.Value.Date;
            if (query.TakeDate_end.HasValue)
                query.TakeDate_end = query.TakeDate_end.Value.AddDays(1).Date.AddSeconds(-1);

            int count = 0;
            var listData = _takeRecordService.GetForPaging(out count, query, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (CreditCardTakeRecord)u)
                .OrderByDescending(u => u.TakeDate).Select(u => new
                {
                    u.Id,
                    u.CardId,
                    TakeDate = u.TakeDate.ToDateString("yyyy-MM-dd"),
                    TakeMoney = u.TakeMoney,
                    Fee = u.Fee,
                    u.CreateTime,
                    u.Remark,
                    HasReturn = u.HasReturn ?? false,
                    ReturnDate = u.ReturnDate.ToDateString("yyyy-MM-dd"),
                    CardName = u.CardObj?.CardName,
                    UserName = u.CardObj?.UserObj?.RealName,
                    u.CardObj?.BillDay,
                    u.CardObj?.RepaymentDay
                }).ToList();

            return BackData(new
            {
                total = count,
                rows = listData,
                FeeTotal = _takeRecordService.Get().Sum(u => (decimal)(u.Fee ?? 0)),
                CurMoney = _takeRecordService.Get(new { HasReturn = false }).Sum(u => (decimal)(u.TakeMoney ?? 0))
            });
        }
        #endregion
        #region 数据编辑
        /// <summary>
        /// 保存数据
        /// </summary>
        public JsonResult Save_TakeRecor(CreditCardTakeRecord model)
        {
            //_takeRecordService
            if (model.Id == Guid.Empty)
                return Create_TakeRecor(model);
            else
                return Update_TakeRecor(model);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        private JsonResult Create_TakeRecor(CreditCardTakeRecord model)
        {
            SystemResult result = new SystemResult();
            model.Id = Guid.NewGuid();
            model.CreateTime = DateTime.Now;
            model.HasReturn = model.HasReturn ?? false;
            try
            {
                if (_takeRecordService.Insert(model))
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
        private JsonResult Update_TakeRecor(CreditCardTakeRecord model)
        {
            SystemResult result = new SystemResult();

            if (_takeRecordService.Update(model))
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
        public JsonResult Delete_TakeRecor(Guid[] ids)
        {
            SystemResult result = new SystemResult();
            int intSuccess = 0;
            int intError = 0;
            //Delete
            try
            {
                var objs = _takeRecordService.List().Where(u => ids.Contains(u.Id)).ToList();
                if (objs.Count > 0)
                {
                    //if (_takeRecordService.DeleteByList(objs))
                    //{
                    //    result.isSuccess = true;
                    //    result.message = string.Format("删除成功", intSuccess, intError);
                    //}
                    //else
                    //{
                    //    result.isSuccess = false;
                    //    result.message = "删除数据失败";
                    //}

                    foreach (var obj in objs)
                    {
                        if (_takeRecordService.Delete(obj))
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
        /// 归还信用卡
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GiveBack_TakeRecor(Guid? id)
        {
            SystemResult result = new SystemResult();
            var obj = _takeRecordService.FindById(id);
            obj.HasReturn = true;
            obj.ReturnDate = DateTime.Now.Date;
            if (_takeRecordService.Update(obj))
                result.isSuccess = true;
            else
            {
                result.isSuccess = false;
                result.message = "归还保存失败";
            }

            return BackData(result);
        }
        #endregion
        #endregion
    }
}