using CreditCardPaymentApi.Models;

namespace CreditCardPaymentApi.Services
{
    public interface ICreditCardPaymentService
    {
        void PostCreditCardPaymentMessage(CreditCardPayment creditCardPayment);
    }
}
