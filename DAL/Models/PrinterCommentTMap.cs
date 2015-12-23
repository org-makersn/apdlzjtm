using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterCommentTMap:ClassMap<PrinterCommentT>
    {
        public PrinterCommentTMap()
        {
            Table("PRINTER_COMMENT");

            Id(x => x.No, "NO");
            Map(x => x.PrinterNo, "PRINTER_NO");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.MemberNoRef, "MEMBER_NO_REF");
            Map(x => x.Writer, "WRITER");
            Map(x => x.Content, "CONTENT");
            Map(x => x.Regdt, "REG_DT");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegIp, "REG_IP");
            Map(x => x.UpdDt, "UPD_DT");
            Map(x => x.UpdId, "UPD_ID");
        }
    }
}
