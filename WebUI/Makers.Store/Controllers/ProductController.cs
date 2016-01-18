using Library.ObjParser;
using Makersn.Models;
using Net.Common.Define;
using Net.Common.Helper;
using Net.Common.Model;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using Newtonsoft.Json;
using QuantumConcepts.Formats.StereoLithography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Makers.Store.Controllers
{
    public class ProductController : BaseController
    {
        StoreLikesBiz likesBiz = new StoreLikesBiz();
        StoreMemberBiz memberBiz = new StoreMemberBiz();
        
        public ActionResult Index()
        {
            ViewBag.UserNo = profileModel.UserNo;

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public ActionResult Create(string ex)
        {
            //IList<StoreProductT> testLst = new StoreProductDac().SelectProductTest();
            new StoreProductDac().SelectProductTest();

            return View();
        }

        #region stl upload
        /// <summary>
        /// stl upload
        /// </summary>
        /// <param name="stlupload"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StlUpload(FormCollection collection, IEnumerable<HttpPostedFileBase> product_3d_files)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;

            int uploadCnt = product_3d_files.Count();
            int[] Result = new int[uploadCnt];
            int index = 0;
            long productNo = 0;

            string temp = collection["temp"];

            foreach (HttpPostedFileBase stlupload in product_3d_files)
            {
                if (stlupload != null)
                {
                    if (stlupload.ContentLength > 0)
                    {
                        if (stlupload.ContentLength < 200 * 1024 * 1024)
                        {
                            string[] extType = { ".stl", ".obj" };

                            string extension = Path.GetExtension(stlupload.FileName);

                            if (extType.Contains(extension.ToLower()))
                            {
                                string modelingDir = Constant.StoreUploadDir.ModelingDir;
                                string save3DPath = string.Format(@"{0}\{1}", instance.PhysicalDir, modelingDir);
                                string fileReName = new FileHelper().UploadFile(stlupload, null, save3DPath, null);

                                StoreProductT _storeProduct = new StoreProductT();

                                _storeProduct.VAR_NO = new DateTimeHelper().ConvertToUnixTime(DateTime.Now).ToString();
                                _storeProduct.NAME = stlupload.FileName.Replace(extension, string.Empty).Replace("_", " ");
                                _storeProduct.FILE_NAME = stlupload.FileName;
                                _storeProduct.FILE_RENAME = fileReName;
                                _storeProduct.FILE_EXT = extension.ToLower();
                                _storeProduct.MIME_TYPE = stlupload.ContentType;
                                _storeProduct.FILE_SIZE = Convert.ToDouble(stlupload.ContentLength.ToString());

                                _storeProduct.MATERIAL_VOLUME = 0;
                                _storeProduct.OBJECT_VOLUME = 0;
                                _storeProduct.SIZE_X = 0;
                                _storeProduct.SIZE_Y = 0;
                                _storeProduct.SIZE_Z = 0;

                                _storeProduct.SCALE = 100;
                                _storeProduct.SHORT_LINK = "";
                                _storeProduct.VIDEO_URL = "";
                                //_storeProduct.VideoType = "";
                                _storeProduct.CATEGORY_NO = 0;
                                _storeProduct.CONTENTS = "";
                                _storeProduct.DESCRIPTION = "";
                                _storeProduct.PART_CNT = 1;
                                _storeProduct.CUSTERMIZE_YN = "Y";
                                _storeProduct.SELL_YN = "Y";
                                _storeProduct.TAG_NAME = "";
                                _storeProduct.CERTIFICATE_YN = 1;
                                _storeProduct.VISIBILITY_YN = "Y";
                                _storeProduct.USE_YN = "Y";
                                _storeProduct.MEMBER_NO = 0;
                                _storeProduct.TXT_SIZE_X = 0;
                                _storeProduct.TXT_SIZE_Y = 0;
                                _storeProduct.DETAIL_TYPE = 0;
                                _storeProduct.DETAIL_DEPTH = 0;
                                _storeProduct.TXT_LOC = "";
                                _storeProduct.REG_DT = DateTime.Now;
                                _storeProduct.REG_ID = profileModel.UserId;

                                productNo = new StoreProductBiz().addStoreProduct(_storeProduct);

                                if (productNo > 0)
                                {
                                    response.Success = true;
                                    response.Result = productNo.ToString();
                                }

                                //Result[index] = articleFileNo;
                                index++;
                            }
                            else
                            {
                                response.Message = Constant.Product.STL_FileUpload_Validate_Ext_Msg;
                            }
                        }
                        else
                        {
                            response.Message = Constant.Product.STL_FileUpload_Max_Size__Msg;
                        }
                    }
                }
            }

            return Json(new { Success = response.Success, Message = response.Message, Result = response.Result, Count = uploadCnt, ProductNo = productNo }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 제품 관련
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public ActionResult Models(int no)
        {
            StoreProductT storeProduct = new StoreProductBiz().getStoreProductById(no);
            ViewBag.AttrYN = storeProduct.MATERIAL_VOLUME == 0 || storeProduct.OBJECT_VOLUME == 0 ? "N" : "Y";
            ViewBag.MaterialList = new StoreMaterialBiz().getAllStoreMaterial();

            return View(storeProduct);
        }

        /// <summary>
        /// modify
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Models(int productNo, string productName, int categoryNo, string content, string description, string tagName, string videoUrl)
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

                int ret = new StoreProductBiz().setStoreProduct(product);
                if (ret > 0)
                {
                    response.Success = true;
                }
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
                    int ret = new StoreProductBiz().setStoreProduct(product);
                    if (ret > 0)
                    {
                        response.Success = true;
                        response.Result = JsonConvert.SerializeObject(getSize);
                    }
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 좋아요 관련

        /// <summary>
        /// 좋아요 CREATE / DELETE (상품번호)
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public JsonResult SetLikes(int productNo = 2)
        {
            int result = 0;

            StoreLikesT like = new StoreLikesT();
            like.MEMBER_NO = profileModel.UserNo;
            like.REG_IP = IPAddressHelper.GetIP(this);
            like.PRODUCT_NO = productNo;
            like.REG_ID = "test";
            like.REG_DT = DateTime.Now;

            result = likesBiz.set(like);

            return Json(new { Success = true, Result = result }, JsonRequestBehavior.AllowGet);
        }

        //
        // 상품별 좋아요 갯수 출력 (상품번호)
        public JsonResult CountLikesByProductNo(int productNo = 1)
        {
            int result = likesBiz.countLikesByProductNo(productNo);

            return Json(new { Success = true, Result = result }, JsonRequestBehavior.AllowGet);
        }

        //
        // 내가 좋아요 체크한 상품 리스트 (회원번호)
        public ActionResult GetLikedProductsByMemberNo(int memberNo = 202)
        {
            var result = likesBiz.getLikedProductsByMemberNo(memberNo);
            return PartialView(result);
            //return Json(new { Success = true, Result = result }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public ActionResult GetReceivedNoteListByMemberNo(int memberNo)
        {
            var result = memberBiz.getReceivedNoteListByMemberNo(memberNo);
            return PartialView(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public ActionResult GetSentNoteListByMemberNo(int memberNo)
        {
            var result = memberBiz.getSentNoteListByMemberNo(memberNo);
            return PartialView(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromMember"></param>
        /// <param name="targetMember"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public JsonResult SendNote(int fromMember, int targetMember, string comment)
        {
            MemberMsgT msg = new MemberMsgT();

            msg.MemberNo = fromMember;
            msg.MemberNoRef = targetMember;
            msg.Comment = comment;
            msg.MsgGubun = "NOTE";
            msg.RegDt = DateTime.Now;
            msg.RegId = "test";
            msg.RegIp = IPAddressHelper.GetIP(this);
            msg.IsNew = "Y";
            msg.SiteGubun = "Store";
            msg.DelFlag = "N";

            int result = memberBiz.sendNote(msg);
            return Json(new { Success = true, Result = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SeqNo"></param>
        /// <returns></returns>
        public JsonResult DeleteNote(int SeqNo)
        {
            int result = memberBiz.deleteNote(SeqNo);
            return Json(new { Success = true, Result = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 가격 선정
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult ProductPricing()
        {
            return View();
        }

        public ActionResult ProductPrintable()
        {
            return View();
        }
    }
}
