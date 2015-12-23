using System.Collections.Generic;
using System.Web.Mvc;
using Makersn.BizDac;
using Makersn.Models;
using Design.Web.Front.Helper;
using System.Linq;
using PagedList.Mvc;
using PagedList;
using System.Web;
using System.IO;
using Makersn.Util;
using System;

namespace Design.Web.Front.Controllers
{
    public class PrintingProfileController : BaseController
    {
        MemberDac _memberDac = new MemberDac();
        PrinterDac _printerDac = new PrinterDac();
        PrinterMemberDac _printerMemberDac = new PrinterMemberDac();
        ReviewDac _reviewDac = new ReviewDac();
        PrinterFileDac _PrinterFileDac = new PrinterFileDac();


        public ActionResult Index(string memberNo = "0", string url = "")
        {
            bool selfFlag = false;
            int memberNum = 0;
            try
            {
                memberNum = int.Parse(memberNo);
            }
            catch (Exception e)
            {
                memberNum = int.Parse(Base64Helper.Base64Decode(memberNo));
            }


            if (memberNum == 0 || memberNum == Profile.UserNo)
            {
                memberNum = Profile.UserNo;
                selfFlag = true;
            }

            PrinterMemberT member = new PrinterMemberT();
            if (url == "")
            {
                member = _printerMemberDac.GetPrinterMemberByNo(memberNum);
            }
            else
            {
                member = _printerMemberDac.GetPrinterMemberByUrl(url);
                if (member == null) { return Content("<script type='text/javascript'>alert('잘못된 주소입니다.'); location.href='/'</script>"); }
                memberNum = member.MemberNo;
            }

            IList<PrinterT> printerList = _printerDac.GetPrinterByPrtMemberNo(memberNum);

            foreach (PrinterT printer in printerList)
            {
                printer.PrinterFileList = _PrinterFileDac.GetFileList(printer.No);
            }

            member.Name = _memberDac.GetMemberProfile(memberNum).Name;

            ViewBag.PrinterMember = member;
            ViewBag.PrinterList = printerList;
            ViewBag.SelfFlag = selfFlag;

            return View();
        }

        public PartialViewResult ReviewSub(int memberNo, int page = 1)
        {

            PrinterMemberT member = _printerMemberDac.GetPrinterMemberByNoWithReview(memberNo);
            IList<ReviewT> reviewList = _reviewDac.GetReviewByMemberNo(memberNo);

            ViewBag.PrinterMember = member;

            return PartialView(reviewList.OrderByDescending(o => o.RegDt).ToPagedList(page, 20));
        }



        #region 사진업로드
        public JsonResult CoverImgUpload(FormCollection collection)
        {

            bool Success = false;
            string Message = string.Empty;

            int printerMemberNo = 0;
            if (collection["PrtMemNo"] != "")
            {
                printerMemberNo = System.Convert.ToInt32(collection["PrtMemNo"]);
                if (printerMemberNo == Profile.UserNo)
                {
                    Success = true;
                }
                else
                {
                    Message = "잘못된 접근 입니다.";
                }
            }


            HttpPostedFileBase imgupload = Request.Files["cover_pic"];

            if (imgupload != null && Success)
            {
                Success = false;
                if (imgupload.ContentLength > 0)
                {
                    string[] extType = { "jpg", "png", "gif" };

                    string extension = Path.GetExtension(imgupload.FileName).ToLower().Replace(".", "").ToLower();

                    if (extType.Contains(extension))
                    {
                        string ReName = FileUpload.UploadFile(imgupload, new ImageSize().GetCoverResize(), "Printer", null);
                        PrinterMemberT printerMember = _printerMemberDac.GetPrinterMemberByNo(printerMemberNo);
                        printerMember.PrinterCoverPic = ReName;
                        _printerMemberDac.InsertPrinterMember(printerMember);
                        Success = true;
                    }
                    else
                    {
                        Message = "gif, jpg, png 형식 파일만 가능합니다.";
                    }
                }
            }
            return Json(new { Success = Success, Message = Message });
        }
        #endregion
    }
}
