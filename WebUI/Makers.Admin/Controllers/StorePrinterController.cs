using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makers.Admin.Controllers
{
    public class StorePrinterController : Controller
    {
        //
        // GET: /StorePrinter/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload() {
            return View();
        }
        public ActionResult Edit() {
            return View();
        }
        public ActionResult Delete() {
            return View();
        }

    }
}
