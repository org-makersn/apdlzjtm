using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makersn.Models;
using NHibernate;

namespace Makersn.BizDac
{
    public class ReportDac
    {
        public IList<ReportT> getReportList()
        {
            string query = @"SELECT AR.NO, AF.PATH, AF.NAME, TD.TITLE, C.NAME AS CATEGORY, M.NAME AS A_NAME , A.REG_DT, (SELECT NAME FROM MEMBER WHERE NO = AR.MEMBER_NO) AS R_NAME, AR.REG_DT AS R_DT, A.VISIBILITY, A.NO AS A_NO
                            FROM ARTICLE_REPORT AR INNER JOIN  ARTICLE A WITH(NOLOCK) 
												ON AR.ARTICLE_NO = A.NO
												INNER JOIN ARTICLE_FILE AS AF WITH(NOLOCK)
					                            ON A.MAIN_IMAGE = AF.NO
					                            LEFT JOIN CODE AS C WITH(NOLOCK)
					                            ON A.CODE_NO = C.NO
					                            LEFT JOIN MEMBER AS M WITH(NOLOCK)
					                            ON A.MEMBER_NO = M.NO
												INNER JOIN TRANSLATION_DETAIL TD WITH(NOLOCK)
												ON TD.ARTICLE_NO = A.NO AND TD.NO = (SELECT MIN(NO) FROM TRANSLATION_DETAIL TD2 WITH(NOLOCK) WHERE TD2.ARTICLE_NO = A.NO)";
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                
                IList<object[]> results = queryObj.List<object[]>();
                session.Flush();

                IList<ReportT> list = new List<ReportT>();
                foreach (object[] row in results)
                {
                    ReportT r = new ReportT();
                    r.No = (int)row[0];
                    r.Path = (string)row[1] + (string)row[2];
                    r.Title = (string)row[3];
                    r.Cate = (string)row[4];
                    r.AName = (string)row[5];
                    r.ADt = (DateTime)row[6];
                    r.RName = (string)row[7];
                    r.RDt = (DateTime)row[8];
                    r.Visibility = (string)row[9];
                    r.ANo = (int)row[10];
                    list.Add(r);
                }
                return list;
            }

        }

        public ReportT getReportByNo(int no)
        {
            ReportT reportT = new ReportT();

            string query = @"SELECT AR.NO, AF.PATH, AF.NAME, TD.TITLE, C.NAME AS CATEGORY, M.NAME AS A_NAME , A.REG_DT, 
                            (SELECT NAME FROM MEMBER WHERE NO = AR.MEMBER_NO) AS R_NAME, AR.REG_DT AS R_DT, A.VISIBILITY, A.NO AS A_NO,
                            (SELECT ID FROM MEMBER WHERE NO = A.MEMBER_NO) AS A_ID, (SELECT ID FROM MEMBER WHERE NO = AR.MEMBER_NO) AS R_ID,
                            AR.REGISTER_COMMENT, AR.REPORTER_COMMENT, AR.REPORT, AR.STATE

                            FROM ARTICLE_REPORT AR INNER JOIN  ARTICLE A WITH(NOLOCK) 
												ON AR.ARTICLE_NO = A.NO
												INNER JOIN ARTICLE_FILE AS AF WITH(NOLOCK)
					                            ON A.MAIN_IMAGE = AF.NO
					                            LEFT JOIN CODE AS C WITH(NOLOCK)
					                            ON A.CODE_NO = C.NO
					                            LEFT JOIN MEMBER AS M WITH(NOLOCK)
					                            ON A.MEMBER_NO = M.NO
                                                INNER JOIN TRANSLATION_DETAIL TD WITH(NOLOCK)
                                                ON A.NO = TD.ARTICLE_NO
                            WHERE AR.No = :no ";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("no", no);

                IList<object[]> results = queryObj.List<object[]>();
                session.Flush();

                foreach (object[] row in results)
                {
                    reportT.No = (int)row[0];
                    reportT.Path = (string)row[1] + (string)row[2];
                    reportT.Title = (string)row[3];
                    reportT.Cate = (string)row[4];
                    reportT.AName = (string)row[5];
                    reportT.ADt = (DateTime)row[6];
                    reportT.RName = (string)row[7];
                    reportT.RDt = (DateTime)row[8];
                    reportT.Visibility = (string)row[9];
                    reportT.ANo = (int)row[10];
                    reportT.AId = (string)row[11];
                    reportT.RId = (string)row[12];
                    reportT.RegisterComment = (string)row[13];
                    reportT.ReporterComment = (string)row[14];
                    reportT.Report = (string)row[15];
                    reportT.State = (int)row[16];
                }
                return reportT;
            }
        }

        public void DeleteReport(int no)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        ReportT reportT = new ReportT();
                        reportT.No = no;

                        session.Delete(reportT);

                        transaction.Commit();
                        session.Flush();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateState(ReportT r)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ReportT report = session.QueryOver<ReportT>().Where(w => w.No == r.No).SingleOrDefault<ReportT>();
                    if (report != null)
                    {
                        report.ReporterComment = r.ReporterComment;
                        report.RegisterComment = r.RegisterComment;
                        report.State = r.State;
                        report.UpdDt = r.UpdDt;
                        report.UpdId = r.UpdId;
                        session.Update(report);

                        ArticleT a = session.QueryOver<ArticleT>().Where(w => w.No == report.ArticleNo).SingleOrDefault<ArticleT>();

                        NoticeT notice = new NoticeT();
                        notice.MemberNo = 1;//세션 프로필 번호 가져와야함
                        notice.Type = "notice";
                        notice.RegDt = DateTime.Now;
                        notice.RegIp = r.RegIp;
                        notice.RegId = "admin"; // 세션 아이디
                        notice.IsNew = "Y";
                        notice.ArticleNo = a.No;
                        notice.IdxName = "n:" + a.No;

                        NoticeT registerNotice = notice;
                        NoticeT reportNotice = new NoticeT();

                        registerNotice.MemberNoRef = a.MemberNo; //게시자 번호
                        registerNotice.Comment = report.RegisterComment;

                        switch (report.State)
                        {
                            case 1:
                                session.Save(registerNotice);

                                reportNotice.MemberNo = notice.MemberNo;
                                reportNotice.Type = notice.Type;
                                reportNotice.RegDt = notice.RegDt;
                                reportNotice.RegIp = notice.RegIp;
                                reportNotice.RegId = notice.RegId; 
                                reportNotice.IsNew = notice.IsNew;
                                reportNotice.ArticleNo = notice.ArticleNo;
                                reportNotice.IdxName = notice.IdxName;
                                reportNotice.MemberNoRef =report.MemberNo;//신고자 번호
                                reportNotice.Comment = report.ReporterComment;
                                session.Save(reportNotice);
                                break;

                            case 2:
                                session.Save(registerNotice);
                                break;

                            case 3:
                                reportNotice = notice;
                                reportNotice.MemberNoRef = report.MemberNo; //신고자 번호
                                reportNotice.Comment = report.ReporterComment;
                                session.Save(reportNotice);
                                break;
                        }
                        transaction.Commit();
                        session.Flush();
                    }
                }
            }
        }

        public int InsertReport(ReportT report)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    int result = (Int32)session.Save(report);
                    transaction.Commit();
                    session.Flush();
                    return result;
                }
            }
        }

    }
}
