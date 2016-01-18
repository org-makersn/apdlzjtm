﻿using Makersn.BizDac;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Net.Common.Model;
using Newtonsoft.Json;
using Makersn.Models;

namespace Makers.Store.Controllers
{
    public class StoreController : BaseController
    {
        //
        // GET: /Store/

        public ActionResult Index()
        {
            return View();
        }

        #region  스토어등록/삭제/업데이트


        public ActionResult StoreMemberRegister()
        {
            ViewBag.userNo = profileModel.UserNo;
            IList<StoreMemberT> _StoreMemberT = new StoreMemberBiz().getAllMemberList().Where(s => s.DEL_YN.ToLower() == "n").ToList<StoreMemberT>();
            ViewBag.StoreMemberList = _StoreMemberT;

            return View();
        }

        /// <summary>
        /// 스토어 회원입력
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public JsonResult RegisterStoreMember(FormCollection collection)
        {
            AjaxResponseModel response = new AjaxResponseModel();

            StoreMemberBiz _StoreMemberBiz = new StoreMemberBiz();
            StoreMemberT _StoreMemberT = new StoreMemberT();

            _StoreMemberT.MEMBER_NO = 1;
            _StoreMemberT.STORE_NAME = collection["StoreName"];
            _StoreMemberT.OFFICE_PHONE = collection["TelNo"];
            _StoreMemberT.CELL_PHONE = collection["CellPhone"];
            _StoreMemberT.STORE_PROFILE_MSG = collection["StoreProfileMsg"];
            _StoreMemberT.STORE_URL = collection["StoreUrl"];
            _StoreMemberT.BANK_NAME = collection["bankName"];
            _StoreMemberT.BANK_USER_NAME = collection["bankUserName"];
            _StoreMemberT.BANK_ACCOUNT = collection["bankAccount"];
            _StoreMemberT.DEL_YN = "N";
            _StoreMemberT.REG_ID = collection["regId"];
            _StoreMemberT.REG_DT = DateTime.Now;

            _StoreMemberBiz.add(_StoreMemberT);

            response.Success = true;

            response.Result = _StoreMemberT.MEMBER_NO + "";
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 스토어 회원정보 업데이트
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public JsonResult UpdateStoreMember(FormCollection collection)
        {
            AjaxResponseModel response = new AjaxResponseModel();

            StoreMemberBiz _StoreMemberBiz = new StoreMemberBiz();
            StoreMemberT _StoreMemberT = _StoreMemberBiz.getStoreMemberById(Convert.ToInt32(collection["no"]));
            if (_StoreMemberT != null)
            {
                _StoreMemberT.MEMBER_NO = 1;
                _StoreMemberT.STORE_NAME = collection["memberNo"];
                _StoreMemberT.OFFICE_PHONE = collection["TelNo"];
                _StoreMemberT.CELL_PHONE = collection["CellPhone"];
                _StoreMemberT.STORE_PROFILE_MSG = collection["StoreProfileMsg"];
                _StoreMemberT.STORE_URL = collection["StoreUrl"];
                _StoreMemberT.BANK_NAME = collection["bankName"];
                _StoreMemberT.BANK_USER_NAME = collection["bankUserName"];
                _StoreMemberT.BANK_ACCOUNT = collection["bankAccount"];
                //_StoreMemberT.DelYn =  collection["delYn"];
                _StoreMemberT.UPD_ID = profileModel.UserId;
                _StoreMemberT.UPD_DT = DateTime.Now;

                _StoreMemberBiz.upd(_StoreMemberT);
            }
            response.Success = true;

            response.Result = _StoreMemberT.NO + "";
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 스토어 회원 삭제
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public JsonResult DeleteStoreMember(int memberno)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            int result = 0;
            StoreMemberBiz _StoreMemberBiz = new StoreMemberBiz();
            StoreMemberT _StoreMemberT =
            _StoreMemberBiz.getStoreMemberById(memberno);
            if (_StoreMemberT != null)
            {
                _StoreMemberT.DEL_YN = "Y";
                result = _StoreMemberBiz.upd(_StoreMemberT);
            }
            else
            {

            }
            response.Success = true;
            response.Result = _StoreMemberT.MEMBER_NO + "";
            return Json(new { Success = true, Result = result });
        }
        #endregion

        #region 스토어팔러우
        /// <summary>
        /// 주석처리 회원아이디가 없으면 90으로 시작
        /// </summary>
        /// <returns></returns>
        public ActionResult FollewingList(int storeNo)
        {

            int memberUserno = profileModel.UserNo == 0 ? 182 : profileModel.UserNo; // 로그인 되어 있는 회원아이디
            IList<FollowerT> _follerList = new FollowerDac().GetFollowerLIst(storeNo, memberUserno).Where(s => s.SiteGubun == "Store").ToList<FollowerT>();

            if (_follerList.Count > 0)
            {
                ViewBag.follerList = _follerList;
            }

            return View();
        }

        /// <summary>
        /// 팔러우 하기
        /// </summary>
        /// <param name="StoreMemberNo"></param>
        /// <returns></returns>
        public JsonResult FollowingStore(int StoreMemberNo)
        {
            AjaxResponseModel response = new AjaxResponseModel();

            int result = 0;

            FollowerT follow = new FollowerT();

            follow.SiteGubun = "Store";
            follow.MemberNoRef = StoreMemberNo;
            follow.MemberNo = profileModel.UserNo;
            follow.IsNew = "Y";
            follow.RegId = "admin";
            follow.RegDt = DateTime.Now;

            if (profileModel.UserNo == 0)
            {

            }
            else
            {
                result = new FollowerDac().SetFollow(follow);
            }

            response.Result = result + "";
            return Json(new { Success = true, Result = result });
        }

        /// <summary>
        /// 팔러우 했는지 여부 확인
        /// </summary>
        /// <param name="StoreMemberNo"></param>
        /// <returns></returns>
        public int ChkFollower(int StoreMemberNo)
        {
            AjaxResponseModel response = new AjaxResponseModel();

            int result = 0;

            FollowerT follow = new FollowerT();

            follow.SiteGubun = "Store";
            follow.MemberNoRef = StoreMemberNo;
            follow.MemberNo = profileModel.UserNo;
            follow.IsNew = "Y";
            follow.RegId = "admin";
            follow.RegDt = DateTime.Now;

            if (profileModel.UserNo == 0)
            {

            }
            else
            {
                result = new FollowerDac().CheckFollow(profileModel.UserNo, StoreMemberNo);
            }
            return result;
        }

        #endregion


        public string test3()
        {
            Net.Framwork.BizDac.StoreLikesBiz testBiz = new Net.Framwork.BizDac.StoreLikesBiz();
            Net.Framework.StoreModel.StoreLikesT like = new Net.Framework.StoreModel.StoreLikesT();
            //like.MemberNo = 1;
            //like.ProductNo = 2;
            //like.RegDt = DateTime.Now;
            //like.RegId = "TEST";
            //like.RegIp = "192.168.219.121";
            //testBiz.add(like);

            //IList<Net.Framework.StoreModel.StoreLikesT > test = testBiz.getAllStorePrinter();

            like.NO = 2;
            like.PRODUCT_NO = 50500;

            testBiz.upd(like);

            return "dd";
        }
    }
}
