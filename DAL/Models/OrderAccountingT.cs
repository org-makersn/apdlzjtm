using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class OrderAccountingT
    {
        public virtual long No { get; set; }
        public virtual long OrderNo { get; set; }
        public virtual int PrinterNo { get; set; }
        public virtual int PrinterMemberNo { get; set; }
        public virtual int Price { get; set; }
        public virtual int Status { get; set; }
        public virtual Nullable<DateTime> RegDt { get; set; }
        public virtual string RegId { get; set; }
        public virtual Nullable<DateTime> SendDt{get;set;}
        public virtual string SendId { get; set; }

    }
}
