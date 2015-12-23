using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class OrderDetailT
    {
        public virtual int No { get; set; }
        public virtual int OrderNo { get; set; }
        public virtual string FileName { get; set; }
        public virtual string FileReName { get; set; }
        public virtual string FileImgRename { get; set; }
        public virtual string FileType { get; set; }
        public virtual int OrderCount { get; set; }
        public virtual int MaterialNo { get; set; }
        public virtual int UnitPrice { get; set; }
        public virtual string Temp { get; set; }
        public virtual double SizeX { get; set; }
        public virtual double SizeY { get; set; }
        public virtual double SizeZ { get; set; }
        public virtual double Volume { get; set; }
        public virtual double PrintVolume { get; set; }
        public virtual int ColorNo{ get; set; }
        public virtual string RegId{ get; set; }
        public virtual DateTime RegDt{ get; set; }

        [IgnoreDataMember]
        public virtual string Brand { get; set; }
        [IgnoreDataMember]
        public virtual string Model { get; set; }
        [IgnoreDataMember]
        public virtual int PrinterNo { get; set; }
        [IgnoreDataMember]
        public virtual string MaterialName { get; set; }
    }
}
