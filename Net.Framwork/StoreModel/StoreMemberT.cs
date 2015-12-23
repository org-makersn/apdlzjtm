using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_MEMBER")]
    public class StoreMemberT
    {
        [Key]
        [Column("NO")]
        public virtual int No { get; set; }

        [Column("MEMBER_NO")]
        public virtual int MemberNo { get; set; }
        [Column("STORE_NAME")]
        public virtual string StoreName { get; set; }
        [Column("HOME_PHONE")]
        public virtual string HomePhone { get; set; }
        [Column("CELL_PHONE")]
        public virtual string CellPhone { get; set; }
        [Column("STORE_PROFILE_MSG")]
        public virtual string StoreProfileMsg { get; set; }
        [Column("SOTRE_URL")]
        public virtual string StoreUrl { get; set; }
        [Column("BANK_NAME")]
        public virtual string BankName { get; set; }
        [Column("BANK_USER_NAME")]
        public virtual string BankUserName { get; set; }
        [Column("BANK_ACCOUNT")]
        public virtual string BankAccount { get; set; }
        [Column("DEL_YN")]
        public virtual char DelYn { get; set; }
        [Column("REG_ID")]
        public virtual string RegId { get; set; }
        [Column("REG_DT")]
        public virtual DateTime RegDt { get; set; }
        [Column("UPD_ID")]
        public virtual string UpdId { get; set; }
        [Column("UPD_DT")]
        public virtual DateTime UpdDt { get; set; }
    }
}
