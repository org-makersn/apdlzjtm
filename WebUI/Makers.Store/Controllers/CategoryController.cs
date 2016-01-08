using Makers.Store.Helper;
using Net.Common.Filter;
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
        public ActionResult Index(int codeNo = 0, int page = 1)
        {
            int pageSize = 40;

            int fromIndex = ((page - 1) * pageSize) + 1;
            int toIndex = page * pageSize;

            IList<StoreProductT> list = null;

            int totalCnt = new StoreProductBiz().getTotalCountByOption(Profile.UserNo, codeNo);

            list = new StoreProductBiz().getProductsByOption(Profile.UserNo, codeNo);

            PagerInfo pager = new PagerInfo();
            pager.CurrentPageIndex = page;
            pager.PageSize = pageSize;
            pager.RecordCount = totalCnt;
            PagerQuery<PagerInfo, IList<StoreProductT>> model = new PagerQuery<PagerInfo, IList<StoreProductT>>(pager, list);

            return View(model);
        }

    }
}
