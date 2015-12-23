using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Makersn.Models
{
    public class ArticleStateTMap : ClassMap<ArticleStateT>
    {
        public ArticleStateTMap()
        {
            Id(x => x.Gbn, "Gbn").GeneratedBy.Assigned();

            Map(x => x.Gbn, "Gbn").Nullable();
            Map(x => x.PublicCnt, "PublicCnt").Nullable();
            Map(x => x.SavedCnt, "SavedCnt").Nullable();
            Map(x => x.DownloadCnt, "DownloadCnt").Nullable();
            Map(x => x.PrintingCnt, "PrintingCnt").Nullable();
            Map(x => x.CodeNo1001, "CodeNo1001").Nullable();
            Map(x => x.CodeNo1002, "CodeNo1002").Nullable();
            Map(x => x.CodeNo1003, "CodeNo1003").Nullable();
            Map(x => x.CodeNo1004, "CodeNo1004").Nullable();
            Map(x => x.CodeNo1005, "CodeNo1005").Nullable();
            Map(x => x.CodeNo1006, "CodeNo1006").Nullable();
        }
    }
}
