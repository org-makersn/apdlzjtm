using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    [Table("STORE_PRODUCT")]
    public class StoreProductT
    {
        [Key]
        [Column("NO")]
        public virtual long No { get; set; }
        
        [Column("VAR_NO")]
        public virtual long VarNo { get; set; }
        [Column("PRODUCT_NAME")]
        public virtual string ProductName { get; set; }
        [Column("FILE_PATH")]
        public virtual string FilePath { get; set; }
        [Column("FILE_NAME")]
        public virtual string FileName { get; set; }
        [Column("FILE_RENAME")]
        public virtual string FileReName { get; set; }
        [Column("FILE_EXT")]
        public virtual string FileExt { get; set; }
        [Column("MIME_TYPE")]
        public virtual string MimeType { get; set; }
        [Column("FILE_SIZE")]
        public virtual double FileSize { get; set; }
        [Column("SLICED_VOLUME")]
        public virtual double SlicedVolume { get; set; }
        [Column("MODEL_VOLUME")]
        public virtual double ModelVolume { get; set; }
        [Column("SIZE_X")]
        public virtual double SizeX { get; set; }
        [Column("SIZE_Y")]
        public virtual double SizeY { get; set; }
        [Column("SIZE_Z")]
        public virtual double SizeZ { get; set; }
        [Column("SCALE")]
        public virtual double Scale { get; set; }
        [Column("SHORT_LINK")]
        public virtual string ShortLing { get; set; }
        [Column("VIDEO_URL")]
        public virtual string VideoUrl { get; set; }
        [Column("VIDEO_TYPE")]
        public virtual string VideoType { get; set; }
        [Column("CATEGORY_NO")]
        public virtual int CategoryNo { get; set; } //exerd 확인
        [Column("CONTENT", TypeName = "text")]
        [MaxLength]
        public virtual string Content { get; set; }
        [Column("DESCRIPTION")]
        public virtual string Description { get; set; }
        [Column("PART_CNT")]
        public virtual int PartCnt { get; set; }
        [Column("CUSTORMIZE_YN")]
        public virtual string CustormizeYn { get; set; } //exerd 확인
        [Column("SELL_YN")]
        public virtual string SellYn { get; set; } //사용 안하기로 하지 않았나?
        [Column("TAG_NAME")]
        public virtual string TagName { get; set; }
        [Column("CERTIFICATE_STATUS")]
        public virtual int CertiFicateStatus { get; set; }
        [Column("VISIBILITY_YN")]
        public virtual string VisibilityYn { get; set; }
        [Column("USE_YN")]
        public virtual string UseYn { get; set; }
        [Column("MEMBER_NO")]
        public virtual int MemberNo { get; set; }
        [Column("TXT_SIZE_X")]
        public virtual double TxtSizeX { get; set; }
        [Column("TXT_SIZE_Y")]
        public virtual double TxtSizeY { get; set; }
        [Column("DETAIL_TYPE")]
        public virtual int DetailType { get; set; } // 두개 다 컬럼명 데이터 타입 잘못된듯
        [Column("DETAIL_DEPTH")]
        public virtual int DetailDepth { get; set; } // 두개 다 컬럼명 데이터 타입 잘못된듯
        [Column("TXT_LOC")]
        public virtual string TxtLoc { get; set; }
        [Column("REG_DT")]
        public virtual DateTime RegDt { get; set; }
        [Column("REG_ID")]
        public virtual string RegId { get; set; }
        [Column("UPD_DT")]
        public virtual DateTime UpdDt { get; set; }
        [Column("UPD_ID")]
        public virtual string UpdId { get; set; }
    }
}
