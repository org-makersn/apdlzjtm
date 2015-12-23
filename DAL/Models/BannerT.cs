using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class BannerT
    {
        public virtual int No { get; set; }
        public virtual int Type { get; set; }
        public virtual string Title { get; set; }
        public virtual string PublishYn { get; set; }
        public virtual string OpenerYn { get; set; }
        public virtual string Link { get; set; }
        public virtual string Source { get; set; }
        public virtual string Term { get; set; }
        public virtual string Image { get; set; }
        public virtual Nullable<int> Priority { get; set; }
        public virtual Nullable<DateTime> RegDt { get; set; }
        public virtual string RegId { get; set; }
        public virtual Nullable<DateTime> UpdDt { get; set; }
        public virtual string UpdId { get; set; }
    }
}
