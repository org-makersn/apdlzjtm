using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class MessageT
    {
        public virtual long No { get; set; }
        public virtual string RoomName { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual int MemberNoRef { get; set; }
        public virtual string Comment { get; set; }
        public virtual string IsNew { get; set; }
        public virtual string DelFlag { get; set; }
        public virtual string RegIp { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }

        [IgnoreDataMember]
        public virtual string MemberName { get; set; }
        [IgnoreDataMember]
        public virtual string ProfilePic { get; set; }
        [IgnoreDataMember]
        public virtual string PicGbn { get; set; }

        public virtual string MsgGubun { get; set; }
    }
}
