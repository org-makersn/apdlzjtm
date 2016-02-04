using Makers.Admin.Models;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makers.Admin.Controllers
{
    [Authorize]
    public class SettingsController : BaseController
    {
        private MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Settings";
            menuModel.MainIndex = 8;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["Group"] = MenuModel(0);

            return View();
        }

        /// <summary>
        /// 재질
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Materials(int no = 0, string mode = "list", int page = 1)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);
            if (mode.Contains("add"))
            {
                return View("MaterialAdd");
            }
            else if (mode.Contains("edit"))
            {
                StoreMaterialT model = new StoreMaterialDac().GetStoreMaterialById(no);

                return View("MaterialEdit", model);
            }
            else
            {
                IList<StoreMaterialT> models = new StoreMaterialDac().GetAllStoreMaterial();

                ViewData["cnt"] = models.Count;

                return View(models.OrderByDescending(p => p.REG_DT).ToPagedList(page, 20));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="name"></param>
        /// <param name="imagename"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PostMaterials(FormCollection collection, HttpPostedFileBase IMAGE_NAME)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            string mode = collection["mode"];

            StoreMaterialT material = new StoreMaterialT();
            material.NAME = collection["NAME"];
            material.IMAGE_NAME = "none";
            material.PRICE = string.IsNullOrEmpty(collection["PRICE"]) ? 0 : Convert.ToInt32(collection["PRICE"]);
            material.SLICE_YN = collection["SLICE_YN"];
            material.SORT = string.IsNullOrEmpty(collection["SORT"]) ? 15 : Convert.ToInt32(collection["SORT"]);
            //HttpPostedFileBase image = Request.Files["IMAGE_NAME"];

            if (mode.Contains("add"))
            {
                material.NO = 0;
                material.REG_DT = DateTime.Now;
                material.REG_ID = Profile.UserNm;
                response.Success = new StoreMaterialDac().AddStoreMaterial(material);
            }

            if (mode.Contains("edit"))
            {
                int no = Convert.ToInt32(collection["NO"]);

                StoreMaterialT origin = new StoreMaterialDac().GetStoreMaterialById(no);
                origin.NAME = material.NAME;
                origin.IMAGE_NAME = material.IMAGE_NAME;
                origin.PRICE = material.PRICE;
                origin.SLICE_YN = material.SLICE_YN;
                origin.SORT = material.SORT;
                origin.UPD_DT = DateTime.Now;
                origin.UPD_ID = Profile.UserNm;

                response.Success = new StoreMaterialDac().UpdateStoreMaterial(origin);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 코드
        /// </summary>
        /// <returns></returns>
        public ActionResult Code()
        {
            ViewData["Group"] = MenuModel(2);
            return View();
        }
    }
}
