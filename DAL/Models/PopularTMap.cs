using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PopularTMap : ClassMap<PopularT>
    {
        public PopularTMap()
        {
            Id(x => x.No, "NO");
            Map(x => x.Word, "WORD");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.Cnt, "CNT");
            Map(x => x.RegIp, "REG_IP");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.RegId, "REG_ID");

            Table("POPULAR");
        }
    }
}
