using Makersn.BizDac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Makersn.Models;
using PagedList;
using Design.Web.Admin.Filter;
using Design.Web.Admin.Models;



namespace Design.Web.Admin.Controllers
{
    [Authorize]
    public class NoticesController : BaseController
    {
        private NoticesDac nd = new NoticesDac();

        private MenuModel menuModel = new MenuModel();

        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Management";
            menuModel.MainIndex = 4;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }

        public ActionResult Index(int page = 1, string text="", string gubun = "", string langFlag = "KR")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);

            IList<BoardT> list;
            if (text.Trim() == "") { list = nd.GetBoardList(langFlag); }
            else { list = nd.GetNoticesByContent(text, gubun, langFlag); }
            ViewData["gubun"] = gubun;
            ViewData["cnt"] = list.Count;
            ViewData["text"] = text;
            ViewBag.Flag = langFlag;
            return View(list.OrderByDescending(o => o.No).ToPagedList(page, 20));
        }

        public ActionResult View(int no)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);
            BoardT notices = nd.GetBoard(no);
            notices.SemiContent = new ContentFilter().HtmlDecode(notices.SemiContent);
            ViewData["pre"] = nd.GetPreBoard(no);
            ViewData["next"] = nd.GetNextBoard(no);
            return View(notices);
        }

        public ActionResult write(string flag="KR")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);
            ViewData["Name"] = "admin"; 
            ViewBag.Flag = flag;
            return View();
        }

        [HttpPost]
        public JsonResult AddNotices(string title, string content, string flag)
        {
            //content = new ContentFilter().HtmlEncode(content);
            BoardT n = new BoardT();
            n.Title = title;
            n.SemiContent = content;
            n.LangFlag = flag;
            n.BoardSetNo = 1;
            n.Writer = "admin";
            n.RegId = "admin";
            n.Visibility = "N";
            n.RegDt = DateTime.Now;
            n.Cnt = 0;

            nd.InsertBoard(n);
            return Json(new AjaxResponseModel { Success=true, Message="저장되었습니다." });
        }

        public JsonResult Delete(int no)
        {
            BoardT n = new BoardT();
            n.No = no;
            nd.DeleteBoard(n);
            return Json(new AjaxResponseModel { Success=true, Message = "삭제되었습니다."});
        }

        public ActionResult Edit(int no)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);
            BoardT n = nd.GetBoard(no);
            return View(n);
        }

        public JsonResult UpdateNotices(int no,string title, string content)
        {
            //content = new ContentFilter().HtmlEncode(content);
            BoardT n = new BoardT();
            n.No = no;
            n.Title = title;
            n.SemiContent = content;
            n.UpdDt = DateTime.Now;
            n.UpdId = "admin";
            nd.UpdateBoard(n);
            return Json(new AjaxResponseModel { Success = true, Message = "수정되었습니다." });
        }
    }
}
