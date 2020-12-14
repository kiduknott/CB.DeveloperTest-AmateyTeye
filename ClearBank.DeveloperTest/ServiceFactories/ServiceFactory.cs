using ClearBank.DeveloperTest.Configuration;
using ClearBank.DeveloperTest.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ClearBank.DeveloperTest.ServiceFactories
{
    public class ServiceFactory
    {
        private readonly ServiceProvider serviceProvider;

        public ServiceFactory(IConfigManager configManager)
        {
            if (configManager.GetDataStoreType() == "Backup")
            {
                serviceProvider = new ServiceCollection()
                    .AddSingleton<IAccountDataStore, BackupAccountDataStore>()
                    .BuildServiceProvider();
            }
            else
            {
                serviceProvider = new ServiceCollection()
                    .AddSingleton<IAccountDataStore, AccountDataStore>()
                    .BuildServiceProvider();
            }
        }

        public IAccountDataStore GetAccountDataStore()
        {
            return serviceProvider.GetService<IAccountDataStore>();
        }
    }
}
