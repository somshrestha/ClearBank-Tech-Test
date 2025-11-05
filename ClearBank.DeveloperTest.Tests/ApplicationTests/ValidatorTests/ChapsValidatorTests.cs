using ClearBank.DeveloperTest.Application.DTOs;
using ClearBank.DeveloperTest.Application.Validators;
using ClearBank.DeveloperTest.Domain.Entities;
using ClearBank.DeveloperTest.Domain.Enums;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.ApplicationTests.ValidatorTests
{
    [TestFixture]
    public class ChapsValidatorTests
    {
        private ChapsValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new ChapsValidator();
        }

        [Test]
        public void Scheme_ShouldReturn_Chaps()
        {
            // Act
            var scheme = _validator.Scheme;

            // Assert
            Assert.That(scheme, Is.EqualTo(PaymentScheme.Chaps));
        }

        [Test]
        public void IsValid_WhenAccountIsNull_ShouldReturnFalse()
        {
            // Arrange
            var request = new MakePaymentRequest();

            // Act
            var result = _validator.IsValid(request, null);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_WhenAccountDoesNotSupportChaps_ShouldReturnFalse()
        {
            // Arrange
            var request = new MakePaymentRequest();
            var account = new Account("accountNumber", 1000m, AccountStatus.Live, AllowedPaymentSchemes.Bacs);

            // Act
            var result = _validator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_WhenAccountStatusIsNotLive_ShouldReturnFalse()
        {
            // Arrange
            var request = new MakePaymentRequest();
            var account = new Account("accountNumber", 1000m, AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Chaps);

            // Act
            var result = _validator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_WhenAccountIsDisabled_ShouldReturnFalse()
        {
            // Arrange
            var request = new MakePaymentRequest();
            var account = new Account("accountNumber", 1000m, AccountStatus.Disabled, AllowedPaymentSchemes.Chaps);

            // Act
            var result = _validator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_WhenAccountSupportsChapsAndIsLive_ShouldReturnTrue()
        {
            // Arrange
            var request = new MakePaymentRequest();
            var account = new Account("accountNumber", 1000m, AccountStatus.Live, AllowedPaymentSchemes.Chaps);

            // Act
            var result = _validator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValid_WhenAccountSupportsMultipleSchemesIncludingChapsAndIsLive_ShouldReturnTrue()
        {
            // Arrange
            var request = new MakePaymentRequest();
            var account = new Account("accountNumber", 1000m, AccountStatus.Live, AllowedPaymentSchemes.Chaps |
                                                                                    AllowedPaymentSchemes.Bacs |
                                                                                    AllowedPaymentSchemes.FasterPayments);

            // Act
            var result = _validator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
