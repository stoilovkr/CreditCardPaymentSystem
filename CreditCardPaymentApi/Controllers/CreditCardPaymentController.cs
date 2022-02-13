using AutoMapper;
using CreditCardPaymentApi.Dtos;
using CreditCardPaymentApi.Models;
using CreditCardPaymentApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CreditCardPaymentApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreditCardPaymentController : ControllerBase
    {
        private readonly ICreditCardPaymentService creditCardPaymentService;
        private readonly IMapper mapper;

        public CreditCardPaymentController(ICreditCardPaymentService creditCardPaymentService, IMapper mapper)
        {
            this.creditCardPaymentService = creditCardPaymentService;
            this.mapper = mapper;
        }

        [HttpPost]
        public IActionResult PostPayment(CreditCardPaymentRequest creditCardPaymentRequest)
        {
            var creditCardPayment = mapper.Map<CreditCardPaymentRequest, CreditCardPayment>(creditCardPaymentRequest);
            try
            {
                creditCardPaymentService.PostCreditCardPaymentMessage(creditCardPayment);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message, null, 500, "Exception was thrown during posting message to message queue.");
            }

            return Accepted();
        }
    }
}
