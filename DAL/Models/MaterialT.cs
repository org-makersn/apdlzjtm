using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class MaterialT
    {
        public virtual int No { get; set; }
        public virtual string Name { get; set; }
        public virtual string DelFlag { get; set; }
        public virtual Nullable<DateTime> DelDt { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }

        [IgnoreDataMember]
        public virtual int UnitPrice { get; set; }
        [IgnoreDataMember]
        public virtual int MinPrice { get; set; }
        [IgnoreDataMember]
        public virtual int MaxPrice { get; set; }
    }
}
