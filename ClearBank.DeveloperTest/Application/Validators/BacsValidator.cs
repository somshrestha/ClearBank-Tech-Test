using ClearBank.DeveloperTest.Application.DTOs;
using ClearBank.DeveloperTest.Application.Interfaces;
using ClearBank.DeveloperTest.Domain.Entities;
using ClearBank.DeveloperTest.Domain.Enums;

namespace ClearBank.DeveloperTest.Application.Validators
{
    public class BacsValidator : IAccountValidator
    {
        public PaymentScheme Scheme => PaymentScheme.Bacs;

        public bool IsValid(MakePaymentRequest request, Account account)
            => account != null && account.SupportsScheme(AllowedPaymentSchemes.Bacs);
    }
}
