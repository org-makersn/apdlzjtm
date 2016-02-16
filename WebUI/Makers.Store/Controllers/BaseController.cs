using Makers.Store.Configurations;
using Net.Common.Configurations;
using Net.Common.Model;
using Net.Common.Util;
using Net.Framwork.BizDac;
using Net.Framework.StoreModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makers.Store.Controllers
{
    public class BaseController : Controller
    {
        public static ApplicationConfiguration.StoreConfiguration instance = ApplicationConfiguration.StoreConfiguration.Instance;
        public ProfileModel profileModel;
        //public CommonValModel commonValModel = new CommonValModel();

        public BaseController()
        {
            //ViewBag.CommonValJoson = JsonConvert.SerializeObject(commonValModel);
            profileModel = Profile;
            ViewBag.LogOnMemner = profileModel;
            ViewBag.LogOnChk = profileModel.UserNo == 0 ? 0 : 1;

            ViewBag.HasStore = profileModel.UserNo > 0 ? new StoreMemberBiz().GetStoreMemberExists(profileModel.UserNo) : false;

            ViewBag.IsMain = "N";
        }

        private ProfileModel Profile
        {
            get
            {
                var user = System.Web.HttpContext.Current.User;

                profileModel = new ProfileModel();

                if (user.Identity.IsAuthenticated)
                {
                    profileModel = JsonConvert.DeserializeObject<ProfileModel>(user.Identity.Name);
                }
                else
                {
                    profileModel.UserNo = 0;
                    profileModel.UserNm = "손님";
                    profileModel.UserId = "anonymous";
                    profileModel.UserProfilePic = "";
                    profileModel.UserLevel = 0;
                }

                return profileModel;
            }
        }

        /// <summary>
        /// 파샬뷰로 빼던지
        /// </summary>
        /// <returns></returns>
        //protected IList<CodeModel> GetArticleList()
        //{
        //    IList<CommonCodeT> list = null;
        //    list = CacheUtil.GetCache("MenuList") as IList<CommonCodeT>;

        //    if (list == null)
        //    {
        //        list = new CommonCodeDac().GetCommonCode("STORE", "PRODUCT");
        //        CacheUtil.SetCache("MenuList", list);
        //    }
        //    IList<CodeModel> menulist = new List<CodeModel>();
        //    foreach (var menu in list)
        //    {
        //        CodeModel model = new CodeModel();
        //        model.MenuTitle = menu.CODE_NAME;
        //        model.MenuCodeNo = menu.NO;
        //        if (menu.NO > 0)
        //        {
        //            model.MenuUrl = "/catalog/latest-" + menu.CODE_KEY;
        //        }
        //        else
        //        {
        //            model.MenuUrl = "/catalog/latest";
        //        }
        //        menulist.Add(model);
        //    }
        //    return menulist;
        //}
    }
}
