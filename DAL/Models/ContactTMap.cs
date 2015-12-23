using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Makersn.Models
{
    public class ContactTMap : ClassMap<ContactT>
    {
        public ContactTMap()
        {
            Table("CONTACT");

            Id(x => x.No, "NO");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.Title, "TITLE");
            Map(x => x.Email , "EMAIL");
            Map(x => x.CodeNo, "CODE_NO");
            Map(x => x.State, "STATE");
            Map(x => x.Comment, "COMMENT");
            Map(x => x.Reply, "REPLY");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.UpdDt, "UPD_DT");
            Map(x => x.UpdId, "UPD_ID");


            Map(x => x.Name, "NAME");
            Map(x => x.Phone, "PHONE");
            Map(x => x.RegIp, "REG_IP");

        }
    }
}
