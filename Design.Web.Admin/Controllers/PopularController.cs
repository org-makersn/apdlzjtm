using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Makersn.BizDac;
using Makersn.Models;
using PagedList;
using Design.Web.Admin.Models;

namespace Design.Web.Admin.Controllers
{
    [Authorize]
    public class PopularController : BaseController
    {
        PopularDac pd = new PopularDac();

        private MenuModel menuModel = new MenuModel();

        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Management";
            menuModel.MainIndex = 4;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }

        public ActionResult Index(int page = 1)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(5);

            IList<PopularStateT> list = pd.SelectPopular();
            ViewData["cnt"] = list.Count;
            return View(list.OrderByDescending(o => o.TotalCnt).ToPagedList(page, 20));
        }

    }
}
