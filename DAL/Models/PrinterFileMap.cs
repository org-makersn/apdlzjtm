
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Makersn.Models
{
    class PrinterFileTMap : ClassMap<PrinterFileT>
    {

        public PrinterFileTMap()
        {

            Table("PRINTER_FILE");
            Id(x => x.No, "NO");
            Map(x => x.PrinterNo, "PRINTER_NO");
            Map(x => x.Name, "NAME");
            Map(x => x.ReName, "RENAME");
            Map(x => x.Path, "PATH");
            Map(x => x.Temp, "TEMP");
            Map(x => x.Size, "SIZE");
            Map(x => x.Width, "WIDTH");
            Map(x => x.Height, "HEIGHT");
            Map(x => x.Use_Yn, "USE_YN");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
        }
    }
}
