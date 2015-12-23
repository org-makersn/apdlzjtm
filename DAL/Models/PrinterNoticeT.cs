using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterNoticeT
    {
        public virtual int No { get; set; }
        public virtual long OrderNo { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual int MemberNoRef { get; set; }
        public virtual string Comment { get; set; }
        public virtual string IsNew { get; set; }
        public virtual string Type { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string RegId { get; set; }
        public virtual string RegIp { get; set; }
    }
}
