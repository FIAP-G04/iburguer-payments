using iBurguer.Payments.Core.Abstractions;
using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Core.Gateways;

namespace iBurguer.Payments.Core.EventHandlers;

public class PaymentEventHandler : IEventHandler<PaymentConfirmed>,
                                   IEventHandler<PaymentRefused>
{
    private readonly IOrderGateway _gateway;
    private readonly ISQSService _service;
    private readonly string _paymentConfirmedQueue = "PaymentApproved";
    private readonly string _paymentRefusedQueue = "PaymentRefused";

    public PaymentEventHandler(IOrderGateway gateway, ISQSService service)
    {
        ArgumentNullException.ThrowIfNull(gateway);

        _gateway = gateway;
        _service = service;
    }

    public async Task Handle(PaymentConfirmed evt, CancellationToken cancellation)
    {
        await _service.SendMessage(evt, _paymentConfirmedQueue, cancellation);
    }
    
    public async Task Handle(PaymentRefused evt, CancellationToken cancellation)
    {
        await _service.SendMessage(evt, _paymentRefusedQueue, cancellation);
    }
}