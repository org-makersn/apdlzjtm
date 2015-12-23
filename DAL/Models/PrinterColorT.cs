using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterColorT
    {
        public virtual int No { get; set; }
        public virtual int PrinterMaterialNo { get; set; }
        public virtual int PrinterNo { get; set; }
        public virtual int ColorNo { get; set; }
        public virtual int UnitPrice { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }

    }
}
