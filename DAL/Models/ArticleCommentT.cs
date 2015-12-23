using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class ArticleCommentT
    {
        public virtual long No { get; set; }
        public virtual int ArticleNo { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual int MemberNoRef { get; set; }
        public virtual string Writer { get; set; }
        public virtual string Content { get; set; }
        public virtual long RefNo { get; set; }
        public virtual int Depth { get; set; }
        public virtual DateTime Regdt { get; set; }
        public virtual string RegId { get; set; }
        public virtual string RegIp { get; set; }
        public virtual Nullable<DateTime> UpdDt { get; set; }
        public virtual string UpdId { get; set; }

        public virtual string ProfilePic { get; set; }
        public virtual string MemberName { get; set; }
        public virtual string Name { get; set; }

        [IgnoreDataMember]
        public virtual IList<ArticleCommentT> replyList { get; set; }
    }
}
