using Commons.Nh.Oracle;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;

namespace SmsGate.Tests.Fixtures
{
    public class NhSessionFactoryFixture
    {
        public ISessionFactory Instance { get; }

        static NhSessionFactoryFixture()
        {
#if NH_PROF
            NHibernateProfiler.Initialize();
#endif
        }

        public NhSessionFactoryFixture()
        {
            Instance = new SessionFactoryBuilder()
                       .UseUtcTimestamps()
                       .CurrentSessionContext<AsyncLocalSessionContext>()
                       .ExposeConfiguration(cfg => cfg.SetProperty(Environment.Hbm2ddlKeyWords, "none"))
                       .Build();
        }
    }
}
