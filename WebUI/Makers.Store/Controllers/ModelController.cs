using Net.Common.Define;
using Net.Common.Helper;
using Net.Common.Model;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makers.Store.Controllers
{
    [Authorize]
    public class ModelController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public ActionResult Index(int no)
        {
            StoreProductT storeProduct = new StoreProductBiz().getStoreProductById(no);
            ViewBag.AttrYN = storeProduct.MATERIAL_VOLUME == 0 || storeProduct.OBJECT_VOLUME == 0 ? "N" : "Y";

            ViewBag.MaterialList = new StoreMaterialBiz().GetAllStoreMaterial();

            return View(storeProduct);
        }


        /// <summary>
        /// modify
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add(int productNo, string productName, int categoryNo, string content, string description, string tagName, string videoUrl)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;

            StoreProductT product = new StoreProductBiz().getStoreProductById(productNo);
            if (product != null)
            {
                product.NAME = productName;
                product.CATEGORY_NO = categoryNo;
                product.CONTENTS = content;
                product.DESCRIPTION = description;
                product.TAG_NAME = tagName;
                product.VIDEO_URL = videoUrl;
                //product.VideoType = "";

                bool ret = new StoreProductBiz().setStoreProduct(product);
                response.Success = ret;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 사이즈, 부피, 불륨
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetModelingAttr(int productNo)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;

            int status = 0;

            string saveJSFolder = Constant.StoreUploadDir.JsonToJsDir;

            StoreProductT product = new StoreProductBiz().getStoreProductById(productNo);

            if (product != null)
            {
                ModelingSize getSize = new ModelingSize();
                string fullpath = string.Format(@"{0}\{1}\{2}", instance.PhysicalDir, Constant.StoreUploadDir.ModelingDir, product.FILE_RENAME);
                if (product.OBJECT_VOLUME == 0)
                {
                    status += 1;
                    _3DModel _3dModel = new Modeling3DHelper().Get3DModel(fullpath, product.FILE_EXT);

                    getSize = new Modeling3DHelper().GetSizeFor3DFile(fullpath, product.FILE_EXT);

                    product.SIZE_X = getSize.X;
                    product.SIZE_Y = getSize.Y;
                    product.SIZE_Z = getSize.Z;
                    product.OBJECT_VOLUME = getSize.ObjectVolume;

                    string jsfullpath = string.Format(@"{0}\{1}\{2}.js", instance.PhysicalDir, saveJSFolder, product.FILE_RENAME);

                    var strBuff = JsonConvert.SerializeObject(_3dModel);

                    bool result = new FileHelper().FileWriteAllText(jsfullpath, strBuff);
                }
                else
                {
                    getSize.X = product.SIZE_X.Value;
                    getSize.Y = product.SIZE_Y.Value;
                    getSize.Z = product.SIZE_Z.Value;
                    getSize.ObjectVolume = product.OBJECT_VOLUME.Value;
                }

                if (product.MATERIAL_VOLUME == 0)
                {
                    status += 1;
                    product.MATERIAL_VOLUME = new Modeling3DHelper().Slicing(fullpath, instance.Slic3rDir);
                    getSize.MaterialVolume = product.MATERIAL_VOLUME.Value;
                }
                else
                {
                    getSize.MaterialVolume = product.MATERIAL_VOLUME.Value;
                }

                if (status > 0)
                {
                    bool ret = new StoreProductBiz().setStoreProduct(product);
                    if (ret)
                    {
                        response.Success = true;
                        response.Result = JsonConvert.SerializeObject(getSize);
                    }
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}
