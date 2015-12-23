using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PopularStateTMap:ClassMap<PopularStateT>
    {
        public PopularStateTMap()
        {
            Id(x => x.RowNum, "ROW_NUM").GeneratedBy.Assigned();
            Map(x => x.Word, "WORD");
            Map(x => x.TotalCnt, "TotalCnt");
            Map(x => x.TodayCnt, "TodayCnt");
            Map(x => x.YesterdayCnt, "YesterdayCnt");
            Map(x => x.ThisweekCnt, "ThisweekCnt");
            Map(x => x.LastweekCnt, "LastweekCnt");
            Map(x => x.ThismonthCnt, "ThismonthCnt");
            Map(x => x.LastmonthCnt, "LastmonthCnt");
        }
    }
}
