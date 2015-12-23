using Makersn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterMemberT : MemberT
    {
        public virtual string AccountNo { get; set; }
        public virtual string Bank { get; set; }
        public virtual string TaxbillFlag { get; set; }
        public virtual string ProfileMSG { get; set; }
        public virtual string SpotName { get; set; }
        [IgnoreDataMember]
        public virtual DateTime OpenDate { get; set; }
        [IgnoreDataMember]
        public virtual int PrintertCnt { get; set; }
        [IgnoreDataMember]
        public virtual int RequestCnt { get; set; }
        [IgnoreDataMember]
        public virtual int AcceptCnt { get; set; }
        [IgnoreDataMember]
        public virtual int RejectCnt { get; set; }
        [IgnoreDataMember]
        public virtual int Sales { get; set; }
    }
}
