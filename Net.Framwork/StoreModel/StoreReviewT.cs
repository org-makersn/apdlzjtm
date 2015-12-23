using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_REVIEW")]
    public class StoreReviewT
    {
        [Key]
        [Column("NO")]
        public virtual long No { get; set; }

        [Column("PRODUCT_NO")]
        public virtual long ProductNo { get; set; }
        [Column("COMMENT")]
        public virtual string Comment { get; set; }
        [Column("SCORE")]
        public virtual float Score { get; set; } //int가 맞지 않나?
        [Column("IMAGE_NAME")]
        public virtual string ImageName { get; set; }
        [Column("PARENT_NO")]
        public virtual long ParentNo { get; set; }
        [Column("DEPTH")]
        public virtual int Depth { get; set; }
        [Column("VISIBILITY_YN")]
        public virtual char VisibilityYn { get; set; }
        [Column("MEMBER_NO")]
        public virtual int MemberNo { get; set; }
        [Column("REG_IP")]
        public virtual string RegIp { get; set; }
        [Column("REG_DT")]
        public virtual DateTime RegDt { get; set; }
        [Column("REG_ID")]
        public virtual string RegId { get; set; }
    }
}
