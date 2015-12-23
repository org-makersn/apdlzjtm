using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterColorTMap:ClassMap<PrinterColorT>
    {
        PrinterColorTMap()
        {
            Table("PRINTER_COLOR");
            Id(x => x.No, "NO");
            Map(x => x.PrinterMaterialNo, "PRINTER_MATERIAL_NO");
            Map(x => x.PrinterNo, "PRINTER_NO");
            Map(x => x.ColorNo, "COLOR_NO");
            Map(x => x.UnitPrice, "UNIT_PRICE");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
        }
    }
}
