using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Makersn.Models;
using Makersn.BizDac;
using PagedList;
using Design.Web.Admin.Models;
using Makersn.Util;
using System.Net;
using System;
using Design.Web.Admin.Helper;

namespace Design.Web.Admin.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        ReportDac rd = new ReportDac();

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

            ViewData["Group"] = MenuModel(4);
            IList<ReportT> list = rd.getReportList();
            return View(list.OrderByDescending(o => o.No).ToPagedList(page, 20));
        }

        public JsonResult SetVisibility(int no,string visibility)
        {
            ArticleDac ad = new ArticleDac();
            ad.UpdateVisibility(no.ToString(), visibility);
            return Json(new AjaxResponseModel { Success = true });
        }

        public ActionResult Edit(int no)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(3);
            ReportT r = rd.getReportByNo(no);

            ViewData.Add("State", new SelectList(EnumHelper.GetItemValueList<MakersnEnumTypes.ReportState>(), "Key", "Value", r.State));

            return View(r);
        }

        public JsonResult Delete(int no)
        {
            rd.DeleteReport(no);
            return Json(new AjaxResponseModel { Success = true });
        }

        public JsonResult SendMessgae(string registerComment, string reporterComment, int no, int state)
        {
            
            ReportT r = new ReportT();
            r.RegisterComment = registerComment;
            r.ReporterComment = reporterComment;
            r.No = no;
            r.State = state;
            r.UpdDt = DateTime.Now;
            r.UpdId = "admin"; // 세션 추가해야됨
            r.RegIp = IPAddressHelper.GetIP(this);

            rd.UpdateState(r);
            return Json(new AjaxResponseModel { Success = true });
        }

    }
}
