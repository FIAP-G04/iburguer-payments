using MongoDB.Driver;

namespace iBurguer.Payments.Infrastructure.MongoDb;

public interface IDbContext
{
    IMongoClient Client { get; }
    
    IMongoDatabase Database { get; }

    Task<IClientSessionHandle> CreateSession();
    
    void BeginTrasaction(IClientSessionHandle session);
    
    Task Commit(IClientSessionHandle session);
    
    Task Rollback(IClientSessionHandle session);
}