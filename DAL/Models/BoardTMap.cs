using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Makersn.Models
{
    public class BoardTMap : ClassMap<BoardT>
    {
        public BoardTMap()
        {
            Id(x => x.No, "NO");
            Map(x => x.BoardSetNoSeq, "BOARD_SET_NO_SEQ");
            Map(x => x.BoardSetNo, "BOARD_SET_NO");
            Map(x => x.LangFlag, "LANG_FLAG");
            Map(x => x.Title, "TITLE");
            Map(x => x.Writer, "WRITER");
            Map(x => x.SemiContent, "SEMI_CONTENT");
            Map(x => x.Visibility, "VISIBILITY");
            Map(x => x.Cnt, "CNT");
            Map(x => x.RegIp, "REG_IP");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.UpdDt, "UPD_DT").Nullable();
            Map(x => x.UpdId, "UPD_ID");

            Table("BOARD");
        }
    }
}
