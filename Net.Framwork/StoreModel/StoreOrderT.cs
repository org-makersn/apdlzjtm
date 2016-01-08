using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_ORDER")]
    public class StoreOrderT
    {
        [Key]
        [Column("NO")]
        public virtual Int64 No { get; set; }

        [Column("OID")]
        public virtual string Oid { get; set; }
        [Column("MID")]
        public virtual string Mid { get; set; }
        [Column("INI_RN")]
        public virtual string IniRn { get; set; }
        [Column("PRICE")]
        public virtual decimal Price { get; set; }
        [Column("GOOD_NAME")]
        public virtual string GoodName { get; set; }
        [Column("CURRENCY")]
        public virtual string Currency { get; set; }
        [Column("URL")]
        public virtual string Url { get; set; }
        [Column("CARD_CODE")]
        public virtual string CardCode { get; set; }
        [Column("PAY_METHOD")]
        public virtual string PayMethod { get; set; }
        [Column("ENCRYPTED")]
        public virtual string Encrypted { get; set; }
        [Column("SESSION_KEY")]
        public virtual string SessionKey { get; set; }
        [Column("TYPE")]
        public virtual string Type { get; set; }
        [Column("NO_INTEREST")]
        public virtual string NoInterest { get; set; }
        [Column("QUOTA_BASE")]
        public virtual string QoutaBase { get; set; }
        [Column("BUYER_NAME")]
        public virtual string BuyerName { get; set; }
        [Column("BUYER_TEL")]
        public virtual string BuyerTel { get; set; }
        [Column("BUYER_EMAIL")]
        public virtual string BuyerEmail { get; set; }
        [Column("RECV_NAME")]
        public virtual string RecvName { get; set; }
        [Column("RECV_TEL")]
        public virtual string RecvTel { get; set; }
        [Column("RECV_ADDR")]
        public virtual string RecvAddr { get; set; }
        [Column("RECV_POST_NUM")]
        public virtual string RecvPostNum { get; set; }
        [Column("RECV_MSG")]
        public virtual string RecvMsg { get; set; }
        [Column("JOIN_CARD")]
        public virtual string JoinCard { get; set; }
        [Column("JOIN_EXPIRE")]
        public virtual string JoinExpire { get; set; }
        [Column("USER_ID")]
        public virtual string UserId { get; set; }
        [Column("TAX")]
        public virtual string Tax { get; set; }
        [Column("TAX_FREE")]
        public virtual string TaxFree { get; set; }
        [Column("MEMBER_NO")]
        public virtual int MemberNo { get; set; }
        [Column("ORDER_DATE")]
        public virtual DateTime OrderDate { get; set; }
        [Column("PAYMENT_STATUS")]
        public virtual string PaymentStatus { get; set; }
        [Column("ADDR1")]
        public virtual string Addr1 { get; set; }
        [Column("ADDR2")]
        public virtual string Addr2 { get; set; }
        [Column("POST_NO")]
        public virtual string PostNo { get; set; }
        [Column("REG_DT")]
        public virtual DateTime RegDt { get; set; }
        [Column("REG_ID")]
        public virtual string RegId { get; set; }
        [Column("UPD_DT")]
        public virtual DateTime UpdDt { get; set; }
        [Column("UPD_ID")]
        public virtual string UpdId { get; set; }
    }
}
