using System.Web.Mvc;
using Makersn.Models;
using log4net;
using Design.Web.Admin.Models;
using Newtonsoft.Json;

namespace Design.Web.Admin.Controllers
{
    public class BaseController : Controller
    {
        public ILog Log { get; set; }
        private static readonly string Key_Login_Member = "Admin_Login_Member";
        private static readonly MemberT anonymous = new MemberT();
        public static string UserNm;
        private ProfileModel profileModel;

        public BaseController()
        {
            //var user = System.Web.HttpContext.Current.User;
            //UserNm = user.Identity.Name;
            UserNm = Profile.UserNm;
            ViewBag.UserNm = UserNm;

            ViewBag.ProfileImgUrl = System.Configuration.ConfigurationManager.AppSettings["ProfileImgUrl"];
            ViewBag.ArticleImgUrl = System.Configuration.ConfigurationManager.AppSettings["ArticleImgUrl"];
            ViewBag.AdminImgUrl = System.Configuration.ConfigurationManager.AppSettings["AdminImgUrl"];
            ViewBag.BannerUrl = System.Configuration.ConfigurationManager.AppSettings["BannerUrl"];
            ViewBag.PrintImgUrl = System.Configuration.ConfigurationManager.AppSettings["PrinterImgUrl"];
            ViewBag.MainUrl = System.Configuration.ConfigurationManager.AppSettings["MainUrl"];
            ViewBag.CurrentDomain = System.Configuration.ConfigurationManager.AppSettings["CurrentDomain"];
            ViewBag.TargetDomain = System.Configuration.ConfigurationManager.AppSettings["TargetDomain"];

            if (Profile.UserLevel < 50)
            {
                System.Web.Security.FormsAuthentication.SignOut();
            }
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
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //todo
            base.OnActionExecuting(filterContext);
        }

        #region 사용안함
        /// <summary>
        /// 
        /// </summary>
        public MemberT Member
        {
            get
            {
                MemberT memberT = (MemberT)System.Web.HttpContext.Current.Session[Key_Login_Member] != null ? (MemberT)System.Web.HttpContext.Current.Session[Key_Login_Member] : anonymous;

                return memberT;
            }
            set
            {
                if (value == null)
                {
                    System.Web.HttpContext.Current.Session.Remove(Key_Login_Member);
                }
                else
                {
                    System.Web.HttpContext.Current.Session[Key_Login_Member] = value;
                }
            }
        }
        #endregion
    }
}
