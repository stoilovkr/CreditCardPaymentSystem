using System;
using System.ComponentModel.DataAnnotations;

namespace CreditCardPaymentApi.Dtos
{
    public class CreditCardPaymentRequest
    {
        /// <summary>
        /// Credit card number.
        /// </summary>
        /// <example>1111222233334444</example>
        [CreditCard]
        [Required]
        public string CreditCardNumber { get; set; }
        /// <summary>
        /// The amout of the payment.
        /// </summary>
        /// <example>99.9</example>
        [Required]
        public decimal Amount { get; set; }
        /// <summary>
        /// The iso code of the currency.
        /// </summary>
        /// <example>EUR</example>
        [StringLength(3, ErrorMessage = "The currency property must be 3 characters long.", MinimumLength = 3)]
        [Required]
        public string CurrencyIsoCode { get; set; }
        /// <summary>
        /// The date and time of the payment.
        /// </summary>
        /// <example>2022-01-01T00:00:00.000Z</example>
        [Required]
        public DateTime PaymentTime { get; set; }
    }
}
