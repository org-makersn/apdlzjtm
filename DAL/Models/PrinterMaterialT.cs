using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterMaterialT
    {
        public virtual int No { get; set; }
        public virtual int PrinterNo { get; set; }
        public virtual int MaterialNo { get; set; }
        
        //public virtual string DelFlag { get; set; }
        //public virtual Nullable<DateTime> DelDt { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }

        [IgnoreDataMember]
        public virtual string MaterialName { get; set; }
        [IgnoreDataMember]
        public virtual IList<PrinterColorT> MaterialColorList { get; set; }
    }
}
