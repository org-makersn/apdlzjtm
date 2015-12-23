using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Makersn.Models
{
    public class TranslationRequestListTMap : ClassMap<TranslationRequestListT>
    {
        public TranslationRequestListTMap()
        {
            Table("TranslationRequestList");

            Id(x => x.No, "NO");

            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.CodeNo, "CODE_NO");
            Map(x => x.MainImage, "MAIN_IMAGE");
            Map(x => x.Title, "TITLE");
            Map(x => x.Contents, "CONTENTS");
            Map(x => x.Tag, "TAG");
            Map(x => x.Copyright, "COPYRIGHT");
            Map(x => x.Visibility, "VISIBILITY");
            Map(x => x.ViewCnt, "VIEWCNT");
            Map(x => x.Temp, "TEMP");
            Map(x => x.RegIp, "REG_IP");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.UpdDt, "UPD_DT");
            Map(x => x.UpdId, "UPD_ID");
            Map(x => x.Status, "STATUS");

            Map(x => x.VideoUrl, "VIDEO_URL");

            Map(x => x.RecommendYn, "RECOMMEND_YN");
            Map(x => x.RecommendDt, "RECOMMEND_DT");

            Map(x => x.RecommendPriority, "RECOMMEND_PRIORITY");
            Map(x => x.RecommendVisibility, "RECOMMEND_VISIBILITY");
        }
    }
}
