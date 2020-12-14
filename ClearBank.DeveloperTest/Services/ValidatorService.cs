using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class ValidatorService : IValidatorService
    {
        public MakePaymentResult ValidatePayment(Account account, MakePaymentRequest request)
        {
            var result = new MakePaymentResult();

            if (null == account)
            {
                result.Success = false;
                return result;
            }
            
            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    if (account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                    {
                        result.Success = true;
                    }
                    break;

                case PaymentScheme.FasterPayments:
                    if (account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                    {
                        result.Success = true;
                    }
                    else if (account.Balance > request.Amount)
                    {
                        result.Success = true;
                    }
                    break;

                case PaymentScheme.Chaps:
                    if (account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                    {
                        result.Success = true;
                    }
                    else if (account.Status == AccountStatus.Live)
                    {
                        result.Success = true;
                    }
                    break;
            }

            return result;
        }
    }
}
