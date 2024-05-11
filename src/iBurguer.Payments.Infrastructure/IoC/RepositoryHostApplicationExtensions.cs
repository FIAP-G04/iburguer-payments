using System.Diagnostics.CodeAnalysis;
using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace iBurguer.Payments.Infrastructure.IoC;

[ExcludeFromCodeCoverage]
public static class RepositoryHostApplicationExtensions
{
    public static IHostApplicationBuilder AddRepositories(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

        return builder;
    }
}