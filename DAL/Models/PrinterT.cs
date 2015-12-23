using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterT
    {
        public virtual int No { get; set;}
        public virtual string Brand { get; set;}
        public virtual string Model { get; set; }
        public virtual int PrtMemberNo { get; set; }
        public virtual float LocX { get; set; }
        public virtual float LocY { get; set; }
        public virtual string Quality { get; set; }
        public virtual string Status { get; set; }
        public virtual string TestCompleteFlag { get; set; }
        public virtual string DelFlag { get; set; }
        public virtual DateTime DelDate { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string UpdId { get; set; }
        public virtual DateTime UpdDt { get; set; }
        public virtual string RecommendYn { get; set; }
        public virtual DateTime RecommendDt { get; set; }
        public virtual string RecommendVisibility { get; set; }
        public virtual int RecommendPriority { get; set; }
        public virtual int MainImg { get; set; }


        [IgnoreDataMember]
        public virtual string MemberName { get; set; }
        [IgnoreDataMember]
        public virtual string PrinterMemberName { get; set; }
        [IgnoreDataMember]
        public virtual int LikeCnt { get; set; }
        [IgnoreDataMember]
        public virtual string ImageName { get; set; }
        [IgnoreDataMember]
        public virtual string Path { get; set; }

    }
}
