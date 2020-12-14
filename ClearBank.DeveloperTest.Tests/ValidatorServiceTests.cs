using System;
using AutoFixture.NUnit3;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests
{
    [TestFixture]
    public class ValidatorServiceTests
    {
        [Test]
        [InlineAutoData(PaymentScheme.Bacs)]
        [InlineAutoData(PaymentScheme.FasterPayments)]
        [InlineAutoData(PaymentScheme.Chaps)]
        public void Null_Account_Returns_False_For_All_Payment_Types(PaymentScheme scheme, MakePaymentRequest request)
        {
            //arrange
            Account account = null;
            request.PaymentScheme = scheme;

            var sut = new ValidatorService();

            //act
            var result = sut.ValidatePayment(account, request);
            
            //assert
            result.Should().BeEquivalentTo(new MakePaymentResult {Success = false});
        }

        [Test]
        [InlineAutoData(AccountStatus.Live, "99999999999.99")]
        [InlineAutoData(AccountStatus.Live, "0.01")]
        [InlineAutoData(AccountStatus.Live, "0.00")]
        [InlineAutoData(AccountStatus.Live, "-0.01")]
        [InlineAutoData(AccountStatus.Disabled, "0.01")]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, "0.01")]
        public void Account_With_Bacs_Allowed_Always_Returns_False_For_Bacs_Payments(AccountStatus accountStatus,
            string accountBalance, Account account, MakePaymentRequest request)
        {
            //arrange
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;
            account.Status = accountStatus;
            account.Balance = Convert.ToDecimal(accountBalance);
            request.PaymentScheme = PaymentScheme.Bacs;

            var sut = new ValidatorService();

            //act
            var result = sut.ValidatePayment(account, request);

            //assert
            result.Should().BeEquivalentTo(new MakePaymentResult { Success = true });
        }
    }
}
