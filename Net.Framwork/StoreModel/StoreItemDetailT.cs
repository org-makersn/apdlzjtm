using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Framework.StoreModel
{
    public partial class StoreItemDetailT : StoreItemT
    {

        //[Column("PROFILE_PIC")]
        public string ProfilePic { get; set; }
        public virtual string MemberProfilePic { get; set; }

        //[Column("MEMBER_NAME")]
        public string MemberName { get; set; }

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
