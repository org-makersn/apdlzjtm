using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Net.Framework.StoreModel
{
    [Table("STORE_CART")]
    public class StoreCartT
    {
        [Key, Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual Int64 No { get; set; }

        [Column("CART_NO")]
        public virtual string CartNo { get; set; }
        [Column("MEMBER_NO")]
        public virtual int MemberNo { get; set; }
        [Column("PRODUCT_DETAIL_NO")]
        public virtual Int64 ProductDetailNo { get; set; }
        [Column("PRODUCT_CNT")]
        public virtual int ProductCnt { get; set; }
        [Column("ORDER_YN")]
        public virtual string OrderYn { get; set; }
        [Column("REG_DT")]
        public virtual DateTime RegDt { get; set; }
        [Column("REG_ID")]
        public virtual string RegId { get; set; }
        [Column("UPD_DT")]
        public virtual Nullable<DateTime> UpdDt { get; set; }
        [Column("UPD_ID")]
        public virtual string UpdId { get; set; }
    }

    public class StoreCartInfo
    {
        public virtual Int64 No { get; set; }
        public virtual string CART_NO { get; set; }
        public virtual int MEMBER_NO { get; set; }
        public virtual string NAME { get; set; }
        public virtual string PRODUCT_NAME { get; set; }
        public virtual int TOTAL_PRICE { get; set; }
        public virtual Int64 PRODUCT_DETAIL_NO { get; set; }
        public virtual string ORDER_YN { get; set; }
        public virtual int PRODUCT_CNT { get; set; }
        public virtual DateTime REG_DT { get; set; }
        public virtual string REG_ID { get; set; }
        public virtual Nullable<DateTime> UPD_DT { get; set; }
        public virtual string UPD_ID { get; set; }
    }
}
