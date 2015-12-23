using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class TranslationDetailTMap : ClassMap<TranslationDetailT>
    {
        public TranslationDetailTMap()
        {
            Table("TRANSLATION_DETAIL");

            Id(x => x.No, "NO");

            Map(x => x.ArticleNo, "ARTICLE_NO");
            Map(x => x.TranslationNo, "TRANSLATION_NO");
            Map(x => x.Title, "TITLE");
            Map(x => x.Contents, "CONTENTS");
            Map(x => x.Tag, "TAG");
            Map(x => x.LangFlag, "LANG_FLAG");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.UpdId, "UPD_ID");
            Map(x => x.UpdDt, "UPD_DT");

        }
    }
}
