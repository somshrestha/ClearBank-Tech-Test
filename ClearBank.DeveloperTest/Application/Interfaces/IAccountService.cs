using ClearBank.DeveloperTest.Domain.Entities;

namespace ClearBank.DeveloperTest.Application.Interfaces
{
    public interface IAccountService
    {
        Account GetAccount(string accountNumber);
        void UpdateAccount(Account account);
    }
}
