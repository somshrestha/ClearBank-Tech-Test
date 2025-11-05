using ClearBank.DeveloperTest.Application.DTOs;
using ClearBank.DeveloperTest.Application.Interfaces;
using ClearBank.DeveloperTest.Application.Services;
using ClearBank.DeveloperTest.Domain.Entities;
using ClearBank.DeveloperTest.Domain.Enums;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ClearBank.DeveloperTest.Tests.ApplicationTests.ServiceTests
{
    [TestFixture]
    public class PaymentServiceTests
    {
        private Mock<IAccountService> _accountServiceMock;
        private Mock<IEnumerable<IAccountValidator>> _validatorsMock;
        private PaymentService _paymentService;

        [SetUp]
        public void Setup()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _validatorsMock = new Mock<IEnumerable<IAccountValidator>>();
            _paymentService = new PaymentService(
                _accountServiceMock.Object,
                _validatorsMock.Object
            );
        }

        [Test]
        public void MakePayment_WhenValidatorReturnsTrue_ShouldDebitAndUpdateAccount_AndReturnSuccess()
        {
            // Arrange
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "ACC123",
                Amount = 100m,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var account = new Account("ACC123", 200m, AccountStatus.Live, AllowedPaymentSchemes.FasterPayments);

            var validatorMock = new Mock<IAccountValidator>();
            validatorMock.Setup(v => v.Scheme).Returns(PaymentScheme.FasterPayments);
            validatorMock.Setup(v => v.IsValid(request, account)).Returns(true);

            _validatorsMock.Setup(v => v.GetEnumerator())
                .Returns(new List<IAccountValidator> { validatorMock.Object }.GetEnumerator());

            _accountServiceMock.Setup(a => a.GetAccount(request.DebtorAccountNumber)).Returns(account);
            _accountServiceMock.Setup(a => a.UpdateAccount(It.IsAny<Account>()))
                .Callback<Account>(acc => Assert.That(acc.Balance, Is.EqualTo(100m)));

            // Act
            var result = _paymentService.MakePayment(request);

            // Assert
            Assert.That(result.Success, Is.True);
            _accountServiceMock.Verify(a => a.UpdateAccount(It.Is<Account>(acc => acc.Balance == 100m)), Times.Once);
        }

        [Test]
        public void MakePayment_WhenValidatorReturnsFalse_ShouldReturnFailure_WithoutUpdating()
        {
            // Arrange
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "ACC123",
                Amount = 100m,
                PaymentScheme = PaymentScheme.Bacs
            };

            var account = new Account("ACC123", 200m, AccountStatus.Live, AllowedPaymentSchemes.Bacs);

            var validatorMock = new Mock<IAccountValidator>();
            validatorMock.Setup(v => v.Scheme).Returns(PaymentScheme.Bacs);
            validatorMock.Setup(v => v.IsValid(request, account)).Returns(false);

            _validatorsMock.Setup(v => v.GetEnumerator())
                .Returns(new List<IAccountValidator> { validatorMock.Object }.GetEnumerator());

            _accountServiceMock.Setup(a => a.GetAccount(request.DebtorAccountNumber)).Returns(account);

            // Act
            var result = _paymentService.MakePayment(request);

            // Assert
            Assert.That(result.Success, Is.False);
            _accountServiceMock.Verify(a => a.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void MakePayment_WhenNoValidatorMatchesScheme_ShouldUseNullValidator_AndReturnFailure()
        {
            // Arrange
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "ACC123",
                Amount = 100m,
                PaymentScheme = PaymentScheme.Chaps
            };

            var account = new Account("ACC123", 200m, AccountStatus.Live, AllowedPaymentSchemes.Chaps |
                                                                            AllowedPaymentSchemes.Bacs |
                                                                            AllowedPaymentSchemes.FasterPayments);

            // No validator has Chaps → NullValidator used
            var bacsValidatorMock = new Mock<IAccountValidator>();
            bacsValidatorMock.Setup(v => v.Scheme).Returns(PaymentScheme.Bacs);

            _validatorsMock.Setup(v => v.GetEnumerator())
                .Returns(new List<IAccountValidator> { bacsValidatorMock.Object }.GetEnumerator());

            _accountServiceMock.Setup(a => a.GetAccount(request.DebtorAccountNumber)).Returns(account);

            // Act
            var result = _paymentService.MakePayment(request);

            // Assert
            Assert.That(result.Success, Is.False);
            _accountServiceMock.Verify(a => a.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void MakePayment_WhenDebitThrows_ShouldPropagateException()
        {
            // Arrange
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "ACC123",
                Amount = -50m, // Invalid amount
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var account = new Account("ACC123", 100m, AccountStatus.Live, AllowedPaymentSchemes.FasterPayments);

            var validatorMock = new Mock<IAccountValidator>();
            validatorMock.Setup(v => v.Scheme).Returns(PaymentScheme.FasterPayments);
            validatorMock.Setup(v => v.IsValid(request, account)).Returns(true);

            _validatorsMock.Setup(v => v.GetEnumerator())
                .Returns(new List<IAccountValidator> { validatorMock.Object }.GetEnumerator());

            _accountServiceMock.Setup(a => a.GetAccount(request.DebtorAccountNumber)).Returns(account);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                _paymentService.MakePayment(request)
            );

            _accountServiceMock.Verify(a => a.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void MakePayment_WithMultipleValidators_ShouldSelectCorrectOneByScheme()
        {
            // Arrange
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "ACC123",
                Amount = 100m,
                PaymentScheme = PaymentScheme.Chaps
            };

            var account = new Account("ACC123", 500m, AccountStatus.Live, AllowedPaymentSchemes.Chaps);

            var bacsValidator = new Mock<IAccountValidator>();
            bacsValidator.Setup(v => v.Scheme).Returns(PaymentScheme.Bacs);
            bacsValidator.Setup(v => v.IsValid(It.IsAny<MakePaymentRequest>(), It.IsAny<Account>())).Returns(false);

            var chapsValidator = new Mock<IAccountValidator>();
            chapsValidator.Setup(v => v.Scheme).Returns(PaymentScheme.Chaps);
            chapsValidator.Setup(v => v.IsValid(request, account)).Returns(true);

            var validators = new List<IAccountValidator>
            {
                bacsValidator.Object,
                chapsValidator.Object
            };

            _validatorsMock.Setup(v => v.GetEnumerator()).Returns(validators.GetEnumerator());
            _accountServiceMock.Setup(a => a.GetAccount(request.DebtorAccountNumber)).Returns(account);

            // Act
            var result = _paymentService.MakePayment(request);

            // Assert
            Assert.That(result.Success, Is.True);
            chapsValidator.Verify(v => v.IsValid(request, account), Times.Once);
            bacsValidator.Verify(v => v.IsValid(It.IsAny<MakePaymentRequest>(), It.IsAny<Account>()), Times.Never);
        }
    }
}
