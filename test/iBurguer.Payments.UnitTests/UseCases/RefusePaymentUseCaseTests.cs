using AutoFixture.Xunit2;
using FluentAssertions;
using static iBurguer.Payments.Core.Exceptions;
using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Core.Gateways;
using iBurguer.Payments.Core.UseCases.ConfirmPayment;
using iBurguer.Payments.Core.UseCases.RefusePaymentUseCase;
using iBurguer.Payments.UnitTests.Util;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace iBurguer.Payments.UnitTests.UseCases;

public class RefusePaymentUseCaseTests : BaseTests
{
    private readonly IPaymentRepository _repository;
    private readonly IRefusePaymentUseCase _sut;

    public RefusePaymentUseCaseTests()
    {
        _repository = Substitute.For<IPaymentRepository>();
        _sut = new RefusePaymentUseCase(_repository);
    }

    [Theory, AutoData]
    public async Task ShouldRefusePaymentWithValidPaymentId(Guid orderId, Amount amount, string qrCode)
    {
        // Arrange
        var payment = new Payment(orderId, amount, qrCode);
        
        _repository.GetById(payment.Id, Arg.Any<CancellationToken>()).Returns(payment);
        _repository.Update(payment, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _sut.RefusePayment(payment.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.OrderId.Should().Be(payment.OrderId);
        result.PaymentId.Should().Be(payment.Id);
        result.PaymentStatus.Should().Be(PaymentStatus.Refused.Name);
        result.RefusedAt.Should().Be(payment.RefusedAt!.Value);
    }

    [Theory, AutoData]
    public async Task ShouldThrowsPaymentNotFoundExceptionWhenRefusingAPaymentWithNonExistingPaymentId(
        Guid invalidPaymentId)
    {
        // Arrange
        _repository.GetById(invalidPaymentId, Arg.Any<CancellationToken>()).ReturnsNull();

        // Act
        Func<Task> act = async () => await _sut.RefusePayment(invalidPaymentId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<PaymentNotFoundException>();
    }

    [Theory, AutoData]
    public async Task ShouldThrowsErrorInPaymentProcessingExceptionWhenRefusePaymentAndUpdateFails(
        Guid orderId, Amount amount, string qrCode)
    {
        // Arrange
        var payment = new Payment(orderId, amount, qrCode);
        
        _repository.GetById(payment.Id, Arg.Any<CancellationToken>()).Returns(payment);
        _repository.Update(payment, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        Func<Task> act = async () => await _sut.RefusePayment(payment.Id, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ErrorInPaymentProcessingException>();
    }
}