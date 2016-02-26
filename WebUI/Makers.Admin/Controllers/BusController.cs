using Common.Func;
using Makers.Admin.Helper;
using Makers.Admin.Models;
using Net.Common.Configurations;
using Net.Common.Filter;
using Net.Common.Helper;
using Net.Framework.BizDac;
using Net.Framework.Entity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makers.Admin.Controllers
{
    [Authorize]
    public class BusController : BaseController
    {
        BusManageDac busManageDac = new BusManageDac();

        private MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Bus";
            menuModel.MainIndex = 4;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }

        #region Dashboard
        /// <summary>
        /// Dashboard
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["Group"] = MenuModel(0);

            ViewBag.MakerBusState = busManageDac.GetMakerbusState();

            ViewData["HistoryCnt"] = busManageDac.getBusHistoryTotalCount();
            ViewData["BlogCnt"] = busManageDac.GetBusBlogTotalCount();

            IList<BusTextbook> textbooks = busManageDac.GetBusTextbookLatest(0, 10);
            return View(textbooks);
        }

        /// <summary>
        /// 교재 업로드
        /// </summary>
        /// <param name="textbook"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PostTextbook(HttpPostedFileBase textbookFile)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            if (textbookFile != null)
            {
                string filename = string.Empty;
                filename = new UploadFunc().FileUpload(textbookFile, null, "Textbook", null);

                BusTextbook textbook = new BusTextbook();
                textbook.VERSION = textbookFile.FileName;
                textbook.RENAME = filename;
                textbook.MIME_TYPE = textbookFile.ContentType;
                textbook.DOWNLOAD_CNT = 0;
                textbook.REG_DT = DateTime.Now;
                textbook.REG_ID = Profile.UserNm;

                int ret = busManageDac.AddTextbook(textbook);
                if (ret > 0)
                {
                    response.Success = true;
                    response.Result = ret.ToString();
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteTextbook(string no)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;
            int bookNo = 0;
            if (int.TryParse(no, out bookNo))
            {
                BusTextbook textbook = busManageDac.GetBusTextbookByNo(bookNo);
                if (textbook != null)
                {
                    string bookPath = string.Format(@"{0}\{1}\{2}", ApplicationConfiguration.Instance.FileServerUncPath, ApplicationConfiguration.BusConfiguration.Instance.TextbookFile, textbook.RENAME);

                    new FileHelper().FileDelete(bookPath);
                    bool ret = busManageDac.DeleteTextbook(textbook);
                    if (ret)
                    {
                        response.Success = true;
                    }
                }
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region History
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult History(int no = 0, int page = 1, string mode = "list")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);

            if (mode.Contains("add"))
            {
                return View("AddHistory");
            }
            else if (mode.Contains("edit"))
            {
                BusHistory history = busManageDac.GetBusHistoryByNo(no);
                return View("UpdateHistory", history);
            }
            else
            {
                IList<BusHistory> list = busManageDac.GetBusHistoryAll();

                ViewData["cnt"] = list.Count;

                return View(list.ToPagedList(page, 50));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PostHistory(FormCollection collection)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;

            string paramMode = collection["mode"];

            string paramTitle = collection["TITLE"];
            string paramProgressDt = collection["PROGRESS_DT"];
            string paramUseYn = collection["USE_YN"];

            if (paramMode.Contains("add"))
            {
                BusHistory history = new BusHistory();
                history.TITLE = paramTitle;
                history.PROGRESS_DT = paramProgressDt;
                history.USE_YN = paramUseYn;
                history.REG_DT = DateTime.Now;
                history.REG_ID = Profile.UserNm;

                int ret = busManageDac.AddHistory(history);
                if (ret > 0)
                {
                    response.Success = true;
                    response.Result = ret.ToString();
                }
            }

            if (paramMode.Contains("edit"))
            {
                int no = Convert.ToInt32(collection["NO"]);
                BusHistory history = busManageDac.GetBusHistoryByNo(no);
                if (history != null)
                {
                    history.TITLE = paramTitle;
                    history.PROGRESS_DT = paramProgressDt;
                    history.USE_YN = paramUseYn;
                    history.UPD_DT = DateTime.Now;
                    history.UPD_ID = Profile.UserNm;

                    bool ret = busManageDac.UpdateHistory(history);
                    if (ret)
                    {
                        response.Success = true;
                        response.Result = history.NO.ToString();
                    }
                }
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Blog
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Blog(int no = 0, int page = 1, string mode = "list")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(2);

            if (mode.Contains("add"))
            {
                return View("AddBlog");
            }
            else if (mode.Contains("edit"))
            {
                BusBlog blog = busManageDac.GetBlogByNo(no);
                blog.BLOG_CONTENTS = HtmlFilter.PunctuationDecode(blog.BLOG_CONTENTS);
                return View("UpdateBlog", blog);
            }
            else
            {
                int pageSize = 40;

                IList<BusBlog> list = null;

                int fromIndex = ((page - 1) * pageSize) + 1;
                int toIndex = page * pageSize;

                int totalCnt = busManageDac.GetBusBlogTotalCount();

                list = busManageDac.GetBusBlogAll();

                PagerInfo pager = new PagerInfo();
                pager.CurrentPageIndex = page;
                pager.PageSize = pageSize;
                pager.RecordCount = totalCnt;
                PagerQuery<PagerInfo, IList<BusBlog>> model = new PagerQuery<PagerInfo, IList<BusBlog>>(pager, list);

                ViewData["cnt"] = totalCnt;

                return View(model);
            }
        }

        /// <summary>
        /// 블로그 추가/수정
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PostBlog(FormCollection collection, HttpPostedFileBase THUMB_NAME)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;

            string paramMode = collection["mode"];

            string paramTitle = collection["BLOG_TITLE"];
            string paramBlogContents = Uri.UnescapeDataString(collection["BLOG_CONTENTS"]);
            HttpPostedFileBase paramThumb = THUMB_NAME;
            bool paramDelThumb = collection["up_thumb_del"] == "true";
            string paramUseYn = collection["USE_YN"];

            string fileName = string.Empty;
            if (paramMode.Contains("add"))
            {
                BusBlog blog = new BusBlog();
                if (paramThumb != null)
                {
                    fileName = new UploadFunc().FileUpload(paramThumb, ImageReSize.GetBlogMainResize(), "Blog", null);

                    blog.THUMB_NAME = paramThumb.FileName;
                    blog.THUMB_RENAME = fileName;
                }
                blog.BLOG_TITLE = paramTitle;
                blog.BLOG_CONTENTS = paramBlogContents;
                blog.AUTHOR = Profile.UserNo;
                blog.VIEW_CNT = 0;
                blog.USE_YN = paramUseYn;
                blog.REG_DT = DateTime.Now;
                blog.REG_ID = Profile.UserNm;

                long ret = busManageDac.AddBlog(blog);
                if (ret > 0)
                {
                    response.Success = true;
                    response.Result = ret.ToString();
                }
            }

            if (paramMode.Contains("edit"))
            {
                long blogNo = long.Parse(collection["NO"]);

                BusBlog blog = busManageDac.GetBlogByNo(blogNo);
                blog.BLOG_TITLE = paramTitle;
                blog.BLOG_CONTENTS = paramBlogContents;
                if (paramThumb != null)
                {
                    if (paramDelThumb)
                    {
                        string backupPath = string.Format(@"{0}\{1}\{2}", ApplicationConfiguration.Instance.FileServerUncPath, ApplicationConfiguration.BusConfiguration.Instance.BlogBackupImg, blog.THUMB_RENAME);
                        string thumbPath = string.Format(@"{0}\{1}\{2}", ApplicationConfiguration.Instance.FileServerUncPath, ApplicationConfiguration.BusConfiguration.Instance.BlogThumbnail, blog.THUMB_RENAME);

                        new FileHelper().FileDelete(backupPath);
                        new FileHelper().FileDelete(thumbPath);

                        fileName = new UploadFunc().FileUpload(paramThumb, ImageReSize.GetBlogMainResize(), "Blog", null);

                        blog.THUMB_NAME = paramThumb.FileName;
                        blog.THUMB_RENAME = fileName;
                    }
                }
                blog.USE_YN = paramUseYn;
                blog.UPD_DT = DateTime.Now;
                blog.UPD_ID = Profile.UserNm;

                bool ret = busManageDac.UpdateBlog(blog);
                if (ret)
                {
                    response.Success = true;
                    response.Result = blog.NO.ToString();
                }
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteBlog(int no)
        {
            return Json(true);
        } 
        #endregion

        #region Apply - 메이커버스 신청 리스트
        /// <summary>
        /// 메이커버스 신청
        /// </summary>
        /// <param name="no"></param>
        /// <param name="page"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public ActionResult Apply(int no = 0, int page = 1, string mode = "list")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(3);

            if (mode.Contains("edit"))
            {
               List<BusApplySchoolT> list = busManageDac.GetMakerbusList();
               list = list.OrderByDescending(p => p.EVENT_DATE).ToList();
                return View("UpdateApplyMakerBus", list);
            }
            else
            {
                IList<BusApplySchoolT> list = busManageDac.GetMakerbusList();
                list = list.OrderByDescending(p => p.EVENT_DATE).ToList();

                ViewData["cnt"] = list.Count;

                return View(list.ToPagedList(page, 30));
            }
        }
        #endregion

        #region Qna - 메이커버스 문의사항 리스트
        /// <summary>
        /// 메이커버스 문의사항 리스트
        /// </summary>
        /// <param name="no"></param>
        /// <param name="page"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public ActionResult Qna(int no = 0, int page = 1, string mode = "list")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(4);

            IList<BusQnaT> list = busManageDac.GetMakerbusQnaList();
            list = list.OrderByDescending(p => p.REG_DT).ToList();

            ViewData["cnt"] = list.Count;

            return View(list.ToPagedList(page, 30));
        }
        #endregion

        #region Faq - 메이커버스 자주묻는질문
        /// <summary>
        /// 메이커버스 자주묻는질문 리스트
        /// </summary>
        /// <param name="no"></param>
        /// <param name="page"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public ActionResult Faq(int no = 0, int page = 1, string mode = "list")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(5);

            IList<BusFaqT> list = busManageDac.GetMakersbusFaqList();
            list = list.OrderByDescending(p => p.REG_DT).ToList();

            ViewData["cnt"] = list.Count;

            return View(list.ToPagedList(page, 30));
        }
        #endregion

        #region Partnership - 메이커버스 파트너쉽 문의사항
        /// <summary>
        /// 메이커버스 파트너쉽 문의사항
        /// </summary>
        /// <param name="no"></param>
        /// <param name="page"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public ActionResult PartnershipQna(int no = 0, int page = 1, string mode = "list")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(6);

            IList<BusPartnershipQnaT> list = busManageDac.GetMakersbusPartnershipQnaList();
            list = list.OrderByDescending(p => p.REG_DT).ToList();

            ViewData["cnt"] = list.Count;

            return View(list.ToPagedList(page, 30));
        }
        #endregion

        #region Partner - 메이커스 파트너 리스트
        /// <summary>
        /// 메이커스 파트너 리스트
        /// </summary>
        /// <param name="no"></param>
        /// <param name="page"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public ActionResult Partnership(int no = 0, int page = 1, string mode = "list")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(7);

            IList<BusPartnerT> list = busManageDac.GetMakersPartnerList();
            list = list.OrderByDescending(p => p.REG_DT).ToList();

            ViewData["cnt"] = list.Count;

            return View(list.ToPagedList(page, 30));
        }
        #endregion
    }
}
