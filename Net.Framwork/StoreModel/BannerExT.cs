using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("BANNER")]
    public partial class BannerExT
    {
        [Key]
        [Column("NO")]
        public int No { get; set; }
        [Column("TYPE")]
        public int Type { get; set; }
        [Column("TITLE")]
        public string Title { get; set; }
        [Column("PUBLISH_YN")]
        public string PublishYn { get; set; }
        [Column("OPENER_YN")]
        public string OpenerYn { get; set; }
        [Column("LINK")]
        public string Link { get; set; }
        [Column("SOURCE")]
        public string Source { get; set; }
        [Column("TERM")]
        public string Term { get; set; }
        [Column("IMAGE")]
        public string Image { get; set; }
        [Column("PRIORITY")]
        public int Priority { get; set; }
        [Column("REG_DT")]
        public DateTime RegDt { get; set; }
        [Column("REG_ID")]
        public string RegId { get; set; }
        [Column("UPD_DT")]
        public Nullable<DateTime> UpdDt { get; set; }
        [Column("UPD_ID")]
        public string UpdId { get; set; }
    }
}
