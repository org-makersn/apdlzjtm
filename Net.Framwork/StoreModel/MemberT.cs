using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Net.Framwork.StoreModel
{
    [Table("MEMBER")]
    public partial class MemberT
    {
        [Key, Column("NO")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NO { get; set; }
        public string ID { get; set; }
        public string BLOG_URL { get; set; }
        public int LEVEL { get; set; }
        public string STATUS { get; set; }
        public string PASSWORD { get; set; }
        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public string CELL_PHONE { get; set; }
        public string URL { get; set; }
        public string SNS_TYPE { get; set; }
        public string SNS_ID { get; set; }
        public string PROFILE_MSG { get; set; }
        public string PROFILE_PIC { get; set; }
        public string COVER_PIC { get; set; }
        public string ALL_IS { get; set; }
        public string REPLE_IS { get; set; }
        public string LIKE_IS { get; set; }
        public string NOTICE_IS { get; set; }
        public string FOLLOW_IS { get; set; }
        public Nullable<System.DateTime> UPD_PASSWORD_DT { get; set; }
        public Nullable<System.DateTime> LAST_LOGIN_DT { get; set; }
        public string LAST_LOGIN_IP { get; set; }
        public Nullable<int> LOGIN_CNT { get; set; }
        public string EMAIL_CERTIFY { get; set; }
        public Nullable<System.DateTime> DEL_DT { get; set; }
        public string DROP_COMMENT { get; set; }
        public string DEL_FLAG { get; set; }
        public Nullable<System.DateTime> REG_DT { get; set; }
        public string REG_ID { get; set; }
        public Nullable<System.DateTime> UPD_DT { get; set; }
        public string UPD_ID { get; set; }
    }
}
