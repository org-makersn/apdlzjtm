using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class ArticleStateT
    {
        public virtual string Gbn { get; set; }
        [IgnoreDataMember]
        public virtual int PublicCnt { get; set; }
        [IgnoreDataMember]
        public virtual int SavedCnt { get; set; }
        [IgnoreDataMember]
        public virtual int DownloadCnt { get; set; }
        [IgnoreDataMember]
        public virtual int PrintingCnt { get; set; }
        [IgnoreDataMember]
        public virtual int CodeNo1001 { get; set; }
        [IgnoreDataMember]
        public virtual int CodeNo1002 { get; set; }
        [IgnoreDataMember]
        public virtual int CodeNo1003 { get; set; }
        [IgnoreDataMember]
        public virtual int CodeNo1004 { get; set; }
        [IgnoreDataMember]
        public virtual int CodeNo1005 { get; set; }
        [IgnoreDataMember]
        public virtual int CodeNo1006 { get; set; }
    }
}
