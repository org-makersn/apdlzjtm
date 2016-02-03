using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_ORDER")]
    public class StoreOrderT
    {
        [Key, Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual Int64 No { get; set; }

        [Column("OID")]
        public virtual string Oid { get; set; }
        [Column("CART_NO")]
        public virtual string CartNo { get; set; }
        [Column("MID")]
        public virtual string Mid { get; set; }
        [Column("PRICE")]
        public virtual int Price { get; set; }
        [Column("GOOD_NAME")]
        public virtual string GoodName { get; set; }
        [Column("PAY_METHOD")]
        public virtual string PayMethod { get; set; }
        [Column("TYPE")]
        public virtual string Type { get; set; }
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
        [Column("RECV_TEL2")]
        public virtual string RecvTel2 { get; set; }
        [Column("SHIPPING_ADDR_NO")]
        public virtual Int64 ShippingAddrNo { get; set; }
        [Column("RECV_MSG")]
        public virtual string RecvMsg { get; set; }
        [Column("USER_ID")]
        public virtual string UserId { get; set; }
        [Column("MEMBER_NO")]
        public virtual int MemberNo { get; set; }
        [Column("ORDER_DATE")]
        public virtual DateTime OrderDate { get; set; }
        [Column("PAYMENT_STATUS")]
        public virtual string PaymentStatus { get; set; }
        [Column("SHIPPING_PRICE")]
        public virtual int ShippingPrice { get; set; }
        [Column("SLOW_MAKE_YN")]
        public virtual string SlowMakeYn { get; set; }
        [Column("REG_DT")]
        public virtual DateTime RegDt { get; set; }
        [Column("REG_ID")]
        public virtual string RegId { get; set; }
        [Column("UPD_DT")]
        public virtual Nullable<DateTime> UpdDt { get; set; }
        [Column("UPD_ID")]
        public virtual string UpdId { get; set; }
    }

    [Table("STORE_ORDER_DETAIL")]
    public class StoreOrderDetailT
    {
        [Key, Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual Int64 No { get; set; }

        [Column("ORDER_MASTER_NO")]
        public virtual Int64 OrderMasterNo { get; set; }
        [Column("PRODUCT_DETAIL_NO")]
        public virtual Int64 ProductDetailNo { get; set; }
        [Column("PRINTING_COM_NO")]
        public virtual Int64 PrintingComNo { get; set; }
        [Column("PRODUCT_PRICE")]
        public virtual int ProductPrice { get; set; }
        [Column("SHIPPING_PRICE")]
        public virtual int ShippingPrice { get; set; }
        [Column("PRODUCT_CNT")]
        public virtual int ProductCnt { get; set; }
        [Column("ORDER_STATUS")]
        public virtual string OrderStatus { get; set; }
        [Column("CANCEL_YN")]
        public virtual string CancelYn { get; set; }
        [Column("GIFT_MSG")]
        public virtual string GigtMsg { get; set; }
        [Column("COMPLETE_DATE")]
        public virtual Nullable<DateTime> CompleteDate { get; set; }
        [Column("REG_DT")]
        public virtual DateTime RegDt { get; set; }
        [Column("REG_ID")]
        public virtual string RegId { get; set; }
    }

    public class OrderMaster
    {
        public virtual string Mid { get; set; }
        public virtual string Oid { get; set; }
        public virtual string Currency { get; set; }
        public virtual int TotalPrice { get; set; }
        public virtual string BuyerName { get; set; }
        public virtual string GoodsName { get; set; }
        public virtual List<OrderInfo> OrderInfoList { get; set; }
        public virtual StoreCartT StoreCart{get; set;}
        public virtual EncResult EncResult{get; set;}
    }

    public class OrderInfo
    {
        public virtual string CART_NO { get; set; }
        public virtual Int64 PRODUCT_DETAIL_NO { get; set; }
        public virtual string PRODUCT_NAME { get; set; }
        public virtual string NAME { get; set; }
        public virtual double FILE_SIZE { get; set; }
        public virtual double MATERIAL_VOLUME { get; set; }
        public virtual double OBJECT_VOLUME { get; set; }
        public virtual double SIZE_X { get; set; }
        public virtual double SIZE_Y { get; set; }
        public virtual double SIZE_Z { get; set; }
        public virtual int TOTAL_PRICE { get; set; }
        public virtual int PRODUCT_CNT { get; set; }
        public virtual int PAYMENT_PRICE { get; set; }
        public virtual int SHIPPING_PRICE { get; set; }    
    }

    public class EncResult
    {
        public virtual string ResultCode { get; set; }
        public virtual string ResultMsg { get; set; }
        public virtual string RnValue { get; set; }
        public virtual string ReturnEnc { get; set; }
        public virtual string IniCertId { get; set; }
        public virtual string IniEncField { get; set; }
    }

    public class PostAddressInfo
    {
        public virtual CmmMsgHeader CmmMsgHeader { get; set; }
        public virtual List<NewAddressListAreaCdSearchAll> AddressList { get; set; }
    }

    public class CmmMsgHeader
    {
        public virtual string RequestMsgId { get; set; }
        public virtual string ResponseMsgId { get; set; }
        public virtual string ResponseTime { get; set; }
        public virtual string SuccessYn { get; set; }
        public virtual string ReturnCode { get; set; }
        public virtual string ErrMsg { get; set; }
        public virtual string TotalCount { get; set; }
        public virtual string CountPerPage { get; set; }
        public virtual string TotalPage { get; set; }
        public virtual string CurrentPage { get; set; }    
    }

    public class NewAddressListAreaCdSearchAll
    {
        public virtual string ZipNo { get; set; }
        public virtual string LnmAdres { get; set; }
        public virtual string RnAdres { get; set; }
    }

    public class BoardInfo
    {
        public int TotalPage { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
