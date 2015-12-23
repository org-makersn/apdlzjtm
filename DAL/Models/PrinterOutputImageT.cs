using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterOutputImageT
    {
        public virtual int No { get; set; }
        public virtual long OrderNo { get; set; }
        public virtual string ImageName { get; set; }
        public virtual string ImageReName { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }
    }
}
