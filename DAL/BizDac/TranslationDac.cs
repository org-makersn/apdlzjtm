using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makersn.Models;
using NHibernate;


namespace Makersn.BizDac
{
    public class TranslationDac
    {
        TranslationDetailDac _translationDetailDac = new TranslationDetailDac();

        #region 번역현황
        public IList<TranslationStateT> GetTranslationStatus(string start, string end)
        {
            string[] arrGbn = { "오늘", "어제", "이번주", "지난주", "이번달", "지난달", "기간선택" };
            string query = @"select
	                            Gbn,
								ISNULL([All], 0) as AllCnt,
								ISNULL([Direct], 0) as DirectCnt,
								ISNULL([Request], 0) as RequestCnt,
								ISNULL([Complete], 0) as CompleteCnt,
								ISNULL([EnForKr], 0) as EnForKrCnt,
								ISNULL([KrForEn], 0) as KrForEnCnt
                            from
                            (
	                            select 
		                            '전체' as Gbn,'All' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            union all
	                            select 
		                            '전체' as Gbn,'Direct' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 2
	                            union all
	                            select 
		                            '전체' as Gbn,'Request' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1
	                            union all
	                            select 
		                            '전체' as Gbn,'Complete' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 2
	                            union all
	                            select 
		                            '전체' as Gbn,'EnForKr' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where LANG_FROM = 'EN' AND LANG_TO = 'KR'
	                            union all
	                            select 
		                            '전체' as Gbn,'KrForEn' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where LANG_FROM = 'KR' AND LANG_TO = 'EN'
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([All],[Direct],[Request],[Complete],[EnForKr],[KrForEn])
                            ) as pvt ";

            string add_where = string.Empty;

            foreach (var gbn in arrGbn)
            {
                switch (gbn)
                {
                    case "오늘":
                        add_where = string.Format(" {0} ", "datediff(day,REG_DT,getdate())=0");
                        break;
                    case "어제":
                        add_where = string.Format(" {0} ", "datediff(day,REG_DT,getdate())=1");
                        break;
                    case "이번주":
                        add_where = string.Format(" REG_DT >= {0} AND REG_DT < {1} ", "dateadd(day,2-datepart(weekday,getdate()),convert(varchar,getdate(),112))", "dateadd(day,9-datepart(weekday,getdate()),convert(varchar,getdate(),112))");
                        //add_where = string.Format(" and {0} ", "datediff(week,REG_DT-1,getdate())=0");
                        break;
                    case "지난주":
                        add_where = string.Format(" REG_DT >= {0} AND REG_DT < {1} ", "dateadd(day,-5-datepart(weekday,getdate()),convert(varchar,getdate(),112))", "dateadd(day,2-datepart(weekday,getdate()),convert(varchar,getdate(),112))");
                        //add_where = string.Format(" and {0} ", "datediff(week,REG_DT-1,getdate())=1");
                        break;
                    case "이번달":
                        add_where = string.Format(" REG_DT >= {0} AND REG_DT < {1} ", "dateadd(day,1-day(getdate()),convert(varchar,getdate(),112))", "dateadd(month,1,dateadd(day,1-day(getdate()),convert(varchar,getdate(),112)))");
                        //add_where = string.Format(" and {0} ", "datediff(month,REG_DT,getdate())=0");
                        break;
                    case "지난달":
                        add_where = string.Format(" REG_DT >= {0} AND REG_DT < {1} ", "dateadd(month,-1,dateadd(day,1-day(getdate()),convert(varchar,getdate(),112)))", "dateadd(day,1-day(getdate()),convert(varchar,getdate(),112))");
                        //add_where = string.Format(" and {0} ", "datediff(month,REG_DT,getdate())=1");
                        break;
                    case "기간선택":
                        add_where = " REG_DT >= :start AND REG_DT <= :end ";
                        break;
                    default:
                        add_where = " 1 = 1 ";
                        break;
                }
                //if (gbn == "기간선택") continue;

                query += string.Format(@" union all select
	                            Gbn,
								ISNULL([All], 0) as AllCnt,
								ISNULL([Direct], 0) as DirectCnt,
								ISNULL([Request], 0) as RequestCnt,
								ISNULL([Complete], 0) as CompleteCnt,
								ISNULL([EnForKr], 0) as EnForKrCnt,
								ISNULL([KrForEn], 0) as KrForEnCnt
                            from
                            (
	                            select 
		                            '{0}' as Gbn,'All' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
                                where {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'Direct' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 2 and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'Request' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'Complete' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 2 and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'EnForKr' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where LANG_FROM = 'EN' AND LANG_TO = 'KR' and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'KrForEn' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where LANG_FROM = 'KR' AND LANG_TO = 'EN' and {1}
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([All],[Direct],[Request],[Complete],[EnForKr],[KrForEn])
                            ) as pvt ", gbn, add_where);
            }
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(TranslationStateT));
                queryObj.SetParameter("start", start);
                queryObj.SetParameter("end", end + " 23:59:59 ");
                IList<TranslationStateT> result = (IList<TranslationStateT>)queryObj.List<TranslationStateT>();

                return result;
            }
        }

        /// <summary>
        /// get article state - daily
        /// </summary>
        /// <returns></returns>
        public IList<TranslationStateT> GetTranslationStatusTargetDaily(string start, string end)
        {
            string query = @"
							select
	                            Gbn,
								ISNULL([All], 0) as AllCnt,
								ISNULL([Direct], 0) as DirectCnt,
								ISNULL([Request], 0) as RequestCnt,
								ISNULL([Complete], 0) as CompleteCnt,
								ISNULL([EnForKr], 0) as EnForKrCnt,
								ISNULL([KrForEn], 0) as KrForEnCnt
                            from
                            (
	                            select 
		                            convert(date, REG_DT) as Gbn,'All' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
                                where  REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'Direct' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 2  and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'Request' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1  and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'Complete' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 2  and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'EnForKr' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where LANG_FROM = 'EN' AND LANG_TO = 'KR'  and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'KrForEn' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where LANG_FROM = 'KR' AND LANG_TO = 'EN'  and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([All],[Direct],[Request],[Complete],[EnForKr],[KrForEn])
                            ) as pvt";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(TranslationStateT));
                queryObj.SetParameter("start", start);
                queryObj.SetParameter("end", end + " 23:59:59 ");
                IList<TranslationStateT> result = (IList<TranslationStateT>)queryObj.List<TranslationStateT>();

                return result;
            }
        }

        /// <summary>
        /// get article state - month
        /// </summary>
        /// <returns></returns>
        public IList<TranslationStateT> GetTranslationStatusTargetMonth(string start, string end)
        {
            string query = @"
							select
	                            Gbn,
								ISNULL([All], 0) as AllCnt,
								ISNULL([Direct], 0) as DirectCnt,
								ISNULL([Request], 0) as RequestCnt,
								ISNULL([Complete], 0) as CompleteCnt,
								ISNULL([EnForKr], 0) as EnForKrCnt,
								ISNULL([KrForEn], 0) as KrForEnCnt
                            from
                            (
	                            select 
		                            month(REG_DT) as Gbn,'All' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
                                where  REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'Direct' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 2  and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'Request' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1  and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'Complete' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 2  and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'EnForKr' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where LANG_FROM = 'EN' AND LANG_TO = 'KR'  and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'KrForEn' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where LANG_FROM = 'KR' AND LANG_TO = 'EN'  and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([All],[Direct],[Request],[Complete],[EnForKr],[KrForEn])
                            ) as pvt";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(TranslationStateT));
                queryObj.SetParameter("start",start);
                queryObj.SetParameter("end", end + " 23:59:59 ");
                IList<TranslationStateT> result = (IList<TranslationStateT>)queryObj.List<TranslationStateT>();

                return result;
            }
        }

        /// <summary>
        /// get article state - year
        /// </summary>
        /// <returns></returns>
        public IList<TranslationStateT> GetTranslationStatusTargetYear()
        {
            string query = @"select
	                            Gbn,
								ISNULL([All], 0) as AllCnt,
								ISNULL([Direct], 0) as DirectCnt,
								ISNULL([Request], 0) as RequestCnt,
								ISNULL([Complete], 0) as CompleteCnt,
								ISNULL([EnForKr], 0) as EnForKrCnt,
								ISNULL([KrForEn], 0) as KrForEnCnt
                            from
                            (
	                            select 
		                            year(REG_DT) as Gbn,'All' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'Direct' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 2 
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'Request' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 11 
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'Complete' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 2  
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'EnForKr' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where LANG_FROM = 'EN' AND LANG_TO = 'KR'  
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'KrForEn' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where LANG_FROM = 'KR' AND LANG_TO = 'EN' 
	                            group by year(REG_DT)
	                            
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([All],[Direct],[Request],[Complete],[EnForKr],[KrForEn])
                            ) as pvt";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(TranslationStateT));

                IList<TranslationStateT> result = (IList<TranslationStateT>)queryObj.List<TranslationStateT>();

                return result;
            }
        }

        #endregion

        #region 번역요청 현황

        /// <summary>
        /// 번역요청현황 초기화면
        /// </summary>
        /// <returns></returns>
        public IList<TranslationStateRequestT> GetTranslationRequestStatus(string start, string end)
        {
            string[] arrGbn = { "오늘", "어제", "이번주", "지난주", "이번달", "지난달", "기간선택" };
            string query = @"select
	                            Gbn,
								ISNULL([Request], 0) as RequestCnt,
								ISNULL([Complete], 0) as CompleteCnt,
                                ISNULL([Hold], 0) as HoldCnt,
								ISNULL([EnForKr], 0) as EnForKrCnt,
								ISNULL([KrForEn], 0) as KrForEnCnt
                            from
                            (
	                            select 
		                            '전체' as Gbn,'Request' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
                                where TRANS_FLAG = 1 AND STATUS = 1
	                            union all
	                            select 
		                            '전체' as Gbn,'Complete' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 2
	                            union all
	                            select 
		                            '전체' as Gbn,'Hold' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 3 
	                            union all
	                            select 
		                            '전체' as Gbn,'EnForKr' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 and LANG_FROM = 'EN' AND LANG_TO = 'KR'
	                            union all
	                            select 
		                            '전체' as Gbn,'KrForEn' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 and LANG_FROM = 'KR' AND LANG_TO = 'EN'
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([Request],[Complete],[Hold],[EnForKr],[KrForEn])
                            ) as pvt ";

            string add_where = string.Empty;

            foreach (var gbn in arrGbn)
            {
                switch (gbn)
                {
                    case "오늘":
                        add_where = string.Format(" {0} ", "datediff(day,REG_DT,getdate())=0");
                        break;
                    case "어제":
                        add_where = string.Format(" {0} ", "datediff(day,REG_DT,getdate())=1");
                        break;
                    case "이번주":
                        add_where = string.Format(" REG_DT >= {0} AND REG_DT < {1} ", "dateadd(day,2-datepart(weekday,getdate()),convert(varchar,getdate(),112))", "dateadd(day,9-datepart(weekday,getdate()),convert(varchar,getdate(),112))");
                        //add_where = string.Format(" and {0} ", "datediff(week,REG_DT-1,getdate())=0");
                        break;
                    case "지난주":
                        add_where = string.Format(" REG_DT >= {0} AND REG_DT < {1} ", "dateadd(day,-5-datepart(weekday,getdate()),convert(varchar,getdate(),112))", "dateadd(day,2-datepart(weekday,getdate()),convert(varchar,getdate(),112))");
                        //add_where = string.Format(" and {0} ", "datediff(week,REG_DT-1,getdate())=1");
                        break;
                    case "이번달":
                        add_where = string.Format(" REG_DT >= {0} AND REG_DT < {1} ", "dateadd(day,1-day(getdate()),convert(varchar,getdate(),112))", "dateadd(month,1,dateadd(day,1-day(getdate()),convert(varchar,getdate(),112)))");
                        //add_where = string.Format(" and {0} ", "datediff(month,REG_DT,getdate())=0");
                        break;
                    case "지난달":
                        add_where = string.Format(" REG_DT >= {0} AND REG_DT < {1} ", "dateadd(month,-1,dateadd(day,1-day(getdate()),convert(varchar,getdate(),112)))", "dateadd(day,1-day(getdate()),convert(varchar,getdate(),112))");
                        //add_where = string.Format(" and {0} ", "datediff(month,REG_DT,getdate())=1");
                        break;
                    case "기간선택":
                        add_where = " REG_DT >= :start AND REG_DT <= :end ";
                        break;
                    default:
                        add_where = " 1 = 1 ";
                        break;
                }
                //if (gbn == "기간선택") continue;

                query += string.Format(@" union all select
	                            Gbn,
								ISNULL([Request], 0) as RequestCnt,
								ISNULL([Complete], 0) as CompleteCnt,
                                ISNULL([Hold], 0) as HoldCnt,
								ISNULL([EnForKr], 0) as EnForKrCnt,
								ISNULL([KrForEn], 0) as KrForEnCnt
                            from
                            (	                            
	                            select 
		                            '{0}' as Gbn,'Request' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 1 and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'Complete' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 2 and {1}
	                            union all
                                select 
		                            '{0}' as Gbn,'Hold' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 3 and {1}
                                union all
	                            select 
		                            '{0}' as Gbn,'EnForKr' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 and LANG_FROM = 'EN' AND LANG_TO = 'KR' and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'KrForEn' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 and LANG_FROM = 'KR' AND LANG_TO = 'EN' and {1}
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([Request],[Complete],[Hold],[EnForKr],[KrForEn])
                            ) as pvt ", gbn, add_where);;
            }
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(TranslationStateRequestT));
                queryObj.SetParameter("start", start);
                queryObj.SetParameter("end", end + " 23:59:59 ");
                IList<TranslationStateRequestT> result = (IList<TranslationStateRequestT>)queryObj.List<TranslationStateRequestT>();

                return result;
            }
        }

        /// <summary>
        /// get article state - daily
        /// </summary>
        /// <returns></returns>
        public IList<TranslationStateRequestT> GetTranslationRequestStatusTargetDaily(string start, string end)
        {
            string query = @"
							select
	                            Gbn,
								ISNULL([Request], 0) as RequestCnt,
								ISNULL([Complete], 0) as CompleteCnt,
                                ISNULL([Hold], 0) as HoldCnt,
								ISNULL([EnForKr], 0) as EnForKrCnt,
								ISNULL([KrForEn], 0) as KrForEnCnt
                            from
                            (	                            
	                            select 
		                            convert(date, REG_DT) as Gbn,'Request' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 1 and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'Complete' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 2  and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
                                union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'Hold' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 3  and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'EnForKr' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 and LANG_FROM = 'EN' AND LANG_TO = 'KR'  and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'KrForEn' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 and LANG_FROM = 'KR' AND LANG_TO = 'EN'  and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([Request],[Complete],[Hold],[EnForKr],[KrForEn])
                            ) as pvt";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(TranslationStateRequestT));
                queryObj.SetParameter("start", start);
                queryObj.SetParameter("end", end + " 23:59:59 ");

                IList<TranslationStateRequestT> result = (IList<TranslationStateRequestT>)queryObj.List<TranslationStateRequestT>();

                return result;
            }
        }

        /// <summary>
        /// get article state - month
        /// </summary>
        /// <returns></returns>
        public IList<TranslationStateRequestT> GetTranslationRequestStatusTargetMonth(string start, string end)
        {
            string query = @"
							select
	                            Gbn,
								ISNULL([Request], 0) as RequestCnt,
								ISNULL([Complete], 0) as CompleteCnt,
                                ISNULL([Hold], 0) as HoldCnt,
								ISNULL([EnForKr], 0) as EnForKrCnt,
								ISNULL([KrForEn], 0) as KrForEnCnt
                            from
                            (
	                            select 
		                            month(REG_DT) as Gbn,'Request' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1  AND STATUS = 1  and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'Complete' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 2 and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
                                select 
		                            month(REG_DT) as Gbn,'Hold' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 3  and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'EnForKr' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND LANG_FROM = 'EN' AND LANG_TO = 'KR'  and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'KrForEn' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND LANG_FROM = 'KR' AND LANG_TO = 'EN'  and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([Request],[Complete],[Hold],[EnForKr],[KrForEn])
                            ) as pvt";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(TranslationStateRequestT));
                queryObj.SetParameter("start", start);
                queryObj.SetParameter("end", end + " 23:59:59 ");

                IList<TranslationStateRequestT> result = (IList<TranslationStateRequestT>)queryObj.List<TranslationStateRequestT>();

                return result;
            }
        }

        /// <summary>
        /// get article state - year
        /// </summary>
        /// <returns></returns>
        public IList<TranslationStateRequestT> GetTranslationRequestStatusTargetYear()
        {
            string query = @"select
	                            Gbn,
								ISNULL([Request], 0) as RequestCnt,
								ISNULL([Complete], 0) as CompleteCnt,
                                ISNULL([Hold], 0) as HoldCnt,
								ISNULL([EnForKr], 0) as EnForKrCnt,
								ISNULL([KrForEn], 0) as KrForEnCnt
                            from
                            (
	                            select 
		                            year(REG_DT) as Gbn,'Request' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 1  
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'Complete' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 2  
	                            group by year(REG_DT)
	                            union all
                                select 
		                            year(REG_DT) as Gbn,'Hold' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = 1 AND STATUS = 3  
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'EnForKr' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where LANG_FROM = 'EN' AND LANG_TO = 'KR'  
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'KrForEn' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where LANG_FROM = 'KR' AND LANG_TO = 'EN' 
	                            group by year(REG_DT)
	                            
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([Request],[Complete],[Hold],[EnForKr],[KrForEn])
                            ) as pvt";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(TranslationStateRequestT));
                IList<TranslationStateRequestT> result = (IList<TranslationStateRequestT>)queryObj.List<TranslationStateRequestT>();

                return result;
            }
        }

        /// <summary>
        /// 번역요청리스트
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="codeNo">카테고리 넘버 기본값 0(전체검색)</param>
        /// <param name="isMain">디자인 메인페이지인지 아닌지를 구분함(Y -TOP 4 N - ALL)</param>
        /// <param name="gubun">추천,인기,신규 구분(R, P, N) 카테고리 페이지는 N이나 공백 사용하면 됨</param>
        /// <param name="fromPage"></param>
        /// <param name="toPage"></param>
        /// <returns></returns>
        public IList<TranslationRequestListT> GetTranslationRequestByOption(int codeNo, string text, int? transFlag, int? status)
        {
            int downloadScore = 40;
            int commentScore = 20;
            int likeScore = 10;
            int viewScore = 5;

            string query = @"SELECT T.NO, ISNULL(AF.IMG_NAME, AF.RENAME) AS MAIN_IMAGE, TD.TITLE, C.NAME AS CATEGORY, M.NAME AS MEMBER_NAME , A.REG_DT, A.VISIBILITY, AF.PATH, A.VIEWCNT,
 
                             				(
						        --(A.VIEWCNT * 5) + 
		                        ((SELECT Count(0) FROM DOWNLOAD B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO AND B.REG_DT >= DATEADD(D, -7, GETDATE())) * 40 ) + 
		                        ((SELECT count(0) FROM ARTICLE_COMMENT B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO AND B.REG_DT >= DATEADD(D, -7, GETDATE())) * 20 ) + 
		                        ((SELECT count(0) FROM LIKES B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO AND B.REG_DT <= DATEADD(D, -7, GETDATE())) * 10 )
	                        ) AS POP,
   
                            (   SELECT count(0) 
                                       FROM   ARTICLE_COMMENT B 
                                       WHERE  A.NO = B.ARTICLE_NO) AS COMMENT_CNT,
                            (   SELECT count(0) 
                                    FROM   LIKES B 
                                    WHERE  A.NO = B.ARTICLE_NO) AS LIKE_CNT,
                            A.MEMBER_NO, A.COPYRIGHT, A.RECOMMEND_YN, A.RECOMMEND_VISIBILITY, A.RECOMMEND_PRIORITY, A.RECOMMEND_DT,
							(SELECT COUNT(0) FROM DOWNLOAD D WITH(NOLOCK) WHERE A.NO = D.ARTICLE_NO) AS DOWNLOAD,
                            T.NO, T.TRANS_FLAG, T.STATUS, T.LANG_FROM, T.LANG_TO, RM.NAME 'REQ_MEM_NAME', T.REQ_DT, WM.NAME 'WORK_MEM_NAME', T.WORK_DT,
                            T.TEMP_FLAG, T.REG_ID, T.REG_DT, T.STATUS
                            
                            FROM ARTICLE A WITH(NOLOCK) INNER JOIN ARTICLE_FILE AS AF WITH(NOLOCK)
					                            ON A.MAIN_IMAGE = AF.NO
					                            LEFT JOIN CODE AS C WITH(NOLOCK)
					                            ON A.CODE_NO = C.NO
					                            LEFT JOIN MEMBER AS M WITH(NOLOCK)
					                            ON A.MEMBER_NO = M.NO
                                                JOIN TRANSLATION AS T WITH(NOLOCK)
                                                ON A.NO = T.ARTICLE_NO
                                                JOIN TRANSLATION_DETAIL TD WITH(NOLOCK)
                                                ON A.NO = TD.ARTICLE_NO AND TD.LANG_FLAG = T.LANG_FROM -- AND TD.TRANSLATION_NO = T.NO
                                                LEFT JOIN MEMBER RM
                                                ON T.REQ_MEMBER_NO = RM.NO
                                                LEFT JOIN MEMBER WM
                                                ON T.WORK_MEMBER_NO = WM.NO
                                                WHERE (M.DEL_FLAG != 'Y' OR M.DEL_FLAG IS NULL) AND T.TRANS_FLAG = 1 ";

            if (codeNo != 0) { query += " AND A.CODE_NO = :codeNo "; };
            //if (option != "") { query += " AND A.TITLE = " + option; };
            if (text != "") { query += " AND TD.TITLE LIKE :text "; };
            if (transFlag != null && transFlag != 0) { query += " AND T.TRANS_FLAG = :transFlag" ; }
            if (status != null && status != 0) { query += " AND T.STATUS = :status "; }
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                if (query.Contains(":codeNo"))
                {
                    queryObj.SetParameter("codeNo", codeNo);
                }
                if (query.Contains(":text"))
                {
                    queryObj.SetParameter("text", "%" + text + "%");
                }
                if (query.Contains(":transFlag"))
                {
                    queryObj.SetParameter("transFlag", transFlag);
                }
                if (query.Contains(":status"))
                {
                    queryObj.SetParameter("status", status);
                }

                IList<object[]> results = queryObj.List<object[]>();
                session.Flush();


                IList<TranslationRequestListT> list = new List<TranslationRequestListT>();
                foreach (object[] row in results)
                {
                    TranslationRequestListT translationRequestList = new TranslationRequestListT();
                    translationRequestList.No = (int)row[0];
                    translationRequestList.ImageName = (string)row[1];
                    translationRequestList.Title = (string)row[2];
                    translationRequestList.Category = (string)row[3];
                    translationRequestList.MemberName = (string)row[4];
                    translationRequestList.RegDt = (DateTime)row[5];
                    translationRequestList.Visibility = (string)row[6];
                    translationRequestList.Path = (string)row[7] + translationRequestList.ImageName;
                    translationRequestList.ViewCnt = (int)row[8];
                    translationRequestList.Pop = (int)row[9];
                    translationRequestList.CommentCnt = (int)row[10];
                    translationRequestList.LikeCnt = (int)row[11];
                    translationRequestList.MemberNo = (int)row[12];
                    translationRequestList.Copyright = (int)row[13]; //테스트용 값임 빼도됨
                    translationRequestList.RecommendYn = (string)row[14];
                    translationRequestList.RecommendVisibility = (string)row[15];
                    if (row[16] != null)
                        translationRequestList.RecommendPriority = (int)row[16];
                    else
                        translationRequestList.RecommendPriority = 0;
                    if (row[17] != null)
                        translationRequestList.RecommendDt = (DateTime)row[17];
                    translationRequestList.DownloadCnt = (int)row[18];
                    translationRequestList.TransNo = (int)row[19];
                    translationRequestList.TransFlag = System.Convert.ToInt32(row[20]);
                    translationRequestList.TransStatus = System.Convert.ToInt32(row[21]);
                    translationRequestList.TransLanFrom = (string)row[22];
                    translationRequestList.TransLanTo = (string)row[23];
                    translationRequestList.TransReqMemName = (string)row[24];
                    if (row[25] != null)
                        translationRequestList.TransReqDt = (DateTime)row[25];
                    translationRequestList.TransWorkMemName = (string)row[26];
                    if (row[27] != null)
                        translationRequestList.TransWorkDt = (DateTime)row[27];
                    translationRequestList.TransRegID = (string)row[29];
                    if (row[30] != null)
                        translationRequestList.TransRegDt = (DateTime)row[30];

                    if ((int)row[31] == 1)
                    {
                        translationRequestList.Remark = string.Format("<span class='btnWrap medium'><a href='javascript:translation.Hold({0})'>보류</a></span>", translationRequestList.No);
                    }
                    else
                    {
                        translationRequestList.Remark = Enum.GetName(typeof(Makersn.Util.MakersnEnumTypes.TranslationStatus),(int)row[31]) ;
                    }

                    list.Add(translationRequestList);
                }

                return list;
            }
        }

        public void UpdateTranslationStatus(TranslationT translationData)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    TranslationT updTranslation = session.QueryOver<TranslationT>().Where(a => a.No == translationData.No).SingleOrDefault<TranslationT>();
                    if (updTranslation != null)
                    {
                        updTranslation.No = translationData.No;
                        updTranslation.Status = translationData.Status;
                        updTranslation.UpdId = translationData.UpdId;
                        updTranslation.UpdDt = translationData.UpdDt;
                        session.Update(updTranslation);
                    }
                    transaction.Commit();
                    session.Flush();
                }
            }
        }

        public TranslationT GetTranslationByTranslationNo(int translationNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<TranslationT>().Where(w => w.No == translationNo).Take(1).SingleOrDefault<TranslationT>();
            }
        }

        public TranslationDetailT GetTranslationDetail(int articleNo, string langFlag)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<TranslationDetailT>().Where(w => w.ArticleNo == articleNo && w.LangFlag == langFlag).Take(1).SingleOrDefault<TranslationDetailT>();
            }
        }
        #endregion

        #region 직접번역

        public IList<ArticleT> SearchArticleWithTranslation(int codeNo, string text, int? transFlag, int? status)
        {
            int downloadScore = 40;
            int commentScore = 20;
            int likeScore = 10;
            int viewScore = 5;

            string query = @"SELECT A.NO, ISNULL(AF.IMG_NAME, AF.RENAME) AS MAIN_IMAGE, TD.TITLE, C.NAME AS CATEGORY, M.NAME AS MEMBER_NAME , A.REG_DT, A.VISIBILITY, AF.PATH, A.VIEWCNT,

                             				(
						        --(A.VIEWCNT * " + viewScore + @") + 
		                        ((SELECT Count(0) FROM DOWNLOAD B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO AND B.REG_DT >= DATEADD(D, -7, GETDATE())) * " + downloadScore + @" ) + 
		                        ((SELECT count(0) FROM ARTICLE_COMMENT B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO AND B.REG_DT >= DATEADD(D, -7, GETDATE())) * " + commentScore + @" ) + 
		                        ((SELECT count(0) FROM LIKES B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO AND B.REG_DT <= DATEADD(D, -7, GETDATE())) * " + likeScore + @" )
	                        ) AS POP,
   
                            (   SELECT count(0) 
                                       FROM   ARTICLE_COMMENT B 
                                       WHERE  A.NO = B.ARTICLE_NO) AS COMMENT_CNT,
                            (   SELECT count(0) 
                                    FROM   LIKES B 
                                    WHERE  A.NO = B.ARTICLE_NO) AS LIKE_CNT,
                            A.MEMBER_NO, A.COPYRIGHT, A.RECOMMEND_YN, A.RECOMMEND_VISIBILITY, A.RECOMMEND_PRIORITY, A.RECOMMEND_DT,
							(SELECT COUNT(0) FROM DOWNLOAD D WITH(NOLOCK) WHERE A.NO = D.ARTICLE_NO) AS DOWNLOAD,
                            T.NO, T.TRANS_FLAG, T.STATUS, T.LANG_FROM, T.LANG_TO, RM.NAME 'REQ_MEM_NAME', T.REQ_DT, WM.NAME 'WORK_MEM_NAME', T.WORK_DT,
                            T.TEMP_FLAG, T.REG_ID, T.REG_DT


                            FROM ARTICLE A WITH(NOLOCK) INNER JOIN ARTICLE_FILE AS AF WITH(NOLOCK)
					                            ON A.MAIN_IMAGE = AF.NO
					                            LEFT JOIN CODE AS C WITH(NOLOCK)
					                            ON A.CODE_NO = C.NO
					                            LEFT JOIN MEMBER AS M WITH(NOLOCK)
					                            ON A.MEMBER_NO = M.NO
                                                JOIN TRANSLATION AS T WITH(NOLOCK)
                                                ON A.NO = T.ARTICLE_NO
                                                JOIN TRANSLATION_DETAIL TD WITH(NOLOCK)
                                                ON A.NO = TD.ARTICLE_NO AND TD.LANG_FLAG = T.LANG_FROM 
                                                JOIN MEMBER RM
                                                ON T.REQ_MEMBER_NO = RM.NO
                                                JOIN MEMBER WM
                                                ON T.WORK_MEMBER_NO = WM.NO
                                                WHERE (M.DEL_FLAG != 'Y' OR M.DEL_FLAG IS NULL)";

            if (codeNo != 0) { query += " AND A.CODE_NO = :codeNo "; };
            //if (option != "") { query += " AND A.TITLE = " + option; };
            if (text != "") { query += " AND TD.TITLE LIKE :text  "; };
            if (transFlag != null) { query += " AND T.TRANS_FLAG = :transFlag "; }
            if (status != null) { query += " AND T.STATUS = :status "; }
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                if (query.Contains(":codeNo"))
                {
                    queryObj.SetParameter("codeNo", codeNo);
                }
                if (query.Contains(":text"))
                {
                    queryObj.SetParameter("text", "%" + text + "%");
                }
                if (query.Contains(":transFlag"))
                {
                    queryObj.SetParameter("transFlag", transFlag);
                }
                if (query.Contains(":status"))
                {
                    queryObj.SetParameter("status", status);
                }

                IList<object[]> results = queryObj.List<object[]>();
                session.Flush();


                IList<ArticleT> list = new List<ArticleT>();
                foreach (object[] row in results)
                {
                    ArticleT article = new ArticleT();
                    article.No = (int)row[0];
                    article.ImageName = (string)row[1];
                    article.Title = (string)row[2];
                    article.Category = (string)row[3];
                    article.MemberName = (string)row[4];
                    article.RegDt = (DateTime)row[5];
                    article.Visibility = (string)row[6];
                    article.Path = (string)row[7] + article.ImageName;
                    article.ViewCnt = (int)row[8];
                    article.Pop = (int)row[9];
                    article.CommentCnt = (int)row[10];
                    article.LikeCnt = (int)row[11];
                    article.MemberNo = (int)row[12];
                    article.Copyright = (int)row[13]; //테스트용 값임 빼도됨
                    article.RecommendYn = (string)row[14];
                    article.RecommendVisibility = (string)row[15];
                    if (row[16] != null)
                        article.RecommendPriority = (int)row[16];
                    else
                        article.RecommendPriority = 0;
                    if (row[17] != null)
                        article.RecommendDt = (DateTime)row[17];
                    article.DownloadCnt = (int)row[18];
                    article.TransNo = (int)row[19];
                    article.TransFlag = System.Convert.ToInt32(row[20]);
                    article.TransStatus = System.Convert.ToInt32(row[21]);
                    article.TransLanFrom = (string)row[22];
                    article.TransLanTo = (string)row[23];
                    article.TransReqMemName = (string)row[24];
                    if (row[25] != null)
                        article.TransReqDt = (DateTime)row[25];
                    article.TransWorkMemName = (string)row[26];
                    if (row[27] != null)
                        article.TransWorkDt = (DateTime)row[27];
                    article.TransRegID = (string)row[29];
                    if (row[30] != null)
                        article.TransRegDt = (DateTime)row[30];
                    list.Add(article);
                }

                return list;
            }
        }

        public IList<TranslationStateDirectT> GetTranslationDirectStatus(string start, string end)
        {
            //gooksong

            string[] arrGbn = { "오늘", "어제", "이번주", "지난주", "이번달", "지난달", "기간선택" };
            string query = string.Format(@"select
	                            Gbn,
								ISNULL([Complete], 0) as CompleteCnt,
                                ISNULL([EnForKr], 0) as EnForKrCnt,
								ISNULL([KrForEn], 0) as KrForEnCnt
                            from
                            (
	                            select 
		                            '전체' as Gbn,'Complete' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = {0} AND STATUS = " + (int)Makersn.Util.MakersnEnumTypes.TranslationStatus.완료 +
                                @" union all
	                            select 
		                            '전체' as Gbn,'EnForKr' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = {0} AND LANG_FROM = 'EN' AND LANG_TO = 'KR'
	                            union all
	                            select 
		                            '전체' as Gbn,'KrForEn' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = {0} AND LANG_FROM = 'KR' AND LANG_TO = 'EN'
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([Complete],[EnForKr],[KrForEn])
                            ) as pvt ", (int)Makersn.Util.MakersnEnumTypes.TranslationFlag.직접번역);

            string add_where = string.Empty;

            foreach (var gbn in arrGbn)
            {
                switch (gbn)
                {
                    case "오늘":
                        add_where = string.Format(" {0} ", "datediff(day,REG_DT,getdate())=0");
                        break;
                    case "어제":
                        add_where = string.Format(" {0} ", "datediff(day,REG_DT,getdate())=1");
                        break;
                    case "이번주":
                        add_where = string.Format(" REG_DT >= {0} AND REG_DT < {1} ", "dateadd(day,2-datepart(weekday,getdate()),convert(varchar,getdate(),112))", "dateadd(day,9-datepart(weekday,getdate()),convert(varchar,getdate(),112))");
                        //add_where = string.Format(" and {0} ", "datediff(week,REG_DT-1,getdate())=0");
                        break;
                    case "지난주":
                        add_where = string.Format(" REG_DT >= {0} AND REG_DT < {1} ", "dateadd(day,-5-datepart(weekday,getdate()),convert(varchar,getdate(),112))", "dateadd(day,2-datepart(weekday,getdate()),convert(varchar,getdate(),112))");
                        //add_where = string.Format(" and {0} ", "datediff(week,REG_DT-1,getdate())=1");
                        break;
                    case "이번달":
                        add_where = string.Format(" REG_DT >= {0} AND REG_DT < {1} ", "dateadd(day,1-day(getdate()),convert(varchar,getdate(),112))", "dateadd(month,1,dateadd(day,1-day(getdate()),convert(varchar,getdate(),112)))");
                        //add_where = string.Format(" and {0} ", "datediff(month,REG_DT,getdate())=0");
                        break;
                    case "지난달":
                        add_where = string.Format(" REG_DT >= {0} AND REG_DT < {1} ", "dateadd(month,-1,dateadd(day,1-day(getdate()),convert(varchar,getdate(),112)))", "dateadd(day,1-day(getdate()),convert(varchar,getdate(),112))");
                        //add_where = string.Format(" and {0} ", "datediff(month,REG_DT,getdate())=1");
                        break;
                    case "기간선택":
                        add_where = " REG_DT >= :start AND REG_DT <= :end ";
                        break;
                    default:
                        add_where = " 1 = 1 ";
                        break;
                }
                //if (gbn == "기간선택") continue;

                query += string.Format(@" union all select
	                            Gbn,
								ISNULL([Complete], 0) as CompleteCnt,
                                ISNULL([EnForKr], 0) as EnForKrCnt,
								ISNULL([KrForEn], 0) as KrForEnCnt
                            from
                            (	         
	                            select 
		                            '{0}' as Gbn,'Complete' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
                                where TRANS_FLAG = {2} AND STATUS = " + (int)Makersn.Util.MakersnEnumTypes.TranslationStatus.완료 + @" and {1}
	                            union all
                                select 
		                            '{0}' as Gbn,'EnForKr' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG =  {2}  AND LANG_FROM = 'EN' AND LANG_TO = 'KR' and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'KrForEn' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG =  {2}  AND LANG_FROM = 'KR' AND LANG_TO = 'EN' and {1}
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([Complete],[EnForKr],[KrForEn])
                            ) as pvt ", gbn, add_where, (int)Makersn.Util.MakersnEnumTypes.TranslationFlag.직접번역);
            }
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(TranslationStateDirectT));
                queryObj.SetParameter("start", start);
                queryObj.SetParameter("end", end);
                IList<TranslationStateDirectT> result = (IList<TranslationStateDirectT>)queryObj.List<TranslationStateDirectT>();

                return result;
            }
        }

        /// <summary>
        /// get article state - daily
        /// </summary>
        /// <returns></returns>
        public IList<TranslationStateDirectT> GetTranslationDirectStatusTargetDaily(string start, string end)
        {
            string query = string.Format(@"
							select
	                            Gbn,
								ISNULL([Complete], 0) as CompleteCnt,
								ISNULL([EnForKr], 0) as EnForKrCnt,
								ISNULL([KrForEn], 0) as KrForEnCnt
                            from
                            (	       
	                            select 
		                            convert(date, REG_DT) as Gbn,'Complete' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = {0} AND STATUS = {1}  and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'EnForKr' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = {0} and LANG_FROM = 'EN' AND LANG_TO = 'KR'  and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'KrForEn' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = {0} and LANG_FROM = 'KR' AND LANG_TO = 'EN'  and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([Complete],[EnForKr],[KrForEn])
                            ) as pvt", (int)Makersn.Util.MakersnEnumTypes.TranslationFlag.직접번역, (int)Makersn.Util.MakersnEnumTypes.TranslationStatus.완료);

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(TranslationStateDirectT));
                queryObj.SetParameter("start", start);
                queryObj.SetParameter("end", end);
                IList<TranslationStateDirectT> result = (IList<TranslationStateDirectT>)queryObj.List<TranslationStateDirectT>();

                return result;
            }
        }

        /// <summary>
        /// get article state - month
        /// </summary>
        /// <returns></returns>
        public IList<TranslationStateDirectT> GetTranslationDirectStatusTargetMonth(string start, string end)
        {
            string query = string.Format(@"
							select
	                            Gbn,
								ISNULL([Complete], 0) as CompleteCnt,
								ISNULL([EnForKr], 0) as EnForKrCnt,
								ISNULL([KrForEn], 0) as KrForEnCnt
                            from
                            (
	                            
	                            select 
		                            month(REG_DT) as Gbn,'Complete' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = {0} AND STATUS = {1} and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'EnForKr' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = {0} AND LANG_FROM = 'EN' AND LANG_TO = 'KR'  and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'KrForEn' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = {0} AND LANG_FROM = 'KR' AND LANG_TO = 'EN'  and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([Complete],[EnForKr],[KrForEn])
                            ) as pvt", (int)Makersn.Util.MakersnEnumTypes.TranslationFlag.직접번역, (int)Makersn.Util.MakersnEnumTypes.TranslationStatus.완료);

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(TranslationStateDirectT));
                queryObj.SetParameter("start", start);
                queryObj.SetParameter("end", end);
                IList<TranslationStateDirectT> result = (IList<TranslationStateDirectT>)queryObj.List<TranslationStateDirectT>();

                return result;
            }
        }

        /// <summary>
        /// get article state - year
        /// </summary>
        /// <returns></returns>
        public IList<TranslationStateDirectT> GetTranslationDirectStatusTargetYear()
        {
            string query = string.Format(@"select
	                            Gbn,
								ISNULL([Complete], 0) as CompleteCnt,
								ISNULL([EnForKr], 0) as EnForKrCnt,
								ISNULL([KrForEn], 0) as KrForEnCnt
                            from
                            (
	                            
	                            select 
		                            year(REG_DT) as Gbn,'Complete' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = {0} AND STATUS = {1}  
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'EnForKr' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = {0} AND LANG_FROM = 'EN' AND LANG_TO = 'KR'  
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'KrForEn' as fortype, COUNT(1) as Total
	                            from TRANSLATION with(nolock)
	                            where TRANS_FLAG = {0} AND LANG_FROM = 'KR' AND LANG_TO = 'EN' 
	                            group by year(REG_DT)
	                            
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([Complete],[EnForKr],[KrForEn])
                            ) as pvt", (int)Makersn.Util.MakersnEnumTypes.TranslationFlag.직접번역, (int)Makersn.Util.MakersnEnumTypes.TranslationStatus.완료);

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(TranslationStateDirectT));
                IList<TranslationStateDirectT> result = (IList<TranslationStateDirectT>)queryObj.List<TranslationStateDirectT>();

                return result;
            }
        }

        #endregion

        public IList<object> GetArticleYearGroup()
        {
            string query = @"select year(reg_dt) as reg_dt from translation group by year(reg_dt) order by reg_dt desc";
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                IList<object> results = queryObj.List<object>();
                return results;
            }
        }

        public int InsertTranslation(TranslationT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return (int)session.Save(data);
            }
        }

        public TranslationT CheckTranslation(TranslationT data)
        {
            using (ISession session = NHibernateHelper.OpenSession()) 
            {
                return session.QueryOver<TranslationT>().Where(w => w.ArticleNo == data.ArticleNo && w.LangTo == data.LangTo).Take(1).SingleOrDefault<TranslationT>();
            }
        }

        public TranslationT GetTranslation(int articleNo, string langTo, int transFlag)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<TranslationT>().Where(w => w.ArticleNo == articleNo && w.LangTo == langTo && w.TransFlag == transFlag).Take(1).SingleOrDefault<TranslationT>();
            }
        }

        public void DeleteTranslation(TranslationT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    _translationDetailDac.DeleteTranslationDetail(data.No);
                        
                    session.Delete(data);
                    transaction.Commit();
                    session.Flush();
                }
            }
        }
    }
}
