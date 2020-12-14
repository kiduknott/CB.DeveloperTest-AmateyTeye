using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
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
        public void Null_Account_Returns_False_For_All_Payment_Types(PaymentScheme scheme, MakePaymentRequest paymentRequest)
        {
            //arrange
            Account account = null;
            paymentRequest.PaymentScheme = scheme;

            var sut = new ValidatorService();

            //act
            var result = sut.ValidatePayment(account, paymentRequest);
            
            //assert
            result.Should().BeEquivalentTo(new MakePaymentResult {Success = false});
        }
    }
}
