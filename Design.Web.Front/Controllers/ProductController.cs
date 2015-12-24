using Design.Web.Front.Helper;
using Design.Web.Front.Models;
using Library.ObjParser;
using Makersn.BizDac;
using Makersn.Models;
using Makersn.Util;
using Net.Common.Filter;
using Net.Common.Helper;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Design.Web.Front.Controllers
{
	public class ProductController : BaseController
	{
		StoreLikesBiz likesBiz = new StoreLikesBiz();
		StoreMemberBiz memberBiz = new StoreMemberBiz();
		//
		// GET: /Product/
		public ActionResult Index()
		{
			ViewBag.UserNo = Profile.UserNo;
			return View();
		}

		//
		// 좋아요 CREATE / DELETE (상품번호)
		public JsonResult SetLikes(int productNo = 2)
		{
			int result = 0;

			StoreLikesT like = new StoreLikesT();
			like.MemberNo = Profile.UserNo;
			like.RegIp = IPAddressHelper.GetIP(this);
			like.ProductNo = productNo;
			like.RegId = "test";
			like.RegDt = DateTime.Now;

			result = likesBiz.set(like);

			return Json(new { Success = true, Result = result }, JsonRequestBehavior.AllowGet);
		}

		//
		// 상품별 좋아요 갯수 출력 (상품번호)
		public JsonResult CountLikesByProductNo(int productNo = 1)
		{
			int result = likesBiz.countLikesByProductNo(productNo);

			return Json(new { Success = true, Result = result }, JsonRequestBehavior.AllowGet);
		}

		//
		// 내가 좋아요 체크한 상품 리스트 (회원번호)
		public ActionResult GetLikedProductsByMemberNo(int memberNo = 202)
		{
			var result = likesBiz.getLikedProductsByMemberNo(memberNo);
			return PartialView(result);
			//return Json(new { Success = true, Result = result }, JsonRequestBehavior.AllowGet);
		}

		//
		public ActionResult GetReceivedNoteListByMemberNo(int memberNo)
		{
			var result = memberBiz.getReceivedNoteListByMemberNo(memberNo);
			return PartialView(result);
		}

		public ActionResult GetSentNoteListByMemberNo(int memberNo)
		{
			var result = memberBiz.getSentNoteListByMemberNo(memberNo);
			return PartialView(result);
		}

		public JsonResult SendNote (int fromMember, int targetMember, string comment)
		{

			MemberMsgT msg = new MemberMsgT();

			msg.MemberNo = fromMember;
			msg.MemberNoRef = targetMember;
			msg.Comment = comment;
			msg.MsgGubun = "NOTE";
			msg.RegDt = DateTime.Now;
			msg.RegId = "test";
			msg.RegIp = IPAddressHelper.GetIP(this);
			msg.IsNew = "Y";
			msg.SiteGubun= "Store";
			msg.DelFlag = "N";

			int result = memberBiz.sendNote(msg);
			return Json(new { Success = true, Result = result }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult DeleteNote(int SeqNo)
		{
			int result = memberBiz.deleteNote(SeqNo);
			return Json(new { Success = true, Result = result }, JsonRequestBehavior.AllowGet);
		}
	}
}
