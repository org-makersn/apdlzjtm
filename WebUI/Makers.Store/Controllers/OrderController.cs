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
    public class OrderController : BaseController
    {
        //
        // GET: /Order/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LIst(FormCollection forms)
        {

            StoreCartT cartT = new StoreCartT();
            StoreOrderT orderT = new StoreOrderT();
            
            cartT.CartNo = forms["cartNo"];

            // 주문정보 가져오기
            

            return View(cartT);
        }

        #region GetOrderList - 주문정보 Get
        /// <summary>
        /// GetOrderList - 주문정보 Get
        /// </summary>
        /// <param name="cartNo"></param>
        /// <returns></returns>
        public List<StoreOrderT> GetOrderList(string cartNo)
        {
            List<StoreOrderT> storeOrderList = new List<StoreOrderT>();



            return storeOrderList;
        }
        #endregion

        #region IniSecurePay - 거래요청
        /// <summary>
        /// IniSecurePay - 거래요청
        /// </summary>
        /// <returns></returns>
        public ActionResult IniSecurePay()
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
        #endregion

        #region PaymentRequest - 결제요청
        /// <summary>
        /// PaymentResponse - 결제요청
        /// </summary>
        /// <returns></returns>
        public ActionResult PaymentRequest()
        {
            return View();
        }
        #endregion

        #region PaymentResponse - 결제결과
        /// <summary>
        /// PaymentResponse - 결제결과
        /// </summary>
        /// <returns></returns>
        public ActionResult PaymentResponse()
        {
            return View();
        }
        #endregion
    }
}
