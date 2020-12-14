using System.Configuration;

namespace ClearBank.DeveloperTest.Configuration
{
    public class ConfigManager : IConfigManager
    {
        public string GetDataStoreType()
        {
            return ConfigurationManager.AppSettings["DataStoreType"];
        }
    }
}
