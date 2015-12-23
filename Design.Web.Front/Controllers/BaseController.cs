using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Makersn.Models;
using Makersn.Util;
using Design.Web.Front.Models;
using Newtonsoft.Json;
using Makersn.BizDac;
using System.Web;

namespace Design.Web.Front.Controllers
{
    public class BaseController : Controller
    {
        //private static readonly MemberT anonymous = new MemberT();
        private ProfileModel profileModel;

        PrinterMemberDac _printerMemberDac = new PrinterMemberDac();
        NoticesDac _noticesDac = new NoticesDac();
        MessageDac _messageDac = new MessageDac();

        public BaseController()
        {
            ViewBag.LogOnMemner = Profile;
            ViewBag.LogOnChk = Profile.UserNo == 0 ? 0 : 1;
            ViewBag.SpotChk = _printerMemberDac.GetPrinterMemberByNo(Profile.UserNo);
            ViewBag.MenuList = GetArticleList();

            //클릭시 가져오는걸로
            ViewBag.NoticeCnt = _noticesDac.GetNoticesCntByMemberNo(Profile.UserNo);
            ViewBag.MessageCnt = _messageDac.GetNewMessageCount(Profile.UserNo);

            ViewBag.ProfileImgUrl = System.Configuration.ConfigurationManager.AppSettings["ProfileImgUrl"];
            ViewBag.ArticleImgUrl = System.Configuration.ConfigurationManager.AppSettings["ArticleImgUrl"];
            ViewBag.Article3DUrl = System.Configuration.ConfigurationManager.AppSettings["Article3DUrl"];
            ViewBag.AdminImgUrl = System.Configuration.ConfigurationManager.AppSettings["AdminImgUrl"];
            ViewBag.ArticleJsUrl = System.Configuration.ConfigurationManager.AppSettings["Article3DJsUrl"];
            ViewBag.PrintImgUrl = System.Configuration.ConfigurationManager.AppSettings["PrinterImgUrl"];
            ViewBag.CurrentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
            ViewBag.TargetDomain = System.Configuration.ConfigurationManager.AppSettings["TargetDomain"];
            ViewBag.LangFlag = System.Configuration.ConfigurationManager.AppSettings["LangFlag"];
            ViewBag.LangFlagName = ViewBag.LangFlag == "KR" ? "한국어" : ViewBag.LangFlag == "EN" ? "English" : "";

            ViewBag.IsMain = "N";

        }

        public ProfileModel Profile
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
