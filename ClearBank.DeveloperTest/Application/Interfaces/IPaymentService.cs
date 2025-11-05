using ClearBank.DeveloperTest.Application.DTOs;

namespace ClearBank.DeveloperTest.Application.Interfaces
{
    public interface IPaymentService
    {
        MakePaymentResult MakePayment(MakePaymentRequest request);
    }
}
