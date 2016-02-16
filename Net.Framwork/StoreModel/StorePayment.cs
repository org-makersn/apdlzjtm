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

        [Column("ORDER_MASTER_NO")]
        public virtual Int64 OrderMasterNo { get; set; }
        [Column("TID")]
        public virtual string Tid { get; set; }
        [Column("RESULT_CODE")]
        public virtual string ResultCode { get; set; }
        [Column("RESULT_MSG")]
        public virtual string ResultMsg { get; set; }
        [Column("MOID")]
        public virtual string MoId { get; set; }
        [Column("APPL_DATE")]
        public virtual string ApplDate { get; set; }
        [Column("APPL_TIME")]
        public virtual string ApplTime { get; set; }
        [Column("APPL_NUM")]
        public virtual string ApplNum { get; set; }
        [Column("PAY_METHOD")]
        public virtual string PayMethod { get; set; }
        [Column("TOT_PRICE")]
        public virtual int TotPrice { get; set; }
        [Column("EVENT_CODE")]
        public virtual string EventCode { get; set; }
        [Column("CARD_NUM")]
        public virtual string CardNum { get; set; }
        [Column("CARD_INTEREST")]
        public virtual string CardInterest { get; set; }
        [Column("CARD_QUOTA")]
        public virtual string CardQuota { get; set; }
        [Column("CARD_CODE")]
        public virtual string CardCode { get; set; }
        [Column("CARD_BANK_CODE")]
        public virtual string CardBankCode { get; set; }
        [Column("ORG_CURRENCY")]
        public virtual string OrgCurrency { get; set; }
        [Column("EXCHANGE_RATE")]
        public virtual string ExchangeRate { get; set; }
        [Column("CARD_OCB_NUM")]
        public virtual string CardOcbNum { get; set; }
        [Column("CARD_OCB_SAVE_APPL_NUM")]
        public virtual string CardOcbSaveApplNum { get; set; }
        [Column("CARD_OCB_PAY_APPL_NUM")]
        public virtual string CardOcbPayApplNum { get; set; }
        [Column("CARD_OCB_APPL_DATE")]
        public virtual string CardOcbApplDate { get; set; }
        [Column("CARD_OCB_PAY_PRICE")]
        public virtual string CardOcbPayPrice { get; set; }
        [Column("CARD_CHECK_FLAG")]
        public virtual string CardCheckFlag { get; set; }
        [Column("ISP_CHECK_FLAG")]
        public virtual string IspCheckFlag { get; set; }
        [Column("ACCT_BANK_CODE")]
        public virtual string AcctBankCode { get; set; }
        [Column("CSHR_RESULT_CODE")]
        public virtual string CshrResultCode { get; set; }
        [Column("CSHR_TYPE")]
        public virtual string CshrType { get; set; }
        [Column("VACT_NUM")]
        public virtual string VactNum { get; set; }
        [Column("VACT_BANK_CODE")]
        public virtual string VactBankCode { get; set; }
        [Column("VACT_NAME")]
        public virtual string VactName { get; set; }
        [Column("VACT_INPUT_NAME")]
        public virtual string VactInputName { get; set; }
        [Column("VACT_DATE")]
        public virtual string VactDate { get; set; }
        [Column("VACT_TIME")]
        public virtual string VactTime { get; set; }
        [Column("HPP_NUM")]
        public virtual string HppNum { get; set; }
        [Column("ARSB_NUM")]
        public virtual string ArsbNum { get; set; }
        [Column("PHNB_NUM")]
        public virtual string PhnbNum { get; set; }
        [Column("OCB_NUM")]
        public virtual string OcbNum { get; set; }
        [Column("OCB_SAVE_APPL_NUM")]
        public virtual string OcbSaveApplNum { get; set; }
        [Column("OCB_PAY_APPL_NUM")]
        public virtual string OcbPayApplNum { get; set; }
        [Column("OCB_PAY_PRICE")]
        public virtual string OcbPayPrice { get; set; }
        [Column("CULT_USER_ID")]
        public virtual string CultUserId { get; set; }
        [Column("TEEN_REMAINS")]
        public virtual string TeenRemains { get; set; }
        [Column("TEEN_USER_ID")]
        public virtual string TeenUserId { get; set; }
        [Column("GAMG_CNT")]
        public virtual string GamgCnt { get; set; }
        [Column("GAMG_NUM")]
        public virtual string GamgNum { get; set; }
        [Column("GAMG_REMAINS")]
        public virtual string GamgRemains { get; set; }
        [Column("GAMG_ERR_MSG")]
        public virtual string GamgErrMsg { get; set; }
        [Column("BCSH_USER_ID")]
        public virtual string BcshUserId{ get; set; }
        [Column("CARD_APPL_PRICE")]
        public virtual int CardApplPrice{ get; set; }
        [Column("SSVC_APPL_PRICE")]
        public virtual Nullable<int> SsvcApplPrice{ get; set; }
        [Column("SSVC_CARD_PRICE")]
        public virtual Nullable<int> SsvcCardPrice { get; set; }
        [Column("SSVC_POINT_PRICE")]
        public virtual Nullable<int> SsvcPointPrice { get; set; }
        [Column("SSVC_REMAIN")]
        public virtual Nullable<int> SsvcRemain { get; set; }
        [Column("REG_DT")]
        public virtual DateTime RegDt{ get; set; }
        [Column("REG_ID")]
        public virtual string RegId{ get; set; }
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

        [Column("RESULT_CODE")]
        public virtual string ResultCode { get; set; }
        [Column("RESULT_MSG")]
        public virtual string ResultMsg { get; set; }
        [Column("CANCEL_DATE")]
        public virtual string CancelDate { get; set; }
        [Column("CANCEL_TIME")]
        public virtual string CancelTime { get; set; }
        [Column("CSHR_CANCEL_NUM")]
        public virtual string CshrCancelNum { get; set; }
        [Column("REG_DT")]
        public virtual DateTime RegDt { get; set; }
        [Column("REG_ID")]
        public virtual string RegId { get; set; }
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
