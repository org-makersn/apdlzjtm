using Common.Func;
using Makers.Admin.Models;
using Makersn.Util;
using Net.Common.Configurations;
using Net.Common.Helper;
using Net.Framework.BizDac;
using Net.Framework.Entity;
using Net.Framework.StoreModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makers.Admin.Controllers
{
    public class BusController : BaseController
    {
        private MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Bus";
            menuModel.MainIndex = 4;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }

        #region Dashboard
        /// <summary>
        /// Dashboard
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["Group"] = MenuModel(0);
            return View();
        } 
        #endregion

        #region History
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult History(int page = 1)
        {
            ViewData["Group"] = MenuModel(1);

            IList<BUS_HISTORY> list = new List<BUS_HISTORY>();
            ViewData["cnt"] = 0;
            return View(list.ToPagedList(page, 20));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddHistory()
        {
            ViewData["Group"] = MenuModel(1);
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddPostHistory(FormCollection collection)
        {
            return Json(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public ActionResult UpdateHistory(int no)
        {
            ViewData["Group"] = MenuModel(1);
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdatePostHistory()
        {
            return Json(true);
        }
        #endregion

        #region Blog
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Blog(int page = 1)
        {
            ViewData["Group"] = MenuModel(2);

            IList<BUS_BLOG> list = new List<BUS_BLOG>();
            ViewData["cnt"] = 0;
            return View(list.ToPagedList(page, 20));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddBlog()
        {
            ViewData["Group"] = MenuModel(2);
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddPostBlog(FormCollection collection)
        {
            return Json(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public ActionResult UpdateBlog(int no)
        {
            ViewData["Group"] = MenuModel(2);
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdatePostBlog()
        {
            return Json(true);
        } 
        #endregion

    }
}
