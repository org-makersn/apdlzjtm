using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Makersn.Models
{
    public class MemberStateTMap : ClassMap<MemberStateT>
    {
        public MemberStateTMap()
        {
            Id(x => x.Gbn, "Gbn").GeneratedBy.Assigned();

            Map(x => x.Gbn, "Gbn").Nullable();
            Map(x => x.EmailCnt, "EmailCnt").Nullable();
            Map(x => x.FacebookCnt, "FacebookCnt").Nullable();
            Map(x => x.DropMemberCnt, "DropMemberCnt").Nullable();
            Map(x => x.ArticleCnt, "ArticleCnt").Nullable();
            Map(x => x.DownloadCnt, "DownloadCnt").Nullable();

            //HasOne<MemberT>(x => x.Gbn).Cascade.None().Constrained();
        }
    }
}
