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
        public void Null_Account_Alway_Returns_False_For_All_Payment_Types(PaymentScheme scheme, MakePaymentRequest request)
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
        public void Account_With_An_Allowed_PaymentScheme_Allowed_Always_Returns_True_For_Payment_With_Corresponding_Payment_Scheme(AccountStatus accountStatus,
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

        [Test]
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.FasterPayments)]
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.Chaps)]
        [InlineAutoData(AccountStatus.Disabled, AllowedPaymentSchemes.FasterPayments)]
        [InlineAutoData(AccountStatus.Disabled, AllowedPaymentSchemes.Chaps)]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.FasterPayments)]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Chaps)]
        public void Account_Without_Bacs_Allowed_Always_Returns_False_For_Bacs_Payment(AccountStatus accountStatus, AllowedPaymentSchemes allowedPaymentScheme,
            Account account, MakePaymentRequest request)
        {
            //arrange
            account.AllowedPaymentSchemes = allowedPaymentScheme;
            account.Status = accountStatus;
            account.Balance = request.Amount + 0.01m;
            request.PaymentScheme = PaymentScheme.Bacs;

            var sut = new ValidatorService();

            //act
            var result = sut.ValidatePayment(account, request);

            //assert
            result.Should().BeEquivalentTo(new MakePaymentResult { Success = false });
        }

        [Test]
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.Bacs, "3425.32", "3425.31")]
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.Chaps, "4523543.84", "4523543.83")]
        [InlineAutoData(AccountStatus.Disabled, AllowedPaymentSchemes.Bacs, "8453.11", "8453.10")]
        [InlineAutoData(AccountStatus.Disabled, AllowedPaymentSchemes.Chaps, "7463.00", "7462.99")]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Bacs, "24345.94", "24345.93")]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Chaps, "36874.78", "36874.77")]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Chaps, "0.01", "0.00")] //Is this a business rule?
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Chaps, "0.00", "-0.01")] //Is this a business rule?
        //
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.Bacs, "3425.32", "3425.32")]
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.Chaps, "4523543.84", "4523543.84")]
        [InlineAutoData(AccountStatus.Disabled, AllowedPaymentSchemes.Bacs, "8453.11", "8453.11")]
        [InlineAutoData(AccountStatus.Disabled, AllowedPaymentSchemes.Chaps, "7463.00", "7463.00")]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Bacs, "24345.94", "24345.94")]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Chaps, "36874.78", "36874.78")]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Chaps, "0.00", "0.00")] //Is this a business rule?
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Chaps, "-0.01", "-0.01")] //Is this a business rule?
        public void Account_Without_FasterPayment_Allowed_With_Balance_Greater_Than_Or_Equal_To_Request_Amount_Always_Returns_True_For_FasterPayments(AccountStatus accountStatus, 
            AllowedPaymentSchemes allowedPaymentScheme, string accountBalance, string requestAmount, Account account, MakePaymentRequest request)
        {
            //arrange
            account.AllowedPaymentSchemes = allowedPaymentScheme;
            account.Status = accountStatus;
            account.Balance = Convert.ToDecimal(accountBalance);
            request.Amount = Convert.ToDecimal(requestAmount);
            request.PaymentScheme = PaymentScheme.FasterPayments;

            var sut = new ValidatorService();

            //act
            var result = sut.ValidatePayment(account, request);

            //assert
            result.Should().BeEquivalentTo(new MakePaymentResult { Success = true });
        }

        [Test]
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.Bacs, "3425.32", "3425.33")]
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.Chaps, "4523543.84", "4523543.85")]
        [InlineAutoData(AccountStatus.Disabled, AllowedPaymentSchemes.Bacs, "8453.11", "8453.12")]
        [InlineAutoData(AccountStatus.Disabled, AllowedPaymentSchemes.Chaps, "7463.00", "7463.01")]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Bacs, "24345.94", "24345.95")]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Chaps, "36874.78", "36874.79")]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Chaps, "0.00", "0.01")] //Is this a business rule?
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Chaps, "-0.01", "0.00")] //Is this a business rule?
        public void Account_Without_FasterPayment_Allowed_With_Balance_Less_Than_Request_Amount_Always_Returns_False_For_FasterPayments(AccountStatus accountStatus,
            AllowedPaymentSchemes allowedPaymentScheme, string accountBalance, string requestAmount, Account account, MakePaymentRequest request)
        {
            //arrange
            account.AllowedPaymentSchemes = allowedPaymentScheme;
            account.Status = accountStatus;
            account.Balance = Convert.ToDecimal(accountBalance);
            request.Amount = Convert.ToDecimal(requestAmount);
            request.PaymentScheme = PaymentScheme.FasterPayments;

            var sut = new ValidatorService();

            //act
            var result = sut.ValidatePayment(account, request);

            //assert
            result.Should().BeEquivalentTo(new MakePaymentResult { Success = false });
        }

        [Test]
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.Bacs, "3425.32", "3425.32")]
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.FasterPayments, "4523543.84", "4523543.84")]
        //
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.Bacs, "3425.32", "3425.31")]
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.FasterPayments, "4523543.84", "4523543.83")]
        //
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.Bacs, "3425.32", "3425.33")]
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.FasterPayments, "4523543.84", "4523543.85")]
        //
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.Bacs, "0.00", "0.01")] //Is this a business rule?
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.Bacs, "-0.01", "0.00")] //Is this a business rule?
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.Bacs, "-0.01", "-0.01")] //Is this a business rule?
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.FasterPayments, "0.00", "0.01")] //Is this a business rule?
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.FasterPayments, "-0.01", "0.00")] //Is this a business rule?
        [InlineAutoData(AccountStatus.Live, AllowedPaymentSchemes.FasterPayments, "-0.05", "-0.01")] //Is this a business rule?
        public void Account_Without_Chaps_Allowed_And_Live_Always_Returns_True_For_Chaps_Payments(AccountStatus accountStatus,
            AllowedPaymentSchemes allowedPaymentScheme, string accountBalance, string requestAmount, Account account, MakePaymentRequest request)
        {
            //arrange
            account.AllowedPaymentSchemes = allowedPaymentScheme;
            account.Status = accountStatus;
            account.Balance = Convert.ToDecimal(accountBalance);
            request.Amount = Convert.ToDecimal(requestAmount);
            request.PaymentScheme = PaymentScheme.Chaps;

            var sut = new ValidatorService();

            //act
            var result = sut.ValidatePayment(account, request);

            //assert
            result.Should().BeEquivalentTo(new MakePaymentResult { Success = true });
        }

        [Test]
        [InlineAutoData(AccountStatus.Disabled, AllowedPaymentSchemes.Bacs, "3425.32", "3425.32")]
        [InlineAutoData(AccountStatus.Disabled, AllowedPaymentSchemes.Bacs, "3425.32", "3425.31")]
        [InlineAutoData(AccountStatus.Disabled, AllowedPaymentSchemes.Bacs, "3425.32", "3425.33")]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Bacs, "3425.32", "3425.32")]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Bacs, "3425.32", "3425.31")]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.Bacs, "3425.32", "3425.33")]
        //
        [InlineAutoData(AccountStatus.Disabled, AllowedPaymentSchemes.FasterPayments, "4523543.84", "4523543.84")]
        [InlineAutoData(AccountStatus.Disabled, AllowedPaymentSchemes.FasterPayments, "4523543.84", "4523543.83")]
        [InlineAutoData(AccountStatus.Disabled, AllowedPaymentSchemes.FasterPayments, "4523543.84", "4523543.85")]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.FasterPayments, "4523543.84", "4523543.84")]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.FasterPayments, "4523543.84", "4523543.83")]
        [InlineAutoData(AccountStatus.InboundPaymentsOnly, AllowedPaymentSchemes.FasterPayments, "4523543.84", "4523543.85")]
        public void Account_Without_Chaps_Allowed_And_Not_Live_Always_Returns_False_For_Chaps_Payments(AccountStatus accountStatus,
            AllowedPaymentSchemes allowedPaymentScheme, string accountBalance, string requestAmount, Account account, MakePaymentRequest request)
        {
            //arrange
            account.AllowedPaymentSchemes = allowedPaymentScheme;
            account.Status = accountStatus;
            account.Balance = Convert.ToDecimal(accountBalance);
            request.Amount = Convert.ToDecimal(requestAmount);
            request.PaymentScheme = PaymentScheme.Chaps;

            var sut = new ValidatorService();

            //act
            var result = sut.ValidatePayment(account, request);

            //assert
            result.Should().BeEquivalentTo(new MakePaymentResult { Success = false });
        }
    }
}
