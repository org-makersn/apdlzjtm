using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_ITEM_FILE")]
    public partial class StoreItemFileT
    {
        [Key, Column("NO")]
        public long No { get; set; }
        [Column("TEMP")]
        public string Temp { get; set; }
        [Column("STORE_MEMBER_NO")]
        public int StoreMemberNo { get; set; }
        [Column("STORE_ITEM_NO")]
        public long StoreItemNo { get; set; }
        [Column("FILE_GBN")]
        public string FileGbn { get; set; }
        [Column("NAME")]
        public string Name { get; set; }
        [Column("RENAME")]
        public string ReName { get; set; }
        [Column("FILE_EXT")]
        public string FileExt { get; set; }
        [Column("MIME_TYPE")]
        public string MimeType { get; set; }
        [Column("FILE_SIZE")]
        public double FileSize { get; set; }
        [Column("IDX")]
        public int Idx { get; set; }
        [Column("IMG_USE_YN")]
        public string ImgUseYn { get; set; }
        [Column("THUMB_YN")]
        public string ThumbYn { get; set; }
        [Column("USE_YN")]
        public string UseYn { get; set; }
        [Column("REG_IP")]
        public string RegIp { get; set; }
        [Column("REG_DT")]
        public System.DateTime RegDt { get; set; }
        [Column("REG_ID")]
        public string RegId { get; set; }
        [Column("UPD_DT")]
        public Nullable<System.DateTime> UpdDt { get; set; }
        [Column("UPD_ID")]
        public string UpdId { get; set; }
    }
}
