using AutoFixture.NUnit3;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests
{
    [TestFixture]
    public class PaymentServiceTests
    {
        [Test]
        [InlineAutoData(true)]
        [InlineAutoData(false)]
        public void MakePayment_Returns_The_Validity_Status_Of_The_Payment(bool isPaymentValid, Account account,
            MakePaymentRequest request)
        {
            //arrange
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.GetAccount(request.DebtorAccountNumber))
                .Returns(account);

            var validatorService = new Mock<IValidatorService>();
            validatorService.Setup(x => x.ValidatePayment(account, request))
                .Returns(new MakePaymentResult { Success = isPaymentValid });

            var sut = new PaymentService(accountService.Object, validatorService.Object);

            //act
            var result = sut.MakePayment(request);

            //assert
            result.Should().BeEquivalentTo(new MakePaymentResult { Success = isPaymentValid });
        }

        [Test]
        [InlineAutoData]
        public void MakePayment_Deducts_The_Request_Amount_From_The_Account_Balance_For_Valid_Payment(
            Account account, MakePaymentRequest request)
        {
            //arrange
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.GetAccount(request.DebtorAccountNumber))
                .Returns(account);

            var validatorService = new Mock<IValidatorService>();
            validatorService.Setup(x => x.ValidatePayment(account, request))
                .Returns(new MakePaymentResult { Success = true });

            var sut = new PaymentService(accountService.Object, validatorService.Object);

            //act
            sut.MakePayment(request);

            //assert
            account.Balance = account.Balance - request.Amount;
            accountService.Verify(x => x.UpdateAccount(account), Times.Once);
        }

        [Test]
        [InlineAutoData]
        public void MakePayment_Does_Not_Deduct_The_Request_Amount_From_The_Account_Balance_For_Invalid_Payment(
            Account account, MakePaymentRequest request)
        {
            //arrange
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.GetAccount(request.DebtorAccountNumber))
                .Returns(account);

            var validatorService = new Mock<IValidatorService>();
            validatorService.Setup(x => x.ValidatePayment(account, request))
                .Returns(new MakePaymentResult { Success = false });

            var sut = new PaymentService(accountService.Object, validatorService.Object);

            //act
            sut.MakePayment(request);

            //assert
            accountService.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }
    }
}
