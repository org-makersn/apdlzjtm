using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Makersn.Models
{
    public class ArticleDetailTMap : ClassMap<ArticleDetailT>
    {
        public ArticleDetailTMap()
        {
            Table("ARTICLE");

            Id(x => x.No, "NO");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.CodeNo, "CODE_NO").Nullable();
            Map(x => x.MainImage, "MAIN_IMAGE");
            Map(x => x.Title, "TITLE");
            Map(x => x.Contents, "CONTENTS").Nullable();
            Map(x => x.Tag, "TAG").Nullable();
            Map(x => x.Copyright, "COPYRIGHT").Nullable();
            Map(x => x.Visibility, "VISIBILITY").Nullable();
            Map(x => x.ViewCnt, "VIEWCNT");
            Map(x => x.Temp, "TEMP");
            Map(x => x.RegIp, "REG_IP").Nullable();
            Map(x => x.RegDt, "REG_DT").Nullable();
            Map(x => x.RegId, "REG_ID").Nullable();

            Map(x => x.RecommendYn, "RECOMMEND_YN").Nullable();
            Map(x => x.RecommendDt, "RECOMMEND_DT").Nullable();

            Map(x => x.MemberProfilePic, "MEMBER_PROFILE_PIC").Nullable();
            Map(x => x.MemberName, "MEMBER_NAME").Nullable();
            Map(x => x.MainImgName, "MAINIMGNAME").Nullable();
            //Map(x => x.Pop, "POP");
            Map(x => x.LikeCnt, "LIKE_CNT").Nullable();
            Map(x => x.CommentCnt, "COMMENT_CNT").Nullable();
            Map(x => x.UploadCnt, "UPLOAD_CNT").Nullable();
            Map(x => x.DraftCnt, "DRAFT_CNT").Nullable();
            Map(x => x.IsLikes, "IS_LIKES").Nullable();

            Map(x => x.VideoUrl, "VIDEO_URL").Nullable();

        }
    }
}
