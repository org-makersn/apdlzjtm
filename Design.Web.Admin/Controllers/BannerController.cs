using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Makersn.BizDac;
using Makersn.Models;
using PagedList;
using Makersn.Util;
using Design.Web.Admin.Models;

namespace Design.Web.Admin.Controllers
{
    [Authorize]
    public class BannerController : BaseController
    {
        BannerDac bannerDac = new BannerDac();

        private MenuModel menuModel = new MenuModel();

        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Management";
            menuModel.MainIndex = 4;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }

        public ActionResult Index(string sfl = null, string query = null, int page = 1, int type = 0)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);

            ViewBag.BannerType = type;

            //Log.Debug("BannerController.Index() called");
            IList<BannerT> list = null;
            if (!string.IsNullOrEmpty(query))
            {
                list = bannerDac.GetBannerLstByQuery(sfl, query.Trim(), type);
            }
            else
            {
                list = bannerDac.GetAllBannerLst(type);
            }

            ViewData["query"] = query;
            ViewData["cnt"] = list.Count;

            return View(list.OrderByDescending(o => o.No).ToPagedList(page, 20));
        }

        public ActionResult Write(int type)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            ViewBag.BannerType = type;
            return View();
        }

        [HttpPost]
        public ActionResult BannerAdd(string title, string publish, string opener, string link, int priority, int bannerType, HttpPostedFileBase image)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            var fileName = string.Empty;
            if (image != null)
            {
                fileName = FileUpload.UploadFile(image, new ImageSize().GetBannerResize(), "Banner", null);
            }

            BannerT bannerT = new BannerT();
            bannerT.Type = bannerType;
            bannerT.Title = title;
            bannerT.PublishYn = publish;
            bannerT.OpenerYn = opener;
            bannerT.Link = link;
            bannerT.Source = null;
            bannerT.Term = null;
            bannerT.Image = fileName;
            bannerT.Priority = priority;
            bannerT.RegId = "admin";
            bannerT.RegDt = DateTime.Now;
            var result = bannerDac.InsertBanner(bannerT);
            
            return Redirect("/banner?type=" + bannerType);
        }

        public ActionResult Edit(int no)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            BannerT bannerT = bannerDac.GetBannerByNo(no);
            ViewBag.BannerType = bannerT.Type;
            return View(bannerT);
        }

        [HttpPost]
        public ActionResult BannerUpdate(int bannerNo, string title, string publish, string opener, string link, int priority,int bannerType, HttpPostedFileBase image, string up_image_del)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            BannerT bannerT = new BannerT();
            bannerT.No = bannerNo;
            bannerT.Type = bannerType;
            bannerT.Title = title;
            bannerT.PublishYn = publish;
            bannerT.OpenerYn = opener;
            bannerT.Link = link;
            bannerT.Source = null;
            bannerT.Term = null;

            var fileName = string.Empty;

            if (up_image_del == "y" || String.IsNullOrEmpty(up_image_del))
            {
                //파일 삭제
                if (up_image_del == "y")
                {
                    bannerT.Image = fileName;
                }
                //이미지 변경
                if (image != null)
                {
                    fileName = FileUpload.UploadFile(image, new ImageSize().GetBannerResize(), "Banner", null);
                    bannerT.Image = fileName;
                }
            }
            bannerT.Priority = priority;
            bannerT.UpdId = "admin";
            bannerT.UpdDt = DateTime.Now;

            bannerDac.UpdateBannerById(bannerT);

            return Redirect("/banner?type=" + bannerType);
        }

        [HttpPost]
        public JsonResult BannerDelete(int bannerNo)
        {
            bannerDac.DeleteBannerByNo(bannerNo);
            return Json(new AjaxResponseModel { Success = true, Message = "수정 되었습니다." });
        }

    }
}
