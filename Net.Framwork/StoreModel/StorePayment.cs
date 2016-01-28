using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    public class StorePaymentT
    {
        public virtual int GoodsPrice { get; set; }
        public virtual COMMON Common { get; set; }
        public virtual APPL Appl { get; set; }
        public virtual VCARD VCard { get; set; }
        public virtual OCB Ocb { get; set; }
        public virtual DirectBank DirectBank { get; set; }
        public virtual VACT Vact { get; set; }
        public virtual HPP Hpp { get; set; }
        public virtual ARSB Arsb { get; set; }
        public virtual PHNB Phnb { get; set; }
        public virtual CULT Cult { get; set; }
        public virtual CSHR Cshr { get; set; }
        public virtual TEEN Teen { get; set; }
        public virtual GAMG Gamg { get; set; }
        public virtual BCSH Bcsh { get; set; }
    }

    /// <summary>
    /// 가.모든 결제 수단에 공통되는 결제 결과 내용
    /// </summary>
    public class COMMON
    {
        public virtual string Tid { get; set; }
        public virtual string Resultcode { get; set; }
        public virtual string ResultMsg { get; set; }
        public virtual string PayMethod { get; set; }
        public virtual string MoId { get; set; }

        // 모든 결제 수단에 대해 결제 실패시에만 결제 결과 데이터 			
        public virtual string ResultErrorcode { get; set; }
    }

    /// <summary>
    /// 나. 신용카드,ISP,핸드폰, 전화 결제, 은행계좌이체, OK CASH BACK Point 결제시에만 결제 결과 내용  (무통장입금 , 문화 상품권 포함)
    /// </summary>
    public class APPL
    {
        public virtual string Date { get; set; }
        public virtual string Time { get; set; }
    }

    /// <summary>
    /// 다. 신용카드  결제수단을 이용시에만  결제결과 내용
    /// </summary>
    public class VCARD
    {
        public virtual string ApplNum { get; set; }
        public virtual string Quota { get; set; }
        public virtual string Interest { get; set; }
        public virtual string Num { get; set; }
        public virtual string Code { get; set; }
        public virtual string BankCode { get; set; }
        public virtual string AuthType { get; set; }
        public virtual string EventCode { get; set; }
    }

    /// <summary>
    /// "신용카드 및 OK CASH BACK 복합결제" 또는"신용카드 지불시에 OK CASH BACK적립"시에 추가되는 내용
    /// </summary>
    public class OCB
    {
        public virtual string ApplTime { get; set; }
        public virtual string SaveApplNum { get; set; }
        public virtual string PayApplNum { get; set; }
        public virtual string ApplDate { get; set; }
        public virtual string Num { get; set; }
        public virtual string CardApplPrice { get; set; }
        public virtual string PayPrice { get; set; }
    }

    /// <summary>
    /// 라. 은행계좌이체 결제수단을 이용시에만  결제결과 내용
    /// 오직 은행계좌시에만 실시 현금 영수증 발행이 가능하며, 가상계좌는 상점관리자 화면이나, 독립적인 현금영수증 발행(이니시스 기술자료실) 모듈을 사용하세요
    /// </summary>
    public class DirectBank
    {
        public virtual string AcctBankCode { get; set; }
        public virtual string RcashRslt { get; set; }
        public virtual string Ruseopt { get; set; }
    }

    /// <summary>
    /// 마.무통장 입금(가상계좌) 결제수단을 이용시 결과 내용
    /// </summary>
    public class VACT
    {
        public virtual string Num { get; set; }
        public virtual string BankCode { get; set; }
        public virtual string Date { get; set; }
        public virtual string Time { get; set; }
        public virtual string InputName { get; set; }
        public virtual string Name { get; set; }
    }

    /// <summary>
    /// 바. 핸드폰, 전화결제시에만  결제 결과 내용 ( "실패 내역 자세히 보기"에서 필요 , 상점에서는 필요없는 정보임)
    /// </summary>
    public class HPP
    {
        public virtual string GWCode { get; set; }

        // 사. 핸드폰 결제수단을 이용시에만  결제 결과 내용
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

    /// <summary>
    /// 차. 문화 상품권 결제수단을 이용시에만  결제 결과 내용
    /// </summary>
    public class CULT
    {
        public virtual string UserId { get; set; }
    }

    /// <summary>
    /// 카. 현금영수증 발급 결과코드 (은행계좌이체시에만 리턴)
    /// </summary>
    public class CSHR
    {
        public virtual string ResultCode { get; set; }
        public virtual string Type { get; set; }
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
    }

    /// <summary>
    /// 하. 도서문화 상품권 결제수단을 이용시에만 결제 결과 내용
    /// </summary>
    public class BCSH
    {
        public virtual string UserId { get; set; }
    }
}
