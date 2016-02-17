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
            storeCartT.CART_NO = forms["cartNo"];

            return PartialView(storeCartT);
        }

        [HttpPost]
        public int AddCart(Int64 productDetailNo, int cnt)
        {            
            StoreCartT storeCartT = new StoreCartT();
            storeCartT.MEMBER_NO = profileModel.UserNo;
            storeCartT.ITEM_CNT = cnt;
            storeCartT.ITEM_NO = productDetailNo;
            storeCartT.REG_DT = DateTime.Now;
            storeCartT.REG_ID = profileModel.UserId;

            int result = new StoreCartBiz().InsertCart(storeCartT);

            return result;
        }

        [HttpPost]
        public int DeleteCart(Int64 cartSeq)
        {
            int result = new StoreCartBiz().DeleteCartByCondition(profileModel.UserNo, cartSeq);

            return result;
        }
    }
}
