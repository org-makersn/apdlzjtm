using Library.ObjParser;
using Makersn.Models;
using Net.Common.Define;
using Net.Common.Filter;
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
            return View();
        }

        #region 모델링 업로드
        /// <summary>
        /// 모델링 업로드
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

        public ActionResult Details(long no)
        {
            string aaa = Md5Encrypt.MD5Hash(no.ToString());
            string bbb = Md5Encrypt.SHA256Hash(no.ToString());

            return View();
        }

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
