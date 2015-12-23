using System;
using System.Web.Mvc;
using System.Web.Security;
using Makersn.BizDac;
using Makersn.Models;
using Design.Web.Front.Models;
using Makersn.Util;
using Microsoft.Web.WebPages.OAuth;
using DotNetOpenAuth.AspNet;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Design.Web.Front.Helper;

namespace Design.Web.Front.Controllers
{
    public class AccountController : BaseController
    {
        private MemberDac _memberDac = new MemberDac();
        CryptFilter crypt = new CryptFilter();

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

        [HttpPost]
        public JsonResult LogOn(AccountModels.LogOnModel model, string returnUrl)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;
            if (ModelState.IsValid)
            {
                string ip = Design.Web.Front.Helper.IPAddressHelper.GetIP(this);
                MemberT member = _memberDac.GetMemberForMemberLogOn(model.UserId, model.Password, ip);
                if (member != null)
                {
                    if (member.emailCertify == "N")
                    {
                        response.Result = member.Email;
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                    ProfileModel profile = new ProfileModel();
                    profile.UserNo = member.No;
                    profile.UserNm = member.Name;
                    profile.UserId = member.Id;
                    profile.UserProfilePic = member.ProfilePic;
                    profile.UserLevel = member.Level;
                    //string hashAttr = string.Format("{0},{1},{2},{3}", member.No, member.Name, member.Id, member.ProfilePic);

                    var hashJson = JsonConvert.SerializeObject(profile);

                    FormsAuthentication.SetAuthCookie(hashJson, model.RememberMe);

                    //FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(hashJson, true, 500000);

                    response.Success = true;
                    response.Result = returnUrl;
                }
            }

            return Json(response, JsonRequestBehavior.AllowGet);
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

            return RedirectToAction("Index", "Main");
        }

        #region 회원가입
        /// <summary>
        /// form
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult JoinOn(string returnUrl, string JoinName, string JoinEmail, string JoinPassword)
        {
            bool result = false;

            if (ModelState.IsValid)
            {
                bool emailCheck = System.Text.RegularExpressions.Regex.IsMatch(JoinEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                if (!emailCheck) { return Json(new { Success = false, Result = 1 }); }
                if (JoinPassword.Length < 6) { return Json(new { Success = false, Result = 2 }); }

                MemberT member = new MemberT();
                member.Email = JoinEmail;
                member.Id = JoinEmail;
                member.Name = JoinName;
                member.Password = crypt.Encrypt(JoinPassword);
                member.Level = 10;
                member.ProfilePic = "";
                member.Status = "1";
                member.SnsType = "em";
                member.AllIs = "on";
                member.RepleIs = "on";
                member.LikeIs = "on";
                member.NoticeIs = "n";
                member.FollowIs = "on";
                member.LoginCnt = 0;
                member.DelFlag = "N";
                member.RegDt = DateTime.Now;
                member.RegId = JoinEmail;
                member.emailCertify = "N";


                if (_memberDac.AddMember(member))
                {
                    //원래 회원가입시 자동 로그인 기능으로 추가하였으나, 이메일 인증 추가로 인해 주석
                    //AccountModels.LogOnModel model = new AccountModels.LogOnModel();
                    //model.UserId = JoinEmail;
                    //model.Password = JoinPassword;
                    //LogOn(model, returnUrl);
                    result = true;
                    sendEmailCertify(member.No, member.Name, member.Email);
                }
                else
                {
                    return Json(new { Success = false, Result = 1 });
                }
            }
            return Json(new { Success = result });
        }
        #endregion

        #region 이메일 인증 발송
        public void sendEmailCertify(int memberNo, string name, string email)
        {
            string no = Base64Helper.Base64Encode(memberNo.ToString());
            string Subject = "makersN 회원가입 이메일 인증";
            string Body = "http://www.makersn.com/account/EmailCertify?chk=" + no;

            SendMailModels oMail = new SendMailModels();
            oMail.SendMail("mailCertify", email, new String[] { Subject, name, Body });
        }
        #endregion

        #region 이메일 인증
        public ActionResult EmailCertify(string chk)
        {
            int memberNo = int.Parse(Base64Helper.Base64Decode(chk));

            bool result = _memberDac.UpdateMemberEmailCertify(memberNo);
            if (result)
            {
                //return RedirectToAction("Index", "Main");
                return Content("<script>alert('이메일 인증에 성공하였습니다.'); location.href='/'</script>");
            }
            else
            {
                return Content("<script>alert('잘못된 접근입니다')</script>");
            }
        }
        #endregion

        #region 이메일 변경 인증
        public ActionResult ChangeEmailCertify(string chk)
        {
            int memberNo = int.Parse(Base64Helper.Base64Decode(chk));
            bool result = _memberDac.UpdateEmailandId(memberNo);
            if (result)
            {
                //return RedirectToAction("Index", "Main");
                return Content("<script>alert('이메일 변경에 성공하였습니다.'); location.href='/'</script>");
            }
            else
            {
                return Content("<script>alert('잘못된 접근입니다')</script>");
            }
        }
        #endregion

        #region 이메일 변경 취소
        public ActionResult CancleEmailCertify(string chk)
        {
            int memberNo = int.Parse(Base64Helper.Base64Decode(chk));
            bool result = _memberDac.UpdateEmailCancel(memberNo);
            if (result)
            {
                //return RedirectToAction("Index", "Main");
                return Content("<script>alert('이메일 변경을 취소 하였습니다.'); location.href='/'</script>");
            }
            else
            {
                return Content("<script>alert('잘못된 접근입니다')</script>");
            }
        }
        #endregion

        #region 임시 비밀번호 발급
        [HttpPost]
        public JsonResult SendMail(string email)
        {
            string Subject = "makersn의 임시비밀번호가 발급되었습니다.";
            string Body = TemporaryPassword();

            if (!_memberDac.UpdateTemporaryPassword(email, Body))
            {
                return Json(new { Success = false });
            }
            SendMailModels oMail = new SendMailModels();
            oMail.SendMail("Email", email, new String[] { Subject, Body });

            return Json(new { Success = true });
        }

        public string TemporaryPassword()
        {
            string randomChars = "abcdefghijklmnopqrstuvwxyz0123456789";
            string password = string.Empty;
            int randomNum;
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                randomNum = random.Next(randomChars.Length);
                password += randomChars[randomNum];
            }
            return password;
        }
        #endregion

        #region sns login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }


        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            MemberT memberT = _memberDac.IsMemberExistById(result.UserName, result.ProviderUserId);
            ProfileModel profile = new ProfileModel();

            if (memberT != null)
            {
                //if (memberT.SnsType == "fb")
                //{
                    profile.UserNo = memberT.No;
                    profile.UserNm = memberT.Name;
                    profile.UserId = memberT.Id;
                    profile.UserProfilePic = memberT.ProfilePic;
                    profile.UserLevel = memberT.Level;

                    var hashJson = JsonConvert.SerializeObject(profile);

                    FormsAuthentication.SetAuthCookie(hashJson, false);
                //}
                //else
                //{
                //    //페이스북으로 저장할것인가?
                //}

                return RedirectToLocal(returnUrl);
            }
            else
            {
                //create
                string saveImgFolder = "Profile/thumb";
                string fileImgpath = string.Format("{0}/FileUpload/{1}/", AppDomain.CurrentDomain.BaseDirectory, saveImgFolder);

                if (!Directory.Exists(fileImgpath))
                {
                    Directory.CreateDirectory(fileImgpath);
                }

                byte[] data;
                string fileNm = Guid.NewGuid().ToString() + ".jpg";
                using (WebClient client = new WebClient())
                {
                    data = client.DownloadData(string.Format("https://graph.facebook.com/{0}/picture?type=large", result.ProviderUserId));
                    System.IO.File.WriteAllBytes(fileImgpath + fileNm, data);
                }

                memberT = new MemberT();
                memberT.Id = result.UserName;
                memberT.Name = result.ExtraData["name"];
                memberT.SnsId = result.ProviderUserId;
                memberT.ProfilePic = fileNm;
                memberT.Email = result.UserName;
                memberT.Level = 10;
                memberT.Status = "1";
                memberT.SnsType = "fb";
                memberT.AllIs = "on";
                memberT.RepleIs = "on";
                memberT.LikeIs = "on";
                memberT.NoticeIs = "n";
                memberT.FollowIs = "on";
                memberT.LoginCnt = 0;
                memberT.DelFlag = "N";
                memberT.RegDt = DateTime.Now;
                memberT.RegId = result.UserName;
                memberT.emailCertify = "Y";

                if (_memberDac.AddMember(memberT))
                {
                    profile.UserNo = memberT.No;
                    profile.UserNm = memberT.Name;
                    profile.UserId = memberT.Id;
                    profile.UserProfilePic = memberT.ProfilePic;
                    profile.UserLevel = memberT.Level;

                    var hashJson = JsonConvert.SerializeObject(profile);

                    FormsAuthentication.SetAuthCookie(hashJson, false);
                }

                return RedirectToLocal(returnUrl);
            }
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public PartialViewResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView(OAuthWebSecurity.RegisteredClientData);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public PartialViewResult ExternalJoins(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView(OAuthWebSecurity.RegisteredClientData);
        }
        #endregion

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
                return RedirectToAction("Index", "Main");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "사용자 이름이 이미 있습니다. 다른 사용자 이름을 입력하십시오.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "해당 전자 메일 주소를 가진 사용자 이름이 이미 있습니다. 다른 전자 메일 주소를 입력하십시오.";

                case MembershipCreateStatus.InvalidPassword:
                    return "제공한 암호가 잘못되었습니다. 올바른 암호 값을 입력하십시오.";

                case MembershipCreateStatus.InvalidEmail:
                    return "제공한 전자 메일 주소가 잘못되었습니다. 값을 확인한 후 다시 시도하십시오.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "제공한 암호 찾기 대답이 잘못되었습니다. 값을 확인한 후 다시 시도하십시오.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "제공한 암호 찾기 질문이 잘못되었습니다. 값을 확인한 후 다시 시도하십시오.";

                case MembershipCreateStatus.InvalidUserName:
                    return "제공한 사용자 이름이 잘못되었습니다. 값을 확인한 후 다시 시도하십시오.";

                case MembershipCreateStatus.ProviderError:
                    return "인증 공급자가 오류를 반환했습니다. 입력한 내용을 확인하고 다시 시도하십시오. 문제가 계속되면 시스템 관리자에게 문의하십시오.";

                case MembershipCreateStatus.UserRejected:
                    return "사용자 생성 요청이 취소되었습니다. 입력한 내용을 확인하고 다시 시도하십시오. 문제가 계속되면 시스템 관리자에게 문의하십시오.";

                default:
                    return "알 수 없는 오류가 발생했습니다. 입력한 내용을 확인하고 다시 시도하십시오. 문제가 계속되면 시스템 관리자에게 문의하십시오.";
            }
        }
        #endregion

        
    }
}
