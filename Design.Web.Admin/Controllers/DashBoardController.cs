using System.Collections.Generic;
using System.Web.Mvc;
using Makersn.BizDac;
using Makersn.Models;
using Design.Web.Admin.Models;

namespace Design.Web.Admin.Controllers
{
    [Authorize]
    public class DashBoardController : BaseController
    {
        private MenuModel menuModel = new MenuModel();

        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_DashBoard";
            menuModel.MainIndex = 0;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }

        public ActionResult Index()
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);

            IList<DashBoardStateT> DashBoardList = new MemberDac().SearchDashBoardStateTargetAll();
            //IList<MemberStateT> memberlst = new MemberDac().SearchMemberStateTargetAll();

            ViewData["DashBoardList"] = DashBoardList;
            return View();
        }

    }
}
