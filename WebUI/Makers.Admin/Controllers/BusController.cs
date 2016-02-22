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
        BusManageDac busManageDac = new BusManageDac();

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
        public ActionResult History(int no = 0, int page = 1, string mode = "list")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);

            if (mode.Contains("add"))
            {
                return View("AddHistory");
            }
            else if (mode.Contains("edit"))
            {
                BusHistory history = busManageDac.GetBusHistoryByNo(no);
                return View("UpdateHistory", history);
            }
            else
            {
                IList<BusHistory> list = busManageDac.GetBusHistoryList();

                ViewData["cnt"] = list.Count;

                return View(list.ToPagedList(page, 30));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PostHistory(FormCollection collection)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;

            string paramMode = collection["mode"];

            string paramTitle = collection["TITLE"];
            string paramProgressDt = collection["PROGRESS_DT"];
            string paramUseYn = collection["USE_YN"];

            if (paramMode.Contains("add"))
            {
                BusHistory history = new BusHistory();
                history.TITLE = paramTitle;
                history.PROGRESS_DT = paramProgressDt;
                history.USE_YN = paramUseYn;
                history.REG_DT = DateTime.Now;
                history.REG_ID = Profile.UserNm;

                int ret = busManageDac.AddHistory(history);
                if (ret > 0)
                {
                    response.Success = true;
                    response.Result = ret.ToString();
                }
            }

            if (paramMode.Contains("edit"))
            {
                int no = Convert.ToInt32(collection["NO"]);
                BusHistory history = busManageDac.GetBusHistoryByNo(no);
                if (history != null)
                {
                    history.TITLE = paramTitle;
                    history.PROGRESS_DT = paramProgressDt;
                    history.USE_YN = paramUseYn;
                    history.UPD_DT = DateTime.Now;
                    history.UPD_ID = Profile.UserNm;

                    bool ret = busManageDac.UpdateHistory(history);
                    if (ret)
                    {
                        response.Success = true;
                        response.Result = history.NO.ToString();
                    }
                }
            }

            return Json(response);
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

            IList<BusBlog> list = new List<BusBlog>();
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
