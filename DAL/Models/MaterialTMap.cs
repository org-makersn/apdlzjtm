using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class MaterialTMap:ClassMap<MaterialT>
    {
        MaterialTMap()
        {
            Table("MATERIAL");

            Id(x=>x.No,"NO");
            Map(x => x.Name, "NAME");
            Map(x => x.DelFlag, "DEL_FLAG");
            Map(x => x.DelDt, "DEL_DT");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
        }
    }
}
