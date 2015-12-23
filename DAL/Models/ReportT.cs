using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class ReportT
    {
        public virtual int No { get; set; }
        public virtual int ArticleNo { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual string Report { get; set; }
        public virtual string RegisterComment { get; set; }
        public virtual string ReporterComment { get; set; }
        public virtual int State { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string RegId { get; set; }
        public virtual string RegIp { get; set; }
        public virtual Nullable<DateTime> UpdDt { get; set; }
        public virtual string UpdId { get; set; }

        public virtual string Path { get; set; }
        public virtual string Title { get; set; }
        public virtual string Cate { get; set; }
        public virtual string AName { get; set; }
        public virtual int ANo { get; set; }
        public virtual string AId { get; set; }
        public virtual DateTime ADt { get; set; }
        public virtual string RName { get; set; }
        public virtual DateTime RDt { get; set; }
        public virtual string RId { get; set; }
        public virtual string Visibility { get; set; }


        
    }
}
