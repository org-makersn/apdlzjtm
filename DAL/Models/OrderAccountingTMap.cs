using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class OrderAccountingTMap : ClassMap<OrderAccountingT>
    {
        public OrderAccountingTMap()
        {
            Table("ORDER_ACCOUNTING");
            Id(x => x.No, "NO");
            Map(x => x.OrderNo, "ORDER_NO");
            Map(x => x.PrinterMemberNo, "PRINTER_MEMBER_NO");
            Map(x => x.PrinterNo, "PRINTER_NO");
            Map(x => x.Price, "PRICE");
            Map(x => x.Status, "STATUS");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.SendDt, "SEND_DT");
            Map(x => x.SendId, "SEND_ID");
        }
    }
}
