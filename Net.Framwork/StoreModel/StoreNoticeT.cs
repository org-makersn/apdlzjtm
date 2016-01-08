using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_NOTICE")]
    public class StoreNoticeT
    {
        [Key]
        [Column("NO")]
        public virtual int No { get; set; }

        [Column("PRODUCT_NO")]
        public virtual int ProductNo { get; set; }        
        [Column("MEMBER_NO")]
        public virtual int MemberNo { get; set; }
        [Column("MEMBER_NO_REF")]
        public virtual int MemberNoRef { get; set; }
        [Column("REF_NO")]
        public virtual int RefNo { get; set; }
        [Column("TYPE")]
        public virtual string Type { get; set; }
        [Column("CONTENTS")]
        public virtual string Contents { get; set; }
        [Column("CHECK_YN")]
        public virtual string CheckYn { get; set; }
        [Column("IS_NEW_YN")]
        public virtual string IsNewYn { get; set; }
        [Column("REG_DT")]
        public virtual DateTime RegDt { get; set; }
        [Column("REG_ID")]
        public virtual string RegId { get; set; }
    }
}
