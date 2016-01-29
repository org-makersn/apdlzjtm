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
            return View();
        }

        public PartialViewResult Grid()
        {
            List<StoreCartInfo> storeCartList = new List<StoreCartInfo>();
            storeCartList = new StoreCartBiz().GetStoreCartListByMemberNo(profileModel.UserNo);

            return PartialView(storeCartList);
        }

        [HttpPost]
        public PartialViewResult plugin_check(FormCollection forms)
        {
            StoreCartT storeCartT = new StoreCartT();
            storeCartT.CartNo = forms["cartNo"];

            return PartialView(storeCartT);
        }

        [HttpPost]
        public void AddCart(Int64 productDetailNo, int cnt)
        {            
            StoreCartT storeCartT = new StoreCartT();
            storeCartT.MemberNo = profileModel.UserNo;
            storeCartT.ProductCnt = cnt;
            storeCartT.ProductDetailNo = productDetailNo;
            storeCartT.RegDt = DateTime.Now;
            storeCartT.RegId = profileModel.UserId;

            new StoreCartBiz().InsertCart(storeCartT);
        }

        [HttpPost]
        public int DeleteCart(Int64 cartSeq)
        {
            int result = new StoreCartBiz().DeleteCartByCondition(profileModel.UserNo, cartSeq);

            return result;
        }
    }
}
