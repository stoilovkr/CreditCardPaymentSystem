using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using System;

namespace CreditCardPaymentApi.RabbitMQ
{
    public class RabbitMQRetryPolicies : IRabbitMQRetryPolicies
    {
        private readonly MessageBrokerConfig messageBrokerConfiguration;
        private readonly ILogger<RabbitMQRetryPolicies> logger;

        public RabbitMQRetryPolicies(IOptions<MessageBrokerConfig> messageBrokerOptions, ILogger<RabbitMQRetryPolicies> logger)
        {
            messageBrokerConfiguration = messageBrokerOptions.Value;
            this.logger = logger;
        }

        public ISyncPolicy GetConnectionRetryPolicy()
        {
            int maxRetries = messageBrokerConfiguration.ConnectionMaxRetries;
            if (maxRetries <= 0)
            {
                maxRetries = 3;
            }

            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetry(maxRetries, retryCount => TimeSpan.FromSeconds(Math.Pow(2, retryCount)),
                onRetry: (ex, timeSpan, retryCount, context) =>
                {
                    logger.LogWarning($"Exception was thrown when trying to create a connection. Exception message: {ex.Message}");
                    logger.LogWarning($"Retries left: {maxRetries - retryCount + 1}");
                });

            return retryPolicy;
        }

        public ISyncPolicy GetChannelRetryPolicy()
        {
            int maxRetries = messageBrokerConfiguration.ChannelMaxRetries;
            if (maxRetries <= 0)
            {
                maxRetries = 3;
            }

            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetry(maxRetries, retryCount => TimeSpan.FromSeconds(Math.Pow(2, retryCount)),
                onRetry: (ex, timeSpan, retryCount, context) =>
                {
                    logger.LogWarning($"Exception was thrown when trying to create a channel. Exception message: {ex.Message}");
                    logger.LogWarning($"Retries left: {maxRetries - retryCount + 1}");
                });

            return retryPolicy;
        }
    }
}
