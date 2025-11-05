using ClearBank.DeveloperTest.Domain.Entities;
using ClearBank.DeveloperTest.Domain.Enums;
using NUnit.Framework;
using System;

namespace ClearBank.DeveloperTest.Tests.DomainTests.EntityTests
{
    [TestFixture]
    public class AccountTests
    {
        private const string ValidAccountNumber = "ACC123456";
        private const decimal InitialBalance = 500m;

        [Test]
        public void Constructor_ShouldSetAllPropertiesCorrectly()
        {
            // Arrange
            var accountNumber = "ACC789";
            var balance = 1000m;
            var status = AccountStatus.Live;
            var schemes = AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.FasterPayments;

            // Act
            var account = new Account(accountNumber, balance, status, schemes);

            // Assert
            Assert.That(account.AccountNumber, Is.EqualTo(accountNumber));
            Assert.That(account.Balance, Is.EqualTo(balance));
            Assert.That(account.Status, Is.EqualTo(status));
            Assert.That(account.AllowedPaymentSchemes, Is.EqualTo(schemes));
        }

        [Test]
        public void Constructor_WithZeroBalance_ShouldAllow()
        {
            // Act
            var account = new Account(ValidAccountNumber, 0m, AccountStatus.Live, AllowedPaymentSchemes.Bacs);

            // Assert
            Assert.That(account.Balance, Is.EqualTo(0m));
        }

        [Test]
        public void Constructor_WithNegativeBalance_ShouldAllow()
        {
            // Act
            var account = new Account(ValidAccountNumber, -50m, AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Chaps);

            // Assert
            Assert.That(account.Balance, Is.EqualTo(-50m));
        }

        [Test]
        public void Debit_WhenSufficientFunds_ShouldReduceBalance()
        {
            // Arrange
            var account = new Account(ValidAccountNumber, InitialBalance, AccountStatus.Live, AllowedPaymentSchemes.Bacs |
                                                                                              AllowedPaymentSchemes.Chaps |
                                                                                              AllowedPaymentSchemes.FasterPayments);
            var debitAmount = 200m;

            // Act
            account.Debit(debitAmount);

            // Assert
            Assert.That(account.Balance, Is.EqualTo(InitialBalance - debitAmount));
        }

        [Test]
        public void Debit_WhenExactBalance_ShouldSetBalanceToZero()
        {
            // Arrange
            var account = new Account(ValidAccountNumber, 300m, AccountStatus.Live, AllowedPaymentSchemes.Bacs |
                                                                                    AllowedPaymentSchemes.Chaps |
                                                                                    AllowedPaymentSchemes.FasterPayments);

            // Act
            account.Debit(300m);

            // Assert
            Assert.That(account.Balance, Is.EqualTo(0m));
        }

        [Test]
        public void Debit_MultipleTimes_ShouldAccumulateCorrectly()
        {
            // Arrange
            var account = new Account(ValidAccountNumber, 1000m, AccountStatus.Live, AllowedPaymentSchemes.Bacs |
                                                                                    AllowedPaymentSchemes.Chaps |
                                                                                    AllowedPaymentSchemes.FasterPayments);

            // Act
            account.Debit(100m);
            account.Debit(250m);

            // Assert
            Assert.That(account.Balance, Is.EqualTo(650m));
        }

        [Test]
        public void Debit_WhenAmountIsZero_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var account = new Account(ValidAccountNumber, InitialBalance, AccountStatus.Live, AllowedPaymentSchemes.Bacs |
                                                                                                AllowedPaymentSchemes.Chaps |
                                                                                                AllowedPaymentSchemes.FasterPayments);

            // Act &Immutable
            var ex = Assert.Throws<InvalidOperationException>(() => account.Debit(0m));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Amount must be positive."));
        }

        [Test]
        public void Debit_WhenInsufficientFunds_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var account = new Account(ValidAccountNumber, 100m, AccountStatus.Live, AllowedPaymentSchemes.Bacs |
                                                                                    AllowedPaymentSchemes.Chaps |
                                                                                    AllowedPaymentSchemes.FasterPayments);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => account.Debit(150m));
            Assert.That(ex.Message, Is.EqualTo("Insufficient funds."));
        }

        [Test]
        public void SupportsScheme_WhenSchemeIsAllowed_ShouldReturnTrue()
        {
            // Arrange
            var account = new Account(
                ValidAccountNumber,
                InitialBalance,
                AccountStatus.Live,
                AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.FasterPayments
            );

            // Act & Assert
            Assert.That(account.SupportsScheme(AllowedPaymentSchemes.Bacs), Is.True);
            Assert.That(account.SupportsScheme(AllowedPaymentSchemes.FasterPayments), Is.True);
        }

        [Test]
        public void SupportsScheme_WhenSchemeIsNotAllowed_ShouldReturnFalse()
        {
            // Arrange
            var account = new Account(
                ValidAccountNumber,
                InitialBalance,
                AccountStatus.Live,
                AllowedPaymentSchemes.Bacs
            );

            // Act & Assert
            Assert.That(account.SupportsScheme(AllowedPaymentSchemes.Chaps), Is.False);
            Assert.That(account.SupportsScheme(AllowedPaymentSchemes.FasterPayments), Is.False);
        }

        [Test]
        public void SupportsScheme_CombinedFlags_ShouldWorkCorrectly()
        {
            // Arrange
            var account = new Account(
                ValidAccountNumber,
                InitialBalance,
                AccountStatus.Live,
                AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps
            );

            // Act & Assert
            Assert.That(account.SupportsScheme(AllowedPaymentSchemes.Bacs), Is.True);
            Assert.That(account.SupportsScheme(AllowedPaymentSchemes.Chaps), Is.True);
            Assert.That(account.SupportsScheme(AllowedPaymentSchemes.FasterPayments), Is.False);
        }

        [Test]
        public void Constructor_WithNullAccountNumber_ShouldAllow()
        {
            // Act
            var account = new Account(null!, 100m, AccountStatus.Live, AllowedPaymentSchemes.Bacs);

            // Assert
            Assert.That(account.AccountNumber, Is.Null);
        }
    }
}
