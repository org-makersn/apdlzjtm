using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class PopularStateT
    {
        public virtual int RowNum { get; set; }
        public virtual string Word { get; set; }
        public virtual int TotalCnt { get; set; }
        public virtual int TodayCnt { get; set; }
        public virtual int YesterdayCnt { get; set; }
        public virtual int ThisweekCnt { get; set; }
        public virtual int LastweekCnt { get; set; }
        public virtual int ThismonthCnt { get; set; }
        public virtual int LastmonthCnt { get; set; }
    }
}
