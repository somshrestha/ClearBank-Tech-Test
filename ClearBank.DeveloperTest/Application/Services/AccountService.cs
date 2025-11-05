using ClearBank.DeveloperTest.Application.Interfaces;
using ClearBank.DeveloperTest.Domain.Entities;
using ClearBank.DeveloperTest.Infrastructure.Interfaces;

namespace ClearBank.DeveloperTest.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountDataStore _accountDataStore;

        public AccountService(IAccountDataStoreFactory accountDataStoreFactory)
        {
            _accountDataStore = accountDataStoreFactory.GetInstance();
        }

        public Account GetAccount(string accountNumber)
            => _accountDataStore.GetAccount(accountNumber);

        public void UpdateAccount(Account account)
            => _accountDataStore.UpdateAccount(account);
    }
}
