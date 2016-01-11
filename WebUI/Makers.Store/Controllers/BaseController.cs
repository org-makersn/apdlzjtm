using Net.Common.Model;
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
        public ProfileModel profileModel;

        public BaseController()
        {
            profileModel = Profile;
            ViewBag.LogOnMemner = profileModel;
            ViewBag.LogOnChk = profileModel.UserNo == 0 ? 0 : 1;

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
    }
}
