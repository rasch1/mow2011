using System.Data;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace ContactListWeb.Infrastructure.NHibernate
{
    public class SessionManager
    {
        private static readonly SessionManager sessionManager = new SessionManager();

        private static ISessionFactory _sessionFactory;

        private SessionManager()
        {
            InitSessionFactory();
        }

        public static SessionManager Instance
        {
            get { return sessionManager; }
        }

        public ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }

        private static void InitSessionFactory()
        {
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

            _sessionFactory = Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2008
                    .ConnectionString(c => c.FromConnectionStringWithKey("DbConnection"))
                    .AdoNetBatchSize(100)
                    .IsolationLevel(IsolationLevel.Snapshot)
                    )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SessionManager>())
                //.ExposeConfiguration((configuration => new SchemaExport(configuration).Create(false, true)))
                .CurrentSessionContext(typeof(LazySessionContext).AssemblyQualifiedName)
                .BuildSessionFactory();
        }
    }
}