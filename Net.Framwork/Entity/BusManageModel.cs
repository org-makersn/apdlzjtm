﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.Entity
{
    public class BusManageModel
    {

    }

    [Table("BUS_APPLY_SCHOOL")]
    public class BusApplySchool
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NO { get; set; }

        public string SCHOOL_NAME { get; set; }
        public string SCHOOL_ADDR { get; set; }
        public string MANAGER { get; set; }
        public string MANAGER_EMAIL { get; set; }
        public string MANAGER_TEL { get; set; }
        public string APPLY_PATH { get; set; }
        public string MAKERBUS_YN { get; set; }
        public Nullable<int> PARTICIPATION_COUNT { get; set; }
        public Nullable<int> MODELING_COUNT { get; set; }
        public Nullable<int> SUPPORT_PRINTER_COUNT { get; set; }
        public Nullable<DateTime> EVENT_DATE { get; set; }
        public string START_TIME { get; set; }
        public string MEMO { get; set; }
        public DateTime REG_DT { get; set; }
        public string REG_ID { get; set; }
        public Nullable<DateTime> CHG_DT { get; set; }
        public string CHG_ID { get; set; }
    }

    [Table("BUS_QNA")]
    public class BusQna
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NO { get; set; }
        
        public string CATEGORY { get; set; }
        public string EMAIL { get; set; }
        public string TITLE { get; set; }
        public string CONTENTS { get; set; }
        public DateTime REG_DT { get; set; }
        public string REG_ID { get; set; }
    }

    [Table("BUS_PARTNERSHIP_QNA")]
    public class BusPartnershipQna
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NO { get; set; }

        public string TITLE { get; set; }
        public string CONTENTS { get; set; }
        public DateTime REG_DT { get; set; }
        public string REG_ID { get; set; }
    }

    [Table("BUS_PARTNERSHIP_FAQ")]
    public class BusPartnershipFaq
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SEQ { get; set; }

        public string TITLE { get; set; }
        public string CONTENTS { get; set; }
        public DateTime REG_DT { get; set; }
        public string REG_ID { get; set; }
    }
    
    /// <summary>
    /// 진행현황
    /// </summary>
    [Table("BUS_HISTORY")]
    public partial class BusHistory
    {
        [Key]
        public int NO { get; set; }

        [Required(ErrorMessage = "진행현황 제목을 입력해주세요.")]
        public string TITLE { get; set; }

        [Required(ErrorMessage = "진행현황 일자를 입력해주세요.")]
        public string PROGRESS_DT { get; set; }

        public string USE_YN { get; set; }
        public virtual DateTime REG_DT { get; set; }
        public virtual string REG_ID { get; set; }
        public virtual Nullable<DateTime> UPD_DT { get; set; }
        public virtual string UPD_ID { get; set; }
    }
    
    /// <summary>
        /// 블로그
    /// </summary>
    [Table("BUS_BLOG")]
    public partial class BusBlog
    {
        [Key]
        public long NO { get; set; }

        [Required(ErrorMessage = "블로그 제목을 입력해주세요.")]
        public string BLOG_TITLE { get; set; }

        [Required(ErrorMessage = "블로그 내용을 입력해주세요.")]
        [Column("BLOG_CONTENTS", TypeName = "text")]
        [MaxLength]
        public string BLOG_CONTENTS { get; set; }

        public string THUMB_NAME { get; set; }
        public string THUMB_RENAME { get; set; }
        public int VIEW_CNT { get; set; }
        public int AUTHOR { get; set; }
        public string USE_YN { get; set; }
        public virtual DateTime REG_DT { get; set; }
        public virtual string REG_ID { get; set; }
        public virtual Nullable<DateTime> UPD_DT { get; set; }
        public virtual string UPD_ID { get; set; }
    }

    public partial class MakerBusState
    {
        public int SchoolCnt { get; set; }
        public int StudentCnt { get; set; }
        public int ModelingCnt { get; set; }
        public int PrinterCnt { get; set; }
    }
}
