using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    class PrinterMemberStateTMap : ClassMap<PrinterMemberStateT>
    {
        public PrinterMemberStateTMap()
        {
            Id(x => x.Gbn, "Gbn").GeneratedBy.Assigned();

            Map(x => x.Gbn, "Gbn").Nullable();
            Map(x => x.SpotOpenCnt, "SpotOpenCnt").Nullable();
            Map(x => x.UploadedPrinterCnt, "UploadedPrinterCnt").Nullable();
            Map(x => x.OrderCnt, "OrderCnt").Nullable();
            Map(x => x.Sales, "Sales").Nullable();

        }
    }
}
