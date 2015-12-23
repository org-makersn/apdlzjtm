using System.Collections.Generic;
using System.Web.Mvc;
using Makersn.BizDac;
using Makersn.Models;
using System;
using System.Linq;
using PagedList;
using Design.Web.Front.Helper;
using Design.Web.Front.Models;
using System.Web;
using System.IO;
using Makersn.Util;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace Design.Web.Front.Controllers
{

    [RoutePrefix("order")]
    public class OrderController : BaseController
    {

        OrderDac _orderDac = new OrderDac();
        PrinterMemberDac _printerMemberDac = new PrinterMemberDac();
        ArticleDac _articleDac = new ArticleDac();
        ArticleFileDac _articleFileDac = new ArticleFileDac();
        PrinterDac _printerDac = new PrinterDac();
        OrderDetailDac _orderDetailDac = new OrderDetailDac();
        ReviewDac _reviewDac = new ReviewDac();
        PrinterMaterialDac _printerMaterialDac = new PrinterMaterialDac();
        MemberDac _memberDac = new MemberDac();
        PrinterOurputImgDac _printerOurputImgDac = new PrinterOurputImgDac();
        PrinterNoticeDac _PrinterNoticeDac = new PrinterNoticeDac();
        OrderAccountingDac _orderAccountingDac = new OrderAccountingDac();
        MaterialDac _materialDac = new MaterialDac();

        public ActionResult Index(int articleNo = 0, int printerNo = 0, int material = 0, int color = 0, int orderNo = 0)
        {
            if (Profile.UserNo == 0)
            {
                return Content("<script>alert('로그인 후 이용해 주세요.'); history.go(-1);</script>");
            }

            string temp = Profile.UserNo + "_" + new DateTimeHelper().ConvertToUnixTime(DateTime.Now);
            IList<OrderDetailT> orderList = new List<OrderDetailT>();
            //ViewBag.WrapClass = "bgW";

            if (articleNo != 0)
            {
                IList<ArticleFileT> list = _articleFileDac.GetFileList(articleNo);
                STLHelper helper = new STLHelper();
                string save3DFolder = "Article/article_3d";
                string file3Dpath = string.Format("{0}/FileUpload/{1}/", AppDomain.CurrentDomain.BaseDirectory, save3DFolder);

                foreach (ArticleFileT file in list)
                {
                    if (file.FileType == "stl" || file.FileType == "obj")
                    {
                        OrderDetailT data = new OrderDetailT();
                        data.FileName = file.Name;
                        data.FileReName = file.Rename;
                        data.FileImgRename = file.ImgName;
                        data.FileType = file.FileType;
                        data.OrderCount = 1;
                        data.MaterialNo = 0;
                        data.UnitPrice = 0;
                        data.Temp = temp;

                        #region 이부분도 직접 데이터 한번 업데이트 하고 삭제해야할듯
                        StlSize size = helper.GetSizeFor3DFile(file3Dpath + file.Rename, file.Ext);

                        data.SizeX = size.X;
                        data.SizeY = size.Y;
                        data.SizeZ = size.Z;
                        data.Volume = size.Volume;
                        #endregion

                        //data.PrintVolume= new STLHelper().slicing(file3Dpath + file.Rename);
                        data.PrintVolume = file.PrintVolume;

                        data.RegDt = DateTime.Now;
                        data.RegId = Profile.UserId;
                        data.No = _orderDetailDac.InsertOrderDetail(data);
                        orderList.Add(data);
                    }
                }

            }

            if (orderNo != 0)
            {
                OrderT order = _orderDac.GetOrderByNo(orderNo, Profile.UserNo);
                if (order == null || !(order.OrderStatus == 210 || order.OrderStatus == 215))
                {
                    return Content("<script>alert('잘못된 접근입니다'); location.href = '/order/myorder';</script>");
                }
                orderList = _orderDac.GetOrderDetailByMyOrder(orderNo);
                //_orderDac.DeleteOrder(order);
            }


            if (printerNo != 0)
            {
                PrinterT printer = new PrinterT();
                printer.No = printerNo;
                printer.colorNo = color;
                printer.materialNo = material;
                ViewBag.SelPrinter = printer;
            }

            ViewBag.MatList = _printerDac.GetAllMaterialList();

            ViewBag.Temp = temp;
            return View(orderList);
        }

        #region 주문관리 홈
        /// <summary>
        /// 주문관리 리스트
        /// </summary>
        /// <param name="searchType"></param>
        /// <param name="status"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public ActionResult MngHome(int page = 1, string searchType = "ordMemNm", string status = "0", string dtStart = "", string dtEnd = "", string text = "")
        {
            int memberNo = Profile.UserNo;
            DateTime nowDt = DateTime.Now;
            DateTime beforDt = nowDt.AddHours(-12);
            IList<OrderT> newOrderList = _orderDac.GetSearchOrderListPrt(memberNo, (int)MakersnEnumTypes.OrderState.결제완료, (int)MakersnEnumTypes.OrderState.결제완료, "", "0", beforDt.ToString("yyyy-MM-dd"), nowDt.ToString("yyyy-MM-dd"), "");
            IList<OrderT> onGoingOrderList = _orderDac.GetSearchOrderListPrt(memberNo, (int)MakersnEnumTypes.OrderState.출력중, (int)MakersnEnumTypes.OrderState.거래완료, searchType, status, dtStart, dtEnd, text);
            ViewBag.PrtMember = _printerMemberDac.GetPrinterMemberByNoWithReview(memberNo);
            ViewBag.NewOrderList = newOrderList;
            ViewBag.OnGoingOrderList = onGoingOrderList;
            ViewBag.OnGoingOrderCnt = _orderDac.GetMyOrderCntPrt(memberNo, (int)MakersnEnumTypes.OrderState.출력중, (int)MakersnEnumTypes.OrderState.배송완료);
            ViewBag.FinishedOrderCnt = _orderDac.GetMyOrderCntPrt(memberNo, (int)MakersnEnumTypes.OrderState.거래완료, (int)MakersnEnumTypes.OrderState.거래완료);

            if (dtStart != "")
            {
                ViewBag.StartDt = dtStart;
            }
            if (dtEnd != "")
            {
                ViewBag.EndDt = dtEnd;
            }


            return View(onGoingOrderList.ToPagedList(page, 10));
        }

        #endregion

        #region 스팟 주문관리 출력 페이지
        public ActionResult MngDetailPrtPre(long orderNo = 0, int status = 0)
        {
            OrderT order = _orderDac.GetOrderByNoPrt(orderNo, Profile.UserNo);
            if (order == null || order.OrderStatus != (int)MakersnEnumTypes.OrderState.출력중)
            {
                ViewBag.Announce = "something wrong";

            }
            else
            {
                ViewBag.Announce = "Success";
                ViewBag.OrderEntity = order;
                ViewBag.OrderMember = _memberDac.GetMemberProfile(order.MemberNo);
                ViewBag.Printer = _printerDac.GetPrinterByNo(order.PrinterNo);
            }
            return View();
        }

        [HttpGet]
        public ActionResult MngDetailPrtDone(long orderNo)
        {
            OrderT order = _orderDac.GetOrderByNoPrt(orderNo, Profile.UserNo);
            ViewBag.OrderEntity = order;
            ViewBag.OrderMember = _memberDac.GetMemberProfile(order.MemberNo);
            ViewBag.ImgList = _printerOurputImgDac.GetImglistByOrderNo(order.No);
            ViewBag.Printer = _printerDac.GetPrinterByNo(order.PrinterNo);
            return View();
        }
        [HttpPost]
        public JsonResult MngDetailPrtDone(FormCollection collection)
        {
            int orderStat = 0;
            AjaxResponseModel response = new AjaxResponseModel();
            string[] imgNameInfo = collection["ImgNameInfo"].Split(';');
            string[] imgReNameInfo = collection["ImgRenameInfo"].Split(';');
            string message = collection["Message"];
            int orderNo = System.Convert.ToInt32(collection["OrderNo"]);
            int orderMemNo = System.Convert.ToInt32(collection["OrderMemNo"]);

            foreach (MakersnEnumTypes.OrderState enumtemp in Enum.GetValues(typeof(MakersnEnumTypes.OrderState)))
            {
                if (enumtemp == MakersnEnumTypes.OrderState.출력완료)
                {
                    orderStat = (int)enumtemp;
                }

            }
            if (imgNameInfo != null && imgReNameInfo != null)
            {
                if (imgNameInfo.Length == imgReNameInfo.Length)
                {
                    //img Upload to DB
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
                    // notice
                    PrinterNoticeT prtNotice = new PrinterNoticeT();
                    prtNotice.OrderNo = orderNo;
                    prtNotice.MemberNo = Profile.UserNo;
                    prtNotice.MemberNoRef = orderMemNo;
                    prtNotice.Comment = message;
                    prtNotice.Type = "outputImage";
                    prtNotice.IsNew = "Y";
                    prtNotice.RegDt = DateTime.Now;
                    prtNotice.RegId = Profile.UserId;
                    prtNotice.RegIp = IPAddressHelper.GetIP(this);
                    _PrinterNoticeDac.InsertNotice(prtNotice);

                    //change status
                    OrderT order = _orderDac.GetOrderByNoPrt(orderNo, Profile.UserNo);

                    if (orderStat != 0)
                    {
                        order.OrderStatus = orderStat;
                        order.UpdDt = System.DateTime.Now;
                        order.UpdId = Profile.UserId;
                        _orderDac.UpdateOrder(order);
                    }

                    //end 
                    response.Success = true;
                }
            }


            response.Result = orderNo + "";
            return Json(response);
        }

        #endregion



        #region 스팟 주문관리 배송 페이지
        [HttpGet]
        public ActionResult MngDetailPostPre(long orderNo = 0)
        {
            OrderT order = _orderDac.GetOrderByNoPrt(orderNo, Profile.UserNo);
            if (order == null || (order.OrderStatus != (int)MakersnEnumTypes.OrderState.배송요청 && order.OrderStatus != (int)MakersnEnumTypes.OrderState.배송중 && (order.OrderStatus != (int)MakersnEnumTypes.OrderState.배송완료)))
            {
                ViewBag.Announce = "something wrong";
            }
            else
            {

                ViewBag.Announce = "Success";
                ViewBag.OrderEntity = order;
                ViewBag.OrderMember = _memberDac.GetMemberProfile(order.MemberNo);
                ViewBag.Printer = _printerDac.GetPrinterByNo(order.PrinterNo);
                ViewBag.PrinterMember = _printerMemberDac.GetPrinterMemberByNo(order.PrinterMemberNo);
                //ViewBag.OrderNo = orderNo;
                //ViewBag.OrderMemNo = order.MemberNo;
                //ViewBag.DeliveryCompany = order.DeliveryCompany;
                //ViewBag.DeliveryNum = order.DeliveryNum;
                //ViewBag.OrderStat = order.OrderStatus;
                //ViewBag.PostMode = order.PostMode;
                //ViewBag.OrderMember = _memberDac.GetMemberProfile(order.MemberNo);
            }
            return View();
        }
        [HttpPost]
        public JsonResult MngDetailPostPre(FormCollection collection)
        {
            AjaxResponseModel response = new AjaxResponseModel();

            OrderT order = null;
            string DeliveryCompany = collection["DeliveryCompany"];
            string DeliveryNum = collection["DeliveryNum"];
            int orderNo = System.Convert.ToInt32(collection["OrderNo"]);
            int orderMemNo = System.Convert.ToInt32(collection["OrderMemNo"]);
            string type = collection[("Type")];
            if ((DeliveryCompany != "" && DeliveryNum != "") || type == "픽업")
            {

                // notice
                if (type == "택배")
                {
                    PrinterNoticeT prtNotice = new PrinterNoticeT();
                    prtNotice.OrderNo = orderNo;
                    prtNotice.MemberNo = Profile.UserNo;
                    prtNotice.MemberNoRef = orderMemNo;
                    prtNotice.Comment = "고객님의 주문이 택배사(" + DeliveryCompany + ") 으로 발송 되었으며 송장번호는 " + DeliveryNum + " 입니다.";
                    prtNotice.Type = "posted";
                    prtNotice.IsNew = "Y";
                    prtNotice.RegDt = DateTime.Now;
                    prtNotice.RegId = Profile.UserId;
                    prtNotice.RegIp = IPAddressHelper.GetIP(this);
                    _PrinterNoticeDac.InsertNotice(prtNotice);
                }

                // change status
                order = _orderDac.GetOrderByNoPrt(orderNo, Profile.UserNo);
                int orderStat = 0;
                if (type == "택배")
                {
                    foreach (MakersnEnumTypes.OrderState enumtemp in Enum.GetValues(typeof(MakersnEnumTypes.OrderState)))
                    {
                        if (enumtemp == MakersnEnumTypes.OrderState.배송중)
                            orderStat = (int)enumtemp;
                    }
                }
                else if (type == "픽업")
                {
                    foreach (MakersnEnumTypes.OrderState enumtemp in Enum.GetValues(typeof(MakersnEnumTypes.OrderState)))
                    {
                        if (enumtemp == MakersnEnumTypes.OrderState.배송완료)
                            orderStat = (int)enumtemp;
                    }
                }
                if (orderStat != 0)
                {
                    order.DeliveryCompany = DeliveryCompany;
                    order.DeliveryNum = DeliveryNum;
                    order.OrderStatus = orderStat;
                    order.UpdDt = System.DateTime.Now;
                    order.PostDt = System.DateTime.Now;
                    order.UpdId = Profile.UserId;
                    _orderDac.UpdateOrder(order);
                }

                //end 
                response.Success = true;

            }
            response.Result = orderNo + "";
            return Json(response);
        }

        [HttpGet]
        public ActionResult MngDetailPostDone(long orderNo)
        {
            OrderT order = _orderDac.GetOrderByNoPrt(orderNo, Profile.UserNo);
            ViewBag.OrderEntity = order;
            ViewBag.OrderMember = _memberDac.GetMemberProfile(order.MemberNo);
            ViewBag.Printer = _printerDac.GetPrinterByNo(order.PrinterNo);
            ViewBag.PrinterMember = _printerMemberDac.GetPrinterMemberByNo(order.PrinterMemberNo);
            return View();
        }




        #endregion
        #region
        public ActionResult MngDetailInfo(long orderNo)
        {
            OrderT order = _orderDac.GetOrderByNoPrt(orderNo, Profile.UserNo);
            order.PrinterName = _printerDac.GetPrinterByNo(order.PrinterNo).Brand + _printerDac.GetPrinterByNo(order.PrinterNo).Model;
            ViewBag.OrderEntity = order;
            ViewBag.OrderMember = _memberDac.GetMemberProfile(order.MemberNo);
            return View();
        }
        #endregion

        #region 사진업로드
        public JsonResult ImgUpload(FormCollection collection)
        {

            bool Success = false;
            string Message = string.Empty;
            string ReName = string.Empty;
            string Name = string.Empty;
            string Size = string.Empty;

            HttpPostedFileBase imgupload = Request.Files["imgupload"];

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
        #endregion

        #region 새주문 관리
        public ActionResult MngDetailNew(int orderNo = 0, int status = 0)
        {
            OrderT order = _orderDac.GetOrderByNoPrt(orderNo, Profile.UserNo);
            if (order.OrderStatus <= (int)MakersnEnumTypes.OrderState.결제완료)
            {
                ViewBag.Order = order;
                ViewBag.OrderMember = _memberDac.GetMemberProfile(order.MemberNo);
                ViewBag.Printer = _printerDac.GetPrinterByNo(order.PrinterNo);
            }
            return View();

        }

        #endregion


        public JsonResult ChangeStat(int orderNo, string status)
        {
            OrderT order = _orderDac.GetOrderByNoPrt(orderNo, Profile.UserNo);
            int orderStat = 0;
            foreach (MakersnEnumTypes.OrderState enumtemp in Enum.GetValues(typeof(MakersnEnumTypes.OrderState)))
            {
                if (enumtemp.ToString() == status)
                    orderStat = (int)enumtemp;
            }
            if (orderStat != 0)
            {
                order.OrderStatus = orderStat;
                order.UpdDt = System.DateTime.Now;
                order.UpdId = Profile.UserId;
                _orderDac.UpdateOrder(order);


                if (orderStat == (int)MakersnEnumTypes.OrderState.요청거부)
                {

                    PrinterNoticeT prtNotice = new PrinterNoticeT();
                    prtNotice.OrderNo = order.No;
                    prtNotice.MemberNo = Profile.UserNo;
                    prtNotice.MemberNoRef = order.MemberNo;
                    //prtNotice.Comment = Profile.UserNm + " 님 께서 새로운 주문이 들어왔습니다.";
                    prtNotice.Type = "orderReqReject";
                    prtNotice.IsNew = "Y";
                    prtNotice.RegDt = DateTime.Now;
                    prtNotice.RegId = Profile.UserId;
                    prtNotice.RegIp = IPAddressHelper.GetIP(this);
                    _PrinterNoticeDac.InsertNotice(prtNotice);
                }
                else if (orderStat == (int)MakersnEnumTypes.OrderState.출력중)
                {

                    PrinterNoticeT prtNotice = new PrinterNoticeT();
                    prtNotice.OrderNo = order.No;
                    prtNotice.MemberNo = Profile.UserNo;
                    prtNotice.MemberNoRef = order.MemberNo;
                    //prtNotice.Comment = Profile.UserNm + " 님 께서 새로운 주문이 들어왔습니다.";
                    prtNotice.Type = "orderReqAccept";
                    prtNotice.IsNew = "Y";
                    prtNotice.RegDt = DateTime.Now;
                    prtNotice.RegId = Profile.UserId;
                    prtNotice.RegIp = IPAddressHelper.GetIP(this);
                    _PrinterNoticeDac.InsertNotice(prtNotice);
                }
            }
            return null;
        }


        public PartialViewResult GetMaterialListInFont(int printerNo)
        {
            IList<PrinterMaterialT> list = _printerDac.GetPrinterMatrialListByPrinterNo(printerNo);
            return PartialView(list);
        }

        public PartialViewResult GetColorListInFront(int PrinterMetrialNo)
        {
            IList<PrinterColorT> list = _printerDac.GetPrinterMaterialColorByPrinterMaterialNo(PrinterMetrialNo);
            return PartialView(list);
        }



        #region 주문 프론트 페이지

        public ActionResult OrderFront(string no)
        {

            return View();
        }


        #endregion 주문 프론트 페이지


        #region 내 주문
        public ActionResult MyOrderDetail(int orderNo = 1)
        {
            OrderT order = _orderDac.GetOrderByNoForOrderDetail(orderNo, Profile.UserNo);
            ViewBag.Order = order;
            IList<OrderDetailT> list = _orderDac.GetOrderDetailByMyOrder(order.No);
            ViewBag.PrinterList = _printerDac.GetPrinterByPrtMemberNo(order.PrinterMemberNo);
            //orderDac.GetNameListFromMyOrderDetail(orderNo);
            IList<MaterialT> matList = _printerDac.GetMatrialListByPrinterNo(order.PrinterNo);
            ViewBag.MaterialList = matList;
            ViewBag.ColorList = _printerDac.GetMaterialColorByPrinterNo(order.PrinterNo, matList[0].No);
            ViewBag.UserNm = Profile.UserNm;

            return View(list);
        }
        #endregion

        public PartialViewResult GetMaterialList(int printerNo)
        {
            IList<MaterialT> list = _printerDac.GetMatrialListByPrinterNo(printerNo);
            return PartialView(list);
        }

        //public PartialViewResult GetColorList(int printerNo, int matNo)
        public PartialViewResult GetColorList(int printerMatNo)
        {
            IList<PrinterColorT> list = _printerDac.GetMaterialColorByPrinterMatNo(printerMatNo);
            return PartialView(list);
        }


        public ActionResult MyOrder(int page = 1)
        {
            ViewBag.WrapClass = "bgW";
            int memberNo = Profile.UserNo;
            IList<OrderT> allList = _orderDac.GetMyOrderList(memberNo);

            IList<OrderT> list = new List<OrderT>();
            foreach (OrderT order in allList)
            {
                if (order.OrderStatus <= (int)MakersnEnumTypes.OrderState.결제완료 && order.RegDt.AddHours(12) < DateTime.Now)
                {
                    _orderDetailDac.DeleteOrderDetailByOrder(order);
                    _orderDac.DeleteOrder(order);
                }
                else
                {
                    list.Add(order);
                }
            }

            return View(list.OrderByDescending(o => o.OrderDt).ToPagedList(page, 10));
        }

        public ActionResult MyOrderReview(int orderNo)
        {
            OrderT order = _orderDac.GetOrderByNo(orderNo, Profile.UserNo);
            if (order == null)
            {
                return Content("<script>alert('잘못된 접근입니다.'); location.href='/order/myorder';</script>");
            }

            if (order.OrderStatus != (int)Makersn.Util.MakersnEnumTypes.OrderState.배송완료)
            {
                return Content("<script>alert('잘못된 접근입니다.'); location.href='/order/myorder';</script>");
            }

            //IList<OrderDetailT> list = _orderDac.GetOrderdetailByQuery(orderNo);
            IList<OrderDetailT> list = _orderDac.GetOrderDetailByMyOrder(orderNo);

            ViewBag.Order = order;
            ViewBag.Printer = _printerDac.GetPrinterByNo(order.PrinterNo);
            ViewBag.Review = _reviewDac.GetReviewByOrderNo(orderNo, Profile.UserNo);
            ViewBag.SpotName = _memberDac.GetMemberProfile(order.PrinterMemberNo).Name;

            return View(list);
        }

        public JsonResult SetReview(string text, int score, int no, int odNo)
        {
            AjaxResponseModel model = new AjaxResponseModel();
            model.Success = false;
            OrderT order = _orderDac.GetOrderByNo(odNo, Profile.UserNo);

            if (order.PrinterMemberNo == Profile.UserNo)
            {
                model.Message = "본인 스팟에는 평점을 남길 수 없습니다.";
                order.OrderStatus = (int)Makersn.Util.MakersnEnumTypes.OrderState.거래완료;
                order.UpdDt = DateTime.Now;
                order.UpdId = Profile.UserId;
                _orderDac.UpdateOrderbyDone(order);
                return Json(model);
            }

            if (order == null)
            {
                model.Message = "잘못된 접근입니다";
                return Json(model);
            }
            try
            {
                IList<OrderDetailT> detail = _orderDac.GetOrderDetailByMyOrder(order.No);

                ReviewT review = new ReviewT();
                review.OrderNo = odNo;
                review.MemberNo = Profile.UserNo;
                review.PrinterNo = no;
                review.Score = score;
                review.Comment = text;
                review.RegId = Profile.UserId;
                review.RegDt = DateTime.Now;

                _reviewDac.addReview(review);

                order.OrderStatus = (int)Makersn.Util.MakersnEnumTypes.OrderState.거래완료;
                order.OrderDoneDt = DateTime.Now;
                order.UpdDt = DateTime.Now;
                order.UpdId = Profile.UserId;

                order.TotalPrice = 0;
                foreach (OrderDetailT od in detail)
                {
                    order.TotalPrice += od.UnitPrice * od.OrderCount;
                }


                //_orderDac.UpdateOrderbyDone(order);

                //OrderAccountingT oa = new OrderAccountingT();

                //oa.OrderNo = order.No;
                //oa.PrinterNo = order.PrinterNo;
                //oa.PrinterMemberNo = order.PrinterMemberNo;
                //oa.Price = order.TotalPrice;
                //oa.Status = (int)Makersn.Util.MakersnEnumTypes.OrderAccountingStatus.미결제;
                //oa.RegDt = DateTime.Now;
                //oa.RegId = Profile.UserId;

                //_orderAccountingDac.InsertOrderAccounting(oa);

                model.Success = true;
            }
            catch
            {
                model.Message = "저장에 실패하였습니다.";
            }

            return Json(model);
        }

        #region 파일 업로드
        public PartialViewResult StlUpload(FormCollection collection)
        {

            string fileName = string.Empty;

            HttpPostedFileBase stlupload = Request.Files["stlupload"];
            string temp = collection["temp"];
            OrderDetailT orderDetail = new OrderDetailT();

            int uiIndex = int.Parse(collection["uiIndex"]);
            ViewBag.uiIndex = uiIndex;

            ViewBag.prtMatNo = collection["prtMatNo"];


            if (stlupload != null)
            {
                if (stlupload.ContentLength > 0)
                {
                    if (stlupload.ContentLength < 100 * 1024 * 1024)
                    {
                        string[] extType = { "stl", "obj" };

                        string extension = Path.GetExtension(stlupload.FileName).ToLower().Replace(".", "").ToLower();

                        if (extType.Contains(extension))
                        {
                            string save3DFolder = "Article/article_3d";
                            string saveJSFolder = "Article/article_js";
                            fileName = FileUpload.UploadFile(stlupload, null, save3DFolder, null);

                            string file3Dpath = string.Format("{0}/FileUpload/{1}/", AppDomain.CurrentDomain.BaseDirectory, save3DFolder);
                            string fileJSpath = string.Format("{0}/FileUpload/{1}/", AppDomain.CurrentDomain.BaseDirectory, saveJSFolder);

                            StlModel stlModel = new STLHelper().GetStlModel(file3Dpath + fileName, extension);

                            StlSize sizeResult = new STLHelper().GetSizeFor3DFile(file3Dpath + fileName, extension);

                            orderDetail.PrintVolume = new STLHelper().slicing(file3Dpath + fileName);

                            var json = JsonConvert.SerializeObject(stlModel);


                            if (!Directory.Exists(fileJSpath))
                            {
                                Directory.CreateDirectory(fileJSpath);
                            }

                            string jsFileNm = fileJSpath + fileName + ".js";

                            System.IO.File.WriteAllText(jsFileNm, json, Encoding.UTF8);


                            //ImgCapture(Base64Helper.Base64Encode(json), fileName + ".jpg"); //캡쳐 안됨

                            orderDetail.FileName = stlupload.FileName;
                            orderDetail.FileType = extension;
                            orderDetail.FileReName = fileName;
                            orderDetail.FileImgRename = fileName + ".jpg";
                            orderDetail.Temp = temp;
                            orderDetail.SizeX = sizeResult.X;
                            orderDetail.SizeY = sizeResult.Y;
                            orderDetail.SizeZ = sizeResult.Z;
                            orderDetail.Volume = sizeResult.Volume;
                            orderDetail.RegDt = DateTime.Now;
                            orderDetail.RegId = Profile.UserId;

                            orderDetail.No = _orderDetailDac.InsertOrderDetail(orderDetail);


                        }
                        else
                        {
                            //message = "stl, obj 형식 파일만 가능합니다.";
                        }
                    }
                    else
                    {
                        //message = "최대 사이즈 100MB 파일만 가능합니다.";
                    }
                }
            }

            return PartialView(orderDetail);
        }
        #endregion

        #region 이미지 캡쳐
        /// <summary>
        /// capture
        /// </summary>
        /// <param name="stl_val"></param>
        /// <param name="stl_img_no"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImgCapture(string stl_val, int stl_img_no, int img_idx)
        {
            string result = "";
            int index = img_idx;

            string saveImgFolder = "Article/article_img";
            string fileImgpath = string.Format("{0}/FileUpload/{1}/", AppDomain.CurrentDomain.BaseDirectory, saveImgFolder);

            if (!Directory.Exists(fileImgpath))
            {
                Directory.CreateDirectory(fileImgpath);
            }

            if (!string.IsNullOrEmpty(stl_val) && stl_img_no != 0)
            {
                //ArticleFileT file = _articleFileDac.GetArticleFileByNo(stl_img_no);
                OrderDetailT file = _orderDetailDac.GetOrderDetailByNo(stl_img_no);

                string fileNm = file.FileImgRename;

                using (FileStream fs = new FileStream(fileImgpath + fileNm, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        byte[] data = Convert.FromBase64String(stl_val.Replace("data:image/png;base64,", ""));
                        bw.Write(data);
                        bw.Close();
                    }
                    fs.Close();
                }

                //file.ImgName = fileNm;
                //response.Success = _articleFileDac.UpdateArticleFile(file);
                result = file.FileImgRename;
            }

            return Json(new { Success = true, Result = result, Index = index });
        }
        #endregion


        #region 파일 삭제
        public JsonResult DeleteFile(int no)
        {
            OrderDetailT detail = new OrderDetailT();
            detail.No = no;
            detail.Temp = Profile.UserNo.ToString();

            _orderDetailDac.DeleteFile(detail);

            return Json(new { });
        }
        #endregion

        #region 프린터 검색
        public PartialViewResult SpotList(int page = 1, string location = "", float locX = 0, float locY = 0, int material = 0, int quality = 0)
        {
            IList<PrinterMemberT> list = _printerDac.GetSpotListInOrder(location, locX, locY, quality, material);
            if (list.Count == 0)
            {
                list = _printerDac.GetSpotListInOrder("", 0, 0, 0, 0).OrderByDescending(o=>o.ReviewScore).Take(5).ToList<PrinterMemberT>();
                ViewBag.NoSearch = "";
            }
            ViewBag.Location = location;
            ViewBag.LocX = locX;
            ViewBag.LocY = locY;
            ViewBag.Mat = material;
            ViewBag.Qual = quality;

            ViewBag.MatList = _materialDac.getMaterialList();

            return PartialView(list.ToPagedList(page, 5));
        }
        #endregion

        #region
        public PartialViewResult GetSpotPrinter(int no = 0, int color = 0, int material = 0, int printerNo = 0)
        {
            PrinterMemberT result = new PrinterMemberT();
            if (no != 0)
            {
                result = _printerDac.GetPrinterListByMemberNo(no);
            }
            if (printerNo != 0)
            {
                result = _printerDac.GetPrinterListByPrinterNo(printerNo);
            }
            ViewBag.ColorNo = color;
            ViewBag.MatNo = material;
            ViewBag.PrinterNo = printerNo;

            return PartialView(result);
        }
        #endregion

        #region
        public JsonResult AddOrder(FormCollection collection)
        {
            AjaxResponseModel response = new AjaxResponseModel();

            if (collection["prtNo"] == "")
            {
                response.Success = false;
                response.Message = "프린터를 선택해 주세요.";
                return Json(response);
            };

            if (collection["hfMatNo"] == "")
            {
                response.Success = false;
                response.Message = "재료를 선택해 주세요.";
                return Json(response);
            }

            if (collection["postMode"] == "")
            {
                response.Success = false;
                response.Message = "수령방법을 선택해 주세요.";
                return Json(response);
            }

            if (collection["no"] == null)
            {
                response.Success = false;
                response.Message = "파일을 등록해 주세요.";
                return Json(response);
            }

            int printerNo = int.Parse(collection["prtNo"]);
            string temp = collection["temp"];
            string[] detailNo = collection["no"].Split(',');
            string[] orderCount = collection["orderCount"].Split(',');
            string[] colorNo = collection["color"].Split(',');
            int postMode = int.Parse(collection["postMode"] == "" ? "0" : collection["postMode"]);
            int matNo = int.Parse(collection["hfMatNo"]);

            if (detailNo.Length != colorNo.Length || colorNo.Contains(""))
            {
                response.Success = false;
                response.Message = "색상을 선택해 주세요.";
                return Json(response);
            }

            if (int.Parse(collection["hfTotalPrice"].Replace(",", "")) < 3000)
            {
                response.Success = false;
                response.Message = "주문 최소 금액은 3천원 이상입니다.";
                return Json(response);
            }

            if (detailNo.Length == 0)
            {
                response.Success = false;
                response.Message = "파일이 등록되지 않았습니다\n파일을 등록하셨다면 새로고침을 해주세요.";
                return Json(response);
            }

            OrderT chkOrder = _orderDac.GetOrderByTemp(temp, Profile.UserNo);
            if (chkOrder != null)
            {
                response.Success = false;
                response.Message = "잘못된 접근입니다.\n 새로고침을 해주세요";
                return Json(response);
            }

            //string orderNo = Base64Helper.Base64Encode(temp);

            PrinterMaterialT mat = _printerMaterialDac.GetPrinterMaterialByNo(matNo);


            PrinterT printer = _printerDac.GetPrinterByNo(printerNo);
            PrinterMemberT pmem = _printerMemberDac.GetPrinterMemberByNo(printer.MemberNo);


            string orderNo = new DateTimeHelper().ConvertToUnixTime(DateTime.Now).ToString() + (printer.No + printer.MemberNo).ToString();

            OrderT order = new OrderT();
            order.OrderNo = orderNo;
            order.MemberNo = Profile.UserNo;
            order.PrinterMemberNo = printer.MemberNo;
            order.PrinterNo = printerNo;
            order.Quality = printer.Quality;
            order.OrderDt = DateTime.Now;
            order.OrderStatus = 100;
            order.Temp = temp;
            order.TestFlag = "N";
            order.PostMode = postMode;
            order.RegId = Profile.UserId;
            order.RegDt = DateTime.Now;
            order.AccountState = (int)MakersnEnumTypes.OrderAccountingStatus.미결제;

            if (postMode == (int)Makersn.Util.MakersnEnumTypes.PostType.택배 && pmem.PostType == (int)Makersn.Util.MakersnEnumTypes.PrinterPostType.고정배송비)
            {
                order.PostPrice = pmem.PostPrice;
            }
            else
            {
                order.PostPrice = 0;
            }

            order.No = _orderDac.InsertOrderReqeust(order);
            response.Result = order.No.ToString();

            for (int i = 0; i < detailNo.Length; i++)
            {
                PrinterColorT color = _printerDac.GetSinglePrinterColorByColorNo(int.Parse(colorNo[i]));
                OrderDetailT detail = new OrderDetailT();
                detail.No = int.Parse(detailNo[i]);
                detail.ColorNo = color.ColorNo;
                detail.OrderCount = int.Parse(orderCount[i]);
                detail.OrderNo = (int)order.No;
                //detail.MaterialNo = matNo;
                detail.MaterialNo = mat.MaterialNo;
                detail.UnitPrice = color.UnitPrice;
                _orderDetailDac.UpdateOrderDetailByOrderReqeust(detail);
            }

            int preOrderNo = int.Parse(collection["PreOrderNo"]);
            if (preOrderNo != 0)
            {
                OrderT preOrder = _orderDac.GetOrderByNo(preOrderNo, Profile.UserNo);
                _orderDac.DeleteOrder(preOrder);
            }

            response.Success = true;

            return Json(response);
        }
        #endregion

        #region 결제 페이지
        public ActionResult Payment(int orderNo)
        {
            ViewBag.WrapClass = "bgW";

            OrderT order = _orderDac.GetOrderByNo(orderNo, Profile.UserNo);
            if (order == null || (int)Makersn.Util.MakersnEnumTypes.OrderState.결제완료 <= order.OrderStatus)
            {
                return Content("<script>alert('잘못된 접근입니다.'); location.href='/order/myorder';</script>");
            }
            MemberT member = _memberDac.GetMemberProfile(Profile.UserNo);
            ViewBag.Member = member;
            order.OrderMemberName = member.Name;
            order.Email = member.Email;
            //order.CellPhone = member 휴대전화 입력란 없음

            ViewBag.Order = order;
            IList<OrderDetailT> list = _orderDac.GetOrderDetailByMyOrder(order.No);

            ViewBag.DefaultAddress = _orderDac.GetDefaultAddress(Profile.UserNo);
            ViewBag.Printer = _printerDac.GetPrinterByNo(order.PrinterNo);
            ViewBag.Spot = _printerMemberDac.GetPrinterMemberByNo(order.PrinterMemberNo);

            return View(list);
        }
        #endregion

        #region 배송지정보
        public PartialViewResult PostInfo(string type)
        {
            DefaultAddressT address = new DefaultAddressT();

            OrderT order = _orderDac.GetLatelyOrderTop1ByMemberNo(Profile.UserNo);

            switch (type)
            {
                case "D":
                    address = _orderDac.GetDefaultAddress(Profile.UserNo);
                    address = address == null ? new DefaultAddressT() : address;
                    break;
                case "B":
                    if (order != null)
                    {
                        address.PostMemberName = order.PostMemberName;
                        address.CellPhone = order.CellPhone;
                        address.AddPhone = order.AddPhone;
                        address.PostNum = order.PostNum;
                        address.Address = order.PostAddress;
                        address.AddressDetail = order.PostAddressDetail;
                    }
                    else
                    {
                        address = new DefaultAddressT();
                    }
                    break;
            }

            return PartialView(address);
        }
        #endregion

        #region 주문 상세
        public ActionResult OrderDetailPop(int orderNo = 1)
        {
            OrderT order = _orderDac.GetOrderByNo(orderNo, Profile.UserNo);

            if (order == null) { return Content("<script>alert('잘못된 접근입니다.'); location.href='/order/myorder';</script>"); }

            ViewBag.Order = order;
            IList<OrderDetailT> list = _orderDac.GetOrderDetailByMyOrder(order.No);

            order.OrderMemberName = _memberDac.GetMemberProfile(order.MemberNo).Name;
            order.PrinterMemberName = _memberDac.GetMemberProfile(order.PrinterMemberNo).Name;

            for (int i = 0; i < list.Count; i++)
            {
                list[i].MaterialName = _materialDac.getMaterialNameByNo(list[i].MaterialNo);
            }

            ViewBag.Printer = _printerDac.GetPrinterByNo(order.PrinterNo);

            ViewBag.UserNm = Profile.UserNm;

            return View(list);
        }
        #endregion

        #region
        public ActionResult ShowOutputImage(int orderNo = 0)
        {
            ViewBag.WrapClass = "bgW";
            OrderT chkOrder = _orderDac.GetOrderByNo(orderNo, Profile.UserNo);
            if (chkOrder == null) { return Content("<script>alert('잘못된 접근입니다'); location.href = '/order/myorder';</script>"); }
            IList<PrinterOutputImageT> list = _orderDac.GetOutputImage(orderNo);
            ViewBag.Comment = _PrinterNoticeDac.GetNoticeByOrderNoAndType(orderNo, "outputImage")[0].Comment;
            ViewBag.PrtMemberNo = chkOrder.PrinterMemberNo.ToString();
            ViewBag.PostMode = chkOrder.PostMode;
            return View(list);
        }
        #endregion


        #region 배송 요청하기
        public JsonResult PostRequest(int no)
        {
            AjaxResponseModel model = new AjaxResponseModel();
            model.Success = false;

            OrderT order = _orderDac.GetOrderByNo(no, Profile.UserNo);

            if (order == null)
            {
                model.Message = "잘못된 경로로 접근하셨습니다.";
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            order.MemberNo = Profile.UserNo;
            order.OrderStatus = (int)(Makersn.Util.MakersnEnumTypes.OrderState.배송요청);
            order.UpdDt = DateTime.Now;
            order.UpdId = Profile.UserId;

            _orderDac.UpdateOrder(order);

            //bool result = _orderDac.UpdateOrderState(order);
            if (order.PostMode > 1)
            {
                //gooksong

                // notice
                PrinterNoticeT prtNotice = new PrinterNoticeT();
                prtNotice.OrderNo = order.No;
                prtNotice.MemberNo = Profile.UserNo;
                prtNotice.MemberNoRef = order.PrinterMemberNo;
                //prtNotice.Comment = Profile.UserNm + " 님 께서 새로운 주문이 들어왔습니다.";
                prtNotice.Type = "postReq";
                prtNotice.IsNew = "Y";
                prtNotice.RegDt = DateTime.Now;
                prtNotice.RegId = Profile.UserId;
                prtNotice.RegIp = IPAddressHelper.GetIP(this);
                _PrinterNoticeDac.InsertNotice(prtNotice);


                model.Message = "배송 요청을 하였습니다.";
            }
            else
            {

                PrinterNoticeT prtNotice = new PrinterNoticeT();
                prtNotice.OrderNo = order.No;
                prtNotice.MemberNo = Profile.UserNo;
                prtNotice.MemberNoRef = order.PrinterMemberNo;
                //prtNotice.Comment = Profile.UserNm + " 님 께서 새로운 주문이 들어왔습니다.";
                prtNotice.Type = "pickupReq";
                prtNotice.IsNew = "Y";
                prtNotice.RegDt = DateTime.Now;
                prtNotice.RegId = Profile.UserId;
                prtNotice.RegIp = IPAddressHelper.GetIP(this);
                _PrinterNoticeDac.InsertNotice(prtNotice);
                model.Message = "방문 요청을 하였습니다. \n스팟에 문의하여 방문 약속을 잡으세요.";
            }


            model.Success = true;

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 주문 취소
        public JsonResult CancelOrder(int no)
        {
            AjaxResponseModel model = new AjaxResponseModel();
            model.Success = false;

            OrderT order = _orderDac.GetOrderByNo(no, Profile.UserNo);
            if (!(order.OrderStatus == 100 || order.OrderStatus == 110 || order.OrderStatus == 120) || order == null)
            {
                model.Message = "잘못된 접근입니다.";
                return Json(model);
            }
            _orderDetailDac.DeleteOrderDetailByOrder(order);
            _orderDac.DeleteOrder(order);
            model.Message = "주문이 취소되었습니다.";
            model.Success = true;
            return Json(model);
        }
        #endregion

        #region 결제하기 클릭
        public JsonResult MakePayment(FormCollection collection)
        {
            AjaxResponseModel model = new AjaxResponseModel();
            model.Success = false;

            try
            {
                int orderNo = int.Parse(collection["hfOrderNo"]);
                int postMode = int.Parse(collection["hfPostMode"]);


                int bankNum = int.Parse(collection["bankNum"]);
                string accountNo = collection["accountNo"];
                string accountName = collection["accountName"];
                string taxFlag = collection["taxYn"];
                int payType = int.Parse(collection["payType"]);
                int taxType = 0;
                string taxNum = string.Empty;

                if (taxFlag == "Y")
                {
                    taxType = int.Parse(collection["taxType"]);
                    switch (taxType)
                    {
                        case 1:
                            string[] numText = collection["cashReceiptsPhone"].Split(',');
                            taxNum = numText[0] + numText[1] + numText[2];
                            break;

                        case 2:
                            numText = collection["cashReceiptsPersonNum"].Split(',');
                            taxNum = numText[0] + numText[1];
                            break;
                        case 3:
                            numText = collection["cashReceiptsCard"].Split(',');
                            taxNum = numText[0] + numText[1] + numText[2] + numText[3];
                            break;
                        case 4:
                            numText = collection["companyNum"].Split(',');
                            taxNum = numText[0] + numText[1] + numText[2];
                            break;
                    }
                }

                string ordName = collection["ordName"];
                string ordEmail = collection["ordEmail"];
                string[] ordHp = collection["ordHp"].Split(',');

                if (!chkEmail(ordEmail))
                {
                    model.Message = "올바른 이메일 주소를 입력해 주세요";
                    return Json(model);
                }

                MemberT member = new MemberT();
                member.No = Profile.UserNo;
                member.CellPhone = ordHp[0] + "-" + ordHp[1] + "-" + ordHp[2]; ;
                member.Email = ordEmail;
                member.UpdDt = DateTime.Now;
                member.UpdId = Profile.UserId;

                _memberDac.UpdateMemberByOrder(member);

                OrderT order = _orderDac.GetOrderByNo(orderNo, Profile.UserNo);
                if (order == null)
                {
                    model.Message = "잘못된 경로입니다;";
                    return Json(model);
                }

                string[] sendHp = new string[3];
                string[] sendHp2 = new string[3];
                string sendLoc = string.Empty;
                string sendName = string.Empty;
                string sendPostNum = string.Empty;
                string sendAddr = string.Empty;
                string sendAddrDetail = string.Empty;
                string cartCheck = string.Empty;
                string sendNoti = string.Empty;
                string ordPay = string.Empty;

                if (postMode == 1)//픽업인 경우
                {
                    order.CellPhone = member.CellPhone;
                    order.PostMemberName = ordName;
                }
                else
                {
                    sendHp = collection["sendHp"].Split(',');
                    sendHp2 = collection["sendHp2"] == null ? " - - ".Split(',') : collection["sendHp2"].Split(',');
                    sendLoc = collection["sendLoc"]; // 배송지 정보 타입 기본,새로운,과거
                    sendName = collection["sendName"];
                    sendPostNum = collection["sendPost"];
                    sendAddr = collection["sendAddr"];
                    sendAddrDetail = collection["sendAddr2"];
                    cartCheck = collection["cartCheck"]; //기본배송지 등록

                    order.PostMemberName = sendName;

                    order.CellPhone = sendHp[0] + "-" + sendHp[1] + "-" + sendHp[2];
                    order.AddPhone = sendHp2[0] + "-" + sendHp2[1] + "-" + sendHp2[2];
                }
                sendNoti = collection["sendNoti"]; //배송 유의사항
                ordPay = collection["ordPay"]; //결제수단

                order.PostNum = sendPostNum;
                order.PostAddress = sendAddr;
                order.PostAddressDetail = sendAddrDetail;
                order.RequireComment = sendNoti;

                order.CurrencyFlag = taxFlag;
                order.CurrencyNum = taxNum;
                order.CurrencyNumType = taxType;
                order.PayType = payType;
                order.PayBank = bankNum;
                order.PayAccountName = accountName;
                order.PayAccountNo = accountNo;

                //order.OrderStatus = (int)Makersn.Util.MakersnEnumTypes.OrderState.결제대기; //실제 운영시 결제 대기임 테스트는 결제완료(운영시 아래 region 삭제)
                #region 운영시 이부분 삭제
                order.OrderStatus = (int)Makersn.Util.MakersnEnumTypes.OrderState.결제완료;
                order.PayDt = DateTime.Now;

                PrinterNoticeT prtNotice = new PrinterNoticeT();
                prtNotice.OrderNo = order.No;
                prtNotice.MemberNo = order.MemberNo;
                prtNotice.MemberNoRef = order.PrinterMemberNo;
                prtNotice.Comment = Profile.UserNm + " 님 께서 새로운 주문이 들어왔습니다.";
                prtNotice.Type = "paymentFinished";
                prtNotice.IsNew = "Y";
                prtNotice.RegDt = DateTime.Now;
                prtNotice.RegId = Profile.UserId;
                prtNotice.RegIp = IPAddressHelper.GetIP(this);
                _PrinterNoticeDac.InsertNotice(prtNotice);

                #endregion

                order.OrderDt = DateTime.Now;
                order.UpdDt = DateTime.Now;
                order.UpdId = Profile.UserId;

                _orderDac.UpdateOrder(order);




                //// notice
                //PrinterNoticeT prtNotice = new PrinterNoticeT();
                //prtNotice.OrderNo = orderNo;
                //prtNotice.MemberNo = Profile.UserNo;
                //prtNotice.MemberNoRef = order.PrinterMemberNo;
                //prtNotice.Comment = Profile.UserNm + " 님 께서 새로운 주문이 들어왔습니다.";
                //prtNotice.Type = "paymentFinished";
                //prtNotice.IsNew = "Y";
                //prtNotice.RegDt = DateTime.Now;
                //prtNotice.RegId = Profile.UserId;
                //prtNotice.RegIp = IPAddressHelper.GetIP(this);
                //_PrinterNoticeDac.InsertNotice(prtNotice);




                if (cartCheck != "" && cartCheck != null)
                {
                    DefaultAddressT address = _orderDac.GetDefaultAddress(Profile.UserNo);
                    if (address == null)
                    {
                        address = new DefaultAddressT();
                        address.MemberNo = Profile.UserNo;
                        address.RegDt = DateTime.Now;
                        address.RegId = Profile.UserId;
                    }
                    address.PostMemberName = order.PostMemberName;
                    address.CellPhone = order.CellPhone;
                    address.AddPhone = order.AddPhone;
                    address.PostNum = order.PostNum;
                    address.Address = order.PostAddress;
                    address.AddressDetail = order.PostAddressDetail;
                    address.UpdDt = DateTime.Now;
                    address.UpdId = Profile.UserId;


                    _orderDac.SaveOrUpdateDefaultAddress(address);
                }
                model.Success = true;
                model.Result = order.No.ToString();
            }
            catch
            {
                model.Message = "주문에 실패하였습니다.";
            }


            return Json(model);
        }
        #endregion

        #region 배송 정보 보기
        public PartialViewResult GetPostInfo(int orderNo)
        {
            OrderT order = _orderDac.GetOrderByNo(orderNo, Profile.UserNo);
            order.PrinterMemberName = _memberDac.GetMemberProfile(order.PrinterMemberNo).Name;

            return PartialView(order);
        }
        #endregion

        #region
        public ActionResult OrderDone(int orderNo)
        {
            OrderT order = _orderDac.GetOrderByNo(orderNo, Profile.UserNo);
            order.PrinterMemberName = _memberDac.GetMemberProfile(order.PrinterMemberNo).Name;
            IList<OrderDetailT> detail = _orderDetailDac.GetDetailListByOrderNo(order.No);

            foreach (OrderDetailT od in detail)
            {
                order.TotalPrice += od.UnitPrice * od.OrderCount;
            }
            order.TotalPrice += order.PostPrice;

            ViewBag.WrapClass = "bgW";
            return View(order);
        }
        #endregion

        #region
        public JsonResult TakePost(int orderNo)
        {
            AjaxResponseModel model = new AjaxResponseModel();
            model.Success = false;

            OrderT order = _orderDac.GetOrderByNo(orderNo, Profile.UserNo);

            if (order == null)
            {
                model.Message = "잘못된 접근입니다.";
            }
            else
            {
                order.OrderStatus = (int)Makersn.Util.MakersnEnumTypes.OrderState.배송완료;
                order.UpdDt = DateTime.Now;
                order.UpdId = Profile.UserId;
                _orderDac.UpdateOrder(order);
                model.Success = true;
            }

            return Json(model);
        }
        #endregion

        #region 이메일 정규식
        private bool chkEmail(string email)
        {
            bool emailCheck = Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            if (emailCheck)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        public ActionResult test()
        {
            System.Data.SqlClient.SqlConnection dbConnection;
            dbConnection = new System.Data.SqlClient.SqlConnection(@"server=MSDN-SPECIAL\SQLEXPRESS;uid=sa;pwd=1234;database=timerTest");
            dbConnection.Open();
            System.Data.SqlClient.SqlCommand cmd;
            cmd = new System.Data.SqlClient.SqlCommand("INSERT INTO TEST(REG_DT) VALUES(GETDATE())", dbConnection);

            cmd.ExecuteNonQuery();
            dbConnection.Close();

            return View();
        }

        #region 파일 다운로드
        public ActionResult FileDownload(string filePath, string fileName)
        {
            string contentType = "application/octet-stream";
            string host = Request.Url.Host;
            string Scheme = Request.Url.Scheme;

            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(Scheme + "://" + host + filePath);
            request.Method = "HEAD";

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                return File(filePath, contentType, fileName);
            }
            catch
            {
                return Content("<script type='text/javascript'> alert('존재하지 않는 파일입니다'); history.go(-1);</script>");
            }
        }
        #endregion

    }


}
