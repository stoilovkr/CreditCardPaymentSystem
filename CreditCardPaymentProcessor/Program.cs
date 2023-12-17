using CreditCardPaymentProcessor.RabbitMQ;
using CreditCardPaymentProcessor.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRabbitMQ(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddHostedService<MessageProcessor>();

var app = builder.Build();
app.Run();