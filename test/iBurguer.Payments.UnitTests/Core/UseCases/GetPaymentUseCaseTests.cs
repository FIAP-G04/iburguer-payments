using AutoFixture.Xunit2;
using FluentAssertions;
using iBurguer.Payments.Core.Domain;
using iBurguer.Payments.Core.UseCases.GenerateQrCode;
using iBurguer.Payments.Core.UseCases.GetPaymentUseCase;
using NSubstitute;
using static iBurguer.Payments.Core.Exceptions;

namespace iBurguer.Payments.UnitTests.Core.UseCases;

public class GetPaymentUseCaseTests
{
    private readonly IGetPaymentUseCase _sut;
    private readonly IPaymentRepository _mockRepository;

    public GetPaymentUseCaseTests()
    {
        _mockRepository = Substitute.For<IPaymentRepository>();
        _sut = new GetPaymentUseCase(_mockRepository);
    }

    [Theory, AutoData]
    public async Task ShouldReturnGenerateQrCodeResponse(Guid orderId)
    {
        // Arrange
        var payment = new Payment(orderId, 100, "QR_CODE");
        payment.Confirm();
        var cancellationToken = new CancellationToken();

        _mockRepository.GetByOrderId(orderId, cancellationToken)
            .Returns(Task.FromResult(payment));

        // Act
        var result = await _sut.GetPayment(orderId, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(GenerateQrCodeResponse.Convert(payment));
    }

    [Theory, AutoData]
    public async Task ShouldThrowPaymentNotFoundException_WhenPaymentDoesNotExist(Guid orderId)
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        _mockRepository.GetByOrderId(orderId, cancellationToken)
            .Returns(Task.FromResult<Payment>(null));

        // Act
        Func<Task> act = async () => await _sut.GetPayment(orderId, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<PaymentNotFoundException>();
    }
}