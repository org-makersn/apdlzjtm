
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Makersn.Models
{
    class PrinterModelTMap : ClassMap<PrinterModelT>
    {
        
        public PrinterModelTMap()
        {
            Table("PRINTER_MODEL");
            Id(x => x.No, "NO");
            Map(x => x.Brand, "BRAND");
            Map(x => x.Model, "MODEL");
            Map(x => x.PropMemberNo, "PROP_MEMBER_NO");
            Map(x => x.ApprYn, "APPR_YN");
            Map(x => x.ApprMemberNo, "APPR_MEMBER_NO");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.UpdId, "UPD_ID");
            Map(x => x.UpdDt, "UPD_DT");
        }
    }
}
