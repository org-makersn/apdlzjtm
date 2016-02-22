using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.Entity
{
    public class BusManageModel
    {

    }

    [Table("BUS_APPLY_SCHOOL")]
    public class BUS_APPLY_SCHOOL
    {
 
    }

    /// <summary>
    /// 진행현황
    /// </summary>
    //[Table("BUS_HISTORY")]
    public partial class BUS_HISTORY
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
    //[Table("BUS_BLOG")]
    public partial class BUS_BLOG
    {
        [Key]
        public int NO { get; set; }

        [Required(ErrorMessage = "블로그 제목을 입력해주세요.")]
        public string BLOG_TITLE { get; set; }

        [Required(ErrorMessage = "블로그 내용을 입력해주세요.")]
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
}
