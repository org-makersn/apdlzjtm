using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class TranslationDetailT
    {
        public virtual int No { get; set; }
        public virtual int ArticleNo { get; set; }
        public virtual int TranslationNo { get; set; }
        public virtual string Title { get; set; }
        public virtual string Contents { get; set; }
        public virtual string Tag { get; set; }
        public virtual string LangFlag { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string UpdId { get; set; }
        public virtual Nullable<DateTime> UpdDt { get; set; }
    }
}
