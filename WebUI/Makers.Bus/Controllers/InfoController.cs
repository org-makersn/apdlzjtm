using Net.Framework.BizDac;
using Net.Framework.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makers.Bus.Controllers
{
    public class InfoController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("about");
        }

        /// <summary>
        /// 소개
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult About()
        {
            //통계
            MakerBusState state = new BusManageDac().GetMakerbusState();
            ViewData["SchoolCnt"] = string.Format("{0:n0}", state.SchoolCnt);
            ViewData["StudentCnt"] = string.Format("{0:n0}", state.StudentCnt);
            ViewData["ModelingCnt"] = string.Format("{0:n0}", state.ModelingCnt);
            ViewData["PrinterCnt"] = string.Format("{0:n0}", state.PrinterCnt);

            IList<BusHistory> historyList = new BusManageDac().getBusHistoryListByUseYn("Y");

            return View(historyList);
        }

        /// <summary>
        /// 워크샵
        /// </summary>
        /// <returns></returns>
        public ActionResult Workshop()
        {
            return View();
        }
    }
}
