using Makersn.BizDac;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using Net.Common.Model;
using Newtonsoft.Json;
using Makersn.Models;
using System.Web.Util;
using System.Text;
using System.Xml;
using System.Collections;
using System.Data;

using Makers.Store.Configurations;

namespace Makers.Store.Controllers
{
    public class ShippingController : BaseController
    {
        //
        // GET: /Shipping/

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult List()
        {
            List<StoreShippingAddrT> model = new List<StoreShippingAddrT>();
            StoreShippingBiz biz = new StoreShippingBiz();

            model = biz.GetShippingAddrListByMemberNo(profileModel.UserNo);

            return PartialView(model);
        }

        #region AddShippingInfo - 배송지 추가
        /// <summary>
        /// 배송지 추가
        /// </summary>
        /// <param name="shippingName"></param>
        /// <param name="addr1"></param>
        /// <param name="addr2"></param>
        /// <param name="addrDetail"></param>
        /// <param name="postNo"></param>
        /// <param name="defaultYn"></param>
        /// <returns></returns>
        [HttpPost]
        public Int64 AddShippingInfo(string shippingName, string addr1, string addr2, string addrDetail, string postNo, string defaultYn)
        {
            StoreShippingAddrT storeShippingAddrT = new StoreShippingAddrT();
            storeShippingAddrT.MemberNo = profileModel.UserNo;
            storeShippingAddrT.ShippingName = shippingName;
            storeShippingAddrT.Addr1 = addr1;
            storeShippingAddrT.Addr2 = addr2;
            storeShippingAddrT.AddrDetail = addrDetail;
            storeShippingAddrT.PostNo = postNo;
            storeShippingAddrT.DefaultYn = defaultYn;
            storeShippingAddrT.RegDt = DateTime.Now;
            storeShippingAddrT.RegId = profileModel.UserId;

            Int64 result = new StoreShippingBiz().InsertShippingInfo(storeShippingAddrT);

            return result;
        }
        #endregion

    }
}
