using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterNoticeTMap:ClassMap<PrinterNoticeT>
    {
        public PrinterNoticeTMap()
        {
            Table("PRINTER_NOTICE");

            Id(x => x.No, "NO");
            Map(x => x.OrderNo, "ORDER_NO");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.MemberNoRef, "MEMBER_NO_REF");
            Map(x => x.Comment, "COMMENT");
            Map(x => x.Type, "TYPE");
            Map(x => x.IsNew, "IS_NEW");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegIp, "REG_IP");
        }
    }
}
