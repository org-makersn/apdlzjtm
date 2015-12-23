using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Makersn.BizDac;
using Makersn.Models;
using Makersn.Util;
using Design.Web.Admin.Models;

namespace Design.Web.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private MenuModel menuModel = new MenuModel();

        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_DashBoard";
            menuModel.MainIndex = 0;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }

        public ActionResult Index()
        {
            ViewData["Group"] = MenuModel(0);
            //IList<CodeT> list = new CodeDac().GetCodeLstByGbn("ARTICLE");
            int i = 18287644;
            byte[] ayData = BitConverter.GetBytes(i);
            string tobase = Convert.ToBase64String(ayData);
            ViewBag.EncodeT = tobase;
            ayData = Convert.FromBase64String(tobase);
            ViewBag.DecodeT = BitConverter.ToInt32(ayData, 0);
            return View();
        }

        [HttpPost]
        public JsonResult UploadImage(HttpPostedFileBase up_image)
        {
            var fileName = string.Empty;
            if (up_image != null)
            {
                fileName = FileUpload.UploadFile(up_image, new ImageSize().GetBannerResize(), "Home", null);
            }

            return Json(fileName, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendMail()
        {
            ViewData["Group"] = MenuModel(0);

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendMail(string Subject, string Body)
        {
            SendMailModels oMail = new SendMailModels();
            oMail.SendMail("Email", "chasy@makersi.com", new String[] { Subject, Body });

            return Content("Success");
        }
    }
}
