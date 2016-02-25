using Net.Common.Filter;
using Net.Common.Helper;
using Net.Common.Model;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Makers.Store.Controllers
{
    public class StoreController : BaseController
    {
        StoreMemberBiz storeMembBiz = new StoreMemberBiz();

        /// <summary>
        /// 스토어 프로필 페이지
        /// </summary>
        /// <param name="q">스토어멤머 no</param>
        /// <param name="url">짧은 주소</param>
        /// <param name="gbn">구분값 => 상품 리스트, 임시저장, 좋아요, 팔로잉, 팔로워 등등</param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(string q = "", string url = "", string gbn = "U", int page = 1)
        {
            if (q == "" && profileModel.UserNo == 0 && url == "") return Redirect("/");

            int sMembNo = 0;
            if (string.IsNullOrEmpty(q)) {
                int sMemberNo = storeMembBiz.GetStoreMemberNoByMemberNo(profileModel.UserNo);
                q = Base64Helper.Base64Encode(sMemberNo.ToString()); 
            }
            sMembNo = int.Parse(Base64Helper.Base64Decode(q));

            StoreMemberExT storeMember = new StoreMemberExT();
            if (!string.IsNullOrEmpty(url))
            {
                //짧은 주소
            }
            else
            {
                storeMember = storeMembBiz.GetFullStoreMemberByMemberNo(sMembNo);
            }

            if (storeMember != null)
            {
                if (storeMember.DelYn == "Y") return Redirect("/");

                if (!string.IsNullOrEmpty(storeMember.StoreProfileMsg))
                {
                    storeMember.StoreProfileMsg = HtmlFilter.PunctuationEncode(storeMember.StoreProfileMsg);
                    storeMember.StoreProfileMsg = HtmlFilter.ConvertContent(storeMember.StoreProfileMsg);
                }
            }

            //int visitorNo = profileModel.UserNo;

            return View(storeMember);
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

            int smRet = storeMembBiz.AddStoreMember(storeMember);
            MemberExT member = new MemberExDac().GetMemberByNo(profileModel.UserNo);
            member.Level = 60;
            if (smRet > 0)
            {
                response.Success = true;
                response.Result = smRet.ToString();
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 스토어 정보 수정
        /// </summary>
        /// <returns></returns>
        public ActionResult Setting()
        {
            return View();
        }

        #region 스토어 회원 삭제

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

            //int memberUserno = profileModel.UserNo == 0 ? 182 : profileModel.UserNo; // 로그인 되어 있는 회원아이디
            //IList<FollowerT> _follerList = new FollowerDac().GetFollowerLIst(storeNo, memberUserno).Where(s => s.SiteGubun == "Store").ToList<FollowerT>();

            //if (_follerList.Count > 0)
            //{
            //    ViewBag.follerList = _follerList;
            //}

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

            //FollowerT follow = new FollowerT();

            //follow.SiteGubun = "Store";
            //follow.MemberNoRef = StoreMemberNo;
            //follow.MemberNo = profileModel.UserNo;
            //follow.IsNew = "Y";
            //follow.RegId = "admin";
            //follow.RegDt = DateTime.Now;

            //if (profileModel.UserNo == 0)
            //{

            //}
            //else
            //{
            //    result = new FollowerDac().SetFollow(follow);
            //}

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

            //FollowerT follow = new FollowerT();

            //follow.SiteGubun = "Store";
            //follow.MemberNoRef = StoreMemberNo;
            //follow.MemberNo = profileModel.UserNo;
            //follow.IsNew = "Y";
            //follow.RegId = "admin";
            //follow.RegDt = DateTime.Now;

            //if (profileModel.UserNo == 0)
            //{

            //}
            //else
            //{
            //    result = new FollowerDac().CheckFollow(profileModel.UserNo, StoreMemberNo);
            //}
            return result;
        }

        #endregion
    }
}
