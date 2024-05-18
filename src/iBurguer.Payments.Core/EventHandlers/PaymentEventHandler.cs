using iBurguer.Payments.Core.Abstractions;
using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Core.Gateways;

namespace iBurguer.Payments.Core.EventHandlers;

public class PaymentEventHandler : IEventHandler<PaymentConfirmed>,
                                   IEventHandler<PaymentRefused>
{
    private readonly IOrderGateway _gateway;

    public PaymentEventHandler(IOrderGateway gateway)
    {
        ArgumentNullException.ThrowIfNull(gateway);

        _gateway = gateway;
    }

    public async Task Handle(PaymentConfirmed evt, CancellationToken cancellation)
    {
        await _gateway.ConfirmOrder(evt.OrderId, cancellation);
    }
    
    public async Task Handle(PaymentRefused evt, CancellationToken cancellation)
    {
        await _gateway.CancelOrder(evt.OrderId, cancellation);
    }
}