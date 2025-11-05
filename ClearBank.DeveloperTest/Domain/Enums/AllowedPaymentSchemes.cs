using System;

namespace ClearBank.DeveloperTest.Domain.Enums
{
    [Flags]
    public enum AllowedPaymentSchemes
    {
        None = 0,
        FasterPayments = 1 << 0,
        Bacs = 1 << 1,
        Chaps = 1 << 2
    }
}
