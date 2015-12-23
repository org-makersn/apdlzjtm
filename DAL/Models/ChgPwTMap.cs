using FluentNHibernate.Mapping;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class ChgPwTMap : ClassMap<ChgPwT>
    {
        ChgPwTMap()
        {
            Table("CHGPASSWORD");

            Id(x => x.No, "NO");
            Map(x => x.Pw1, "PW1");
            Map(x => x.Pw2, "PW2");

        }
    }
}
