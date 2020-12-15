using System;
using AutoFixture.NUnit3;
using ClearBank.DeveloperTest.Configuration;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.ServiceFactories;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests
{
    [TestFixture]
    public class AccountServiceTests
    {
        [Test]
        [InlineAutoData]
        public void GetAccount_Calls_GetAccount_In_The_Account_Data_Store(int acctNumber)
        {
            //arrange
            var accountDataStore = new Mock<IAccountDataStore>();
            var accountNumber = Convert.ToString(acctNumber);

            var sut = new AccountService(accountDataStore.Object);

            //act
            sut.GetAccount(accountNumber);

            //assert
            accountDataStore.Verify(x => x.GetAccount(accountNumber), Times.Once);
        }

        [Test]
        [InlineAutoData]
        public void UpdateAccount_Calls_UpdateAccount_In_The_Account_Data_Store(Account account)
        {
            //arrange
            var accountDataStore = new Mock<IAccountDataStore>();

            var sut = new AccountService(accountDataStore.Object);

            //act
            sut.UpdateAccount(account);

            //assert
            accountDataStore.Verify(x => x.UpdateAccount(account), Times.Once);
        }
    }
}
