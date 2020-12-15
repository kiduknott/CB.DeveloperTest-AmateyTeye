using ClearBank.DeveloperTest.Data;

namespace ClearBank.DeveloperTest.ServiceFactories
{
    public interface IServiceFactory
    {
        public IAccountDataStore GetAccountDataStore();
    }
}
