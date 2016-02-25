using Net.Common.Configurations;
using Net.Common.Helper;
using Net.Framework.BizDac;
using Net.Framework.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makers.Bus.Controllers
{
    public class InfoController : BaseController
    {
        BusManageDac busManageDac = new BusManageDac();
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
            ViewData["Menu"] = "about";
            //통계
            MakerBusState state = busManageDac.GetMakerbusState();
            ViewData["SchoolCnt"] = string.Format("{0:n0}", state.SchoolCnt);
            ViewData["StudentCnt"] = string.Format("{0:n0}", state.StudentCnt);
            ViewData["ModelingCnt"] = string.Format("{0:n0}", state.ModelingCnt);
            ViewData["PrinterCnt"] = string.Format("{0:n0}", state.PrinterCnt);

            IList<BusHistory> historyList = busManageDac.getBusHistoryListByUseYn("Y");

            return View(historyList);
        }

        /// <summary>
        /// 워크샵
        /// </summary>
        /// <returns></returns>
        public ActionResult Workshop()
        {
            ViewData["Menu"] = "workshop";
            BusTextbook textbook = busManageDac.GetBusTextbookLatest();

            return View(textbook);
        }

        /// <summary>
        /// 교재 다운로드
        /// </summary>
        /// <returns></returns>
        public ActionResult TextbookDownload(int no)
        {
            ApplicationConfiguration common = ApplicationConfiguration.Instance;
            BusTextbook textbook = busManageDac.GetBusTextbookByNo(no);

            var path = string.Format(@"{0}/{1}/{2}", common.FileServerUncPath, instance.TextbookFile, textbook.RENAME);
            var stream = new FileStream(path, FileMode.Open);

            bool ret = busManageDac.UpdateTextbookDownloadCnt(no);

            return File(stream, textbook.MIME_TYPE, textbook.VERSION); 
        }
    }
}
