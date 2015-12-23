using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Makersn.Models
{
    class FollowerTMap:ClassMap<FollowerT>
    {
        public FollowerTMap()
        {
            Table("FOLLOWER");

            Id(x => x.No, "NO");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.MemberNoRef, "MEMBER_NO_REF");
            Map(x => x.IsNew, "IS_NEW");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
        }
    }
}
