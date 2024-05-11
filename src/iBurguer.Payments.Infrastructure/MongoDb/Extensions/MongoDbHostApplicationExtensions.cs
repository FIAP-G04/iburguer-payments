using System.Diagnostics.CodeAnalysis;
using iBurguer.Payments.Infrastructure.MongoDb.Configurations;
using iBurguer.Payments.Infrastructure.MongoDb.Serializers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace iBurguer.Payments.Infrastructure.MongoDb.Extensions;

[ExcludeFromCodeCoverage]
public static class MongoDbHostApplicationExtensions
{
    public static IHostApplicationBuilder AddMongoDb(this IHostApplicationBuilder builder)
    {
        var mongoDbConfiguration = builder.Configuration
            .GetRequiredSection("MongoDb")
            .Get<MongoDbConfiguration>();

        mongoDbConfiguration!.ThrowIfInvalid();

        builder.Services.AddSingleton(mongoDbConfiguration);
        builder.Services.AddSingleton<IDbContext, DbContext>();

        MongoDbSerializers.Register();
        MongoDbConventions.Register();

        return builder;
    }
}