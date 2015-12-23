using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class LikesT
    {
        public virtual int No { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual int ArticleNo { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string RegIp { get; set; }
    }
}
