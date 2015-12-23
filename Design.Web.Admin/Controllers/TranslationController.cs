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
    public class TranslationController : BaseController
    {

        private MenuModel menuModel = new MenuModel();

        TranslationDac _translationDac = new TranslationDac();
        TranslationDetailDac _translationDetailDac = new TranslationDetailDac();
        ArticleDac _articleDac = new ArticleDac();
        ArticleFileDac _articleFileDac = new ArticleFileDac();

        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Trans";
            menuModel.MainIndex = 5;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }

        public ActionResult Index(string start = "", string end = "")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);

            if (start == "") { start = DateTime.Now.ToString("yyyy-MM-dd"); }
            if (end == "") { end = DateTime.Now.ToString("yyyy-MM-dd"); }

            IList<TranslationStateT> list = _translationDac.GetTranslationStatus(start, end);

            ViewBag.Start = start;
            ViewBag.End = end;

            return View(list);
        }

        #region 변역현황

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
            IList<TranslationStateT> returnLst = new List<TranslationStateT>();
            IList<TranslationStateT> statelst = _translationDac.GetTranslationStatusTargetDaily(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());
            for (int day = 1; day <= days; day++)
            {
                string stringDt = firstDay.AddDays(day - 1).ToShortDateString();

                TranslationStateT retState = statelst.SingleOrDefault(g => Convert.ToDateTime(g.Gbn).ToShortDateString() == stringDt);
                if (retState != null)
                {
                    returnLst.Add(retState);
                }
                else
                {
                    returnLst.Add(new TranslationStateT());
                }
            }

            Dictionary<string, string> monthlst = new Dictionary<string, string>();

            foreach (int item in Enum.GetValues(typeof(MakersnEnumTypes.MonthLst)))
            {
                monthlst.Add(item + "월", Convert.ToString(item));
            }

            ViewData["Month"] = new SelectList(monthlst, "Value", "Key", makeDt.Month);

            //연도 셀렉트 아이템
            IList<object> yearGroup = _translationDac.GetArticleYearGroup();

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
                makeDt = Convert.ToDateTime(year + "-01");
            }

            DateTime firstDay = new DateTimeHelper().FirstDayOfYear(makeDt);
            DateTime lastDay = new DateTimeHelper().LastDayOfYear(makeDt);

            IList<TranslationStateT> returnLst = new List<TranslationStateT>();
            IList<TranslationStateT> statelst = _translationDac.GetTranslationStatusTargetMonth(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());

            foreach (int item in Enum.GetValues(typeof(MakersnEnumTypes.MonthLst)))
            {
                TranslationStateT retState = statelst.SingleOrDefault(g => Convert.ToInt32(g.Gbn) == item);
                if (retState != null)
                {
                    returnLst.Add(retState);
                }
                else
                {
                    returnLst.Add(new TranslationStateT());
                }
            }

            //연도 셀렉트 아이템
            IList<object> yearGroup = _translationDac.GetArticleYearGroup();

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
            IList<TranslationStateT> returnLst = _translationDac.GetTranslationStatusTargetYear();

            return View(returnLst);
        }

        #endregion

        #region 번역요청
        //번역요청 현황 초기 화면
        public ActionResult TranslationReqState(string start = "", string end = "")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);

            if (start == "") { start = DateTime.Now.ToString("yyyy-MM-dd"); }
            if (end == "") { end = DateTime.Now.ToString("yyyy-MM-dd"); }

            IList<TranslationStateRequestT> list = _translationDac.GetTranslationRequestStatus(start, end);

            ViewBag.Start = start;
            ViewBag.End = end;

            return View(list);
        }

        public ActionResult DailyStateRequest(string year, string month)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);
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
            IList<TranslationStateRequestT> returnLst = new List<TranslationStateRequestT>();
            IList<TranslationStateRequestT> statelst = _translationDac.GetTranslationRequestStatusTargetDaily(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());
            for (int day = 1; day <= days; day++)
            {
                string stringDt = firstDay.AddDays(day - 1).ToShortDateString();

                TranslationStateRequestT retState = statelst.SingleOrDefault(g => Convert.ToDateTime(g.Gbn).ToShortDateString() == stringDt);
                if (retState != null)
                {
                    returnLst.Add(retState);
                }
                else
                {
                    returnLst.Add(new TranslationStateRequestT());
                }
            }

            Dictionary<string, string> monthlst = new Dictionary<string, string>();

            foreach (int item in Enum.GetValues(typeof(MakersnEnumTypes.MonthLst)))
            {
                monthlst.Add(item + "월", Convert.ToString(item));
            }

            ViewData["Month"] = new SelectList(monthlst, "Value", "Key", makeDt.Month);

            //연도 셀렉트 아이템
            IList<object> yearGroup = _translationDac.GetArticleYearGroup();

            Dictionary<string, string> YearList = new Dictionary<string, string>();

            foreach (int item in yearGroup)
            {
                YearList.Add(item + "년", Convert.ToString(item));
            }

            ViewData["year"] = new SelectList(YearList, "Value", "Key", year);

            return View(returnLst);
        }

        public ActionResult MonthStateRequest(string year = null)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);
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

            IList<TranslationStateRequestT> returnLst = new List<TranslationStateRequestT>();
            IList<TranslationStateRequestT> statelst = _translationDac.GetTranslationRequestStatusTargetMonth(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());

            foreach (int item in Enum.GetValues(typeof(MakersnEnumTypes.MonthLst)))
            {
                TranslationStateRequestT retState = statelst.SingleOrDefault(g => Convert.ToInt32(g.Gbn) == item);
                if (retState != null)
                {
                    returnLst.Add(retState);
                }
                else
                {
                    returnLst.Add(new TranslationStateRequestT());
                }
            }

            //연도 셀렉트 아이템
            IList<object> yearGroup = _translationDac.GetArticleYearGroup();

            Dictionary<string, string> YearList = new Dictionary<string, string>();

            foreach (int item in yearGroup)
            {
                YearList.Add(item + "년", Convert.ToString(item));
            }

            ViewData["year"] = new SelectList(YearList, "Value", "Key", year);

            return View(returnLst);
        }

        public ActionResult YearStateRequest()
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);
            IList<TranslationStateRequestT> returnLst = _translationDac.GetTranslationRequestStatusTargetYear();

            return View(returnLst);
        }

        //번역 요청 리스트
        public ActionResult TranslationReqList(int page = 1, string orderby = "regdt", int cate = 0, string option = "", string text = "")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);

            IList<TranslationRequestListT> list = _translationDac.GetTranslationRequestByOption(cate, text, (int)MakersnEnumTypes.TranslationFlag.번역요청, 0);
            ViewData["cnt"] = list.Count;
            ViewData["cateList"] = new ArticleDac().GetArticleCodeNo();
            ViewData["setCate"] = cate;
            ViewData["text"] = text;

            if (orderby == "pop")
            {
                return View(list.OrderByDescending(o => o.Pop).ToPagedList(page, 20));
            }
            if (orderby == "TransReqDt")
            {
                return View(list.OrderByDescending(o => o.TransReqDt).ToPagedList(page, 20));
            }
            if (orderby == "TransWorkDt")
            {
                return View(list.OrderByDescending(o => o.TransWorkDt).ToPagedList(page, 20));
            }
            if (orderby == "regdt")
            {
                return View(list.OrderByDescending(o => o.RegDt).ToPagedList(page, 20));
            }
            return View(list.OrderByDescending(o => o.RegDt).ToPagedList(page, 20));
        }

        public JsonResult TranslationHold(string No = "")
        {
            ViewData["Group"] = MenuModel(1);
            TranslationT translation = new TranslationT();
            translation.No = int.Parse(No); ;
            translation.Status = 3;
            translation.UpdDt = DateTime.Now;
            translation.UpdId = Profile.UserId;
            _translationDac.UpdateTranslationStatus(translation);
            return Json(new { Message = "보류처리 되였습니다.", Success = true });
        }

        #region 번역 페이지
        public ActionResult TranslationWork(string no = "")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            int translationNo = 0;
            ViewData["Group"] = MenuModel(1);

            if (no == "")
            {
                return RedirectToAction("TranslationReqList");
            }

            if (!Int32.TryParse(no, out translationNo))
            {
                //history
                return RedirectToAction("TranslationReqList");
            }

            TranslationT translation = _translationDac.GetTranslationByTranslationNo(translationNo);
            TranslationDetailT transDetail = _translationDac.GetTranslationDetail(translation.ArticleNo, translation.LangFrom);

            ViewBag.TransDetailTo = _translationDetailDac.GetTranslationDetailByArticleNoAndLangFlag(translation.ArticleNo, translation.LangTo);

            return View(transDetail);
        }

        #endregion

        #region
        public JsonResult SetTranslation(FormCollection collection)
        {
            AjaxResponseModel model = new AjaxResponseModel();
            model.Success = false;
            int translationNo = int.Parse(collection["No"]);
            int articleNo = int.Parse(collection["ArticleNo"]);
            string transTitle = collection["trans_title"];
            string transContents = collection["trans_contents"];
            string transTags = collection["trans_tags"];

            TranslationT translation = _translationDac.GetTranslationByTranslationNo(translationNo);
            TranslationDetailT transDetailTemp = _translationDetailDac.GetTranslationDetailByArticleNoAndLangFlag(articleNo, translation.LangTo);
            //_translationDetailDac.ChkTranslationDetailAndDelete(articleNo, translation.LangTo); // 기존 번역본 있는지 체크 후 삭제

            TranslationDetailT transDetail = new TranslationDetailT();
            transDetail.No = transDetailTemp.No;
            transDetail.ArticleNo = translation.ArticleNo;
            transDetail.TranslationNo = translation.No;
            transDetail.Title = transTitle;
            transDetail.Contents = transContents;
            transDetail.Tag = transTags;
            transDetail.LangFlag = translation.LangTo;
           
            if (transDetailTemp.No > 0)
            {
                transDetail.UpdId = Profile.UserId;
                transDetail.UpdDt = DateTime.Now;
                transDetail.RegId = transDetailTemp.RegId;
                transDetail.RegDt = transDetailTemp.RegDt;
            }
            else {
                transDetail.RegId = Profile.UserId;
                transDetail.RegDt = DateTime.Now;
            }

            translation.WorkDt = DateTime.Now;
            translation.WorkMemberNo = Profile.UserNo;
            translation.UpdDt = DateTime.Now;
            translation.UpdId = Profile.UserId;

            translation.Status = (int)Makersn.Util.MakersnEnumTypes.TranslationStatus.완료;//완료
            model.Result = _translationDetailDac.SaveOrUpdateTranslationDetail(transDetail, translation).ToString();
            model.Message = "번역 되었습니다.";
            model.Success = true;


            return Json(model);
        }
        #endregion

        #endregion


        #region 직접번역
        //public ActionResult Index(int page = 1, string orderby = "regdt", int cate = 0, string visibility = "", string option = "", string text = "")
        //{
        //    ViewData["Group"] = MenuModel(1);

        //    IList<ArticleT> list = articleDac.GetArticleListByAdminPage(cate, visibility, text);
        //    ViewData["cnt"] = list.Count;
        //    ViewData["cateList"] = articleDac.GetArticleCodeNo();
        //    ViewData["setCate"] = cate;
        //    ViewData["visibility"] = visibility;
        //    ViewData["text"] = text;

        //    if (orderby == "pop")
        //    {
        //        return View(list.OrderByDescending(o => o.Pop).ToPagedList(page, 20));
        //    }
        //    if (orderby == "recommend")
        //    {
        //        list = list.Where(w => w.RecommendYn == "Y").ToList<ArticleT>();
        //    }
        //    if (orderby == "download")
        //    {
        //        return View(list.OrderByDescending(o => o.DownloadCnt).ThenByDescending(o => o.RegDt).ToPagedList(page, 20));
        //    }
        //    if (orderby == "comment")
        //    {
        //        return View(list.OrderByDescending(o => o.CommentCnt).ThenByDescending(o => o.RegDt).ToPagedList(page, 20));
        //    }


        //    return View(list.OrderByDescending(o => o.RegDt).ToPagedList(page, 20));
        //}
        public ViewResult TranslationDirectList(int page = 1, string orderby = "regdt", int cate = 0, string option = "", string text = "")
        {
            ViewData["Group"] = MenuModel(2);

            IList<ArticleT> list = _translationDac.SearchArticleWithTranslation(cate, text, (int)MakersnEnumTypes.TranslationFlag.직접번역, (int)MakersnEnumTypes.TranslationStatus.완료);
            ViewData["cnt"] = list.Count;
            ViewData["cateList"] = new ArticleDac().GetArticleCodeNo();
            ViewData["setCate"] = cate;
            ViewData["text"] = text;

            if (orderby == "pop")
            {
                return View(list.OrderByDescending(o => o.Pop).ToPagedList(page, 20));
            }
            if (orderby == "workdt")
            {
                return View(list.OrderByDescending(o => o.TransWorkDt).ToPagedList(page, 20));
            }
            return View(list.OrderByDescending(o => o.RegDt).ToPagedList(page, 20));
        }

        public ViewResult TranslationDirectState(string start = "", string end = "")
        {

            ViewData["Group"] = MenuModel(2);


            if (start == "") { start = DateTime.Now.ToString("yyyy-MM-dd"); }
            if (end == "") { end = DateTime.Now.ToString("yyyy-MM-dd"); }
            IList<TranslationStateDirectT> returnLst = _translationDac.GetTranslationDirectStatus(start, end);
            ViewBag.Start = start;
            ViewBag.End = end;
            return View(returnLst);
        }


        public ActionResult DailyStateDirect(string year, string month)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(2);
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
            IList<TranslationStateDirectT> returnLst = new List<TranslationStateDirectT>();
            IList<TranslationStateDirectT> statelst = _translationDac.GetTranslationDirectStatusTargetDaily(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());
            for (int day = 1; day <= days; day++)
            {
                string stringDt = firstDay.AddDays(day - 1).ToShortDateString();

                TranslationStateDirectT retState = statelst.SingleOrDefault(g => Convert.ToDateTime(g.Gbn).ToShortDateString() == stringDt);
                if (retState != null)
                {
                    returnLst.Add(retState);
                }
                else
                {
                    returnLst.Add(new TranslationStateDirectT());
                }
            }

            Dictionary<string, string> monthlst = new Dictionary<string, string>();

            foreach (int item in Enum.GetValues(typeof(MakersnEnumTypes.MonthLst)))
            {
                monthlst.Add(item + "월", Convert.ToString(item));
            }

            ViewData["Month"] = new SelectList(monthlst, "Value", "Key", makeDt.Month);

            //연도 셀렉트 아이템
            IList<object> yearGroup = _translationDac.GetArticleYearGroup();

            Dictionary<string, string> YearList = new Dictionary<string, string>();

            foreach (int item in yearGroup)
            {
                YearList.Add(item + "년", Convert.ToString(item));
            }

            ViewData["year"] = new SelectList(YearList, "Value", "Key", year);

            return View(returnLst);
        }

        public ActionResult MonthStateDirect(string year = null)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(2);
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

            IList<TranslationStateDirectT> returnLst = new List<TranslationStateDirectT>();
            IList<TranslationStateDirectT> statelst = _translationDac.GetTranslationDirectStatusTargetMonth(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());

            foreach (int item in Enum.GetValues(typeof(MakersnEnumTypes.MonthLst)))
            {
                TranslationStateDirectT retState = statelst.SingleOrDefault(g => Convert.ToInt32(g.Gbn) == item);
                if (retState != null)
                {
                    returnLst.Add(retState);
                }
                else
                {
                    returnLst.Add(new TranslationStateDirectT());
                }
            }

            //연도 셀렉트 아이템
            IList<object> yearGroup = _translationDac.GetArticleYearGroup();

            Dictionary<string, string> YearList = new Dictionary<string, string>();

            foreach (int item in yearGroup)
            {
                YearList.Add(item + "년", Convert.ToString(item));
            }

            ViewData["year"] = new SelectList(YearList, "Value", "Key", year);

            return View(returnLst);
        }

        public ActionResult YearStateDirect()
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(2);
            IList<TranslationStateDirectT> returnLst = _translationDac.GetTranslationDirectStatusTargetYear();

            return View(returnLst);
        }


        #endregion

    }
}
