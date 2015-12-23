using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Design.Web.Front.Models
{
    public class CodeModel
    {
        public virtual string MenuTitle { get; set; }
        public virtual string MenuUrl { get; set; }
        public virtual int MenuCodeNo { get; set; }
        public virtual string MenuValue { get; set; }
    }
}
