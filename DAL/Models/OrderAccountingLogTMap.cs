using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class OrderAccountingLogTMap : ClassMap<OrderAccountingLogT>
    {
        public OrderAccountingLogTMap()
        {
            Table("ORDER_ACCOUNTING_LOG");
            Id(x => x.No, "NO");
            Map(x => x.Year, "YEAR");
            Map(x => x.Month, "MONTH");
            Map(x => x.PrinterMemberNo, "PRINTER_MEMBER_NO");
            Map(x => x.PaidPrice, "PAID_PRICE");
            Map(x => x.PostPrice, "POST_PRICE");
            Map(x => x.PayDt, "PAY_DT");
        }
    }
}
