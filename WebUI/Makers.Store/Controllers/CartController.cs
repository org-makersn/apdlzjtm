using Makersn.BizDac;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Net.Common.Model;
using Newtonsoft.Json;
using Makersn.Models;

namespace Makers.Store.Controllers
{
    public class CartController : BaseController
    {
        //
        // GET: /Cart/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            int memberNo = 1; // admin
            List<StoreCartInfo> storeCartList = new List<StoreCartInfo>();
            storeCartList = new StoreCartBiz().GetStoreCartListByMemberNo(memberNo);
            
            return View(storeCartList);
        }

        [HttpPost]
        public PartialViewResult plugin_check(FormCollection forms)
        {
            StoreCartT storeCartT = new StoreCartT();
            storeCartT.CartNo = forms["cartNo"];

            return PartialView(storeCartT);
        }

        public void AddCart()
        {
            int cnt = int.Parse(Request["cnt"]);

            StoreCartT storeCartT = new StoreCartT();
            storeCartT.MemberNo = 1;
            storeCartT.ProductCnt = cnt;
            storeCartT.ProductDetailNo = 1;
            storeCartT.RegDt = DateTime.Now;
            storeCartT.RegId = "admin";

            int result = new StoreCartBiz().InsertCart(storeCartT);

            Response.Write(result);
        }
    }
}
