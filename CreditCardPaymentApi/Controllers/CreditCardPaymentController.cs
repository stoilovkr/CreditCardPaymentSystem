using AutoMapper;
using CreditCardPaymentApi.Dtos;
using CreditCardPaymentApi.Models;
using CreditCardPaymentApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CreditCardPaymentApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CreditCardPaymentController(ICreditCardPaymentService creditCardPaymentService, IMapper mapper) : ControllerBase
{
    private readonly ICreditCardPaymentService _creditCardPaymentService = creditCardPaymentService;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Posts a credit card payment requests onto a message queue for later processing.
    /// </summary>
    /// <param name="creditCardPaymentRequest"></param>
    /// <returns>A credit card payment request.</returns>
    /// <response code="202">Empty response.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="500">If the message queue broker connection is broken.</response>
    [HttpPost]
    [Route("postPayment")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult PostPayment(CreditCardPaymentRequest creditCardPaymentRequest)
    {
        var creditCardPayment = _mapper.Map<CreditCardPaymentRequest, CreditCardPayment>(creditCardPaymentRequest);
        try
        {
            _creditCardPaymentService.PostCreditCardPaymentMessage(creditCardPayment);
        }
        catch(Exception ex)
        {
            return Problem(ex.Message, null, 500, "Exception was thrown during posting message to message queue.");
        }

        return Accepted();
    }
}
