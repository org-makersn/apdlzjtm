using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class ArticleFileT
    {
        public virtual int No { get; set; }
        public virtual string FileGubun { get; set; }
        public virtual string FileType { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual int ArticleNo { get; set; }
        public virtual int Seq { get; set; }
        public virtual string ImgUseYn { get; set; }
        public virtual string Ext { get; set; }
        public virtual string ThumbYn { get; set; }
        public virtual string MimeType { get; set; }
        public virtual string Name { get; set; }
        public virtual string Size { get; set; }
        public virtual string Rename { get; set; }
        public virtual string ImgName { get; set; }
        public virtual string Path { get; set; }
        public virtual string Width { get; set; }
        public virtual string Height { get; set; }
        public virtual string UseYn { get; set; }
        public virtual string Temp { get; set; }
        public virtual string RegIp { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string RegId { get; set; }
        public virtual Nullable<DateTime> UpdDt { get; set; }
        public virtual string UpdId { get; set; }

        public virtual double Volume { get; set; }
        public virtual double X { get; set; }
        public virtual double Y { get; set; }
        public virtual double Z { get; set; }
        public virtual double PrintVolume { get; set; }
    }
}
