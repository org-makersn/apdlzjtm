using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using Makersn.Models;
using NHibernate;
using Makersn.Util;
using NHibernate.Criterion;
using System.Net;


namespace Makersn.BizDac
{
    public class MemberDac
    {
        /// <summary>
        /// search target member
        /// </summary>
        /// <param name="startDt"></param>
        /// <param name="endDt"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<MemberT> SelectQuery(string startDt, string endDt, string name = "", string id = "")
        {
            string query = @"SELECT 
		                    NO, ID, BLOG_URL, NAME, EMAIL, URL,  SNS_TYPE, SNS_ID, PROFILE_MSG, PROFILE_PIC, COVER_PIC, REG_DT,
                            (SELECT Count(0) AS DownloadCnt FROM download d WHERE  ( d.MEMBER_NO = m.NO)) AS DownloadCnt,
                            (SELECT Count(0) AS UploadCnt FROM ARTICLE A WHERE  ( A.MEMBER_NO = m.NO) AND A.VISIBILITY = 'Y') AS UploadCntY,
                            (SELECT Count(0) AS UploadCnt FROM ARTICLE A WHERE  ( A.MEMBER_NO = m.NO) AND A.VISIBILITY = 'N') AS UploadCntN,
                            (SELECT Count(0) AS CommentCnt FROM ARTICLE_COMMENT IC WHERE  ( IC.MEMBER_NO = m.NO)) AS CommentCnt,
                            (SELECT Count(0) AS OrderCnt FROM ORDER_REQ O WHERE (O.MEMBER_NO = m.NO AND O.ORDER_STATUS = '" + (int)(MakersnEnumTypes.OrderState.거래완료) + @"')) AS OrderCnt 
	                        FROM MEMBER M with(nolock)
                            WHERE (M.DEL_FLAG  is null OR M.DEL_FLAG = 'N' ) AND M.LEVEL < 50 "
                             + " AND REG_DT >= :startDt ";
            if (endDt != "")
            {
                query += " AND REG_DT <= :endDt ";
            }
            if (name != "")
            {
                query += " AND NAME LIKE :name ";
            }
            if (id != "")
            {
                query += " AND ID LIKE :id ";
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("startDt", startDt);
                if (query.Contains(":endDt"))
                {
                    queryObj.SetParameter("endDt", endDt + " 23:59:59");
                }
                if (query.Contains(":name"))
                {
                    queryObj.SetParameter("name", name + "%");
                }
                if (query.Contains(":id"))
                {
                    queryObj.SetParameter("id", id + "%");
                }

                IList<object[]> results = queryObj.List<object[]>();
                session.Flush();

                IList<MemberT> members = new List<MemberT>();
                foreach (object[] row in results)
                {
                    MemberT member = new MemberT();
                    member.No = (int)row[0];
                    member.Id = (string)row[1];
                    member.BlogUrl = (string)row[2];
                    member.Name = (string)row[3];
                    member.Email = (string)row[4];
                    member.Url = (string)row[5];
                    member.SnsType = (string)row[6];
                    member.SnsId = (string)row[7];
                    member.ProfileMsg = (string)row[8];
                    member.ProfilePic = (string)row[9];
                    member.CoverPic = (string)row[10];
                    member.RegDt = (DateTime)row[11];
                    member.DownloadCnt = (int)row[12];
                    member.UploadCntY = (int)row[13];
                    member.UploadCntN = (int)row[14];
                    member.CommentCnt = (int)row[15];
                    member.OrderCnt = (int)row[16];
                    members.Add(member);
                }

                return members;
            }
        }

        /// <summary>
        /// search target drop member 
        /// </summary>
        /// <param name="startDt"></param>
        /// <param name="endDt"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<MemberT> DropMemberSelectQuery(string startDt, string endDt, string name = "", string id = "")
        {
            string query = @"SELECT 
		                    NO, ID, NAME, DROP_COMMENT, DEL_DT
	                        FROM MEMBER m with(nolock)
                            WHERE DEL_FLAG = 'Y' AND LEVEL < 50 "
                            + " AND DEL_DT >= :startDt";
            if (endDt != "")
            {
                query += " AND DEL_DT <= :endDt ";
            }
            if (name != "")
            {
                query += " AND NAME LIKE :name ";
            }
            if (id != "")
            {
                query += " AND ID LIKE :id ";
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("startDt", startDt);
                if (query.Contains(":endDt"))
                {
                    queryObj.SetParameter("endDt", endDt + " 23:59:59");
                }
                if (query.Contains(":name"))
                {
                    queryObj.SetParameter("name", name + "%");
                }
                if (query.Contains(":id"))
                {
                    queryObj.SetParameter("id", id + "%");
                }

                IList<object[]> results = queryObj.List<object[]>();
                session.Flush();

                IList<MemberT> members = new List<MemberT>();
                foreach (object[] row in results)
                {
                    MemberT member = new MemberT();
                    member.No = (int)row[0];
                    member.Id = (string)row[1];
                    member.Name = (string)row[2];
                    member.DropComment = (string)row[3];
                    member.DelDt = (DateTime)row[4];
                    members.Add(member);
                }

                return members;
            }
        }

        /// <summary>
        /// search target drop member 
        /// </summary>
        /// <param name="startDt"></param>
        /// <param name="endDt"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<PrinterMemberT> PrinterMemberSelectQuery(string startDt, string endDt, string name = "", string id = "")
        {
            string query = @"SELECT 
		                    m.NO, m.ID, m.BLOG_URL, m.NAME, m.EMAIL, m.URL,  m.SNS_TYPE, m.SNS_ID, m.PROFILE_MSG, m.PROFILE_PIC, m.COVER_PIC, m.REG_DT , p.REG_DT ,
                            (SELECT Count(0) FROM ORDER_REQ O WHERE (O.MEMBER_NO = m.NO)) AS RequstCnt,
                            (SELECT Count(0) FROM ORDER_REQ O WHERE (O.MEMBER_NO = m.NO AND O.ORDER_STATUS <> '" + (int)(MakersnEnumTypes.OrderState.주문요청) + @"')) AS AcceptCnt,
                            (SELECT Count(0) FROM ORDER_REQ O WHERE (O.MEMBER_NO = m.NO AND O.ORDER_STATUS = '" + (int)(MakersnEnumTypes.OrderState.요청거부) + @"')) AS RejectCnt ,
                            ISNULL((SELECT Sum(UNIT_PRICE) FROM ORDER_REQ O, ORDER_DETAIL OD WHERE (O.MEMBER_NO = m.NO AND O.NO = OD.ORDER_NO )),0) AS Sales,
                            (SELECT Count(0) FROM PRINTER p WHERE p.MEMBER_NO = m.NO ) AS PrinterCnt ,
                            p.NAME
	                        FROM MEMBER m 
                            JOIN PRINTER_MEMBER p
                            ON m.NO = p.MEMBER_NO 
                            WHERE (M.DEL_FLAG  is null OR M.DEL_FLAG = 'N' ) AND M.LEVEL < 50 "
                         + " AND m.REG_DT >=  :startDt ";
            if (endDt != "")
            {
                query += " AND m.REG_DT <= :endDt ";
            }
            if (name != "")
            {
                query += " AND m.NAME LIKE :name ";
            }
            if (id != "")
            {
                query += " AND m.ID LIKE :id ";
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("startDt", startDt);
                if (query.Contains(":endDt"))
                {
                    queryObj.SetParameter("endDt", endDt + " 23:59:59");
                }
                if (query.Contains(":name"))
                {
                    queryObj.SetParameter("name", name + "%");
                }
                if (query.Contains(":id"))
                {
                    queryObj.SetParameter("id", id + "%");
                }

                IList<object[]> results = queryObj.List<object[]>();

                session.Flush();

                IList<PrinterMemberT> members = new List<PrinterMemberT>();
                foreach (object[] row in results)
                {
                    PrinterMemberT member = new PrinterMemberT();
                    member.No = (int)row[0];
                    member.Id = (string)row[1];
                    member.BlogUrl = (string)row[2];
                    member.Name = (string)row[3];
                    member.Email = (string)row[4];
                    member.Url = (string)row[5];
                    member.SnsType = (string)row[6];
                    member.SnsId = (string)row[7];
                    member.ProfileMsg = (string)row[8];
                    member.ProfilePic = (string)row[9];
                    member.CoverPic = (string)row[10];
                    member.RegDt = (DateTime)row[11];
                    member.OpenDate = (DateTime)row[12];
                    member.RequestCnt = (int)row[13];
                    member.AcceptCnt = (int)row[14];
                    member.RejectCnt = (int)row[15];
                    member.Sales = (int)row[16];
                    member.PrintertCnt = (int)row[17];
                    member.SpotName = (String)row[18];
                    members.Add(member);
                }

                return members;

            }
        }

        /// <summary>
        /// get member state - all
        /// </summary>
        /// <returns></returns>
        public IList<MemberStateT> SearchMemberStateTargetAll()
        {
            string[] arrGbn = { "오늘", "어제", "이번주", "지난주", "이번달", "지난달", "기간선택" };

            string query = @"select
	                            Gbn, 
	                            ISNULL(pvt.em, 0) as EmailCnt, 
	                            ISNULL(pvt.fb, 0) as FacebookCnt, 
	                            ISNULL(pvt.[drop], 0) as DropmemberCnt, 
	                            ISNULL(pvt.article, 0) as ArticleCnt,
	                            ISNULL(pvt.download, 0) as DownloadCnt
                            from
                            (
	                            select 
		                            '전체' as Gbn, SNS_TYPE as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50
	                            group by SNS_TYPE
	                            union all
	                            select 
		                            '전체' as Gbn, ('drop') as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and DEL_FLAG = 'Y'
	                            union all
	                            select 
		                            '전체' as Gbn,'article' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            union all
	                            select 
		                            '전체' as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([em],[fb],[drop],[article],[download])
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
                    default:
                        add_where = " 1 = 1 ";
                        break;
                }

                if (gbn == "기간선택") continue;

                query += string.Format(@" union all select
	                            Gbn, 
	                            ISNULL(pvt.em, 0) as EmailCnt, 
	                            ISNULL(pvt.fb, 0) as FacebookCnt, 
	                            ISNULL(pvt.[drop], 0) as DropmemberCnt, 
	                            ISNULL(pvt.article, 0) as ArticleCnt,
	                            ISNULL(pvt.download, 0) as DownloadCnt
                            from
                            (
	                            select 
		                            '{0}' as Gbn, SNS_TYPE as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and {1}
	                            group by SNS_TYPE
	                            union all
	                            select 
		                            '{0}' as Gbn, ('drop') as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and DEL_FLAG = 'Y' and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'article' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
								where {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock)
								where {1}
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([em],[fb],[drop],[article],[download])
                            ) as pvt ", gbn, add_where);
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<MemberStateT> result = (IList<MemberStateT>)session.CreateSQLQuery(query).AddEntity(typeof(MemberStateT)).List<MemberStateT>();

                return result;
            }
        }

        /// <summary>
        /// get member state - daily
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IList<MemberStateT> SearchMemberStateTargetDaily(string start, string end)
        {
            string query = @"select
	                            Gbn, 
	                            ISNULL(pvt.em, 0) as EmailCnt, 
	                            ISNULL(pvt.fb, 0) as FacebookCnt, 
	                            ISNULL(pvt.[drop], 0) as DropMemberCnt, 
	                            ISNULL(pvt.article, 0) as ArticleCnt,
	                            ISNULL(pvt.download, 0) as DownloadCnt
                            from
                            (
	                            select 
		                            convert(date, REG_DT) as Gbn, SNS_TYPE as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT),  SNS_TYPE
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn, ('drop') as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and DEL_FLAG = 'Y' and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT),  DEL_FLAG
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'article' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([em],[fb],[drop],[article],[download])
                            ) as pvt";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(MemberStateT));
                queryObj.SetParameter("start", start);
                queryObj.SetParameter("end", end);
                IList<MemberStateT> result = (IList<MemberStateT>)queryObj.List<MemberStateT>();

                return result;
            }
        }

        /// <summary>
        /// get member state - month
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IList<MemberStateT> SearchMemberStateTargetMonth(string start, string end)
        {
            string query = string.Format(@"select
	                            Gbn, 
	                            ISNULL(pvt.em, 0) as EmailCnt, 
	                            ISNULL(pvt.fb, 0) as FacebookCnt, 
	                            ISNULL(pvt.[drop], 0) as DropmemberCnt, 
	                            ISNULL(pvt.article, 0) as ArticleCnt,
	                            ISNULL(pvt.download, 0) as DownloadCnt
                            from
                            (
	                            select 
		                            month(REG_DT) as Gbn, SNS_TYPE as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT),  SNS_TYPE
	                            union all
	                            select 
		                            month(REG_DT) as Gbn, ('drop') as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and DEL_FLAG = 'Y' and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT),  DEL_FLAG
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'article' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([em],[fb],[drop],[article],[download])
                            ) as pvt");

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(MemberStateT));
                queryObj.SetParameter("start", start);
                queryObj.SetParameter("end", end);
                IList<MemberStateT> result = (IList<MemberStateT>)queryObj.List<MemberStateT>();
                return result;
            }
        }

        public IList<object> GetMemberYearGroup()
        {
            string query = @"select year(reg_dt) as reg_dt from member group by year(reg_dt) order by reg_dt desc";
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<object> results = session.CreateSQLQuery(query).List<object>();
                return results;
            }
        }

        /// <summary>
        /// get member state - year
        /// </summary>
        /// <returns></returns>
        public IList<MemberStateT> SearchMemberStateTargetYear()
        {
            string query = @"select
	                            Gbn, 
	                            ISNULL(pvt.em, 0) as EmailCnt, 
	                            ISNULL(pvt.fb, 0) as FacebookCnt, 
	                            ISNULL(pvt.[drop], 0) as DropmemberCnt, 
	                            ISNULL(pvt.article, 0) as ArticleCnt,
	                            ISNULL(pvt.download, 0) as DownloadCnt
                            from
                            (
	                            select 
		                            year(REG_DT) as Gbn, SNS_TYPE as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50
	                            group by year(REG_DT),  SNS_TYPE
	                            union all
	                            select 
		                            year(REG_DT) as Gbn, ('drop') as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and DEL_FLAG = 'Y'
	                            group by year(REG_DT),  DEL_FLAG
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'article' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock)
	                            group by year(REG_DT)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([em],[fb],[drop],[article],[download])
                            ) as pvt";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<MemberStateT> result = (IList<MemberStateT>)session.CreateSQLQuery(query).AddEntity(typeof(MemberStateT)).List<MemberStateT>();

                return result;
            }
        }

        public IList<MemberStateT> SearchMemberStateTargetSearch(string start, string end)
        {
            string[] arrGbn = { "오늘", "어제", "이번주", "지난주", "이번달", "지난달", "기간선택" };

            string query = @"select
	                            Gbn, 
	                            ISNULL(pvt.em, 0) as EmailCnt, 
	                            ISNULL(pvt.fb, 0) as FacebookCnt, 
	                            ISNULL(pvt.[drop], 0) as DropmemberCnt, 
	                            ISNULL(pvt.article, 0) as ArticleCnt,
	                            ISNULL(pvt.download, 0) as DownloadCnt
                            from
                            (
	                            select 
		                            '전체' as Gbn, SNS_TYPE as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50
	                            group by SNS_TYPE
	                            union all
	                            select 
		                            '전체' as Gbn, ('drop') as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and DEL_FLAG = 'Y'
	                            union all
	                            select 
		                            '전체' as Gbn,'article' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            union all
	                            select 
		                            '전체' as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([em],[fb],[drop],[article],[download])
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
                        add_where = string.Format(" REG_DT >= {0} AND REG_DT <= {1} ", "'" + start + "'", "'" + end + " 23:59:59" + "'");
                        break;
                    default:
                        add_where = " 1 = 1 ";
                        break;
                }

                //if (gbn == "기간선택") continue;

                query += string.Format(@" union all select
	                            Gbn, 
	                            ISNULL(pvt.em, 0) as EmailCnt, 
	                            ISNULL(pvt.fb, 0) as FacebookCnt, 
	                            ISNULL(pvt.[drop], 0) as DropmemberCnt, 
	                            ISNULL(pvt.article, 0) as ArticleCnt,
	                            ISNULL(pvt.download, 0) as DownloadCnt
                            from
                            (
	                            select 
		                            '{0}' as Gbn, SNS_TYPE as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and {1}
	                            group by SNS_TYPE
	                            union all
	                            select 
		                            '{0}' as Gbn, ('drop') as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and DEL_FLAG = 'Y' and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'article' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
								where {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock)
								where {1}
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([em],[fb],[drop],[article],[download])
                            ) as pvt ", gbn, add_where);
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<MemberStateT> result = (IList<MemberStateT>)session.CreateSQLQuery(query).AddEntity(typeof(MemberStateT)).List<MemberStateT>();

                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public int UpdateMember(MemberT member)
        {
            int result = 0;

            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    MemberT mem = session.QueryOver<MemberT>().Where(w => w.No == member.No).SingleOrDefault<MemberT>();

                    try
                    {
                        if (mem != null)
                        {
                            //이메일 변경 보류
                            //if (member.Email != mem.Email)
                            //{
                            //    mem.emailCertify = "N";
                            //    result = 7;
                            //}
                            CryptFilter cryptFilter = new CryptFilter();
                            mem.Name = member.Name;
                            mem.BlogUrl = member.BlogUrl;
                            mem.Email = member.Email;
                            mem.Url = member.Url;
                            if (member.Password != "******") { mem.Password = cryptFilter.Encrypt(member.Password); };
                            mem.ProfileMsg = member.ProfileMsg;
                            mem.AllIs = member.AllIs;
                            mem.RepleIs = member.RepleIs;
                            mem.LikeIs = member.LikeIs;
                            mem.FollowIs = member.FollowIs;
                            mem.UpdId = member.UpdId;
                            mem.UpdDt = member.UpdDt;
                            mem.UpdPasswordDt = member.UpdPasswordDt;

                            session.Update(mem);

                            transaction.Commit();
                            session.Flush();
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public int UpdateProfilePic(MemberT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                MemberT member = session.QueryOver<MemberT>().Where(w => w.No == data.No).SingleOrDefault<MemberT>();
                int result = 0;
                if (member != null)
                {
                    member.ProfilePic = data.ProfilePic;
                    session.Update(member);
                    result = member.No;
                }

                session.Flush();
                return result;
            }
        }



        #region 프로필 사진 삭제
        public bool DeleteProfilePic(int memberNo)
        {
            bool result = false;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                MemberT member = session.QueryOver<MemberT>().Where(w => w.No == memberNo).Take(1).SingleOrDefault<MemberT>();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    member.ProfilePic = "";
                    session.Update(member);
                    transaction.Commit();
                    session.Flush();
                    result = true;
                }
            }
            return result;
        }
        #endregion

        public int CheckBlogUrl(string blog, int memberNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<MemberT>().Where(w => w.BlogUrl == blog && w.No != memberNo).RowCount();
            }
        }

        public void DeleteMember(int memberNo, string dropComment)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    MemberT member = session.QueryOver<MemberT>().Where(w => w.No == memberNo).SingleOrDefault<MemberT>();
                    if (member != null)
                    {
                        member.ProfileMsg = "탈퇴한 회원입니다.";
                        member.DropComment = dropComment;
                        member.DelFlag = "Y";
                        member.DelDt = DateTime.Now;
                        session.Update(member);
                        transaction.Commit();
                        session.Flush();
                    }
                }
            }
        }

        #region 회원 정보 가져오기
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public MemberT GetMemberProfile(int memberNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<MemberT>().Where(w => w.No == memberNo).SingleOrDefault<MemberT>();
            }
        }
        #endregion

        #region 프로필 페이지 카운트 가져오기
        public ProfileT GetCntList(int memberNo)
        {
            string query = string.Format(@"SELECT
	                            (SELECT COUNT(0) FROM	ARTICLE		WITH(NOLOCK) WHERE	VISIBILITY='Y'  AND	MEMBER_NO	= :memberNo	   ) AS DesignCnt,
	                            (SELECT COUNT(0) FROM	ARTICLE		WITH(NOLOCK) WHERE	VISIBILITY='N'  AND	MEMBER_NO	= :memberNo	   ) AS DraftCnt,
	                            (
								SELECT COUNT(0) FROM LIKES AS L WITH(NOLOCK) INNER JOIN ARTICLE AS A WITH(NOLOCK) ON L.ARTICLE_NO = A.NO AND A.VISIBILITY = 'Y' 
								 WHERE						L.MEMBER_NO	= :memberNo	   
								
								) AS LikesCnt,
	                            (SELECT COUNT(0) FROM	FOLLOWER F	WITH(NOLOCK) INNER JOIN
	                                MEMBER M2 WITH(NOLOCK) ON F.MEMBER_NO_REF = M2.NO AND M2.DEL_FLAG='N'             
                                                                    WHERE						MEMBER_NO	= :memberNo	   ) AS FollowingCnt,
	                            (SELECT COUNT(0) FROM	FOLLOWER F	WITH(NOLOCK) INNER JOIN
	                                MEMBER M2 WITH(NOLOCK) ON F.MEMBER_NO = M2.NO AND M2.DEL_FLAG='N'             
                                                                    WHERE						MEMBER_NO_REF= :memberNo   ) AS FollowerCnt, 
                                (SELECT COUNT(0) FROM	NOTICE	    WITH(NOLOCK) WHERE	IS_NEW='Y'      AND	MEMBER_NO_REF= :memberNo   ) AS NoticeCnt,
                                (SELECT COUNT(0) FROM	MEMBER_MSG	WITH(NOLOCK) WHERE	IS_NEW='Y'		AND	MEMBER_NO_REF= :memberNo   ) AS MessageCnt,
                                (SELECT COUNT(0) FROM   LIST WITH(NOLOCK) WHERE MEMBER_NO = :memberNo ) AS ListCnt");
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("memberNo", memberNo);

                IList<object[]> result = queryObj.List<object[]>();
                ProfileT profile = new ProfileT();
                foreach (object[] row in result)
                {
                    profile.DesignCnt = (int)row[0];
                    profile.DraftCnt = (int)row[1];
                    profile.LikesCnt = (int)row[2];
                    profile.FollowingCnt = (int)row[3];
                    profile.FollowerCnt = (int)row[4];
                    profile.NoticeCnt = (int)row[5];
                    profile.MessageCnt = (int)row[6];
                    profile.ListCnt = (int)row[7];
                }
                return profile;
            }
        }
        #endregion

        #region 회원가입
        public bool AddMember(MemberT member)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                int chk = session.QueryOver<MemberT>().Where(w => w.Id == member.Id && w.DelFlag == "N").RowCount();
                if (chk == 0)
                {
                    using (ITransaction transcation = session.BeginTransaction())
                    {
                        session.Save(member);
                        transcation.Commit();
                        session.Flush();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region 임시 비밀번호 발급
        public bool UpdateTemporaryPassword(string id, string password)
        {
            bool result = false;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                //MemberT member = session.QueryOver<MemberT>().Where(w => w.Id == id && w.SnsType != "fb").Take(1).SingleOrDefault<MemberT>();
                MemberT member = session.QueryOver<MemberT>().Where(w => w.Id == id).Take(1).SingleOrDefault<MemberT>();
                if (member != null)
                {
                    CryptFilter crypt = new CryptFilter();
                    member.Password = crypt.Encrypt(password);
                    session.Update(member);
                    session.Flush();
                    result = true;
                }
                return result;
            }
        }
        #endregion

        #region 이메일인증
        public bool UpdateMemberEmailCertify(int memberNo)
        {
            bool result = false;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                MemberT member = session.QueryOver<MemberT>().Where(w => w.No == memberNo).Take(1).SingleOrDefault<MemberT>();
                if (member != null)
                {
                    member.emailCertify = "Y";
                    session.Update(member);
                    session.Flush();
                    result = true;
                }
            }
            return result;
        }
        #endregion

        #region 이메일 변경
        public bool UpdateEmailandId(int memberNo)
        {
            bool result = false;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                MemberT mem = session.QueryOver<MemberT>().Where(w => w.No == memberNo).Take(1).SingleOrDefault<MemberT>();
                if (mem != null)
                {
                    mem.emailCertify = "Y";
                    mem.Id = mem.Email;
                    mem.UpdDt = DateTime.Now;
                    mem.UpdId = mem.Email;
                    session.Update(mem);
                    session.Flush();
                    result = true;
                }
            }
            return result;
        }
        #endregion

        #region 이메일 변경 취소
        public bool UpdateEmailCancel(int memberNo)
        {
            bool result = false;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                MemberT mem = session.QueryOver<MemberT>().Where(w => w.No == memberNo).Take(1).SingleOrDefault<MemberT>();
                if (mem != null)
                {
                    mem.emailCertify = "Y";
                    mem.Email = mem.Id;
                    mem.UpdDt = DateTime.Now;
                    mem.UpdId = mem.Email;
                    session.Update(mem);
                    session.Flush();
                    result = true;
                }
            }
            return result;
        }
        #endregion

        #region 블로그 주소 가져오기
        public int GetMemberNoByBlogUrl(string url)
        {
            int memberNo = 0;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                MemberT member = session.QueryOver<MemberT>().Where(w => w.BlogUrl == url).SingleOrDefault<MemberT>();
                if (member != null) { memberNo = member.No; };
                return memberNo;
            }
        }

        public MemberT GetMemberNoByBlogUrl2(string url)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                MemberT member = session.QueryOver<MemberT>().Where(w => w.BlogUrl == url).SingleOrDefault<MemberT>();
                return member;
            }
        }
        #endregion

        /// <summary>
        /// admin 로그인
        /// </summary>
        /// <param name="membId"></param>
        /// <param name="membPw"></param>
        /// <returns></returns>
        public MemberT GetMemberForAdminLogOn(string membId, string membPw, string ip)
        {
            CryptFilter crypt = new CryptFilter();

            using (ISession session = NHibernateHelper.OpenSession())
            {
                MemberT member = session.QueryOver<MemberT>().Where(m => (m.Id == membId && m.Password == crypt.Encrypt(membPw)) && m.Level >= 50 && m.DelFlag == "N").SingleOrDefault<MemberT>();
                if (member != null)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        member.LoginCnt = member.LoginCnt + 1;
                        member.LastLoginIp = ip;
                        member.LastLoginDt = DateTime.Now;
                        session.Update(member);
                        transaction.Commit();
                        session.Flush();
                    }
                }
                return member;
            }
        }

        public MemberT GetMemberForMemberLogOn(string membId, string membPw, string ip)
        {
            CryptFilter crypt = new CryptFilter();

            using (ISession session = NHibernateHelper.OpenSession())
            {
                MemberT member = session.QueryOver<MemberT>().Where(m => (m.Id == membId && m.Password == crypt.Encrypt(membPw) && m.DelFlag == "N")).Take(1).SingleOrDefault<MemberT>();
                if (member != null)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        member.LoginCnt = member.LoginCnt + 1;
                        member.LastLoginIp = ip;
                        member.LastLoginDt = DateTime.Now;
                        session.Update(member);
                        transaction.Commit();
                        session.Flush();
                    }
                }
                return member;
                //return session.QueryOver<MemberT>().Where(m => (m.Id == membId)).SingleOrDefault<MemberT>();
            }
        }

        /// <summary>
        /// 있는지 없는지 확인
        /// </summary>
        /// <param name="membId"></param>
        /// <returns></returns>
        public MemberT IsMemberExistById(string membId, string openId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<MemberT>().Where(m => (m.Id == membId && m.DelFlag != "Y")).OrderBy(o => o.No).Asc.Take(1).SingleOrDefault<MemberT>();
                //return session.QueryOver<MemberT>().Where(m => (m.Id == membId && m.SnsId == openId)).SingleOrDefault<MemberT>();
            }
        }

        public IList<DashBoardStateT> SearchDashBoardStateTargetAll(string start = "", string end = "")
        {
            string[] arrGbn = { "오늘", "어제", "이번주", "지난주", "이번달", "지난달", "기간선택" };

            string query = @"select
	                            Gbn, 
	                            ISNULL(pvt.em, 0) as EmailCnt, 
	                            ISNULL(pvt.fb, 0) as FacebookCnt, 
	                            ISNULL(pvt.[drop], 0) as DropmemberCnt, 
	                            ISNULL(pvt.article, 0) as ArticleCnt,
	                            ISNULL(pvt.download, 0) as DownloadCnt,
								ISNULL(pvt.spot, 0) as SpotCnt,
								ISNULL(pvt.printer, 0) as PrinterCnt,
								ISNULL(pvt.[order], 0) as OrderCnt,
								ISNULL(pvt.totalprice, 0) as TotalPrice,
                                ISNULL(pvt.orderMemCnt, 0) as OrderMemCnt
                            from
                            (
	                            select 
		                            '전체' as Gbn, SNS_TYPE as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50
	                            group by SNS_TYPE
	                            union all
	                            select 
		                            '전체' as Gbn, ('drop') as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and DEL_FLAG = 'Y'
	                            union all
	                            select 
		                            '전체' as Gbn,'article' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            union all
	                            select 
		                            '전체' as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock)
	                            union all
								select 
									'전체' as Gbn, 'spot' as fortype, COUNT(1) as Total
								FROM PRINTER_MEMBER with(nolock)
								union all
								select 
									'전체' as Gbn, 'printer' as fortype, COUNT(1) as Total
								FROM PRINTER with(nolock)
								union all
								select 
									'전체' as Gbn, ('order') as fortype, COUNT(1) as Total
								FROM ORDER_REQ with(nolock)
								union all
								select 
									'전체' as Gbn, 'totalprice' as fortype, SUM(UNIT_PRICE) as Total
								FROM ORDER_DETAIL with(nolock)
                                union all
								select 
									'전체' as Gbn, 'orderMemCnt' as fortype, COUNT(DISTINCT MEMBER_NO) as Total
								FROM ORDER_REQ with(nolock)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([em],[fb],[drop],[article],[download],[spot],[printer],[order],[totalprice],[orderMemCnt])
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
	                            ISNULL(pvt.em, 0) as EmailCnt, 
	                            ISNULL(pvt.fb, 0) as FacebookCnt, 
	                            ISNULL(pvt.[drop], 0) as DropmemberCnt, 
	                            ISNULL(pvt.article, 0) as ArticleCnt,
	                            ISNULL(pvt.download, 0) as DownloadCnt,
								ISNULL(pvt.spot, 0) as SpotCnt,
								ISNULL(pvt.printer, 0) as PrinterCnt,
								ISNULL(pvt.[order], 0) as OrderCnt,
								ISNULL(pvt.totalprice, 0) as TotalPrice,
                                ISNULL(pvt.orderMemCnt, 0) as OrderMemCnt
                            from
                            (
	                            select 
		                            '{0}' as Gbn, SNS_TYPE as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and {1}
	                            group by SNS_TYPE
	                            union all
	                            select 
		                            '{0}' as Gbn, ('drop') as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and DEL_FLAG = 'Y' and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'article' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
								where {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock)
								where {1}
                                union all
								select 
									'{0}' as Gbn, 'spot' as fortype, COUNT(1) as Total
								FROM PRINTER_MEMBER with(nolock)
                                where {1}
								union all
								select 
									'{0}' as Gbn, 'printer' as fortype, COUNT(1) as Total
								FROM PRINTER with(nolock)
                                where {1}
								union all
								select 
									'{0}' as Gbn, ('order') as fortype, COUNT(1) as Total
								FROM ORDER_REQ with(nolock)
                                where {1}
								union all
								select 
									'{0}' as Gbn, 'totalprice' as fortype, SUM(UNIT_PRICE) as Total
								FROM ORDER_DETAIL with(nolock)
                                where {1}
                                union all
								select 
									'{0}' as Gbn, 'orderMemCnt' as fortype, COUNT(DISTINCT MEMBER_NO) as Total
								FROM ORDER_REQ with(nolock)
                                where {1}
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([em],[fb],[drop],[article],[download],[spot],[printer],[order],[totalprice],[orderMemCnt])
                            ) as pvt ", gbn, add_where);
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(DashBoardStateT));
                queryObj.SetParameter("start", start);
                queryObj.SetParameter("end", end + " 23:59:59");

                IList<DashBoardStateT> result = (IList<DashBoardStateT>)queryObj.List<DashBoardStateT>();
                return result;
            }
        }

        public IList<DashBoardStateT> GetMemberStateTargetDaily(string start, string end)
        {
            string query = string.Format(@"select
	                            Gbn, 
	                            ISNULL(pvt.em, 0) as EmailCnt, 
	                            ISNULL(pvt.fb, 0) as FacebookCnt, 
	                            ISNULL(pvt.[drop], 0) as DropMemberCnt, 
	                            ISNULL(pvt.article, 0) as ArticleCnt,
	                            ISNULL(pvt.download, 0) as DownloadCnt,
								ISNULL(pvt.spot, 0) as SpotCnt,
								ISNULL(pvt.printer, 0) as PrinterCnt,
								ISNULL(pvt.[order], 0) as OrderCnt,
								ISNULL(pvt.totalprice, 0) as TotalPrice,
                                ISNULL(pvt.orderMemCnt, 0) as OrderMemCnt
                            from
                            (
	                            select 
		                            convert(date, REG_DT) as Gbn, SNS_TYPE as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT),  SNS_TYPE
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn, ('drop') as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and DEL_FLAG = 'Y' and REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT),  DEL_FLAG
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'article' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
	                            union all
	                            select 
		                            convert(date, REG_DT) as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
                                union all
								select 
									 convert(date, REG_DT) as Gbn, 'spot' as fortype, COUNT(1) as Total
								FROM PRINTER_MEMBER with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
								union all
								select 
									 convert(date, REG_DT) as Gbn, 'printer' as fortype, COUNT(1) as Total
								FROM PRINTER with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
								union all
								select 
									 convert(date, REG_DT) as Gbn, ('order') as fortype, COUNT(1) as Total
								FROM ORDER_REQ with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
								union all
								select 
									 convert(date, REG_DT) as Gbn, 'totalprice' as fortype, SUM(UNIT_PRICE) as Total
								FROM ORDER_DETAIL with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
                                union all
								select 
									 convert(date, REG_DT) as Gbn, 'orderMemCnt' as fortype, COUNT(DISTINCT MEMBER_NO) as Total
								FROM ORDER_REQ with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by convert(date, REG_DT)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([em],[fb],[drop],[article],[download],[spot],[printer],[order],[totalprice],[orderMemCnt])
                            ) as pvt");

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(DashBoardStateT));
                queryObj.SetParameter("start", start);
                queryObj.SetParameter("end", end);

                IList<DashBoardStateT> result = (IList<DashBoardStateT>)queryObj.List<DashBoardStateT>();
                return result;
            }
        }

        public IList<DashBoardStateT> GetMemberStateTargetMonth(string start, string end)
        {
            {
                string query = string.Format(@"select
	                            Gbn, 
	                            ISNULL(pvt.em, 0) as EmailCnt, 
	                            ISNULL(pvt.fb, 0) as FacebookCnt, 
	                            ISNULL(pvt.[drop], 0) as DropmemberCnt, 
	                            ISNULL(pvt.article, 0) as ArticleCnt,
	                            ISNULL(pvt.download, 0) as DownloadCnt,
								ISNULL(pvt.spot, 0) as SpotCnt,
								ISNULL(pvt.printer, 0) as PrinterCnt,
								ISNULL(pvt.[order], 0) as OrderCnt,
								ISNULL(pvt.totalprice, 0) as TotalPrice,
                                ISNULL(pvt.orderMemCnt, 0) as OrderMemCnt
                            from
                            (
	                            select 
		                            month(REG_DT) as Gbn, SNS_TYPE as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT),  SNS_TYPE
	                            union all
	                            select 
		                            month(REG_DT) as Gbn, ('drop') as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and DEL_FLAG = 'Y' and REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT),  DEL_FLAG
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'article' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
	                            union all
	                            select 
		                            month(REG_DT) as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
                                union all
								select 
									 month(REG_DT) as Gbn, 'spot' as fortype, COUNT(1) as Total
								FROM PRINTER_MEMBER with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
								union all
								select 
									 month(REG_DT) as Gbn, 'printer' as fortype, COUNT(1) as Total
								FROM PRINTER with(nolock)
	                               where REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
								union all
								select 
									 month(REG_DT) as Gbn, ('order') as fortype, COUNT(1) as Total
								FROM ORDER_REQ with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
								union all
								select 
									 month(REG_DT) as Gbn, 'totalprice' as fortype, SUM(UNIT_PRICE) as Total
								FROM ORDER_DETAIL with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
                                union all
								select 
									 month(REG_DT) as Gbn, 'orderMemCnt' as fortype, COUNT(DISTINCT MEMBER_NO) as Total
								FROM ORDER_REQ with(nolock)
	                            where REG_DT >= :start and REG_DT < :end
	                            group by month(REG_DT)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                    for fortype IN ([em],[fb],[drop],[article],[download],[spot],[printer],[order],[totalprice],[orderMemCnt])
                            ) as pvt");

                using (ISession session = NHibernateHelper.OpenSession())
                {
                    IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(DashBoardStateT));
                    queryObj.SetParameter("start", start);
                    queryObj.SetParameter("end", end);

                    IList<DashBoardStateT> result = (IList<DashBoardStateT>)queryObj.List<DashBoardStateT>();
                    return result;
                }
            }
        }

        public IList<DashBoardStateT> GetMemberStateTargetYear()
        {
            string query = @"select
	                            Gbn, 
	                            ISNULL(pvt.em, 0) as EmailCnt, 
	                            ISNULL(pvt.fb, 0) as FacebookCnt, 
	                            ISNULL(pvt.[drop], 0) as DropmemberCnt, 
	                            ISNULL(pvt.article, 0) as ArticleCnt,
	                            ISNULL(pvt.download, 0) as DownloadCnt,
								ISNULL(pvt.spot, 0) as SpotCnt,
								ISNULL(pvt.printer, 0) as PrinterCnt,
								ISNULL(pvt.[order], 0) as OrderCnt,
								ISNULL(pvt.totalprice, 0) as TotalPrice,
                                ISNULL(pvt.orderMemCnt, 0) as OrderMemCnt
                            from
                            (
	                            select 
		                            year(REG_DT) as Gbn, SNS_TYPE as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50
	                            group by year(REG_DT),  SNS_TYPE
	                            union all
	                            select 
		                            year(REG_DT) as Gbn, ('drop') as fortype, COUNT(1) as Total
	                            from MEMBER with(nolock)
	                            where LEVEL < 50 and DEL_FLAG = 'Y'
	                            group by year(REG_DT),  DEL_FLAG
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'article' as fortype, COUNT(1) as Total
	                            from ARTICLE with(nolock)
	                            group by year(REG_DT)
	                            union all
	                            select 
		                            year(REG_DT) as Gbn,'download' as fortype, COUNT(1) as Total
	                            from DOWNLOAD with(nolock)
	                            group by year(REG_DT)
                                union all
								select 
									 year(REG_DT) as Gbn, 'spot' as fortype, COUNT(1) as Total
								FROM PRINTER_MEMBER with(nolock)
	                            group by year(REG_DT)
								union all
								select 
									 year(REG_DT) as Gbn, 'printer' as fortype, COUNT(1) as Total
								FROM PRINTER with(nolock)
	                            group by year(REG_DT)
								union all
								select 
									 year(REG_DT) as Gbn, ('order') as fortype, COUNT(1) as Total
								FROM ORDER_REQ with(nolock)
	                            group by year(REG_DT)
								union all
								select 
									 year(REG_DT) as Gbn, 'totalprice' as fortype, SUM(UNIT_PRICE) as Total
								FROM ORDER_DETAIL with(nolock)
	                            group by year(REG_DT)
                                union all
								select 
									 year(REG_DT) as Gbn, 'orderMemCnt' as fortype, COUNT(DISTINCT MEMBER_NO) as Total
								FROM ORDER_REQ with(nolock)
	                            group by year(REG_DT)
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                 for fortype IN ([em],[fb],[drop],[article],[download],[spot],[printer],[order],[totalprice],[orderMemCnt])
                            ) as pvt";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<DashBoardStateT> result = (IList<DashBoardStateT>)session.CreateSQLQuery(query).AddEntity(typeof(DashBoardStateT)).List<DashBoardStateT>();

                return result;
            }
        }

        public IList<PrinterMemberStateT> SearchPrinterMemberStateTargetAll(string start, string end)
        {
            string[] arrGbn = { "오늘", "어제", "이번주", "지난주", "이번달", "지난달", "기간선택" };

            string query = string.Format(@"select
	                            Gbn, 
	                            ISNULL(pvt.[spotopen], 0) as SpotOpenCnt, 
	                            ISNULL(pvt.[uploadedprinter], 0) as UploadedPrinterCnt,
	                            ISNULL(pvt.[order], 0) as OrderCnt,
								ISNULL(pvt.[sales], 0) as sales
                            from
                            (
	                            select 
		                            '전체' as Gbn, 'spotopen' as fortype, COUNT(1) as Total
	                            from MEMBER M, PRINTER_MEMBER PM with(nolock)
	                            where M.LEVEL < 50 and M.NO = PM.MEMBER_NO
	                            union all
	                            select 
		                            '전체' as Gbn,'uploadedprinter' as fortype, COUNT(1) as Total
	                            from PRINTER P with(nolock)
	                            union all
	                            select 
		                            '전체' as Gbn,'order' as fortype, COUNT(1) as Total
	                            from ORDER_REQ O with(nolock)
                                where O.ORDER_STATUS = {0}
	                            union all
								select 
									'전체' as Gbn, 'sales' as fortype, SUM(OD.UNIT_PRICE) as Total
								from ORDER_REQ O, ORDER_DETAIL OD with(nolock)
                                where O.ORDER_STATUS = {0} and O.NO = OD.ORDER_NO
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([spotopen],[uploadedprinter],[order],[sales])
                            ) as pvt ", (int)MakersnEnumTypes.OrderState.거래완료);

            string add_where = string.Empty;
            foreach (var gbn in arrGbn)
            {
                switch (gbn)
                {
                    case "오늘":
                        add_where = string.Format(" {0} ", "datediff(day,MAIN.REG_DT,getdate())=0");
                        break;
                    case "어제":
                        add_where = string.Format(" {0} ", "datediff(day,MAIN.REG_DT,getdate())=1");
                        break;
                    case "이번주":
                        add_where = string.Format(" MAIN.REG_DT >= {0} AND MAIN.REG_DT < {1} ", "dateadd(day,2-datepart(weekday,getdate()),convert(varchar,getdate(),112))", "dateadd(day,9-datepart(weekday,getdate()),convert(varchar,getdate(),112))");
                        //add_where = string.Format(" and {0} ", "datediff(week,REG_DT-1,getdate())=0");
                        break;
                    case "지난주":
                        add_where = string.Format(" MAIN.REG_DT >= {0} AND MAIN.REG_DT < {1} ", "dateadd(day,-5-datepart(weekday,getdate()),convert(varchar,getdate(),112))", "dateadd(day,2-datepart(weekday,getdate()),convert(varchar,getdate(),112))");
                        //add_where = string.Format(" and {0} ", "datediff(week,REG_DT-1,getdate())=1");
                        break;
                    case "이번달":
                        add_where = string.Format(" MAIN.REG_DT >= {0} AND MAIN.REG_DT < {1} ", "dateadd(day,1-day(getdate()),convert(varchar,getdate(),112))", "dateadd(month,1,dateadd(day,1-day(getdate()),convert(varchar,getdate(),112)))");
                        //add_where = string.Format(" and {0} ", "datediff(month,REG_DT,getdate())=0");
                        break;
                    case "지난달":
                        add_where = string.Format(" MAIN.REG_DT >= {0} AND MAIN.REG_DT < {1} ", "dateadd(month,-1,dateadd(day,1-day(getdate()),convert(varchar,getdate(),112)))", "dateadd(day,1-day(getdate()),convert(varchar,getdate(),112))");
                        //add_where = string.Format(" and {0} ", "datediff(month,REG_DT,getdate())=1");
                        break;
                    case "기간선택":
                        add_where = " MAIN.REG_DT >= :start AND MAIN.REG_DT <= :end ";
                        break;
                    default:
                        add_where = " 1 = 1 ";
                        break;
                }

                //if (gbn == "기간선택") continue;

                query += string.Format(@" union all select
	                            Gbn, 
	                            ISNULL(pvt.[spotopen], 0) as SpotOpenCnt, 
	                            ISNULL(pvt.[uploadedprinter], 0) as UploadedPrinterCnt,
	                            ISNULL(pvt.[order], 0) as OrderCnt,
								ISNULL(pvt.[sales], 0) as Sales
                            from
                            (
	                            select 
		                            '{0}' as Gbn, 'spotopen' as fortype, COUNT(1) as Total
	                            from MEMBER M, PRINTER_MEMBER MAIN with(nolock)
	                            where M.LEVEL < 50 and M.NO = MAIN.MEMBER_NO and {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'uploadedprinter' as fortype, COUNT(1) as Total
	                            from PRINTER MAIN with(nolock)
                                where {1}
	                            union all
	                            select 
		                            '{0}' as Gbn,'order' as fortype, COUNT(1) as Total
	                            from ORDER_REQ MAIN with(nolock)
                                where MAIN.ORDER_STATUS = {2} and {1}
	                            union all
								select 
									'{0}' as Gbn, 'sales' as fortype, SUM(OD.UNIT_PRICE) as Total
								from ORDER_REQ MAIN, ORDER_DETAIL OD with(nolock)
                                where MAIN.ORDER_STATUS = {2} and MAIN.NO = OD.ORDER_NO and {1}
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([spotopen],[uploadedprinter],[order],[sales])
                            ) as pvt ", gbn, add_where, (int)MakersnEnumTypes.OrderState.거래완료);
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(PrinterMemberStateT));
                queryObj.SetParameter("start", start);
                queryObj.SetParameter("end", end);
                IList<PrinterMemberStateT> result = (IList<PrinterMemberStateT>)queryObj.List<PrinterMemberStateT>();

                return result;
            }
        }

        public IList<PrinterMemberStateT> SearchPrinterMemberStateTargetDaily(string start, string end)
        {
            string query = string.Format(@"select
	                            Gbn, 
	                            ISNULL(pvt.[spotopen], 0) as SpotOpenCnt, 
	                            ISNULL(pvt.[uploadedprinter], 0) as UploadedPrinterCnt,
	                            ISNULL(pvt.[order], 0) as OrderCnt,
								ISNULL(pvt.[sales], 0) as Sales
                            from
                            (
	                            select 
		                            convert(date, MAIN.REG_DT) as Gbn, 'spotopen' as fortype, COUNT(1) as Total
	                            from MEMBER M, PRINTER_MEMBER MAIN with(nolock)
	                            where M.LEVEL < 50 and M.NO = MAIN.MEMBER_NO and MAIN.REG_DT >= :start and MAIN.REG_DT < :end
                                group by convert(date, MAIN.REG_DT)
	                            union all
	                            select 
		                            convert(date, MAIN.REG_DT) as Gbn,'uploadedprinter' as fortype, COUNT(1) as Total
	                            from PRINTER MAIN with(nolock)
                                where MAIN.REG_DT >= :start and MAIN.REG_DT < :end
                                group by convert(date, MAIN.REG_DT)
	                            union all
	                            select 
		                            convert(date, MAIN.REG_DT) as Gbn,'order' as fortype, COUNT(1) as Total
	                            from ORDER_REQ MAIN with(nolock)
                                where MAIN.ORDER_STATUS = {2} and MAIN.REG_DT >= :start and MAIN.REG_DT < :end
                                group by convert(date, MAIN.REG_DT), MAIN.ORDER_STATUS
	                            union all
								select 
									convert(date, MAIN.REG_DT) as Gbn, 'sales' as fortype, SUM(OD.UNIT_PRICE) as Total
								from ORDER_REQ MAIN, ORDER_DETAIL OD with(nolock)
                                where MAIN.ORDER_STATUS = {2} and MAIN.NO = OD.ORDER_NO and MAIN.REG_DT >= :start and MAIN.REG_DT < :end
                                group by convert(date, MAIN.REG_DT), MAIN.ORDER_STATUS
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([spotopen],[uploadedprinter],[order],[sales])
                            ) as pvt ", start, end, (int)MakersnEnumTypes.OrderState.거래완료);

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(PrinterMemberStateT));
                queryObj.SetParameter("start", start);
                queryObj.SetParameter("end", end);
                IList<PrinterMemberStateT> result = (IList<PrinterMemberStateT>)queryObj.List<PrinterMemberStateT>();

                return result;
            }

        }
        public IList<PrinterMemberStateT> SearchPrinterMemberStateTargetMonth(string start, string end)
        {
            string query = string.Format(@"select
	                            Gbn, 
	                            ISNULL(pvt.[spotopen], 0) as SpotOpenCnt, 
	                            ISNULL(pvt.[uploadedprinter], 0) as UploadedPrinterCnt,
	                            ISNULL(pvt.[order], 0) as OrderCnt,
								ISNULL(pvt.[sales], 0) as Sales
                            from
                            (
	                            select 
		                            month(MAIN.REG_DT)  as Gbn, 'spotopen' as fortype, COUNT(1) as Total
	                            from MEMBER M, PRINTER_MEMBER MAIN with(nolock)
	                            where M.LEVEL < 50 and M.NO = MAIN.MEMBER_NO and MAIN.REG_DT >= :start and MAIN.REG_DT < :end
                                group by month(MAIN.REG_DT) 
	                            union all
	                            select 
		                            month(MAIN.REG_DT)  as Gbn,'uploadedprinter' as fortype, COUNT(1) as Total
	                            from PRINTER MAIN with(nolock)
                                where MAIN.REG_DT >= :start and MAIN.REG_DT < :end
                                group by month(MAIN.REG_DT) 
	                            union all
	                            select 
		                            month(MAIN.REG_DT)  as Gbn,'order' as fortype, COUNT(1) as Total
	                            from ORDER_REQ MAIN with(nolock)
                                where MAIN.ORDER_STATUS = {2} and MAIN.REG_DT >= :start and MAIN.REG_DT < :end
                                group by month(MAIN.REG_DT) , MAIN.ORDER_STATUS
	                            union all
								select 
									month(MAIN.REG_DT)  as Gbn, 'sales' as fortype, SUM(OD.UNIT_PRICE) as Total
								from ORDER_REQ MAIN, ORDER_DETAIL OD with(nolock)
                                where MAIN.ORDER_STATUS = {2} and MAIN.NO = OD.ORDER_NO and MAIN.REG_DT >= :start and MAIN.REG_DT < :end
                                group by month(MAIN.REG_DT) , MAIN.ORDER_STATUS
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([spotopen],[uploadedprinter],[order],[sales])
                            ) as pvt ", (int)MakersnEnumTypes.OrderState.거래완료);

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(PrinterMemberStateT));
                queryObj.SetParameter("start", start);
                queryObj.SetParameter("end", end);
                IList<PrinterMemberStateT> result = (IList<PrinterMemberStateT>)queryObj.List<PrinterMemberStateT>();
                return result;
            }

        }
        public IList<PrinterMemberStateT> SearchPrinterMemberStateTargetYear()
        {
            string query = string.Format(@"select
	                            Gbn, 
	                            ISNULL(pvt.[spotopen], 0) as SpotOpenCnt, 
	                            ISNULL(pvt.[uploadedprinter], 0) as UploadedPrinterCnt,
	                            ISNULL(pvt.[order], 0) as OrderCnt,
								ISNULL(pvt.[sales], 0) as Sales
                            from
                            (
	                            select 
		                            year(MAIN.REG_DT)  as Gbn, 'spotopen' as fortype, COUNT(1) as Total
	                            from MEMBER M, PRINTER_MEMBER MAIN with(nolock)
	                            where M.LEVEL < 50 and M.NO = MAIN.MEMBER_NO
                                group by year(MAIN.REG_DT) 
	                            union all
	                            select 
		                            year(MAIN.REG_DT)  as Gbn,'uploadedprinter' as fortype, COUNT(1) as Total
	                            from PRINTER MAIN with(nolock)
                                group by year(MAIN.REG_DT) 
	                            union all
	                            select 
		                            year(MAIN.REG_DT)  as Gbn,'order' as fortype, COUNT(1) as Total
	                            from ORDER_REQ MAIN with(nolock)
                                where MAIN.ORDER_STATUS = {0} 
                                group by year(MAIN.REG_DT) , MAIN.ORDER_STATUS
	                            union all
								select 
									year(MAIN.REG_DT)  as Gbn, 'sales' as fortype, SUM(OD.UNIT_PRICE) as Total
								from ORDER_REQ MAIN, ORDER_DETAIL OD with(nolock)
                                where MAIN.ORDER_STATUS = {0} and MAIN.NO = OD.ORDER_NO 
                                group by year(MAIN.REG_DT) , MAIN.ORDER_STATUS
                            ) as tb
                            PIVOT
                            (
                                SUM(Total)
                                for fortype IN ([spotopen],[uploadedprinter],[order],[sales])
                            ) as pvt ",  (int)MakersnEnumTypes.OrderState.거래완료);

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<PrinterMemberStateT> result = (IList<PrinterMemberStateT>)session.CreateSQLQuery(query).AddEntity(typeof(PrinterMemberStateT)).List<PrinterMemberStateT>();

                return result;
            }

        }

        public int UpdateMemberByOrder(MemberT data)
        {
            int result = 0;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                MemberT mem = session.QueryOver<MemberT>().Where(w => w.No == data.No).Take(1).SingleOrDefault<MemberT>();
                if (mem != null)
                {
                    mem.Email = data.Email;
                    mem.CellPhone = data.CellPhone;
                    mem.UpdDt = data.UpdDt;
                    mem.UpdId = data.UpdId;
                    session.Update(mem);
                    session.Flush();
                    result = 1;
                }
            }
            return result;
        }

        public IList<MemberT> GetMemberListForSendAllNotice()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<MemberT>().Where(w => w.DelFlag != "Y" && w.Level < 50).List<MemberT>();
            }
        }
    }
}
