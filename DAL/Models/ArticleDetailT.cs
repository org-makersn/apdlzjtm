using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class ArticleDetailT
    {
        public virtual int No { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual int CodeNo { get; set; }
        public virtual int MainImage { get; set; }
        public virtual string Title { get; set; }
        [IgnoreDataMember]
        public virtual string Contents { get; set; }
        [IgnoreDataMember]
        public virtual string Tag { get; set; }
        [IgnoreDataMember]
        public virtual int Copyright { get; set; }
        [IgnoreDataMember]
        public virtual string Visibility { get; set; }
        [IgnoreDataMember]
        public virtual int ViewCnt { get; set; }
        [IgnoreDataMember]
        public virtual string Temp { get; set; }
        [IgnoreDataMember]
        public virtual string RegIp { get; set; }
        [IgnoreDataMember]
        public virtual DateTime RegDt { get; set; }
        [IgnoreDataMember]
        public virtual string RegId { get; set; }
        [IgnoreDataMember]
        public virtual string RecommendYn { get; set; }
        [IgnoreDataMember]
        public virtual Nullable<DateTime> RecommendDt { get; set; }
        [IgnoreDataMember]
        public virtual int Pop { get; set; }
        [IgnoreDataMember]
        public virtual string MemberProfilePic { get; set; }
        [IgnoreDataMember]
        public virtual string MemberName { get; set; }
        [IgnoreDataMember]
        public virtual string MainImgName { get; set; }
        [IgnoreDataMember]
        public virtual int LikeCnt { get; set; }
        [IgnoreDataMember]
        public virtual int CommentCnt { get; set; }
        [IgnoreDataMember]
        public virtual int UploadCnt { get; set; }
        [IgnoreDataMember]
        public virtual int DraftCnt { get; set; }
        [IgnoreDataMember]
        public virtual int IsLikes { get; set; }
        [IgnoreDataMember]
        public virtual string VideoUrl { get; set; }
    }
}
