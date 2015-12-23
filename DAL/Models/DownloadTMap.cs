using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Makersn.Models
{
    public class DownloadTMap : ClassMap<DownloadT>
    {
        public DownloadTMap()
        {
            Table("DOWNLOAD");

            Id(x => x.No, "NO");

            Map(x => x.ArticleNo, "ARTICLE_NO");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.Cnt, "CNT");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegIp, "REG_IP");

        }
    }
}
