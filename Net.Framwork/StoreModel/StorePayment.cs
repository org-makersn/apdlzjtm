using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_PAYMENT_HISTORY")]
    public class StorePaymentHistoryT
    {
        [Key, Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual Int64 No { get; set; }

        public virtual Int64 ORDER_MASTER_NO { get; set; }
        public virtual string TID { get; set; }
        public virtual string RESULT_CODE { get; set; }
        public virtual string RESULT_MSG { get; set; }
        public virtual string M_OID { get; set; }
        public virtual string APPL_DATE { get; set; }
        public virtual string APPL_TIME { get; set; }
        public virtual string APPL_NUM { get; set; }
        public virtual string PAY_METHOD { get; set; }
        public virtual int TOT_PRICE { get; set; }
        public virtual string EVENT_CODE { get; set; }
        public virtual string CARD_NUM { get; set; }
        public virtual string CARD_INTEREST { get; set; }
        public virtual string CARD_QUOTA { get; set; }
        public virtual string CARD_CODE { get; set; }
        public virtual string CARD_BANK_CODE { get; set; }
        public virtual string ORG_CURRENCY { get; set; }
        public virtual string EXCHANGE_RATE { get; set; }
        public virtual string CARD_OCB_NUM { get; set; }
        public virtual string CARD_OCB_SAVE_APPL_NUM { get; set; }
        public virtual string CARD_OCB_PAY_APPL_NUM { get; set; }
        public virtual string CARD_OCB_APPL_DATE { get; set; }
        public virtual string CARD_OCB_PAY_PRICE { get; set; }
        public virtual string CARD_CHECK_FLAG { get; set; }
        public virtual string ISP_CHECK_FLAG { get; set; }
        public virtual string ACCT_BANK_CODE { get; set; }
        public virtual string CSHR_RESULT_CODE { get; set; }
        public virtual string CSHR_TYPE { get; set; }
        public virtual string VACT_NUM { get; set; }
        public virtual string VACT_BANK_CODE { get; set; }
        public virtual string VACT_NAME { get; set; }
        public virtual string VACT_INPUT_NAME { get; set; }
        public virtual string VACT_DATE { get; set; }
        public virtual string VACT_TIME { get; set; }
        public virtual string HPP_NUM { get; set; }
        public virtual string ARSB_NUM { get; set; }
        public virtual string PHNB_NUM { get; set; }
        public virtual string OCB_NUM { get; set; }
        public virtual string OCB_SAVE_APPL_NUM { get; set; }
        public virtual string OCB_PAY_APPL_NUM { get; set; }
        public virtual string OCB_PAY_PRICE { get; set; }
        public virtual string CULT_USER_ID { get; set; }
        public virtual string TEEN_REMAINS { get; set; }
        public virtual string TEEN_USER_ID { get; set; }
        public virtual string GAMG_CNT { get; set; }
        public virtual string GAMG_NUM { get; set; }
        public virtual string GAMG_REMAINS { get; set; }
        public virtual string GAMG_ERR_MSG { get; set; }
        public virtual string BCSH_USER_ID { get; set; }
        public virtual int CARD_APPL_PRICE { get; set; }
        public virtual Nullable<int> SSVC_APPL_PRICE { get; set; }
        public virtual Nullable<int> SSVC_CARD_PRICE { get; set; }
        public virtual Nullable<int> SSVC_POINT_PRICE { get; set; }
        public virtual Nullable<int> SSVC_REMAIN { get; set; }
        public virtual DateTime REG_DT { get; set; }
        public virtual string REG_ID { get; set; }
    }

    /// <summary>
    /// 결제취소 테이블
    /// </summary>
    [Table("STORE_PAYMENT_CANCEL")]
    public class StorePaymentCancelT
    {
        [Key, Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual Int64 No { get; set; }

        public virtual string RESULT_CODE { get; set; }
        public virtual string RESULT_MSG { get; set; }
        public virtual string CANCEL_DATE { get; set; }
        public virtual string CANCEL_TIME { get; set; }
        public virtual string CSHR_CANCEL_NUM { get; set; }
        public virtual DateTime REG_DT { get; set; }
        public virtual string REG_ID { get; set; }
    }

    public class StorePaymentT
    {
        public virtual Int64 OrderMasterNo { get; set; }
        public virtual COMMON Common { get; set; }
        public virtual VCARD VCard { get; set; }
        public virtual DirectBank DirectBank { get; set; }
        public virtual VBank VBank { get; set; }
        public virtual HPP Hpp { get; set; }
        public virtual ARSB Arsb { get; set; }
        public virtual PHNB Phnb { get; set; }
        public virtual OCB Ocb { get; set; }
        public virtual CULT Cult { get; set; }
        public virtual TEEN Teen { get; set; }
        public virtual GAMG Gamg { get; set; }
        public virtual BCSH Bcsh { get; set; }
        public virtual SSVC Ssvc { get; set; }
    }

    /// <summary>
    /// 가상계좌 입금확인
    /// </summary>
    public class VacctInputData
    {
        public virtual string noTid { get; set; }
        public virtual string noOid { get; set; }
        public virtual string cdBank { get; set; }
        public virtual string cdDeal { get; set; }
        public virtual string dtTrans { get; set; }
        public virtual string tmTrans { get; set; }
        public virtual string noVacct { get; set; }
        public virtual string amtInput { get; set; }
        public virtual string amtCheck { get; set; }
        public virtual string flgClose { get; set; }
        public virtual string clClose { get; set; }
        public virtual string typeMsg { get; set; }
        public virtual string nmInputBank { get; set; }
        public virtual string nmInput { get; set; }
        public virtual string dtInputStd { get; set; }
        public virtual string dtCalculStd { get; set; }
        public virtual string dtTransBase { get; set; }
        public virtual string clTrans { get; set; }
        public virtual string clKor { get; set; }
        public virtual string dtCshr { get; set; }
        public virtual string tmCshr { get; set; }
        public virtual string noCshrAppl { get; set; }
        public virtual string noCshrTid { get; set; }
    }

    /// <summary>
    /// 가.모든 결제 수단에 공통되는 결제 결과 내용
    /// </summary>
    public class COMMON
    {        
        public virtual string Tid { get; set; }
        public virtual string Resultcode { get; set; }
        public virtual string ResultMsg { get; set; }
        public virtual string MoId { get; set; }
        public virtual string ApplDate { get; set; }
        public virtual string ApplTime { get; set; }
        public virtual string ApplNum { get; set; }
        public virtual string PayMethod { get; set; }
        public virtual int TotPrice { get; set; }        
        public virtual string EventCode { get; set; }

        // 모든 결제 수단에 대해 결제 실패시에만 결제 결과 데이터 			
        public virtual string ResultErrorcode { get; set; }
    }

    /// <summary>
    /// 다. 신용카드  결제수단을 이용시에만  결제결과 내용
    /// </summary>
    public class VCARD
    {
        public virtual string Num { get; set; }
        public virtual string Interest { get; set; }
        public virtual string Quota { get; set; }
        public virtual string Code { get; set; }                
        public virtual string BankCode { get; set; }
        public virtual string OrgCurrency { get; set; }
        public virtual string ExchangeRate { get; set; }
        public virtual string OcbNum { get; set; }
        public virtual string OcbSaveApplNum { get; set; }
        public virtual string OcbPayApplNum { get; set; }
        public virtual string OcbApplDate { get; set; }
        public virtual string OcbApplTime { get; set; }
        public virtual string OcbPayPrice { get; set; }
        public virtual string CheckFlag { get; set; }
        public virtual string IspCheckFlag { get; set; }
        public virtual string AuthType { get; set; }
    }

    /// <summary>
    /// 라. 은행계좌이체 결제수단을 이용시에만  결제결과 내용
    /// 오직 은행계좌시에만 실시 현금 영수증 발행이 가능하며, 가상계좌는 상점관리자 화면이나, 독립적인 현금영수증 발행(이니시스 기술자료실) 모듈을 사용하세요
    /// </summary>
    public class DirectBank
    {
        public virtual string AcctBankCode { get; set; }
        public virtual string CshrResultCode { get; set; }
        public virtual string CshrType { get; set; }
    }

    /// <summary>
    /// 마.무통장 입금(가상계좌) 결제수단을 이용시 결과 내용
    /// </summary>
    public class VBank
    {
        public virtual string VactNum { get; set; }
        public virtual string VactBankCode { get; set; }
        public virtual string VactName { get; set; }
        public virtual string VactInputName { get; set; }
        public virtual string VactDate { get; set; }
        public virtual string VactTime { get; set; }              
    }

    /// <summary>
    /// 바. 핸드폰, 전화결제시에만  결제 결과 내용 ( "실패 내역 자세히 보기"에서 필요 , 상점에서는 필요없는 정보임)
    /// </summary>
    public class HPP
    {
        public virtual string GWCode { get; set; }
        public virtual string Num { get; set; }
    }

    /// <summary>
    /// 아. ARS 전화 결제수단을 이용시에만  결제 결과 내용
    /// </summary>
    public class ARSB
    {
        public virtual string Num { get; set; }
    }

    /// <summary>
    /// 자. 받는 전화 결제수단을 이용시에만  결제 결과 내용
    /// </summary>
    public class PHNB
    {
        public virtual string Num { get; set; }
    }

    public  class OCB
    {
        public virtual string Num { get; set; }
        public virtual string SaveApplNum { get; set; }
        public virtual string PayApplNum { get; set; }
        public virtual string PayPrice { get; set; }
    }

    /// <summary>
    /// 차. 문화 상품권 결제수단을 이용시에만  결제 결과 내용
    /// </summary>
    public class CULT
    {
        public virtual string UserId { get; set; }
    }

    /// <summary>
    /// 파. 틴캐쉬 결제수단을 이용시에만  결제 결과 내용	
    /// </summary>
    public class TEEN
    {
        public virtual string Remains { get; set; }
        public virtual string UserId { get; set; }
    }

    /// <summary>
    /// 타.스마트문상(게임 문화 상품권) 결제수단을 이용시에만 결제 결과 내용
    /// </summary>
    public class GAMG
    {
        public virtual string Cnt { get; set; }
        public virtual string Num { get; set; }
        public virtual string Remains { get; set; }
        public virtual string ErrMsg { get; set; }
    }

    /// <summary>
    /// 하. 도서문화 상품권 결제수단을 이용시에만 결제 결과 내용
    /// </summary>
    public class BCSH
    {
        public virtual string UserId { get; set; }
    }

    /// <summary>
    /// 바우처 결제(신용카드 / 신용카드 + 바우처)
    /// </summary>
    public class SSVC
    {
        public virtual int CardApplPrice { get; set; }
        public virtual int ApplPrice { get; set; }
        public virtual int CardPrice { get; set; }
        public virtual int PointPricee { get; set; }
        public virtual int Remain { get; set; }
    }

}
