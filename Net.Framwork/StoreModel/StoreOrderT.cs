﻿using System;
using System.Collections.Generic;
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

    [Table("STORE_ORDER_DETAIL")]
    public class StoreOrderDetailT
    {
        [Key]
        [Column("NO")]
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
        public virtual string ShippingPrice { get; set; }
        [Column("PRODUCT_CNT")]
        public virtual int ProductCnt { get; set; }
        [Column("ORDER_STATUS")]
        public virtual string OrderStatus { get; set; }
        [Column("CANCEL_YN")]
        public virtual string CancelYn { get; set; }
        [Column("GIFT_MSG")]
        public virtual string GigtMsg { get; set; }
        [Column("SLOW_MAKE_YN")]
        public virtual string SlowMakeYn { get; set; }
        [Column("DISCOUNT_NO")]
        public virtual int DiscountNo { get; set; }
        [Column("COMPLETE_DATE")]
        public virtual DateTime CompleteDate { get; set; }
        [Column("IMG_SRC")]
        public virtual string ImgSrc { get; set; }
        [Column("SCALE")]
        public virtual int Scale { get; set; }
        [Column("REG_DT")]
        public virtual DateTime RegDt { get; set; }
        [Column("REG_ID")]
        public virtual string RegId { get; set; }
        [Column("UPD_DT")]
        public virtual DateTime UpdDt { get; set; }
        [Column("UPD_ID")]
        public virtual string UpdId { get; set; }
    }

    public class OrderMaster
    {
        public virtual List<OrderInfo> OrderInfoList { get; set; }
        public virtual StoreCartT StoreCart{get; set;}
        public virtual EncResult EncResult{get; set;}
    }

    [Table("ORDER_INFO")]
    public class OrderInfo
    {
        [Key]
        [Column("CART_NO")]
        public virtual string CartNo { get; set; }

        [Column("PRODUCT_DETAIL_NO")]
        public virtual Int64 ProductDetailNo { get; set; }
        [Column("PRODUCT_NAME")]
        public virtual string ProductName { get; set; }
        [Column("NAME")]
        public virtual string Name { get; set; }
        [Column("FILE_SIZE")]
        public virtual int FIleSize { get; set; }
        [Column("MATERIAL_VOLUME")]
        public virtual string MaterialVolume { get; set; }
        [Column("OBJECT_VOLUME")]
        public virtual string ObjectVolume { get; set; }
        [Column("SIZE_X")]
        public virtual float SIzeX { get; set; }
        [Column("SIZE_Y")]
        public virtual float SizeY { get; set; }
        [Column("TOTAL_PRICE")]
        public virtual int TotalPrice { get; set; }
        [Column("PRODUCT_CNT")]
        public virtual int ProductCnt { get; set; }
        [Column("PAYMENT_PRICE")]
        public virtual int PaymentPrice { get; set; }
        [Column("SHIPPING_PRICE")]
        public virtual int ShippingPrice { get; set; }    
    }

    public class EncResult
    {
        public virtual string resultcode { get; set; }
        public virtual string resultmsg { get; set; }
        public virtual string rn_value { get; set; }
        public virtual string return_enc { get; set; }
        public virtual string ini_certid { get; set; }
        public virtual string ini_encfield { get; set; }
    }
}
