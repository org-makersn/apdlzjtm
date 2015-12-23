using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class OrderAccountingLogT
    {
        public virtual int No { get; set; }
        public virtual int Year{ get; set; }
        public virtual int Month { get; set; }
        public virtual int PrinterMemberNo { get; set; }
        public virtual int PaidPrice { get; set; }
        public virtual int PostPrice { get; set; }
        public virtual DateTime PayDt { get; set; }
    }
}
