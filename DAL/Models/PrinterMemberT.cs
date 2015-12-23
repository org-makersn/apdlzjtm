using Makersn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterMemberT
    {
        public virtual int No { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual string AccountNo { get; set; }
        public virtual string Bank { get; set; }
        public virtual string BankName { get; set; }
        public virtual string HomePhone { get; set; }
        public virtual string CellPhone { get; set; }
        public virtual string SpotName { get; set; }
        public virtual string SpotAddress { get; set; }
        public virtual string SpotAddressDetail { get; set; }
        public virtual string SpotUrl { get; set; }
        public virtual double LocX { get; set; }
        public virtual double LocY { get; set; }
        public virtual int PostMode { get; set; }
        public virtual int PostType { get; set; }
        public virtual int PostPrice { get; set; }
        public virtual string TaxbillFlag { get; set; }
        public virtual string PrinterProfileMsg { get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string UpdId { get; set; }
        public virtual DateTime? UpdDt { get; set; }
        public virtual string PrinterProfilePic { get; set; }
        public virtual string PrinterCoverPic { get; set; }
        public virtual string SaveFlag { get; set; }
        public virtual string DelFlag { get; set; }
        public virtual int ViewCnt { get; set; }

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
        [IgnoreDataMember]
        public virtual int ReviewCnt { get; set; }
        [IgnoreDataMember]
        public virtual double ReviewScore { get; set; }



        // member property
        [IgnoreDataMember]
        public virtual string Id { get; set; }
        [IgnoreDataMember]
        public virtual string BlogUrl { get; set; }
        [IgnoreDataMember]
        public virtual string Name { get; set; }
        [IgnoreDataMember]
        public virtual string Email { get; set; }
        [IgnoreDataMember]
        public virtual string Url { get; set; }
        [IgnoreDataMember]
        public virtual string SnsType { get; set; }
        [IgnoreDataMember]
        public virtual string SnsId { get; set; }
        [IgnoreDataMember]
        public virtual string ProfileMsg { get; set; }
        [IgnoreDataMember]
        public virtual string ProfilePic { get; set; }
        [IgnoreDataMember]
        public virtual string CoverPic { get; set; }
        [IgnoreDataMember]
        public virtual string MemberRegId { get; set; }
        [IgnoreDataMember]
        public virtual DateTime MemberRegDt { get; set; }

        [IgnoreDataMember]
        public virtual IList<PrinterT> printerList { get; set; }
        [IgnoreDataMember]
        public virtual IList<PrinterMaterialT> matList { get; set; }
        [IgnoreDataMember]
        public virtual IList<PrinterColorT> colorList { get; set; }
        [IgnoreDataMember]
        public virtual IList<ReviewT> reviewList { get; set; }

        [IgnoreDataMember]
        public virtual int MinPrice { get; set; }
        [IgnoreDataMember]
        public virtual int MaxPrice { get; set; }
    }
}
