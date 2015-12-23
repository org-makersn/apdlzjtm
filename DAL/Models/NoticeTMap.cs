using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class NoticeTMap:ClassMap<NoticeT>
    {
        public NoticeTMap(){
            Id(x => x.No,"NO");
            Map(x => x.IdxName, "IDX_NAME");
            Map(x => x.ArticleNo, "ARTICLE_NO");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.MemberNoRef, "MEMBER_NO_REF");
            Map(x => x.RefNo, "REF_NO");
            Map(x => x.Type, "TYPE");
            Map(x => x.Comment, "COMMENT");
            Map(x => x.IsNew, "IS_NEW");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.RegIp, "REG_IP");

            Table("NOTICE");
        }
    }
}
