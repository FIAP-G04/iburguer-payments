using iBurguer.Payments.Core.Abstractions;
using static iBurguer.Payments.Core.Exceptions;

namespace iBurguer.Payments.Core.Domain;

public class Payment : Entity<Guid>, IAggregateRoot
{
    public Guid OrderId { get; init; }
    public Amount Amount { get; init; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? PayedAt { get; private set; }
    public DateTime? RefusedAt { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string QrData { get; init; }

    public Payment(Guid orderId, Amount amount, string qrData)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        Amount = amount;
        Status = PaymentStatus.Pending;
        QrData = qrData;
        CreatedAt = DateTime.Now;
    }
    
    public bool Confirmed => Status == PaymentStatus.Processed && PayedAt is not null;

    public void Confirm()
    {
        CannotToConfirmPaymentException.ThrowIf(Status != PaymentStatus.Pending);

        PayedAt = DateTime.Now;
        Status = PaymentStatus.Processed;
        
        RaiseEvent(new PaymentConfirmed(OrderId));
    }

    public void Refuse()
    {
        CannotToRefusePaymentException.ThrowIf(Status != PaymentStatus.Pending);

        RefusedAt = DateTime.Now;
        Status = PaymentStatus.Refused;
        
        RaiseEvent(new PaymentRefused(OrderId));
    }
}