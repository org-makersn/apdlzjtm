using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class ReviewTMap : ClassMap<ReviewT>
    {
        public ReviewTMap() {
            Table("REVIEW");

            Id(x => x.No, "NO");
            Map(x => x.OrderNo, "ORDER_NO");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.PrinterNo, "PRINTER_NO");
            Map(x => x.Score, "SCORE");
            Map(x => x.Comment, "COMMENT");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.UpdId, "UPD_ID").Nullable();
            Map(x => x.UpdDt, "UPD_DT").Nullable();
        }
    }
}
