using ClearBank.DeveloperTest.Infrastructure.Data;
using ClearBank.DeveloperTest.Infrastructure.Interfaces;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.InfrastructureTests.DataTests
{
    [TestFixture]
    public class AccountDataStoreFactoryTests
    {
        private Mock<IConfigSettings> _configSettingsMock;
        private AccountDataStoreFactory _accountDataStoreFactory;

        [SetUp]
        public void SetUp()
        {
            _configSettingsMock = new Mock<IConfigSettings>();
            _accountDataStoreFactory = new AccountDataStoreFactory(_configSettingsMock.Object);
        }

        [Test]
        public void ShouldReturnBackUpDataStore()
        {
            //Arrange
            _configSettingsMock.Setup(settings => settings.GetDataStoreType).Returns("Backup");

            //Act
            var result = _accountDataStoreFactory.GetInstance();

            //Assert
            Assert.That(result, Is.InstanceOf<BackupAccountDataStore>());
        }

        [Test]
        public void ShouldReturnAccountDataStoreAsDefault()
        {
            //Act
            var result = _accountDataStoreFactory.GetInstance();

            //Assert
            Assert.That(result, Is.InstanceOf<AccountDataStore>());
        }
    }
}
