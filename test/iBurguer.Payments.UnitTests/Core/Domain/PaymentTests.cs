using AutoFixture.Xunit2;
using FluentAssertions;
using iBurguer.Payments.Core.Domain;
using static iBurguer.Payments.Core.Exceptions;

namespace iBurguer.Payments.UnitTests.Core.Domain;

public class PaymentTests : Util.BaseTests
{
    [Theory, AutoData]
    public void ShouldInitializePaymentWithPendingStatus(Guid orderId, Amount amount, string qrCode)
    {
        // Act
        var payment = new Payment(orderId, amount, qrCode);

        // Assert
        payment.Status.Should().Be(PaymentStatus.Pending);
        payment.Amount.Value.Should().Be(amount.Value);
        payment.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        payment.PayedAt.Should().BeNull();
        payment.RefusedAt.Should().BeNull();
        payment.QrData.Should().Be(qrCode);
    }

    [Theory, AutoData]
    public void ShouldUpdateStatusToProcessed(Guid orderId, Amount amount, string qrCode)
    {
        // Arrange
        var payment = new Payment(orderId, amount, qrCode);

        // Act
        payment.Confirm();

        // Assert
        payment.Status.Should().Be(PaymentStatus.Processed);
        payment.PayedAt.Should().NotBeNull();
        payment.PayedAt!.Value.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Theory, AutoData]
    public void ShouldUpdateStatusToRefused(Guid orderId, Amount amount, string qrCode)
    {
        // Arrange
        var payment = new Payment(orderId, amount, qrCode);

        // Act
        payment.Refuse();

        // Assert
        payment.Status.Should().Be(PaymentStatus.Refused);
        payment.RefusedAt.Should().NotBeNull();
        payment.RefusedAt!.Value.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Theory, AutoData]
    public void ShouldThrowExceptionWhenTryingToConfirmAnAlreadyProcessedPayment(Guid orderId, Amount amount, string qrCode)
    {
        // Arrange
        var payment = new Payment(orderId, amount, qrCode);
        
        payment.Confirm();

        // Act
        Action act = () => payment.Confirm();

        // Assert
        act.Should().Throw<CannotToConfirmPaymentException>();
    }

    [Theory, AutoData]
    public void ShouldThrowExceptionWhenTryingToRefuseAnAlreadyProcessedPayment(Guid orderId, Amount amount, string qrCode)
    {
        // Arrange
        var payment = new Payment(orderId, amount, qrCode);
        
        payment.Refuse();

        // Act
        Action act = () => payment.Refuse();

        // Assert
        act.Should().Throw<CannotToRefusePaymentException>();
    }

    [Theory, AutoData]
    public void ShouldReturnTrueWhenProcessedAndPayedAtIsNotNull(Guid orderId, Amount amount, string qrCode)
    {
        // Arrange
        var payment = new Payment(orderId, amount, qrCode);
        
        payment.Confirm();

        // Act
        var result = payment.Confirmed;

        // Assert
        result.Should().BeTrue();
    }

    [Theory, AutoData]
    public void ShouldReturnFalseWhenNotProcessed(Guid orderId, Amount amount, string qrCode)
    {
        // Arrange
        var payment = new Payment(orderId, amount, qrCode);
        
        // Act & Assert
        payment.Confirmed.Should().BeFalse();
    }
    
    [Theory, AutoData]
    public void ShouldThrowPaymentConfirmedDomainEventWhenConfirmingAPayment(Guid orderId, Amount amount, string qrCode)
    {
        // Arrange
        var payment = new Payment(orderId, amount, qrCode);
        
        payment.Confirm();
        
        // Act & Assert
        payment.Events.Any().Should().BeTrue();

        var @event = payment.Events.First() as PaymentConfirmed;
        @event!.OrderId.Should().Be(orderId);
    }
    
    [Theory, AutoData]
    public void ShouldThrowPaymentRefusedDomainEventWhenRefusingAPayment(Guid orderId, Amount amount, string qrCode)
    {
        // Arrange
        var payment = new Payment(orderId, amount, qrCode);
        
        payment.Refuse();
        
        // Act & Assert
        payment.Events.Any().Should().BeTrue();

        var @event = payment.Events.First() as PaymentRefused;
        @event!.OrderId.Should().Be(orderId);
    }
}