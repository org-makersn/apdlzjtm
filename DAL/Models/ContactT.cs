using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class ContactT
    {
        public virtual int No { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual string Title { get; set; }
        public virtual string Email { get; set; }
        public virtual int CodeNo { get; set; }
        public virtual string CodeName { get; set; }
        public virtual int State { get; set; }
        public virtual string StrState { get; set; }
        public virtual string Comment { get; set; }
        public virtual string Reply { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string RegId { get; set; }
        public virtual Nullable<DateTime> UpdDt { get; set; }
        public virtual string UpdId { get; set; }


        public virtual string Name { get; set; }
        public virtual string Phone { get; set; }
        public virtual string RegIp { get; set; }
    }
}
