using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class BoardT
    {
        public virtual int No { get; set; }
        public virtual int BoardSetNoSeq { get; set; }
        public virtual int BoardSetNo { get; set; }
        public virtual string LangFlag { get; set; }
        public virtual string Title { get; set; }
        public virtual string Writer { get; set; }
        //public virtual string Link { get; set; }
        public virtual string SemiContent { get; set; }
        public virtual string Visibility { get; set; }
        public virtual int Cnt { get; set; }
        public virtual string RegIp { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string RegId { get; set; }
        public virtual Nullable<DateTime> UpdDt { get; set; }
        public virtual string UpdId { get; set; }
    }
}
