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
                _notice.PRODUCT_NO = ProductNo;
                _notice.MEMBER_NO = MemberNo;
                _notice.MEMBER_NO_REF = MemberNoRef;
                _notice.REF_NO = RefNo;
                _notice.TYPE = Type;
                _notice.CONTENTS = Content;
                _notice.CHECK_YN = CheckYN;
                _notice.IS_NEW_YN = IsNewYN;
                _notice.REG_DT = DateTime.Now;
                _notice.REG_ID = profileModel.UserId;

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

            _notice = _noticeBiz.SelectAllStoreNotice().Where(m => m.CHECK_YN == "N").ToList<Net.Framework.StoreModel.StoreNotificationsT>();

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
                _notice.IS_NEW_YN = "N";
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
                _notice.CHECK_YN = "Y";
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
