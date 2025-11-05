using ClearBank.DeveloperTest.Domain.Enums;
using System;

namespace ClearBank.DeveloperTest.Domain.Entities
{
    public class Account
    {
        public string AccountNumber { get; private set; }
        public decimal Balance { get; private set; }
        public AccountStatus Status { get; private set; }
        public AllowedPaymentSchemes AllowedPaymentSchemes { get; private set; }

        public Account(
            string accountNumber,
            decimal balance,
            AccountStatus status,
            AllowedPaymentSchemes allowedPaymentSchemes
            )
        {
            AccountNumber = accountNumber;
            Balance = balance;
            Status = status;
            AllowedPaymentSchemes = allowedPaymentSchemes;
        }

        public void Debit(decimal amount)
        {
            if (amount <= 0) 
                throw new InvalidOperationException("Amount must be positive.");
            if (Balance < amount) 
                throw new InvalidOperationException("Insufficient funds.");

            Balance -= amount;
        }

        public bool SupportsScheme(AllowedPaymentSchemes scheme)
            => AllowedPaymentSchemes.HasFlag(scheme);
    }
}
