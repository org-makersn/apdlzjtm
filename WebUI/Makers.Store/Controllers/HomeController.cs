﻿using Net.Common.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makers.Store.Controllers
{
    //[Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
