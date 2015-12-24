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

        private static readonly string _Server_IP = "192.168.219.120";
        private static readonly string _UserNm = "makers";
        private static readonly string _Password = "1234";
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
    }
}