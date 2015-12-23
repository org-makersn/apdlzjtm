using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class OrderT
    {

        public virtual long No { get; set; }
        public virtual string OrderNo { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual int PrinterMemberNo { get; set; }
        public virtual int PrinterNo { get; set; }
        public virtual int Quality { get; set; }
        public virtual Nullable<DateTime> OrderDt { get; set; }
        public virtual Nullable<DateTime> PostDt { get; set; }
        public virtual int OrderStatus{ get; set; }
        public virtual int PayType{ get; set; }
        public virtual Nullable<DateTime> PayDt{ get; set; }
        public virtual int PayBank { get; set; }
        public virtual string PayAccountNo { get; set; }
        public virtual string PayAccountName { get; set; }
        public virtual string CurrencyFlag { get; set; }
        public virtual string CurrencyNum { get; set; }
        public virtual int CurrencyNumType { get; set; }
        public virtual string PostMemberName { get; set; }
        public virtual string PostAddress { get; set; }
        public virtual string PostAddressDetail { get; set; }
        public virtual string PostNum { get; set; }
        public virtual int PostMode { get; set; }
        public virtual int PostPrice { get; set; }
        public virtual string DeliveryCompany { get; set; }
        public virtual string DeliveryNum { get; set; }
        public virtual string RequireComment { get; set; }
        public virtual string ApproveFlag { get; set; }
        public virtual string CancleComment { get; set; }
        public virtual Nullable<DateTime> CancleDt { get; set; }
        public virtual string Temp { get; set; }
        public virtual string Memo { get; set; }
        public virtual string TestFlag { get; set; }
        public virtual string CellPhone { get; set; }
        public virtual string AddPhone { get; set; }
        public virtual string OrderPath{ get; set; }
        public virtual string RegId { get; set; }
        public virtual DateTime RegDt { get; set; }
        public virtual string UpdId { get; set; }
        public virtual Nullable<DateTime> UpdDt { get; set; }

        // ==> these colums is for order accounting 
        public virtual Nullable<DateTime> OrderDoneDt { get; set; }
        public virtual Nullable<DateTime> AccountDoneDt { get; set; }
        public virtual string AccountDoneID { get; set; }
        public virtual int AccountState { get; set; }
        // <== finish accounting 

        [IgnoreDataMember]
        public virtual string PrinterName { get; set; }
        [IgnoreDataMember]
        public virtual string MaterialName { get; set; }
        [IgnoreDataMember]
        public virtual string ColorName { get; set; }
        [IgnoreDataMember]
        public virtual int TotalPrice { get; set; }
        [IgnoreDataMember]
        public virtual string fileImgName { get; set; }
        [IgnoreDataMember]
        public virtual string fileName { get; set; }
        [IgnoreDataMember]
        public virtual string PrinterMemberName { get; set; }
        [IgnoreDataMember]
        public virtual int DetailCount { get; set; }
        [IgnoreDataMember]
        public virtual IList<OrderDetailT> orderDetailList { get; set; }
        [IgnoreDataMember]
        public virtual string OrderMemberName { get; set; }
        [IgnoreDataMember]
        public virtual string Email { get; set; }
        
    }
}
