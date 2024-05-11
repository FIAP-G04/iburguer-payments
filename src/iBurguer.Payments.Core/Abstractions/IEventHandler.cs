namespace iBurguer.Payments.Core.Abstractions;

public interface IEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task Handle(TEvent evt, CancellationToken cancellation);
}