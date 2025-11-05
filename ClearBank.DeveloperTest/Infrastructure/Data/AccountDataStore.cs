using ClearBank.DeveloperTest.Domain.Entities;
using ClearBank.DeveloperTest.Domain.Enums;
using ClearBank.DeveloperTest.Infrastructure.Interfaces;

namespace ClearBank.DeveloperTest.Infrastructure.Data
{
    public class AccountDataStore : IAccountDataStore
    {
        public Account GetAccount(string accountNumber)
        {
            // Access database to retrieve account, code removed for brevity 
            return new Account(accountNumber, 1000m, AccountStatus.Live, AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps);
        }

        public void UpdateAccount(Account account)
        {
            // Update account in database, code removed for brevity
        }
    }
}
