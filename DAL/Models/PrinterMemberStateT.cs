using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterMemberStateT
    {

        public virtual string Gbn { get; set; }
        [IgnoreDataMember]
        public virtual int SpotOpenCnt { get; set; }
        [IgnoreDataMember]
        public virtual int UploadedPrinterCnt { get; set; }
        [IgnoreDataMember]
        public virtual int OrderCnt { get; set; }
        [IgnoreDataMember]
        public virtual int Sales { get; set; }
    }
}
