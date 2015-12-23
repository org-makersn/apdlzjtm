using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Makersn.Models
{
    public class CodeTMap : ClassMap<CodeT>
    {
        public CodeTMap()
        {
            Table("CODE");

            Id(x => x.No).Column("NO").GeneratedBy.Assigned();
            //CompositeId()
            //    .KeyReference(x => x.No, "NO")
            //    .KeyReference(x => x.Idx, "IDX");
            Map(x => x.Name).Column("NAME");
            Map(x => x.CodeGbn).Column("CODE_GBN");
            Map(x => x.CodeKey).Column("CODE_KEY");
            Map(x => x.Visibility).Column("VISIBILITY");
            Map(x => x.RegDt).Column("REG_DT");
            Map(x => x.RegId).Column("REG_DT");
        }
    }
}
