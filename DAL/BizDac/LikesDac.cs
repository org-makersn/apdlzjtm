using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Makersn.Models;

namespace Makersn.BizDac
{
    public class LikesDac
    {
        public int GetLikesCntByMemberNo(int no)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<LikesT>().Where(l => l.MemberNo == no).RowCount();
            }
        }
        //public int GetLikesCntByArticleNo(int no)
        //{
        //    using (ISession session = NHibernateHelper.OpenSession())
        //    {
        //        return session.QueryOver<LikesT>().Where(l => l.ArticleNo == no).RowCount();
        //    }
        //}

        public int SetLikes(LikesT likes)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    int result = 0;
                    LikesT chkLikes = session.QueryOver<LikesT>().Where(w => w.ArticleNo == likes.ArticleNo && w.MemberNo == likes.MemberNo).SingleOrDefault<LikesT>();
                    if (chkLikes != null)
                    {
                        session.Delete(chkLikes);
                    }
                    else
                    {
                        result = (Int32)session.Save(likes);
                    }
                    transaction.Commit();
                    session.Flush();
                    return result;
                }
            }
        }

        //public int GetLikesState(int articleNo, int memberNo)
        //{
        //    using (ISession session = NHibernateHelper.OpenSession())
        //    {
        //        int result = session.QueryOver<LikesT>().Where(w => w.ArticleNo == articleNo && w.MemberNo == memberNo).RowCount();
        //        return result;
        //    }
        //}
    }
}
