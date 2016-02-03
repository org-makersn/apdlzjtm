using Makers.Admin.Models;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makers.Admin.Controllers
{
    [Authorize]
    public class SettingsController : BaseController
    {
        private MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Settings";
            menuModel.MainIndex = 8;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["Group"] = MenuModel(0);

            return View();
        }

        /// <summary>
        /// 재질
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Materials(string mode = "list", int page = 1)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);
            if (mode.Contains("add"))
            {
                IList<StoreMaterialT> models = new StoreMaterialBiz().getAllStoreMaterial();

                ViewData["cnt"] = models.Count;

                return View("MaterialAdd");
            }
            else if (mode.Contains("edit"))
            {
                IList<StoreMaterialT> models = new StoreMaterialBiz().getAllStoreMaterial();

                ViewData["cnt"] = models.Count;

                return View("MaterialEdit");
            }
            else
            {
                IList<StoreMaterialT> models = new StoreMaterialBiz().getAllStoreMaterial();

                ViewData["cnt"] = models.Count;

                return View(models.OrderByDescending(p => p.REG_DT).ToPagedList(page, 20));
            }
        }

        [HttpPost]
        public JsonResult Materials(string mode, string name, string imagename)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            if (mode.Contains("add"))
            {

            }
            else
            {

            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 코드
        /// </summary>
        /// <returns></returns>
        public ActionResult Code()
        {
            ViewData["Group"] = MenuModel(2);
            return View();
        }
    }
}
