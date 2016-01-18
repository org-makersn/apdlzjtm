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
        public virtual Int64 NO { get; set; }

        [Column("CART_NO")]
        public virtual string CART_NO { get; set; }
        [Column("MEMBER_NO")]
        public virtual int MEMBER_NO { get; set; }
        [Column("PRODUCT_DETAIL_NO")]
        public virtual Int64 PRODUCT_DETAIL_NO { get; set; }
        [Column("PRODUCT_CNT")]
        public virtual int PRODUCT_CNT { get; set; }
        [Column("ORDER_YN")]
        public virtual string ORDER_YN { get; set; }
        [Column("REG_DT")]
        public virtual DateTime REG_DT { get; set; }
        [Column("REG_ID")]
        public virtual string REG_ID { get; set; }
        [Column("UPD_DT")]
        public virtual Nullable<DateTime> UPD_DT { get; set; }
        [Column("UPD_ID")]
        public virtual string UPD_ID { get; set; }
        [Column("NAME")]
        public virtual string NAME { get; set; }
        [Column("PRODUCT_NAME")]
        public virtual string PRODUCT_NAME { get; set; }
        [Column("TOTAL_PRICE")]
        public virtual int TOTAL_PRICE { get; set; }
    }
}
