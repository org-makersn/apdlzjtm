using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class CodeT
    {
        public virtual int No { get; set; }
        public virtual int Idx { get; set; }
        public virtual string CodeGbn { get; set; }
        public virtual string CodeKey { get; set; }
        public virtual string Name { get; set; }
        public virtual string Visibility { get; set; }
        public virtual Nullable<DateTime> RegDt { get; set; }
        public virtual string RegId { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
