﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class ArticleT
    {
        public virtual int No { get; set; }
        public virtual int MemberNo { get; set; }
        [IgnoreDataMember]
        public virtual string MemberName { get; set; }
        public virtual int CodeNo { get; set; }
        [IgnoreDataMember]
        public virtual string Category { get; set; }
        public virtual int MainImage { get; set; }
        public virtual string Title { get; set; }
        public virtual string Contents { get; set; }
        public virtual string Tag { get; set; }
        public virtual int Copyright { get; set; }
        public virtual string Visibility { get; set; }
        public virtual int ViewCnt { get; set; }
        public virtual string Temp { get; set; }
        public virtual string RegIp { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string RegId { get; set; }
        public virtual Nullable<DateTime> UpdDt { get; set; }
        public virtual string UpdId { get; set; }
        public virtual string RecommendVisibility { get; set; }



        [IgnoreDataMember]
        public virtual int RecommendPriority { get; set; }
        [IgnoreDataMember]
        public virtual string Path { get; set; }
        [IgnoreDataMember]
        public virtual int Pop { get; set; }
        [IgnoreDataMember]
        public virtual int LikeCnt { get; set; }
        [IgnoreDataMember]
        public virtual int CommentCnt { get; set; }

        public virtual string RecommendYn { get; set; }
        public virtual Nullable<DateTime> RecommendDt { get; set; }
        [IgnoreDataMember]
        public virtual string MemberProfilePic { get; set; }
        [IgnoreDataMember]
        public virtual int chkLikes { get; set; }

        [IgnoreDataMember]
        public virtual string ImageName { get; set; }

        [IgnoreDataMember]
        public virtual string VideoUrl { get; set; }

        [IgnoreDataMember]
        public virtual int UploadCnt { get; set; }
        [IgnoreDataMember]
        public virtual int DraftCnt { get; set; }
        [IgnoreDataMember]
        public virtual int DownloadCnt { get; set; }
    }
}
