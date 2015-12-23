using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_PRODUCT_UPDATE_HISTORY")]
    public class StoreProductUpdateHistoryT
    {
        [Key]
        [Column("NO")]
        public virtual long No { get; set; }

        [Column("PRODUCT_NO")]
        public virtual long ProductNo { get; set; }
        [Column("CONTENT")]
        public virtual string Content { get; set; }
        [Column("SUCCESS_FLAG")]
        public virtual char SuccessFlag { get; set; }
        [Column("ERROR_MSG")]
        public virtual string ErrorMsg { get; set; }
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
