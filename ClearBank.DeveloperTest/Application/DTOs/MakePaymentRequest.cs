using ClearBank.DeveloperTest.Domain.Enums;
using System;

namespace ClearBank.DeveloperTest.Application.DTOs
{
    public class MakePaymentRequest
    {
        public string CreditorAccountNumber { get; set; } = string.Empty;

        public string DebtorAccountNumber { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public PaymentScheme PaymentScheme { get; set; }
    }
}
