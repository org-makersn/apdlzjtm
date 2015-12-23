using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class ListArticleT
    {
        public virtual int No { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual int ListNo { get; set; }
        public virtual int ArticleNo { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string RegId { get; set; }

        [IgnoreDataMember]
        public virtual string ImgName { get; set; }
    }
}
