using GYX.Service.ServiceManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 主页
/// </summary>
namespace GYX.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //Response.Redirect("/SysDict/Index");
            return View();
        }
        
    }
}