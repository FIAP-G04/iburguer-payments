using iBurguer.Payments.Infrastructure.MongoDb.Configurations;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;

namespace iBurguer.Payments.Infrastructure.MongoDb;

public class DbContext : IDbContext
{
    public IMongoClient Client { get; }
    public IMongoDatabase Database { get; }
    
    public DbContext(MongoDbConfiguration configuration)
    {
        var url = new MongoUrl(configuration.ConnectionString);
        var settings = MongoClientSettings.FromUrl(url);
        var options = new InstrumentationOptions { CaptureCommandText = true };

        settings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber(options));

        Client = new MongoClient(settings);
        Database = Client.GetDatabase(configuration.Database);
    }

    public Task<IClientSessionHandle> CreateSession() => Client.StartSessionAsync();

    public void BeginTransaction(IClientSessionHandle session) => session.StartTransaction();
    
    public Task Commit(IClientSessionHandle session) => session.CommitTransactionAsync();
    
    public Task Rollback(IClientSessionHandle session) => session.AbortTransactionAsync();
}