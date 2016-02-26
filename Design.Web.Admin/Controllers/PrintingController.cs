using Design.Web.Admin.Helper;
using Design.Web.Admin.Models;
using Makersn.BizDac;
using Makersn.Models;
using Makersn.Util;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using PagedList;
using Makersn.Util;
using System.Linq;

namespace Design.Web.Admin.Controllers
{
    [Authorize]
    public class PrintingController : BaseController
    {

        ArticleDac articleDac = new ArticleDac();
        PrinterDac printerDac = new PrinterDac();
        MemberDac memberDac = new MemberDac();
        private MenuModel menuModel = new MenuModel();
        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Printing";
            menuModel.MainIndex = 3;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Listing(int page = 1, string dateGubun="", string start = "", string end = "", string txtGubun = "", string text = "", int state=0)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            IList<PrinterStateT> list = printerDac.GetPrintingListForAdmin(dateGubun,start,end,txtGubun,text,state);
            ViewData["Group"] = MenuModel(1);

            if (start == "") { start = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"); }
            if (end == "") { end = DateTime.Now.ToString("yyyy-MM-dd"); }

            ViewBag.Start = start;
            ViewBag.End = end;
            ViewData["text"] = text;

            Dictionary<int, string> stateList = new Dictionary<int, string>();
            stateList.Add(0, "전체");

            foreach (var a in EnumHelper.GetItemValueList<MakersnEnumTypes.OrderState>())
            {
                stateList.Add(a.Key, a.Value);
            }

            ViewData.Add("state", new SelectList(stateList, "Key", "Value", state));

            return View(list.ToPagedList(page, 20));
        }
        public ActionResult State(string start = "", string end = "")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            if (start == "") { start = DateTime.Now.ToString("yyyy-MM-dd"); }
            if (end == "") { end = DateTime.Now.ToString("yyyy-MM-dd"); }
            IList<PrinterMemberStateT> returnLst = memberDac.SearchPrinterMemberStateTargetAll(start, end);

            ViewBag.Start = start;
            ViewBag.End = end;

            return View(returnLst);
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
            IList<PrinterMemberStateT> returnlst = new List<PrinterMemberStateT>();
            IList<PrinterMemberStateT> statelst = memberDac.SearchPrinterMemberStateTargetDaily(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());

            for (int day = 1; day <= days; day++)
            {
                string stringDt = firstDay.AddDays(day - 1).ToShortDateString();

                PrinterMemberStateT retState = statelst.SingleOrDefault(g => Convert.ToDateTime(g.Gbn).ToShortDateString() == stringDt);
                if (retState != null)
                {
                    returnlst.Add(retState);
                }
                else
                {
                    returnlst.Add(new PrinterMemberStateT());
                }
            }

            Dictionary<string, string> monthlst = new Dictionary<string, string>();

            foreach (int item in Enum.GetValues(typeof(MakersnEnumTypes.MonthLst)))
            {
                monthlst.Add(item + "월", Convert.ToString(item));
            }

            ViewData["Month"] = new SelectList(monthlst, "Value", "Key", makeDt.Month);

            //연도 셀렉트 아이템
            IList<object> yearGroup = memberDac.GetMemberYearGroup();

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
            IList<PrinterMemberStateT> returnlst = new List<PrinterMemberStateT>();
            IList<PrinterMemberStateT> statelst = memberDac.SearchPrinterMemberStateTargetMonth(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());

            foreach (int item in Enum.GetValues(typeof(MakersnEnumTypes.MonthLst)))
            {
                PrinterMemberStateT retState = statelst.SingleOrDefault(g => Convert.ToInt32(g.Gbn) == item);
                if (retState != null)
                {
                    returnlst.Add(retState);
                }
                else
                {
                    returnlst.Add(new PrinterMemberStateT());
                }
            }

            //연도 셀렉트 아이템
            IList<object> yearGroup = memberDac.GetMemberYearGroup();

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
            IList<PrinterMemberStateT> statelst = memberDac.SearchPrinterMemberStateTargetYear();
            return View(statelst);
        }

    }
}
