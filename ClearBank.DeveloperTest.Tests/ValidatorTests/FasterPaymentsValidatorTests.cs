using ClearBank.DeveloperTest.Application.DTOs;
using ClearBank.DeveloperTest.Application.Validators;
using ClearBank.DeveloperTest.Domain.Entities;
using ClearBank.DeveloperTest.Domain.Enums;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.ValidatorTests
{
    [TestFixture]
    public class FasterPaymentsValidatorTests
    {
        private FasterPaymentsValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new FasterPaymentsValidator();
        }

        [Test]
        public void Scheme_ShouldReturn_FasterPayments()
        {
            // Act
            var scheme = _validator.Scheme;

            // Assert
            Assert.That(scheme, Is.EqualTo(PaymentScheme.FasterPayments));
        }

        [Test]
        public void IsValid_WhenAccountIsNull_ShouldReturnFalse()
        {
            // Arrange
            var request = new MakePaymentRequest { Amount = 100m };

            // Act
            var result = _validator.IsValid(request, null);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_WhenAccountDoesNotSupportFasterPayments_ShouldReturnFalse()
        {
            // Arrange
            var request = new MakePaymentRequest { Amount = 100m };
            var account = new Account("accountNumber", 200m, AccountStatus.Live, AllowedPaymentSchemes.Bacs);

            // Act
            var result = _validator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_WhenBalanceIsLessThanRequestAmount_ShouldReturnFalse()
        {
            // Arrange
            var request = new MakePaymentRequest { Amount = 200m };
            var account = new Account("accountNumber", 150m, AccountStatus.Live, AllowedPaymentSchemes.FasterPayments);

            // Act
            var result = _validator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_WhenAmountIsZero_ShouldReturnTrue_IfSupported()
        {
            // Arrange
            var request = new MakePaymentRequest { Amount = 0m };
            var account = new Account("accountNumber", 100m, AccountStatus.Live, AllowedPaymentSchemes.FasterPayments);

            // Act
            var result = _validator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValid_WhenAmountIsNegative_ShouldReturnTrue_IfSupported()
        {
            // Arrange
            var request = new MakePaymentRequest { Amount = -50m };
            var account = new Account("accountNumber", 100m, AccountStatus.Live, AllowedPaymentSchemes.FasterPayments);

            // Act
            var result = _validator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValid_WhenAccountSupportsFasterPaymentsAndHasSufficientBalance_ShouldReturnTrue()
        {
            // Arrange
            var request = new MakePaymentRequest { Amount = 100m };
            var account = new Account("accountNumber", 150m, AccountStatus.Live, AllowedPaymentSchemes.FasterPayments);

            // Act
            var result = _validator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValid_WhenAccountSupportsMultipleSchemesIncludingFasterPaymentsAndHasEnoughBalance_ShouldReturnTrue()
        {
            // Arrange
            var request = new MakePaymentRequest { Amount = 75m };
            var account = new Account("accountNumber", 100m, AccountStatus.Live, AllowedPaymentSchemes.Chaps |
                                                                                    AllowedPaymentSchemes.Bacs |
                                                                                    AllowedPaymentSchemes.FasterPayments);

            // Act
            var result = _validator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValid_WhenBalanceEqualsRequestAmount_ShouldReturnTrue()
        {
            // Arrange
            var request = new MakePaymentRequest { Amount = 100m };
            var account = new Account("accountNumber", 100m, AccountStatus.Live, AllowedPaymentSchemes.FasterPayments);

            // Act
            var result = _validator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
