using AutoFixture.Xunit2;
using iBurguer.Payments.Core.Abstractions;
using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Core.EventHandlers;
using iBurguer.Payments.Core.Gateways;
using NSubstitute;

namespace iBurguer.Payments.UnitTests.Core.EventHandlers;

public class PaymentEventHandlerTests
{
    private readonly IOrderGateway _gateway;
    private readonly ISQSService _sqsService;
    private readonly PaymentEventHandler _sut;

    public PaymentEventHandlerTests()
    {
        _gateway = Substitute.For<IOrderGateway>();
        _sqsService = Substitute.For<ISQSService>();
        _sut = new PaymentEventHandler(_gateway, _sqsService);
    }

    [Theory, AutoData]
    public async Task ShouldConfirmOrderWhenPaymentIsConfirmed(PaymentConfirmed evt)
    {
        // Act
        await _sut.Handle(evt, CancellationToken.None);

        // Assert
        await _gateway.Received(1).ConfirmOrder(evt.OrderId, Arg.Any<CancellationToken>());
    }

    [Theory, AutoData]
    public async Task ShouldCancelOrderWhenPaymentIsRefused(PaymentRefused evt)
    {
        // Act
        await _sut.Handle(evt, CancellationToken.None);

        // Assert
        await _gateway.Received(1).CancelOrder(evt.OrderId, Arg.Any<CancellationToken>());
    }
}