using Makersn.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.BizDac
{
    public class ArticleCommentDac
    {
        public IList<ArticleCommentT> GetArticleCommentListByNo(int articleNo)
        {
//            string query = @"SELECT M.PROFILE_PIC, M.NAME , AC.CONTENT, AC.REG_DT, AC.NO, M.NO AS MEMBER_NO, AC.MEMBER_NO_REF, AC.ARTICLE_NO
//                                FROM ARTICLE_COMMENT AC WITH(NOLOCK) INNER JOIN MEMBER M WITH(NOLOCK)
//						                                ON AC.MEMBER_NO = M.NO
//                                WHERE ARTICLE_NO = " + articleNo + " ORDER BY REG_DT DESC";
//            using (ISession session = NHibernateHelper.OpenSession())
//            {
//                IList<ArticleCommentT> list = new List<ArticleCommentT>();
//                IList<object[]> result = session.CreateSQLQuery(query).List<object[]>();
//                foreach (object[] row in result)
//                {
//                    ArticleCommentT a = new ArticleCommentT();
//                    a.ProfilePic = (string)row[0];
//                    a.MemberName = (string)row[1];
//                    a.Content = (string)row[2];
//                    a.Regdt = (DateTime)row[3];
//                    a.No = (long)row[4];
//                    a.MemberNo = (int)row[5];
//                    a.MemberNoRef = (int)row[6];
//                    a.ArticleNo = (int)row[7];
//                    list.Add(a);
//                }

//                return list;
//            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<ArticleCommentT> list = session.QueryOver<ArticleCommentT>().Where(w => w.ArticleNo == articleNo && w.Depth == 0).List<ArticleCommentT>();

                foreach (ArticleCommentT comment in list)
                {
                    MemberT member = session.QueryOver<MemberT>().Where(w => w.No == comment.MemberNo).Take(1).SingleOrDefault<MemberT>();
                    comment.MemberName = member.Name;
                    comment.ProfilePic = member.ProfilePic;
                    comment.replyList = session.QueryOver<ArticleCommentT>().Where(w => w.ArticleNo == articleNo && w.RefNo == comment.RefNo && w.Depth == 1).OrderBy(o => o.Regdt).Asc.List<ArticleCommentT>();

                    foreach (ArticleCommentT inComment in comment.replyList)
                    {
                        MemberT inMem = session.QueryOver<MemberT>().Where(w => w.No == inComment.MemberNo).Take(1).SingleOrDefault<MemberT>();
                        inComment.ProfilePic = inMem.ProfilePic;
                        inComment.MemberName = inMem.Name;
                    }
                }
                return list;
            }
        }

        public void DeleteArticleCommentByNo(int no)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ArticleCommentT comment = session.QueryOver<ArticleCommentT>().Where(a => a.No == no).SingleOrDefault<ArticleCommentT>();
                    if (comment != null)
                    {
                        session.Delete(comment);
                    }
                    transaction.Commit();
                    session.Flush();
                }
            }
        }

        public void UpdateArticleCommentByNo(ArticleCommentT act)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ArticleCommentT updAct = session.QueryOver<ArticleCommentT>().Where(a => a.No == act.No).SingleOrDefault<ArticleCommentT>();
                    if (updAct != null)
                    {
                        updAct.Content = act.Content;
                        updAct.UpdId = act.UpdId;
                        updAct.UpdDt = act.UpdDt;
                        session.Update(updAct);
                    }
                    transaction.Commit();
                    session.Flush();
                }
            }
        }

        #region 댓글 삽입(댓글의 댓글삽입은 InsertArticleCommentInReply)
        public long InsertArticleComment(ArticleCommentT act)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    act.RefNo = (long)session.Save(act);
                    session.Update(act);
                    transaction.Commit();
                    session.Flush();
                    return act.RefNo;
                }
            }
        }
        #endregion

        #region 댓글의 댓글 삽입(그냥 댓글삽입은 InsertArticleComment)
        public long InsertArticleCommentInReply(ArticleCommentT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    long no = (long)session.Save(data);
                    transaction.Commit();
                    session.Flush();
                    return no;
                }
            }
        }
        #endregion

        //public int GetArticleCommentCntByArticleNo(int no)
        //{
        //    using (ISession session = NHibernateHelper.OpenSession())
        //    {
        //        return session.QueryOver<ArticleCommentT>().Where(w => w.ArticleNo == no).RowCount();
        //    }
        //}

        #region 답글의 부모 댓글 가져오기
        public ArticleCommentT GetRefCommentByRefNo(int refNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<ArticleCommentT>().Where(w => w.RefNo == refNo && w.Depth == 0).OrderBy(o => o.No).Asc.Take(1).SingleOrDefault<ArticleCommentT>();
            }
        }
        #endregion
    }
}
