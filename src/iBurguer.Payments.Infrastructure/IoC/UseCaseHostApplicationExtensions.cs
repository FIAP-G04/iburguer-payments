using System.Diagnostics.CodeAnalysis;
using iBurguer.Payments.Core.UseCases.ConfirmPayment;
using iBurguer.Payments.Core.UseCases.GenerateQrCode;
using iBurguer.Payments.Core.UseCases.GetPaymentUseCase;
using iBurguer.Payments.Core.UseCases.RefusePaymentUseCase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace iBurguer.Payments.Infrastructure.IoC;

[ExcludeFromCodeCoverage]
public static class UseCaseHostApplicationExtensions
{
    public static IHostApplicationBuilder AddUseCases(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IConfirmPaymentUseCase, ConfirmPaymentUseCase>()
                        .AddScoped<IRefusePaymentUseCase, RefusePaymentUseCase>()
                        .AddScoped<IGenerateQrCodeUseCase, GenerateQrCodeUseCase>()
                        .AddScoped<IGetPaymentUseCase, GetPaymentUseCase>();

        return builder;
    }
}