using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Makersn.BizDac;
using Design.Web.Admin.Models;
using Makersn.Models;

namespace Design.Web.Admin.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        private MenuModel menuModel = new MenuModel();

        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Admin";
            menuModel.MainIndex = 6;
            menuModel.SubIndex = subIndex;

            return menuModel;
        }

        public ActionResult Admin()
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            return View();
        }

        public ActionResult Admin_Auth()
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(0);

            IList<MemberT> mem = new AdminDac().GetAdminList();
            return View(mem);
        }

        [HttpPost]
        public ActionResult Admin_Add_Popup(int no, string gubun)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            //ViewData["Group"] = Group();
            MemberT mem = new MemberT();
            if (no != 0)
            {
                mem = new AdminDac().GetAdminBySeq(no);
            }
            ViewData["gubun"] = "e"; //edit인지 insert인지
            if (gubun == "i") { ViewData["gubun"] = "i"; };
            return View(mem);
        }

        public JsonResult Admin_Edit(int no, string id, string name, string pw, string email, string phone)
        {
            MemberT mem = new MemberT();
            mem.No = no;
            mem.Id = id;
            mem.Name = name;
            mem.Password = pw;
            mem.Email = email;
            new AdminDac().EditAdmin(mem);
            return Json(new AjaxResponseModel { Success = true, Message = "수정되었습니다." });
        }

        public JsonResult Member_Delete(int no)
        {
            MemberT mem = new MemberT();
            mem.No = no;
            new AdminDac().DeleteMember(mem);
            return Json(new AjaxResponseModel { Success = true, Message = "삭제되었습니다." });
        }

        public JsonResult Admin_Add(string id, string name, string pw, string email, string phone)
        {
            MemberT mem = new MemberT();
            mem.Id = id;
            mem.Name = name;
            mem.Password = pw;
            mem.Level = 50;
            mem.Status = "1";
            mem.Email = email;
            mem.SnsType = "EM";
            mem.AllIs = "ON";
            mem.RepleIs = "ON";
            mem.LikeIs = "ON";
            mem.NoticeIs = "N";
            mem.RegDt = DateTime.Now;
            mem.RegId = id;
            mem.DelFlag = "N";

            int result = new AdminDac().InsertAdmin(mem);
            return Json(new AjaxResponseModel { Success = true, Result = result.ToString(), Message = "생성되었습니다." });
        }

        public JsonResult Check_Id(string id)
        {
            //int result = new AdminDac().CheckId(id);
            //if(result > 0){
            //    return Json(new AjaxResponseModel { Success = true, Result = "0", Message = "중복되는 아이디 입니다." });           
            //}
            //else{
            //    return Json(new AjaxResponseModel { Success = true, Result = "1", Message = "사용 가능한 아이디 입니다." });           
            //}
            var result = new AdminDac().CheckId(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
