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
        [InlineAutoData(AccountStatus.Live, "99999999999.99", PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
        [InlineAutoData(AccountStatus.Live, "0.01", PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
        [InlineAutoData(AccountStatus.Live, "0.00", PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
        [InlineAutoData(AccountStatus.Live, "-0.01", PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
        [InlineAutoData(AccountStatus.Disabled, "0.01", PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, "0.01", PaymentScheme.Bacs, AllowedPaymentSchemes.Bacs)]
        //
        [InlineAutoData(AccountStatus.Live, "99999999999.99", PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments)]
        [InlineAutoData(AccountStatus.Live, "0.01", PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments)]
        [InlineAutoData(AccountStatus.Live, "0.00", PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments)]
        [InlineAutoData(AccountStatus.Live, "-0.01", PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments)]
        [InlineAutoData(AccountStatus.Disabled, "0.01", PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments)]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, "0.01", PaymentScheme.FasterPayments, AllowedPaymentSchemes.FasterPayments)]
        //
        [InlineAutoData(AccountStatus.Live, "99999999999.99", PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps)]
        [InlineAutoData(AccountStatus.Live, "0.01", PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps)]
        [InlineAutoData(AccountStatus.Live, "0.00", PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps)]
        [InlineAutoData(AccountStatus.Live, "-0.01", PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps)]
        [InlineAutoData(AccountStatus.Disabled, "0.01", PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps)]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, "0.01", PaymentScheme.Chaps, AllowedPaymentSchemes.Chaps)]
        public void Account_With_PaymentScheme_Allowed_Always_Returns_True_For_Corresponding_PaymentScheme(AccountStatus accountStatus,
            string accountBalance, PaymentScheme paymentScheme, AllowedPaymentSchemes allowedPaymentScheme, 
            Account account, MakePaymentRequest request)
        {
            //arrange
            account.AllowedPaymentSchemes = allowedPaymentScheme;
            account.Status = accountStatus;
            account.Balance = Convert.ToDecimal(accountBalance);
            request.PaymentScheme = paymentScheme;

            var sut = new ValidatorService();

            //act
            var result = sut.ValidatePayment(account, request);

            //assert
            result.Should().BeEquivalentTo(new MakePaymentResult { Success = true });
        }

       
    }
}
