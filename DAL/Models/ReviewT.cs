using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class ReviewT
    {
        public virtual int No { get; set; }
        public virtual int OrderNo { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual int PrinterNo { get; set; }
        public virtual int Score { get; set; }
        public virtual string Comment { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string UpdId { get; set; }
        public virtual Nullable<DateTime> UpdDt { get; set; }

        [IgnoreDataMember]
        public virtual string MemberName { get; set; }
        [IgnoreDataMember]
        public virtual string PrinterName { get; set; }
    }
}
