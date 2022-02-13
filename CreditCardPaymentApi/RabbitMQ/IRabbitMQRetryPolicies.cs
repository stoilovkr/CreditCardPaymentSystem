using Polly;

namespace CreditCardPaymentApi.RabbitMQ
{
    public interface IRabbitMQRetryPolicies
    {
        ISyncPolicy GetConnectionRetryPolicy();
        ISyncPolicy GetChannelRetryPolicy();
    }
}