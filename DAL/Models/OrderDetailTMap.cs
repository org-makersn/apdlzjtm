using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class OrderDetailTMap : ClassMap<OrderDetailT>
    {
        public OrderDetailTMap()
        {
            Table("ORDER_DETAIL");
            Id(x => x.No, "NO");
            Map(x => x.OrderNo, "ORDER_NO");
            Map(x => x.FileName, "FILE_NAME");
            Map(x => x.FileReName, "FILE_RENAME");
            Map(x => x.FileImgRename, "FILE_IMG_RENAME");
            Map(x => x.FileType, "FILE_TYPE");
            Map(x => x.OrderCount, "ORDER_COUNT");
            Map(x => x.MaterialNo, "MATERIAL_NO");
            Map(x => x.UnitPrice, "UNIT_PRICE");
            Map(x => x.Temp, "TEMP");
            Map(x => x.SizeX, "SIZE_X");
            Map(x => x.SizeY, "SIZE_Y");
            Map(x => x.SizeZ, "SIZE_Z");
            Map(x => x.Volume, "VOLUME");
            Map(x => x.PrintVolume, "PRINT_VOLUME");
            Map(x => x.ColorNo, "COLOR_NO");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
        }
    }
}
