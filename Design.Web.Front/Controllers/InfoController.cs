using Makersn.BizDac;
using Makersn.Models;
using Design.Web.Front.Helper;
using Design.Web.Front.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Makersn.Util;

namespace Design.Web.Front.Controllers
{
    public class InfoController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[OutputCache(Duration = 60 * 60, VaryByParam = "none")]
        public ActionResult About()
        {
            ViewBag.InfoList = GetInfoList();
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.InfoList = GetInfoList();
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[OutputCache(Duration = 60 * 60, VaryByParam = "none")]
        public ActionResult License()
        {
            ViewBag.InfoList = GetInfoList();
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[OutputCache(Duration = 60 * 60, VaryByParam = "none")]
        public ActionResult Terms()
        {
            ViewBag.InfoList = GetInfoList();
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[OutputCache(Duration = 60 * 60, VaryByParam = "none")]
        public ActionResult Privacy()
        {
            ViewBag.InfoList = GetInfoList();
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[OutputCache(Duration = 60 * 60, VaryByParam = "none")]
        public ActionResult Blog()
        {
            ViewBag.InfoList = GetInfoList();
            return View();
        }

        public ActionResult Customer()
        {
            IList<CodeModel> code = GetContactList();
            ViewBag.InfoList = GetInfoList();
            return View(code);
        }

        public ActionResult Employment()
        {
            ViewBag.InfoList = GetInfoList();
            return View();
        }

        //[OutputCache(Duration = 60 * 60, VaryByParam = "none")]
        public ActionResult Notice(int page = 1)
        {
            ViewBag.InfoList = GetInfoList();
            NoticesDac noticesDac = new NoticesDac();

            IList<BoardT> list = noticesDac.GetNoticesByContent("", "", "KR");
            return View(list.OrderByDescending(o => o.No).ToPagedList(page, 10));
        }

        public JsonResult SendQnA(AccountModels.QnaModel model)
        {
            if (!chkEmail(model.Email)) { return Json(new { Success = false, Message = "이메일 형식이 틀렸습니다. 다시 입력해 주세요." }); }
            ContactDac contactDac = new ContactDac();
            ContactT contact = new ContactT();
            contact.Email = model.Email;
            contact.Title = model.Title;
            contact.Comment = model.Comment;
            contact.CodeNo = model.QnACode;
            contact.RegDt = DateTime.Now;
            contact.RegId = Profile.UserId;
            contact.MemberNo = Profile.UserNo;
            contact.RegIp = IPAddressHelper.GetClientIP();
            contact.State = 1;
            contact.Reply = "";
            contactDac.InsertQnA(contact);

            sendEmailQnA(Profile.UserId, model.Title, model.Comment, model.Email);

            return Json(new { Success = true });
        }

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

        #region 문의사항 목록
        protected IList<CodeModel> GetContactList()
        {
            var menuLst = EnumHelper.GetEnumDictionary<MakersnEnumTypes.ContactName>();
            IList<CodeModel> list = new List<CodeModel>();
            foreach (var menu in menuLst)
            {
                CodeModel model = new CodeModel();
                model.MenuTitle = menu.Value;
                model.MenuCodeNo = menu.Key;
                list.Add(model);
            }
            return list;
        }
        #endregion

        #region 소개 목록
        protected IList<CodeModel> GetInfoList()
        {
            var menuLst = EnumHelper.GetEnumDictionaryT<MakersnEnumTypes.InfoName>();

            IList<CodeModel> list = new List<CodeModel>();
            foreach (var menu in menuLst)
            {
                CodeModel model = new CodeModel();
                model.MenuTitle = menu.Value;
                model.MenuUrl = "/info/" + menu.Key;
                model.MenuValue = menu.Key;
                list.Add(model);
            }
            return list;
        }
        #endregion

        #region 문의사항 등록시 이메일 발송
        public void sendEmailQnA(string id, string title, string comment, string email)
        {
            string Subject = id + "님으로 부터 문의사항이 접수되었습니다.";

            SendMailModels oMail = new SendMailModels();
            oMail.SendMail("sendQnA", "info@makersi.com", new String[] { Subject, title, comment });
        }
        #endregion

    }
}
