using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    public partial class StoreItemDetailT
    {
        [Key, Column("NO")]
        public long No { get; set; }
        [Column("TEMP")]
        public string Temp { get; set; }
        [Column("STORE_MEMBER_NO")]
        public int StoreMemberNo { get; set; }
        [Column("CODE_NO")]
        public int CodeNo { get; set; }
        [Column("MAIN_IMG")]
        public int MainImg { get; set; }
        [Column("TITLE")]
        public string Title { get; set; }
        [Column("CONTENTS")]
        public string Contents { get; set; }
        [Column("BASE_PRICE")]
        public int BasePrice { get; set; }
        [Column("SHIPPING_TYPE")]
        public int ShippingType { get; set; }
        [Column("SHIPPING_COST")]
        public int ShippingCost { get; set; }
        [Column("STOCK_CNT")]
        public int StockCnt { get; set; }
        [Column("TAGS")]
        public string Tags { get; set; }
        [Column("VIEW_CNT")]
        public int ViewCnt { get; set; }
        [Column("VIDEO_SOURCE")]
        public string VideoSource { get; set; }
        [Column("USE_YN")]
        public string UseYn { get; set; }
        [Column("FEATURED_YN")]
        public string FeaturedYn { get; set; }
        [Column("FEATURED_DT")]
        public Nullable<System.DateTime> FeaturedDt { get; set; }
        [Column("FEATURED_VISIBILITY")]
        public string FeaturedVisibility { get; set; }
        [Column("FEATURED_PRIORITY")]
        public Nullable<int> FeaturedPriority { get; set; }
        [Column("REG_IP")]
        public string RegIp { get; set; }
        [Column("REG_DT")]
        public System.DateTime RegDt { get; set; }
        [Column("REG_ID")]
        public string RegId { get; set; }

        public string ShippingName { get; set; }
        //[Column("PROFILE_PIC")]
        public string ProfilePic { get; set; }

        //[Column("STORE_NAME")]
        public string StoreName { get; set; }

        //[Column("MAIN_IMG_NAME")]
        public string MainImgName { get; set; }

        //[Column("LIKE_CNT")]
        public virtual int LikeCnt { get; set; }

        //[Column("COMMENT_CNT")]
        public int CommentCnt { get; set; }

        //[Column("IS_LIKES")]
        public int IsLikes { get; set; }
    }
}
