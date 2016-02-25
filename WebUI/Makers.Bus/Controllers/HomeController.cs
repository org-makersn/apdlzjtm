﻿using Net.Framework.BizDac;
using Net.Framework.Entity;
using Net.Framework.Util;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Makers.Bus.Controllers
{
    public class HomeController : BaseController
    {
        BusManageDac busManageDac = new BusManageDac();

        /// <summary>
        /// 메인
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Index()
        {
            ViewData["IsMain"] = "Y";
            IList<BannerExT> banners = new BannerExDac().GetAllBannerList((int)BannerType.Bus);
            ViewBag.MainBanner = banners;

            //통계
            MakerBusState state = busManageDac.GetMakerbusState();
            ViewData["SchoolCnt"] = string.Format("{0:n0}", state.SchoolCnt);
            ViewData["StudentCnt"] = string.Format("{0:n0}", state.StudentCnt);
            ViewData["ModelingCnt"] = string.Format("{0:n0}", state.ModelingCnt);
            ViewData["PrinterCnt"] = string.Format("{0:n0}", state.PrinterCnt);

            IList<BusBlog> blogs = busManageDac.GetBusBlogListByOption("Y", 1, 3);

            return View(blogs);
        }

    }
}
