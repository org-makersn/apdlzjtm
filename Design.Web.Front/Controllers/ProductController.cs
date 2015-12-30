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


        /// <summary>
        /// 가격 선정
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult ProductPricing() {
            return View();
        }
        public ActionResult ProductPrintable() {
            return View();
        }
        //public ActionResult ProductUpload() {
        //    return View();
        //}


        [Authorize, HttpGet]
        public ActionResult ProductUpload()
        {
            ViewBag.Temp = Profile.UserNo + "_" + new DateTimeHelper().ConvertToUnixTime(DateTime.Now);

            //int articleNo = 0;
            //ArticleT articleT = new ArticleT();
            //if (no != "")
            //{
            //    articleT = articleDac.GetArticleByNo(articleNo);
            //}

            //if (!Int32.TryParse(no, out articleNo))
            //{
            //    //history
            //    return RedirectToAction("Upload");
            //}

            return View();
        }

        /// <summary>
        /// article upload
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
    //    [HttpPost]
    //    public JsonResult ProductUpload(FormCollection collection)
    //    {
    //        AjaxResponseModel response = new AjaxResponseModel();
    //        response.Success = false;

    //        int articleNo = 0;

    //        string paramNo = collection["No"];
    //        string temp = collection["temp"];
    //        string mode = collection["mode"];
    //        int mainImg = Convert.ToInt32(collection["main_img"]);
    //        string articleTitle = collection["article_title"];
    //        string articleContents = collection["article_contents"];
    //        int codeNo = Convert.ToInt32(collection["lv1"]);
    //        int copyright = Convert.ToInt32(collection["copyright"]);
    //        string delNo = collection["del_no"];
    //        string VideoSource = collection["insertVideoSource"];
    //        //string[] splitArray = articleContents.Split('#');
    //        //string tags = "";
    //        string tags = collection["article_tags"];

    //        var mulltiSeq = collection["multi[]"];
    //        string[] seqArray = mulltiSeq.Split(',');


    //        //for (int i = 1; i < splitArray.Length; i++)
    //        //{
    //        //    try
    //        //    {
    //        //        splitArray[i] = splitArray[i].Replace("\r\n", " ");
    //        //        splitArray[i] = splitArray[i].Replace("\n", " ");
    //        //        splitArray[i] = splitArray[i].Replace("\r", " ");
    //        //        tags += splitArray[i].Substring(0, splitArray[i].IndexOf(' '));
    //        //    }
    //        //    catch
    //        //    {
    //        //        tags += splitArray[i];
    //        //    }
    //        //    if (i < splitArray.Length - 1) { tags += ","; };
    //        //}
    //        //articleContents = articleContents.Replace("#", "");
    //        if (tags.Length > 1000)
    //        {
    //            response.Message = "태그는 1000자 이하로 가능합니다.";
    //            return Json(response, JsonRequestBehavior.AllowGet);
    //        }
    //        ArticleT articleT = null;
    //        //TranslationT tran = null;
    //        TranslationDetailT tranDetail = null;
    //        if (!Int32.TryParse(paramNo, out articleNo))
    //        {
    //            response.Success = false;
    //            response.Message = "pk error";
    //        }

    //        if (articleNo > 0)
    //        {
    //            //update
    //            articleT = _articleDac.GetArticleByNo(articleNo);
    //            tranDetail = _translationDetailDac.GetTranslationDetailByArticleNoAndLangFlag(articleT.No, "KR");
    //            if (tranDetail != null)
    //            {
    //                articleT.Title = tranDetail.Title;
    //                articleT.Contents = tranDetail.Contents;
    //                articleT.Tag = tranDetail.Tag;
    //            }

    //            if (articleT != null)
    //            {
    //                if (articleT.MemberNo == Profile.UserNo && articleT.Temp == temp)
    //                {
    //                    articleT.UpdDt = DateTime.Now;
    //                    articleT.UpdId = Profile.UserId;
    //                    articleT.RegIp = IPAddressHelper.GetIP(this);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            //save
    //            articleT = new ArticleT();
    //            articleT.MemberNo = Profile.UserNo;
    //            //태그 #**
    //            articleT.Tag = tags;
    //            articleT.Temp = temp;
    //            articleT.ViewCnt = 0;
    //            articleT.RegDt = DateTime.Now;
    //            articleT.RegId = Profile.UserId;
    //            articleT.RegIp = IPAddressHelper.GetIP(this);
    //            articleT.RecommendYn = "N";
    //            articleT.RecommendDt = null;
    //        }

    //        if (tranDetail == null)
    //        {

    //            //영문텍스트 TRANSLATION_DETAIL
    //            tranDetail = new TranslationDetailT();
    //            tranDetail.LangFlag = "KR";
    //            tranDetail.RegId = Profile.UserId;
    //            tranDetail.RegDt = DateTime.Now;
    //        }

    //        //return Json(response);

    //        if (articleT != null)
    //        {
    //            articleT.Title = articleTitle;
    //            articleT.Contents = articleContents;
    //            articleT.Visibility = mode == "upload" ? "Y" : "N";
    //            articleT.Copyright = copyright;
    //            articleT.CodeNo = codeNo;
    //            articleT.MainImage = mainImg;

    //            articleT.VideoUrl = VideoSource;

    //            articleT.Tag = tags;
    //            articleNo = _articleDac.SaveOrUpdate(articleT, delNo);


    //            //영문텍스트 TRANSLATION_DETAIL
    //            tranDetail.ArticleNo = articleNo;
    //            tranDetail.Title = articleT.Title;
    //            tranDetail.Contents = articleT.Contents;
    //            tranDetail.Tag = articleT.Tag;


    //            _translationDetailDac.SaveOrUpdateTranslationDetail(tranDetail);

    //            response.Success = true;
    //            response.Result = articleNo.ToString();
    //        }

    //        _articleFileDac.UpdateArticleFileSeq(seqArray);


    //        return Json(response, JsonRequestBehavior.AllowGet);
    //    }
    }
}
