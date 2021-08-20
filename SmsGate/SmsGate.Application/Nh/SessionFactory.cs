using Commons.Nh.Contexts;
using Commons.Nh.Oracle;
using NHibernate;
using NHibernate.Cfg;

namespace SmsGate.Application.Nh
{
    public static class SessionFactory
    {
        public static ISessionFactory Instance { get; }

        static SessionFactory()
        {
            Instance = new SessionFactoryBuilder()
                       .UseUtcTimestamps()
                       .CurrentSessionContext<AsyncLocalSessionContext>()
                       .ExposeConfiguration(cfg => cfg.SetProperty(Environment.Hbm2ddlKeyWords, "none"))
                       .Build();
        }
    }
}
