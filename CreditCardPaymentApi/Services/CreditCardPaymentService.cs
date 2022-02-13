using CreditCardPaymentApi.Models;
using CreditCardPaymentApi.RabbitMQ;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace CreditCardPaymentApi.Services
{
    public class CreditCardPaymentService : ICreditCardPaymentService
    {
        private readonly IModel channel;
        private readonly ILogger<CreditCardPaymentService> logger;
        private readonly MessageQueueConfig messageQueueConfig;

        public CreditCardPaymentService(IModel channel, IOptions<MessageQueueConfig> messageQueueConfigOptions, ILogger<CreditCardPaymentService> logger)
        {
            this.channel = channel;
            this.logger = logger;
            messageQueueConfig = messageQueueConfigOptions.Value;
        }

        public void PostCreditCardPaymentMessage(CreditCardPayment creditCardPayment)
        {
            var message = JsonSerializer.Serialize(creditCardPayment);
            var body = Encoding.UTF8.GetBytes(message);
            logger.LogInformation($"Posting message to message queue: CC number:{creditCardPayment.CreditCardNumber}, Amount: {creditCardPayment.Amount} {creditCardPayment.CurrencyIsoCode}");
            try
            {
                channel.BasicPublish(exchange: "",
                                     routingKey: messageQueueConfig.Name,
                                     basicProperties: null,
                                     body: body);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);

                throw;
            }
        }
    }
}
