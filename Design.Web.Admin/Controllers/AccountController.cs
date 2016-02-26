using System.Web.Mvc;
using System.Web.Security;
using Makersn.BizDac;
using Makersn.Models;
using Design.Web.Admin.Models;
using Newtonsoft.Json;

namespace Design.Web.Admin.Controllers
{
    public class AccountController : BaseController
    {
        private MemberDac memberDac = new MemberDac();

        //[Authorize]
        [ActionName("Index")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// LogOn
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LogOn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new AccountModels.LogOnPageModel() { ReturlUrl = returnUrl });
        }

        /// <summary>
        /// form
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogOn(AccountModels.LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string ip =Design.Web.Admin.Helper.IPAddressHelper.GetIP(this);
                MemberT member = memberDac.GetMemberForAdminLogOn(model.UserId, model.Password, ip);
                if (member != null)
                {
                    FormsAuthentication.SetAuthCookie(model.UserId, false);
                    //Member = member;

                    ProfileModel profile = new ProfileModel();
                    profile.UserNo = member.No;
                    profile.UserNm = member.Name;
                    profile.UserId = member.Id;
                    profile.UserProfilePic = member.ProfilePic;
                    profile.UserLevel = member.Level;
                    //string hashAttr = string.Format("{0},{1},{2},{3}", member.No, member.Name, member.Id, member.ProfilePic);

                    var hashJson = JsonConvert.SerializeObject(profile);

                    FormsAuthentication.SetAuthCookie(hashJson, model.RememberMe);

                    return RedirectToLocal(returnUrl);
                }
            }
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "DashBoard");
        }

        #region 도우미
        /// <summary>
        /// 도우미
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "DashBoard");
            }
        }
        #endregion
    }
}
