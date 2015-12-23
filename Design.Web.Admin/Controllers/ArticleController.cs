using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Threading;
using Design.Web.Admin.Helper;
using Makersn.Util;
using Makersn.BizDac;
using Makersn.Models;
using Design.Web.Admin.Models;

namespace Design.Web.Admin.Controllers
{
    [Authorize]
    public class ArticleController : BaseController
    {
        ArticleDac articleDac = new ArticleDac();

        private MenuModel menuModel = new MenuModel();

        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Article";
            menuModel.MainIndex = 2;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }

        public ActionResult Index(int page = 1, string orderby = "regdt", int cate = 0, string visibility = "", string option = "", string text = "")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);

            IList<ArticleT> list = articleDac.GetArticleListByAdminPage(cate,"",visibility, text);
            ViewData["cnt"] = list.Count;
            ViewData["cateList"] = articleDac.GetArticleCodeNo();
            ViewData["setCate"] = cate;
            ViewData["visibility"] = visibility;
            ViewData["text"] = text;

            if (orderby == "pop")
            {
                return View(list.OrderByDescending(o => o.Pop).ToPagedList(page, 20));
            }
            if (orderby == "recommend")
            {
                list = list.Where(w => w.RecommendYn == "Y").ToList<ArticleT>();
            }
            if(orderby == "download")
            {
                return View(list.OrderByDescending(o=>o.DownloadCnt).ThenByDescending(o => o.RegDt).ToPagedList(page, 20));
            }
            if (orderby == "comment")
            {
                return View(list.OrderByDescending(o => o.CommentCnt).ThenByDescending(o => o.RegDt).ToPagedList(page, 20));
            }


            return View(list.OrderByDescending(o => o.RegDt).ToPagedList(page, 20));
        }

        public JsonResult SetVisibiliy(string no = "", string setVisi = "")
        {
            articleDac.UpdateVisibility(no, setVisi);
            return Json(new { result = 1 });
        }

        public JsonResult SetRecommend(string no = "", string setNo = "")
        {
            articleDac.UpdateRecommend(no, setNo);
            return Json(new { result = 1 });
        }

        public ActionResult State(string start = "", string end = "")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            if (start == "") { start = DateTime.Now.ToString("yyyy-MM-dd"); }
            if (end == "") { end = DateTime.Now.ToString("yyyy-MM-dd"); }
            IList<ArticleStateT> returnLst = articleDac.SearchArticleStateTargetSaerch(start, end);

            ViewBag.Start = start;
            ViewBag.End = end;

            return View(returnLst);
        }

        public ActionResult DailyState(string year, string month)
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
            IList<ArticleStateT> returnLst = new List<ArticleStateT>();
            IList<ArticleStateT> statelst = articleDac.SearchArticleStateTargetDaily(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());
            for (int day = 1; day <= days; day++)
            {
                string stringDt = firstDay.AddDays(day - 1).ToShortDateString();

                ArticleStateT retState = statelst.SingleOrDefault(g => Convert.ToDateTime(g.Gbn).ToShortDateString() == stringDt);
                if (retState != null)
                {
                    returnLst.Add(retState);
                }
                else
                {
                    returnLst.Add(new ArticleStateT());
                }
            }

            Dictionary<string, string> monthlst = new Dictionary<string, string>();

            foreach (int item in Enum.GetValues(typeof(MakersnEnumTypes.MonthLst)))
            {
                monthlst.Add(item + "월", Convert.ToString(item));
            }

            ViewData["Month"] = new SelectList(monthlst, "Value", "Key", makeDt.Month);

            //연도 셀렉트 아이템
            IList<object> yearGroup = articleDac.GetArticleYearGroup();

            Dictionary<string, string> YearList = new Dictionary<string, string>();

            foreach (int item in yearGroup)
            {
                YearList.Add(item + "년", Convert.ToString(item));
            }

            ViewData["year"] = new SelectList(YearList, "Value", "Key", year);

            return View(returnLst);
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
                makeDt = Convert.ToDateTime(year+"-01");
            }

            DateTime firstDay = new DateTimeHelper().FirstDayOfYear(makeDt);
            DateTime lastDay = new DateTimeHelper().LastDayOfYear(makeDt);

            IList<ArticleStateT> returnLst = new List<ArticleStateT>();
            IList<ArticleStateT> statelst = articleDac.SearchArticleStateTargetMonth(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());
            
            foreach (int item in Enum.GetValues(typeof(MakersnEnumTypes.MonthLst)))
            {
                ArticleStateT retState = statelst.SingleOrDefault(g => Convert.ToInt32(g.Gbn) == item);
                if (retState != null)
                {
                    returnLst.Add(retState);
                }
                else
                {
                    returnLst.Add(new ArticleStateT());
                }
            }

            //연도 셀렉트 아이템
            IList<object> yearGroup = articleDac.GetArticleYearGroup();

            Dictionary<string, string> YearList = new Dictionary<string, string>();

            foreach (int item in yearGroup)
            {
                YearList.Add(item + "년", Convert.ToString(item));
            }

            ViewData["year"] = new SelectList(YearList, "Value", "Key", year);

            return View(returnLst);
        }

        public ActionResult YearState()
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            IList<ArticleStateT> returnLst = articleDac.SearchArticleStateTargetYear();

            return View(returnLst);
        }
        
       
    }
}
