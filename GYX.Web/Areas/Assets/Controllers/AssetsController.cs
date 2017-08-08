using GYX.Service.IServiceManger.Assets;
using GYX.Service.ServiceManger.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GYX.Web.Areas.Assets.Controllers
{
    public class AssetsController : BaseController
    {
        IAssetsService _assetsService = new AssetsService();
        // GET: Assets
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