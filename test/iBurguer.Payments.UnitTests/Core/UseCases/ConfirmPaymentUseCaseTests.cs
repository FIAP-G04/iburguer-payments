using AutoFixture.Xunit2;
using FluentAssertions;
using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Core.UseCases.ConfirmPayment;
using iBurguer.Payments.UnitTests.Util;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using static iBurguer.Payments.Core.Exceptions;

namespace iBurguer.Payments.UnitTests.Core.UseCases;

public class ConfirmPaymentUseCaseTests : BaseTests
{
    private readonly IPaymentRepository _repository;
    private readonly IConfirmPaymentUseCase _sut;

    public ConfirmPaymentUseCaseTests()
    {
        _repository = Substitute.For<IPaymentRepository>();
        _sut = new ConfirmPaymentUseCase(_repository);
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
        result.PaymentStatus.Should().Be(PaymentStatus.Processed.Name);
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