using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class TranslationTMap:ClassMap<TranslationT>
    {
        public TranslationTMap()
        {
            Table("TRANSLATION");

            Id(x => x.No, "NO");
            Map(x => x.ArticleNo, "ARTICLE_NO");
            Map(x => x.TransFlag, "TRANS_FLAG");
            Map(x => x.Status, "STATUS");
            Map(x => x.LangFrom, "LANG_FROM");
            Map(x => x.LangTo, "LANG_TO");
            Map(x => x.ReqMemberNo, "REQ_MEMBER_NO");
            Map(x => x.ReqDt, "REQ_DT");
            Map(x => x.WorkMemberNo, "WORK_MEMBER_NO");
            Map(x => x.WorkDt, "WORK_DT");
            Map(x => x.TempFlag, "TEMP_FLAG");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.UpdId, "UPD_ID");
            Map(x => x.UpdDt, "UPD_DT");
        }
    }
}
