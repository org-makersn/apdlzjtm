using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Net.Framework.StoreModel
{
    [Table("MEMBER")]
    public partial class MemberExT
    {
        [Key, Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int No { get; set; }

        [Column("ID")]
        public string Id { get; set; }

        [Column("BLOG_URL")]
        public string BlogUrl { get; set; }

        [Column("LEVEL")]
        public int Level { get; set; }

        [Column("STATUS")]
        public string Status { get; set; }

        [Column("PASSWORD")]
        public string Password { get; set; }

        [Column("NAME")]
        public string Name { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; }

        [Column("CELL_PHONE")]
        public string CellPhone { get; set; }

        [Column("URL")]
        public string Url { get; set; }

        [Column("SNS_TYPE")]
        public string SnsType { get; set; }

        [Column("SNS_ID")]
        public string SnsId { get; set; }

        [Column("PROFILE_MSG")]
        public string ProfileMsg { get; set; }

        [Column("PROFILE_PIC")]
        public string ProfilePic { get; set; }

        [Column("COVER_PIC")]
        public string CoverPic { get; set; }

        [Column("ALL_IS")]
        public string Allis { get; set; }

        [Column("REPLE_IS")]
        public string Repleis { get; set; }

        [Column("LIKE_IS")]
        public string Likeis { get; set; }

        [Column("NOTICE_IS")]
        public string Noticeis { get; set; }

        [Column("FOLLOW_IS")]
        public string Followis { get; set; }

        [Column("UPD_PASSWORD_DT")]
        public Nullable<System.DateTime> UpdPasswordDt { get; set; }

        [Column("LAST_LOGIN_DT")]
        public Nullable<System.DateTime> LastLoginDt { get; set; }

        [Column("LAST_LOGIN_IP")]
        public string LastLoginIp { get; set; }

        [Column("LOGIN_CNT")]
        public Nullable<int> LoginCnt { get; set; }

        [Column("EMAIL_CERTIFY")]
        public string EmailCertify { get; set; }

        [Column("DEL_DT")]
        public Nullable<System.DateTime> DelDt { get; set; }

        [Column("DROP_COMMENT")]
        public string DropComment { get; set; }

        [Column("DEL_FLAG")]
        public string DelFlag { get; set; }

        [Column("REG_DT")]
        public Nullable<System.DateTime> RegDt { get; set; }

        [Column("REG_ID")]
        public string RegId { get; set; }

        [Column("UPD_DT")]
        public Nullable<System.DateTime> UpdDt { get; set; }

        [Column("UPD_ID")]
        public string UpdId { get; set; }
    }
}
