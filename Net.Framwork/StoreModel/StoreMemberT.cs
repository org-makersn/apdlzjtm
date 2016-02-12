using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_MEMBER")]
    public partial class StoreMemberT
    {
        [Key]
        [Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int No { get; set; }

        [Column("MEMBER_NO")]
        [Required]
        public int MemberNo { get; set; }
        [Column("STORE_NAME")]
        [Required]
        public string StoreName { get; set; }
        [Column("OFFICE_PHONE")]
        [Required]
        public string OfficePhone { get; set; }
        [Column("CELL_PHONE")]
        public string CellPhone { get; set; }
        [Column("STORE_PROFILE_MSG")]
        public string StoreProfileMsg { get; set; }
        [Column("STORE_URL")]
        public string StoreUrl { get; set; }
        [Column("BANK_NAME")]
        public string BankName { get; set; }
        [Column("BANK_USER_NAME")]
        public string BankUserName { get; set; }
        [Column("BANK_ACCOUNT")]
        public string BankAccount { get; set; }
        [Column("DEL_YN")]
        public string DelYn { get; set; }
        [Column("REG_ID")]
        public string RegId { get; set; }
        [Column("REG_DT")]
        public DateTime RegDt { get; set; }
        [Column("UPD_ID")]
        public string UpdId { get; set; }
        [Column("UPD_DT")]
        public Nullable<System.DateTime> UpdDt { get; set; }
    }

    //[Table("STORE_MEMBER")]
    //public partial class StoreMemberT
    //{
    //    [Key, Column("NO")]
    //    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    //    public int NO { get; set; }
    //    public int MEMBER_NO { get; set; }
    //    public string STORE_NAME { get; set; }
    //    public string OFFICE_PHONE { get; set; }
    //    public string CELL_PHONE { get; set; }
    //    public string STORE_PROFILE_MSG { get; set; }
    //    public string STORE_URL { get; set; }
    //    public string BANK_NAME { get; set; }
    //    public string BANK_USER_NAME { get; set; }
    //    public string BANK_ACCOUNT { get; set; }
    //    public string DEL_YN { get; set; }
    //    public System.DateTime REG_DT { get; set; }
    //    public string REG_ID { get; set; }
    //    public Nullable<System.DateTime> UPD_DT { get; set; }
    //    public string UPD_ID { get; set; }
    //}
}
