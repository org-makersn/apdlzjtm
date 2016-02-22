using Common.Func;
using Makers.Admin.Models;
using Makersn.Util;
using Net.Common.Configurations;
using Net.Common.Helper;
using Net.Framework.BizDac;
using Net.Framework.StoreModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makers.Admin.Controllers
{
    public class BusController : BaseController
    {
        //
        // GET: /Bus/

        public ActionResult Index()
        {
            return View();
        }

    }
}
