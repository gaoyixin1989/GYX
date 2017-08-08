using GYX.Service.IServiceManger.Assets;
using GYX.Service.ServiceManger.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 账本
/// </summary>
namespace GYX.Web.Areas.Assets.Controllers
{

    public class AccountBookController : BaseController
    {
        IAccountBookService _accountBookService = new AccountBookService();
        // GET: AccountBook
        public ActionResult Index()
        {
            return View();
        }

        #region 数据查询
        #endregion

        #region 数据编辑
        #endregion

    }
}