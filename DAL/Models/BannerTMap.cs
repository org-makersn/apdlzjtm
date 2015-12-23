using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Makersn.Models
{
    public class BannerTMap : ClassMap<BannerT>
    {
        public BannerTMap()
        {
            Id(x => x.No, "NO");
            Map(x => x.Type, "TYPE");
            Map(x => x.Title, "TITLE").Length(250);
            Map(x => x.PublishYn, "PUBLISH_YN").Length(1);
            Map(x => x.OpenerYn, "OPENER_YN").Length(1);
            Map(x => x.Link, "LINK").Length(250);
            Map(x => x.Source, "SOURCE");
            Map(x => x.Term, "TERM").Length(250);
            Map(x => x.Image, "IMAGE").Length(250);
            Map(x => x.Priority, "PRIORITY");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.UpdDt, "UPD_DT");
            Map(x => x.UpdId, "UPD_ID");
            Table("BANNER");
        }
    }
}
