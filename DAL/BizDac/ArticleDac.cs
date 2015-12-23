using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Makersn.Models;
using NHibernate.Criterion.Lambda;
using Makersn.Util;

namespace Makersn.BizDac
{
    public class ArticleDac : BizDacHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeNo"></param>
        /// <param name="RecommendYn"></param>
        /// <param name="option"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public IList<ArticleT> GetArticleListByAdminPage(int codeNo, string RecommendYn, string visilibity,string text)
        {
            int downloadScore = 40;
            int commentScore = 20;
            int likeScore = 10;
            int viewScore = 5;

            string query = @"SELECT A.NO, ISNULL(AF.IMG_NAME, AF.RENAME) AS MAIN_IMAGE, TD.TITLE
                            , C.NAME AS CATEGORY, M.NAME AS MEMBER_NAME , A.REG_DT, A.VISIBILITY, AF.PATH, A.VIEWCNT,

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
							(SELECT COUNT(0) FROM DOWNLOAD D WITH(NOLOCK) WHERE A.NO = D.ARTICLE_NO) AS DOWNLOAD


                            FROM ARTICLE A WITH(NOLOCK) INNER JOIN ARTICLE_FILE AS AF WITH(NOLOCK)
					                            ON A.MAIN_IMAGE = AF.NO
					                            LEFT JOIN CODE AS C WITH(NOLOCK)
					                            ON A.CODE_NO = C.NO
					                            LEFT JOIN MEMBER AS M WITH(NOLOCK)
					                            ON A.MEMBER_NO = M.NO
												INNER JOIN TRANSLATION_DETAIL TD WITH(NOLOCK)
												ON TD.ARTICLE_NO = A.NO AND TD.NO = (SELECT MIN(NO) FROM TRANSLATION_DETAIL TD2 WITH(NOLOCK) WHERE TD2.ARTICLE_NO = A.NO )
                                                WHERE (M.DEL_FLAG != 'Y' OR M.DEL_FLAG IS NULL)";

            if (codeNo != 0) { query += " AND C.NO = :codeNo"; };
            if (RecommendYn != "") { query += " AND A.RECOMMEND_YN = :RecommendYn"; };
            if (visilibity != "") { query += " AND A.VISIBILITY = :visilibity"; };
            //if (option != "") { query += " AND A.TITLE = " + option; };
            if (text != "") { query += " AND TD.TITLE LIKE :text"; };

            IQuery queryObj = Session.CreateSQLQuery(query);
            if (query.Contains(":codeNo"))
            {
                queryObj.SetParameter("codeNo", codeNo);
            }
            if (query.Contains(":RecommendYn"))
            {
                queryObj.SetParameter("RecommendYn", RecommendYn);
            }
            if (query.Contains(":visilibity"))
            {
                queryObj.SetParameter("visilibity", visilibity);
            }
            if (query.Contains(":text"))
            {
                queryObj.SetParameter("text", "%" + text + "%");
            }

            IList<object[]> results = queryObj.List<object[]>();
            Session.Flush();


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
                list.Add(article);
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<CodeT> GetArticleCodeNo()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<CodeT> list = Session.QueryOver<CodeT>().Where(c => c.CodeGbn == "ARTICLE").OrderBy(o => o.No).Desc.List();
                Session.Flush();
                return list;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <param name="visibility"></param>
        /// <returns></returns>
        public int UpdateVisibility(string no, string visibility)
        {
            string query = "UPDATE ARTICLE SET VISIBILITY = :visibility WHERE ";

            if (no.LastIndexOf(',') > 0)
            {
                no = no.Substring(0, no.LastIndexOf(','));
            }
            string[] noList = no.Split(',');
            for (int i = 0; i < noList.Length; i++)
            {
                query += " NO = ? ";
                if (i < noList.Length - 1)
                {
                    query += " OR ";
                }
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    IQuery queryObj = session.CreateSQLQuery(query);
                    for (int i = 0; i < noList.Length; i++)
                    {
                        queryObj.SetParameter(i, noList[i]);
                    }
                    queryObj.SetParameter("visibility", visibility);
                    queryObj.ExecuteUpdate();

                    transaction.Commit();

                    session.Flush();
                    return 1;
                }
            }
        }

        public int UpdateRecommend(string no, string recommend)
        {
            string query = "";
            if (recommend == "Y") { query = "UPDATE ARTICLE SET RECOMMEND_PRIORITY=(select max(RECOMMEND_PRIORITY) from article), RECOMMEND_DT = GETDATE(), RECOMMEND_YN = :recommend WHERE "; }
            else { query = "UPDATE ARTICLE SET RECOMMEND_PRIORITY=0,RECOMMEND_DT = null, RECOMMEND_YN = :recommend WHERE "; }

            if (no.LastIndexOf(',') > 0)
            {
                no = no.Substring(0, no.LastIndexOf(','));
            }
            string[] noList = no.Split(',');
            for (int i = 0; i < noList.Length; i++)
            {
                query += " NO = ? ";
                if (i < noList.Length - 1)
                {
                    query += " OR ";
                }
            }


            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {

                    IQuery queryObj = session.CreateSQLQuery(query);
                    for (int i = 0; i < noList.Length; i++)
                    {
                        queryObj.SetParameter(i, noList[i]);
                    }
                    queryObj.SetParameter("recommend", recommend);
                    queryObj.ExecuteUpdate();
                    transaction.Commit();
                    session.Flush();
                    return 1;
                }
            } 
        }


        /// <summary>
        /// reg_dt group_by year
        /// </summary>
        /// <returns></returns>
        public IList<object> GetArticleYearGroup()
        {
            string query = @"select year(reg_dt) as reg_dt from article group by year(reg_dt) order by reg_dt desc";
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<object> results = session.CreateSQLQuery(query).List<object>();
                return results;
            }
        }

        /// <summary>
        /// get article state - all
        /// </summary>
        /// <returns></returns>
        //        public IList<ArticleStateT> SearchArticleStateTargetAll()
        //        {
        //            string[] arrGbn = { "오늘", "어제", "이번주", "지난주", "이번달", "지난달", "기간선택" };
        //            string query = @"select
        //	                            Gbn,
        //								ISNULL([public], 0) as PublicCnt,
        //								ISNULL([Saved], 0) as SavedCnt,
        //								ISNULL([Download], 0) as DownloadCnt,
        //								ISNULL([Printing], 0) as PrintingCnt,  
        //								ISNULL([1001], 0) as CodeNo1001,
        //								ISNULL([1002], 0) as CodeNo1002,
        //								ISNULL([1003], 0) as CodeNo1003,
        //								ISNULL([1004], 0) as CodeNo1004,
        //								ISNULL([1005], 0) as CodeNo1005,
        //								ISNULL([1006], 0) as CodeNo1006
        //                            from
        //                            (
        //	                            select 
        //		                            '전체' as Gbn,'public' as fortype, COUNT(1) as Total
        //	                            from ARTICLE with(nolock)
        //	                            where  VISIBILITY = 'Y'
        //	                            union all
        //	                            select 
        //		                            '전체' as Gbn,'saved' as fortype, COUNT(1) as Total
        //	                            from ARTICLE with(nolock)
        //	                            where VISIBILITY = 'N'
        //	                            union all
        //	                            select 
        //		                            '전체' as Gbn,'1001' as fortype, COUNT(1) as Total
        //	                            from ARTICLE with(nolock)
        //	                            where CODE_NO = 1001
        //	                            union all
        //	                            select 
        //		                            '전체' as Gbn,'1002' as fortype, COUNT(1) as Total
        //	                            from ARTICLE with(nolock)
        //	                            where CODE_NO = 1002
        //	                            union all
        //	                            select 
        //		                            '전체' as Gbn,'1003' as fortype, COUNT(1) as Total
        //	                            from ARTICLE with(nolock)
        //	                            where CODE_NO = 1003
        //	                            union all
        //	                            select 
        //		                            '전체' as Gbn,'1004' as fortype, COUNT(1) as Total
        //	                            from ARTICLE with(nolock)
        //	                            where CODE_NO = 1004
        //	                            union all
        //	                            select 
        //		                            '전체' as Gbn,'1005' as fortype, COUNT(1) as Total
        //	                            from ARTICLE with(nolock)
        //	                            where CODE_NO = 1005
        //	                            union all
        //	                            select 
        //		                            '전체' as Gbn,'1006' as fortype, COUNT(1) as Total
        //	                            from ARTICLE with(nolock)
        //	                            where CODE_NO = 1006
        //	                            union all
        //	                            select 
        //		                            '전체' as Gbn,'download' as fortype, COUNT(1) as Total
        //	                            from DOWNLOAD with(nolock)
        //                                union all
        //                                select 
        //		                            '전체' as Gbn,'printing' as fortype, COUNT(1) as Total
        //	                            from ORDER_REQ with(nolock)
        //                            ) as tb
        //                            PIVOT
        //                            (
        //                                SUM(Total)
        //                                for fortype IN ([public],[saved],[1001],[1002],[1003],[1004],[1005],[1006],[download],[printing])
        //                            ) as pvt ";

        //            string add_where = string.Empty;

        //            foreach (var gbn in arrGbn)
        //            {
        //                switch (gbn)
        //                {
        //                    case "오늘":
        //                        add_where = string.Format(" {0} ", "datediff(day,REG_DT,getdate())=0");
        //                        break;
        //                    case "어제":
        //                        add_where = string.Format(" {0} ", "datediff(day,REG_DT,getdate())=1");
        //                        break;
        //                    case "이번주":
        //                        add_where = string.Format(" REG_DT >= {0} AND REG_DT < {1} ", "dateadd(day,2-datepart(weekday,getdate()),convert(varchar,getdate(),112))", "dateadd(day,9-datepart(weekday,getdate()),convert(varchar,getdate(),112))");
        //                        //add_where = string.Format(" and {0} ", "datediff(week,REG_DT-1,getdate())=0");
        //                        break;
        //                    case "지난주":
        //                        add_where = string.Format(" REG_DT >= {0} AND REG_DT < {1} ", "dateadd(day,-5-datepart(weekday,getdate()),convert(varchar,getdate(),112))", "dateadd(day,2-datepart(weekday,getdate()),convert(varchar,getdate(),112))");
        //                        //add_where = string.Format(" and {0} ", "datediff(week,REG_DT-1,getdate())=1");
        //                        break;
        //                    case "이번달":
        //                        add_where = string.Format(" REG_DT >= {0} AND REG_DT < {1} ", "dateadd(day,1-day(getdate()),convert(varchar,getdate(),112))", "dateadd(month,1,dateadd(day,1-day(getdate()),convert(varchar,getdate(),112)))");
        //                        //add_where = string.Format(" and {0} ", "datediff(month,REG_DT,getdate())=0");
        //                        break;
        //                    case "지난달":
        //                        add_where = string.Format(" REG_DT >= {0} AND REG_DT < {1} ", "dateadd(month,-1,dateadd(day,1-day(getdate()),convert(varchar,getdate(),112)))", "dateadd(day,1-day(getdate()),convert(varchar,getdate(),112))");
        //                        //add_where = string.Format(" and {0} ", "datediff(month,REG_DT,getdate())=1");
        //                        break;
        //                    default:
        //                        add_where = " 1 = 1 ";
        //                        break;
        //                }
        //                if (gbn == "기간선택") continue;

        //                query += string.Format(@" union all select
        //	                            Gbn,
        //								ISNULL([public], 0) as PublicCnt,
        //								ISNULL([Saved], 0) as SavedCnt,
        //								ISNULL([Download], 0) as DownloadCnt,
        //								ISNULL([Printing], 0) as PrintingCnt,  
        //								ISNULL([1001], 0) as CodeNo1001,
        //								ISNULL([1002], 0) as CodeNo1002,
        //								ISNULL([1003], 0) as CodeNo1003,
        //								ISNULL([1004], 0) as CodeNo1004,
        //								ISNULL([1005], 0) as CodeNo1005,
        //								ISNULL([1006], 0) as CodeNo1006
        //                            from
        //                            (
        //	                            select 
        //		                            '{0}' as Gbn,'public' as fortype, COUNT(1) as Total
        //	                            from ARTICLE with(nolock)
        //	                            where  VISIBILITY = 'Y' and {1}
        //	                            union all
        //	                            select 
        //		                            '{0}' as Gbn,'saved' as fortype, COUNT(1) as Total
        //	                            from ARTICLE with(nolock)
        //	                            where VISIBILITY = 'N' and {1}
        //	                            union all
        //	                            select 
        //		                            '{0}' as Gbn,'1001' as fortype, COUNT(1) as Total
        //	                            from ARTICLE with(nolock)
        //	                            where CODE_NO = 1001 and {1}
        //	                            union all
        //	                            select 
        //		                            '{0}' as Gbn,'1002' as fortype, COUNT(1) as Total
        //	                            from ARTICLE with(nolock)
        //	                            where CODE_NO = 1002 and {1}
        //	                            union all
        //	                            select 
        //		                            '{0}' as Gbn,'1003' as fortype, COUNT(1) as Total
        //	                            from ARTICLE with(nolock)
        //	                            where CODE_NO = 1003 and {1}
        //	                            union all
        //	                            select 
        //		                            '{0}' as Gbn,'1004' as fortype, COUNT(1) as Total
        //	                            from ARTICLE with(nolock)
        //	                            where CODE_NO = 1004 and {1}
        //	                            union all
        //	                            select 
        //		                            '{0}' as Gbn,'1005' as fortype, COUNT(1) as Total
        //	                            from ARTICLE with(nolock)
        //	                            where CODE_NO = 1005 and {1}
        //	                            union all
        //	                            select 
        //		                            '{0}' as Gbn,'1006' as fortype, COUNT(1) as Total
        //	                            from ARTICLE with(nolock)
        //	                            where CODE_NO = 1006 and {1}
        //	                            union all
        //	                            select 
        //		                            '{0}' as Gbn,'download' as fortype, COUNT(1) as Total
        //	                            from DOWNLOAD with(nolock)
        //	                            where {1}
        //                            ) as tb
        //                            PIVOT
        //                            (
        //                                SUM(Total)
        //                                for fortype IN ([public],[saved],[1001],[1002],[1003],[1004],[1005],[1006],[download],[printing])
        //                            ) as pvt", gbn, add_where);
        //            }
        //            using (ISession session = NHibernateHelper.OpenSession())
        //            {
        //                IList<ArticleStateT> result = (IList<ArticleStateT>)session.CreateSQLQuery(query).AddEntity(typeof(ArticleStateT)).List<ArticleStateT>();

        //                return result;
        //            }
        //        }

        /// <summary>
        /// get article state - daily
        /// </summary>
        /// <returns></returns>
        public IList<ArticleStateT> SearchArticleStateTargetDaily(string start, string end)
        {
            string query = @"select
	                            Gbn,
								ISNULL([public], 0) as PublicCnt,
								ISNULL([Saved], 0) as SavedCnt,
								ISNULL([Download], 0) as DownloadCnt,
								ISNULL([Printing], 0) as PrintingCnt,  
								ISNULL([1001], 0) as CodeNo1001,
								ISNULL([1002], 0) as CodeNo1002,
								ISNULL([1003], 0) as CodeNo1003,
								ISNULL([1004], 0) as CodeNo1004,
								ISNULL([1005], 0) as CodeNo1005,
								ISNULL([1006], 0) as CodeNo1006
                            from
                            (
	                            select 
		                            convert(date, REG_DT) as Gbn,'public' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where  VISIBILITY = 'Y' and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'saved' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where VISIBILITY = 'N' and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'1001' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1001 and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'1002' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1002 and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'1003' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1003 and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'1004' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1004 and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'1005' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1005 and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'1006' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1006 and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
                                union all                                
                                select convert(date, REG_DT) as Gbn,'printing' as fortype, COUNT(1) as Total
	                            from ORDER_REQ with(nolock)
	                            where ORDER_PATH = 'M' and ORDER_STATUS = " + (int)(MakersnEnumTypes.OrderState.거래완료) + @" and REG_DT < :end 
	                            group by convert(date, REG_DT)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([public],[saved],[1001],[1002],[1003],[1004],[1005],[1006],[download],[printing])
                            ) as pvt";

            using (ISession session = NHibernateHelper.OpenSession())
            {

                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(ArticleStateT));

                queryObj.SetParameter("start", start);


                queryObj.SetParameter("end", end);


                IList<ArticleStateT> result = (IList<ArticleStateT>)queryObj.List<ArticleStateT>();

                return result;
            }
        }

        /// <summary>
        /// get article state - month
        /// </summary>
        /// <returns></returns>
        public IList<ArticleStateT> SearchArticleStateTargetMonth(string start, string end)
        {
             string query = @"select
	                            Gbn,
								ISNULL([public], 0) as PublicCnt,
								ISNULL([Saved], 0) as SavedCnt,
								ISNULL([Download], 0) as DownloadCnt,
								ISNULL([Printing], 0) as PrintingCnt,  
								ISNULL([1001], 0) as CodeNo1001,
								ISNULL([1002], 0) as CodeNo1002,
								ISNULL([1003], 0) as CodeNo1003,
								ISNULL([1004], 0) as CodeNo1004,
								ISNULL([1005], 0) as CodeNo1005,
								ISNULL([1006], 0) as CodeNo1006
                            from
                            (
	                            select 
		                            month(REG_DT) as Gbn,'public' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where  VISIBILITY = 'Y' and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'saved' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where VISIBILITY = 'N' and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'1001' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1001 and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'1002' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1002 and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'1003' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1003 and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'1004' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1004 and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'1005' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1005 and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'1006' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1006 and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
                                union all                                
                                select month(REG_DT) as Gbn,'printing' as fortype, COUNT(1) as Total
	                            from ORDER_REQ with(nolock)
	                            where ORDER_PATH = 'M' and ORDER_STATUS = "+(int)(MakersnEnumTypes.OrderState.거래완료)+@" and REG_DT >= :start and REG_DT < :end 
	                            group by month(REG_DT)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([public],[saved],[1001],[1002],[1003],[1004],[1005],[1006],[download],[printing])
                            ) as pvt";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(ArticleStateT));

                queryObj.SetParameter("start", start);


                queryObj.SetParameter("end", end);


                IList<ArticleStateT> result = (IList<ArticleStateT>)queryObj.List<ArticleStateT>();

                return result;
            }
        }
        

        /// <summary>
        /// get article state - year
        /// </summary>
        /// <returns></returns>
        public IList<ArticleStateT> SearchArticleStateTargetYear()
        {
            string query = string.Format(@"select
	                            Gbn,
								ISNULL([public], 0) as PublicCnt,
								ISNULL([Saved], 0) as SavedCnt,
								ISNULL([Download], 0) as DownloadCnt,
								ISNULL([Printing], 0) as PrintingCnt,  
								ISNULL([1001], 0) as CodeNo1001,
								ISNULL([1002], 0) as CodeNo1002,
								ISNULL([1003], 0) as CodeNo1003,
								ISNULL([1004], 0) as CodeNo1004,
								ISNULL([1005], 0) as CodeNo1005,
								ISNULL([1006], 0) as CodeNo1006
                            from
                            (
	                            select 
		                            year(REG_DT) as Gbn,'public' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where  VISIBILITY = 'Y'
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'saved' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where VISIBILITY = 'N'
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'1001' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1001
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'1002' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1002
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'1003' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1003
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'1004' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1004
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'1005' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1005
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'1006' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1006
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock)
	                            group by year(REG_DT)

                                union all                                
                                select year(REG_DT) as Gbn,'printing' as fortype, COUNT(1) as Total
	                            from ORDER_REQ with(nolock)
	                            where ORDER_PATH = 'M' and ORDER_STATUS = '{0}'  
	                            group by year(REG_DT)

                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([public],[saved],[1001],[1002],[1003],[1004],[1005],[1006],[download],[printing])
                            ) as pvt", (int)(MakersnEnumTypes.OrderState.거래완료));

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<ArticleStateT> result = (IList<ArticleStateT>)session.CreateSQLQuery(query).AddEntity(typeof(ArticleStateT)).List<ArticleStateT>();

                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IList<ArticleStateT> SearchArticleStateTargetSaerch(string start, string end)
        {
            string[] arrGbn = { "오늘", "어제", "이번주", "지난주", "이번달", "지난달", "기간선택" };
            string query = @"select
	                            Gbn,
								ISNULL([public], 0) as PublicCnt,
								ISNULL([Saved], 0) as SavedCnt,
								ISNULL([Download], 0) as DownloadCnt,
								ISNULL([Printing], 0) as PrintingCnt,  
								ISNULL([1001], 0) as CodeNo1001,
								ISNULL([1002], 0) as CodeNo1002,
								ISNULL([1003], 0) as CodeNo1003,
								ISNULL([1004], 0) as CodeNo1004,
								ISNULL([1005], 0) as CodeNo1005,
								ISNULL([1006], 0) as CodeNo1006 
                            from
                            (
	                            select 
		                            '전체' as Gbn,'public' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where  VISIBILITY = 'Y'
	                            union all
	                            select 
		                            '전체' as Gbn,'saved' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where VISIBILITY = 'N'
	                            union all
	                            select 
		                            '전체' as Gbn,'1001' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1001
	                            union all
	                            select 
		                            '전체' as Gbn,'1002' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1002
	                            union all
	                            select 
		                            '전체' as Gbn,'1003' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1003
	                            union all
	                            select 
		                            '전체' as Gbn,'1004' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1004
	                            union all
	                            select 
		                            '전체' as Gbn,'1005' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1005
	                            union all
	                            select 
		                            '전체' as Gbn,'1006' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1006
	                            union all
	                            select 
		                            '전체' as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock)
                                union all
                                select 
		                            '전체' as Gbn,'printing' as fortype, COUNT(1) as Total
	                            from ORDER_REQ with(nolock)
                                where ORDER_PATH = 'M' and ORDER_STATUS = " + (int)(MakersnEnumTypes.OrderState.거래완료) + @" 
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([public],[saved],[1001],[1002],[1003],[1004],[1005],[1006],[download], [printing])
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
								ISNULL([public], 0) as PublicCnt,
								ISNULL([Saved], 0) as SavedCnt,
								ISNULL([Download], 0) as DownloadCnt,
								ISNULL([Printing], 0) as PrintingCnt, 
								ISNULL([1001], 0) as CodeNo1001,
								ISNULL([1002], 0) as CodeNo1002,
								ISNULL([1003], 0) as CodeNo1003,
								ISNULL([1004], 0) as CodeNo1004,
								ISNULL([1005], 0) as CodeNo1005,
								ISNULL([1006], 0) as CodeNo1006
                            from
                            (
	                            select 
		                            '{0}' as Gbn,'public' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where  VISIBILITY = 'Y' and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'saved' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where VISIBILITY = 'N' and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'1001' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1001 and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'1002' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1002 and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'1003' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1003 and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'1004' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1004 and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'1005' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1005 and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'1006' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where CODE_NO = 1006 and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock) 
                                where {1}
                                union all
	                            select 
		                            '{0}' as Gbn,'printing' as fortype, COUNT(1) as Total
	                            from ORDER_REQ with(nolock)
	                            where ORDER_PATH = 'M' and ORDER_STATUS = '{2}' and {1} 
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([public],[saved],[1001],[1002],[1003],[1004],[1005],[1006],[download],[printing])
                            ) as pvt", gbn, add_where, (int)(MakersnEnumTypes.OrderState.거래완료));
            }
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(ArticleStateT));

                queryObj.SetParameter("start", start);


                queryObj.SetParameter("end", end + " 23:59:59");


                IList<ArticleStateT> result = (IList<ArticleStateT>)queryObj.List<ArticleStateT>();

                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="gubun"></param>
        /// <param name="visitorNo"></param>
        /// <returns></returns>
        public IList<ArticleT> GetMemberArticleByNo(string memberNo, string gubun, int visitorNo)
        {
            string query = @"SELECT A.NO, ISNULL(AF.IMG_NAME, AF.RENAME) AS MAIN_IMAGE, 
							(SELECT TOP 1 TD.TITLE FROM TRANSLATION_DETAIL TD WITH(NOLOCK) WHERE TD.ARTICLE_NO = A.NO ORDER BY NO ASC ) AS TITLE, 
                            C.NAME AS CATEGORY, M.NAME AS MEMBER_NAME , A.REG_DT, A.VISIBILITY, AF.PATH, A.VIEWCNT,
                            (SELECT count(0) 
                                       FROM   ARTICLE_COMMENT B 
                                       WHERE  A.NO = B.ARTICLE_NO) AS COMMENT_CNT,
                            (SELECT count(0) 
                                    FROM   LIKES B 
                                    WHERE  A.NO = B.ARTICLE_NO) AS LIKE_CNT,
                            A.MEMBER_NO,
                            (SELECT count(0) 
                                    FROM   LIKES B 
                                    WHERE   
                                        A.NO = B.ARTICLE_NO AND B.MEMBER_NO = " + visitorNo + @" ) AS CHK_LIKES

                            FROM ARTICLE A WITH(NOLOCK) INNER JOIN ARTICLE_FILE AS AF WITH(NOLOCK)
					                            ON A.MAIN_IMAGE = AF.NO
					                            LEFT JOIN CODE AS C WITH(NOLOCK)
					                            ON A.CODE_NO = C.NO
					                            LEFT JOIN MEMBER AS M WITH(NOLOCK)
					                            ON A.MEMBER_NO = M.NO ";

            switch (gubun)
            {
                //                case "":
                //                    query += @" WHERE (M.DEL_FLAG != 'Y' OR M.DEL_FLAG IS NULL)
                //                                                AND A.VISIBILITY = 'Y'  AND A.MEMBER_NO = " + memberNo + " ORDER BY A.REG_DT DESC";
                //                    break;
                case "L":
                    query += @"INNER JOIN LIKES AS L WITH(NOLOCK)
												ON A.NO = L.ARTICLE_NO
                                                WHERE (M.DEL_FLAG != 'Y' OR M.DEL_FLAG IS NULL)
                                                AND A.VISIBILITY = 'Y' AND L.MEMBER_NO = " + memberNo + " ORDER BY L.REG_DT DESC";
                    break;
                case "D":
                    query += @" WHERE (M.DEL_FLAG != 'Y' OR M.DEL_FLAG IS NULL)
                                                AND A.VISIBILITY = 'N'  AND A.MEMBER_NO = " + memberNo + " ORDER BY A.REG_DT DESC";
                    break;
                default:
                    query += @" WHERE (M.DEL_FLAG != 'Y' OR M.DEL_FLAG IS NULL)
                                                AND A.VISIBILITY = 'Y'  AND A.MEMBER_NO = " + memberNo + " ORDER BY A.REG_DT DESC";
                    break;
            }


            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<object[]> results = session.CreateSQLQuery(query).List<object[]>();
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
                    article.CommentCnt = (int)row[9];
                    article.LikeCnt = (int)row[10];
                    article.MemberNo = (int)row[11];
                    article.chkLikes = (int)row[12];
                    list.Add(article);
                }

                return list;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public int GetUploadCntByMemberNo(int no)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<ArticleT>().Where(a => a.MemberNo == no).RowCount();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public int GetDraftCntByMemberNo(int no)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<ArticleT>().Where(a => a.MemberNo == no && a.Visibility != "Y").RowCount();
            }
        }

        #region 검색결과
        public IList<ArticleT> GetSearchList(string text, int memberNo, string tag, string globalType)
        {
            string query = @"SELECT A.NO, ISNULL(AF.IMG_NAME, AF.RENAME) AS MAIN_IMAGE, TD.TITLE, M.NAME AS MEMBER_NAME , AF.PATH, A.MEMBER_NO, A.VIEWCNT,
            
                                        (   SELECT count(0) 
                                                   FROM   ARTICLE_COMMENT B 
                                                   WHERE  A.NO = B.ARTICLE_NO) AS COMMENT_CNT,
                                        (   SELECT count(0) 
                                                FROM   LIKES B 
                                                WHERE  A.NO = B.ARTICLE_NO) AS LIKE_CNT,
                                        
                                        (SELECT count(0) 
                                                FROM   LIKES B 
                                                WHERE   
                                                    A.NO = B.ARTICLE_NO AND B.MEMBER_NO = :memberNo ) AS CHK_LIKES, A.CODE_NO
            
            
                                        FROM ARTICLE A WITH(NOLOCK) INNER JOIN ARTICLE_FILE AS AF WITH(NOLOCK)
            					                            ON A.MAIN_IMAGE = AF.NO
            					                            LEFT JOIN CODE AS C WITH(NOLOCK)
            					                            ON A.CODE_NO = C.NO
            					                            LEFT JOIN MEMBER AS M WITH(NOLOCK)
            					                            ON A.MEMBER_NO = M.NO
                                                            INNER JOIN TRANSLATION_DETAIL TD WITH(NOLOCK)
                                                            ON A.NO = TD.ARTICLE_NO
                                                            WHERE (M.DEL_FLAG != 'Y' OR M.DEL_FLAG IS NULL) AND A.VISIBILITY = 'Y'";

            if (text != "" && tag != "Y") { query += " AND (TD.TITLE LIKE :text OR TD.TAG LIKE :text OR TD.CONTENTS LIKE :text OR M.NAME LIKE :text)"; };
            if (text != "" && tag == "Y") { query += " AND TD.TAG LIKE :text"; };
            if (globalType != "") { query += "AND TD.LANG_FLAG = :globalType "; }
            else { query += " AND TD.NO IN (select MIN(TDL.NO) FROM TRANSLATION_DETAIL TDL WITH(NOLOCK) group by TDL.ARTICLE_NO)"; }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("text", "%" + text + "%");
                queryObj.SetParameter("memberNo", memberNo);
                queryObj.SetParameter("globalType", globalType);


                IList<object[]> results = queryObj.List<object[]>();

                //IList<object[]> results = session.CreateSQLQuery(query).List<object[]>();


                session.Flush();

                IList<ArticleT> list = new List<ArticleT>();
                foreach (object[] row in results)
                {
                    ArticleT article = new ArticleT();
                    article.No = (int)row[0];
                    article.ImageName = (string)row[1];
                    article.Title = (string)row[2];
                    article.MemberName = (string)row[3];
                    article.Path = (string)row[4] + article.ImageName;
                    article.MemberNo = (int)row[5];
                    article.ViewCnt = (int)row[6];
                    article.CommentCnt = (int)row[7];
                    article.LikeCnt = (int)row[8];
                    article.chkLikes = (int)row[9];
                    article.CodeNo = (int)row[10];
                    list.Add(article);
                }

                return list;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public ArticleT GetArticleByNo(int no)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ArticleT article = session.QueryOver<ArticleT>().Where(a => a.No == no).SingleOrDefault<ArticleT>();
                MemberT member = session.QueryOver<MemberT>().Where(w => w.No == article.MemberNo).SingleOrDefault<MemberT>();

                if (string.IsNullOrEmpty(member.ProfilePic))
                {
                    article.MemberProfilePic = "";
                }
                else
                {
                    //article.MemberProfilePic = member.ProfilePic == null ? "facebook/" + member.FacebookPic : "thumb/" + member.ProfilePic;
                    article.MemberProfilePic = "thumb/" + member.ProfilePic;
                }
                article.MemberName = member.Name;
                return article;
            }
        }

        public ArticleDetailT GetArticleDetailByArticleNo(int articleNo, int visiteNo)
        {
            string query = @"select 
	                                        A.[NO], A.MEMBER_NO, A.MAIN_IMAGE,  A.CODE_NO,
											(SELECT TOP 1 TD.TITLE FROM TRANSLATION_DETAIL TD WITH(NOLOCK) WHERE TD.ARTICLE_NO = A.NO ORDER BY NO ASC ) AS TITLE, 
											(SELECT TOP 1 TD.CONTENTS FROM TRANSLATION_DETAIL TD WITH(NOLOCK) WHERE TD.ARTICLE_NO = A.NO ORDER BY NO ASC ) AS CONTENTS,
											(SELECT TOP 1 TD.TAG FROM TRANSLATION_DETAIL TD WITH(NOLOCK) WHERE TD.ARTICLE_NO = A.NO ORDER BY NO ASC ) AS TAG,
                                            A.COPYRIGHT, A.VISIBILITY, A.VIEWCNT, A.TEMP, A.REG_IP,
	                                        A.REG_DT, A.REG_ID, A.RECOMMEND_YN, A.RECOMMEND_DT, M.NAME as MEMBER_NAME, M.PROFILE_PIC as MEMBER_PROFILE_PIC, 
											case F.FILE_TYPE when 'img' then F.RENAME else F.IMG_NAME end as MAINIMGNAME, 
	                                        (SELECT COUNT(0) FROM LIKES WHERE ARTICLE_NO = A.[NO]) AS LIKE_CNT,
	                                        (SELECT COUNT(0) FROM ARTICLE_COMMENT WHERE ARTICLE_NO = A.[NO]) AS COMMENT_CNT,
	                                        (SELECT COUNT(0) FROM LIKES WHERE ARTICLE_NO = A.[NO] AND MEMBER_NO = :visiteNo) AS IS_LIKES, '0' as UPLOAD_CNT, '0' as DRAFT_CNT,
                                            A.VIDEO_URL 
                                        from ARTICLE A with(nolock)
                                        inner join MEMBER M with(nolock) on M.[NO] = A.MEMBER_NO
                                        inner join ARTICLE_FILE F with(nolock) on F.[NO] = A.MAIN_IMAGE
                                        where A.[NO] = :articleNo";
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(ArticleDetailT));
                queryObj.SetParameter("articleNo", articleNo);
                queryObj.SetParameter("visiteNo", visiteNo);

                ArticleDetailT detailT = queryObj.UniqueResult<ArticleDetailT>();

                session.Flush();

                return detailT;
            }
        }

        public ArticleT GetArticleForEdit(int no)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ArticleT article = session.QueryOver<ArticleT>().Where(a => a.No == no).SingleOrDefault<ArticleT>();
                ArticleFileT articleFile = session.QueryOver<ArticleFileT>().Where(w => w.No == article.MainImage).SingleOrDefault<ArticleFileT>();
                article.ImageName = articleFile.ImgName == null ? articleFile.Rename : articleFile.ImgName;
                return article;
            }
        }

        #region 게시자의 게시물 4개
        /// <summary>
        /// 게시자의 게시물 4개
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public IList<ArticleT> GetMemberArticleTop4(int no)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ArticleT a = session.QueryOver<ArticleT>().Where(w => w.No == no).SingleOrDefault<ArticleT>();
                string query = @"SELECT TOP 4 A.NO, ISNULL(AF.IMG_NAME, AF.RENAME) AS MAIN_IMAGE, 
											(SELECT TOP 1 TD.TITLE FROM TRANSLATION_DETAIL TD WITH(NOLOCK) WHERE TD.ARTICLE_NO = A.NO ORDER BY NO ASC ) AS TITLE, 
                                            C.NAME AS CATEGORY, M.NAME AS MEMBER_NAME ,A.MEMBER_NO, A.CODE_NO, AF.PATH
                            FROM ARTICLE A WITH(NOLOCK) INNER JOIN ARTICLE_FILE AS AF WITH(NOLOCK)
					                            ON A.MAIN_IMAGE = AF.NO
					                            LEFT JOIN CODE AS C WITH(NOLOCK)
					                            ON A.CODE_NO = C.NO
					                            LEFT JOIN MEMBER AS M WITH(NOLOCK)
					                            ON A.MEMBER_NO = M.NO 
                                                WHERE (M.DEL_FLAG != 'Y' OR M.DEL_FLAG IS NULL)
                                                AND A.VISIBILITY = 'Y'  AND A.MEMBER_NO = :MemberNo AND A.NO != :no  ORDER BY A.REG_DT DESC";

                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("no", no);
                queryObj.SetParameter("MemberNo", a.MemberNo);

                IList<object[]> results = queryObj.List<object[]>();
                //session.Flush();

                IList<ArticleT> list = new List<ArticleT>();
                foreach (object[] row in results)
                {
                    ArticleT article = new ArticleT();
                    article.No = (int)row[0];
                    article.ImageName = (string)row[1];
                    article.Title = (string)row[2];
                    article.Category = (string)row[3];
                    article.MemberName = (string)row[4];
                    article.MemberNo = (int)row[5];
                    article.CodeNo = (int)row[6];
                    article.Path = (string)row[7] + article.ImageName;
                    list.Add(article);
                }
                if (results.Count == 0)
                {
                    ArticleT article = new ArticleT();
                    MemberT member = session.QueryOver<MemberT>().Where(w => w.No == a.MemberNo).SingleOrDefault<MemberT>();
                    article.MemberName = member.Name;
                    article.MemberNo = member.No;
                    list.Add(article);
                }

                return list;

            }
        }
        #endregion

        #region 조회수 증가
        public void UpdateViewCnt(int no)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ArticleT article = session.QueryOver<ArticleT>().Where(w => w.No == no).SingleOrDefault<ArticleT>();
                    article.ViewCnt = article.ViewCnt + 1;
                    session.Update(article);
                    transaction.Commit();
                    session.Flush();
                }
            }
        }
        #endregion

        #region 순위 업데이트
        public void UpdatePriority(int no, int priority)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ArticleT article = session.QueryOver<ArticleT>().Where(w => w.No == no).SingleOrDefault<ArticleT>();
                    article.RecommendPriority = priority;
                    session.Update(article);
                    transaction.Commit();
                    session.Flush();
                }
            }
        }
        #endregion

        #region 추천 사용여부 업데이트
        public void UpdateRecommendVisibility(int no, string visibility)
        {
            // if visibility is null just change article.RecommendVisibility's value to the other, 
            // else change article.RecommendVisibility to visibility 
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ArticleT article = session.QueryOver<ArticleT>().Where(w => w.No == no).SingleOrDefault<ArticleT>();

                    if (visibility == null)
                    {
                        if (article.RecommendVisibility == "Y")
                            article.RecommendVisibility = "N";
                        else
                            article.RecommendVisibility = "Y";
                    }
                    else
                    {
                        article.RecommendVisibility = visibility;
                    }
                    session.Update(article);
                    transaction.Commit();
                    session.Flush();
                }
            }
        }
        #endregion

        #region 리스팅 페이지 동적쿼리
        /// <summary>
        /// 리스팅 페이지 동적쿼리
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="codeNo">카테고리 넘버 기본값 0(전체검색)</param>
        /// <param name="isMain">디자인 메인페이지인지 아닌지를 구분함(Y -TOP 4 N - ALL)</param>
        /// <param name="gubun">추천,인기,신규 구분(R, P, N) 카테고리 페이지는 N이나 공백 사용하면 됨</param>
        /// <param name="fromPage"></param>
        /// <param name="toPage"></param>
        /// <returns></returns>
        public IList<ArticleDetailT> GetListByOption(int memberNo, int codeNo, string gubun, string globleType, int fromPage, int toPage)
        {

            int downloadScore = 40;
            int commentScore = 20;
            int likeScore = 10;
            int viewScore = 5;

            string targetOptQuery = string.Empty;

            string rowNumQuery = string.Empty;

            string whereQuery = string.Empty;

            switch (gubun.ToUpper())
            {
                case "N":
                    if (codeNo != 0 && codeNo < 10000) { whereQuery += " AND A.CODE_NO  like :codeLikeNo "; }
                    else if (codeNo != 0 && codeNo > 10000) { whereQuery += " AND A.CODE_NO = :codeNo "; }

                    rowNumQuery = ", ROW_NUMBER() OVER(ORDER BY REG_DT DESC) AS ROW_NUM ";
                    break;
                case "P":
                    rowNumQuery = ", ROW_NUMBER() OVER(ORDER BY POP DESC) AS ROW_NUM ";
                    break;
                case "R":
                    rowNumQuery = ", ROW_NUMBER() OVER(ORDER BY RECOMMEND_PRIORITY DESC, RECOMMEND_DT DESC) AS ROW_NUM ";
                    break;
                default:
                    if (codeNo != 0 && codeNo < 10000) { whereQuery += " AND A.CODE_NO  like :codeLikeNo "; }
                    else if (codeNo != 0 && codeNo > 10000) { whereQuery += " AND A.CODE_NO = :codeNo "; }

                    rowNumQuery = ", ROW_NUMBER() OVER(ORDER BY REG_DT DESC) AS ROW_NUM ";
                    break;
            }
            string addJoinQuere = string.Empty;
            if (globleType != "") { whereQuery += "AND TD.LANG_FLAG = :globleType "; }
            else { addJoinQuere = " AND TD.NO IN (select MIN(TDL.NO) FROM TRANSLATION_DETAIL TDL WITH(NOLOCK) group by TDL.ARTICLE_NO)"; }

            string addQuery = string.Empty;
            if (gubun == "R")
            {
                addQuery += " AND A.RECOMMEND_VISIBILITY='Y' ";
            }

            targetOptQuery = @"SELECT OutQ.* , '0' as COPYRIGHT, '' as VISIBILITY, '' as OPT, '0' as VIEWCNT, '' as TEMP, '' as REG_IP, '' as REG_ID, OutQ.REG_DT, '' as RECOMMEND_YN, OutQ.REG_DT as RECOMMEND_DT, '0' as UPLOAD_CNT, '0' as DRAFT_CNT, '' as MEMBER_PROFILE_PIC FROM 
                            (SELECT InQ.NO, InQ.MAINIMGNAME, InQ.MAIN_IMAGE, InQ.TITLE, InQ.CONTENTS, InQ.TAG, InQ.MEMBER_NAME, InQ.MEMBER_NO, InQ.CODE_NO, InQ.VIEWCNT, InQ.COMMENT_CNT, InQ.LIKE_CNT, InQ.IS_LIKES, InQ.REG_DT ,InQ.VIDEO_URL" + rowNumQuery + @" FROM 

						    (SELECT  A.NO, ISNULL(AF.IMG_NAME, AF.RENAME) AS MAINIMGNAME, A.MAIN_IMAGE, 
							TD.TITLE,
                            TD.TAG,
                            TD.CONTENTS,
                            M.NAME AS MEMBER_NAME,  A.MEMBER_NO, A.CODE_NO, A.VIEWCNT, (SELECT count(0) FROM ARTICLE_COMMENT B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO) AS COMMENT_CNT,
	                        (SELECT count(0) FROM LIKES B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO) AS LIKE_CNT,
	                        (SELECT count(0) FROM LIKES B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO AND B.MEMBER_NO = :memberNo ) AS IS_LIKES,
	                        (
						        --(A.VIEWCNT * " + viewScore + @") + 
		                        ((SELECT Count(0) FROM DOWNLOAD B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO AND B.REG_DT >= DATEADD(D, -7, GETDATE())) * " + downloadScore + @" ) + 
		                        ((SELECT count(0) FROM ARTICLE_COMMENT B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO AND B.REG_DT >= DATEADD(D, -7, GETDATE())) * " + commentScore + @" ) + 
		                        ((SELECT count(0) FROM LIKES B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO AND B.REG_DT <= DATEADD(D, -7, GETDATE())) * " + likeScore + @" )
	                        ) AS POP, A.RECOMMEND_DT, A.REG_DT, A.VIDEO_URL, A.RECOMMEND_PRIORITY
                        FROM ARTICLE A WITH(NOLOCK) 
                        INNER JOIN TRANSLATION_DETAIL TD WITH(NOLOCK) ON A.NO = TD.ARTICLE_NO"+ addJoinQuere + @"
                        INNER JOIN ARTICLE_FILE AF WITH(NOLOCK) ON AF.NO = A.MAIN_IMAGE 
                        --INNER JOIN CODE C WITH(NOLOCK) ON C.NO = A.CODE_NO  
                        LEFT OUTER JOIN MEMBER M WITH(NOLOCK) ON M.NO = A.MEMBER_NO  
                        WHERE (M.DEL_FLAG != 'Y' OR M.DEL_FLAG IS NULL) " + addQuery + @" AND A.VISIBILITY = 'Y' " + whereQuery + " ) InQ ) OutQ";
            targetOptQuery += @" WHERE ROW_NUM BETWEEN :fromPage AND :toPage";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(targetOptQuery).AddEntity(typeof(ArticleDetailT));
                if (targetOptQuery.Contains(":codeNo"))
                {
                    queryObj.SetParameter("codeNo", codeNo);
                }
                if (targetOptQuery.Contains(":codeLikeNo"))
                {
                    queryObj.SetParameter("codeLikeNo", "%" + (codeNo % 100));
                }
                queryObj.SetParameter("memberNo", memberNo);
                //queryObj.SetParameter("gubun",gubun);
                queryObj.SetParameter("fromPage", fromPage);
                queryObj.SetParameter("toPage", toPage);
                if (targetOptQuery.Contains(":globleType"))
                {
                    queryObj.SetParameter("globleType", globleType);
                }


                IList<ArticleDetailT> results = (IList<ArticleDetailT>)queryObj.List<ArticleDetailT>();
                return results;
            }
        }
        #endregion

        #region get total Cnt target option
        /// <summary>
        /// get total Cnt target option
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="codeNo"></param>
        /// <param name="isMain"></param>
        /// <param name="gubun"></param>
        /// <returns></returns>
        public int GetTotalCountByOption(int memberNo, int codeNo, string isMain, string gubun, string globalType)
        {
            string targetCntQuery = string.Empty;
            string whereQuery = string.Empty;
            string addQuery = string.Empty;

            if (gubun == "R")
            {
                addQuery += " AND A.RECOMMEND_YN='Y' ";
            }
            if (codeNo != 0)
            {
                whereQuery += " AND A.CODE_NO = :codeNo ";
            }

            if (globalType != "")
            {
                whereQuery += string.Format(" AND TD.LANG_FLAG = '{0}'", globalType);
            }
            else
            {
                whereQuery += "  AND TD.NO IN (select MIN(TDL.NO) FROM TRANSLATION_DETAIL TDL WITH(NOLOCK) group by TDL.ARTICLE_NO)";
            }

            if (isMain != "Y")
            {
                targetCntQuery += @"SELECT COUNT(1) 
                                    FROM ARTICLE A WITH(NOLOCK)  
                                    LEFT OUTER JOIN MEMBER M WITH(NOLOCK) ON M.NO = A.MEMBER_NO  
                                    INNER JOIN TRANSLATION_DETAIL TD WITH(NOLOCK) ON A.NO = TD.ARTICLE_NO
                                    WHERE (M.DEL_FLAG != 'Y' OR M.DEL_FLAG IS NULL) " + whereQuery + addQuery + @" AND A.VISIBILITY = 'Y' ";
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(targetCntQuery);
                if (targetCntQuery.Contains(":codeNo"))
                {
                    queryObj.SetParameter("codeNo", codeNo);
                }
                int rowCnt = (int)queryObj.UniqueResult();

                session.Flush();

                return rowCnt;
            }
        }
        #endregion

        #region detail cntList
        public ArticleT GetDetailPageCntList(int articleNo, int visitorNo)
        {
            string query = string.Format(@"SELECT 
		                        (SELECT VIEWCNT FROM ARTICLE WHERE NO = :articleNo) AS VIEWCNT,
		                        (SELECT COUNT(0) FROM LIKES WHERE ARTICLE_NO = :articleNo) AS LIKESCNT,
		                        (SELECT COUNT(0) FROM ARTICLE_COMMENT WHERE ARTICLE_NO = :articleNo) AS COMMENTCNT,
                                (SELECT COUNT(0) FROM LIKES WHERE ARTICLE_NO = :articleNo AND MEMBER_NO = :visitorNo) AS CHKLIKES");
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("articleNo", articleNo);
                queryObj.SetParameter("visitorNo", visitorNo);
                IList<object[]> result = queryObj.List<object[]>();
                ArticleT article = new ArticleT();
                foreach (object[] row in result)
                {
                    article.ViewCnt = (int)row[0];
                    article.LikeCnt = (int)row[1];
                    article.CommentCnt = (int)row[2];
                    article.chkLikes = (int)row[3];
                }
                return article;
            }
        }
        #endregion

        #region insert article
        /// <summary>
        /// insert article and update articlefile
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int SaveOrUpdate(ArticleT data, string delno)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    int articleNo = 0;
                    try
                    {
                        if (data.No > 0)
                        {
                            session.Update(data);
                            articleNo = data.No;
                        }
                        else
                        {
                            articleNo = (Int32)session.Save(data);
                        }

                        if (!string.IsNullOrEmpty(delno))
                        {
                            string[] delNoL = delno.Split(',');
                            //int[] delnoLT = new int[] { };
                            //if (delNoL.Length > 1)
                            //{
                            //    delnoLT = delNoL.Cast<int>().ToArray();
                            //}
                            //else
                            //{
                            //    delnoLT = new int[] { Convert.ToInt32(delno) };
                            //}
                            string delfileQuery = string.Empty;
                            foreach (var delNo in delNoL)
                            {
                                //delfileQuery += @" UPDATE ARTICLE_FILE SET FILE_GUBUN = 'DELETE' WHERE [NO] =" + delNo + " AND TEMP='" + data.Temp + "'";
                                delfileQuery += @" UPDATE ARTICLE_FILE SET FILE_GUBUN = 'DELETE' WHERE [NO] = ? AND TEMP= ? ";
                            }

                            IQuery queryObj = session.CreateSQLQuery(delfileQuery);
                            for (int i = 0; i < delNoL.Length; i++)
                            {
                                queryObj.SetParameter(i*2,delNoL[i]);
                                queryObj.SetParameter((i * 2) + 1, data.Temp);
                            }

                            int cnt = queryObj.ExecuteUpdate();
                        }

                        //string updfileQuery = @"UPDATE ARTICLE_FILE set FILE_GUBUN='article', ARTICLE_NO = '" + articleNo + "' , UPD_DT = GETDATE(), UPD_ID = '" + data.RegId + "' where FILE_GUBUN='temp' and TEMP='" + data.Temp + "' ";      


                        string updfileQuery = @"UPDATE ARTICLE_FILE set FILE_GUBUN='article', ARTICLE_NO = :articleNo , UPD_DT = GETDATE(), UPD_ID = :RegId  where FILE_GUBUN='temp' and TEMP= :Temp";

                        IQuery queryObj2 = session.CreateSQLQuery(updfileQuery);
                        queryObj2.SetParameter("articleNo",articleNo);
                        queryObj2.SetParameter("RegId",data.RegId);
                        queryObj2.SetParameter("Temp",data.Temp);

                        queryObj2.ExecuteUpdate();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                    transaction.Commit();
                    session.Flush();

                    return articleNo;
                }
            }
        }
        #endregion

        /// <summary>
        /// delete article
        /// </summary>
        /// <param name="articleNo"></param>
        public void DeleteArticle(int articleNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    string deleteQuery = "DELETE FROM ARTICLE WHERE NO= :articleNo ";
                    //int delCnt = (int)session.CreateSQLQuery(deleteQuery).SetParameter("NO", articleNo).ExecuteUpdate();
                    //IQuery queryObj  = session.CreateSQLQuery(deleteQuery);
                    //queryObj.SetParameter("articleNo", articleNo);
                    //queryObj.ExecuteUpdate();
                    //if (delCnt > 0)
                    //{
                    string deletFileQuery = "UPDATE ARTICLE_FILE SET FILE_GUBUN='DELETE' WHERE ARTICLE_NO = :articleNo";
                    //session.CreateSQLQuery(deletFileQuery).SetParameter("NO", articleNo).ExecuteUpdate();
                    IQuery queryObj = session.CreateSQLQuery(deleteQuery + deletFileQuery);
                    queryObj.SetParameter("articleNo", articleNo);
                    queryObj.ExecuteUpdate();
                    //}

                    transaction.Commit();
                    session.Flush();
                }
            }
        }

        /// <summary>
        /// 임시파일이 있으면 file_gubun = DELETE
        /// </summary>
        /// <param name="p"></param>
        public void CheckTempFile(string temp)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    string changeFileQuery = string.Format(@"UPDATE ARTICLE_FILE SET FILE_GUBUN='DELETE' WHERE FILE_GUBUN='TEMP' AND TEMP = :temp");

                    IQuery queryObj = session.CreateSQLQuery(changeFileQuery);
                    queryObj.SetParameter("temp", temp);
                    queryObj.ExecuteUpdate();
                    transaction.Commit();
                    session.Flush();
                }
            }
        }

        public int InsertNewList(ListT data)
        {
            int result = 0;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                result = (Int32)session.Save(data);
                session.Flush();
            }
            return result;
        }


        #region 공모전 관련
        public IList<ArticleDetailT> GetCompetitionList(int memberNo, int fromPage, int toPage, string competitionName)
        {
            //IList<ArticleT> list = new List<ArticleT>();

            int downloadScore = 40;
            int commentScore = 20;
            int likeScore = 10;
            int viewScore = 5;

            string targetOptQuery = string.Empty;

            string rowNumQuery = string.Empty;

            string whereQuery = string.Empty;

            //if (codeNo != 0) { whereQuery += " AND A.CODE_NO = " + codeNo; };
            rowNumQuery = ", ROW_NUMBER() OVER(ORDER BY REG_DT DESC) AS ROW_NUM ";

            string addQuery = string.Empty;
            addQuery += string.Format(" AND TD.TITLE LIKE '%{0}%'", competitionName);

            targetOptQuery = @"SELECT OutQ.* , '' as CONTENTS, '' as TAG, '0' as COPYRIGHT, '' as VISIBILITY, '' as OPT, '0' as VIEWCNT, '' as TEMP, '' as REG_IP, '' as REG_ID, OutQ.REG_DT, '' as RECOMMEND_YN, OutQ.REG_DT as RECOMMEND_DT, '0' as UPLOAD_CNT, '0' as DRAFT_CNT, '' as MEMBER_PROFILE_PIC FROM 
                            (SELECT InQ.NO, InQ.MAINIMGNAME, InQ.MAIN_IMAGE, InQ.TITLE, InQ.MEMBER_NAME, InQ.MEMBER_NO, InQ.CODE_NO, InQ.VIEWCNT, InQ.COMMENT_CNT, InQ.LIKE_CNT, InQ.IS_LIKES, InQ.REG_DT ,InQ.VIDEO_URL" + rowNumQuery + @" FROM 
						    (SELECT  A.NO, ISNULL(AF.IMG_NAME, AF.RENAME) AS MAINIMGNAME, A.MAIN_IMAGE, A.TITLE, M.NAME AS MEMBER_NAME,  A.MEMBER_NO, A.CODE_NO, A.VIEWCNT,
	                        (SELECT count(0) FROM ARTICLE_COMMENT B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO) AS COMMENT_CNT,
	                        (SELECT count(0) FROM LIKES B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO) AS LIKE_CNT,
	                        (SELECT count(0) FROM LIKES B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO AND B.MEMBER_NO = " + memberNo + @" ) AS IS_LIKES,
	                        (
						        --(A.VIEWCNT * " + viewScore + @") + 
		                        ((SELECT Count(0) FROM DOWNLOAD B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO AND B.REG_DT >= DATEADD(D, -7, GETDATE())) * " + downloadScore + @" ) + 
		                        ((SELECT count(0) FROM ARTICLE_COMMENT B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO AND B.REG_DT >= DATEADD(D, -7, GETDATE())) * " + commentScore + @" ) + 
		                        ((SELECT count(0) FROM LIKES B WITH(NOLOCK) WHERE A.NO = B.ARTICLE_NO AND B.REG_DT <= DATEADD(D, -7, GETDATE())) * " + likeScore + @" )
	                        ) AS POP, A.RECOMMEND_DT, A.REG_DT, A.VIDEO_URL, A.RECOMMEND_PRIORITY
                        FROM ARTICLE A WITH(NOLOCK) 
                        INNER JOIN ARTICLE_FILE AF WITH(NOLOCK) ON AF.NO = A.MAIN_IMAGE 
                        --INNER JOIN CODE C WITH(NOLOCK) ON C.NO = A.CODE_NO  
                        LEFT OUTER JOIN MEMBER M WITH(NOLOCK) ON M.NO = A.MEMBER_NO  
                        INNER JOIN TRANSLATION_DETAIL TD WITH(NOLOCK) ON TD.ARTICLE_NO = A.NO
                        WHERE (M.DEL_FLAG != 'Y' OR M.DEL_FLAG IS NULL) " + addQuery + @" AND A.VISIBILITY = 'Y' " + whereQuery + " ) InQ ) OutQ";
            targetOptQuery += @" WHERE ROW_NUM BETWEEN " + fromPage + @" AND " + toPage;

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<ArticleDetailT> results = (IList<ArticleDetailT>)session.CreateSQLQuery(targetOptQuery).AddEntity(typeof(ArticleDetailT)).List<ArticleDetailT>();
                return results;
            }
        }

        public int GetCompetitionListCount(string competitionName)
        {
            int result = 0;
            string query = string.Format(@"SELECT COUNT(0) FROM TRANSLATION_DETAIL TD WITH(NOLOCK) WHERE TD.TITLE LIKE '%{0}%'", competitionName);
            using (ISession session = NHibernateHelper.OpenSession())
            {
                result = (int)session.CreateSQLQuery(query).UniqueResult();
            }
            return result;
        }
        #endregion
    }
}
