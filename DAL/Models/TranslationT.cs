using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class TranslationT
    {
        public virtual int No { get; set; }
        public virtual int ArticleNo { get; set; }
        public virtual int TransFlag { get; set; }
        public virtual int Status { get; set; }
        public virtual string LangFrom { get; set; }
        public virtual string LangTo { get; set; }
        public virtual int ReqMemberNo { get; set; }
        public virtual DateTime ReqDt { get; set; }
        public virtual int WorkMemberNo { get; set; }
        public virtual Nullable<DateTime> WorkDt { get; set; }
        public virtual string TempFlag { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string UpdId { get; set; }
        public virtual Nullable<DateTime> UpdDt { get; set; }
    }
}
