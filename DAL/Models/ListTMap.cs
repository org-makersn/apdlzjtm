using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class ListTMap : ClassMap<ListT>
    {
        public ListTMap()
        {
            Table("LIST");

            Id(x => x.No, "NO");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.ListName, "LIST_NAME");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.RegId, "REG_ID");
         }
    }
}
