using ClearBank.DeveloperTest.Application.DTOs;
using ClearBank.DeveloperTest.Application.Validators;
using ClearBank.DeveloperTest.Domain.Entities;
using ClearBank.DeveloperTest.Domain.Enums;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.ApplicationTests.ValidatorTests
{
    [TestFixture]
    public class NullValidatorTests
    {
        private NullValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new NullValidator();
        }

        [Test]
        public void Scheme_ShouldReturn_Default_PaymentScheme()
        {
            // Act
            var scheme = _validator.Scheme;

            // Assert
            Assert.That(scheme, Is.EqualTo(default(PaymentScheme)));
            Assert.That(scheme, Is.EqualTo((PaymentScheme)0));
        }

        [Test]
        public void IsValid_ShouldAlwaysReturnFalse_RegardlessOfInput()
        {
            // Arrange
            var request = new MakePaymentRequest { Amount = 100m };
            var account = new Account("accountNumber", 500m, AccountStatus.Live, AllowedPaymentSchemes.Bacs |
                                                                                    AllowedPaymentSchemes.FasterPayments);

            // Act
            var result = _validator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_WhenRequestIsNull_ShouldReturnFalse()
        {
            // Arrange
            MakePaymentRequest request = null;
            var account = new Account("accountNumber", 500m, AccountStatus.Live, AllowedPaymentSchemes.Bacs);

            // Act
            var result = _validator.IsValid(request!, account);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_WhenAccountIsNull_ShouldReturnFalse()
        {
            // Arrange
            var request = new MakePaymentRequest { Amount = 50m };

            // Act
            var result = _validator.IsValid(request, null);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_WhenBothRequestAndAccountAreNull_ShouldReturnFalse()
        {
            // Act
            var result = _validator.IsValid(null!, null!);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
