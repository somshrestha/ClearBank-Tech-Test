using ClearBank.DeveloperTest.Application.DTOs;
using ClearBank.DeveloperTest.Application.Validators;
using ClearBank.DeveloperTest.Domain.Entities;
using ClearBank.DeveloperTest.Domain.Enums;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.ValidatorTests
{
    [TestFixture]
    public class BacsValidatorTests
    {
        private BacsValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new BacsValidator();
        }

        [Test]
        public void Scheme_ShouldReturn_Bacs()
        {
            // Act
            var scheme = _validator.Scheme;

            // Assert
            Assert.That(scheme, Is.EqualTo(PaymentScheme.Bacs));
        }

        [Test]
        public void IsValid_WhenAccountIsNull_ShouldReturnFalse()
        {
            // Arrange
            MakePaymentRequest request = new MakePaymentRequest();
            Account account = null;

            // Act
            var result = _validator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_WhenAccountDoesNotSupportBacs_ShouldReturnFalse()
        {
            // Arrange
            var request = new MakePaymentRequest();
            var account = new Account("accountNumber", 1000m, AccountStatus.Live, AllowedPaymentSchemes.FasterPayments);

            // Act
            var result = _validator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_WhenAccountSupportsBacs_ShouldReturnTrue()
        {
            // Arrange
            var request = new MakePaymentRequest();
            var account = new Account("accountNumber", 1000m, AccountStatus.Live, AllowedPaymentSchemes.Bacs);

            // Act
            var result = _validator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
