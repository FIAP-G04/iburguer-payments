using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace iBurguer.Payments.Infrastructure.Swagger;

[ExcludeFromCodeCoverage]
public static class SwaggerHostApplicationExtensions
{
    private const string Title = "iBurguer Payments API";
    private const string Description = "The Payment Status API is a vital tool within the iBurger platform, specifically crafted for Byte Burger. This RESTful API offers a solution for tracking all stages of payments associated with orders, ensuring a transaction process from start to finish.";
    private const string Version = "v1";
    
    public static IHostApplicationBuilder AddSwagger(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(Version, new OpenApiInfo
            {
                Title = Title, 
                Description = Description, 
                Version = Version
            });
            
            //options.ExampleFilters();
            options.EnableAnnotations();
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "iBurguer.Payments.API.xml"));
            options.DescribeAllParametersInCamelCase();
        });

        return builder;
    }
    
    public static void ConfigureSwagger(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;
        
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Title} {Version}");
            c.DisplayRequestDuration();
        });
    }
}