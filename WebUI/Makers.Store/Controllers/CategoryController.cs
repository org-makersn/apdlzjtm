﻿using Makers.Store.Helper;
using Net.Common.Filter;
using Net.Framework.BizDac;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makers.Store.Controllers
{
    public class CategoryController : BaseController
    {
        StoreItemDac sItemDac = new StoreItemDac();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeNo"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(string category = "", int page = 1)
        {
            int pageSize = 40;
            int codeNo = 0;
            string gbn = "";

            if (!string.IsNullOrEmpty(category))
            {
                //latest-fashion-and-accessories
                string[] strarr = category.Split('-');
                gbn = strarr.First();

                if (strarr.Length > 1)
                {
                    string codekey = category.Replace(gbn + "-", string.Empty);

                    var categoryT = new CommonCodeDac().GetCommonCode("STORE", "PRODUCT").SingleOrDefault(m => m.CODE_KEY.Equals(codekey, StringComparison.OrdinalIgnoreCase));
                    if (categoryT != null)
                    {
                        ViewBag.Title = categoryT.CODE_NAME;
                        codeNo = categoryT.NO;
                    }
                }
            }

            int fromIndex = ((page - 1) * pageSize) + 1;
            int toIndex = page * pageSize;

            IList<StoreItemDetailT> list = null;

            int totalCnt = sItemDac.GetTotalCountByOption(profileModel.UserNo, codeNo, gbn);

            list = sItemDac.GetStoreItemsByOption(profileModel.UserNo, codeNo, gbn, fromIndex, toIndex);

            if (gbn == "featured")
            {
                if (list.Count() == 0)
                {
                    //추천이 없을시 
                }
            }

            PagerInfo pager = new PagerInfo();
            pager.CurrentPageIndex = page;
            pager.PageSize = pageSize;
            pager.RecordCount = totalCnt;
            PagerQuery<PagerInfo, IList<StoreItemDetailT>> model = new PagerQuery<PagerInfo, IList<StoreItemDetailT>>(pager, list);

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Initialize()
        {
            return View();
        }
    }
}
