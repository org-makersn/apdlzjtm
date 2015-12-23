using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class OrderAccountingStateT
    {
        [IgnoreDataMember]
        public virtual int Year { get; set; }
        [IgnoreDataMember]
        public virtual int Month { get; set; }
        [IgnoreDataMember]
        public virtual string PrinterMemberName { get; set; }
        [IgnoreDataMember]
        public virtual int Bank{ get; set; }
        [IgnoreDataMember]
        public virtual string BankName { get; set; }
        [IgnoreDataMember]
        public virtual string Account { get; set; }
        [IgnoreDataMember]
        public virtual int Price { get; set; }
        [IgnoreDataMember]
        public virtual int PostPrice { get; set; }
        [IgnoreDataMember]
        public virtual int PaidPrice { get; set; }
        [IgnoreDataMember]
        public virtual int PrinterMemberNo { get; set; }
        [IgnoreDataMember]
        public virtual int Status { get; set; }

        [IgnoreDataMember]
        public virtual IList<OrderT> OrderList { get; set; }

    }
}
