using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class FollowerT
    {
        public virtual int No { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual int MemberNoRef { get; set; }
        public virtual string IsNew { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }

        public virtual int DesignCnt { get; set; }
        public virtual int LikesCnt { get; set; }
        public virtual int FollowerCnt { get; set; }
        public virtual string MemberName { get; set; }
        public virtual string ProfilePic { get; set; }
        public virtual string MemberBlog { get; set; }
        public virtual int ChkFollow { get; set; }
    }
}
