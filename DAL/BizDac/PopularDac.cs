using Makersn.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.BizDac
{
    public class PopularDac
    {
        public IList<PopularStateT> SelectPopular()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {

                string query = @"select 
									ROW_NUMBER() OVER(ORDER BY b.totalCnt DESC) AS ROW_NUM ,b.WORD, b.totalCnt,
	                                IsNull((select SUM(CNT) from POPULAR where WORD = b.WORD and datediff(DAY,REG_DT-1,getdate())=1), 0) as TodayCnt,
	                                IsNull((select SUM(CNT) from POPULAR where WORD = b.WORD and datediff(DAY,REG_DT-1,getdate())=2), 0) as YesterdayCnt,
	                                IsNull((select SUM(CNT) from POPULAR where WORD = b.WORD and REG_DT >= dateadd(day,2-datepart(weekday,getdate()),convert(varchar,getdate(),112)) AND REG_DT < dateadd(day,9-datepart(weekday,getdate()),convert(varchar,getdate(),112)) ), 0) as ThisweekCnt,
	                                IsNull((select SUM(CNT) from POPULAR where WORD = b.WORD and REG_DT >= dateadd(day,-5-datepart(weekday,getdate()),convert(varchar,getdate(),112)) AND REG_DT < dateadd(day,2-datepart(weekday,getdate()),convert(varchar,getdate(),112))), 0) as LastweekCnt,
	                                IsNull((select SUM(CNT) from POPULAR where WORD = b.WORD and REG_DT >= dateadd(day,1-day(getdate()),convert(varchar,getdate(),112)) AND REG_DT < dateadd(month,1,dateadd(day,1-day(getdate()),convert(varchar,getdate(),112)))), 0) as ThismonthCnt,
	                                IsNull((select SUM(CNT) from POPULAR where WORD = b.WORD and REG_DT >= dateadd(month,-1,dateadd(day,1-day(getdate()),convert(varchar,getdate(),112))) AND REG_DT < dateadd(day,1-day(getdate()),convert(varchar,getdate(),112)) ), 0) as LastmonthCnt
                                from(
	                                select DISTINCT(a.WORD), SUM(a.CNT) as totalCnt
	                                FROM POPULAR a
	                                where WORD is not null and WORD <> ''
	                                GROUP BY WORD
                                ) as b
								order by b.totalCnt desc";

                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(PopularStateT));

                IList<PopularStateT> list = (List<PopularStateT>)queryObj.List<PopularStateT>();

                return list;
            }
        }

        public void AddSearchText(PopularT pop)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    PopularT popular = session.QueryOver<PopularT>().Where(p => p.Word == pop.Word && p.RegDt == pop.RegDt).SingleOrDefault<PopularT>();
                    if (popular == null)
                    {
                        session.Save(pop);
                    }
                    else
                    {
                        popular.Cnt = popular.Cnt + 1;
                        session.Update(popular);
                    }
                    transaction.Commit();
                    session.Flush();
                }
            }
        }
    }
}
