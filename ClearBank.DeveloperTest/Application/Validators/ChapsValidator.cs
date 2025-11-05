using ClearBank.DeveloperTest.Application.DTOs;
using ClearBank.DeveloperTest.Application.Interfaces;
using ClearBank.DeveloperTest.Domain.Entities;
using ClearBank.DeveloperTest.Domain.Enums;

namespace ClearBank.DeveloperTest.Application.Validators
{
    public class ChapsValidator : IAccountValidator
    {
        public PaymentScheme Scheme => PaymentScheme.Chaps;

        public bool IsValid(MakePaymentRequest request, Account account)
            => account != null &&
                account.SupportsScheme(AllowedPaymentSchemes.Chaps) &&
                account.Status == AccountStatus.Live;
    }
}
