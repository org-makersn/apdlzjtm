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
        public ActionResult Materials(string mode = "list", int page = 1)
        {
            ViewData["Group"] = MenuModel(1);
            if (mode.Contains("add"))
            {
                IList<StoreMaterialT> models = new StoreMaterialBiz().getAllStoreMaterial();

                ViewData["cnt"] = models.Count;

                return View("MaterialAdd", models.OrderByDescending(p => p.RegDt).ToPagedList(page, 20));
            }
            else if (mode.Contains("edit"))
            {
                IList<StoreMaterialT> models = new StoreMaterialBiz().getAllStoreMaterial();

                ViewData["cnt"] = models.Count;

                return View("MaterialEdit", models.OrderByDescending(p => p.RegDt).ToPagedList(page, 20));
            }
            else
            {
                IList<StoreMaterialT> models = new StoreMaterialBiz().getAllStoreMaterial();

                ViewData["cnt"] = models.Count;

                return View(models.OrderByDescending(p => p.RegDt).ToPagedList(page, 20));
            }
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
