using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PopularT
    {
        public virtual int No{get;set;}
        public virtual string Word { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual int Cnt { get; set; }
        public virtual string RegIp { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string RegId { get; set; }

    }
}
