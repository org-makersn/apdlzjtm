using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class ReportTMap : ClassMap<ReportT>
    {
        public ReportTMap()
        {
            Id(x => x.No, "NO");
            Map(x => x.ArticleNo, "ARTICLE_NO");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.Report, "REPORT");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.RegIp, "REG_IP");
            Map(x => x.UpdDt, "UPD_DT");
            Map(x => x.UpdId, "UPD_ID");

            Map(x => x.State, "STATE");
            Map(x => x.RegisterComment, "REGISTER_COMMENT");
            Map(x => x.ReporterComment, "REPORTER_COMMENT");

            Table("ARTICLE_REPORT");
        }
    }
}
