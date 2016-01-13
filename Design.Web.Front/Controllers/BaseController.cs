using Design.Web.Front.Configurations;
using Design.Web.Front.Models;
using Makersn.BizDac;
using Makersn.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Design.Web.Front.Controllers
{
    public class BaseController : Controller
    {
        public ApplicationConfiguration instance = ApplicationConfiguration.Instance;

        public ProfileModel profileModel;

        PrinterMemberDac _printerMemberDac = new PrinterMemberDac();
        NoticesDac _noticesDac = new NoticesDac();
        MessageDac _messageDac = new MessageDac();

        public BaseController()
        {
            profileModel = Profile;

            ViewBag.LogOnMemner = profileModel;
            ViewBag.LogOnChk = profileModel.UserNo == 0 ? 0 : 1;
            ViewBag.SpotChk = _printerMemberDac.GetPrinterMemberByNo(profileModel.UserNo);
            ViewBag.MenuList = GetArticleList();

            //클릭시 가져오는걸로
            ViewBag.NoticeCnt = _noticesDac.GetNoticesCntByMemberNo(profileModel.UserNo);
            ViewBag.MessageCnt = _messageDac.GetNewMessageCount(profileModel.UserNo);

            ViewBag.ProfileImgUrl = instance.ProfileImgUrl;
            ViewBag.ArticleImgUrl = instance.ArticleImgUrl;
            ViewBag.Article3DUrl = instance.Article3DUrl;
            ViewBag.ArticleJsUrl = instance.Article3DJsUrl;
            ViewBag.AdminImgUrl = instance.AdminImgUrl;
            ViewBag.PrintImgUrl = instance.PrinterImgUrl;

            ViewBag.CurrentDomain = instance.CurrentDomain;
            ViewBag.TargetDomain = instance.TargetDomain;
            ViewBag.LangFlag = System.Configuration.ConfigurationManager.AppSettings["LangFlag"];
            ViewBag.LangFlagName = ViewBag.LangFlag == "KR" ? "한국어" : ViewBag.LangFlag == "EN" ? "English" : "";

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
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    //todo
        //    base.OnActionExecuting(filterContext);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IList<CodeModel> GetArticleList()
        {
            var menuList = EnumHelper.GetEnumDictionary<MakersnEnumTypes.CateName>();
            IList<CodeModel> list = new List<CodeModel>();
            foreach (var menu in menuList)
            {
                CodeModel model = new CodeModel();
                model.MenuTitle = menu.Value;
                model.MenuCodeNo = menu.Key;
                if (menu.Key > 0)
                {
                    model.MenuUrl = "/cate/" + Enum.GetName(typeof(MakersnEnumTypes.CateNameToUrl), menu.Key);
                }
                else
                {
                    model.MenuUrl = "/cate";
                }
                list.Add(model);
            }
            return list;
        }
    }
}
