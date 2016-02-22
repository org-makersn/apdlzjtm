using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makers.Bus.Controllers
{
    public class BlogController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
