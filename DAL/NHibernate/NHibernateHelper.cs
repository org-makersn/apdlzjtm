using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Makersn.Models;
using NHibernate;
using NHibernate.Caches.SysCache;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Configuration;
using System.Web;


namespace Makersn
{
    public class NHibernateHelper
    {
        private static readonly DbSettings instance = new DbSettings();
        private static DbSettings Instance { get { return instance; } }

        private static ISessionFactory sessionFactory;
        
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
                                        .Server(instance._Server_IP)
                                        .Database(instance._Database)
                                        .Username(instance._UserNm)
                                        .Password(instance._Password)))
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

        public class DbSettings
        {
            public DbSettings()
            {
                _Server_IP = ConfigurationManager.AppSettings["DBServerIP"] ?? "storedb_dev";
                _UserNm = ConfigurationManager.AppSettings["DBUser"] ?? "0000";
                _Password = ConfigurationManager.AppSettings["DBPwd"] ?? "0000";
                _Database = ConfigurationManager.AppSettings["Database"] ?? "0000";
            }

            /// <summary>
            /// 
            /// </summary>
            public string _Server_IP { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public string _UserNm { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public string _Password { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public string _Database { get; private set; }
        }
    }
}