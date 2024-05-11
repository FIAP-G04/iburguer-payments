using iBurguer.Payments.Core.Abstractions;

namespace iBurguer.Payments.Core.Domain;

public record PaymentRefused(Guid OrderId) : IDomainEvent;