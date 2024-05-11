using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;

namespace iBurguer.Payments.Infrastructure.Logger;

[ExcludeFromCodeCoverage]
public static class LoggerHostApplicationExtensions
{
    public static IHostApplicationBuilder AddSerilog(this IHostApplicationBuilder builder)
    {
        var webApplicationBuilder = (WebApplicationBuilder)builder;

        webApplicationBuilder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
        {
            loggerConfiguration
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(hostingContext.Configuration)
                .WriteTo.Console( new JsonFormatter());
        });

        return webApplicationBuilder;
    }
}