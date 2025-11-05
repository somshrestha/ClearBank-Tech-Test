using ClearBank.DeveloperTest.Application.DTOs;
using ClearBank.DeveloperTest.Domain.Entities;
using ClearBank.DeveloperTest.Domain.Enums;

namespace ClearBank.DeveloperTest.Application.Interfaces
{
    public interface IAccountValidator
    {
        PaymentScheme Scheme { get; }
        bool IsValid(MakePaymentRequest request, Account account);
    }
}
