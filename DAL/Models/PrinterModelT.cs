using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterModelT
    {
        public virtual int No { get; set;}
        public virtual string Brand { get; set;}
        public virtual string Model { get; set; }
        public virtual int PropMemberNo { get; set; }
        public virtual string ApprYn { get; set; }
        public virtual int ApprMemberNo { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string UpdId { get; set; }
        public virtual Nullable<DateTime> UpdDt{get;set;}

        [IgnoreDataMember]
        public virtual string PropMemberName { get; set; }
    }
}
