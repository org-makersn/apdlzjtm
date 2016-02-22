using Common.Func;
using Makers.Admin.Models;
using Makersn.Util;
using Net.Common.Configurations;
using Net.Common.Helper;
using Net.Framework.BizDac;
using Net.Framework.StoreModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Makers.Admin.Controllers
{
    [Authorize]
    public class BannerController : BaseController
    {
        BannerExDac bannerDac = new BannerExDac();

        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Management";
            menuModel.MainIndex = 5;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sfl"></param>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Index(string sfl = null, string query = null, int page = 1, int type = 0)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);

            ViewBag.BannerType = type;

            //Log.Debug("BannerController.Index() called");
            IList<BannerExT> list = new List<BannerExT>();
            if (!string.IsNullOrEmpty(query))
            {
                list = bannerDac.GetBannerListByQuery(sfl, query.Trim(), type);
            }
            else
            {
                list = bannerDac.GetAllBannerList(type);
            }

            ViewData["sfl"] = sfl;
            ViewData["query"] = query;
            ViewData["cnt"] = list.Count;

            return View(list.ToPagedList(page, 20));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Write(int type)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            ViewBag.BannerType = type;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="publish"></param>
        /// <param name="opener"></param>
        /// <param name="link"></param>
        /// <param name="priority"></param>
        /// <param name="bannerType"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BannerAdd(string title, string publish, string opener, string link, int priority, int bannerType, HttpPostedFileBase image)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            var fileName = string.Empty;
            if (image != null)
            {
                fileName = new UploadFunc().FileUpload(image, ImageReSize.GetBannerResize(), "Banner", null);
            }

            BannerExT bannerT = new BannerExT();
            bannerT.Type = bannerType;
            bannerT.Title = title;
            bannerT.PublishYn = publish;
            bannerT.OpenerYn = opener;
            bannerT.Link = link;
            bannerT.Image = fileName;
            bannerT.Priority = priority;
            bannerT.RegId = "admin";
            bannerT.RegDt = DateTime.Now;
            var result = bannerDac.AddBanner(bannerT);

            return Redirect("/banner?type=" + bannerType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public ActionResult Edit(int no)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            BannerExT bannerT = bannerDac.GetBannerByNo(no);
            ViewBag.BannerType = bannerT.Type;
            return View(bannerT);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bannerNo"></param>
        /// <param name="title"></param>
        /// <param name="publish"></param>
        /// <param name="opener"></param>
        /// <param name="link"></param>
        /// <param name="priority"></param>
        /// <param name="bannerType"></param>
        /// <param name="image"></param>
        /// <param name="up_image_del"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BannerUpdate(int bannerNo, string title, string publish, string opener, string link, int priority, int bannerType, HttpPostedFileBase image, string up_image_del)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            BannerExT bannerT = bannerDac.GetBannerByNo(bannerNo);
            if (bannerT != null)
            {
                bannerT.Type = bannerType;
                bannerT.Title = title;
                bannerT.PublishYn = publish;
                bannerT.OpenerYn = opener;
                bannerT.Link = link;

                var fileName = string.Empty;

                if (up_image_del == "y" || String.IsNullOrEmpty(up_image_del))
                {
                    //파일 삭제
                    if (up_image_del == "y")
                    {
                        ApplicationConfiguration instance = ApplicationConfiguration.Instance;
                        string backupPath = string.Format(@"{0}\{1}\{2}", instance.FileServerUncPath, instance.BannerBackup, bannerT.Image);
                        string fullsizePath = string.Format(@"{0}\{1}\{2}", instance.FileServerUncPath, instance.BannerFullImg, bannerT.Image);
                        string thumbPath = string.Format(@"{0}\{1}\{2}", instance.FileServerUncPath, instance.BannerThumbnail, bannerT.Image);
                        
                        new FileHelper().FileDelete(backupPath);
                        new FileHelper().FileDelete(fullsizePath);
                        new FileHelper().FileDelete(thumbPath);

                        bannerT.Image = fileName;
                    }
                    //이미지 변경
                    if (image != null)
                    {
                        fileName = new UploadFunc().FileUpload(image, ImageReSize.GetBannerResize(), "Banner", null);
                        bannerT.Image = fileName;
                    }
                }
                bannerT.Priority = priority;
                bannerT.UpdId = "admin";
                bannerT.UpdDt = DateTime.Now;

                bool ret = bannerDac.UpdateBanner(bannerT);
            }

            return Redirect("/banner?type=" + bannerType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bannerNo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BannerDelete(int bannerNo)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            bool ret = bannerDac.DeleteBanner(bannerNo);

            response.Success = ret;
            if (ret)
            {
                response.Message = "수정 되었습니다.";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}
