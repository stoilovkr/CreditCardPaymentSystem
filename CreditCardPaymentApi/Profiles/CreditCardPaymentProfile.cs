using AutoMapper;
using CreditCardPaymentApi.Dtos;
using CreditCardPaymentApi.Models;

namespace CreditCardPaymentApi.Profiles
{
    public class CreditCardPaymentProfile : Profile
    {
        public CreditCardPaymentProfile()
        {
            CreateMap<CreditCardPaymentRequest, CreditCardPayment>();
        }
    }
}
