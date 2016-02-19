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

        public virtual string OID { get; set; }
        public virtual string CART_NO { get; set; }
        public virtual string MID { get; set; }
        public virtual int PRICE { get; set; }
        public virtual string GOOD_NAME { get; set; }
        public virtual string PAY_METHOD { get; set; }
        public virtual string TYPE { get; set; }
        public virtual string BUYER_NAME { get; set; }
        public virtual string BUYER_TEL { get; set; }
        public virtual string BUYER_EMAIL { get; set; }
        public virtual string RECV_NAME { get; set; }
        public virtual string RECV_TEL { get; set; }
        public virtual string RECV_TEL2 { get; set; }
        public virtual Int64 SHIPPING_ADDR_NO { get; set; }
        public virtual string RECV_MSG { get; set; }
        public virtual string USER_ID { get; set; }
        public virtual int MEMBER_NO { get; set; }
        public virtual int SELLER_NO { get; set; }
        public virtual DateTime ORDER_DATE { get; set; }
        public virtual string ORDER_STATUS { get; set; }
        public virtual string PAYMENT_STATUS { get; set; }
        public virtual string SHIPPING_STATUS { get; set; }
        public virtual int SHIPPING_COST { get; set; }
        public virtual string SLOW_MAKE_YN { get; set; }
        public virtual DateTime REG_DT { get; set; }
        public virtual string REG_ID { get; set; }
        public virtual Nullable<DateTime> UPD_DT { get; set; }
        public virtual string UPD_ID { get; set; }
    }

    [Table("STORE_ORDER_DETAIL")]
    public class StoreOrderDetailT
    {
        [Key, Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual Int64 No { get; set; }

        public virtual Int64 ORDER_MASTER_NO { get; set; }
        public virtual Int64 ITEM_NO { get; set; }
        public virtual int ITEM_PRICE { get; set; }
        public virtual int ITEM_CNT { get; set; }
        public virtual string PRINTING_STATUS { get; set; }
        public virtual Nullable<DateTime> PRINTING_COMPLETE_DATE { get; set; }
        public virtual string GIFT_MSG { get; set; }        
        public virtual DateTime REG_DT { get; set; }
        public virtual string REG_ID { get; set; }
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
        public virtual List<OrderGroupItem> OrderGroupItemList { get; set; }
        public virtual StoreCartT StoreCart{get; set;}
        public virtual EncResult EncResult{get; set;}
    }

    public class OrderInfo
    {
        public virtual string CART_NO { get; set; }
        public virtual Int64 ITEM_NO { get; set; }
        public virtual string ITEM_NAME { get; set; }
        public virtual string NAME { get; set; }
        public virtual int BASE_PRICE { get; set; }
        public virtual int ITEM_CNT { get; set; }
        public virtual int PAYMENT_PRICE { get; set; }
        public virtual Nullable<int> SHIPPING_COST { get; set; }
        public virtual string RENAME { get; set; }
        public virtual string STORE_URL { get; set; }
        public virtual string STORE_NAME { get; set; }
        public virtual string STORE_PROFILE_MSG { get; set; }
        public virtual int STORE_NO { get; set; }
    }

    public class OrderGroupItem
    {
        public virtual int STORE_NO { get; set; }
        public virtual string STORE_NAME { get; set; }
        public virtual int PAY_PRICE { get; set; }
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

    #region 우편번호 API 관련 entity

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
        public virtual int TotalPage { get; set; }
        public virtual int TotalCount { get; set; }
        public virtual int CurrentPage { get; set; }
        public virtual int PageSize { get; set; }
    }

    #endregion

    public class ContractDetail
    {
        public virtual Int64 NO { get; set; }
        public virtual string ITEM_NAME { get; set; }
        public virtual int BASE_PRICE { get; set; }
        public virtual int ITEM_CNT { get; set; }
        public virtual int PAY_PRICE { get; set; }               
        public virtual string PRINTING_STATUS { get; set; }
    }

    public class OrderCancel
    {
        public virtual string ResultCode { get; set; }
        public virtual string ResultMsg { get; set; }
        public virtual string CancelDate { get; set; }
        public virtual string CancelTime { get; set; }
        public virtual string CshrCancelNum { get; set; }
    }
}
