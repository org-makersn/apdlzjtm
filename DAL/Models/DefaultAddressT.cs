using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class DefaultAddressT
    {
        public virtual int No { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual string PostMemberName { get; set; }
        public virtual string Address { get; set; }
        public virtual string AddressDetail { get; set; }
        public virtual string PostNum { get; set; }
        public virtual string CellPhone { get; set; }
        public virtual string AddPhone { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string UpdId { get; set; }
        public virtual Nullable<DateTime> UpdDt { get; set; }
    }
}
