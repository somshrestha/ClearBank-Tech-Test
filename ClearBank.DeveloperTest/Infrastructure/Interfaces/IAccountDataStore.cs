using ClearBank.DeveloperTest.Domain.Entities;

namespace ClearBank.DeveloperTest.Infrastructure.Interfaces
{
    public interface IAccountDataStore
    {
        Account GetAccount(string accountNumber);
        void UpdateAccount(Account account);
    }
}
