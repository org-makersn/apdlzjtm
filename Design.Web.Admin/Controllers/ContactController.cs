using Makersn.Models;
using Makersn.BizDac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Design.Web.Admin.Models;
using Makersn.Util;

namespace Design.Web.Admin.Controllers
{
    [Authorize]
    public class ContactController : BaseController
    {
        ContactDac cd = new ContactDac();

        private MenuModel menuModel = new MenuModel();

        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Management";
            menuModel.MainIndex = 4;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }

        public ActionResult Index(int page = 1, int cate = 0, string gubun = "", string text = "", string state = "")
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(3);

            IList<ContactT> list = null;
            if (text != "" || gubun != "" || cate != 0 || state != "") { list = cd.GetContactListBySearch(cate, gubun, text, state); }
            else { list = cd.GetContactList(); }
            ViewData["cateList"] = cd.GetContactCodeList();
            ViewData["setCate"] = cate;
            ViewData["gubun"] = gubun;
            ViewData["text"] = text;

            Dictionary<int, string> stateList = EnumHelper.GetItemValueList<MakersnEnumTypes.ContactState>();
            ViewData.Add("state", new SelectList(stateList, "Key", "Value", state));

            return View(list.OrderByDescending(o => o.No).ToPagedList(page, 20));
        }

        public ActionResult View(int no)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(3);
            ContactT c = cd.GetContactListByNo(no);
            return View(c);
        }

        public ActionResult Edit(int no)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(3);
            ContactT c = cd.GetContactListByNo(no);



            //Dictionary<int, string> stateList = new Dictionary<int, string>();

            //foreach (int item in Enum.GetValues(typeof(MakersnEnumTypes.ContactState)))
            //{
            //    stateList.Add(item, Convert.ToString(item));
            //}

            //ViewData["State"] = new SelectList(stateList, "Value", "Key", c.State);

            ViewData.Add("State", new SelectList(EnumHelper.GetItemValueList<MakersnEnumTypes.ContactState>(), "Key", "Value", c.State));


            return View(c);
        }

        public JsonResult UpdateContact(int no, string reply, int state)
        {
            ContactT c = new ContactT();
            c.No = no;
            c.Reply = reply;
            c.State = state;
            c.UpdDt = DateTime.Now;
            c.UpdId = "admin";
            cd.UpdateContact(c);
            return Json(new AjaxResponseModel { Success = true, Message = "수정되었습니다." });
        }

        public JsonResult sendAnswer(int no, string answer)
        {
            ContactT contact = cd.GetContactListByNo(no);

            string email = contact.Email;
            string title = contact.Title;
            string comment = contact.Comment;

            string Subject = "MakersN 문의사항 답변 메일입니다.";

            SendMailModels oMail = new SendMailModels();
            oMail.SendMail("QnA", email, new String[] { Subject, title, comment, answer });
            return Json(new AjaxResponseModel { Success = true, Message = "발송되었습니다." });
        }

        public ActionResult Reply(int no)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(3);
            ContactT c = cd.GetContactListByNo(no);

            return View(c);
        }

        public JsonResult SendReply(int no, string reply)
        {
            bool success = false;
            string message = string.Empty;

            if (Profile.UserLevel > 50)
            {
                ContactT c = new ContactT();
                c.No = no;
                c.Reply = reply;
                c.State = (int)Makersn.Util.MakersnEnumTypes.ContactState.답변;
                c.UpdDt = DateTime.Now;
                c.UpdId = "admin";
                ContactT contact = cd.UpdateContact(c);

                if (contact == null)
                {
                    message = "문의글이 없습니다.";
                    return Json(new { Success = success, Message = message });
                }

                string email = contact.Email;
                string title = contact.Title;
                string comment = contact.Comment;

                string Subject = "MakersN 문의사항 답변 메일입니다.";

                SendMailModels oMail = new SendMailModels();
                oMail.SendMail("QnA", email, new String[] { Subject, title, comment, reply });
                success = true;
            }
            else
            {
                message = "관리자가 아닙니다.";
            }

            return Json(new { Success = success, Message = message });
        }

    }
}
