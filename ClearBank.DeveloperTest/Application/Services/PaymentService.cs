using ClearBank.DeveloperTest.Application.DTOs;
using ClearBank.DeveloperTest.Application.Interfaces;
using ClearBank.DeveloperTest.Application.Validators;
using System.Collections.Generic;
using System.Linq;

namespace ClearBank.DeveloperTest.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountService _accountService;
        private readonly IEnumerable<IAccountValidator> _validators;

        public PaymentService(
                                IAccountService accountService,
                                IEnumerable<IAccountValidator> validators)
        {
            _accountService = accountService;
            _validators = validators;
        }
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = _accountService.GetAccount(request.DebtorAccountNumber);
            var validator = _validators.FirstOrDefault(v => v.Scheme == request.PaymentScheme) ?? new NullValidator();

            if (!validator.IsValid(request, account))
                return new MakePaymentResult { Success = false };

            account.Debit(request.Amount);
            _accountService.UpdateAccount(account);

            return new MakePaymentResult { Success = true };
        }
    }
}
