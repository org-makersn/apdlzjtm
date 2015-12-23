
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Makersn.Models
{
    public class PrinterFileTMap : ClassMap<PrinterFileT>
    {

        public PrinterFileTMap()
        {

            Table("PRINTER_FILE");
            Id(x => x.No, "NO");
            Map(x=> x.FileGubun,"FILE_GUBUN");
            Map(x => x.PrinterNo, "PRINTER_NO");
            Map(x => x.Name, "NAME");
            Map(x => x.Rename, "RENAME");
            Map(x => x.Seq, "SEQ");
            Map(x => x.Path, "PATH");
            Map(x => x.Temp, "TEMP");
            Map(x => x.Size, "SIZE");
            Map(x => x.Width, "WIDTH");
            Map(x => x.Height, "HEIGHT");
            Map(x => x.Ext, "EXT");
            Map(x => x.RegIp, "REG_IP");

            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
        }
    }
}
