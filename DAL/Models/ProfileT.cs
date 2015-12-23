using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class ProfileT
    {
        [IgnoreDataMember]
        public virtual int DesignCnt { get; set; }
        [IgnoreDataMember]
        public virtual int DraftCnt { get; set; }
        [IgnoreDataMember]
        public virtual int LikesCnt { get; set; }
        [IgnoreDataMember]
        public virtual int FollowingCnt { get; set; }
        [IgnoreDataMember]
        public virtual int FollowerCnt { get; set; }
        [IgnoreDataMember]
        public virtual int NoticeCnt { get; set; }
        [IgnoreDataMember]
        public virtual int MessageCnt { get; set; }
        [IgnoreDataMember]
        public virtual int ListCnt { get; set; }
    }
}
