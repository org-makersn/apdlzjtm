using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Makersn.Models;
using NHibernate.Criterion.Lambda;

namespace Makersn.BizDac
{
    public class BannerDac
    {
        /// <summary>
        /// get all banners
        /// </summary>
        /// <returns></returns>
        public IList<BannerT> GetAllBannerLst(int type)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<BannerT> banners = session.QueryOver<BannerT>().Where(w => w.Type == type)
                                        .OrderBy(o => o.No).Desc
                                        .List();
                session.Flush();

                return banners;
            }
        }

        /// <summary>
        /// 배너 검색 type = 0 메인, 1 디자인, 2 프린팅
        /// </summary>
        /// <param name="sfl"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<BannerT> GetBannerLstByQuery(string sfl, string query, int type)
        {
            string[] queryLst = query.Split(' ');
            string searchQuery = @"select 
	                                    NO, TYPE, TITLE, PUBLISH_YN, OPENER_YN, LINK, SOURCE, TERM, IMAGE, PRIORITY, REG_DT, REG_ID, UPD_DT, UPD_ID   
                                    from BANNER with(nolock) 
                                    where 1=1 AND TYPE = :type ";
            if (sfl == "title")
            {
                searchQuery += " AND (";
                for (int i = 0; i < queryLst.Length; i++)
                {
                    //if (i == 0) { searchQuery += @" TITLE LIKE '%" + queryLst[i] + "' "; }
                    //else { searchQuery += " OR TITLE LIKE '%" + queryLst[i] + "' "; }
                    if (i == 0) { searchQuery += @" TITLE LIKE ? "; }
                    else { searchQuery += " OR TITLE LIKE ? "; }
                }
                searchQuery += ")";
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                //IList<BannerT> banners = session.QueryOver<BannerT>()
                //                        .OrderBy(o => o.No).Desc
                //                        .List();

                IQuery queryObj = session.CreateSQLQuery(searchQuery).AddEntity(typeof(BannerT));
                queryObj.SetParameter("type", type);
                for (int i = 0; i < queryLst.Length; i++)
                {
                    queryObj.SetParameter(i, "%" + queryLst[i] + "%");
                }
                IList<BannerT> banners = (IList<BannerT>)queryObj.List<BannerT>();
                session.Flush();

                return banners;
            }
        }

        /// <summary>
        /// get banner by no
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public BannerT GetBannerByNo(int bannerNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                BannerT bannerT = session.QueryOver<BannerT>().Where(b => b.No == bannerNo).SingleOrDefault<BannerT>();

                session.Flush();
                return bannerT;
            }
        }

        /// <summary>
        /// insert single banner
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        public int InsertBanner(BannerT banner)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                int bannerNo = (Int32)session.Save(banner);
                session.Flush();

                return bannerNo;
            }
        }

        /// <summary>
        /// update banner by id
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        public void UpdateBannerById(BannerT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        BannerT bannerT = session.QueryOver<BannerT>().Where(b => b.No == data.No).SingleOrDefault<BannerT>();
                        if (bannerT != null)
                        {
                            bannerT.Title = data.Title;
                            bannerT.PublishYn = data.PublishYn;
                            bannerT.OpenerYn = data.OpenerYn;
                            bannerT.Link = data.Link;
                            if (bannerT.Image != data.Image && data.Image != null)
                            {
                                bannerT.Image = data.Image;
                            }
                            bannerT.Priority = data.Priority;

                            session.SaveOrUpdate(bannerT);

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

        /// <summary>
        /// delete banner
        /// </summary>
        /// <param name="bannerNo"></param>
        public void DeleteBannerByNo(int bannerNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        BannerT bannerT = new BannerT();
                        bannerT.No = bannerNo;

                        session.Delete(bannerT);

                        transaction.Commit();
                        session.Flush();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public IList<BannerT> GetBannerInFront()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<BannerT> list = session.QueryOver<BannerT>().Where(w => w.PublishYn == "y" && w.Type == 1).OrderBy(o=>o.Priority).Desc.List<BannerT>();
                return list;
            }
        }
    }
}
