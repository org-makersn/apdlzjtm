using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class MemberStateT
    {
        [IgnoreDataMember]
        public virtual string Gbn { get; set; }
        [IgnoreDataMember]
        public virtual int EmailCnt { get; set; }
        [IgnoreDataMember]
        public virtual int FacebookCnt { get; set; }
        [IgnoreDataMember]
        public virtual int DropMemberCnt { get; set; }
        [IgnoreDataMember]
        public virtual int ArticleCnt { get; set; }
        [IgnoreDataMember]
        public virtual int DownloadCnt { get; set; }
        
    }
}
