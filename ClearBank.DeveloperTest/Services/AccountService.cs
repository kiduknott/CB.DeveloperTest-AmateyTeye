using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.ServiceFactories;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountDataStore accountDataStore;

        public AccountService(IServiceFactory serviceFactory)
        {
            accountDataStore = serviceFactory.GetAccountDataStore();
        }

        public Account GetAccount(string accountNumber)
        {
            return accountDataStore.GetAccount(accountNumber);
        }

        public void UpdateAccount(Account account)
        {
            accountDataStore.UpdateAccount(account);
        }
    }
}
