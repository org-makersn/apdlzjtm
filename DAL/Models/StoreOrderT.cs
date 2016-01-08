using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class StoreOrderT
    {
        public virtual Int64 No { get; set; }
        public virtual string Oid { get; set; }
        public virtual string Mid { get; set; }
        public virtual string IniRn { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string GoodName { get; set; }
        public virtual string Currency { get; set; }
        public virtual string Url { get; set; }
        public virtual string CardCode { get; set; }
        public virtual string PayMethod { get; set; }
        public virtual string Encrypted { get; set; }
        public virtual string SessionKey { get; set; }
        public virtual string Type { get; set; }
        public virtual string NoInterest { get; set; }
        public virtual string QoutaBase { get; set; }
        public virtual string BuyerName { get; set; }
        public virtual string BuyerTel { get; set; }
        public virtual string BuyerEmail { get; set; }
        public virtual string RecvName { get; set; }
        public virtual string RecvTel { get; set; }
        public virtual string RecvAddr { get; set; }
        public virtual string RecvPostNum { get; set; }
        public virtual string RecvMsg { get; set; }
        public virtual string JoinCard { get; set; }
        public virtual string JoinExpire { get; set; }
        public virtual string UserId { get; set; }
        public virtual string Tax { get; set; }
        public virtual string TaxFree { get; set; }
        public virtual int MemberNo { get; set; }
        public virtual Nullable<DateTime> OrderDate { get; set; }
        public virtual string PaymentStatus { get; set; }
        public virtual string Addr1 { get; set; }
        public virtual string Addr2 { get; set; }
        public virtual string PostNo { get; set; }
        public virtual Nullable<DateTime> RegDt { get; set; }
        public virtual string RegId { get; set; }
        public virtual Nullable<DateTime> UpdDt { get; set; }
        public virtual string UpdId { get; set; }
    }
}
