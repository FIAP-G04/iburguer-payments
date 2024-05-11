using System.Diagnostics.CodeAnalysis;
using iBurguer.Payments.Infrastructure.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace iBurguer.Payments.Infrastructure.WebApi;

[ExcludeFromCodeCoverage]
public static class WebApiHostApplicationExtensions
{
    public static IHostApplicationBuilder AddWebApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.AddSwagger();
        builder.Services.AddHealthChecks();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials()
                    .AllowAnyHeader();
            });
        });

        return builder;
    }
    
    public static WebApplication UseWebApi(this WebApplication app)
    {
        app.ConfigureSwagger();
        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.MapControllers();
        app.MapHealthChecks("/hc");

        return app;
    }
}

