using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.Entity
{
    public class BusManageModel
    {

    }

    [Table("BUS_APPLY_SCHOOL")]
    public partial class BusApplySchoolT
    {
        [Key]
        public virtual int NO { get; set; }

        public virtual string SCHOOL_NAME { get; set; }
        public virtual string SCHOOL_ADDR { get; set; }
        public virtual string MANAGER { get; set; }
        public virtual string MANAGER_EMAIL { get; set; }
        public virtual string MANAGER_TEL { get; set; }
        public virtual string APPLY_PATH { get; set; }
        public virtual string MAKERBUS_YN { get; set; }
        public virtual Nullable<int> PARTICIPATION_COUNT { get; set; }
        public virtual Nullable<int> MODELING_COUNT { get; set; }
        public virtual Nullable<int> SUPPORT_PRINTER_COUNT { get; set; }
        public virtual Nullable<DateTime> EVENT_DATE { get; set; }
        public virtual string START_TIME { get; set; }
        public virtual string MEMO { get; set; }
        public virtual DateTime REG_DT { get; set; }
        public virtual string REG_ID { get; set; }
        public virtual Nullable<DateTime> CHG_DT { get; set; }
        public virtual string CHG_ID { get; set; }
    }

    [Table("BUS_QNA")]
    public partial class BusQnaT
    {
        [Key]
        public int NO { get; set; }
        
        public string CATEGORY { get; set; }
        public string EMAIL { get; set; }
        public string TITLE { get; set; }
        public string CONTENTS { get; set; }
        public DateTime REG_DT { get; set; }
        public string REG_ID { get; set; }
    }

    [Table("BUS_FAQ")]
    public partial class BusFaqT
    {
        [Key]
        public int NO { get; set; }

        public string TITLE { get; set; }
        public string CONTENTS { get; set; }
        public DateTime REG_DT { get; set; }
        public string REG_ID { get; set; }
        public Nullable<DateTime> CHG_DT { get; set; }
        public string CHG_ID { get; set; }
    }

    [Table("BUS_PARTNERSHIP_QNA")]
    public partial class BusPartnershipQnaT
    {
        [Key]
        public int NO { get; set; }

        public string EMAIL { get; set; }
        public string TITLE { get; set; }
        public string CONTENTS { get; set; }
        public DateTime REG_DT { get; set; }
        public string REG_ID { get; set; }
    }

    [Table("BUS_PARTNER")]
    public partial class BusPartnerT
    {
        [Key]
        public int NO { get; set; }

        public string PARTNER_NAME { get; set; }
        public string LOGO_IMAGE { get; set; }
        public DateTime REG_DT { get; set; }
        public string REG_ID { get; set; }
        public Nullable<DateTime> CHG_DT { get; set; }
        public string CHG_ID { get; set; }
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
