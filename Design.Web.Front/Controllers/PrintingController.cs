using System.Collections.Generic;
using System.Web.Mvc;
using Makersn.BizDac;
using Makersn.Models;
using Design.Web.Front.Helper;
using System.Linq;
using PagedList.Mvc;
using PagedList;
using System;
using Design.Web.Front.Models;
using System.Web;
using System.IO;
using Makersn.Util;
using System.EnterpriseServices;

namespace Design.Web.Front.Controllers
{
    [RoutePrefix("printer")]
    public class PrintingController : BaseController
    {
        MemberDac _memberDac = new MemberDac();
        PrinterDac _printerDac = new PrinterDac();
        PrinterMemberDac _printerMemberDac = new PrinterMemberDac();
        PrinterCommentDac _printerCommentDac = new PrinterCommentDac();
        PrinterFileDac _printerFileDac = new PrinterFileDac();
        PrinterModelDac _printerModelDac = new PrinterModelDac();
        PrinterMaterialDac _printerMaterialDac = new PrinterMaterialDac();
        MaterialDac _materialDac = new MaterialDac();
        OrderDac _orderDac = new OrderDac();
        OrderDetailDac _orderDetailDac = new OrderDetailDac();
        PrinterOurputImgDac _printerOurputImgDac = new PrinterOurputImgDac();
        ReviewDac _reviewDac = new ReviewDac();


        public ActionResult Index()
        {
            //ViewBag.RecommendList = _printerDac.SerachPrinterInMain("", 0, 0, "R", "");
            //ViewBag.NewList = _printerDac.SerachPrinterInMain("", 0, 0, "N", "");
            ViewBag.GnbArea = "on";

            IList<PrinterT> recommendList = _printerDac.SearchPrinterForMainTop4("R");
            IList<PrinterT> popList = _printerDac.SearchPrinterForMainTop4("P");
            ViewBag.RecommendList = recommendList;
            ViewBag.NewList = _printerDac.SearchPrinterForMainTop4("N");
            ViewBag.PopList = popList == null ? recommendList : popList;

            ViewBag.MatList = _printerDac.GetAllMaterialList();

            ViewBag.IsMain = "Y";

            return View();
        }

        public ActionResult PrtTest(int printerNo)
        {
            ViewBag.PrinterNo = printerNo;
            ViewBag.FileName = new ArticleFileDac().GetArticleFileByType("test").Rename;
            return View();
        }

        [Authorize, HttpGet]
        public ActionResult PrtTestUpload(int printerNo)
        {
            ViewBag.Temp = printerNo + "_" + new DateTimeHelper().ConvertToUnixTime(DateTime.Now);
            ViewBag.PrinterNo = printerNo;
            return View();
        }

        [HttpPost]
        public JsonResult PrtTestUpload(FormCollection collection)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            //string delno = collection["del_no"];
            //int printerNo = System.Convert.ToInt32(collection["printerNo"]);
            //string temp = collection["temp"];
            //_printerDac.UpdatePrinterTemp(printerNo, delno, temp);

            int printerNo = System.Convert.ToInt32(collection["printerNo"]);
            string[] imgNameInfo = collection["ImgNameInfo"].Split(';');
            string[] imgReNameInfo = collection["ImgRenameInfo"].Split(';');
            //string[] imgSizeInfo = collection["ImgSizeInfo"].Split(';');
            string message = collection["item_message"];

            if (imgNameInfo != null && imgReNameInfo != null)
            {
                if (imgNameInfo.Length == imgReNameInfo.Length)
                {
                    OrderT order = new OrderT();
                    order.PrinterMemberNo = Profile.UserNo;
                    order.MemberNo = 1;
                    order.PrinterNo = printerNo;
                    order.TestFlag = "Y";
                    order.RequireComment = message;
                    order.OrderStatus = (int)Makersn.Util.MakersnEnumTypes.OrderState.테스트요청;
                    order.RegDt = DateTime.Now;
                    order.RegId = Profile.UserId;


                    long orderNo = _orderDac.InsertOrderInTest(order);

                    List<PrinterOutputImageT> imgList = new List<PrinterOutputImageT>();
                    for (int i = 0; i < imgNameInfo.Length; i++)
                    {
                        if (imgNameInfo[i] != "" && imgReNameInfo[i] != "")
                        {
                            PrinterOutputImageT img = new PrinterOutputImageT();
                            img.OrderNo = orderNo;
                            img.ImageName = imgNameInfo[i];
                            img.ImageReName = imgReNameInfo[i];
                            img.RegId = Profile.UserId;
                            img.RegDt = DateTime.Now;
                            imgList.Add(img);
                        }
                    }
                    _printerOurputImgDac.InsertImgByString(imgList);

                    //notice


                }
            }
            response.Success = true;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #region 업로드된 파일 리스트
        /// <summary>
        /// 업로드된 파일 리스트
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [HttpPost]
        public PartialViewResult PrtTestUploadView(FormCollection collection)
        {
            string paramNo = collection["printerNo"];
            string uploadCnt = collection["uploadCnt"];
            string temp = collection["temp"];
            string mode = collection["mode"];
            string delNo = collection["del_no"];

            ViewBag.UploadCnt = uploadCnt;

            IList<PrinterFileT> fileLst = new List<PrinterFileT>();

            if (mode == "edit")
            {
                fileLst = _printerFileDac.GetFileList(int.Parse(paramNo));
            }
            else
            {
                fileLst = _printerFileDac.GetPrinterFilesByTemp(temp);
            }

            if (!string.IsNullOrEmpty(delNo))
            {
                string[] delNos = delNo.Split(',');

                IList<PrinterFileT> files = fileLst.Where(n => delNos.Contains(n.No.ToString())).ToList();

                fileLst = fileLst.Except(files).ToList();
            }

            return PartialView(fileLst);
        }
        #endregion



        public PartialViewResult MemberOtherPrinter(int no)
        {
            IList<PrinterT> list = _printerDac.GetPrinterTop4ByPrtMemberNo(no);
            return PartialView(list);
        }




        #region 리스팅 페이지
        public ActionResult List(int page = 1, string pageGubun = "")
        {
            int pageSize = 20;
            IList<PrinterT> list = null;
            int totalCnt = 0;
            string recommendYn = "";
            ViewBag.Gubun = pageGubun;

            switch (pageGubun)
            {
                case "N":
                    ViewBag.PageTitle = "신규 프린터";
                    break;

                case "P":
                    ViewBag.PageTitle = "인기 프린터";
                    break;

                case "R":
                    ViewBag.PageTitle = "추천 프린터";
                    recommendYn = "Y";
                    break;
                default:
                    break;
            }

            string recommendVisibility = recommendYn;

            int fromIndex = ((page - 1) * pageSize) + 1;
            int toIndex = page * pageSize;


            list = _printerDac.GetSearchList("", recommendYn, recommendVisibility, pageGubun, fromIndex, toIndex);

            totalCnt = _printerDac.GetSearchCount("", recommendYn, recommendVisibility);

            PagerInfo pager = new PagerInfo();
            pager.CurrentPageIndex = page;
            pager.PageSize = pageSize;
            pager.RecordCount = totalCnt;

            ViewBag.GnbArea = "on";

            PagerQuery<PagerInfo, IList<PrinterT>> model = new PagerQuery<PagerInfo, IList<PrinterT>>(pager, list);
            return View(model);
        }


        #endregion 리스팅 페이지

        #region 프린터 검색
        public ActionResult PrtSearch(int page = 1, string location = "", int quality = 0, int material = 0, string text = "")
        {
            IList<PrinterT> list = _printerDac.SearchPrinterInMain(location, quality, material, "", text);

            if (list.Count == 0)
            {
                int pageSize = 20;
                int fromIndex = ((page - 1) * pageSize) + 1;
                int toIndex = page * pageSize;

                list = _printerDac.GetSearchList("", "Y", "Y", "R", fromIndex, toIndex);
                ViewBag.NoResult = "Y";
            }

            //ViewBag.WrapClass = "bgW";
            string searchText = string.Empty;
            if (text != "")
            {
                searchText += text;
            }

            if (location != "")
            {
                searchText += searchText == "" ? "" : ",";
                searchText += location;
            }

            if (quality != 0)
            {
                searchText += searchText == "" ? "" : ",";
                searchText += Enum.GetName(typeof(MakersnEnumTypes.QualityType), quality);
            }

            if (material != 0)
            {
                searchText += searchText == "" ? "" : ",";
                searchText += Enum.GetName(typeof(MakersnEnumTypes.Material), material);
            }

            ViewBag.Text = text;
            ViewBag.SearchText = searchText;

            return View(list.OrderByDescending(o => o.RegDt).ToPagedList(page, 20));
        }
        #endregion



        #region 주문 프론트 페이지

        public ActionResult OrderFront(string no)
        {

            return View();
        }


        #endregion 주문 프론트 페이지
        // public ActionResult Index(int page = 1, string codeNo = "0", string pageGubun = "")
        //{
        //    int pageSize = 20;
        //    int codeNum = int.Parse(codeNo);
        //    IList<ArticleDetailT> list = null;
        //    ViewBag.PageTitle = EnumHelper.GetEnumTitle((MakersnEnumTypes.CateName)int.Parse(codeNo)); //EnumTitle 가져오기 string반환
        //    ViewBag.CodeNo = codeNo;
        //    ViewBag.Gubun = pageGubun;

        //    switch (pageGubun)
        //    {
        //        case "N":
        //            ViewBag.PageTitle = "신규";
        //            break;

        //        case "P":
        //            ViewBag.PageTitle = "인기";
        //            break;

        //        case "R":
        //            ViewBag.PageTitle = "추천";
        //            break;
        //        default:
        //            break;
        //    }

        //    int totalCnt = articleDac.GetTotalCountByOption(Profile.UserNo, codeNum, "", pageGubun);

        //    int fromIndex = ((page - 1) * pageSize) + 1;
        //    int toIndex = page * pageSize;

        //    PagerInfo pager = new PagerInfo();
        //    pager.CurrentPageIndex = page;
        //    pager.PageSize = pageSize;
        //    pager.RecordCount = totalCnt;


        //    list = articleDac.GetListByOption(Profile.UserNo, codeNum, pageGubun, fromIndex, toIndex);
        //    if (pageGubun == "R")
        //    {
        //        if (list.Count() == 0)
        //        {
        //            list = articleDac.GetListByOption(Profile.UserNo, codeNum, "N", fromIndex, toIndex);
        //        }
        //    }
        //    PagerQuery<PagerInfo, IList<ArticleDetailT>> model = new PagerQuery<PagerInfo, IList<ArticleDetailT>>(pager, list);

        //    return View(model);
        //}
        //#endregion

        #region detail
        public ActionResult Detail(int no = 1)
        {
            PrinterT printer = new PrinterT();
            printer = _printerDac.GetPrinterDetailByPrinterNo(no);

            MemberT member = _memberDac.GetMemberProfile(printer.MemberNo);
            PrinterMemberT spot = _printerMemberDac.GetPrinterMemberByNo(printer.MemberNo);
            printer.MemberName = spot.SpotName;
            printer.MemberProfilePic = member.ProfilePic;
            //printer.PostMode = _printerMemberDac.GetPrinterMemberByNo(printer.MemberNo).PostMode;

            IList<ReviewT> reviewList = new List<ReviewT>();
            reviewList = _reviewDac.GetReviewListByPrinterNo(printer.No);

            int totalScore = 0;
            int index = 0;

            foreach (ReviewT review in reviewList)
            {
                totalScore += review.Score;
                index++;
            }
            if (index > 0)
            {
                printer.Score = totalScore / index + totalScore % index * 0.1;
            }
            else
            {
                printer.Score = 0;
            }

            IList<MaterialT> matList = _printerDac.GetMatrialListByPrinterNo(printer.No);
            ViewBag.MatList = matList;


            //IList<PrinterColorT> colorList = _printerDac.GetMaterialColorByPrinterNo(printer.No, matList[0].No);

            List<PrinterColorT> colorList = new List<PrinterColorT>();

            foreach (PrinterMaterialT mat in printer.PrinterMaterialList)
            {
                colorList.AddRange(mat.MaterialColorList);
            }

            printer.MinPrice = colorList.OrderBy(o => o.UnitPrice).Take(1).SingleOrDefault<PrinterColorT>().UnitPrice;
            printer.MaxPrice = colorList.OrderByDescending(o => o.UnitPrice).Take(1).SingleOrDefault<PrinterColorT>().UnitPrice;

            ViewBag.ColorList = colorList;
            ViewBag.Spot = spot;

            ViewBag.PrinterFileList = _printerDac.GetPrinterFile(no);
            //ViewBag.AuthorNo = Profile.UserNo; //프린팅 팔로우 기능 없다고 판단해서 구현 안함 만약 한다면 본인 여부 부터 체크

            ViewBag.Class = "bdB mgB15";
            ViewBag.WrapClass = "bgW";
            ViewBag.GnbArea = "on";
            ViewBag.GnbAreaClass = "mgB15";

            return View(printer);
        }
        #endregion

        #region 댓글 가져오기
        public PartialViewResult Reply(int page = 1, int no = 1, string goReply = "N")
        {
            ViewBag.No = no;
            ViewBag.MemberNo = Profile.UserNo;
            ViewBag.GoReply = goReply;
            IList<PrinterCommentT> list = _printerCommentDac.GetPrinterCommentList(no);
            return PartialView(list.OrderByDescending(o => o.Regdt).ToPagedList(page, 5));
        }
        #endregion

        #region 댓글 삭제
        /// <summary>
        /// 댓글 삭제
        /// </summary>
        /// <param name="commentNo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CommentDel(string commentNo = "")
        {
            commentNo = Base64Helper.Base64Decode(commentNo);
            _printerCommentDac.DeletePrinterCommentByNo(int.Parse(commentNo));
            return Json(new { Message = "삭제되었습니다.", Success = true });
        }
        #endregion

        #region 댓글 수정
        [HttpPost]
        public JsonResult CommentUpd(string commentNo = "", string content = "")
        {
            PrinterCommentT prt = new PrinterCommentT();
            prt.No = int.Parse(commentNo);
            prt.Content = content;
            prt.UpdDt = DateTime.Now;
            prt.UpdId = Profile.UserId;

            //no = Base64Helper.Base64Decode(no);
            _printerCommentDac.UpdatePrinterCommentByNo(prt);
            return Json(new { Message = "수정되었습니다.", Success = true });
        }
        #endregion

        #region 댓글 추가
        public JsonResult CommentAdd(int printerNo = 0, string content = "", string printerMemberNo = "0")
        {
            if (Profile.UserNo == 0) { return Json(new { Success = false }); };
            printerMemberNo = Base64Helper.Base64Decode(printerMemberNo);
            string getIp = IPAddressHelper.GetIP(this);
            PrinterCommentT prt = new PrinterCommentT();
            prt.PrinterNo = printerNo;
            prt.Content = content;
            prt.Regdt = DateTime.Now;
            prt.RegId = Profile.UserId;
            prt.MemberNoRef = int.Parse(printerMemberNo);
            prt.MemberNo = Profile.UserNo;
            prt.Writer = Profile.UserNm;
            prt.RegIp = getIp;
            _printerCommentDac.InsertPrinterComment(prt);

            return Json(new { Message = "등록되었습니다.", Success = true });
        }
        #endregion

        [Authorize, HttpGet]
        public ActionResult PrtUpload(int printerNo = 0, string flag = "")
        {
            ViewBag.Temp = Profile.UserNo + "_" + new DateTimeHelper().ConvertToUnixTime(DateTime.Now);
            ViewBag.PrinterBrandlList = _printerModelDac.GetPrinterBrandList();
            ViewBag.MaterialList = _materialDac.getMaterialList();
            PrinterT printer = new PrinterT();
            if (printerNo != 0)
            {
                printer = printer = _printerDac.GetPrinterByNo(printerNo);
                printer.PrinterMaterialList = _printerMaterialDac.GetPrinterMaterialByPrinterNo(printerNo);
                printer.PrinterFileList = _printerFileDac.GetFileList(printerNo);
            }

            ViewBag.Flag = flag;

            return View(printer);
        }

        [HttpPost]
        public JsonResult PrtUpload(FormCollection collection)
        {

            bool Success = false;
            string Result = string.Empty;
            string Message = string.Empty;
            //string Type = string.Empty;

            string printerNoStr = collection["PrinterNo"];
            string[] imgNameInfo = collection["ImgNameInfo"].Split(';');
            string[] imgReNameInfo = collection["ImgRenameInfo"].Split(';');
            string[] imgSizeInfo = collection["ImgSizeInfo"].Split(';');
            string[] matInfo = collection["matInfo"].Split(';');


            PrinterT printer = null;
            int printerNo = 0;

            if (!Int32.TryParse(printerNoStr, out printerNo))
            {
                Success = false;
                Message = "pk error";
            }
            //get Printer
            if (printerNoStr != "" && printerNoStr != "0")
            {
                printerNo = System.Convert.ToInt32(printerNoStr);
                printer = _printerDac.GetPrinterByNo(printerNo);
            }
            else
            {
                printer = new PrinterT();
            }

            printer.Comment = collection["item_title"];
            printer.Quality = collection["quality"] != "" ? System.Convert.ToInt32(collection["quality"]) : 0;
            //printer.PostMode = collection["postMode"] != "" ? System.Convert.ToInt32(collection["postMode"]) : 0;
            //printer.PostPrice = collection["postPrice"] != "" ? System.Convert.ToInt32(collection["postPrice"]) : 0;
            printer.MainImg = collection["main_img"] != "" ? System.Convert.ToInt32(collection["main_img"]) : 0;
            printer.Status = System.Convert.ToInt32(collection["status"]);
            printer.SaveFlag = collection["saveFlag"];

            int printerModelNo = collection["printerModelNo"] != "" ? System.Convert.ToInt32(collection["printerModelNo"]) : 0;

            PrinterModelT printerModel = _printerModelDac.GetPrinterModelByNo(printerModelNo);
            printer.Brand = printerModel.Brand;
            printer.Model = printerModel.Model;


            printer.MemberNo = Profile.UserNo;
            //printer.DelDt = System.DateTime.Now;


            //gooksong




            //if (!Int32.TryParse(printerNoStr, out printerNo))
            //{
            //    Success = false;
            //    Message = "pk error";
            //}
            //insert Printer
            if (printerNoStr != "" && printerNoStr != "0")
            {
                printer.UpdDt = System.DateTime.Now;
                printer.UpdId = Profile.UserNm;
                _printerDac.UpdatePrinter(printer);
            }
            else
            {
                printer.RegId = Profile.UserNm;
                printer.RegDt = System.DateTime.Now;
                printer.DelFlag = "N";
                printer.RecommendPriority = 0;
                //printer.RecommendDt = System.DateTime.Now;
                printer.RecommendYn = "N";
                printer.TestCompleteFlag = "N";
                printerNo = _printerDac.InsertPrinter(printer);
            }

            Success = _printerDac.doPrtEdit(printerNo, Profile.UserId, Profile.UserNm, IPAddressHelper.GetIP(this), printer, imgNameInfo, imgReNameInfo, imgSizeInfo, matInfo);


            //int printerNo = 70;
            //insert PrinterFile
            //if(printerNo != 0){

            //}
            //_printerFileDac.RemovePrinterFileWithPrtNo(printerNo);
            //if (imgNameInfo != null && imgReNameInfo != null && imgSizeInfo != null)
            //{
            //    if (imgNameInfo.Length == imgReNameInfo.Length && imgNameInfo.Length == imgSizeInfo.Length && imgReNameInfo.Length == imgSizeInfo.Length)
            //    {
            //        //img Upload to DB
            //        List<PrinterFileT> imgList = new List<PrinterFileT>();
            //        for (int i = 1; i < imgNameInfo.Length; i++)
            //        {
            //            // the first Img must be main Img
            //            if (imgNameInfo[i] != "" && imgReNameInfo[i] != "")
            //            {
            //                PrinterFileT img = new PrinterFileT();
            //                img.PrinterNo = printerNo;
            //                img.Name = imgNameInfo[i];
            //                img.Rename = imgReNameInfo[i];
            //                img.Size = imgSizeInfo[i];
            //                img.FileGubun = "prt_img";
            //                img.Seq = i;
            //                img.RegId = Profile.UserId;
            //                img.RegDt = DateTime.Now;
            //                img.RegIp = IPAddressHelper.GetIP(this);
            //                imgList.Add(img);
            //            }
            //        }

            //        _printerFileDac.InsertPrinterFileByList(imgList);
            //    }
            //}

            ////insert material
            //_printerMaterialDac.RemovePrinterMaterialAndColorWithPrtNo(printerNo);
            //for (int i = 0; i < matInfo.Length; i++)
            //{
            //    if (matInfo[i] != "")
            //    {
            //        _printerMaterialDac.InsertWithColorByStr(printerNo, Profile.UserNm, matInfo[i]);
            //    }
            //}



            //Success = true;
            Result = printerNo.ToString();
            Message = "";

            return Json(new { Success = Success, Message = Message, Result = Result });
        }

        public JsonResult ImgUpload(FormCollection collection)
        {

            bool Success = false;
            string Message = string.Empty;
            string ReName = string.Empty;
            string Name = string.Empty;
            string Size = string.Empty;
            string fileName = string.Empty;

            HttpPostedFileBase imgupload = Request.Files["imgupload"];
            string temp = collection["temp"];
            string fileGubun = collection["fileGubun"];

            if (imgupload != null)
            {
                if (imgupload.ContentLength > 0)
                {
                    string[] extType = { "jpg", "png", "gif" };

                    string extension = Path.GetExtension(imgupload.FileName).ToLower().Replace(".", "").ToLower();

                    if (extType.Contains(extension))
                    {
                        ReName = FileUpload.UploadFile(imgupload, new ImageSize().GetPrinterResize(), "Printer", null);
                        Name = imgupload.FileName;
                        Size = imgupload.ContentLength.ToString();
                        Success = true;
                    }
                    else
                    {
                        Message = "gif, jpg, png 형식 파일만 가능합니다.";
                    }
                }
            }
            return Json(new { Success = Success, Message = Message, Rename = ReName, Name = Name, Size = Size });
        }


        #region 업로드된 파일 리스트
        /// <summary>
        /// 업로드된 파일 리스트
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [HttpPost]
        public PartialViewResult PrtUploadView(FormCollection collection)
        {
            string paramNo = collection["No"];
            string uploadCnt = collection["uploadCnt"];
            string temp = collection["temp"];
            string mode = collection["mode"];
            string delNo = collection["del_no"];

            ViewBag.UploadCnt = uploadCnt;

            IList<PrinterFileT> fileLst = new List<PrinterFileT>();

            if (mode == "edit")
            {
                fileLst = _printerFileDac.GetFileList(int.Parse(paramNo));
            }
            else
            {
                fileLst = _printerFileDac.GetPrinterFilesByTemp(temp);
            }

            if (!string.IsNullOrEmpty(delNo))
            {
                string[] delNos = delNo.Split(',');

                IList<PrinterFileT> files = fileLst.Where(n => delNos.Contains(n.No.ToString())).ToList();

                fileLst = fileLst.Except(files).ToList();
            }

            return PartialView(fileLst);
        }
        #endregion

        public PartialViewResult GetPrinterModel(int no)
        {
            IList<PrinterModelT> list = _printerModelDac.GetPrinterModelByPrinterModelNo(no);

            return PartialView(list);
        }

        //프린터 등록 요청(기능만 구현해둠)
        public JsonResult RequestAddPrinterModel(string brand, string model)
        {
            AjaxResponseModel response = new AjaxResponseModel();

            PrinterModelT printerModel = new PrinterModelT();

            printerModel.Brand = brand;
            printerModel.Model = model;
            printerModel.ApprYn = "N";
            printerModel.PropMemberNo = Profile.UserNo;
            printerModel.RegDt = DateTime.Now;
            printerModel.RegId = Profile.UserId;

            response.Success = _printerModelDac.AddPrinterModel(printerModel);
            response.Message = "요청 되었습니다.";

            return Json(response);
        }


        #region
        public ActionResult PrtMng(int page = 1)
        {
            int prtMemNo = Profile.UserNo;
            IList<PrinterT> printerList = _printerDac.GetPrinterByPrtMemberNo(prtMemNo);
            foreach (PrinterT printer in printerList)
            {
                printer.PrinterMaterialList = _printerMaterialDac.GetPrinterMaterialByPrinterNo(printer.No);
                printer.PrinterFileList = _printerFileDac.GetFileList(printer.No);
                int minPrice = 999999999;//max Num for price
                int maxPrice = -1;//min num for price
                foreach (PrinterMaterialT material in printer.PrinterMaterialList)
                {
                    foreach (PrinterColorT color in material.MaterialColorList)
                    {
                        maxPrice = Math.Max(maxPrice, color.UnitPrice);
                        minPrice = Math.Min(minPrice, color.UnitPrice);
                    }
                    material.MaterialName = _materialDac.getMaterialNameByNo(material.MaterialNo);
                }
                printer.MaxPrice = maxPrice;
                printer.MinPrice = minPrice;
            }

            //ViewBag.PrinterList = printerList;
            return View(printerList.OrderByDescending(o => o.RegDt).ToPagedList(page, 20));
        }
        #endregion

        #region 약관 동의
        public ActionResult AcceptTermsOfUse()
        {
            PrinterMemberT printerMember = _printerMemberDac.GetPrinterMemberByNo(Profile.UserNo);
            if (printerMember != null)
            {
                return Redirect("/printing/prtUpload");
            }
            return View();
        }
        #endregion

        #region 스팟 오픈
        public ActionResult SpotOpen()
        {
            int memberNo = Profile.UserNo;
            PrinterMemberT printerMember = _printerMemberDac.GetPrinterMemberByNo(memberNo);
            if (printerMember != null)
            {
                return Redirect("/PrintingProfile");
            }
            //if (printerMember == null)
            //{
            //    printerMember = new PrinterMemberT();
            //    printerMember.MemberNo = memberNo;
            //    printerMember.SpotAddress = "";
            //    printerMember.Bank = "";
            //    printerMember.PostMode = 2;
            //    printerMember.SpotName = "";
            //    printerMember.PrinterProfileMsg = "";
            //    printerMember.TaxbillFlag = "Y";
            //    printerMember.HomePhone = "--";
            //    printerMember.CellPhone = "--";
            //    printerMember.ViewCnt = 0;
            //    printerMember.BankName = "";

            //}
            return PartialView();
        }
        #endregion

        public JsonResult CheckSpotUrl(string url)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            if (_printerMemberDac.CheckSpotUrl(url) == true)
            {
                response.Result = "Y";
            }
            else
            {
                response.Result = "N";
            }

            return Json(response);
        }

        public JsonResult DeletePrt(int printerNo)
        {

            AjaxResponseModel response = new AjaxResponseModel();
            IList<OrderT> orderList = _orderDac.GetOrderByPrtNo(printerNo, (int)MakersnEnumTypes.OrderState.출력중, (int)MakersnEnumTypes.OrderState.배송중);
            if (orderList.Count == 0)
            {
                PrinterT printer = _printerDac.GetPrinterByNo(printerNo);
                printer.DelFlag = "Y";
                printer.DelDt = System.DateTime.Now;
                _printerDac.UpdatePrinter(printer);
                response.Success = true;
            }
            else
            {
                response.Message = "아직 완료되지 않은 주문이 있어 해당 프린터를 삭제할 수 없습니다!";
                response.Success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SpotOpenDone()
        {
            PrinterMemberT spot = _printerMemberDac.GetPrinterMemberByNo(Profile.UserNo);
            ViewBag.ContClass = "w100";
            return View(spot);
        }
    }
}
