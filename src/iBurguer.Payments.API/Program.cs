using iBurguer.Payments.Infrastructure.Http;
using iBurguer.Payments.Infrastructure.IoC;
using iBurguer.Payments.Infrastructure.Logger;
using iBurguer.Payments.Infrastructure.MongoDb.Extensions;
using iBurguer.Payments.Infrastructure.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddRestClient()
       .AddWebApi()
       .AddMongoDb()
       .AddEventHandlers(builder.Configuration)
       .AddRepositories()
       .AddUseCases()
       .AddSerilog();

var app = builder.Build();

app.UseWebApi();
app.Run();