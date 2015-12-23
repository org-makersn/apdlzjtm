using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Makersn.Models;
using Makersn.BizDac;
using PagedList;
using Makersn.Util;
using Design.Web.Admin.Helper;
using Design.Web.Admin.Models;
using Design.Web.Admin.Filter;

namespace Design.Web.Admin.Controllers
{
    [Authorize]
    public class MemberController : BaseController
    {
        MemberDac _memberDac = new MemberDac();
        NoticesDac _noticeDac = new NoticesDac();

        private MenuModel menuModel = new MenuModel();

        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Member";
            menuModel.MainIndex = 1;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }

        public ActionResult Index(string startDt = "", string endDt = "", int page = 1, string name = "", string id = "", string gubun = "")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);
            ViewData["startDt"] = startDt;
            ViewData["endDt"] = endDt;
            ViewData["option"] = "name";
            if (id != "") { ViewData["option"] = "id"; };
            ViewData["text"] = name + id;
            //if (startDt == null) { startDt = ""; };
            //if (endDt == null) { endDt = ""; };
            //List<MemberT> list = md.SelectQuery(startDt, endDt);
            IList<MemberT> list = _memberDac.SelectQuery(startDt, endDt, name, id);
            ViewData["cnt"] = list.Count;

            ViewBag.Gubun = gubun;
            if (gubun == "D")
            {
                return View(list.OrderByDescending(o => o.DownloadCnt).ThenByDescending(t => t.No).ToPagedList(page, 20));
            }
            if (gubun == "U")
            {
                return View(list.OrderByDescending(o => o.UploadCntY).ThenByDescending(t => t.No).ToPagedList(page, 20));
            }
            if (gubun == "C")
            {
                return View(list.OrderByDescending(o => o.CommentCnt).ThenByDescending(t => t.No).ToPagedList(page, 20));
            }

            return View(list.OrderByDescending(o => o.No).ToPagedList(page, 20));
        }

        public ActionResult State(string start = "", string end = "")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            if (start == "") { start = DateTime.Now.ToString("yyyy-MM-dd"); }
            if (end == "") { end = DateTime.Now.ToString("yyyy-MM-dd"); }

            IList<DashBoardStateT> returnlst = _memberDac.SearchDashBoardStateTargetAll(start, end);
            ViewBag.Start = start;
            ViewBag.End = end;

            return View(returnlst);
        }

        public ActionResult DailyState(string year = null, string month = null)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            DateTime makeDt = DateTime.Now;
            if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month))
            {
                makeDt = DateTime.Now;
            }
            else
            {
                makeDt = Convert.ToDateTime(string.Format("{0}-{1}", year, month));
            }

            int days = Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(makeDt.Year, makeDt.Month);

            DateTime firstDay = new DateTimeHelper().FirstDayOfMonth(makeDt);
            DateTime lastDay = new DateTimeHelper().LastDayOfMonth(makeDt);
            //IList<MemberStateT> returnlst = new List<MemberStateT>();
            //IList<MemberStateT> statelst = memberDac.SearchMemberStateTargetDaily(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());
            IList<DashBoardStateT> returnlst = new List<DashBoardStateT>();
            IList<DashBoardStateT> statelst = _memberDac.GetMemberStateTargetDaily(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());

            for (int day = 1; day <= days; day++)
            {
                string stringDt = firstDay.AddDays(day - 1).ToShortDateString();

                DashBoardStateT retState = statelst.SingleOrDefault(g => Convert.ToDateTime(g.Gbn).ToShortDateString() == stringDt);
                if (retState != null)
                {
                    returnlst.Add(retState);
                }
                else
                {
                    returnlst.Add(new DashBoardStateT());
                }
            }

            Dictionary<string, string> monthlst = new Dictionary<string, string>();

            foreach (int item in Enum.GetValues(typeof(MakersnEnumTypes.MonthLst)))
            {
                monthlst.Add(item + "월", Convert.ToString(item));
            }

            ViewData["Month"] = new SelectList(monthlst, "Value", "Key", makeDt.Month);

            //연도 셀렉트 아이템
            IList<object> yearGroup = _memberDac.GetMemberYearGroup();

            Dictionary<string, string> YearList = new Dictionary<string, string>();

            foreach (int item in yearGroup)
            {
                YearList.Add(item + "년", Convert.ToString(item));
            }

            ViewData["year"] = new SelectList(YearList, "Value", "Key", year);

            return View(returnlst);
        }

        public ActionResult MonthState(string year = null)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            DateTime makeDt = DateTime.Now;
            if (string.IsNullOrEmpty(year))
            {
                makeDt = Convert.ToDateTime(DateTime.Now.Year + "-01");
            }
            else
            {
                makeDt = Convert.ToDateTime(year + "-01");
            }

            DateTime firstDay = new DateTimeHelper().FirstDayOfYear(makeDt);
            DateTime lastDay = new DateTimeHelper().LastDayOfYear(makeDt);

            //IList<MemberStateT> returnlst = new List<MemberStateT>();
            //IList<MemberStateT> statelst = memberDac.SearchMemberStateTargetMonth(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());
            IList<DashBoardStateT> returnlst = new List<DashBoardStateT>();
            IList<DashBoardStateT> statelst = _memberDac.GetMemberStateTargetMonth(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());

            foreach (int item in Enum.GetValues(typeof(MakersnEnumTypes.MonthLst)))
            {
                DashBoardStateT retState = statelst.SingleOrDefault(g => Convert.ToInt32(g.Gbn) == item);
                if (retState != null)
                {
                    returnlst.Add(retState);
                }
                else
                {
                    returnlst.Add(new DashBoardStateT());
                }
            }

            //연도 셀렉트 아이템
            IList<object> yearGroup = _memberDac.GetMemberYearGroup();

            Dictionary<string, string> YearList = new Dictionary<string, string>();

            foreach (int item in yearGroup)
            {
                YearList.Add(item + "년", Convert.ToString(item));
            }

            ViewData["year"] = new SelectList(YearList, "Value", "Key", year);

            return View(returnlst);
        }

        public ActionResult YearState(string year)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            //IList<MemberStateT> statelst = memberDac.SearchMemberStateTargetYear();
            IList<DashBoardStateT> statelst = _memberDac.GetMemberStateTargetYear();
            return View(statelst);
        }

        public ActionResult DropMember(string startDt = "", string endDt = "", int page = 1, int listCnt = 20, string name = "", string id = "")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(3);
            ViewData["startDt"] = startDt;
            ViewData["endDt"] = endDt;
            ViewData["option"] = "name";
            if (id != "") { ViewData["option"] = "id"; };
            ViewData["text"] = name + id;
            IList<MemberT> list = _memberDac.DropMemberSelectQuery(startDt, endDt, name, id);
            ViewData["cnt"] = list.Count;
            ViewData["listCnt"] = listCnt;

            return View(list.OrderByDescending(o => o.No).ToPagedList(page, listCnt));
        }
        public ActionResult PrinterMember(string startDt = "", string endDt = "", int page = 1, int listCnt = 20, string name = "", string id = "")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(2);
            ViewData["startDt"] = startDt;
            ViewData["endDt"] = endDt;
            ViewData["option"] = "name";
            if (id != "") { ViewData["option"] = "id"; };
            ViewData["text"] = name + id;
            IList<PrinterMemberT> list = _memberDac.PrinterMemberSelectQuery(startDt, endDt, name, id);
            ViewData["cnt"] = list.Count;
            ViewData["listCnt"] = listCnt;
            return View(list.OrderByDescending(o => o.No).ToPagedList(page, listCnt));
        }


        //public class EnumObject
        //{
        //    public short Key { get; set; }
        //    public string Value { get; set; }
        //}

        public ActionResult AllNotice(int page = 1)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(4);

            IList<NoticeT> before = _noticeDac.GetAllNoticeList();

            IList<string> chkComment = new List<string>();
            IList<NoticeT> list = new List<NoticeT>();

            foreach(NoticeT n in before){
                if (!chkComment.Contains(n.Comment))
                {
                    list.Add(n);
                    chkComment.Add(n.Comment);
                }
            }

            return View(list.OrderByDescending(o=>o.RegDt).ToPagedList(page,20));
        }

        public ActionResult SendNotice()
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(4);
            return View();
        }

        public JsonResult AddAllNotice(string title, string content)
        {
            NoticeT notice = new NoticeT();
            notice.IdxName = "n:0";
            notice.ArticleNo = 0;
            notice.MemberNo = Profile.UserNo;
            notice.IsNew = "Y";
            notice.RefNo = 0;
            notice.Type = "AllNotice";
            notice.Comment = "제목:" + title.Replace("'","''") + "내용:" + content.Replace("'","''");
            notice.RegDt = DateTime.Now;
            notice.RegId = Profile.UserId;
            notice.RegIp = IPAddressHelper.GetClientIP();

            IList<MemberT> memberList = _memberDac.GetMemberListForSendAllNotice();

            //foreach (MemberT member in memberList)
            //{
            //    if (member.Email != "" && member.Email != "0")
            //    {
            //        try
            //        {
            //            sendNoticeEmail(member.Email, title, content);
            //        }
            //        catch
            //        {

            //        }
            //    }
            //}

            //sendNoticeEmail("astio002@nate.com", title, content);
            //sendNoticeEmail("astio022@naver.com", title, content);
            //sendNoticeEmail("chasy@makersi.com", title, content);
            //sendNoticeEmail("astio022@hanmail.net", title, content);
            //sendNoticeEmail("ryan@makersi.com", title, content);
            //강희주 위로 전송 실패 521

            bool Success = _noticeDac.InsertAllNotice(notice, memberList);
            //bool Success = true;

            return Json(new { Success = Success });
        }

        //public JsonResult sendNoticeEmail(string email, string title, string content)
        //{
        //    SendMailModels oMail = new SendMailModels();
        //    content = new ContentFilter().HtmlEncode(content);
        //    oMail.SendMail("sendNotice", email, new String[] { title, content });
        //    return Json(new AjaxResponseModel { Success = true, Message = "발송되었습니다." });
        //}
    }
}
