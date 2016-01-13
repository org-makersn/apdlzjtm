using Makersn.BizDac;
using Makersn.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makers.Admin.Controllers
{
    [Authorize]
    public class CommonRequestController : BaseController
    {
        public ActionResult Index()
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            return View();
        }
    }
}
