using Makersn.BizDac;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using System.Web.Util;
using System.Text;
using System.Xml;
using System.Collections;
using System.Data;
using Net.Common.Model;
using Net.Framework.Util;
using Newtonsoft.Json;
using Makers.Store.Models;
using Makersn.Models;
using Makers.Store.Configurations;
using log4net;
using log4net.Config;

namespace Makers.Store.Controllers
{
    public class OrderController : BaseController
    {
        //
        // GET: /Order/

        private Hashtable hstCode_PayMethod = new Hashtable();        

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PopPostInfo()
        {
            return View();
        }

        #region List - 주문서 컨트롤러
        /// <summary>
        /// 주문서 컨트롤러 (order/list)
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LIst(FormCollection forms)
        {
            OrderMaster master = new OrderMaster();
            StoreOrderBiz biz = new StoreOrderBiz();
            master.OrderInfoList = new List<OrderInfo>();
            master.StoreCart = new StoreCartT();
            master.Mid = instance.Mid;
            master.Oid = biz.GetNewOrderNo();
            master.Currency = instance.Currency;
            master.BuyerName = profileModel.UserNm;
            master.StoreCart.CART_NO = forms["cartNo"];
            master.OrderInfoList = biz.GetStoreOrderListByMemberNo(profileModel.UserNo); // admin으로 조회     

            int index = 0;
            string firstGoodsName = "";
            foreach (OrderInfo item in master.OrderInfoList)
            {
                if (index == 0)
                {
                    master.TotalPrice += item.PAYMENT_PRICE;
                    firstGoodsName = item.ITEM_NAME;
                }
                else
                {
                    master.TotalPrice += item.PAYMENT_PRICE;
                }
                index++;
            }

            if (index == 1)
            {
                master.GoodsName = firstGoodsName;                
            }
            else
            {
                master.GoodsName = firstGoodsName + " 외 " + (index - 1) + "건";
            }
            master.EncResult = StartChkFake(master.TotalPrice.ToString()); // 페이지 위변조 체크

            return View(master);
        }
        #endregion            

        #region 1. StartChkFake - 위변조 체크
        /// <summary>
        /// StartChkFake - 위변조 체크
        /// </summary>
        /// <returns></returns>
        public EncResult StartChkFake(string payPrice)
        {
            //###############################################################################
            //# 1. 객체 생성 #
            //################
            INIPAY50Lib.INItx50 INIpay = new INIPAY50Lib.INItx50Class();

            //###############################################################################
            //# 2. 인스턴스 초기화 /  3. 체크 유형 설정 #
            //######################
            //
            int intPInst = INIpay.Initialize(string.Empty);

            INIpay.SetActionType(ref intPInst, "chkfake");

            //###############################################################################
            //# 5. 암호화 처리 필드 세팅        #
            //###################################
            INIpay.SetField(ref intPInst, "mid", instance.Mid);		// 상점 아이디 
            INIpay.SetField(ref intPInst, "admin", instance.MidPassword);			// 상점 키패스워드
            INIpay.SetField(ref intPInst, "price", payPrice);			// 결제 금액
            INIpay.SetField(ref intPInst, "nointerest", "no");		// 무이자 설정 세팅 
            INIpay.SetField(ref intPInst, "quotabase", "lumpsum:00:02:03:04:05:06:07:08:09:10:11:");//할부 개월 및 카드사별 무이자 세팅
            INIpay.SetField(ref intPInst, "currency", instance.Currency);			// 통화단위
            INIpay.SetField(ref intPInst, "debug", "true");			// 로그모드("true"로 설정하면 상세한 로그를 남김)

            //###############################################################################
            //# 5. 체크 처리를 위한 암호화 처리 #
            //###################################
            INIpay.StartAction(ref intPInst);

            //###############################################################################
            //6. 암호화  결과 #
            //###############################################################################
            EncResult ER = new EncResult();
            ER.ResultCode = INIpay.GetResult(ref intPInst, "resultcode");		//결과코드 성공이면 '00' 실패 '01'
            ER.ResultMsg = INIpay.GetResult(ref intPInst, "resultmsg");		//결과메세지 
            ER.RnValue = INIpay.GetResult(ref intPInst, "rn");				// 암호화 결과값
            ER.ReturnEnc = INIpay.GetResult(ref intPInst, "return_enc");		// 암호화 결과값
            ER.IniCertId = INIpay.GetResult(ref intPInst, "ini_certid");		// 암호화 결과값
            ER.IniEncField = ER.ReturnEnc;

            //###############################################################################
            //7. RN 값 세션에 저장 #
            //###############################################################################
            Session["INI_RN"] = ER.RnValue; //	//RN값 => 결제 처리 페이지에서 체크 하기 위해 세션에 저장 (또는 DB에 저장)하여 다음 결제 처리 페이지 에서 체크)
            Session["INI_PRICE"] = payPrice; //결제 금액 =>  결제 처리 페이지에서 체크 하기 위해 세션에 저장 (또는 DB에 저장)하여 다음 결제 처리 페이지 에서

            //###############################################################################
            //# 8. 인스턴스 해제 #
            //###############################################################################
            INIpay.Destroy(ref intPInst);


            //###############################################################################
            //# 9. 결제 페이지 생성 성공 유무에 대한 처리  #
            //###############################################################################
            if (!ER.ResultCode.Equals("00"))
            {
                Response.Write("결제 페이지 생성에 문제 발생<BR>");
                Response.Write("에러원인 :  " + ER.ResultMsg);
            }

            return ER;

        }
        #endregion

        #region 2. StartINIsecurePay - 결제요청
        /// <summary>
        /// 결제요청
        /// </summary>
        /// <returns></returns>
        public StorePaymentT StartINIsecurePay()
        {
            //###############################################################################
            //# 1. 객체 생성 #
            //################
            INIPAY50Lib.INItx50 INIpay = new INIPAY50Lib.INItx50Class();

            //###############################################################################
            //# 2. 인스턴스 초기화  / 3.거래 유형 설정#
            //######################
            StorePaymentT storePaymentT = new StorePaymentT();
            int intPInst = INIpay.Initialize(string.Empty);

            INIpay.SetActionType(ref intPInst, "securepay");

            //###############################################################################
            //# 4. 지불 정보 설정 #
            //###############################################################################

            string strpaymethod = Request.Params["paymethod"];

            if (hstCode_PayMethod.ContainsKey(strpaymethod))
            {
                strpaymethod = hstCode_PayMethod[strpaymethod].ToString();
            }
            string strprice = Session["INI_PRICE"].ToString();

            INIpay.SetField(ref intPInst, "pgid", "INInet" + strpaymethod);				// PG ID (고정)
            INIpay.SetField(ref intPInst, "spgip", "203.238.3.10");						// 예비 PG IP (고정)
            INIpay.SetField(ref intPInst, "uid", Request.Params["uid"]);					// INIpay User ID(이니시스 내부변수 수정불가, 상점사용 user id 를 사용하지 마세요)
            INIpay.SetField(ref intPInst, "mid", instance.Mid);						    // 상점아이디
            INIpay.SetField(ref intPInst, "rn", Session["INI_RN"].ToString());           // 결제 요청  페이지에서  세션에 저장 (또는 DB에 저장)한 것을 체크 하기 위해  결제 처리 페이지 에서 세팅)
            INIpay.SetField(ref intPInst, "price", strprice);								// 가격



            /**************************************************************************************************
            '* admin 은 키패스워드 변수명입니다. 수정하시면 안됩니다. 1111의 부분만 수정해서 사용하시기 바랍니다.
            '* 키패스워드는 상점관리자 페이지(https://iniweb.inicis.com)의 비밀번호가 아닙니다. 주의해 주시기 바랍니다.
            '* 키패스워드는 숫자 4자리로만 구성됩니다. 이 값은 키파일 발급시 결정됩니다. 
            '* 키패스워드 값을 확인하시려면 상점측에 발급된 키파일 안의 readme.txt 파일을 참조해 주십시오.
            '**************************************************************************************************/
            INIpay.SetField(ref intPInst, "admin", instance.MidPassword);								//키패스워드(상점아이디에 따라 변경)
            INIpay.SetField(ref intPInst, "goodname", Request.Params["goodname"]);		// 상품명
            INIpay.SetField(ref intPInst, "currency", instance.Currency);								// 화폐단위
            INIpay.SetField(ref intPInst, "buyername", Request.Params["buyername"]);		// 이용자 이름
            INIpay.SetField(ref intPInst, "buyertel", Request.Params["buyertel"]);		// 이용자 이동전화
            INIpay.SetField(ref intPInst, "buyeremail", Request.Params["buyeremail"]);	// 이용자 이메일
            INIpay.SetField(ref intPInst, "paymethod", Request.Params["paymethod"]);		// 지불방법
            INIpay.SetField(ref intPInst, "encrypted", Request.Params["encrypted"]);		// 암호문
            INIpay.SetField(ref intPInst, "sessionkey", Request.Params["sessionkey"]);	// 암호문
            INIpay.SetField(ref intPInst, "url", "http://makersn.com/");					// 홈페이지 주소
            INIpay.SetField(ref intPInst, "debug", "true");								// 로그모드(실서비스시에는 "false"로)
            INIpay.SetField(ref intPInst, "merchantreserved1", "예비1");	                // 예비필드1
            INIpay.SetField(ref intPInst, "merchantreserved2", "예비2");	                // 예비필드2  
            INIpay.SetField(ref intPInst, "merchantreserved3", "예비3");	                // 예비필드3

            ////*-----------------------------------------------------------------*
            // 수취인 정보 *                                                                    
            //-----------------------------------------------------------------*
            // 실물배송을 하는 상점의 경우에 사용되는 필드들이며       *
            // 아래의 값들은 INIsecurepaystart.aspx 페이지에서 포스트 되도록  *
            // 필드를 만들어 주도록 하십시요                          *
            // 컨텐츠 제공업체의 경우 삭제하셔도 무방합니다           *
            //-----------------------------------------------------------------*
            INIpay.SetField(ref intPInst, "recvname", Request.Params["recvname"]);		 //수취인명
            INIpay.SetField(ref intPInst, "recvtel", Request.Params["recvtel"]);			 //수취인 전화번호
            INIpay.SetField(ref intPInst, "recvaddr", Request.Params["recvaddr"] + " " + Request.Params["recvaddrDetail"]);		 //수취인 주소
            INIpay.SetField(ref intPInst, "recvpostnum", Request.Params["recvpostnum"]);	 //수취인 우편번호
            INIpay.SetField(ref intPInst, "recvmsg", Request.Params["recvmsg"]);			 //수취인 전달 메세지


            INIpay.SetField(ref intPInst, "tax", Request.Params["tax"]);		 //tax
            INIpay.SetField(ref intPInst, "taxfree", Request.Params["taxfree"]);		 //taxfree
            //###############################################################################
            //# 5. 지불 요청 #
            //################
            INIpay.StartAction(ref intPInst);		                                      //지불처리


            //###############################################################################
            //6. 지불 결과 #
            //###############################################################################
            //-------------------------------------------------------------------------------
            // 가.모든 결제 수단에 공통되는 결제 결과 내용
            //-------------------------------------------------------------------------------
            storePaymentT.Common = new COMMON();
            storePaymentT.Common.Tid = INIpay.GetResult(ref intPInst, "Tid");               // 거래번호    
            storePaymentT.Common.Resultcode = INIpay.GetResult(ref intPInst, "Resultcode"); // 결과코드 ("00"이면 지불성공)
            storePaymentT.Common.ResultMsg = INIpay.GetResult(ref intPInst, "ResultMsg");   // 결과내용 : resultMsg 는 결제실패시 추적할수 있는 단서입니다. 반드시 결과페이지에 출력하시기 바랍니다.
            storePaymentT.Common.PayMethod = INIpay.GetResult(ref intPInst, "PayMethod");   // 지불방법 (매뉴얼 참조)
            storePaymentT.Common.MoId = INIpay.GetResult(ref intPInst, "Moid");             // 상점 사용 주문번호

            //**********************************************************************************
            //* 결제결과금액 =>원상품가격과  결제결과금액과 비교하여 금액이 동일하지 않다면  
            //* 결제 금액의 위변조가 의심됨으로 정상적인 처리가 되지않도록 처리 바랍니다. (해당 거래 취소 처리) 
            //**********************************************************************************
            storePaymentT.Common.TotPrice = int.Parse(INIpay.GetResult(ref intPInst, "TotPrice"));                 //결제결과금액

            //원결제금액
            if (!strprice.Equals(storePaymentT.Common.TotPrice.ToString()))
            {
                // 결제금액 위변조가 된것입니다.
                Response.Write("결재 금액 위변조");
                // 에러 처리 코드를 넣어 주시기 바랍니다.
            }

            //-------------------------------------------------------------------------------
            // 나. 신용카드,ISP,핸드폰, 전화 결제, 은행계좌이체, OK CASH BAG Point 결제시에만 결제 결과 내용  (무통장입금 , 문화 상품권 , 네모 제외) 
            //-------------------------------------------------------------------------------
            storePaymentT.Common.ApplDate = INIpay.GetResult(ref intPInst, "ApplDate");		//이니시스 승인날짜
            storePaymentT.Common.ApplTime = INIpay.GetResult(ref intPInst, "ApplTime");		//이니시스 승인시각

            //-------------------------------------------------------------------------------
            // 다. 신용카드  결제수단을 이용시에만  결제결과 내용
            //-------------------------------------------------------------------------------
            storePaymentT.VCard = new VCARD();
            storePaymentT.Common.ApplNum = INIpay.GetResult(ref intPInst, "ApplNum");			//신용카드 승인번호
            storePaymentT.VCard.Quota = INIpay.GetResult(ref intPInst, "CARD_Quota");		//할부기간  
            storePaymentT.VCard.Interest = INIpay.GetResult(ref intPInst, "CARD_Interest");	//무이자할부 여부("1"이면 무이자할부)
            storePaymentT.VCard.Num = INIpay.GetResult(ref intPInst, "CARD_Num");			//카드번호 12자리
            storePaymentT.VCard.Code = INIpay.GetResult(ref intPInst, "CARD_Code");		//신용카드사 코드 메뉴얼이나 샘플폴더 안의 //카드사_은행_코드.txt// 파일을 참고하세요
            storePaymentT.VCard.BankCode = INIpay.GetResult(ref intPInst, "CARD_BankCode");	//신용카드 발급사(은행) 코드 (매뉴얼 참조)
            storePaymentT.VCard.AuthType = INIpay.GetResult(ref intPInst, "CARD_AuthType");	//본인인증 수행여부 ("00"이면 수행)
            storePaymentT.Common.EventCode = INIpay.GetResult(ref intPInst, "EventCode");		//각종 이벤트 적용 여부

            //아래 내용은 "신용카드 및 OK CASH BAG 복합결제" 또는"신용카드 지불시에 OK CASH BAG적립"시에 추가되는 내용
            storePaymentT.Ocb = new OCB();
            storePaymentT.VCard.OcbApplTime = INIpay.GetResult(ref intPInst, "OCB_ApplTime");		//OK Cashbag 적립 승인번호
            storePaymentT.VCard.OcbSaveApplNum = INIpay.GetResult(ref intPInst, "OCB_SaveApplNum");	//OK Cashbag 적립 승인번호
            storePaymentT.VCard.OcbPayApplNum = INIpay.GetResult(ref intPInst, "OCB_PayApplNum");	//OK Cashbag 사용 승인번호
            storePaymentT.VCard.OcbApplDate = INIpay.GetResult(ref intPInst, "OCB_ApplDate");		//OK Cashbag 승인일시
            storePaymentT.Ocb.Num = INIpay.GetResult(ref intPInst, "OCB_Num");			//OK Cashbag 번호
            storePaymentT.VCard.OcbPayPrice = INIpay.GetResult(ref intPInst, "CARD_ApplPrice");	//OK Cashbag 복합결재시 신용카드 지불금액
            storePaymentT.Ocb.PayPrice = INIpay.GetResult(ref intPInst, "OCB_PayPrice");		//OK Cashbag 복합결재시 포인트 지불금액

            //-------------------------------------------------------------------------------
            // 라. 은행계좌이체 결제수단을 이용시에만  결제결과 내용
            //	오직 은행계좌시에만 실시 현금 영수증 발행이 가능하며, 가상계좌는 상점관리자 화면이나, 독립적인 현금영수증 발행(이니시스 기술자료실) 모듈을 사용하세요
            //-------------------------------------------------------------------------------
            storePaymentT.DirectBank = new DirectBank();
            storePaymentT.DirectBank.AcctBankCode = INIpay.GetResult(ref intPInst, "ACCT_BankCode");	//은행코드
            storePaymentT.DirectBank.CshrResultCode = INIpay.GetResult(ref intPInst, "rcash_rslt");		//현금영주증 결과코드 ("0000"이면 지불성공)
            storePaymentT.DirectBank.CshrType = INIpay.GetResult(ref intPInst, "ruseopt");			//현금영수증 발행구분코드 

            //-------------------------------------------------------------------------------
            // 마. 무통장 입금(가상계좌) 결제수단을 이용시 결과 내용
            //-------------------------------------------------------------------------------
            storePaymentT.VBank = new VBank();
            storePaymentT.VBank.VactNum = INIpay.GetResult(ref intPInst, "VACT_Num"); 		// 입금 계좌 번호
            storePaymentT.VBank.VactBankCode = INIpay.GetResult(ref intPInst, "VACT_BankCode"); 	// 입금 은행 코드
            storePaymentT.VBank.VactDate = INIpay.GetResult(ref intPInst, "VACT_Date"); 		// 입금 예정 날짜
            storePaymentT.VBank.VactTime = INIpay.GetResult(ref intPInst, "VACT_Time"); 		// 입금 예정 시간 [ 20061018 ]
            storePaymentT.VBank.VactInputName = INIpay.GetResult(ref intPInst, "VACT_InputName"); 	// 송금자명
            storePaymentT.VBank.VactName = INIpay.GetResult(ref intPInst, "VACT_Name");		// 예금주명

            //-------------------------------------------------------------------------------
            // 바. 핸드폰, 전화결제시에만  결제 결과 내용 ( "실패 내역 자세히 보기"에서 필요 , 상점에서는 필요없는 정보임)
            //-------------------------------------------------------------------------------
            storePaymentT.Hpp = new HPP();
            storePaymentT.Hpp.GWCode = INIpay.GetResult(ref intPInst, "HPP_GWCode");		//핸드폰,전화결제시 gateway

            //-------------------------------------------------------------------------------
            // 사. 핸드폰 결제수단을 이용시에만  결제 결과 내용
            //-------------------------------------------------------------------------------
            storePaymentT.Hpp.Num = INIpay.GetResult(ref intPInst, "HPP_Num");			//핸드폰 결제에 사용된 휴대폰번호

            //-------------------------------------------------------------------------------
            // 아. ARS 전화 결제수단을 이용시에만  결제 결과 내용
            //-------------------------------------------------------------------------------
            storePaymentT.Arsb = new ARSB();
            storePaymentT.Arsb.Num = INIpay.GetResult(ref intPInst, "ARSB_Num");			//전화결제에  사용된 전화번호

            // 자. 받는 전화 결제수단을 이용시에만  결제 결과 내용
            //-------------------------------------------------------------------------------
            storePaymentT.Phnb = new PHNB();
            storePaymentT.Phnb.Num = INIpay.GetResult(ref intPInst, "PHNB_Num");			//전화결제에  사용된 전화번호

            //-------------------------------------------------------------------------------
            // 차. 문화 상품권 결제수단을 이용시에만  결제 결과 내용	
            //-------------------------------------------------------------------------------
            storePaymentT.Cult = new CULT();
            storePaymentT.Cult.UserId = INIpay.GetResult(ref intPInst, "CULT_UserID");		//문화상품권 ID

            //-------------------------------------------------------------------------------
            // 카. 현금영수증 발급 결과코드 (은행계좌이체시에만 리턴);
            //-------------------------------------------------------------------------------            
            storePaymentT.DirectBank.CshrResultCode = INIpay.GetResult(ref intPInst, "CSHR_ResultCode");	// 결과코드
            storePaymentT.DirectBank.CshrType = INIpay.GetResult(ref intPInst, "CSHR_Type");		//발급구분코드

            //-------------------------------------------------------------------------------
            // 파. 틴캐시 결제수단을 이용시에만 결제 결과 내용
            //-------------------------------------------------------------------------------            
            storePaymentT.Teen = new TEEN();
            storePaymentT.Teen.Remains = INIpay.GetResult(ref intPInst, "TEEN_Remains");		//틴캐시 잔액
            storePaymentT.Teen.UserId = INIpay.GetResult(ref intPInst, "TEEN_UserID");		//틴캐시 ID

            //-------------------------------------------------------------------------------
            // 타.스마트문상(게임 문화 상품권) 결제수단을 이용시에만 결제 결과 내용
            //-------------------------------------------------------------------------------
            storePaymentT.Gamg = new GAMG();
            storePaymentT.Gamg.Cnt = INIpay.GetResult(ref intPInst, "GAMG_Cnt");			//카드 사용 갯수

            //-------------------------------------------------------------------------------
            // 하. 도서문화 상품권 결제수단을 이용시에만 결제 결과 내용
            //-------------------------------------------------------------------------------
            storePaymentT.Bcsh = new BCSH();
            storePaymentT.Bcsh.UserId = INIpay.GetResult(ref intPInst, "BCSH_UserID");		//문화상품권 ID

            //-------------------------------------------------------------------------------
            // 바우처 결제
            //-------------------------------------------------------------------------------
            storePaymentT.Ssvc = new SSVC();
            

            //-------------------------------------------------------------------------------
            // * . 모든 결제 수단에 대해 결제 실패시에만 결제 결과 데이터 				
            //-------------------------------------------------------------------------------
            storePaymentT.Common.ResultErrorcode = INIpay.GetResult(ref intPInst, "ResultErrorCode");	//결제실패시 상세에러코드 


            /*###############################################################################
			# 결과 수신 확인 #
			#################################################################################
			지불결과를 잘 수신하였음을 이니시스에 통보.
			[주의] 이 과정이 누락되면 모든 거래가 자동취소됩니다.	*/

            string AckResult = string.Empty;
            if (storePaymentT.Common.Resultcode == "00")
            {
                AckResult = INIpay.Ack(ref intPInst);
                if (AckResult != "SUCCESS")  //실패
                {
                    /*
                        '=================================================================
                        ' 정상수신 통보 실패인 경우 이 승인은 이니시스에서 자동 취소되므로
                        ' 지불결과를 다시 받아옵니다(성공 -> 실패).
                        '=================================================================
                    */
                    storePaymentT.Common.Resultcode = INIpay.GetResult(ref intPInst, "Resultcode");
                    storePaymentT.Common.ResultMsg = INIpay.GetResult(ref intPInst, "ResultMsg");
                }
            }


            INIpay.Destroy(ref intPInst);

            #region 주문서 저장(DB 처리)
            /*###############################################################################
            # 지불결과 DB 연동 부분 #
            # 지불결과를 디비처리하시고 디비처리시 실패가 나시면 이니시스에 취소요청을 합니다
            ###############################################################################*/
            
            StoreOrderT storeOrderT = new StoreOrderT();
            StoreOrderBiz orderBiz = new StoreOrderBiz();
            StoreCartBiz cartBiz = new StoreCartBiz();
            List<OrderInfo> OrderInfoList = new List<OrderInfo>();
            List<StoreOrderDetailT> storeOrderDetailList = new List<StoreOrderDetailT>();
            Int64 orderMasterNo = 0;
            int orderDetailNo = 0;

            storeOrderT.OID = Request.Params["oid"];
            storeOrderT.CART_NO = Request.Params["cartNo"];
            storeOrderT.MID = instance.Mid;
            storeOrderT.PRICE = storePaymentT.Common.TotPrice;
            storeOrderT.GOOD_NAME = Request.Params["goodname"];
            storeOrderT.PAY_METHOD = storePaymentT.Common.PayMethod;
            storeOrderT.TYPE = "chkfake"; // 고정값
            storeOrderT.BUYER_NAME = Request.Params["buyername"];
            storeOrderT.BUYER_EMAIL = Request.Params["buyeremail"];
            storeOrderT.BUYER_TEL = Request.Params["buyertel"];
            storeOrderT.RECV_NAME = Request.Params["recvname"];
            storeOrderT.RECV_TEL = Request.Params["recvtel"];
            storeOrderT.RECV_MSG = Request.Params["recvmsg"];
            storeOrderT.USER_ID = profileModel.UserId;
            storeOrderT.MEMBER_NO = profileModel.UserNo;
            storeOrderT.ORDER_DATE = DateTime.Now;
            storeOrderT.ORDER_STATUS = StringEnum.GetValue(OrderStatus.Complete); // 주문완료
            if (storePaymentT.Common.PayMethod.Equals("VBank"))
            {
                storeOrderT.PAYMENT_STATUS = StringEnum.GetValue(PaymentStatus.Waiting); // 결제대기
            }
            else
            {
                storeOrderT.PAYMENT_STATUS = StringEnum.GetValue(PaymentStatus.Complete); // 결제완료
            }
            storeOrderT.SHIPPING_ADDR_NO = Int64.Parse(Request.Params["shippingAddrNo"]);
            storeOrderT.SHIPPING_COST = int.Parse(Request.Params["shippingCost"]);
            storeOrderT.SLOW_MAKE_YN = Request.Params["rdoSlowMakeYn"];
            storeOrderT.REG_DT = DateTime.Now;
            storeOrderT.REG_ID = profileModel.UserId;

            // 주문 마스터 저장
            orderMasterNo = orderBiz.InsertOrderInfo(storeOrderT);

            // 주문상세내역 호출
            OrderInfoList = orderBiz.GetStoreOrderListByMemberNo(profileModel.UserNo);

            foreach(OrderInfo info in OrderInfoList)
            {
                StoreOrderDetailT item = new StoreOrderDetailT();
                item.ORDER_MASTER_NO = orderMasterNo;
                item.ITEM_NO = info.ITEM_NO;
                item.ITEM_PRICE = info.BASE_PRICE;
                item.ITEM_CNT = info.ITEM_CNT;
                item.PRINTING_STATUS = StringEnum.GetValue(PrintingStatus.Waiting); // 출력대기
                item.REG_DT = DateTime.Now;
                item.REG_ID = profileModel.UserId;
                storeOrderDetailList.Add(item);
            }

            
            // 주문상세내역 저장
            if (orderMasterNo > 0)
            {
                orderDetailNo = orderBiz.InsertOrderDetailInfo(storeOrderDetailList);

                // 카트 주문상태 업데이트
                cartBiz.SetCartByCartNo(storeOrderT.CART_NO);

                storePaymentT.OrderMasterNo = orderMasterNo;
            }
            #endregion

            #region 결제 실패시(DB 연결 실패)
            if (orderDetailNo == 0)
            {
                // * 데이터베이스 처리부분 삽입
                // * 처리 실패시 아래 주석부분을 풀면, 이니시스에 해당 거래를 취소요청합니다

                //###############################################################################
                //# 1. 객체 생성 #
                //################
                INIPAY50Lib.INItx50 INIpayCancel = new INIPAY50Lib.INItx50Class();

                //###############################################################################
                //# 2. 인스턴스 초기화 /3. 거래 유형 설정 #
                //######################
                INIpayCancel.Initialize("cancel");
                intPInst = INIpay.Initialize(string.Empty);

                INIpay.SetActionType(ref intPInst, "cancel");
                //###############################################################################
                //# 4. 정보 설정 #
                //################
                INIpay.SetField(ref intPInst, "pgid", instance.PgId); //PG ID (고정)
                INIpayCancel.SetField(ref intPInst, "mid", instance.Mid);				// 상점아이디
                INIpayCancel.SetField(ref intPInst, "admin", instance.MidPassword);								// 키패스워드(상점아이디에 따라 변경)
                INIpayCancel.SetField(ref intPInst, "tid", storePaymentT.Common.Tid);								// 취소할 거래번호
                INIpayCancel.SetField(ref intPInst, "CancelMsg", "MERCHANT'S DB FAIL");			// 취소 사유
                INIpayCancel.SetField(ref intPInst, "debug", "true");								// 로그모드(실서비스시에는 "false"로)

                //###############################################################################
                //# 5. 취소 요청 #
                //################
                INIpayCancel.StartAction(ref intPInst);

                //###############################################################################
                //# 6. 취소 결과 #
                //################
                storePaymentT.Common.Resultcode = INIpayCancel.GetResult(ref intPInst, "resultcode"); // 결과코드 ("00"이면 지불성공)
                storePaymentT.Common.ResultMsg = INIpayCancel.GetResult(ref intPInst, "resultmsg"); // 결과내용

                if (storePaymentT.Common.Resultcode.Equals("00"))
                {
                    storePaymentT.Common.Resultcode = "01";
                    storePaymentT.Common.ResultMsg = "지불결과 DB 연동 실패";
                }

                //###############################################################################
                //# 7. 인스턴스 해제 #
                //####################
                INIpayCancel.Destroy(ref intPInst);
            }
            #endregion

            return storePaymentT;
        }
        #endregion
        
        #region 3. INIsecureResult - 결제결과 컨트롤러
        /// <summary>
        /// 결제결과 컨트롤러
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult INIsecureResult(FormCollection forms)
        {
            StorePaymentHistoryT storePaymentHistoryT = new StorePaymentHistoryT();
            StorePaymentT model = new StorePaymentT();
            StorePaymentBiz biz = new StorePaymentBiz();
            HashData(); // 결제모듈 보다 먼저 실행되어야함
            model = StartINIsecurePay();
            

            if (model != null)
            {
                storePaymentHistoryT.ORDER_MASTER_NO = model.OrderMasterNo;
                storePaymentHistoryT.TID = model.Common.Tid;
                storePaymentHistoryT.RESULT_CODE = model.Common.Resultcode;
                storePaymentHistoryT.RESULT_MSG = model.Common.ResultMsg;
                storePaymentHistoryT.M_OID = model.Common.MoId;
                storePaymentHistoryT.APPL_DATE = model.Common.ApplDate;
                storePaymentHistoryT.APPL_TIME = model.Common.ApplTime;
                storePaymentHistoryT.APPL_NUM = model.Common.ApplNum;
                storePaymentHistoryT.PAY_METHOD = model.Common.PayMethod;
                storePaymentHistoryT.TOT_PRICE = model.Common.TotPrice;
                storePaymentHistoryT.EVENT_CODE = model.Common.EventCode;
                storePaymentHistoryT.CARD_NUM = model.VCard.Num;
                storePaymentHistoryT.CARD_INTEREST = model.VCard.Interest;
                storePaymentHistoryT.CARD_QUOTA = model.VCard.Quota;
                storePaymentHistoryT.CARD_CODE = model.VCard.Code;
                storePaymentHistoryT.CARD_BANK_CODE = model.VCard.BankCode;
                storePaymentHistoryT.ORG_CURRENCY = instance.Currency;
                storePaymentHistoryT.EXCHANGE_RATE = model.VCard.ExchangeRate;
                storePaymentHistoryT.CARD_OCB_NUM = model.VCard.OcbNum;
                storePaymentHistoryT.CARD_OCB_SAVE_APPL_NUM = model.VCard.OcbSaveApplNum;
                storePaymentHistoryT.CARD_OCB_PAY_APPL_NUM = model.VCard.OcbPayApplNum;
                storePaymentHistoryT.CARD_OCB_APPL_DATE = model.VCard.OcbApplDate;
                storePaymentHistoryT.CARD_OCB_PAY_PRICE = model.VCard.OcbPayPrice;
                storePaymentHistoryT.CARD_CHECK_FLAG = model.VCard.CheckFlag;
                storePaymentHistoryT.ISP_CHECK_FLAG = model.VCard.IspCheckFlag;
                storePaymentHistoryT.ACCT_BANK_CODE = model.DirectBank.AcctBankCode;
                storePaymentHistoryT.CSHR_RESULT_CODE = model.DirectBank.CshrResultCode;
                storePaymentHistoryT.CSHR_TYPE = model.DirectBank.CshrType;
                storePaymentHistoryT.VACT_NUM = model.VBank.VactNum;
                storePaymentHistoryT.VACT_BANK_CODE = model.VBank.VactBankCode;
                storePaymentHistoryT.VACT_NAME = model.VBank.VactName;
                storePaymentHistoryT.VACT_INPUT_NAME = model.VBank.VactInputName;
                storePaymentHistoryT.VACT_DATE = model.VBank.VactDate;
                storePaymentHistoryT.VACT_TIME = model.VBank.VactTime;
                storePaymentHistoryT.HPP_NUM = model.Hpp.Num;
                storePaymentHistoryT.ARSB_NUM = model.Arsb.Num;
                storePaymentHistoryT.PHNB_NUM = model.Phnb.Num;
                storePaymentHistoryT.OCB_NUM = model.Ocb.Num;
                storePaymentHistoryT.OCB_SAVE_APPL_NUM = model.VCard.OcbSaveApplNum;
                storePaymentHistoryT.OCB_PAY_APPL_NUM = model.VCard.OcbPayApplNum;
                storePaymentHistoryT.OCB_PAY_PRICE = model.VCard.OcbPayPrice;
                storePaymentHistoryT.CULT_USER_ID = model.Cult.UserId;
                storePaymentHistoryT.TEEN_REMAINS = model.Teen.Remains;
                storePaymentHistoryT.TEEN_USER_ID = model.Teen.UserId;
                storePaymentHistoryT.GAMG_CNT = model.Gamg.Cnt;
                storePaymentHistoryT.GAMG_NUM = model.Gamg.Num;
                storePaymentHistoryT.GAMG_ERR_MSG = model.Gamg.ErrMsg;
                storePaymentHistoryT.BCSH_USER_ID = model.Bcsh.UserId;
                storePaymentHistoryT.CARD_APPL_PRICE = model.Ssvc.CardApplPrice;
                storePaymentHistoryT.SSVC_APPL_PRICE = model.Ssvc.ApplPrice;
                storePaymentHistoryT.SSVC_CARD_PRICE = model.Ssvc.CardPrice;
                storePaymentHistoryT.SSVC_POINT_PRICE = model.Ssvc.PointPricee;
                storePaymentHistoryT.REG_DT = DateTime.Now;
                storePaymentHistoryT.REG_ID = profileModel.UserId;

                // 결과 저장
                biz.InsertStorePaymentHistory(storePaymentHistoryT);
            }


            return View(model);
        }
        #endregion
        
        #region GetPostInfoData - 우편번호 검색
        /// <summary>
        /// 우편번호 검색
        /// </summary>
        [HttpPost]
        public JsonResult GetPostInfoData(string srchwrd, int countPerPage, int currentPage)
        {
            AjaxResponseModel response = new AjaxResponseModel();            
            PostAddressInfo postInfo = new PostAddressInfo();
            XmlDocument docX = new XmlDocument();

            response.Success = false;
            string ServiceUrl = instance.ServiceUrl;
            string ServiceKey = instance.ServiceKey;      
            
            WebClient wc = new WebClient() { Encoding = Encoding.UTF8 };

            #region api호출            
            srchwrd = HttpUtility.UrlEncode(srchwrd, Encoding.GetEncoding("UTF-8"));
            string qry = String.Format("{0}?ServiceKey={1}&countPerPage={2}&currentPage={3}&srchwrd={4}", ServiceUrl, ServiceKey, countPerPage, currentPage, srchwrd);
            #endregion

            try
            {
                docX.Load(qry);
                XmlNode x = docX.ChildNodes[1];                
                postInfo.CmmMsgHeader = new CmmMsgHeader();                
                postInfo.AddressList = new List<NewAddressListAreaCdSearchAll>();
                int index = 0;

                foreach (XmlNode node in x.ChildNodes)
                {
                    if (index == 0)
                    {
                        postInfo.CmmMsgHeader.RequestMsgId = node.ChildNodes[0].InnerText;
                        postInfo.CmmMsgHeader.ResponseMsgId = node.ChildNodes[1].InnerText;
                        postInfo.CmmMsgHeader.ResponseTime = node.ChildNodes[2].InnerText;
                        postInfo.CmmMsgHeader.SuccessYn = node.ChildNodes[3].InnerText;
                        postInfo.CmmMsgHeader.ReturnCode = node.ChildNodes[4].InnerText;
                        postInfo.CmmMsgHeader.ErrMsg = node.ChildNodes[5].InnerText;
                        postInfo.CmmMsgHeader.TotalCount = node.ChildNodes[6].InnerText;
                        postInfo.CmmMsgHeader.CountPerPage = node.ChildNodes[7].InnerText;
                        postInfo.CmmMsgHeader.TotalPage = node.ChildNodes[8].InnerText;
                        postInfo.CmmMsgHeader.CurrentPage = node.ChildNodes[9].InnerText;
                    }
                    else
                    {
                        NewAddressListAreaCdSearchAll item = new NewAddressListAreaCdSearchAll();
                        item.ZipNo = node.ChildNodes[0].InnerText;
                        item.LnmAdres = node.ChildNodes[1].InnerText;
                        item.RnAdres = node.ChildNodes[2].InnerText;
                        postInfo.AddressList.Add(item);
                    }
                    index++;
                }

                response.Success = true;
                response.Result = JsonConvert.SerializeObject(postInfo);

            }
            catch (Exception ex)
            {

            }

            
            return Json(response, JsonRequestBehavior.AllowGet); ;

        }
        #endregion

        #region Paging - 페이징처리
        /// <summary>
        /// 페이징처리
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="PageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public PartialViewResult Paging(int currentPage = 1, int PageSize = 20, int totalCount = 0)
        {
            BoardInfo model = new BoardInfo();
            model.CurrentPage = currentPage;
            model.PageSize = PageSize;
            model.TotalCount = totalCount;

            return PartialView(model);
        }
        #endregion

        #region HashData - 절대 임의로 수정하지 말것
        /// <summary>
        /// 지불수단별로 PGID를 다르게 표시한다 (2003.12.19: ts@inicis.com) 
        /// 하단의 Code_PayMethod 함수는 지불수단별로 TID를 별도로 표시하도록 하며,
        /// 임의로 수정하는 경우 지불 실패가 발생 될수 있으므로 절대로 수정 하지 않도록 하시기 바랍니다.
        /// 임의로 수정하여 발생된 문제에 대해서는 (주)이니시스에 책임이 없으니 주의 하시기 바랍니다.
        /// </summary>
        private void HashData()
        {
            hstCode_PayMethod["Card"] = "CARD";
            hstCode_PayMethod["VCard"] = "ISP_";
            hstCode_PayMethod["Account"] = "ACCT";
            hstCode_PayMethod["DirectBank"] = "DBNK";
            hstCode_PayMethod["INIcard"] = "INIC";
            hstCode_PayMethod["OCBPoint"] = "OCBP";
            hstCode_PayMethod["MDX"] = "MDX_";
            hstCode_PayMethod["HPP"] = "HPP_";
            hstCode_PayMethod["Nemo"] = "NEMO";
            hstCode_PayMethod["ArsBill"] = "ARSB";
            hstCode_PayMethod["PhoneBill"] = "PHNB";
            hstCode_PayMethod["Ars1588Bill"] = "1588";
            hstCode_PayMethod["VBank"] = "VBNK";
            hstCode_PayMethod["Culture"] = "CULT";
            hstCode_PayMethod["CMS"] = "CMS_";
            hstCode_PayMethod["AUTH"] = "AUTH";
        }
        #endregion

        #region ContractList - 구매내역
        /// <summary>
        /// 구매내역
        /// </summary>
        /// <returns></returns>
        public ActionResult ContractList()
        {
            StoreOrderBiz biz = new StoreOrderBiz();
            DateTime startDt = DateTime.Today.AddMonths(-1);
            DateTime endDt = DateTime.Today.AddDays(1);

            List<StoreOrderT> model = biz.GetContractListByCondition(profileModel.UserNo, startDt, endDt);

            return View(model);
        }
        #endregion

        #region GetContractDetailList - oId에 따른 구매상세내역
        /// <summary>
        /// oId에 따른 구매상세내역
        /// </summary>
        /// <param name="oId"></param>
        /// <returns></returns>
        public List<ContractDetail> GetContractDetailList(string oId)
        {
            List<ContractDetail> list = new List<ContractDetail>();
            StoreOrderBiz biz = new StoreOrderBiz();

            list = biz.GetContractDetailListByOid(oId);

            return list;
        }
        #endregion

        #region StartINICancel - 주문취소
        /// <summary>
        /// 주문취소
        /// </summary>
        /// <param name="oId"></param>
        [HttpPost]
        public JsonResult StartINICancel(string oId, string cancelMsg, string cancelReason)
        {
            AjaxResponseModel response = new AjaxResponseModel();            
            StoreOrderBiz biz = new StoreOrderBiz();
            string tId = biz.GetTradeId(oId);

            //###############################################################################
            //# 1. 객체 생성 #
            //################
            INIPAY50Lib.INItx50 INIpay = new INIPAY50Lib.INItx50Class();

            //###############################################################################
            //# 2. 인스턴스 초기화 /3. 거래 유형 설정 #
            //######################

            int intPInst = INIpay.Initialize(string.Empty);

            INIpay.SetActionType(ref intPInst, "cancel");


            //###############################################################################
            //# 4. 정보 설정 #
            //################
            INIpay.SetField(ref intPInst, "pgid", instance.PgId); //PG ID (고정)

            INIpay.SetField(ref intPInst, "mid", instance.Mid);	  // 상점아이디

            /**************************************************************************************************
            '* admin 은 키패스워드 변수명입니다. 수정하시면 안됩니다. 1111의 부분만 수정해서 사용하시기 바랍니다.
            '* 키패스워드는 상점관리자 페이지(https://iniweb.inicis.com)의 비밀번호가 아닙니다. 주의해 주시기 바랍니다.
            '* 키패스워드는 숫자 4자리로만 구성됩니다. 이 값은 키파일 발급시 결정됩니다. 
            '* 키패스워드 값을 확인하시려면 상점측에 발급된 키파일 안의 readme.txt 파일을 참조해 주십시오.
            '**************************************************************************************************/
            INIpay.SetField(ref intPInst, "admin", instance.MidPassword);						// 키패스워드(상점아이디에 따라 변경)

            INIpay.SetField(ref intPInst, "tid", tId);			// 취소할 거래번호
            INIpay.SetField(ref intPInst, "CancelMsg", cancelMsg);	// 취소 사유
            INIpay.SetField(ref intPInst, "CancelReason", cancelReason);	// 취소 코드
            INIpay.SetField(ref intPInst, "debug", "true");						// 로그모드(실서비스시에는 "false"로)

            //###############################################################################
            //# 5. 취소 요청 #
            //################
            INIpay.StartAction(ref intPInst);

            //###############################################################################
            //# 6. 취소 결과 #
            //################
            StorePaymentCancelT storePaymentCancelT = new StorePaymentCancelT();
            storePaymentCancelT.RESULT_CODE = INIpay.GetResult(ref intPInst, "resultcode");					// 결과코드 ("00"이면 지불성공)
            storePaymentCancelT.RESULT_MSG = INIpay.GetResult(ref intPInst, "resultmsg");					// 결과내용
            storePaymentCancelT.CANCEL_DATE = INIpay.GetResult(ref intPInst, "CancelDate");					// 이니시스 취소날짜
            storePaymentCancelT.CANCEL_TIME = INIpay.GetResult(ref intPInst, "CancelTime");					// 이니시스 취소시각
            storePaymentCancelT.CSHR_CANCEL_NUM = INIpay.GetResult(ref intPInst, "CSHR_CancelNum");			//현금영수증 취소 승인번호
            storePaymentCancelT.REG_DT = DateTime.Now;
            storePaymentCancelT.REG_ID = profileModel.UserId;

            //###############################################################################
            //# 7. 인스턴스 해제 #
            //####################
            INIpay.Destroy(ref intPInst);

            if (storePaymentCancelT.RESULT_CODE.Equals("00"))
            {
                // 결제취소 테이블 저장
                biz.InsertOrderCancelInfo(storePaymentCancelT);

                // 주문취소 상태값 업데이트
                //biz.UpdateOrderStatus(oId, StringEnum.GetValue(OrderStatus.Cancel));

                // 결제취소 상태값 업데이트
                biz.UpdatePaymentStatus(oId, StringEnum.GetValue(PaymentStatus.Cancel));

                string email = "sslee@makersi.com";
                string title = "결제취소";
                string comment = storePaymentCancelT.RESULT_MSG;
                string Subject = "주문에 대해서 결제취소 처리 되었습니다.";

                SendMailModels oMail = new SendMailModels();
                oMail.SendMail("PaymentCancel", email, new String[] { Subject, title, comment });
                
            }

            response.Success = true;
            response.Result = storePaymentCancelT.RESULT_MSG;

            return Json(response, JsonRequestBehavior.AllowGet); ;
        }
        #endregion

        #region GetVirtualBankPaymentCheckData - 가상계좌 입금확인

        public ActionResult VacctinputConfirmTest()
        {
            return View();
        }

        /// <summary>
        /// 가상계좌 입금확인
        /// </summary>
        /// <param name="forms"></param>
        [HttpPost]
        public void GetVirtualBankPaymentCheckData(FormCollection forms)
        {
            VacctInputData data = new VacctInputData();
            string requestIp = Request.UserHostAddress;
            string pgIp = instance.PgIp.Substring(0, 10);

            // pg에서 보냈는지 ip로 체크
            if (requestIp.Equals(pgIp))
            {
                data.noTid = forms["NO_TID"];
                data.noOid = forms["NO_OID"];
                data.cdBank = forms["CD_BANK"];
                data.cdDeal = forms["CD_DEAL"];
                data.dtTrans = forms["DT_TRANS"];
                data.tmTrans = forms["TM_TRANS"];
                data.noVacct = forms["NO_VACCT"];
                data.amtInput = forms["AMT_INPUT"];
                data.amtCheck = forms["AMT_CHECK"];
                data.flgClose = forms["FLG_CLOSE"];
                data.clClose = forms["CL_CLOSE"];
                data.typeMsg = forms["TYPE_MSG"];
                data.nmInputBank = forms["NM_INPUTBANK"];
                data.nmInput = forms["NM_INPUT"];
                data.dtInputStd = forms["DT_INPUTSTD"];
                data.dtCalculStd = forms["DT_CALCULSTD"];
                data.dtTransBase = forms["DT_TRANSBASE"];
                data.clTrans = forms["CL_TRANS"];
                data.clKor = forms["CL_KOR"];
                data.dtCshr = forms["DT_CSHR"];
                data.tmCshr = forms["TM_CSHR"];
                data.noCshrAppl = forms["NO_CSHR_APPL"];
                data.noCshrTid = forms["NO_CSHR_TID"];

            }

            var jsonData = JsonConvert.SerializeObject(data);
            
            // 로그 남기기
            log4net.Config.XmlConfigurator.Configure();
            logger.Debug(Environment.NewLine + jsonData);

            // 입금확인 성공시 출력
            Response.Write("OK");



        }
        #endregion
    }
}
