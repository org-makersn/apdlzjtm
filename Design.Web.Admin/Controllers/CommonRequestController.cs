using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Makersn.BizDac;
using Makersn.Util;

namespace Design.Web.Admin.Controllers
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
