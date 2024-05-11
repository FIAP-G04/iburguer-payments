using iBurguer.Payments.Core.Abstractions;

namespace iBurguer.Payments.Core.Domain;

public record PaymentConfirmed(Guid OrderId) : IDomainEvent;