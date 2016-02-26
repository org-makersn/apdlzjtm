using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Caches.SysCache;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;

namespace Makersn.Models
{
    public class NHibernateHelper
    {
        private static ISessionFactory sessionFactory;

        private static readonly string _Server_IP = "makersndb_real";
        private static readonly string _UserNm = "luckymun";
        private static readonly string _Password = "pingoli1234";
        private static readonly string _Database = "dbluckymun";

        private static ISessionFactory GetSessionFactory<T>() where T : ICurrentSessionContext
        {
            if (sessionFactory == null)
            {
                sessionFactory = Fluently.Configure()
                            .Database(MsSqlConfiguration.MsSql2008
                        #if DEBUG
                            //.ShowSql()
                        #endif
                            .ConnectionString(c => c
                                        .Server(_Server_IP)
                                        .Database(_Database)
                                        .Username(_UserNm)
                                        .Password(_Password)))
                        .Mappings(m => m.FluentMappings
                            .AddFromAssemblyOf<ArticleT>()
                            .AddFromAssemblyOf<ArticleCommentT>()
                            .AddFromAssemblyOf<ArticleDetailT>()
                            .AddFromAssemblyOf<ArticleFileT>()
                            .AddFromAssemblyOf<BannerT>()
                            .AddFromAssemblyOf<CodeT>()
                            .AddFromAssemblyOf<ContactT>()
                            .AddFromAssemblyOf<DownloadT>()
                            .AddFromAssemblyOf<FollowerT>()
                            .AddFromAssemblyOf<LikesT>()
                            .AddFromAssemblyOf<MemberT>()
                            .AddFromAssemblyOf<MessageT>()
                            .AddFromAssemblyOf<NoticeT>()
                            .AddFromAssemblyOf<PopularT>()
                            .AddFromAssemblyOf<ReportT>()
                            .AddFromAssemblyOf<ChgPwT>())
                            .ExposeConfiguration(cfg => new SchemaExport(cfg)
                            .Create(false, false))
                        .Cache(c => c.ProviderClass<SysCacheProvider>().UseQueryCache())
                        .CurrentSessionContext<T>().BuildSessionFactory();
            }
            return sessionFactory;
        }

        /// <summary>
        /// Open the session
        /// </summary>
        /// <returns></returns>
        public static ISession OpenSession()
        {
            if (sessionFactory == null) 
                sessionFactory = HttpContext.Current != null ? GetSessionFactory<WebSessionContext>() : GetSessionFactory<ThreadStaticSessionContext>();

            //if (CurrentSessionContext.HasBind(sessionFactory))
            //    return sessionFactory.GetCurrentSession();

            ISession session = sessionFactory.OpenSession();
            //CurrentSessionContext.Bind(session);

            return session;
        }

        /// <summary>
        /// Closes the session.
        /// </summary>
        public static void CloseSession()
        {
            if (sessionFactory != null && CurrentSessionContext.HasBind(sessionFactory))
            {
                var session = CurrentSessionContext.Unbind(sessionFactory);
                //session.Flush();
                session.Close();
            }
        }


        /// <summary>
        /// Commits the session.
        /// </summary>
        /// <param name="session">The session.</param>
        public static void CommitSession(ISession session)
        {
            try
            {
                session.Transaction.Commit();
            }
            catch (Exception)
            {
                session.Transaction.Rollback();
                throw;
            }
        }

    //    public static void executeUpdate(String hql, String... params) {

    //    Session session = openSession();
    //    Transaction tx = null;

    //    try {
    //        tx = session.beginTransaction();
    //        Query query = session.createQuery(hql);

    //        if (params != null && params.length > 0) {
    //            for (int i = 0; i < params.length; i++) {
    //                query.setParameter(i, params[i]);
    //                // System.out.println("query influenced: "+params[i]);
    //            }
    //        }
    //        System.out.println("query influenced: " + query.getQueryString());
    //        int n = query.executeUpdate();
    //        System.out.println("query influence: " + n);
    //        tx.commit();
    //    } catch (Exception e) {
    //        if (tx != null)
    //            tx.rollback();
    //        throw new RuntimeException(e.getMessage());
    //    } finally {
    //        if (session != null && session.isOpen()) {
    //            session.close();
    //        }
    //    }
    //}
    }
}