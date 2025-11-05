using ClearBank.DeveloperTest.Application.DTOs;
using ClearBank.DeveloperTest.Application.Interfaces;
using ClearBank.DeveloperTest.Domain.Entities;
using ClearBank.DeveloperTest.Domain.Enums;

namespace ClearBank.DeveloperTest.Application.Validators
{
    public class NullValidator : IAccountValidator
    {
        public PaymentScheme Scheme => default;

        public bool IsValid(MakePaymentRequest request, Account account) => false;
    }
}
