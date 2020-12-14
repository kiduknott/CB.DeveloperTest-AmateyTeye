using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public interface IValidatorService
    {
        public MakePaymentResult ValidatePayment(Account account, MakePaymentRequest request);
    }
}
