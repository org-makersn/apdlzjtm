using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterMaterialTMap:ClassMap<PrinterMaterialT>
    {
        PrinterMaterialTMap()
        {
            Table("PRINTER_MATERIAL");

            Id(x=>x.No,"NO");
            Map(x => x.MaterialNo, "MATERIAL_NO");
            Map(x => x.PrinterNo, "PRINTER_NO");
            //Map(x => x.DelFlag, "DEL_FLAG");
            //Map(x => x.DelDt, "DEL_DT");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
        }
    }
}
