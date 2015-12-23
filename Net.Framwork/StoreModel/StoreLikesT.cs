using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_LIKES")]
    public class StoreLikesT
    {
        [Key]
        [Column("NO")]
        public virtual int No { get; set; }

        [Column("MEMBER_NO")]
        public virtual int MemberNo{ get; set; }
        [Column("PRODUCT_NO")]
        public virtual int ProductNo { get; set; }
        [Column("REG_DT")]
        public virtual DateTime RegDt{get;set;}
        [Column("REG_ID")]
        public virtual string RegId { get; set; }
        [Column("REG_IP")]
        public virtual string RegIp { get; set; }

    }
}
