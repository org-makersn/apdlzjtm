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
        public virtual int MemberNo { get; set; }
        public virtual string Comment { get; set; }
        public virtual float LocX { get; set; }
        public virtual float LocY { get; set; }
        public virtual int Quality { get; set; }
        public virtual int Resolution { get; set; }
        public virtual int Status { get; set; }
        public virtual string TestCompleteFlag { get; set; }
        public virtual string DelFlag { get; set; }
        public virtual Nullable<DateTime> DelDt { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string UpdId { get; set; }
        public virtual Nullable<DateTime> UpdDt { get; set; }
        public virtual string RecommendYn { get; set; }
        public virtual Nullable<DateTime> RecommendDt { get; set; }
        public virtual string RecommendVisibility { get; set; }
        public virtual int? RecommendPriority { get; set; }
        public virtual int? MainImg { get; set; }
        public virtual string locName { get; set;  }
        public virtual string SaveFlag { get; set; }
        public virtual int PostMode { get; set; }
        public virtual int PostPrice { get; set; }
        public virtual string Temp { get; set; }

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


        [IgnoreDataMember]
        public virtual int Price { get; set; }
        [IgnoreDataMember]
        public virtual double Score { get; set; }
        [IgnoreDataMember]
        public virtual PrinterMemberT MemberEntity { get; set; }
        [IgnoreDataMember]
        public virtual IList<PrinterMaterialT> PrinterMaterialList { get; set; }
        [IgnoreDataMember]
        public virtual IList<PrinterFileT> PrinterFileList { get; set; }
        [IgnoreDataMember]
        public virtual int materialNo { get; set; }
        [IgnoreDataMember]
        public virtual int colorNo { get; set; }
        [IgnoreDataMember]
        public virtual int MinPrice { get; set; }
        [IgnoreDataMember]
        public virtual int MaxPrice { get; set; }
        [IgnoreDataMember]
        public virtual string SpotName { get; set; }
        [IgnoreDataMember]
        public virtual long orderNo { get; set; }
        [IgnoreDataMember]
        public virtual string MemberProfilePic { get; set; }
    }
}
