using Common.Func;
using Net.Common.Helper;
using Net.Common.Model;
using Net.Framework.BizDac;
using Net.Framework.StoreModel;
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

        public ActionResult Index()
        {
            return View();
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
            string paramTitle = collection["article_title"];
            string paramContents = collection["article_contents"];
            int paramCodeNo = Convert.ToInt32(collection["lv1"]);
            //배송코드
            int paramDeliveryType = Convert.ToInt32(collection["copyright"]);
            string paramDelNo = collection["del_no"];
            string ParamVideoSource = collection["insertVideoSource"];

            string tags = collection["article_tags"];

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
                    if (storeItem.MemberNo == profileModel.UserNo && storeItem.Temp == paramTemp)
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
                storeItem.MemberNo = profileModel.UserNo;
                storeItem.Tags = tags;
                storeItem.Temp = paramTemp;
                storeItem.ViewCnt = 0;
                storeItem.RegDt = DateTime.Now;
                storeItem.RegId = profileModel.UserId;
                storeItem.RegIp = IPAddressHelper.GetIP(this);
                storeItem.FeaturedYn = "N";
            }

            if (storeItem != null)
            {
                storeItem.Title = paramTitle;
                storeItem.CodeNo = paramCodeNo;
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
                        storeItemNo = sItemDac.AddItem(storeItem);
                    }
                    else
                    {
                        bool ret = sItemDac.UpdateItem(storeItem);
                        if (ret) 
                        {
                            storeItemNo = storeItem.No; 
                        }
                    }

                    if (storeItemNo <= 0) throw new ArgumentException("실패");

                    IList<StoreItemFileT> storeItemFiles = sItemFileDac.GetItemFilesByTemp(storeItem.Temp);

                    foreach (StoreItemFileT file in storeItemFiles)
                    {
                        file.ItemNo = storeItemNo;
                        bool ret = sItemFileDac.UpdateItemFile(file);
                        if(!ret) throw new ArgumentException("실패");
                    }

                    transaction.Complete();
                }

                response.Success = storeItemNo > 0;
                response.Result = storeItemNo.ToString(); ;
            }

            //_articleFileDac.UpdateArticleFileSeq(seqArray);

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
                        fileName = new UploadFunc().FileUpload(file, ImageSize.GetStoreResize(), "Store", null);

                        StoreItemFileT storeItemFile = new StoreItemFileT();

                        storeItemFile.Temp = temp;
                        storeItemFile.FileGbn = "img";
                        storeItemFile.MemberNo = profileModel.UserNo;
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
