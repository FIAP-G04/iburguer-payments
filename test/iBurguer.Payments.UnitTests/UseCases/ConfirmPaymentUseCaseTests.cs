using AutoFixture.Xunit2;
using FluentAssertions;
using static iBurguer.Payments.Core.Exceptions;
using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Core.Gateways;
using iBurguer.Payments.Core.UseCases.ConfirmPayment;
using iBurguer.Payments.UnitTests.Util;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace iBurguer.Payments.UnitTests.UseCases;

public class ConfirmPaymentUseCaseTests : BaseTests
{
    private readonly IPaymentRepository _repository;
    private readonly IOrderGateway _gateway;
    private readonly ConfirmPaymentUseCase _sut;

    public ConfirmPaymentUseCaseTests()
    {
        _repository = Substitute.For<IPaymentRepository>();
        _gateway = Substitute.For<IOrderGateway>();
        _sut = new ConfirmPaymentUseCase(_repository, _gateway);
    }

    [Theory, AutoData]
    public async Task ShouldConfirmPaymentWithValidPaymentId(Guid orderId, Amount amount, string qrCode)
    {
        // Arrange
        var payment = new Payment(orderId, amount, qrCode);
        
        _repository.GetById(payment.Id, Arg.Any<CancellationToken>()).Returns(payment);
        _repository.Update(payment, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _sut.ConfirmPayment(payment.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.OrderId.Should().Be(payment.OrderId);
        result.PaymentId.Should().Be(payment.Id);
        result.PaymentStatus.Should().Be(payment.Status.Name);
        result.PayedAt.Should().Be(payment.PayedAt!.Value);
    }

    [Theory, AutoData]
    public async Task ShouldThrowsPaymentNotFoundExceptionWhenConfirmingAPaymentWithNonExistingPaymentId(
        Guid invalidPaymentId)
    {
        // Arrange
        _repository.GetById(invalidPaymentId, Arg.Any<CancellationToken>()).ReturnsNull();

        // Act
        Func<Task> act = async () => await _sut.ConfirmPayment(invalidPaymentId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<PaymentNotFoundException>();
    }

    [Theory, AutoData]
    public async Task ShouldThrowsErrorInPaymentProcessingExceptionWhenConfirmPaymentAndUpdateFails(
        Guid orderId, Amount amount, string qrCode)
    {
        // Arrange
        var payment = new Payment(orderId, amount, qrCode);
        
        _repository.GetById(payment.Id, Arg.Any<CancellationToken>()).Returns(payment);
        _repository.Update(payment, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        Func<Task> act = async () => await _sut.ConfirmPayment(payment.Id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ErrorInPaymentProcessingException>();
    }
}