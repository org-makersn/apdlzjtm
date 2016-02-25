using Makers.Bus.Configurations;
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
using log4net;
using log4net.Config;

namespace Makers.Bus.Controllers
{
    public class BaseController : Controller
    {
        public static ApplicationConfiguration.BusConfiguration instance = ApplicationConfiguration.BusConfiguration.Instance;
        protected static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ProfileModel profileModel;
        public int page_size { get; set; }

        public BaseController()
        {
            ViewData["AdminImgUrl"] = instance.AdminImgUrl;
            ViewData["IsMain"] = "N";
            ViewData["Menu"] = string.Empty;

            page_size = int.Parse(instance.BlogPageSize);
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