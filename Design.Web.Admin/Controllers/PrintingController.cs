using Design.Web.Admin.Helper;
using Design.Web.Admin.Models;
using Makersn.BizDac;
using Makersn.Models;
using Makersn.Util;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using PagedList;
using Makersn.Util;
using System.Linq;

namespace Design.Web.Admin.Controllers
{
    [Authorize]
    public class PrintingController : BaseController
    {

        ArticleDac articleDac = new ArticleDac();
        PrinterDac _printerDac = new PrinterDac();
        MemberDac memberDac = new MemberDac();
        private MenuModel menuModel = new MenuModel();
        PrinterModelDac _printerModelDac = new PrinterModelDac();
        PrinterOurputImgDac _printerOuputimgDac = new PrinterOurputImgDac();
        OrderDac _orderDac = new OrderDac();
        MessageDac _messageDac = new MessageDac();
        PrinterNoticeDac _PrinterNoticeDac = new PrinterNoticeDac();
        OrderAccountingDac _orderAccountingDac = new OrderAccountingDac();
        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Printing";
            menuModel.MainIndex = 3;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Listing(int page = 1, string dateGubun = "", string start = "", string end = "", string txtGubun = "", string text = "", int state = 0)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            IList<PrinterStateT> list = _printerDac.GetPrintingListForAdmin(dateGubun, start, end, txtGubun, text, state);
            ViewData["Group"] = MenuModel(1);

            if (start == "") { start = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"); }
            if (end == "") { end = DateTime.Now.ToString("yyyy-MM-dd"); }

            ViewBag.Start = start;
            ViewBag.End = end;
            ViewData["text"] = text;

            Dictionary<int, string> stateList = new Dictionary<int, string>();
            stateList.Add(0, "전체");

            foreach (var a in EnumHelper.GetItemValueList<MakersnEnumTypes.OrderState>())
            {
                stateList.Add(a.Key, a.Value);
            }

            ViewData.Add("state", new SelectList(stateList, "Key", "Value", state));

            return View(list.ToPagedList(page, 20));
        }
        public ActionResult State(string start = "", string end = "")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            if (start == "") { start = DateTime.Now.ToString("yyyy-MM-dd"); }
            if (end == "") { end = DateTime.Now.ToString("yyyy-MM-dd"); }
            IList<PrinterMemberStateT> returnLst = memberDac.SearchPrinterMemberStateTargetAll(start, end);

            ViewBag.Start = start;
            ViewBag.End = end;

            return View(returnLst);
        }
        public ActionResult DailyState(string year = null, string month = null)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            DateTime makeDt = DateTime.Now;
            if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month))
            {
                makeDt = DateTime.Now;
            }
            else
            {
                makeDt = Convert.ToDateTime(string.Format("{0}-{1}", year, month));
            }

            int days = Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(makeDt.Year, makeDt.Month);

            DateTime firstDay = new DateTimeHelper().FirstDayOfMonth(makeDt);
            DateTime lastDay = new DateTimeHelper().LastDayOfMonth(makeDt);
            //IList<MemberStateT> returnlst = new List<MemberStateT>();
            //IList<MemberStateT> statelst = memberDac.SearchMemberStateTargetDaily(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());
            IList<PrinterMemberStateT> returnlst = new List<PrinterMemberStateT>();
            IList<PrinterMemberStateT> statelst = memberDac.SearchPrinterMemberStateTargetDaily(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());

            for (int day = 1; day <= days; day++)
            {
                string stringDt = firstDay.AddDays(day - 1).ToShortDateString();

                PrinterMemberStateT retState = statelst.SingleOrDefault(g => Convert.ToDateTime(g.Gbn).ToShortDateString() == stringDt);
                if (retState != null)
                {
                    returnlst.Add(retState);
                }
                else
                {
                    returnlst.Add(new PrinterMemberStateT());
                }
            }

            Dictionary<string, string> monthlst = new Dictionary<string, string>();

            foreach (int item in Enum.GetValues(typeof(MakersnEnumTypes.MonthLst)))
            {
                monthlst.Add(item + "월", Convert.ToString(item));
            }

            ViewData["Month"] = new SelectList(monthlst, "Value", "Key", makeDt.Month);

            //연도 셀렉트 아이템
            IList<object> yearGroup = memberDac.GetMemberYearGroup();

            Dictionary<string, string> YearList = new Dictionary<string, string>();

            foreach (int item in yearGroup)
            {
                YearList.Add(item + "년", Convert.ToString(item));
            }

            ViewData["year"] = new SelectList(YearList, "Value", "Key", year);

            return View(returnlst);
        }

        public ActionResult MonthState(string year = null)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            DateTime makeDt = DateTime.Now;
            if (string.IsNullOrEmpty(year))
            {
                makeDt = Convert.ToDateTime(DateTime.Now.Year + "-01");
            }
            else
            {
                makeDt = Convert.ToDateTime(year + "-01");
            }

            DateTime firstDay = new DateTimeHelper().FirstDayOfYear(makeDt);
            DateTime lastDay = new DateTimeHelper().LastDayOfYear(makeDt);

            //IList<MemberStateT> returnlst = new List<MemberStateT>();
            //IList<MemberStateT> statelst = memberDac.SearchMemberStateTargetMonth(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());
            IList<PrinterMemberStateT> returnlst = new List<PrinterMemberStateT>();
            IList<PrinterMemberStateT> statelst = memberDac.SearchPrinterMemberStateTargetMonth(firstDay.ToShortDateString(), lastDay.AddDays(1).ToShortDateString());

            foreach (int item in Enum.GetValues(typeof(MakersnEnumTypes.MonthLst)))
            {
                PrinterMemberStateT retState = statelst.SingleOrDefault(g => Convert.ToInt32(g.Gbn) == item);
                if (retState != null)
                {
                    returnlst.Add(retState);
                }
                else
                {
                    returnlst.Add(new PrinterMemberStateT());
                }
            }

            //연도 셀렉트 아이템
            IList<object> yearGroup = memberDac.GetMemberYearGroup();

            Dictionary<string, string> YearList = new Dictionary<string, string>();

            foreach (int item in yearGroup)
            {
                YearList.Add(item + "년", Convert.ToString(item));
            }

            ViewData["year"] = new SelectList(YearList, "Value", "Key", year);

            return View(returnlst);
        }

        public ActionResult YearState(string year)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);
            //IList<MemberStateT> statelst = memberDac.SearchMemberStateTargetYear();
            IList<PrinterMemberStateT> statelst = memberDac.SearchPrinterMemberStateTargetYear();
            return View(statelst);
        }

        public ActionResult AddPrinterModel(int page = 1, string orderby = "", string desc = "A")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(2);
            ViewBag.WaitList = _printerModelDac.GetPendingOrApprovedPrinterModel("N");
            IList<PrinterModelT> list = _printerModelDac.GetPendingOrApprovedPrinterModel("Y");
            ViewBag.Count = list.Count;

            ViewBag.OrderBy = orderby;

            switch (orderby)
            {
                case "B":
                    if (desc == "D")
                    {
                        ViewBag.Desc = "A";
                        return View(list.OrderByDescending(o => o.Brand).ThenBy(t => t.Model).ThenBy(t => t.RegDt).ToPagedList(page, 20));
                    }
                    else
                    {
                        ViewBag.Desc = "D";
                        return View(list.OrderBy(o => o.Brand).ThenBy(t => t.Model).ThenBy(t => t.RegDt).ToPagedList(page, 20));
                    }
                case "M":
                    if (desc == "D")
                    {
                        ViewBag.Desc = "A";
                        return View(list.OrderByDescending(o => o.PropMemberNo).ThenBy(t => t.RegDt).ToPagedList(page, 20));
                    }
                    else
                    {
                        ViewBag.Desc = "D";
                        return View(list.OrderBy(o => o.PropMemberNo).ThenBy(t => t.RegDt).ToPagedList(page, 20));
                    }
                case "D":
                    if (desc == "D")
                    {
                        ViewBag.Desc = "A";
                        return View(list.OrderByDescending(o => o.RegDt).ToPagedList(page, 20));
                    }
                    else
                    {
                        ViewBag.Desc = "D";
                        return View(list.OrderBy(o => o.RegDt).ToPagedList(page, 20));
                    }
            }

            return View(list.ToPagedList(page, 20));
        }

        public JsonResult ApprPrinterModel(int no, string flag)
        {
            AjaxResponseModel response = new AjaxResponseModel();

            response.Success = _printerModelDac.ApprPrinterModel(no, flag);

            return Json(response);
        }

        public JsonResult DeletePrinterModel(int no)
        {
            AjaxResponseModel response = new AjaxResponseModel();

            response.Success = _printerModelDac.DeletePrinterModel(no);

            return Json(response);
        }

        public ActionResult ApprPrinterTest(int page = 1)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(3);

            IList<PrinterT> list = _printerDac.GetTestReqeustPrinter("N");

            return View(list.ToPagedList(page, 20));
        }

        public JsonResult SetApprPrinterTest(int no, string flag)
        {
            AjaxResponseModel model = new AjaxResponseModel();
            model.Success = false;

            if (Profile.UserLevel < 50)
            {
                model.Message = "관리자가 아닙니다";
                return Json(model);
            }

            try
            {
                model.Success = _printerDac.ApprPrinterTest(no, flag);
                model.Message = "승인하였습니다.";
            }
            catch
            {
                model.Message = "실패하였습니다.";
            }
            return Json(model);
        }

        public ActionResult EditModel(int no)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(2);
            PrinterModelT model = _printerModelDac.GetPrinterModelByNo(no);
            return View(model);
        }

        public ActionResult setEditModel(string brand, string model, int no)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            if (Profile.UserLevel < 50)
            {
                return Content("<script>alert('잘못된 경로입니다'); location.href='/';</script>");
            }

            PrinterModelT printerModel = _printerModelDac.GetPrinterModelByNo(no);
            printerModel.Brand = brand;
            printerModel.Model = model;
            printerModel.UpdDt = DateTime.Now;
            printerModel.UpdId = Profile.UserId;

            _printerModelDac.UpdatePrinterModel(printerModel);

            return Redirect("/printing/addprintermodel");
        }

        public ActionResult ShowImage(int no)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(3);

            IList<PrinterOutputImageT> list = _printerOuputimgDac.GetImglistByOrderNo(no);
            ViewBag.PrinterNo = _orderDac.GetOrderByAdmin(list[0].OrderNo).PrinterNo;
            return View(list);
        }

        public JsonResult DeletePrinterTest(int no, string comment)
        {
            AjaxResponseModel response = new AjaxResponseModel();

            OrderT order = _orderDac.GetOrderByAdmin(no);

            MessageT message = new MessageT();
            message.MemberNo = Profile.UserNo;
            message.MemberNoRef = order.PrinterMemberNo;
            message.Comment = comment;
            message.RoomName = message.MemberNo + "_" + message.MemberNoRef;
            message.IsNew = "Y";
            message.DelFlag = "N";
            message.RegId = Profile.UserId;
            message.RegDt = DateTime.Now;
            message.RegIp = IPAddressHelper.GetClientIP();

            message.MsgGubun = "MSG";

            _messageDac.AddMessage(message);


            IList<PrinterOutputImageT> imgList = _printerOuputimgDac.GetImglistByOrderNo(order.No);

            bool chk = _printerOuputimgDac.DeleteOutputImageByList(imgList);

            if (chk)
            {
                _orderDac.DeleteOrder(order);
                response.Success = true;
            }

            return Json(response);
        }

        public ActionResult MngApprPrinterTest(int page = 1, string orderby = "", string desc = "A")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(4);

            IList<PrinterT> list = _printerDac.GetTestReqeustPrinter("Y");

            switch (orderby)
            {
                case "B":
                    if (desc == "D")
                    {
                        ViewBag.Desc = "A";
                        return View(list.OrderByDescending(o => o.Brand).ThenBy(t => t.Model).ThenBy(t => t.RegDt).ToPagedList(page, 20));
                    }
                    else
                    {
                        ViewBag.Desc = "D";
                        return View(list.OrderBy(o => o.Brand).ThenBy(t => t.Model).ThenBy(t => t.RegDt).ToPagedList(page, 20));
                    }
                case "M":
                    if (desc == "D")
                    {
                        ViewBag.Desc = "A";
                        return View(list.OrderByDescending(o => o.SpotName).ThenBy(t => t.RegDt).ToPagedList(page, 20));
                    }
                    else
                    {
                        ViewBag.Desc = "D";
                        return View(list.OrderBy(o => o.SpotName).ThenBy(t => t.RegDt).ToPagedList(page, 20));
                    }
                case "D":
                    if (desc == "D")
                    {
                        ViewBag.Desc = "A";
                        return View(list.OrderByDescending(o => o.RegDt).ToPagedList(page, 20));
                    }
                    else
                    {
                        ViewBag.Desc = "D";
                        return View(list.OrderBy(o => o.RegDt).ToPagedList(page, 20));
                    }
                case "S":
                    if (desc == "D")
                    {
                        ViewBag.Desc = "A";
                        return View(list.OrderByDescending(o => o.Score).ToPagedList(page, 20));
                    }
                    else
                    {
                        ViewBag.Desc = "D";
                        return View(list.OrderBy(o => o.Score).ToPagedList(page, 20));
                    }
            }

            return View(list.ToPagedList(page, 20));
        }

        public ActionResult ApprPayment(int page = 1)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(5);

            IList<OrderT> list = _orderDac.GetOrderListForApprPayment((int)MakersnEnumTypes.OrderState.결제대기);

            IList<OrderT> ApprList = _orderDac.GetOrderListForApprovedPayment();

            ViewBag.Cnt = list.Count;
            ViewBag.ApprList = ApprList;

            return View(list.ToPagedList(page, 20));
        }



        public JsonResult ApprOrderStatus(int no)
        {
            AjaxResponseModel model = new AjaxResponseModel();

            if (Profile.UserLevel < 50)
            {
                model.Message = "잘못된 경로로 접근하셨습니다.";
                return Json(model);
            }

            OrderT order = _orderDac.GetOrderByAdmin(no);

            if (order == null)
            {
                model.Message = "존재하지 않는 주문입니다.";
                return Json(model);
            }

            order.OrderStatus = (int)Makersn.Util.MakersnEnumTypes.OrderState.결제완료;
            order.PayDt = DateTime.Now;
            order.UpdDt = DateTime.Now;
            order.UpdId = Profile.UserId;

            _orderDac.UpdateOrder(order);

            // notice
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


            model.Message = "승인처리 되었습니다.";

            return Json(model);
        }

        public JsonResult CancelApprOrderStatus( int no)
        {
            AjaxResponseModel model = new AjaxResponseModel();

            if (Profile.UserLevel < 50)
            {
                model.Message = "잘못된 경로로 접근하셨습니다.";
                return Json(model);
            }

            OrderT order = _orderDac.GetOrderByAdmin(no);

            if (order == null)
            {
                model.Message = "존재하지 않는 주문입니다.";
                return Json(model);
            }

            order.OrderStatus = (int)Makersn.Util.MakersnEnumTypes.OrderState.결제대기;
            order.PayDt = null;
            order.UpdDt = null;
            order.UpdId = Profile.UserId;
            _orderDac.UpdateOrder(order);
            _PrinterNoticeDac.RemoveWithOrderNo(order.No);

            model.Message = "승인 취소 처리 되었습니다.";

            return Json(model);
        }




        public ActionResult ApprPaymentCancle(int page = 1)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            //gooksong

            ViewData["Group"] = MenuModel(6);
            DateTime nowDt = DateTime.Now;
            DateTime endDt = nowDt.AddHours(-12);

            IList<OrderT> list = _orderDac.GetSearchOrderListPrt(null, (int)MakersnEnumTypes.OrderState.결제완료, (int)MakersnEnumTypes.OrderState.결제완료, "", "0", "", endDt.ToString("yyyy-MM-dd HH:mm:ss"), "");

            IList<OrderT> ApprList = _orderDac.GetSearchOrderListPrt(null, (int)MakersnEnumTypes.OrderState.환불완료, (int)MakersnEnumTypes.OrderState.환불완료, "", "0", "", "", "");

            ViewBag.Cnt = list.Count;
            ViewBag.ApprList = ApprList;

            return View(list.ToPagedList(page, 20));
        }
        public JsonResult ChangeOrderStatus(int no, int status) {


            AjaxResponseModel model = new AjaxResponseModel();

            if (Profile.UserLevel < 50)
            {
                model.Message = "잘못된 경로로 접근하셨습니다.";
                return Json(model);
            }

            OrderT order = _orderDac.GetOrderByAdmin(no);

            if (order == null)
            {
                model.Message = "존재하지 않는 주문입니다.";
                return Json(model);
            }

            order.OrderStatus = status;
            //order.PayDt = null;
            order.UpdDt = DateTime.Now;
            order.UpdId = Profile.UserId;
            _orderDac.UpdateOrder(order);

            //model.Message = "승인 취소 처리 되었습니다.";

            return Json(model);
        }

        //gooksong
        public ActionResult Accounting(int page = 1)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(7);
            IList<OrderAccountingStateT> ListUndo= _orderAccountingDac.GetOrderAccountingList((int)MakersnEnumTypes.OrderAccountingStatus.미결제,"");
            ViewBag.ListFinished = _orderAccountingDac.GetOrderAccountingList((int)MakersnEnumTypes.OrderAccountingStatus.결제완료,"desc");

            return View(ListUndo.ToPagedList(page, 20));
        }
        public JsonResult Adjustment(int year, int month, int printerMemberNo, int paidPrice, int postPrice)
        {
            AjaxResponseModel model = new AjaxResponseModel();

            if (Profile.UserLevel < 50)
            {
                model.Message = "잘못된 경로로 접근하셨습니다.";
                return Json(model);
            }

            string id = Profile.UserId;



            _orderAccountingDac.Adjustment(year, month, printerMemberNo, paidPrice,postPrice, id);
            


            return Json(model);
        }

    }
}
