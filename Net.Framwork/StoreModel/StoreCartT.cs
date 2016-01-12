using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Net.Framework.StoreModel
{
    [Table("STORE_CART")]
    public class StoreCartT
    {
        [Key]
        [Column("NO")]
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

        [NotMapped]
        public virtual string Name { get; set; }
        [NotMapped]
        public virtual string ProductName { get; set; }
        [NotMapped]
        public virtual int TotalPrice { get; set; }
    }
}
