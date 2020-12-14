using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class ValidatorService : IValidatorService
    {
        public MakePaymentResult ValidatePayment(Account account, MakePaymentRequest paymentRequest)
        {
            var result = new MakePaymentResult();
            
            if (null == account)
            {
                result.Success = false;
            }

            return result;
        }
    }
}
