using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    class PrinterFileT
    {
        public virtual int No { get; set;}
        public virtual int PrinterNo { get; set; }
        public virtual string Name { get; set; }
        public virtual string ReName { get; set; }
        public virtual string Path { get; set; }
        public virtual string Temp { get; set; }
        public virtual string Size { get; set; }
        public virtual string Width { get; set; }
        public virtual string Height { get; set; }
        public virtual string Use_Yn { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }

    }
}
