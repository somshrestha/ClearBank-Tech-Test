using ClearBank.DeveloperTest.Application.DTOs;
using ClearBank.DeveloperTest.Application.Interfaces;
using ClearBank.DeveloperTest.Domain.Entities;
using ClearBank.DeveloperTest.Domain.Enums;

namespace ClearBank.DeveloperTest.Application.Validators
{
    public class FasterPaymentsValidator : IAccountValidator
    {
        public PaymentScheme Scheme => PaymentScheme.FasterPayments;

        public bool IsValid(MakePaymentRequest request, Account account)
            => account != null &&
                account.SupportsScheme(AllowedPaymentSchemes.FasterPayments) &&
                account.Balance >= request.Amount;

    }
}
