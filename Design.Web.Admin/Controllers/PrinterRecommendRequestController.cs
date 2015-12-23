using Design.Web.Admin.Models;
using Makersn.BizDac;
using Makersn.Models;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Design.Web.Admin.Controllers
{
    [Authorize]
    public class PrinterRecommendRequestController : BaseController
    {

        private MenuModel menuModel = new MenuModel();
        PrinterDac printerDac = new PrinterDac();
        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Management";
            menuModel.MainIndex = 4;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }
        public ActionResult Printer()
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(2);
            return View();
        }


        //Edit recommandation
        //public JsonResult SetVisibiliy(string no = "", string setVisi = "")
        //{
        //    printerDac.UpdateVisibility(no, setVisi);
        //    return Json(new { result = 1 });
        //}

        public JsonResult SetRecommend(string no = "", string setNo = "")
        {
            printerDac.UpdateRecommend(no, setNo);
            return Json(new { result = 1 });
        }


        public PartialViewResult RecommendEdit(int page = 1, string orderby = "regdt", int cate = 0, string RecommendYn = "", string option = "", string text = "")
        {
            ViewData["Group"] = MenuModel(1);
            IList<PrinterT> list = printerDac.GetSearchList(text, RecommendYn,null,null,1,0);
            ViewData["cnt"] = list.Count;
            ViewData["RecommendYn"] = RecommendYn;
            ViewData["text"] = text;

            //if (orderby == "pop")
            //{
            //    return PartialView(list.OrderByDescending(o => o.Pop).ToPagedList(page, 20));
            //}
            //if (orderby == "recommend")
            //{
            //    list = list.Where(w => w.RecommendYn == "Y").ToList<PrinterT>();
            //}
            return PartialView(list.OrderByDescending(o => o.RegDt).ToPagedList(page, 20));
        }


        //LIST recommendation
        public PartialViewResult RecommendList(int page = 1, string orderby = "regdt", string option = "", string text = "")
        {
            string RecommendYn = "Y";
            ViewData["Group"] = MenuModel(1);

            IList<PrinterT> list = printerDac.GetSearchList(text, RecommendYn,null,null,1,0);
            ViewData["cnt"] = list.Count;
            ViewData["text"] = text;

            //if (orderby == "pop")
            //{
            //    return PartialView(list.OrderByDescending(o => o.Pop).ToPagedList(page, 20));
            //}
            //if (orderby == "recommend")
            //{
            //    list = list.Where(w => w.RecommendYn == "Y").ToList<PrinterT>();
            //}
            //if (orderby == "recommendPriority")
            //{
            //    return PartialView(list.OrderByDescending(o => o.Priority).ToPagedList(page, 20));
            //}
            return PartialView(list.OrderByDescending(o => o.RecommendPriority).ThenByDescending(w => w.RecommendDt).ToPagedList(page, 20));
        }



        public ActionResult RecommendListEdit(int no)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);
            PrinterT PrinterT = printerDac.GetPrinterByNo(no);
            return PartialView(PrinterT);
        }

        public ActionResult RecommendListUpdatePriority(int no, int priority)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            printerDac.UpdatePriority(no, priority);
            return Redirect("../PrinterRecommendRequest/Printer");
        }

        public ActionResult RecommendListUpdateVisibility(int no, string visibility)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            printerDac.UpdateRecommendVisibility(no, visibility);
            return Redirect("../PrinterRecommendRequest/Printer");
        }
    }
}