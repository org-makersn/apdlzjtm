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
        private Extent Size { get; set; }// Stl Model Size
        //
        // GET: /Product/
        public ActionResult Index()
        {
            ViewBag.UserNo = profileModel.UserNo;
            return View();
        }

        public ActionResult Create(string ex)
        {
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
                                string save3DFolder = string.Format(@"{0}\product\3d-files", instance.PhysicalDir);

                                string fileReName = new FileHelper().UploadFile(stlupload, null, save3DFolder, null);

                                StoreProductT _storeProduct = new StoreProductT();

                                _storeProduct.VarNo = 10000;
                                _storeProduct.ProductName = stlupload.FileName.Replace(extension, string.Empty).Replace("_", " ");
                                _storeProduct.FilePath = save3DFolder;
                                _storeProduct.FileName = stlupload.FileName;
                                _storeProduct.FileReName = fileReName;
                                _storeProduct.FileExt = extension.ToLower();
                                _storeProduct.MimeType = stlupload.ContentType;
                                _storeProduct.FileSize = Convert.ToDouble(stlupload.ContentLength.ToString());

                                _storeProduct.SlicedVolume = 0;
                                _storeProduct.ModelVolume = 0;
                                _storeProduct.SizeX = 0;
                                _storeProduct.SizeY = 0;
                                _storeProduct.SizeZ = 0
                                    ;
                                _storeProduct.Scale = 100;
                                _storeProduct.ShortLing = "";
                                _storeProduct.VideoUrl = "";
                                //_storeProduct.VideoType = "";
                                _storeProduct.CategoryNo = 0;
                                _storeProduct.Contents = "";
                                _storeProduct.Description = "";
                                _storeProduct.PartCnt = 1;
                                _storeProduct.CustormizeYn = "Y";
                                _storeProduct.SellYn = "Y";
                                _storeProduct.TagName = "";
                                _storeProduct.CertiFicateStatus = 1;
                                _storeProduct.VisibilityYn = "Y";
                                _storeProduct.UseYn = "Y";
                                _storeProduct.MemberNo = 0;
                                _storeProduct.TxtSizeX = 0;
                                _storeProduct.TxtSizeY = 0;
                                _storeProduct.DetailType = 0;
                                _storeProduct.DetailDepth = 0;
                                _storeProduct.TxtLoc = "";
                                _storeProduct.RegDt = DateTime.Now;
                                _storeProduct.RegId = profileModel.UserId;

                                IList<StoreProductT> list = new StoreProductBiz().getAllStoreProduct();
                                productNo = new StoreProductBiz().addStoreProduct(_storeProduct);

                                response.Success = true;
                                response.Result = productNo.ToString();

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
            ViewBag.AttrYN = storeProduct.SlicedVolume == 0 ? "N" : "Y";
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
                product.ProductName = productName;
                product.CategoryNo = categoryNo;
                product.Contents = content;
                product.Description = description;
                product.TagName = tagName;
                product.VideoUrl = videoUrl;
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

            string saveJSFolder = @"product\js-files";

            StoreProductT product = new StoreProductBiz().getStoreProductById(productNo);

            if (product != null)
            {
                ModelingSize getSize = null;
                string fullpath = string.Format(@"{0}\{1}\{2}", instance.PhysicalDir, product.FilePath, product.FileReName);
                if (product.ModelVolume == 0)
                {
                    _3DModel _3dModel = new Modeling3DHelper().GetStlModel(fullpath, product.FileExt);

                    getSize = new Modeling3DHelper().GetSizeFor3DFile(fullpath, product.FileExt);

                    product.SizeX = getSize.X;
                    product.SizeY = getSize.Y;
                    product.SizeZ = getSize.Z;
                    product.ModelVolume = getSize.Volume / 1000;

                    string jsfullpath = string.Format(@"{0}\{1}\{2}.js", instance.PhysicalDir, saveJSFolder, product.FileReName);

                    var strBuff = JsonConvert.SerializeObject(_3dModel);

                    bool result = new FileHelper().FileWriteAllText(jsfullpath, strBuff);
                }

                if (product.SlicedVolume == 0)
                {
                    product.SlicedVolume = new Modeling3DHelper().Slicing(fullpath, instance.Slic3rDir);
                    getSize.Slicing = product.SlicedVolume;
                }

                int ret = new StoreProductBiz().setStoreProduct(product);
                if (ret > 0)
                {
                    response.Success = true;
                    response.Result = JsonConvert.SerializeObject(getSize);
                }

            }
            return Json(response);
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
            like.MemberNo = profileModel.UserNo;
            like.RegIp = IPAddressHelper.GetIP(this);
            like.ProductNo = productNo;
            like.RegId = "test";
            like.RegDt = DateTime.Now;

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

        public ActionResult GetSentNoteListByMemberNo(int memberNo)
        {
            var result = memberBiz.getSentNoteListByMemberNo(memberNo);
            return PartialView(result);
        }

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
