using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture.NUnit3;
using ClearBank.DeveloperTest.Configuration;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.ServiceFactories;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests
{
    [TestFixture]
    public class ServiceFactoryTests
    {
        [Test]
        public void Config_DataStoreType_Of_Backup_Returns_Backup_Account_Data_Store()
        {
            //arrange
            var configManager = new Mock<IConfigManager>();
            configManager.Setup(x => x.GetDataStoreType()).Returns("Backup");

            var sut = new ServiceFactory(configManager.Object);

            //act
            var accountDataStore = sut.GetAccountDataStore();

            //assert
            accountDataStore.Should().NotBeNull();
            accountDataStore.GetType().Should().Be(typeof(BackupAccountDataStore));
        }

        [Test]
        [InlineAutoData]
        [InlineAutoData("Main")]
        public void Config_DataStoreType_Not_Of_Backup_Returns_Account_Data_Store(string dataStoreType)
        {
            //arrange
            var configManager = new Mock<IConfigManager>();
            configManager.Setup(x => x.GetDataStoreType()).Returns(dataStoreType);

            var sut = new ServiceFactory(configManager.Object);

            //act
            var accountDataStore = sut.GetAccountDataStore();

            //assert
            accountDataStore.Should().NotBeNull();
            accountDataStore.GetType().Should().Be(typeof(AccountDataStore));
        }
    }
}
