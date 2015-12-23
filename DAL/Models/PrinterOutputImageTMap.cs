using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterOutputImageTMap:ClassMap<PrinterOutputImageT>
    {
        public PrinterOutputImageTMap()
        {
            Table("PRINTER_OUTPUT_IMG");

            Id(x => x.No, "NO");
            Map(x => x.OrderNo, "ORDER_NO");
            Map(x => x.ImageName, "IMG_NAME");
            Map(x => x.ImageReName, "IMG_RENAME");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
        }
    }
}
