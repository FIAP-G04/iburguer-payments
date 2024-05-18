using FluentAssertions;
using iBurguer.Payments.Core.Domain;

namespace iBurguer.Payments.UnitTests.Core.Domain;

public class PaymentStatusTests
{
    [Fact]
    public void ShouldCreateAValidInstanceOfPendingPaymentStatus()
    {
        // Arrange & Act
        var status = PaymentStatus.Pending;

        // Assert
        status.Value.Should().Be(1);
        status.Name.Should().Be("Pending");
    }

    [Fact]
    public void ShouldCreateAValidInstanceOfProcessedPaymentStatus()
    {
        // Arrange & Act
        var status = PaymentStatus.Processed;

        // Assert
        status.Value.Should().Be(2);
        status.Name.Should().Be("Processed");
    }

    [Fact]
    public void ShouldCreateAValidInstanceOfRefusedPaymentStatus()
    {
        // Arrange & Act
        var status = PaymentStatus.Refused;

        // Assert
        status.Value.Should().Be(3);
        status.Name.Should().Be("Refused");
    }
}