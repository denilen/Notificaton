using Commons.DataAccess;
using SmsGate.Tests.Configuration;

namespace SmsGate.Tests.Fixtures
{
    public class DatabaseConfigurationFixture
    {
        static DatabaseConfigurationFixture()
        {
            ConnectionStringsManager.ReadFromConfiguration(ConfigurationFacade.Configuration);
        }
    }
}
