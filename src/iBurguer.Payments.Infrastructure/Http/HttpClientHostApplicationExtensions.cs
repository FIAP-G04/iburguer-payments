using System.Diagnostics.CodeAnalysis;
using iBurguer.Payments.Core.Gateways;
using iBurguer.Payments.Infrastructure.Http.MercadoPago;
using iBurguer.Payments.Infrastructure.Http.Order;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace iBurguer.Payments.Infrastructure.Http;

[ExcludeFromCodeCoverage]
public static class HttpClientHostApplicationExtensions
{
    public static IHostApplicationBuilder AddRestClient(this IHostApplicationBuilder builder)
    {
        var configuration = builder.Configuration.GetRequiredSection("OrderApi").Get<OrderApiConfiguration>();

        configuration!.ThrowIfInvalid();

        builder.Services.AddHttpClient<IOrderGateway, OrderApiClient>(options =>
        {
            options.BaseAddress = new Uri(configuration.Url);
            options.DefaultRequestHeaders.Clear();
        })
            .AddResilienceHandler("MenuApi", options =>
        {
            options
                .AddRetry(new HttpRetryStrategyOptions()
                {
                    MaxRetryAttempts = configuration.MaxRetryAttempts, 
                    Delay = TimeSpan.FromMilliseconds(configuration.RetryDelay)
                })
                .AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions()
                {
                    MinimumThroughput = configuration.CircuitBreakerMinimumThroughput, 
                    BreakDuration = TimeSpan.FromSeconds(configuration.CircuitBreakerBreakDuration)
                })
                .AddTimeout(new HttpTimeoutStrategyOptions()
                {
                    Timeout = TimeSpan.FromSeconds(configuration.Timeout)
                });
        });
        
        builder.Services.AddSingleton(configuration);
        
        builder.Services.AddScoped<IOrderGateway, OrderApiClient>();
        builder.Services.AddScoped<IPaymentGateway, MercadoPagoClient>();

        return builder;
    }
}