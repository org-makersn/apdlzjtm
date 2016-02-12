using Common.Func;
using Net.Common.Filter;
using Net.Common.Helper;
using Net.Common.Model;
using Net.Framework.BizDac;
using Net.Framework.StoreModel;
using Net.Framework.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace Makers.Store.Controllers
{
    public class ItemController : BaseController
    {
        StoreItemDac sItemDac = new StoreItemDac();
        StoreItemFileDac sItemFileDac = new StoreItemFileDac();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public ActionResult Detail(string no)
        {
            int articleNo = 0;
            var visitorNo = profileModel.UserNo;
            //ViewBag.GoReply = goReply;
            StoreItemDetailT itemDetail = new StoreItemDetailT();
            if (Int32.TryParse(no, out articleNo))
            {
                //조회수 증가 방지
                if (Request.Cookies[no] == null)
                {
                    Response.Cookies[no].Value = no;
                    Response.Cookies[no].Expires = DateTime.Now.AddDays(1);

                    //뷰 업데이트
                }

                itemDetail = sItemDac.GetItemDetailByItemNo(articleNo, visitorNo);

                itemDetail.DeliveryName = EnumHelper.GetEnumTitle((StoreDeliveryType)itemDetail.DeliveryType);

                if ((itemDetail.StoreMemberNo != visitorNo && profileModel.UserLevel < 50) && itemDetail.UseYn.ToUpper() == "N")
                {
                    return Content("<script>alert('비공개 처리된 게시물 입니다.'); location.href='/';</script>");
                }
            }
            else
            {

            }

            string itemContents = itemDetail != null ? itemDetail.Contents : string.Empty;
            itemDetail.Contents = new HtmlFilter().PunctuationEncode(itemContents);
            itemDetail.Contents = new HtmlFilter().ConvertContent(itemContents);

            ViewBag.MetaDescription = itemContents;

            ViewBag.MainImg = itemDetail != null ? itemDetail.MainImgName : string.Empty;

            ViewBag.Files = sItemFileDac.GetItemFileByItemNo(articleNo);
            ViewBag.ListCnt = 5;
            ViewBag.ListList = null;

            ViewBag.VisitorNo = visitorNo;

            ViewBag.No = no;
            ViewBag.CodeNo = itemDetail.CodeNo;

            ViewBag.Class = "bdB mgB15";
            ViewBag.WrapClass = "bgW";

            return View(itemDetail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Upload()
        {
            ViewBag.Temp = new DateTimeHelper().ConvertToUnixTime(DateTime.Now);

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Upload(FormCollection collection)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;

            long storeItemNo = 0;

            string paramNo = collection["No"];
            string paramTemp = collection["temp"];
            string paramMode = collection["mode"];
            int mainImg = Convert.ToInt32(collection["main_img"]);
            int basePrice = Convert.ToInt32(collection["BasePrice"]);
            string paramTitle = collection["item_title"];
            string paramContents = collection["item_contents"];
            int paramCodeNo = Convert.ToInt32(collection["category_no"]);
            //배송코드
            int paramDeliveryType = Convert.ToInt32(collection["delivery_no"]);
            string paramDelNo = collection["del_no"];
            string ParamVideoSource = collection["insertVideoSource"];

            string tags = collection["item_tags"];

            var mulltiSeq = collection["multi[]"];
            string[] seqArray = mulltiSeq.Split(',');

            if (tags.Length > 1000)
            {
                response.Message = "태그는 1000자 이하로 가능합니다.";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            StoreItemT storeItem = null;

            if (!long.TryParse(paramNo, out storeItemNo))
            {
                response.Success = false;
                response.Message = "pk error";
            }

            if (storeItemNo > 0)
            {
                //update
                storeItem = sItemDac.GetItemByNo(storeItemNo);

                if (storeItem != null)
                {
                    if (storeItem.StoreMemberNo == profileModel.UserNo && storeItem.Temp == paramTemp)
                    {
                        storeItem.UpdDt = DateTime.Now;
                        storeItem.UpdId = profileModel.UserId;
                        storeItem.RegIp = IPAddressHelper.GetIP(this);
                    }
                }
            }
            else
            {
                //save
                storeItem = new StoreItemT();
                storeItem.StoreMemberNo = profileModel.UserNo;
                storeItem.Tags = tags;
                storeItem.Temp = paramTemp;
                storeItem.ViewCnt = 0;
                storeItem.RegDt = DateTime.Now;
                storeItem.RegId = profileModel.UserId;
                storeItem.RegIp = IPAddressHelper.GetIP(this);
                storeItem.FeaturedVisibility = "N";
                storeItem.FeaturedYn = "N";
            }

            if (storeItem != null)
            {
                storeItem.Title = paramTitle;
                storeItem.CodeNo = paramCodeNo;
                storeItem.BasePrice = basePrice;
                storeItem.Tags = tags;
                storeItem.MainImg = mainImg;
                storeItem.Contents = paramContents;
                storeItem.VideoSource = ParamVideoSource;
                //배송코드
                storeItem.DeliveryType = paramDeliveryType;
                storeItem.UseYn = paramMode == "upload" ? "Y" : "N";

                using (var transaction = new TransactionScope())
                {
                    if (storeItem.No > 0)
                    {
                        bool ret = sItemDac.UpdateItem(storeItem);
                        if (ret)
                        {
                            storeItemNo = storeItem.No;
                        }
                    }
                    else
                    {
                        storeItemNo = sItemDac.AddItem(storeItem);
                    }

                    if (storeItemNo <= 0) throw new ArgumentException("실패");

                    IList<StoreItemFileT> storeItemFiles = sItemFileDac.GetItemFilesByTemp(storeItem.Temp);

                    foreach (StoreItemFileT file in storeItemFiles)
                    {
                        file.StoreItemNo = storeItemNo;
                        bool ret = sItemFileDac.UpdateItemFile(file);
                        if(!ret) throw new ArgumentException("실패");
                    }

                    transaction.Complete();
                }

                response.Success = storeItemNo > 0;
                response.Result = storeItemNo.ToString();
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #region img upload
        /// <summary>
        /// img upload
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImgUpload(HttpPostedFileBase file, string temp, string fileIdx)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;
            string fileName = string.Empty;
            string[] idxArr = null;

            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    string[] extType = { ".jpg", ".png", ".gif" };

                    if (!string.IsNullOrEmpty(fileIdx))
                    {
                        idxArr = fileIdx.Split(',');
                    }
                    string extension = Path.GetExtension(file.FileName).ToLower();
                    if (extType.Contains(extension))
                    {
                        fileName = new UploadFunc().FileUpload(file, ImageReSize.GetStoreResize(), "Store", null);

                        StoreItemFileT storeItemFile = new StoreItemFileT();

                        storeItemFile.Temp = temp;
                        storeItemFile.FileGbn = "img";
                        storeItemFile.StoreMemberNo = profileModel.UserNo;
                        storeItemFile.Name = file.FileName;
                        storeItemFile.ReName = fileName;
                        storeItemFile.FileExt = extension;
                        storeItemFile.MimeType = file.ContentType;
                        storeItemFile.FileSize = file.ContentLength;

                        storeItemFile.Idx = idxArr != null ? idxArr.Length + 1 : 0;
                        storeItemFile.ThumbYn = "Y";
                        storeItemFile.ImgUseYn = "Y";
                        storeItemFile.UseYn = "Y";
                        storeItemFile.RegIp = IPAddressHelper.GetIP(this);
                        storeItemFile.RegId = profileModel.UserId;
                        storeItemFile.RegDt = DateTime.Now;

                        long storeItemFileNo = sItemFileDac.AddItemFile(storeItemFile);

                        response.Success = storeItemFileNo > 0;
                        response.Result = storeItemFileNo.ToString();
                    }
                    else
                    {
                        response.Message = "gif, jpg, png 형식 파일만 가능합니다.";
                    }

                    if (idxArr != null && idxArr.Length > 1)
                    {
                        //출력 할 순서 update
                        for (int i = 0; i < idxArr.Length; i++)
                        {
                            StoreItemFileT storeItemFile = sItemFileDac.GetItemFileByNo(Convert.ToInt64(idxArr[i]));

                            storeItemFile.Idx = i + 1;
                            storeItemFile.UpdDt = DateTime.Now;
                            storeItemFile.UpdId = profileModel.UserId;

                            bool ret = sItemFileDac.UpdateItemFile(storeItemFile);
                        }
                    }
                }
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 업로드된 파일 리스트
        /// <summary>
        /// 업로드된 파일 리스트
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult LoadUploadFiles(FormCollection collection)
        {
            string paramNo = collection["No"];
            string uploadCnt = collection["uploadCnt"];
            string temp = collection["temp"];
            string mode = collection["mode"];
            string delNo = collection["del_no"];

            ViewBag.UploadCnt = uploadCnt;

            IList<StoreItemFileT> fileLst = new List<StoreItemFileT>();

            if (mode == "edit")
            {
                fileLst = sItemFileDac.GetItemFileByItemNo(long.Parse(paramNo));
            }
            else
            {
                fileLst = sItemFileDac.GetItemFilesByTemp(temp);
            }

            if (!string.IsNullOrEmpty(delNo))
            {
                string[] delNos = delNo.Split(',');

                IList<StoreItemFileT> files = fileLst.Where(n => delNos.Contains(n.No.ToString())).ToList();

                fileLst = fileLst.Except(files).ToList();
            }

            return PartialView(fileLst.OrderBy(m => m.Idx).ToList());
        }
        #endregion

        #region AppendFile
        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        public PartialViewResult AppendFile(long no, int idx)
        {
            StoreItemFileT file = sItemFileDac.GetItemFileByNo(no);
            ViewBag.Index = idx;
            return PartialView(file);
        }
        #endregion
    }
}
