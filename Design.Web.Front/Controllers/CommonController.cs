using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Design.Web.Front.Controllers
{
    public class CommonController : BaseController
    {
        //
        // GET: /Common/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult AddNotice(int ProductNo, int MemberNo, int MemberNoRef, int RefNo, string Type, string Content, string CheckYN, string IsNewYN)
        {
            bool result = false;
            string message = "";

            Net.Framwork.BizDac.StoreNotificationsBiz _noticeBiz = new Net.Framwork.BizDac.StoreNotificationsBiz();
            Net.Framework.StoreModel.StoreNotificationsT _notice = new Net.Framework.StoreModel.StoreNotificationsT();

            try
            {
                _notice.ProductNo = ProductNo;
                _notice.MemberNo = MemberNo;
                _notice.MemberNoRef = MemberNoRef;
                _notice.RefNo = RefNo;
                _notice.Type = Type;
                _notice.Contents = Content;
                _notice.CheckYn = CheckYN;
                _notice.IsNewYn = IsNewYN;
                _notice.RegDt = DateTime.Now;
                _notice.RegId = profileModel.UserId;

                _noticeBiz.add(_notice);

                result = true;
                message = "성공하였습니다.";
            }
            catch {
                result = false;
                message = "실패하였습니다.";
            }
            return Json(new { Success = result, Message = message });
        }

        public ActionResult ListNotice()
        {
            Net.Framwork.BizDac.StoreNotificationsBiz _noticeBiz = new Net.Framwork.BizDac.StoreNotificationsBiz();
            List<Net.Framework.StoreModel.StoreNotificationsT> _notice = new List<Net.Framework.StoreModel.StoreNotificationsT>();

            _notice = _noticeBiz.SelectAllStoreNotice().Where(m => m.CheckYn == "N").ToList<Net.Framework.StoreModel.StoreNotificationsT>();

            return PartialView(_notice);
        }

        public JsonResult ViewNotice(int no)
        {
            bool result = false;
            string message = "";

            Net.Framwork.BizDac.StoreNotificationsBiz _noticeBiz = new Net.Framwork.BizDac.StoreNotificationsBiz();
            Net.Framework.StoreModel.StoreNotificationsT _notice = new Net.Framework.StoreModel.StoreNotificationsT();

            _notice = _noticeBiz.getStoreNoticeById(no);
            if (_notice != null)
            {
                _notice.IsNewYn = "N";
                _noticeBiz.upd(_notice);
                result = true;
                message = "성공하였습니다.";
            }
            else
            {
                result = false;
                message = "실패하였습니다.";
            }

            return Json(new { Success = result, Message = message });
        }

        public JsonResult DeleteListNotice(int no)
        {
            bool result = false;
            string message = "";

            Net.Framwork.BizDac.StoreNotificationsBiz _noticeBiz = new Net.Framwork.BizDac.StoreNotificationsBiz();
            Net.Framework.StoreModel.StoreNotificationsT _notice = new Net.Framework.StoreModel.StoreNotificationsT();

            _notice = _noticeBiz.getStoreNoticeById(no);
            if (_notice != null)
            {
                _notice.CheckYn = "Y";
                _noticeBiz.upd(_notice);
                result = true;
                message = "성공하였습니다.";
            }
            else
            {
                result = false;
                message = "실패하였습니다.";
            }

            return Json(new { Success = result, Message = message });
        }
    }
}
