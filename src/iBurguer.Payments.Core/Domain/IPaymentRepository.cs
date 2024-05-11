namespace iBurguer.Payments.Core.Domain;

public interface IPaymentRepository
{
    Task Save(Payment payment);

    Task<bool> Update(Payment payment, CancellationToken cancellationToken);

    Task<Payment?> GetById(Guid paymentId, CancellationToken cancellationToken);

    Task<bool> IsPaymentForOrder(Guid orderId, CancellationToken cancellationToken);
}