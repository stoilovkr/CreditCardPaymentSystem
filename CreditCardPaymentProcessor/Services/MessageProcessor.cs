using CreditCardPaymentApi.Models;
using CreditCardPaymentApi.RabbitMQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CreditCardPaymentProcessor.Services
{
    public class MessageProcessor : BackgroundService
    {
        private readonly AsyncEventingBasicConsumer consumer;
        private readonly IModel channel;
        private readonly ILogger<MessageProcessor> logger;
        private readonly MessageQueueConfig messageQueueConfig;

        public MessageProcessor(AsyncEventingBasicConsumer consumer, IModel channel, IOptions<MessageQueueConfig> messageQueueConfigOptions, ILogger<MessageProcessor> logger)
        {
            this.consumer = consumer;
            this.channel = channel;
            this.logger = logger;
            messageQueueConfig = messageQueueConfigOptions.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            consumer.Received += ProcessEventAsync;
            string consumerTag = channel.BasicConsume(messageQueueConfig.Name, false, consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            return;
        }

        private Task ProcessEventAsync(object ch, BasicDeliverEventArgs eventArgs)
        {
            var messageByteArray = eventArgs.Body.ToArray();
            var message = JsonSerializer.Deserialize<CreditCardPayment>(messageByteArray);
            logger.LogInformation($"Cc number: {message.CreditCardNumber}, Amount: {message.Amount} {message.CurrencyIsoCode}");

            channel.BasicAck(eventArgs.DeliveryTag, false);

            return Task.CompletedTask;
        }
    }
}
