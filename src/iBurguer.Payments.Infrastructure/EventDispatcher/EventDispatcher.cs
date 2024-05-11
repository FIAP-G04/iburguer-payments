using iBurguer.Payments.Core.Abstractions;

namespace iBurguer.Payments.Infrastructure.EventDispatcher;

public interface IEventDispatcher
{
    Task Dispatch<TEvent>(TEvent @event, CancellationToken cancellation)
        where TEvent : IDomainEvent;
}

public class EventDispatcher : IEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public EventDispatcher(IServiceProvider provider) => _serviceProvider = provider;

    public async Task Dispatch<TEvent>(TEvent @event, CancellationToken cancellation)
        where TEvent : IDomainEvent
    {
        var eventType = @event.GetType();
        var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

        dynamic instance = _serviceProvider.GetService(handlerType);

        if (instance == null)
            throw new InvalidOperationException(
                "Não foi possível encontrar nenhum EventHandler para tratar este evento.");

        await instance.Handle(@event as dynamic, cancellation);
    }
}