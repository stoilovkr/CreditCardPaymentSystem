using System;
using System.ComponentModel.DataAnnotations;

namespace CreditCardPaymentApi.Dtos
{
    public class CreditCardPaymentRequest
    {
        [CreditCard]
        [Required]
        public string CreditCardNumber { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [StringLength(3, ErrorMessage = "The currency property must be 3 characters long.", MinimumLength = 3)]
        [Required]
        public string CurrencyIsoCode { get; set; }
        [Required]
        public DateTime PaymentTime { get; set; }
    }
}
