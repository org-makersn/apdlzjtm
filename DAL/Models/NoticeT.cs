using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class NoticeT
    {
        public virtual int No { get; set; }
        public virtual string IdxName { get; set; }
        public virtual int ArticleNo { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual int MemberNoRef { get; set; }
        public virtual long RefNo { get; set; }
        public virtual string Type { get; set; }
        public virtual string Comment { get; set; }
        public virtual string IsNew { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string RegId { get; set; }
        public virtual string RegIp { get; set; }


        [IgnoreDataMember]
        public virtual string ArticleTItle { get; set; }
        [IgnoreDataMember]
        public virtual string MemberProfilePic { get; set; }
        [IgnoreDataMember]
        public virtual string MemberName { get; set; }

        [IgnoreDataMember]
        public virtual IList<PrinterOutputImageT> OutputImageList { get; set; }
    }

}
