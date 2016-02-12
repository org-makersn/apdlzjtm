using Makersn.BizDac;
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
        StoreMemberBiz storeMembBiz = new StoreMemberBiz();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string q)
        {
            return View();
        }

        #region 스토어 개설
        /// <summary>
        /// 스토어 개설
        /// </summary>
        /// <returns></returns>
        [StoreRegist]
        public ActionResult Regist()
        {
            return View();
        }

        /// <summary>
        /// post regist
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PostRegist(FormCollection collection)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;

            string paramStoreName = collection["StoreName"];
            string paramOfficePhone = collection["OfficePhone"];
            string paramCellPhone = collection["CellPhone"];
            string paramStoreProfileMsg = collection["StoreProfileMsg"];
            string paramStoreUrl = collection["StoreUrl"];
            string paramBankName = collection["bankName"];
            string paramBankUserName = collection["bankUserName"];
            string paramBankAccount = collection["bankAccount"];
            
            StoreMemberT storeMember = new StoreMemberT();

            storeMember.MemberNo = profileModel.UserNo;
            storeMember.StoreName = paramStoreName;
            storeMember.OfficePhone = paramOfficePhone;
            storeMember.CellPhone = paramCellPhone;
            storeMember.StoreProfileMsg = paramStoreProfileMsg;
            storeMember.StoreUrl = paramStoreUrl;
            storeMember.BankName = paramBankName;
            storeMember.BankUserName = paramBankUserName;
            storeMember.BankAccount = paramBankAccount;
            storeMember.DelYn = "N";
            storeMember.RegDt = DateTime.Now;
            storeMember.RegId = profileModel.UserId;

            int ret = storeMembBiz.AddStoreMember(storeMember);
            if (ret > 0)
            {
                response.Success = true;
                response.Result = ret.ToString();
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region  스토어등록/삭제/업데이트


        public ActionResult StoreMemberRegister()
        {
            ViewBag.userNo = profileModel.UserNo;
            IList<StoreMemberT> _StoreMemberT = new StoreMemberBiz().GetAllStoreMember().Where(s => s.DelYn.ToLower() == "n").ToList<StoreMemberT>();
            ViewBag.StoreMemberList = _StoreMemberT;

            return View();
        }

        /// <summary>
        /// 스토어 회원정보 업데이트
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public JsonResult UpdateStoreMember(FormCollection collection)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            StoreMemberT _StoreMemberT = storeMembBiz.GetStoreMemberByNo(Convert.ToInt32(collection["no"]));
            if (_StoreMemberT != null)
            {
                _StoreMemberT.MemberNo = 1;
                _StoreMemberT.StoreName = collection["memberNo"];
                _StoreMemberT.OfficePhone = collection["TelNo"];
                _StoreMemberT.CellPhone = collection["CellPhone"];
                _StoreMemberT.StoreProfileMsg = collection["StoreProfileMsg"];
                _StoreMemberT.StoreUrl = collection["StoreUrl"];
                _StoreMemberT.BankName = collection["bankName"];
                _StoreMemberT.BankUserName = collection["bankUserName"];
                _StoreMemberT.BankAccount = collection["bankAccount"];
                //_StoreMemberT.DelYn =  collection["delYn"];
                _StoreMemberT.UpdId = profileModel.UserId;
                _StoreMemberT.UpdDt = DateTime.Now;

                storeMembBiz.UpdateStoreMember(_StoreMemberT);
            }
            response.Success = true;

            response.Result = _StoreMemberT.No + "";
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

            StoreMemberT _StoreMemberT =
            storeMembBiz.GetStoreMemberByNo(memberno);
            if (_StoreMemberT != null)
            {
                _StoreMemberT.DelYn = "Y";
                bool ret = storeMembBiz.UpdateStoreMember(_StoreMemberT);
            }
            else
            {

            }
            response.Success = true;
            response.Result = _StoreMemberT.MemberNo + "";
            return Json(new { Success = true});
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
