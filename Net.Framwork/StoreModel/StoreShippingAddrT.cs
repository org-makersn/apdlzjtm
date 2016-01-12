using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_SHIPPING_ADDR")]
    public class StoreShippingAddrT
    {
        [Key]
        [Column("NO")]
        public virtual Int64 No { get; set; }

        [Column("MEMBER_NO")]
        public virtual Int64 MemberNo { get; set; }
        [Column("ADDR1")]
        public virtual string Addr1 { get; set; }
        [Column("ADDR2")]
        public virtual string Addr2 { get; set; }
        [Column("POST_NO")]
        public virtual string PostNo { get; set; }
        [Column("DEFAULT_YN")]
        public virtual string DefaultYn { get; set; }
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
