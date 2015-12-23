using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class MessageTMap : ClassMap<MessageT>
    {
        public MessageTMap()
        {
            Table("MEMBER_MSG");

            Id(x => x.No, "NO");
            Map(x => x.RoomName, "ROOM_NAME");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.MemberNoRef, "MEMBER_NO_REF");
            Map(x => x.Comment, "COMMENT");
            Map(x => x.IsNew, "IS_NEW");
            Map(x => x.DelFlag, "DEL_FLAG");
            Map(x => x.RegIp, "REG_IP");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");

            Map(x => x.MsgGubun, "MSG_GUBUN");
            //Map(x => x.MemberName, "NAME").Nullable();
            //Map(x => x.ProfilePic, "PROFILE_PIC").Nullable();
        }
    }
}
