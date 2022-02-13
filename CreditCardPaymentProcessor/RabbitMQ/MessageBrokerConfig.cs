namespace CreditCardPaymentProcessor.RabbitMQ
{
    public class MessageBrokerConfig
    {
        public string ConnectionString { get; set; }
        public int ConnectionMaxRetries { get; set; }
        public int ChannelMaxRetries { get; set; }
    }
}
