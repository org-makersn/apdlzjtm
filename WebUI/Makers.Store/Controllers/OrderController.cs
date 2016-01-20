using Makersn.BizDac;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using Net.Common.Model;
using Newtonsoft.Json;
using Makersn.Models;
using System.Web.Util;
using System.Text;
using System.Xml;
using Makers.Store.Configurations;

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
            OrderMaster master = new OrderMaster();
            master.OrderInfoList = new List<OrderInfo>();
            master.StoreCart = new StoreCartT();           

            master.StoreCart.CartNo = forms["cartNo"];
            master.EncResult = StartChkFake(); // 페이지 위변조 체크
            master.OrderInfoList = GetOrderList(1); // admin으로 조회        

            return View(master);
        }

        public ActionResult PopPostInfo()
        {
            return View();
        }

        #region PostNo - 우편번호 검색
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

        #region GetOrderList - 주문정보 Get
        /// <summary>
        /// GetOrderList - 주문정보 Get
        /// </summary>
        /// <param name="cartNo"></param>
        /// <returns></returns>
        public List<OrderInfo> GetOrderList(int memberNo)
        {
            List<OrderInfo> orderInfoList = new List<OrderInfo>();
            orderInfoList = new StoreOrderBiz().GetStoreOrderListByMemberNo(memberNo);

            return orderInfoList;
        }
        #endregion

        #region StartChkFake - 위변조 체크
        /// <summary>
        /// StartChkFake - 위변조 체크
        /// </summary>
        /// <returns></returns>
        public EncResult StartChkFake()
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
            Session["INI_PRICE"] = "1004"; //결제 금액 =>  결제 처리 페이지에서 체크 하기 위해 세션에 저장 (또는 DB에 저장)하여 다음 결제 처리 페이지 에서

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
