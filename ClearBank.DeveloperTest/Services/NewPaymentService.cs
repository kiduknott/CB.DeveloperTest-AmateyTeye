using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class NewPaymentService : IPaymentService
    {
        private readonly IAccountService accountService;
        private readonly IValidatorService validatorService;

        public NewPaymentService(IAccountService acctService, IValidatorService validator)
        {
            accountService = acctService;
            validatorService = validator;
        }
        
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = accountService.GetAccount(request.DebtorAccountNumber);

            var result = validatorService.ValidatePayment(account, request);

            if (result.Success)
            {
                account.Balance -= request.Amount;
                accountService.UpdateAccount(account);
            }

            return result;
        }
    }
}
