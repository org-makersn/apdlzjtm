using Net.Common.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makers.Bus.Controllers
{
    public class BusController : BaseController
    {
        //
        // GET: /Bus/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Apply()
        {
            return View();
        }

        public ActionResult Qna()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }

        public PartialViewResult SubHeader()
        {
            return PartialView();
        }

    }
}
