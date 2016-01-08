using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class StoreCartT
    {
        public virtual Int64 No { get; set; }
        public virtual string CartNo { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual Int64 ProductDetailNo { get; set; }
        public virtual int ProductCnt { get; set; }
        public virtual string OrderYn { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string RegId { get; set; }
        public virtual Nullable<DateTime> UpdDt { get; set; }
        public virtual string UpdId { get; set; }

        [IgnoreDataMember]
        public virtual string Name { get; set; }

        [IgnoreDataMember]
        public virtual string ProductName { get; set; }

        [IgnoreDataMember]
        public virtual int TotalPrice { get; set; }
    }
}
