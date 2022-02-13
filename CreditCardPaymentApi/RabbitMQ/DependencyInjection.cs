using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace CreditCardPaymentApi.RabbitMQ
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

            return services;
        }
    }
}
