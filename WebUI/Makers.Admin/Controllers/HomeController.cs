using Makers.Admin.Models;
using Makersn.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Makers.Admin.Controllers
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

        ///// <summary>
        ///// ExportExcel - xls
        ///// </summary>
        ///// <returns></returns>
        //public FileResult ExportExcel()
        //{
        //    var sbHtml = new StringBuilder();
        //    sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
        //    sbHtml.Append("<tr>");
        //    var lstTitle = new List<string> { "컬럼1", "컬럼1", "컬럼3", "컬럼4" };
        //    foreach (var item in lstTitle)
        //    {
        //        sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", item);
        //    }
        //    sbHtml.Append("</tr>");

        //    for (int i = 0; i < 1000; i++)
        //    {
        //        sbHtml.Append("<tr>");
        //        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", i);
        //        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", i);
        //        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", new Random().Next(20, 30) + i);
        //        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", DateTime.Now);
        //        sbHtml.Append("</tr>");
        //    }
        //    sbHtml.Append("</table>");

        //    ////방법 1
        //    //byte[] fileContents = Encoding.Default.GetBytes(sbHtml.ToString());
        //    //return File(fileContents, "application/ms-excel", "fileContents.xls");

        //    //application/vnd.ms-excel - For BIFF .xls files
        //    //application/vnd.openxmlformats-officedocument.spreadsheetml.sheet - For Excel2007 and above .xlsx files
        //    //방법 2
        //    byte[] fileContents = Encoding.Default.GetBytes(sbHtml.ToString());
        //    var fileStream = new MemoryStream(fileContents);
        //    return File(fileStream, "application/ms-excel", "fileStream.xls");
        //}
    }
}
