using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Design.Web.Front.Areas.Common.Controllers
{
    public class RequestController : Controller
    {
        //
        // GET: /Common/Request/
        public JsonResult Index()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }
	}
}