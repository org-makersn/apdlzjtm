using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class DashBoardStateTMap : ClassMap<DashBoardStateT>
    {
        public DashBoardStateTMap()
        {
            Id(x => x.Gbn, "Gbn").GeneratedBy.Assigned();

            Map(x => x.Gbn, "Gbn").Nullable();
            Map(x => x.EmailCnt, "EmailCnt").Nullable();
            Map(x => x.FacebookCnt, "FacebookCnt").Nullable();
            Map(x => x.DropMemberCnt, "DropMemberCnt").Nullable();
            Map(x => x.ArticleCnt, "ArticleCnt").Nullable();
            Map(x => x.DownloadCnt, "DownloadCnt").Nullable();
            Map(x => x.SpotCnt, "SpotCnt").Nullable();
            Map(x => x.PrinterCnt, "PrinterCnt").Nullable();
            Map(x => x.OrderCnt, "OrderCnt").Nullable();
            Map(x => x.TotalPrice, "TotalPrice").Nullable();
            Map(x => x.OrderMemCnt, "OrderMemCnt").Nullable();

            //HasOne<MemberT>(x => x.Gbn).Cascade.None().Constrained();
        }
    }
}
