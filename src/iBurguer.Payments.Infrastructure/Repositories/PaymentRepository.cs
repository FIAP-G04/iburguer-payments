using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Infrastructure.EventDispatcher;
using iBurguer.Payments.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace iBurguer.Payments.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly IMongoCollection<Payment> _collection;
    private readonly IDbContext _context;
    private readonly IEventDispatcher _dispatcher;
    
    public PaymentRepository(IDbContext mongoDbContext, IEventDispatcher dispatcher)
    {
        _collection = mongoDbContext.Database.GetCollection<Payment>("payments");
        _context = mongoDbContext;
        _dispatcher = dispatcher;
    }

    public async Task Save(Payment payment)
    {
        await _collection.InsertOneAsync(payment, null);
    }

    public async Task<bool> Update(Payment payment, CancellationToken cancellationToken)
    {
        var update = Builders<Payment>.Update
            .Set(p => p.Status, payment.Status)
            .Set(p => p.RefusedAt, payment.RefusedAt)
            .Set(p => p.PayedAt, payment.PayedAt);
        
        using (var session = await _context.CreateSession())
        {
            _context.BeginTrasaction(session);

            try
            {
                await _collection.UpdateOneAsync(session, i => i.Id == payment.Id, update, null);
                
                foreach (var @event in payment.Events)
                {
                    await _dispatcher.Dispatch(@event, cancellationToken);
                }

                await _context.Commit(session);

                return true;
            }
            catch (Exception e)
            {
                await _context.Rollback(session);
                
                return false;
            }
        }
    }

    public async Task<Payment?> GetById(Guid paymentId, CancellationToken cancellationToken)
    {
        return await _collection.Find(i => i.Id == paymentId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsPaymentForOrder(Guid orderId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}