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

        public virtual string CART_NO { get; set; }
        public virtual int MEMBER_NO { get; set; }
        public virtual Int64 ITEM_NO { get; set; }
        public virtual int ITEM_CNT { get; set; }
        public virtual string ORDER_YN { get; set; }
        public virtual DateTime REG_DT { get; set; }
        public virtual string REG_ID { get; set; }
        public virtual Nullable<DateTime> UPD_DT { get; set; }
        public virtual string UPD_ID { get; set; }
    }

    public class StoreCartInfo
    {
        public virtual Int64 NO { get; set; }
        public virtual string CART_NO { get; set; }
        public virtual int MEMBER_NO { get; set; }
        public virtual string NAME { get; set; }
        public virtual string ITEM_NAME { get; set; }
        public virtual int BASE_PRICE { get; set; }
        public virtual Int64 ITEM_NO { get; set; }
        public virtual string ORDER_YN { get; set; }
        public virtual int ITEM_CNT { get; set; }
        public virtual DateTime REG_DT { get; set; }
        public virtual string REG_ID { get; set; }
        public virtual Nullable<DateTime> UPD_DT { get; set; }
        public virtual string UPD_ID { get; set; }
    }
}
