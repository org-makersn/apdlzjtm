using Design.Web.Front.Models;
using Makersn.BizDac;
using Makersn.Models;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Design.Web.Front.Controllers
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
            ViewBag.userNo = Profile.UserNo;
            IList<StoreMemberT> _StoreMemberT = new StoreMemberBiz().getAllMemberList().Where(s => s.DelYn.ToLower() == "n").ToList<StoreMemberT>();
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

            _StoreMemberT.MemberNo = 1;
            _StoreMemberT.StoreName = collection["StoreName"];
            _StoreMemberT.HomePhone = collection["TelNo"];
            _StoreMemberT.CellPhone = collection["CellPhone"];
            _StoreMemberT.StoreProfileMsg = collection["StoreProfileMsg"];
            _StoreMemberT.StoreUrl = collection["StoreUrl"];
            _StoreMemberT.BankName = collection["bankName"];
            _StoreMemberT.BankUserName = collection["bankUserName"];
            _StoreMemberT.BankAccount = collection["bankAccount"];
            _StoreMemberT.DelYn = "N";
            _StoreMemberT.RegId = collection["regId"];
            _StoreMemberT.RegDt = DateTime.Now;
            _StoreMemberT.UpdId = collection["StoreProfileMsg"];
            _StoreMemberT.UpdDt = DateTime.Now;

            _StoreMemberBiz.add(_StoreMemberT);

            response.Success = true;

            response.Result = _StoreMemberT.MemberNo + "";
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
                _StoreMemberT.MemberNo = 1;
                _StoreMemberT.StoreName = collection["memberNo"];
                _StoreMemberT.HomePhone = collection["TelNo"];
                _StoreMemberT.CellPhone = collection["CellPhone"];
                _StoreMemberT.StoreProfileMsg = collection["StoreProfileMsg"];
                _StoreMemberT.StoreUrl = collection["StoreUrl"];
                _StoreMemberT.BankName = collection["bankName"];
                _StoreMemberT.BankUserName = collection["bankUserName"];
                _StoreMemberT.BankAccount = collection["bankAccount"];
                //_StoreMemberT.DelYn =  collection["delYn"];
                _StoreMemberT.RegId = collection["regId"];
                _StoreMemberT.RegDt = DateTime.Now;
                _StoreMemberT.UpdId = collection["StoreProfileMsg"];
                _StoreMemberT.UpdDt = DateTime.Now;

                _StoreMemberBiz.upd(_StoreMemberT);
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
            int result = 0;
            StoreMemberBiz _StoreMemberBiz = new StoreMemberBiz();
            StoreMemberT _StoreMemberT =
            _StoreMemberBiz.getStoreMemberById(memberno);
            if (_StoreMemberT != null)
            {
                _StoreMemberT.DelYn = "Y";
                result = _StoreMemberBiz.upd(_StoreMemberT);
            }
            else
            {

            }
            response.Success = true;
            response.Result = _StoreMemberT.MemberNo + "";
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

            int memberUserno = Profile.UserNo == 0 ? 182 : Profile.UserNo; // 로그인 되어 있는 회원아이디
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
            follow.MemberNo = Profile.UserNo;
            follow.IsNew = "Y";
            follow.RegId = "admin";
            follow.RegDt = DateTime.Now;

            if (Profile.UserNo == 0)
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
            follow.MemberNo = Profile.UserNo;
            follow.IsNew = "Y";
            follow.RegId = "admin";
            follow.RegDt = DateTime.Now;

            if (Profile.UserNo == 0)
            {

            }
            else
            {
                result = new FollowerDac().CheckFollow(Profile.UserNo, StoreMemberNo);
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

            like.No = 2;
            like.ProductNo = 50500;
            testBiz.upd(like);

            return "dd";
        }


        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult Order()
        {
            //###############################################################################
            //# 1. 객체 생성 #
            //################
            INIPAY50Lib.INItx50 INIpay = new INIPAY50Lib.INItx50();

            //###############################################################################
            //# 2. 인스턴스 초기화 /  3. 체크 유형 설정 #
            //######################
            //
            int intPInst = INIpay.Initialize(string.Empty);

            INIpay.SetActionType(ref intPInst, "chkfake");

            //###############################################################################
            //# 5. 암호화 처리 필드 세팅        #
            //###################################
            INIpay.SetField(ref intPInst, "mid", "INIpayTest");		// 상점 아이디 
            INIpay.SetField(ref intPInst, "admin", "1111");			// 상점 키패스워드
            INIpay.SetField(ref intPInst, "price", "1004");			// 결제 금액
            INIpay.SetField(ref intPInst, "nointerest", "no");		// 무이자 설정 세팅 
            INIpay.SetField(ref intPInst, "quotabase", "lumpsum:00:02:03:04:05:06:07:08:09:10:11:");//할부 개월 및 카드사별 무이자 세팅
            INIpay.SetField(ref intPInst, "currency", "WON");			// 통화단위
            INIpay.SetField(ref intPInst, "debug", "true");			// 로그모드("true"로 설정하면 상세한 로그를 남김)





            //###############################################################################
            //# 5. 체크 처리를 위한 암호화 처리 #
            //###################################
            INIpay.StartAction(ref intPInst);

            //###############################################################################
            //6. 암호화  결과 #
            //###############################################################################
            //resultcode = INIpay.GetResult(ref intPInst, "resultcode");		//결과코드 성공이면 '00' 실패 '01'
            //resultmsg = INIpay.GetResult(ref intPInst, "resultmsg");		//결과메세지 
            //rn_value = INIpay.GetResult(ref intPInst, "rn");				// 암호화 결과값
            //return_enc = INIpay.GetResult(ref intPInst, "return_enc");		// 암호화 결과값
            //ini_certid.Value = INIpay.GetResult(ref intPInst, "ini_certid");		// 암호화 결과값
            //ini_encfield.Value = return_enc;

            //###############################################################################
            //7. RN 값 세션에 저장 #
            //###############################################################################
            //Session["INI_RN"] = rn_value; //	//RN값 => 결제 처리 페이지에서 체크 하기 위해 세션에 저장 (또는 DB에 저장)하여 다음 결제 처리 페이지 에서 체크)
            //Session["INI_PRICE"] = "1004"; //결제 금액 =>  결제 처리 페이지에서 체크 하기 위해 세션에 저장 (또는 DB에 저장)하여 다음 결제 처리 페이지 에서

            //###############################################################################
            //# 8. 인스턴스 해제 #
            //###############################################################################
            //INIpay.Destroy(ref intPInst);


            //###############################################################################
            //# 9. 결제 페이지 생성 성공 유무에 대한 처리  #
            //###############################################################################
            //if (!resultcode.Equals("00"))
            //{
            //    Response.Write("결제 페이지 생성에 문제 발생<BR>");
            //    Response.Write("에러원인 :  " + resultmsg);
            //}

            return View();
        }

        public ActionResult PaymentRequest()
        {
            return View();
        }

        public ActionResult PaymentResponse()
        {
            return View();
        }
    }
}
