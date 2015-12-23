using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PrinterStateTMap:ClassMap<PrinterStateT>
    {
        public PrinterStateTMap(){
            Id(x=>x.No,"NO");

            Map(x => x.OrderNo, "ORDER_NO");
            Map(x => x.ImgName, "IMG_NAME");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.OrderMemberName, "ORDER_MEMBER_NAME");
            Map(x => x.SpotName, "SPOT_NAME");
            Map(x => x.OrderStatus, "ORDER_STATUS");
            Map(x => x.PayType, "PAY_TYPE");
            Map(x => x.Price, "UNIT_PRICE");
        }
    }
}
