using System;

namespace CreditCardPaymentApi.Models
{
    public class CreditCardPayment
    {
        public string CreditCardNumber { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyIsoCode { get; set; }
        public DateTime PaymentTime { get; set; }
    }
}
