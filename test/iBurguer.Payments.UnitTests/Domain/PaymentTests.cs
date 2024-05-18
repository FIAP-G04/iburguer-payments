using FluentAssertions;
using iBurguer.Payments.Core;
using iBurguer.Payments.Core.Domain;
using static iBurguer.Payments.Core.Exceptions;

namespace iBurguer.Payments.UnitTests.Domain;

public class PaymentTests
{
    private const string QrCode = "sample-qr-data";

    [Fact]
    public void ShouldInitializePaymentWithPendingStatus()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var amount = new Amount(100);

        // Act
        var payment = new Payment(orderId, amount, QrCode);

        // Assert
        payment.Status.Should().Be(PaymentStatus.Pending);
        payment.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        payment.PayedAt.Should().BeNull();
        payment.RefusedAt.Should().BeNull();
        payment.QrData.Should().Be(QrCode);
    }

    [Fact]
    public void ShouldUpdateStatusToProcessed()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var amount = new Amount(100);
        var payment = new Payment(orderId, amount, QrCode);

        // Act
        payment.Confirm();

        // Assert
        payment.Status.Should().Be(PaymentStatus.Processed);
        payment.PayedAt.Should().NotBeNull();
        payment.PayedAt.Value.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Refuse_ShouldUpdateStatusToRefused()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var amount = new Amount(100);
        var payment = new Payment(orderId, amount, QrCode);

        // Act
        payment.Refuse();

        // Assert
        payment.Status.Should().Be(PaymentStatus.Refused);
        payment.RefusedAt.Should().NotBeNull();
        payment.RefusedAt.Value.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Confirm_ShouldThrowExceptionIfAlreadyProcessed()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var amount = new Amount(100);
        var payment = new Payment(orderId, amount, QrCode);
        payment.Confirm();

        // Act
        Action act = () => payment.Confirm();

        // Assert
        act.Should().Throw<CannotToConfirmPaymentException>();
    }

    [Fact]
    public void Refuse_ShouldThrowExceptionIfAlreadyProcessed()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var amount = new Amount(100);
        var payment = new Payment(orderId, amount, QrCode);
        payment.Refuse();

        // Act
        Action act = () => payment.Refuse();

        // Assert
        act.Should().Throw<CannotToRefusePaymentException>();
    }

    [Fact]
    public void Confirmed_ShouldReturnTrueWhenProcessedAndPayedAtIsNotNull()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var amount = new Amount(100);
        var payment = new Payment(orderId, amount, QrCode);
        payment.Confirm();

        // Act
        var result = payment.Confirmed;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Confirmed_ShouldReturnFalseWhenNotProcessed()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var amount = new Amount(100);
        var payment = new Payment(orderId, amount, QrCode);

        // Act
        var result = payment.Confirmed;

        // Assert
        result.Should().BeFalse();
    }
}