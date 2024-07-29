using System.Diagnostics.CodeAnalysis;
using iBurguer.Payments.Core.Abstractions;
using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Core.EventHandlers;
using iBurguer.Payments.Infrastructure.EventDispatcher;
using iBurguer.Payments.Infrastructure.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace iBurguer.Payments.Infrastructure.IoC;

[ExcludeFromCodeCoverage]
public static class EventHandlerHostApplicationExtensions
{
    public static IHostApplicationBuilder AddEventHandlers(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddScoped<IEventDispatcher, EventDispatcher.EventDispatcher>();
        builder.Services.AddScoped<IEventHandler<PaymentConfirmed>, PaymentEventHandler>();
        builder.Services.AddScoped<IEventHandler<PaymentRefused>, PaymentEventHandler>();
        builder.Services.AddScoped<ISQSService, SQSService>();
        builder.Services.Configure<SQSConfiguration>(configuration.GetRequiredSection("MassTransit"));

        return builder;
    }
}