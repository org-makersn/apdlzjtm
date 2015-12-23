using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class ListArticleTMap : ClassMap<ListArticleT>
    {
        public ListArticleTMap()
        {
            Table("LIST_ARTICLE");

            Id(x => x.No, "NO");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.ListNo, "LIST_NO");
            Map(x => x.ArticleNo, "ARTICLE_NO");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.RegId, "REG_ID");
        }
    }
}
