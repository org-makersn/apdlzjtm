using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Makersn.Models
{
    public class DropMemberTMap : ClassMap<DropMemberT>
    {
        public DropMemberTMap()
        {
            Id(x => x.No, "NO").GeneratedBy.Assigned();
            Map(x => x.Id, "ID");
            Map(x => x.Level, "LEVEL");
            Map(x => x.Name, "NAME");
            Map(x => x.LastLoginDt, "LAST_LOGIN_DT");
            Map(x => x.LastLoginIp, "LAST_LOGIN_IP");
            Map(x => x.LoginCnt, "LOGIN_CNT");
            Map(x => x.DelDt, "DEL_DT");
            Map(x => x.DropComment, "DROP_COMMENT");
            Map(x => x.DelFlag, "DEL_FLAG");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.RegId, "REG_ID");
            Table("DROP_MEMBER");
        }
    }
}
