using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using Makersn.Models;
using NHibernate;

namespace Makersn.BizDac
{
    public class NoticesDac
    {
        public IList<BoardT> GetBoardList(string flag)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<BoardT> list = session.QueryOver<BoardT>().Where(n => n.BoardSetNo == 1 && n.LangFlag == flag).OrderBy(o => o.BoardSetNoSeq).Desc.List();
                session.Flush();
                return list;
            }
        }

        public BoardT GetBoard(int no)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                BoardT notices = session.QueryOver<BoardT>().Where(n => n.BoardSetNo == 1 && n.No == no).SingleOrDefault<BoardT>();
                session.Flush();
                return notices;
            }
        }

        public BoardT GetPreBoard(int no)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                BoardT notices = new BoardT();
                notices = session.QueryOver<BoardT>().Where(n => n.BoardSetNo == 1 && n.No < no).OrderBy(o => o.No).Desc.Take(1).SingleOrDefault<BoardT>();
                session.Flush();
                return notices;
            }
        }
        public BoardT GetNextBoard(int no)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                BoardT notices = new BoardT();
                notices = session.QueryOver<BoardT>().Where(n => n.BoardSetNo == 1 && n.No > no).OrderBy(o => o.No).Asc.Take(1).SingleOrDefault<BoardT>();
                session.Flush();
                return notices;
            }
        }

        public int InsertBoard(BoardT n)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                int result = (Int32)session.Save(n);
                session.Flush();

                return result;
            }
        }
        public void DeleteBoard(BoardT n)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                session.Delete(n);
                session.Flush();
            }
        }

        public void UpdateBoard(BoardT n)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        BoardT notices = session.QueryOver<BoardT>().Where(a => a.No == n.No).SingleOrDefault<BoardT>();
                        if (notices != null)
                        {
                            notices.Title = n.Title;
                            notices.SemiContent = n.SemiContent;
                            notices.UpdDt = n.UpdDt;
                            notices.UpdId = n.UpdId;
                            session.Update(notices);
                            transaction.Commit();
                            session.Flush();
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public IList<BoardT> GetNoticesByContent(string text, string gubun, string flag)
        {
            IList<BoardT> list = new List<BoardT>();
            string[] textList = text.Split(' ');

            string query = string.Format(@"SELECT NO, TITLE, SEMI_CONTENT, REG_DT
                                FROM BOARD WITH(NOLOCK)
                                WHERE 1=1 AND LANG_FLAG='{0}'", flag);
            if (gubun == "sfl1")
            {
                for (int i = 0; i < textList.Length; i++)
                {
                    //if (i == 0) { query += @"TITLE LIKE '%" + textList[i] + "%' OR SEMI_CONTENT LIKE '%" + textList[i] + "%' "; }
                    //else { query += " OR TITLE LIKE '%" + textList[i] + "%' OR SEMI_CONTENT LIKE '%" + textList[i] + "%' "; }
                    query += " AND (TITLE LIKE ? OR SEMI_CONTENT LIKE ?) ";
                }
            }
            else if (gubun == "sfl2")
            {
                for (int i = 0; i < textList.Length; i++)
                {
                    //if (i == 0) { query += @"TITLE LIKE '%" + textList[i] + "%' "; }
                    //else { query += " OR TITLE LIKE '%" + textList[i] + "%' "; }
                    query += " AND TITLE LIKE ? ";
                }
            }
            else if (gubun == "sfl3")
            {
                for (int i = 0; i < textList.Length; i++)
                {
                    //if (i == 0) { query += @"SEMI_CONTENT LIKE '%" + textList[i] + "%' "; }
                    //else { query += " OR SEMI_CONTENT LIKE '%" + textList[i] + "%' "; }
                    query += " AND SEMI_CONTENT LIKE ? ";
                }
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                if (gubun == "sfl1")
                {
                    for (int i = 0; i < textList.Length; i++)
                    {
                        queryObj.SetParameter(i * 2, "%" + textList[i] + "%");
                        queryObj.SetParameter(i * 2 + 1, "%" + textList[i] + "%");
                    }
                }
                else if (gubun == "sfl2" || gubun == "sfl3")
                {
                    for (int i = 0; i < textList.Length; i++)
                    {
                        queryObj.SetParameter(i, "%" + textList[i] + "%");
                    }
                }

                IList<object[]> results = queryObj.List<object[]>();
                session.Flush();
                if (results == null) { return list; }

                foreach (object[] row in results)
                {
                    BoardT n = new BoardT();
                    n.No = (int)row[0];
                    n.Title = (string)row[1];
                    n.SemiContent = (string)row[2];
                    n.RegDt = (DateTime)row[3];
                    list.Add(n);
                }
            }


            return list;
        }

        public int GetNoticesCntByMemberNo(int no)
        {
            string query = string.Format(@" SELECT 
                                             (SELECT COUNT(0) FROM NOTICE N WITH(NOLOCK) INNER JOIN MEMBER M WITH(NOLOCK)
					                                            ON N.MEMBER_NO_REF = M.NO
					                                            AND M.ALL_IS='on' AND M.REPLE_IS='on' AND N.TYPE = 'comment' AND N.IS_NEW = 'Y'
					                                            WHERE M.NO = {0}) +

                                             (SELECT COUNT(0) FROM NOTICE N WITH(NOLOCK) INNER JOIN MEMBER M WITH(NOLOCK)
					                                            ON N.MEMBER_NO_REF = M.NO
					                                            AND M.ALL_IS='on' AND M.LIKE_IS='on' AND N.TYPE = 'like' AND N.IS_NEW = 'Y'
					                                            WHERE M.NO = {0})+

                                            (SELECT COUNT(0) FROM NOTICE N WITH(NOLOCK) INNER JOIN MEMBER M WITH(NOLOCK)
					                                            ON N.MEMBER_NO_REF = M.NO
					                                            AND N.TYPE = 'AllNotice' AND N.IS_NEW = 'Y'
					                                            WHERE M.NO = {0})+

                                            (SELECT COUNT(0) FROM NOTICE N WITH(NOLOCK) INNER JOIN MEMBER M WITH(NOLOCK)
					                                            ON N.MEMBER_NO_REF = M.NO
					                                            AND N.TYPE = 'inComment' AND N.IS_NEW = 'Y' AND M.REPLE_IS='on' 
					                                            WHERE M.NO = {0})+

                                            (SELECT COUNT(0) FROM FOLLOWER F WITH(NOLOCK) INNER JOIN MEMBER M WITH(NOLOCK)
					                                            ON F.MEMBER_NO_REF = M.NO
					                                            AND  M.ALL_IS='on' AND M.FOLLOW_IS='on' AND F.IS_NEW = 'Y'
					                                            WHERE M.NO = {0})+

                                            (SELECT COUNT(0) FROM PRINTER_NOTICE WITH(NOLOCK)
                                                                WHERE MEMBER_NO_REF = {0} AND IS_NEW = 'Y')", no);
            using (ISession session = NHibernateHelper.OpenSession())
            {
                int result = (int)session.CreateSQLQuery(query).UniqueResult();

                //int noticeCnt = session.QueryOver<NoticeT>().Where(n => n.MemberNoRef == no && n.IsNew == "Y").RowCount();
                //int followCnt = session.QueryOver<FollowerT>().Where(f => f.MemberNoRef == no && f.IsNew=="Y").RowCount();
                //return noticeCnt + followCnt;
                return result;
            }
        }

        public IList<NoticeT> GetNoticeList(int no)
        {
            string query = string.Format(@"SELECT M.PROFILE_PIC, M.NAME, N.TYPE, N.COMMENT, N.REG_DT, N.ARTICLE_NO, TD.TITLE, N.MEMBER_NO
							FROM NOTICE N WITH(NOLOCK) INNER JOIN MEMBER M WITH(NOLOCK)
                            ON N.MEMBER_NO = M.NO AND M.DEL_FLAG = 'N'
                            LEFT OUTER JOIN ARTICLE A WITH(NOLOCK)
                            ON N.ARTICLE_NO = A.NO
						    LEFT OUTER JOIN TRANSLATION_DETAIL TD WITH(NOLOCK)
						    ON TD.ARTICLE_NO = A.NO AND TD.NO = (SELECT MIN(NO) FROM TRANSLATION_DETAIL TD2 WITH(NOLOCK) WHERE TD2.ARTICLE_NO = A.NO )
                            WHERE N.MEMBER_NO_REF = :no 

							AND (
									(((SELECT LIKE_IS FROM MEMBER WITH(NOLOCK) WHERE NO = :no) = 'on' AND N.TYPE='like')
									OR ((SELECT REPLE_IS FROM MEMBER WITH(NOLOCK) WHERE NO = :no) = 'on' AND N.TYPE='comment')
                                    OR N.TYPE='AllNotice'
                                    OR N.TYPE = 'inComment') 
									AND (SELECT ALL_IS FROM MEMBER WITH(NOLOCK) WHERE NO = :no) = 'on'
								)

							UNION ALL

							SELECT M.PROFILE_PIC ,M.NAME,'newArticle' AS TYPE, '' AS COMMENT, A.REG_DT, A.NO, TD.TITLE, A.MEMBER_NO
							FROM ARTICLE A WITH(NOLOCK)
							INNER JOIN MEMBER M WITH(NOLOCK)
							ON A.MEMBER_NO = M.NO AND M.DEL_FLAG = 'N'
							INNER JOIN TRANSLATION_DETAIL TD WITH(NOLOCK)
							ON TD.ARTICLE_NO = A.NO AND TD.NO = (SELECT MIN(NO) FROM TRANSLATION_DETAIL TD2 WITH(NOLOCK) WHERE TD2.ARTICLE_NO = A.NO )
                            WHERE MEMBER_NO IN (SELECT MEMBER_NO_REF FROM FOLLOWER F WITH(NOLOCK) WHERE A.REG_DT >= F.REG_DT AND F.MEMBER_NO = :no)
							AND (SELECT FOLLOW_IS FROM MEMBER WITH(NOLOCK) WHERE NO = :no) = 'on'
							AND (SELECT ALL_IS FROM MEMBER WITH(NOLOCK) WHERE NO = :no) = 'on'
							

							UNION ALL

							SELECT M.PROFILE_PIC ,M.NAME,'newFollower' AS TYPE, '' AS COMMENT, F.REG_DT, '' AS NO, '' AS TITLE, M.NO
							FROM FOLLOWER F WITH(NOLOCK)
							INNER JOIN MEMBER M WITH(NOLOCK)
							ON F.MEMBER_NO = M.NO AND M.DEL_FLAG = 'N'
							WHERE MEMBER_NO_REF = :no
							AND (SELECT FOLLOW_IS FROM MEMBER WITH(NOLOCK) WHERE NO = :no) = 'on'
							AND (SELECT ALL_IS FROM MEMBER WITH(NOLOCK) WHERE NO = :no) = 'on'

							UNION ALL

							SELECT M.PROFILE_PIC ,M.NAME, N.TYPE, N.COMMENT, N.REG_DT, N.ARTICLE_NO, '' AS TITLE, N.MEMBER_NO
							FROM NOTICE N WITH(NOLOCK) INNER JOIN MEMBER M WITH(NOLOCK)
                            ON N.MEMBER_NO = M.NO AND M.DEL_FLAG = 'N'
                            WHERE N.MEMBER_NO_REF = :no 
                            AND N.TYPE = 'notice' AND N.ARTICLE_NO is not null

                            UNION ALL
        
							SELECT M.PROFILE_PIC, M.NAME, PN.TYPE, PN.COMMENT, PN.REG_DT, PN.ORDER_NO, '' AS TITLE, PN.MEMBER_NO
                            FROM PRINTER_NOTICE PN WITH(NOLOCK) INNER JOIN MEMBER M WITH(NOLOCK)
		                    ON PN.MEMBER_NO = M.NO
		                    WHERE PN.MEMBER_NO_REF = :no
                            ", no);

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("no", no);
                IList<object[]> result = queryObj.List<object[]>();

                IList<NoticeT> list = new List<NoticeT>(); 
                foreach (object[] row in result)
                {
                    NoticeT n = new NoticeT();
                    n.MemberProfilePic = (string)row[0];
                    n.MemberName = (string)row[1];
                    n.Type = (string)row[2];
                    n.Comment = (string)row[3];
                    n.RegDt = (DateTime)row[4];
                    //n.RefNo = (long)row[5];
                    n.RefNo = Convert.ToInt64(row[5]);
                    n.ArticleTItle = (string)row[6];
                    n.MemberNo = (int)row[7];
                    list.Add(n);
                }
                session.Flush();
                return list;
            }
        }

        #region
        public IList<NoticeT> GetFollowerNewArticleLIst(int memberNo)
        {
            string query = @"		SELECT A.NO, M.NAME, A.REG_DT
							FROM ARTICLE A WITH(NOLOCK)
							INNER JOIN MEMBER M WITH(NOLOCK)
							ON A.MEMBER_NO = M.NO
							WHERE MEMBER_NO IN (SELECT MEMBER_NO_REF FROM FOLLOWER F WITH(NOLOCK) WHERE A.REG_DT >= F.REG_DT AND F.MEMBER_NO = :memberNo )";
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("memberNo", memberNo);
                IList<object[]> result = queryObj.List<object[]>();

                IList<NoticeT> list = new List<NoticeT>();
                foreach (object[] row in result)
                {
                    NoticeT n = new NoticeT();
                    n.ArticleNo = (int)row[0];
                    n.MemberName = (string)row[1];
                    n.RegDt = (DateTime)row[2];
                    n.Type = "newArticle";
                    list.Add(n);
                }
                return list;
            }
        }
        #endregion

        #region 확인여부
        public void UpdateNoticeIsNew(int no)
        {
            string query = @"UPDATE NOTICE SET IS_NEW = 'N' WHERE IS_NEW='Y' AND MEMBER_NO_REF = :no";
            query += @" UPDATE FOLLOWER SET IS_NEW = 'N' WHERE IS_NEW='Y' AND MEMBER_NO_REF = :no";
            query += @" UPDATE PRINTER_NOTICE SET IS_NEW = 'N' WHERE IS_NEW='Y' AND MEMBER_NO_REF = :no";
            using (ISession session = NHibernateHelper.OpenSession())
            {

                using (ITransaction transaction = session.BeginTransaction())
                {
                    //IList<NoticeT> list = (IList<NoticeT>)session.CreateSQLQuery(query).List<NoticeT>();

                    //IList<NoticeT> list = session.QueryOver<NoticeT>().Where(w => w.MemberNoRef == no && w.IsNew == "Y").List<NoticeT>();
                    //if (list.Count != 0)
                    //{
                    //    session.Update(list);
                    //}
                    IQuery queryObj = session.CreateSQLQuery(query);
                    queryObj.SetParameter("no", no);
                    queryObj.ExecuteUpdate();

                    transaction.Commit();
                    session.Flush();
                }
            }
        }
        #endregion

        public void InsertNoticesLikes(NoticeT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                NoticeT notice = session.QueryOver<NoticeT>().Where(w => w.ArticleNo == data.ArticleNo && w.MemberNo == data.MemberNo && w.MemberNoRef == data.MemberNoRef && w.Type == "like").Take(1).SingleOrDefault<NoticeT>();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (notice != null)
                    {
                        session.Delete(notice);
                    }
                    else
                    {
                        session.Save(data);
                    }
                    transaction.Commit();
                    session.Flush();
                }
            }
        }

        #region 출력 이미지 가져오기
        public IList<NoticeT> GetPrinterOutputImage(IList<NoticeT> data, IList<NoticeT> result)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                foreach (NoticeT noti in data)
                {
                    noti.OutputImageList = session.QueryOver<PrinterOutputImageT>().Where(w => w.OrderNo == noti.RefNo).List<PrinterOutputImageT>();
                    result.Add(noti);
                }
            }

            return result;
        }
        #endregion

        #region All Notice 삽입
        public bool InsertAllNotice(NoticeT data, IList<MemberT> memberList)
        {
            bool result = false;

            string query = "BEGIN TRANSACTION; ";

            foreach (MemberT member in memberList)
            {
                query += string.Format(@" INSERT INTO NOTICE (IDX_NAME, ARTICLE_NO, MEMBER_NO, MEMBER_NO_REF, REF_NO, TYPE, COMMENT, IS_NEW, REG_ID, REG_DT, REG_IP)
                    values('{0}',{1},{2},{3},{4},'{5}','{6}','{7}','{8}',GETDATE(),'{10}') ",
                    data.IdxName, data.ArticleNo, data.MemberNo, member.No, data.RefNo, data.Type, data.Comment, data.IsNew, data.RegId, data.RegDt, data.RegIp);
            }
            query += "COMMIT TRANSACTION;";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                try
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.CreateSQLQuery(query).ExecuteUpdate();
                        transaction.Commit();
                        session.Flush();
                    }
                    result = true;
                }
                catch
                {

                }
            }
            return result;

//            bool result = false;

//            //string query = "BEGIN TRANSACTION; ";
//            string query = "";

//            foreach (MemberT member in memberList)
//            {
////                query += string.Format(@" INSERT INTO NOTICE (IDX_NAME, ARTICLE_NO, MEMBER_NO, MEMBER_NO_REF, REF_NO, TYPE, COMMENT, IS_NEW, REG_ID, REG_DT, REG_IP)
////                    values('{0}',{1},{2},{3},{4},'{5}','{6}','{7}','{8}',GETDATE(),'{10}') ",
////                    data.IdxName, data.ArticleNo, data.MemberNo, member.No, data.RefNo, data.Type, data.Comment, data.IsNew, data.RegId, data.RegDt, data.RegIp);

//                query += string.Format(@" INSERT INTO NOTICE (IDX_NAME, ARTICLE_NO, MEMBER_NO, MEMBER_NO_REF, REF_NO, TYPE, COMMENT, IS_NEW, REG_ID, REG_DT, REG_IP)
//                    values(:IdxName, :ArticleNo,:MemberNo,?,:RefNo,:_Type,:Comment,:IsNew,:RegId,GETDATE(),:RegIp) ");
//            }
//            //query += "COMMIT TRANSACTION;";

//            using (ISession session = NHibernateHelper.OpenSession())
//            {
//                try
//                {
//                    using (ITransaction transaction = session.BeginTransaction())
//                    {
//                        IQuery queryObj = session.CreateSQLQuery(query);
//                        queryObj.SetParameter("IdxName", data.IdxName);
//                        queryObj.SetParameter("ArticleNo", data.ArticleNo);
//                        queryObj.SetParameter("MemberNo", data.MemberNo);
//                        queryObj.SetParameter("RefNo", data.RefNo);
//                        queryObj.SetParameter("_Type", data.Type);
//                        queryObj.SetParameter("Comment", data.Comment);
//                        queryObj.SetParameter("IsNew", data.IsNew);
//                        queryObj.SetParameter("RegId", data.RegId);
//                        queryObj.SetParameter("RegIp", data.RegIp);

//                        for (int i = 0; i < memberList.Count; i ++  ) {

//                            queryObj.SetParameter(i, memberList[i].No);
//                        }
//                        int a = queryObj.ExecuteUpdate();
//                        transaction.Commit();
//                        session.Flush();
//                    }
//                    result = true;
//                }
//                catch
//                {

//                }
//            }
//            return result;
        }
        #endregion

        public IList<NoticeT> GetAllNoticeList()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<NoticeT>().Where(w => w.Type == "AllNotice").List<NoticeT>();
            }
        }

        #region
        public void InsertNoticeByComment(NoticeT data)
        {
            using(ISession session = NHibernateHelper.OpenSession())
            {
                session.Save(data);
            }
        }
        #endregion
    }
}
