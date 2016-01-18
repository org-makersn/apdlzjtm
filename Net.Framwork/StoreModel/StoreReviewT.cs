using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    //[Table("STORE_REVIEW")]
    //public class StoreReviewT
    //{
    //    [Key]
    //    [Column("NO")]
    //    public virtual long No { get; set; }

    //    [Column("PRODUCT_NO")]
    //    public virtual long ProductNo { get; set; }
    //    [Column("COMMENT")]
    //    public virtual string Comment { get; set; }
    //    [Column("SCORE")]
    //    public virtual double Score { get; set; } //int가 맞지 않나?
    //    [Column("IMAGE_NAME")]
    //    public virtual string ImageName { get; set; }
    //    [Column("PARENT_NO")]
    //    public virtual long ParentNo { get; set; }
    //    [Column("DEPTH")]
    //    public virtual int Depth { get; set; }
    //    [Column("VISIBILITY_YN")]
    //    public virtual string VisibilityYn { get; set; }
    //    [Column("MEMBER_NO")]
    //    public virtual int MemberNo { get; set; }
    //    [Column("REG_IP")]
    //    public virtual string RegIp { get; set; }
    //    [Column("REG_DT")]
    //    public virtual DateTime RegDt { get; set; }
    //    [Column("REG_ID")]
    //    public virtual string RegId { get; set; }
    //}

    [Table("STORE_REVIEW")]
    public partial class StoreReviewT
    {
        [Key, Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long NO { get; set; }
        public long PRODUCT_NO { get; set; }
        public string COMMENT { get; set; }
        public double SCORE { get; set; }
        public string IMAGE_NAME { get; set; }
        public long PARENT_NO { get; set; }
        public int DEPTH { get; set; }
        public string VISIBILITY_YN { get; set; }
        public int MEMBER_NO { get; set; }
        public string REG_IP { get; set; }
        public System.DateTime REG_DT { get; set; }
        public string REG_ID { get; set; }
        public Nullable<System.DateTime> UPD_DT { get; set; }
        public string UPD_ID { get; set; }
    }
}
