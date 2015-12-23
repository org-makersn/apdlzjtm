using Makersn.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.BizDac
{
    public class FollowerDac
    {
        public int GetFollwerCnt(int memberNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<FollowerT>().Where(w => w.MemberNoRef == memberNo).RowCount();
            }
        }

        public int GetFollowingCnt(int memberNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<FollowerT>().Where(w => w.MemberNo == memberNo).RowCount();
            }
        }

        public IList<FollowerT> GetFollowingList(int memberNo, int visitorNo)
        {
            string query = @"SELECT 
	                            (SELECT COUNT(0) FROM ARTICLE A WHERE A.MEMBER_NO = F.MEMBER_NO_REF AND VISIBILITY='Y') AS DESIGN_CNT, 
	                            (SELECT COUNT(0) FROM LIKES L WHERE L.MEMBER_NO = F.MEMBER_NO_REF) AS LIKES_CNT,
	                            (SELECT COUNT(0) FROM FOLLOWER F2 INNER JOIN
	                                MEMBER M2 WITH(NOLOCK) ON F2.MEMBER_NO = M2.NO
	                                AND M2.DEL_FLAG='N' WHERE F2.MEMBER_NO_REF = F.MEMBER_NO_REF) AS FOLLOWER_CNT,
	                            M.NAME,
	                            M.PROFILE_PIC,
	                            M.NO,
	                            M.BLOG_URL,
                                (SELECT COUNT(0) FROM FOLLOWER F3 WHERE F3.MEMBER_NO = :visitorNo AND F3.MEMBER_NO_REF = F.MEMBER_NO_REF) AS CHK_FOLLOW
	
                            FROM FOLLOWER F INNER JOIN MEMBER M
				                            ON F.MEMBER_NO_REF = M.NO
                                            AND M.DEL_FLAG='N'

                            WHERE F.MEMBER_NO= :memberNo
                            GROUP BY F.MEMBER_NO_REF, M.NAME, M.PROFILE_PIC, M.NO, M.BLOG_URL";
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("visitorNo", visitorNo);
                queryObj.SetParameter("memberNo", memberNo);

                IList<object[]> results = queryObj.List<object[]>();


                IList<FollowerT> list = new List<FollowerT>();
                foreach (object[] row in results)
                {
                    FollowerT follower = new FollowerT();
                    follower.DesignCnt = (int)row[0];
                    follower.LikesCnt = (int)row[1];
                    follower.FollowerCnt = (int)row[2];
                    follower.MemberName = (string)row[3];
                    follower.ProfilePic = (string)row[4];
                    follower.MemberNo = (int)row[5];
                    follower.MemberBlog = (string)row[6];
                    follower.ChkFollow = (int)row[7];

                    if ((int)row[5] == visitorNo) { follower.ChkFollow = 2; };

                    list.Add(follower);
                }

                return list;
            }
        }

        public IList<FollowerT> GetFollowerLIst(int memberNo, int visitorNo)
        {
            string query = @"SELECT 
            	                 (SELECT COUNT(0) FROM ARTICLE A WHERE A.MEMBER_NO = F.MEMBER_NO AND VISIBILITY='Y') AS DESIGN_CNT, 
            	                 (SELECT COUNT(0) FROM LIKES L WHERE L.MEMBER_NO = F.MEMBER_NO) AS LIKES_CNT,
            	                 (SELECT COUNT(0) FROM FOLLOWER F2 INNER JOIN
            	                     MEMBER M2 WITH(NOLOCK) ON F2.MEMBER_NO = M2.NO
            	                     AND M2.DEL_FLAG='N' WHERE F2.MEMBER_NO_REF = F.MEMBER_NO) AS FOLLOWER_CNT,
            	                 M.NAME,
            	                 M.PROFILE_PIC,
            	                 M.NO,
            	                 M.BLOG_URL,
                                 (SELECT COUNT(0) FROM FOLLOWER F3 WHERE F3.MEMBER_NO = :visitorNo AND F3.MEMBER_NO_REF = F.MEMBER_NO) AS CHK_FOLLOW
            	             
                             FROM FOLLOWER F INNER JOIN MEMBER M
            			                     ON F.MEMBER_NO = M.NO
                                             AND M.DEL_FLAG='N'
                             
                             WHERE F.MEMBER_NO_REF= :memberNo
                             GROUP BY F.MEMBER_NO, M.NAME, M.PROFILE_PIC,M.NO, M.BLOG_URL";
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("visitorNo", visitorNo);
                queryObj.SetParameter("memberNo", memberNo);

                IList<object[]> results = queryObj.List<object[]>();

                IList<FollowerT> list = new List<FollowerT>();
                foreach (object[] row in results)
                {
                    FollowerT follower = new FollowerT();
                    follower.DesignCnt = (int)row[0];
                    follower.LikesCnt = (int)row[1];
                    follower.FollowerCnt = (int)row[2];
                    follower.MemberName = (string)row[3];
                    follower.ProfilePic = (string)row[4];
                    follower.MemberNo = (int)row[5];
                    follower.MemberBlog = (string)row[6];
                    follower.ChkFollow = (int)row[7];

                    if ((int)row[5] == visitorNo) { follower.ChkFollow = 2; };

                    list.Add(follower);
                }

                return list;
            }
        }
        public int SetFollow(FollowerT follow)
        {
            int result = 0;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    FollowerT chkFollow = session.QueryOver<FollowerT>().Where(w => (w.MemberNo == follow.MemberNo && w.MemberNoRef == follow.MemberNoRef)).Take(1).SingleOrDefault<FollowerT>();
                    if (chkFollow == null)
                    {
                        session.Save(follow);
                        result = 1;
                    }
                    else
                    {
                        session.Delete(chkFollow);
                    }

                    transaction.Commit();
                    session.Flush();
                }
            }
            return result;
        }

        public int CheckFollow(int memberNo, int visitorNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<FollowerT>().Where(w => w.MemberNoRef == memberNo && w.MemberNo == visitorNo).RowCount();
            }
        }

    }
}
