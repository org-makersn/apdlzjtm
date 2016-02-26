
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Makersn.Models
{
    class PrinterTMap : ClassMap<PrinterT>
    {
        
        public PrinterTMap()
        {

            Table("PRINTER");
            Id(x => x.No, "NO");
            Map(x => x.Brand, "BRAND");
            Map(x => x.Model, "MODEL");
            Map(x => x.PrtMemberNo, "PRT_MEMBER_NO");
            Map(x => x.LocX, "LOC_X");
            Map(x => x.LocY, "LOC_Y");
            Map(x => x.Quality, "QUALITY");
            Map(x => x.Status, "STATUS");
            Map(x => x.TestCompleteFlag , "TEST_COMPLETE_FLAG");
            Map(x => x.DelFlag, "DEL_FLAG");
            Map(x => x.DelDate, "DEL_DATE");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.UpdId, "UPD_ID");
            Map(x => x.UpdDt, "UPD_DT");
            Map(x => x.RecommendYn, "RECOMMEND_YN");
            Map(x => x.RecommendDt, "RECOMMEND_DT");
            Map(x => x.RecommendVisibility, "RECOMMEND_VISIBILITY");
            Map(x => x.RecommendPriority, "RECOMMEND_PRIORITY");
            Map(x => x.MainImg, "MAIN_IMG");
        }
    }
}
