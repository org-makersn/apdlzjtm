using System.Collections.Generic;
using System.Web.Mvc;
using Design.Web.Admin.Models;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using Makersn.Util;
using System.Linq;
using PagedList;

namespace Design.Web.Admin.Controllers
{
    public class StoreController : BaseController
    {
        private MenuModel menuModel = new MenuModel();
        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Store";
            menuModel.MainIndex = 4;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }
        /// <summary>
        /// Store Managing Default Page
        /// This is a Order Listing Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            return View();
        }
        public ActionResult ProductCertificateList(string query = "", int status = (int)MakersnEnumTypes.ProductCertificateStatus.Request, int page = 1)
        {

            if(Profile.UserLevel < 50) { return Redirect("/account/logon"); }
            ViewData["Group"] = MenuModel(0);

            List<StoreProductT> productList = new StoreProductBiz().searchProductWithCertification(status, query);
            ViewBag.ProductList = productList;

            ViewData["certificateType"] = status;
            ViewData["query"] = query;
            ViewData["cnt"] = productList.Count;

            return View(productList.OrderByDescending(p => p.RegDt).ToPagedList(page, 20));
        }

    }
}
