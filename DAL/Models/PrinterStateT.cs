using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterStateT
    {
        public virtual int No { get; set; }
        public virtual string OrderNo { get; set; }
        public virtual string ImgName { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string OrderMemberName { get; set; }
        public virtual string SpotName { get; set; }
        public virtual int OrderStatus { get; set; }
        public virtual int PayType { get; set; }
        public virtual int Price { get; set; }
    }
}
