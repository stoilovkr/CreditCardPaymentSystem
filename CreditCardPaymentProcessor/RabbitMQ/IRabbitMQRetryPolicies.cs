using Polly;

namespace CreditCardPaymentProcessor.RabbitMQ
{
    public interface IRabbitMQRetryPolicies
    {
        ISyncPolicy GetConnectionRetryPolicy();
        ISyncPolicy GetChannelRetryPolicy();
    }
}