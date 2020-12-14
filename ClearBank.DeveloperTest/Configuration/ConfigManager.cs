using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

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
