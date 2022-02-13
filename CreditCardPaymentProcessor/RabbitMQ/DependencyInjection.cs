using CreditCardPaymentApi.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CreditCardPaymentProcessor.RabbitMQ
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfigurationSection configuration)
        {
            services.Configure<MessageBrokerConfig>(configuration.GetSection("MessageBroker"));
            services.Configure<MessageQueueConfig>(configuration.GetSection("MessageQueue"));

            services.AddSingleton<IRabbitMQRetryPolicies, RabbitMQRetryPolicies>();

            services.AddSingleton(sp =>
            {
                var connectionFactory = new ConnectionFactory() { HostName = configuration.GetValue<string>("MessageBroker:ConnectionString") };
                connectionFactory.DispatchConsumersAsync = true;
                var retryPolicies = sp.GetRequiredService<IRabbitMQRetryPolicies>();

                return retryPolicies
                        .GetConnectionRetryPolicy()
                        .Execute(() => connectionFactory.CreateConnection());
            });

            services.AddSingleton(sp =>
            {
                var connection = sp.GetRequiredService<IConnection>();
                var retryPolicies = sp.GetRequiredService<IRabbitMQRetryPolicies>();

                return retryPolicies
                        .GetChannelRetryPolicy()
                        .Execute(() => connection.CreateModel());
            });

            services.AddSingleton(sp =>
            {
                var messageQueueConfig = configuration.GetSection("MessageQueue").Get<MessageQueueConfig>();
                var channel = sp.GetRequiredService<IModel>();
                channel.QueueDeclare(queue: messageQueueConfig.Name,
                         durable: messageQueueConfig.Durable,
                         exclusive: messageQueueConfig.Exclusive,
                         autoDelete: messageQueueConfig.AutoDelete,
                         arguments: messageQueueConfig.Arguments);

                return new AsyncEventingBasicConsumer(channel);
            });

            return services;
        }
    }
}
