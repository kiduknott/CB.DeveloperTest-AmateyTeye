using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class AccountService : IAccountService
    {
        private IAccountDataStore accountDataStore;
        
        public AccountService(IAccountDataStore store)
        {
            accountDataStore = store;
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
