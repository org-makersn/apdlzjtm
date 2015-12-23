using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterFileT
    {
        public virtual int No { get; set;}
        public virtual int PrinterNo { get; set; }
        public virtual string FileGubun { get; set; }
        public virtual string Name { get; set; }
        public virtual string Rename { get; set; }
        public virtual string Path { get; set; }
        public virtual int Seq { get; set; }
        public virtual string Temp { get; set; }
        public virtual string Size { get; set; }
        public virtual string Width { get; set; }
        public virtual string Height { get; set; }
        public virtual string UseYn { get; set; }
        public virtual string Ext { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string RegIp { get; set; }

    }
}
